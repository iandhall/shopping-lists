using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.DataAccessLayer
{
    public class ShoppingListRepository : CrudRepository<ShoppingList>
    {
        private ShoppingListsDbContext _dbContext;

        public ShoppingListRepository(ShoppingListsDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public ShoppingList Get(long id, bool includeListItems = false, bool includeCreator = false)
        {
            ShoppingList shoppingList = _dbContext.ShoppingLists.Find(id);
            if (includeListItems)
            {
                _dbContext.Entry(shoppingList).Collection(sl => sl.ListItems).Load();
            }
            if (includeCreator)
            {
                _dbContext.Entry(shoppingList).Reference(sl => sl.Creator).Load();
            }
            return shoppingList;
        }

        public IEnumerable<ShoppingList> FindAllForUser(string userId)
        {
            return _dbContext.ShoppingLists.Include(sl => sl.Creator).Where(sl =>
                sl.CreatorId == userId
                || (
                    sl.ShoppingListPermissions.Any(p =>
                        p.PermissionTypeId == Permissions.View
                        && p.UserId == userId
                    )
                )
            ).OrderBy(sl => sl.Title).ToList();
        }

        public ShoppingList FindByTitle(string title, string userId) {
            return _dbContext.ShoppingLists.FirstOrDefault(sl => sl.CreatorId == userId && sl.Title == title);
        }
    }
}
