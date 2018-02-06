using System.Collections.Generic;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Shared.ServiceInterfaces
{
    public interface IUserService
    {
        void Create(User entity);
        void Delete(string id);
        User Get(string id, bool includePermissions = false, long? shoppingListId = null);
        IEnumerable<User> GetAllForShoppingList(long shoppingListId);
        User GetByName(string userName);
        void SetPermissions(string userId, long shoppingListId, IEnumerable<long> permissionIds);
        void Update(User entity);
    }
}