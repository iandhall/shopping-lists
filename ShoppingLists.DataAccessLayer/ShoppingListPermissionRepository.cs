using System.Data;
using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Core;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.DataAccessLayer
{
    public class ShoppingListPermissionRepository : CrudRepository<ShoppingListPermission>
    {
        private ShoppingListsDbContext _dbContext;

        public ShoppingListPermissionRepository(ShoppingListsDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public ShoppingListPermission Get(Permissions permission, string userId, long shoppingListId)
        {
            return _dbContext.ShoppingListPermissions.FirstOrDefault(slp => slp.PermissionTypeId == permission && slp.UserId == userId && slp.ShoppingListId == shoppingListId);
        }

        public void DeleteAllForUserAndShoppingList(long shoppingListId, string userId)
        {
            _dbContext.ShoppingListPermissions.Where(slp => slp.ShoppingListId == shoppingListId && slp.UserId == userId).ToList().ForEach(slp =>
                _dbContext.ShoppingListPermissions.Remove(slp)
            );
        }

        public IEnumerable<ShoppingListPermission> FindAllForUserAndShoppingList(string userId, long shoppingListId)
        {
            return _dbContext.ShoppingListPermissions.Where(slp => slp.UserId == userId && slp.ShoppingListId == shoppingListId).ToList();
        }
    }
}
