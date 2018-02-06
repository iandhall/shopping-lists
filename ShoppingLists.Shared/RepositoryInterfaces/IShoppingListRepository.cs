using System.Collections.Generic;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Shared.RepositoryInterfaces
{
    public interface IShoppingListRepository
    {
        void Create(ShoppingList shoppingList);
        void Delete(long id);
        IEnumerable<ShoppingList> FindAllForUser(string userId);
        ShoppingList FindByTitle(string title, string userId);
        ShoppingList Get(long id, bool includeListItems = false, bool includeCreator = false);
        void Update(ShoppingList shoppingList);
    }
}