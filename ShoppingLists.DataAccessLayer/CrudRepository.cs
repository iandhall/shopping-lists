using ShoppingLists.Core.Entities;
using ShoppingLists.Core.RepositoryInterfaces;
using System.Data.Entity;
using System;
using ShoppingLists.Core.Exceptions;

namespace ShoppingLists.DataAccessLayer
{
    public abstract class CrudRepository<TEntity> : ICrudRepository<TEntity> where TEntity : TimestampedEntity, new()
    {
        protected internal ShoppingListsDbContext dbContext;

        public virtual TEntity Get(long id)
        {
            return dbContext.Set<TEntity>().Find(id);
        }

        public virtual void Create(TEntity entity, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new NullOrWhiteSpaceException("userId");
            }

            entity.CreatorId = userId;
            entity.CreatedDate = DateTime.Now.ToUniversalTime();
            dbContext.Set<TEntity>().Add(entity);
            dbContext.SaveChanges(); // SaveChanges here so that entity.Id gets populated with the new Id.
        }

        public virtual void Update(TEntity entity, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new NullOrWhiteSpaceException("userId");
            }

            entity.AmenderId = userId;
            entity.AmendedDate = DateTime.Now.ToUniversalTime();
            dbContext.Entry(entity).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public virtual void Delete(long id)
        {
            var entity = dbContext.Set<TEntity>().Find(id);
            dbContext.Set<TEntity>().Remove(entity);
            dbContext.SaveChanges();
        }
    }
}
