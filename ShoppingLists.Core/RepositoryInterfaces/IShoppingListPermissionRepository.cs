using System;
using ShoppingLists.Core.Entities;
using System.Collections.Generic;

namespace ShoppingLists.Core.RepositoryInterfaces
{
    public interface IShoppingListPermissionRepository  : ICrudRepository<ShoppingListPermission>
    {
        void DeleteAllForUserAndShoppingList(long shoppingListId, string userId);

        ShoppingListPermission Get(Permissions permission, string userId, long shoppingListId);

        IEnumerable<ShoppingListPermission> FindAllForUserAndShoppingList(string userId, long shoppingListId);
    }
}
