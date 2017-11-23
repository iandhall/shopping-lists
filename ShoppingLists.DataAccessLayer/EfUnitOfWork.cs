using ShoppingLists.Core;

namespace ShoppingLists.DataAccessLayer
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private ShoppingListsDbContext _dbContext;

        public EfUnitOfWork(ShoppingListsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
