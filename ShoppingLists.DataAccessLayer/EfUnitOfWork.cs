using ShoppingLists.Shared;

namespace ShoppingLists.DataAccessLayer
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private bool disposedValue = false; // To detect redundant calls
        private ShoppingListsDbContext _dbContext;

        public EfUnitOfWork(ShoppingListsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects).
                    _dbContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
