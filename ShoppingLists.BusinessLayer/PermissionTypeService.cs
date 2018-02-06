using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Shared.Entities;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.Shared.RepositoryInterfaces;
using ShoppingLists.Shared.ServiceInterfaces;

namespace ShoppingLists.BusinessLayer
{
    public class PermissionTypeService : IPermissionTypeService
    {
        private IPermissionTypeRepository _permissionTypeRepository;

        public PermissionTypeService(IPermissionTypeRepository repository)
        {
            this._permissionTypeRepository = repository;
        }

        public IEnumerable<PermissionType> GetAll()
        {
            var permissions = _permissionTypeRepository.GetAll();
            if (permissions.Count() == 0)
            {
                throw new NoEntitiesFoundException(typeof(PermissionType));
            }
            return permissions;
        }
    }
}
