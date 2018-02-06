using System.Data;
using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;
using ShoppingLists.Shared.RepositoryInterfaces;

namespace ShoppingLists.DataAccessLayer
{
    public class PermissionRepository : IPermissionRepository
    {
        private ShoppingListsDbContext _dbContext;

        public PermissionRepository(ShoppingListsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Permission Get(Permissions permission, string userId, long shoppingListId)
        {
            return _dbContext.Permissions.FirstOrDefault(slp => slp.PermissionTypeId == permission && slp.UserId == userId && slp.ShoppingListId == shoppingListId);
        }
        
        public virtual void Create(Permission permission)
        {
            _dbContext.Permissions.Add(permission);
        }

        public void DeleteAllForUserAndShoppingList(long shoppingListId, string userId)
        {
            _dbContext.Permissions.Where(slp => slp.ShoppingListId == shoppingListId && slp.UserId == userId).ToList().ForEach(slp =>
                _dbContext.Permissions.Remove(slp)
            );
        }

        public IEnumerable<Permission> FindAllForUserAndShoppingList(string userId, long shoppingListId)
        {
            return _dbContext.Permissions.Where(slp => slp.UserId == userId && slp.ShoppingListId == shoppingListId).ToList();
        }
    }
}
