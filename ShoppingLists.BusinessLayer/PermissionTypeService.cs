using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Core.Entities;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.DataAccessLayer;

namespace ShoppingLists.BusinessLayer
{
    public class PermissionTypeService
    {
        private PermissionTypeRepository _permissionTypeRepository;

        public PermissionTypeService(PermissionTypeRepository repository)
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
