using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Core.Entities;
using Dapper;
using System.Transactions;

namespace ShoppingLists.Tests.DataAccess
{
    [TestClass]
    public class UnitOfWorkTests
    {
        private EfUnitOfWork uow;
        private ShoppingList testShoppingList;
        private static EfUnitOfWorkTestData td;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            // Create test data.
            td = TestUtils.InitialiseTestData<EfUnitOfWorkTestData>(@"DataAccessLayer\sql\InitEfUnitOfWorkTestData.sql");
        }

        [TestMethod]
        public void TestRollbackOnError()
        {
            try
            { // Prevent SimulatedFailureException from halting tests when running in debug:
                SimulateFailureWhileUpdatingData();
            }
            catch (SimulatedFailureException ex)
            {
                using (var ts = new TransactionScope(TransactionScopeOption.RequiresNew)) // Will error if the UOW TransactionScope hasn't been disposed for some reason.
                using (var con = TestUtils.GetConnection())
                {
                    // Check that update gets rollback after SimulatedFailureException.
                    Assert.AreEqual("Test UOW - Rollback test - Initial state", con.Query<string>("select Title from ShoppingLists where Id = @id", new { id = td.shoppingListRollbackId }).First());
                    ts.Complete();
                }
                return;
            }
            Assert.Fail("Expected SimulateFailureWhileInsertingData to throw SimulatedFailureException.");
        }

        private void SimulateFailureWhileUpdatingData()
        {
            using (var dbContext = new ShoppingListsDbContext())
            using (uow = new EfUnitOfWork(dbContext))
            {
                testShoppingList = dbContext.ShoppingLists.Find(td.shoppingListRollbackId);
                testShoppingList.Title = "Test UOW - This change should get rolled back!";
                dbContext.Entry(testShoppingList).State = EntityState.Modified;
                throw new SimulatedFailureException();
                uow.Complete();
            }
        }

        private class SimulatedFailureException : ApplicationException
        {
        }

        [TestMethod]
        public void TestComplete()
        {
            const string titleChange = "Test UOW - Complete test - This change should stick!";

            using (var dbContext = new ShoppingListsDbContext())
            using (uow = new EfUnitOfWork(dbContext))
            { 
                testShoppingList = dbContext.ShoppingLists.Find(td.shoppingListCompleteId);
                testShoppingList.Title = titleChange;
                dbContext.Entry(testShoppingList).State = EntityState.Modified;
                uow.Complete();
            }

            using (var ts = new TransactionScope(TransactionScopeOption.RequiresNew)) // Will error if the UOW TransactionScope hasn't been disposed for some reason.
            using (var con = TestUtils.GetConnection())
            {
                // Check that update gets rollback after SimulatedFailureException.
                Assert.AreEqual(titleChange, con.Query<string>("select Title from ShoppingLists where Id = @id", new { id = td.shoppingListCompleteId }).First());
                ts.Complete();
            }
        }
    }
    
    public class EfUnitOfWorkTestData
    {
        public string userId0 { get; set; }
        public long shoppingListRollbackId { get; set; }
        public long shoppingListCompleteId { get; set; }
    }
}
