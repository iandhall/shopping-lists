using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Shared.RepositoryInterfaces
{
    public interface IListItemRepository
    {
        void Create(ListItem listItem);
        void Delete(long id);
        ListItem FindByDescription(string description, long shoppingListId);
        ListItem Get(long id);
        void UnpickAllListItems(long shoppingListId);
        void Update(ListItem listItem);
    }
}