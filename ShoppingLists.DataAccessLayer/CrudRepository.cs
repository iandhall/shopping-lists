using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core.RepositoryInterfaces;
using ShoppingLists.Core;
using System.Data.Entity;

namespace ShoppingLists.DataAccessLayer
{
    public abstract class CrudRepository<TEntity> : ICrudRepository<TEntity> where TEntity : Entity, new()
    {
        protected internal ShoppingListsDbContext dbContext;

        public virtual TEntity Get(long id)
        {
            return dbContext.Set<TEntity>().Find(id);
        }

        public virtual void Create(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);
            dbContext.SaveChanges(); // SaveChanges here so that entity.Id gets populated with the new Id.
        }

        public virtual void Update(TEntity entity)
        {
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
