using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoppingLists.Core;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core.RepositoryInterfaces;
using System.Runtime.CompilerServices;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace ShoppingLists.DataAccessLayer
{
    public class ShoppingListRepository : CrudRepository<ShoppingList>, IShoppingListRepository
    {
        public ShoppingListRepository(ShoppingListsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ShoppingList Get(long id, bool includeListItems = false, bool includeCreator = false)
        {
            ShoppingList shoppingList = dbContext.ShoppingLists.Find(id);
            if (includeListItems)
            {
                dbContext.Entry(shoppingList).Collection(sl => sl.ListItems).Load();
            }
            if (includeCreator)
            {
                dbContext.Entry(shoppingList).Reference(sl => sl.Creator).Load();
            }
            return shoppingList;
        }

        public IEnumerable<ShoppingList> FindAllForUser(string userId)
        {
            return dbContext.ShoppingLists.Include(sl => sl.Creator).Where(sl =>
                sl.CreatorId == userId
                || (
                    sl.ShoppingListPermissions.Any(p =>
                        p.PermissionTypeId == Permissions.View
                        && p.UserId == userId
                    )
                )
            ).OrderBy(sl => sl.Title).ToList();
        }

        // Returns matches in no specific order.
        public IEnumerable<ShoppingList> FindByPartialTitleMatch(string partialTitle, string userId) {
            return dbContext.ShoppingLists.Where(sl => sl.CreatorId == userId && SqlFunctions.PatIndex(partialTitle + "%", sl.Title) != 0).ToList();
        }

        public ShoppingList GetByTitle(string title, string userId) {
            return dbContext.ShoppingLists.FirstOrDefault(sl => sl.CreatorId == userId && sl.Title == title);
        }
    }
}
