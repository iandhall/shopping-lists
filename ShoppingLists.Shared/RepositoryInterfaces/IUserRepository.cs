using System.Collections.Generic;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Shared.RepositoryInterfaces
{
    public interface IUserRepository
    {
        void Create(User entity);
        void Delete(string id);
        IEnumerable<User> FindAllForShoppingList(long shoppingListId);
        User FindByName(string username);
        User Get(string id, bool includePermissions = false, long? shoppingListId = null);
        void Update(User entity);
    }
}