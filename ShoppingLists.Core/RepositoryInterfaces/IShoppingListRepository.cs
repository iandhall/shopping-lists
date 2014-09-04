using System;
using ShoppingLists.Core.Entities;
using System.Collections.Generic;

namespace ShoppingLists.Core.RepositoryInterfaces
{
    public interface IShoppingListRepository : ICrudRepository<ShoppingList>
    {

        ShoppingList Get(long id, bool includeListItems = false, bool includeCreator = false);

        IEnumerable<ShoppingList> FindAllForUser(string userId);

        IEnumerable<ShoppingList> FindByPartialTitleMatch(string partialTitle, string userId);

        ShoppingList FindByTitle(string title, string userId);
    }
}
