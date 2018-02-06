using System.Collections.Generic;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Shared.ServiceInterfaces
{
    public interface IPermissionService
    {
        void Check(string userId, Permissions permission, long shoppingListId);
        void CheckAlreadyExists(string userId, Permissions permission, long shoppingListId);
        void Create(string userId, Permissions permission, long shoppingListId);
        void DeleteAllForUser(long shoppingListId, string userId);
        Permission Get(string userId, Permissions permission, long entityId);
        IEnumerable<Permission> GetAllForUserAndEntity(string userId, long entityId);
        void SetAllForUser(string userId, long shoppingListId, IEnumerable<long> permissionIds);
    }
}