using System.Collections.Generic;
using ShoppingLists.Shared.Entities;
using ShoppingLists.Shared;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.Shared.RepositoryInterfaces;
using ShoppingLists.Shared.ServiceInterfaces;

namespace ShoppingLists.BusinessLayer
{
    public class PermissionService : IPermissionService
    {
        private IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public void Check(string userId, Permissions permission, long shoppingListId)
        {
            var entityPermission = _permissionRepository.Get(permission, userId, shoppingListId);
            if (entityPermission == null) throw new PermissionNotFoundException(permission, userId, shoppingListId);
        }

        public Permission Get(string userId, Permissions permission, long entityId)
        {
            return _permissionRepository.Get(permission, userId, entityId);
        }

        public void Create(string userId, Permissions permission, long shoppingListId)
        {
            var entityPermission = new Permission
            {
                PermissionTypeId = permission,
                UserId = userId,
                ShoppingListId = shoppingListId
            };
            _permissionRepository.Create(entityPermission);
        }

        public void CheckAlreadyExists(string userId, Permissions permission, long shoppingListId)
        {
            if (_permissionRepository.Get(permission, userId, shoppingListId) != null)
            {
                throw new PermissionAlreadyExistsException(permission, userId, shoppingListId);
            }
        }

        public void DeleteAllForUser(long shoppingListId, string userId)
        {
            _permissionRepository.DeleteAllForUserAndShoppingList(shoppingListId, userId);
        }

        public IEnumerable<Permission> GetAllForUserAndEntity(string userId, long entityId)
        {
            return _permissionRepository.FindAllForUserAndShoppingList(userId, entityId);
        }

        public void SetAllForUser(string userId, long shoppingListId, IEnumerable<long> permissionIds)
        {
            _permissionRepository.DeleteAllForUserAndShoppingList(shoppingListId, userId);
            foreach (long permissionId in permissionIds)
            {
                Create(userId, (Permissions)permissionId, shoppingListId);
            }
        }
    }
}
