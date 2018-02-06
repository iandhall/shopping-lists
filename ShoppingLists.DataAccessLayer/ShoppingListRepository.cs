using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;
using ShoppingLists.Shared.RepositoryInterfaces;

namespace ShoppingLists.DataAccessLayer
{
    public class ShoppingListRepository : IShoppingListRepository
    {
        private ShoppingListsDbContext _dbContext;

        public ShoppingListRepository(ShoppingListsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ShoppingList Get(long id, bool includeListItems = false, bool includeCreator = false)
        {
            ShoppingList shoppingList = _dbContext.ShoppingLists.Find(id);
            if (shoppingList != null)
            {
                if (includeListItems)
                {
                    _dbContext.Entry(shoppingList).Collection(sl => sl.ListItems).Load();
                }
                if (includeCreator)
                {
                    _dbContext.Entry(shoppingList).Reference(sl => sl.Creator).Load();
                }
            }
            return shoppingList;
        }

        public virtual void Create(ShoppingList shoppingList)
        {
            _dbContext.ShoppingLists.Add(shoppingList);
        }
        
        public virtual void Update(ShoppingList shoppingList)
        {
            _dbContext.Entry(shoppingList).State = EntityState.Modified;
        }

        public virtual void Delete(long id)
        {
            var shoppingList = _dbContext.ShoppingLists.Find(id);
            _dbContext.ShoppingLists.Remove(shoppingList);
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
