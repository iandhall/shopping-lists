using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoppingLists.Core;
using ShoppingLists.Core.Entities;
using System.Runtime.CompilerServices;
using ShoppingLists.Core.RepositoryInterfaces;

namespace ShoppingLists.DataAccessLayer
{
    public class ListItemRepository : CrudRepository<ListItem>, IListItemRepository
    {
        public ListItemRepository(ShoppingListsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ListItem FindByDescription(string description, long shoppingListId)
        {
            return dbContext.ListItems.Where(li => li.Description == description && li.ShoppingListId == shoppingListId).FirstOrDefault();
        }

        public void UnpickAllListItems(long shoppingListId)
        {
            var shoppingList = dbContext.ShoppingLists.Find(shoppingListId);
            dbContext.Entry(shoppingList).Collection(sl => sl.ListItems).Load();
            shoppingList.ListItems.ToList().ForEach(li => li.StatusId = Statuses.NotPicked);
            dbContext.Entry(shoppingList).State = EntityState.Modified;
        }
    }
}
