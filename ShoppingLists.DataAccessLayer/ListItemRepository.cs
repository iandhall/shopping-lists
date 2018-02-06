using System.Data;
using System.Data.Entity;
using System.Linq;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;
using ShoppingLists.Shared.RepositoryInterfaces;

namespace ShoppingLists.DataAccessLayer
{
    public class ListItemRepository : IListItemRepository
    {
        private ShoppingListsDbContext _dbContext;

        public ListItemRepository(ShoppingListsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual ListItem Get(long id)
        {
            return _dbContext.ListItems.Find(id);
        }

        public virtual void Create(ListItem listItem)
        {
            _dbContext.ListItems.Add(listItem);
        }

        public virtual void Update(ListItem listItem)
        {
            _dbContext.Entry(listItem).State = EntityState.Modified;
        }

        public virtual void Delete(long id)
        {
            var entity = _dbContext.ListItems.Find(id);
            _dbContext.ListItems.Remove(entity);
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
