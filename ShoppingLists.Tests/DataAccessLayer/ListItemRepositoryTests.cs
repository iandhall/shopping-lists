using System;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingLists.Core.Entities;
using ShoppingLists.Tests;
using ShoppingLists.Core.RepositoryInterfaces;
using ShoppingLists.Core;
using LightInject;
using LogForMe;

namespace ShoppingLists.Tests.DataAccess
{
    [TestClass]
    public class ListItemRepositoryTests
    {
        private IUnitOfWork uow;
        private IListItemRepository repository;
        private static ServiceContainer container;
        private Scope scope;
        private static ListItemRepositoryTestData td;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            // Create test data.
            td = TestUtils.InitialiseTestData<ListItemRepositoryTestData>(@"DataAccessLayer\sql\InitListItemRepositoryTestData.sql");

            // Init DI.
            container = TestUtils.GetDiContainer();
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
            scope = container.BeginScope();
            uow = container.GetInstance<IUnitOfWork>();
            repository = container.GetInstance<IListItemRepository>();
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            scope.Dispose(); // Disposes the Di container, scope and IDisposable implementors like the UnitOfWork.
        }

        [TestMethod]
        public void TestGet()
        {
            var listItem = repository.Get(td.listItemGetId);
            uow.Complete();
            Assert.AreEqual(td.listItemGetId, listItem.Id);
        }

        [TestMethod]
        public void TestDelete()
        {
            repository.Delete(td.listItemDeleteId);
            uow.Complete();
            uow.Dispose();
            
            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(0, con.Query<int>("select count(1) from ListItems where Id = @id", new { id = td.listItemDeleteId }).First());
            }
        }

        [TestMethod]
        public void TestCreate()
        {
            const string insertDescription = "LiRepo - Test ListItem to insert";
            var listItem = new ListItem { Description = insertDescription, Quantity = 1, StatusId = Statuses.NotPicked, ShoppingListId = td.shoppingListGetId, CreatorId = td.userId0, CreatedDate = DateTime.Now };
            repository.Create(listItem);
            uow.Complete();
            uow.Dispose();

            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(1, con.Query<int>("select count(1) from ListItems where Description = @desc", new { desc = insertDescription }).First());
            }
        }

        [TestMethod]
        public void TestUpdate()
        {
            const string updateDescription = "LiRepo - Test ListItem to update - Updated!";
            var listItem = repository.Get(td.listItemUpdateId);
            listItem.Description = updateDescription;
            repository.Update(listItem);
            uow.Complete();
            uow.Dispose();

            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(1, con.Query<int>("select count(1) from ListItems where Description = @desc", new { desc = updateDescription }).First());
            }
        }
        
        [TestMethod]
        public void TestGetByDescription()
        {
            var listItem = repository.GetByDescription("LiRepo - To be matched by description.", td.shoppingListGetId);
            Assert.AreEqual(td.listItemGetByDescId, listItem.Id);
        }

        [TestMethod]
        public void TestUnpickAllListItems()
        {
            repository.UnpickAllListItems(td.shoppingListUnpickAllId);
            uow.Complete();
            uow.Dispose();
            
            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(0, con.Query<int>("select count(1) from ListItems where ShoppingListId = @id and StatusId = @statusId", new { id = td.shoppingListUnpickAllId, statusId = Statuses.Picked }).First());
            }
        }
    }

    public class ListItemRepositoryTestData
    {
        public string userId0 { get; set; }
        public long shoppingListGetId { get; set; }
        public long shoppingListUnpickAllId { get; set; }
        public long listItemGetId { get; set; }
        public long listItemGetByDescId { get; set; }
        public long listItemUpdateId { get; set; }
        public long listItemDeleteId { get; set; }
    }
}
