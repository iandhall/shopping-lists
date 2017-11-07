using System;
using System.Transactions;
using ShoppingLists.Core;

namespace ShoppingLists.DataAccessLayer
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private TransactionScope transactionScope;
        private ShoppingListsDbContext dbContext;
        //private static long instanceCount;
        //private long instanceId;
        //private static object locker = new object();

        public EfUnitOfWork(ShoppingListsDbContext dbContext)
        {
            //lock (locker) {
            //    instanceId = ++instanceCount;
            //}
            //_log.Debug("instanceId={0}", instanceId);
            if (Transaction.Current != null) throw new ApplicationException("A transaction already exists.");
            transactionScope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.DefaultTimeout
                },
                TransactionScopeAsyncFlowOption.Enabled
            );
            this.dbContext = dbContext;
        }

        public void Complete()
        {
            //lock (locker) {
            //  _log.Debug("instanceId={0}", instanceId);
            //}
            if (dbContext.ChangeTracker.HasChanges())
            {
                dbContext.SaveChanges();
            }
            transactionScope.Complete();
        }

        public void Dispose()
        {
            //lock (locker) {
            //  _log.Debug("instanceId={0}", instanceId);
            //}
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
            if (transactionScope != null)
            {
                transactionScope.Dispose();
            }
            transactionScope = null;
        }
    }
}
