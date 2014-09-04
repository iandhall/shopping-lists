using System;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.Core.RepositoryInterfaces
{
    public interface IListItemRepository : ICrudRepository<ListItem>
    {
        ListItem FindByDescription(string description, long shoppingListId);

        void UnpickAllListItems(long shoppingListId);
    }
}
