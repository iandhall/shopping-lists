using System.Data.Entity;
using System.Data.Entity.Infrastructure.Pluralization;
using ShoppingLists.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using ShoppingLists.Core;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ShoppingLists.DataAccessLayer
{
    public class ShoppingListsDbContext : DbContext
    {
        private IPluralizationService _pluralizationService;
        private IUserContext _userContext;

        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<ListItem> ListItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ShoppingListPermission> ShoppingListPermissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }

        public ShoppingListsDbContext(IUserContext userContext)
        {
            _userContext = userContext;
            this.Database.Log = (s) => Trace.Write(s);
            this.Configuration.LazyLoadingEnabled = false;
            this._pluralizationService = new EnglishPluralizationService();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // ShoppingList
            ConfigureDerivedEntity<ShoppingList>(modelBuilder);
            modelBuilder.Entity<ShoppingList>().HasMany(sl => sl.ListItems).WithRequired(li => li.ShoppingList).HasForeignKey(li => li.ShoppingListId).WillCascadeOnDelete(true);
            modelBuilder.Entity<ShoppingList>().HasMany(sl => sl.ShoppingListPermissions).WithRequired(slp => slp.ShoppingList).HasForeignKey(slp => slp.ShoppingListId).WillCascadeOnDelete(true);

            // ListItem
            ConfigureDerivedEntity<ListItem>(modelBuilder);
            
            // ShoppingListPermission
            ConfigureDerivedEntity<ShoppingListPermission>(modelBuilder);
            modelBuilder.Entity<ShoppingListPermission>().HasRequired(slp => slp.PermissionType).WithMany().HasForeignKey(slp => slp.PermissionTypeId);

            // User
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<User>().HasMany(u => u.ShoppingListPermissions).WithRequired(slp => slp.User).HasForeignKey(slp => slp.UserId).WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureDerivedEntity<TEntity>(DbModelBuilder modelBuilder) where TEntity : TimestampedEntity
        {       
            modelBuilder.Entity<TEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<TEntity>().Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<TEntity>().HasRequired(e => e.Creator).WithMany().HasForeignKey(e => e.CreatorId);
            modelBuilder.Entity<TEntity>().HasRequired(e => e.Amender).WithMany().HasForeignKey(e => e.AmenderId);
            modelBuilder.Entity<TEntity>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable(_pluralizationService.Pluralize(typeof(TEntity).Name));
            });
        }

        public override int SaveChanges()
        {
            // Set timestamps and userIds
            ChangeTracker.Entries()
                .Where(e => e.Entity is TimestampedEntity)
                .ToList().ForEach(e =>
                {
                    if (e.State == EntityState.Modified)
                    {
                        var entity = (TimestampedEntity)e.Entity;
                        entity.AmenderId = _userContext.UserId;
                        entity.AmendedDate = DateTime.Now;
                    }
                    else if (e.State == EntityState.Added)
                    {
                        var entity = (TimestampedEntity)e.Entity;
                        entity.CreatorId = _userContext.UserId;
                        entity.CreatedDate = DateTime.Now;
                    }
                });

            return base.SaveChanges();
        }
    }
}
