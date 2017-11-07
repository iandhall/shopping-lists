using System.Data;
using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Core;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core.RepositoryInterfaces;

namespace ShoppingLists.DataAccessLayer
{
    public class ShoppingListPermissionRepository : CrudRepository<ShoppingListPermission>, IShoppingListPermissionRepository
    {
        public ShoppingListPermissionRepository(ShoppingListsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ShoppingListPermission Get(Permissions permission, string userId, long shoppingListId)
        {
            return dbContext.ShoppingListPermissions.FirstOrDefault(slp => slp.PermissionTypeId == permission && slp.UserId == userId && slp.ShoppingListId == shoppingListId);
        }

        public void DeleteAllForUserAndShoppingList(long shoppingListId, string userId)
        {
            dbContext.ShoppingListPermissions.Where(slp => slp.ShoppingListId == shoppingListId && slp.UserId == userId).ToList().ForEach(slp =>
                dbContext.ShoppingListPermissions.Remove(slp)
            );
        }

        public IEnumerable<ShoppingListPermission> FindAllForUserAndShoppingList(string userId, long shoppingListId)
        {
            return dbContext.ShoppingListPermissions.Where(slp => slp.UserId == userId && slp.ShoppingListId == shoppingListId).ToList();
        }
    }
}
