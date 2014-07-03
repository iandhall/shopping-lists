using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core;
using ShoppingLists.Core.RepositoryInterfaces;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.BusinessLayer.Exceptions;

namespace ShoppingLists.BusinessLayer
{
    public class ShoppingListPermissionHelper
    {
        private IUnitOfWork uow;
        private IShoppingListPermissionRepository repository;
        private Timestamper<ShoppingListPermission> timestamper;

        public ShoppingListPermissionHelper(IUnitOfWork uow, IShoppingListPermissionRepository repository, Timestamper<ShoppingListPermission> timestamper)
        {
            this.uow = uow;
            this.repository = repository;
            this.timestamper = timestamper;
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
            timestamper.Create(entityPermission, userId);
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
            return repository.GetAllForUserAndShoppingList(userId, entityId);
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
