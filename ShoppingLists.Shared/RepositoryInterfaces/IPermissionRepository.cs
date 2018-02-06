using System.Collections.Generic;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Shared.RepositoryInterfaces
{
    public interface IPermissionRepository
    {
        void Create(Permission permission);
        void DeleteAllForUserAndShoppingList(long shoppingListId, string userId);
        IEnumerable<Permission> FindAllForUserAndShoppingList(string userId, long shoppingListId);
        Permission Get(Permissions permission, string userId, long shoppingListId);
    }
}