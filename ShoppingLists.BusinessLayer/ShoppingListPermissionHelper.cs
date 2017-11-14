using System.Collections.Generic;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core;
using ShoppingLists.Core.RepositoryInterfaces;
using ShoppingLists.BusinessLayer.Exceptions;

namespace ShoppingLists.BusinessLayer
{
    public class ShoppingListPermissionHelper
    {
        private IUnitOfWork uow;
        private IShoppingListPermissionRepository repository;

        public ShoppingListPermissionHelper(IUnitOfWork uow, IShoppingListPermissionRepository repository)
        {
            this.uow = uow;
            this.repository = repository;
        }

        internal void Check(string userId, Permissions permission, long shoppingListId)
        {
            var entityPermission = repository.Get(permission, userId, shoppingListId);
            if (entityPermission == null) throw new PermissionNotFoundException(permission, userId, shoppingListId);
        }

        internal ShoppingListPermission Get(string userId, Permissions permission, long entityId)
        {
            return repository.Get(permission, userId, entityId);
        }

        internal void Create(string userId, Permissions permission, long shoppingListId)
        {
            var entityPermission = new ShoppingListPermission
            {
                PermissionTypeId = permission,
                UserId = userId,
                ShoppingListId = shoppingListId
            };
            repository.Create(entityPermission, userId);
        }

        internal void CheckAlreadyExists(string userId, Permissions permission, long shoppingListId)
        {
            if (repository.Get(permission, userId, shoppingListId) != null)
            {
                throw new PermissionAlreadyExistsException(permission, userId, shoppingListId);
            }
        }

        internal void DeleteAllForUser(long shoppingListId, string userId)
        {
            repository.DeleteAllForUserAndShoppingList(shoppingListId, userId);
        }

        internal IEnumerable<ShoppingListPermission> GetAllForUserAndEntity(string userId, long entityId)
        {
            return repository.FindAllForUserAndShoppingList(userId, entityId);
        }

        internal void SetAllForUser(string userId, long shoppingListId, IEnumerable<long> permissionIds)
        {
            repository.DeleteAllForUserAndShoppingList(shoppingListId, userId);
            foreach (long permissionId in permissionIds)
            {
                Create(userId, (Permissions)permissionId, shoppingListId);
            }
        }
    }
}
