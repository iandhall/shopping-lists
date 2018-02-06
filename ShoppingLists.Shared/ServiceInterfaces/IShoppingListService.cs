using System.Collections.Generic;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Shared.ServiceInterfaces
{
    public interface IShoppingListService
    {
        ShoppingList Create();
        void Delete(long shoppingListId);
        IEnumerable<ShoppingList> FindAllForCurrentUser();
        ShoppingList Get(long id, bool includeListItems = false);
        IEnumerable<Permission> GetPermissionsForUser(long shoppingListId, string forUserId);
        void Ignore(long shoppingListId);
        void RemoveSharingUser(long shoppingListId, string sharingUserId);
        void ShareWithUser(long shoppingListId, string userToShareWithId);
        void UnpickAllListItems(long shoppingListId);
        ShoppingList Update(long shoppingListId, string title);
    }
}