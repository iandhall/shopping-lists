﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Configuration;
using LogForMe;
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
            //Logger.Debug("instanceId={0}", instanceId);
            if (Transaction.Current != null) throw new ApplicationException("A transaction already exists.");
            transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled);
            this.dbContext = dbContext;
        }

        public void Complete()
        {
            //lock (locker) {
            //  Logger.Debug("instanceId={0}", instanceId);
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
            //  Logger.Debug("instanceId={0}", instanceId);
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
