using System;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.Core.RepositoryInterfaces
{
    public interface ICrudRepository<TEntity> where TEntity : Entity, new()
    {
        void Create(TEntity entity);

        void Delete(long id);

        TEntity Get(long id);

        void Update(TEntity entity);
    }
}
