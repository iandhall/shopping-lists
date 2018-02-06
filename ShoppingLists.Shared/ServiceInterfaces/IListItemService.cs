using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Shared.ServiceInterfaces
{
    public interface IListItemService
    {
        ListItem Create(string description, int quantity, long shoppingListId);
        void Delete(long listItemId, long shoppingListId);
        ListItem Pick(long listItemId, long shoppingListId);
        ListItem Unpick(long listItemId, long shoppingListId);
        ListItem Update(string description, int quantity, long listItemId, long shoppingListId);
    }
}