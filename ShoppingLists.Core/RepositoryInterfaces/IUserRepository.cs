using System;
using ShoppingLists.Core.Entities;
using System.Collections.Generic;

namespace ShoppingLists.Core.RepositoryInterfaces
{
    public interface IUserRepository
    {

        void Create(User entity);

        void Delete(string id);

        User Get(string id, bool includePermissions = false, long? shoppingListId = null);

        IEnumerable<User> GetAllForShoppingList(long shoppingListId);

        User GetByName(string userName);

        void Update(User entity);
    }
}
