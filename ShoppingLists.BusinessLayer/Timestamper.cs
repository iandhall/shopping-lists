using System;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core;
using ShoppingLists.Core.RepositoryInterfaces;

namespace ShoppingLists.BusinessLayer
{
    public class Timestamper<TEntity> where TEntity : TimestampedEntity, new()
    {
        private IUnitOfWork uow;
        private ICrudRepository<TEntity> crudRepository;

        public Timestamper(IUnitOfWork uow, ICrudRepository<TEntity> crudRepository)
        {
            this.uow = uow;
            this.crudRepository = crudRepository;
        }

        internal virtual void Create(TEntity entity, string userId)
        {
            entity.CreatorId = userId;
            entity.CreatedDate = DateTime.Now.ToUniversalTime();
            crudRepository.Create(entity);
        }

        internal virtual void Update(TEntity entity, string userId)
        {
            entity.AmenderId = userId;
            entity.AmendedDate = DateTime.Now.ToUniversalTime();
            crudRepository.Update(entity);
        }
    }
}
