using System.Data;
using System.Data.Entity;
using System.Linq;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.DataAccessLayer
{
    public class ListItemRepository : CrudRepository<ListItem>
    {
        private ShoppingListsDbContext _dbContext;

        public ListItemRepository(ShoppingListsDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public ListItem FindByDescription(string description, long shoppingListId)
        {
            return _dbContext.ListItems.Where(li => li.Description == description && li.ShoppingListId == shoppingListId).FirstOrDefault();
        }

        public void UnpickAllListItems(long shoppingListId)
        {
            var shoppingList = _dbContext.ShoppingLists.Find(shoppingListId);
            _dbContext.Entry(shoppingList).Collection(sl => sl.ListItems).Load();
            shoppingList.ListItems.ToList().ForEach(li => li.StatusId = Statuses.NotPicked);
            _dbContext.Entry(shoppingList).State = EntityState.Modified;
        }
    }
}
