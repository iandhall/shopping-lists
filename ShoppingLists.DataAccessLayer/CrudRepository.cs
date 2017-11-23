using ShoppingLists.Core.Entities;
using System.Data.Entity;

namespace ShoppingLists.DataAccessLayer
{
    public abstract class CrudRepository<TEntity> where TEntity : Entity, new()
    {
        private ShoppingListsDbContext _dbContext;

        public CrudRepository(ShoppingListsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual TEntity Get(long id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public virtual void Create(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            //_dbContext.SaveChanges(); // SaveChanges here so that entity.Id gets populated with the new Id.
        }

        public virtual void Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            //_dbContext.SaveChanges();
        }

        public virtual void Delete(long id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);
            _dbContext.Set<TEntity>().Remove(entity);
            //_dbContext.SaveChanges();
        }
    }
}
