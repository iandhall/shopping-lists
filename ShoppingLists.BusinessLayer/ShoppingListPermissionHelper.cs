using System.Collections.Generic;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.DataAccessLayer;

namespace ShoppingLists.BusinessLayer
{
    public class ShoppingListPermissionHelper
    {
        private ShoppingListPermissionRepository _permissionRepository;

        public ShoppingListPermissionHelper(ShoppingListPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        internal void Check(string userId, Permissions permission, long shoppingListId)
        {
            var entityPermission = _permissionRepository.Get(permission, userId, shoppingListId);
            if (entityPermission == null) throw new PermissionNotFoundException(permission, userId, shoppingListId);
        }

        internal ShoppingListPermission Get(string userId, Permissions permission, long entityId)
        {
            return _permissionRepository.Get(permission, userId, entityId);
        }

        internal void Create(string userId, Permissions permission, long shoppingListId)
        {
            var entityPermission = new ShoppingListPermission
            {
                PermissionTypeId = permission,
                UserId = userId,
                ShoppingListId = shoppingListId
            };
            _permissionRepository.Create(entityPermission);
        }

        internal void CheckAlreadyExists(string userId, Permissions permission, long shoppingListId)
        {
            if (_permissionRepository.Get(permission, userId, shoppingListId) != null)
            {
                throw new PermissionAlreadyExistsException(permission, userId, shoppingListId);
            }
        }

        internal void DeleteAllForUser(long shoppingListId, string userId)
        {
            _permissionRepository.DeleteAllForUserAndShoppingList(shoppingListId, userId);
        }

        internal IEnumerable<ShoppingListPermission> GetAllForUserAndEntity(string userId, long entityId)
        {
            return _permissionRepository.FindAllForUserAndShoppingList(userId, entityId);
        }

        internal void SetAllForUser(string userId, long shoppingListId, IEnumerable<long> permissionIds)
        {
            _permissionRepository.DeleteAllForUserAndShoppingList(shoppingListId, userId);
            foreach (long permissionId in permissionIds)
            {
                Create(userId, (Permissions)permissionId, shoppingListId);
            }
        }
    }
}
