using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core;
using ShoppingLists.Core.RepositoryInterfaces;
using ShoppingLists.BusinessLayer.Exceptions;

namespace ShoppingLists.BusinessLayer
{
    public class PermissionTypeService
    {
        private IUnitOfWork uow;
        private IPermissionTypeRepository repository;

        public PermissionTypeService(IUnitOfWork uow, IPermissionTypeRepository repository)
        {
            this.uow = uow;
            this.repository = repository;
        }

        public IEnumerable<PermissionType> GetAll()
        {
            var permissions = repository.GetAll();
            if (permissions.Count() == 0)
            {
                throw new NoEntitiesFoundException(typeof(PermissionType));
            }
            return permissions;
        }
    }
}
