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

namespace ShoppingLists.Tests.DataAccessLayer
{
    [TestClass]
    public class ShoppingListRepositoryTests
    {
        private IUnitOfWork uow;
        private IShoppingListRepository repository;
        private static ServiceContainer container;
        private Scope scope;
        private static ShoppingListRepositoryTestData td;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            // Create test data.
            td = TestUtils.InitialiseTestData<ShoppingListRepositoryTestData>(@"DataAccessLayer\sql\InitShoppingListRepositoryTestData.sql");

            // Init DI.
            container = TestUtils.GetDiContainer();
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
            scope = container.BeginScope();
            uow = container.GetInstance<IUnitOfWork>();
            repository = container.GetInstance<IShoppingListRepository>();
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            scope.Dispose(); // Disposes the Di container, scope and IDisposable implementors like the UnitOfWork.
        }

        [TestMethod]
        public void TestGetWithoutIncludes()
        {
            var shoppingList = repository.Get(td.shoppingListGetId);
            uow.Complete();
            Assert.AreEqual(td.shoppingListGetId, shoppingList.Id);
            Assert.IsNull(shoppingList.ListItems);
        }

        [TestMethod]
        public void TestGetIncludeListItems()
        {
            var shoppingList = repository.Get(td.shoppingListGetId, includeListItems: true);
            uow.Complete();
            Assert.AreEqual(2, shoppingList.ListItems.Count);
        }

        [TestMethod]
        public void TestGetIncludeCreator()
        {
            var shoppingList = repository.Get(td.shoppingListGetId, includeCreator: true);
            uow.Complete();
            Assert.AreEqual(td.username0, shoppingList.Creator.Username);
        }

        [TestMethod]
        public void TestFindAllForUser()
        {
            var shoppingLists = repository.FindAllForUser(td.userId3);
            Assert.AreEqual(3, shoppingLists.Count());
            Assert.AreEqual(td.username3, shoppingLists.First().Creator.Username);
            uow.Complete();
        }

        [TestMethod]
        public void TestDelete()
        {
            repository.Delete(td.shoppingListDeleteId);
            uow.Complete();
            uow.Dispose();
            
            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(0, con.Query<int>("select count(1) from ShoppingLists where Id = @id", new { id = td.shoppingListDeleteId }).First());
                Assert.AreEqual(0, con.Query<int>("select count(1) from ListItems where ShoppingListId = @id", new { id = td.shoppingListDeleteId }).First());
                Assert.AreEqual(0, con.Query<int>("select count(1) from ShoppingListPermissions where ShoppingListId = @id", new { id = td.shoppingListDeleteId }).First());
            }
        }

        [TestMethod]
        public void TestCreate()
        {
            const string insertTitle = "Test list to insert";
            var shoppingList = new ShoppingList { Title = insertTitle };
            repository.Create(shoppingList, td.userId0);
            uow.Complete();
            uow.Dispose();

            Assert.IsTrue(shoppingList.Id != 0);

            // Check that CreatorId is set correctly
            Assert.AreEqual(td.userId0, shoppingList.CreatorId);

            // Check that a CreatedDate has been set
            Assert.AreNotEqual(default(DateTime), shoppingList.CreatedDate);

            // Check that there is only one list with this title
            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(1, con.Query<int>("select count(1) from ShoppingLists where Title = @title", new { title = insertTitle }).First());
            }

        }

        [TestMethod]
        public void TestUpdate()
        {
            const string updateTitle = "SlRepo - Test list to update - Updated!";
            var shoppingList = repository.Get(td.shoppingListUpdateId);
            shoppingList.Title = updateTitle;
            var userId = Guid.NewGuid().ToString();
            repository.Update(shoppingList, userId);
            uow.Complete();
            uow.Dispose();

            Assert.AreEqual(userId, shoppingList.AmenderId);
            Assert.AreNotEqual(default(DateTime), shoppingList.AmendedDate);

            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(1, con.Query<int>("select count(1) from ShoppingLists where Title = @title", new { title = updateTitle }).First());
            }
        }
        
        [TestMethod]
        public void TestFindByPartialTitleMatch()
        {
            var shoppingLists = repository.FindByPartialTitleMatch("SlRepo - Many per user ", td.userId1).ToList();
            int i = 0;
            Assert.AreEqual("SlRepo - Many per user a", shoppingLists[i++].Title);
            Assert.AreEqual("SlRepo - Many per user 1", shoppingLists[i++].Title);
            Assert.AreEqual("SlRepo - Many per user user", shoppingLists[i++].Title);
            Assert.AreEqual("SlRepo - Many per user 10", shoppingLists[i++].Title);
            Assert.AreEqual(i, shoppingLists.Count);
        }

        [TestMethod]
        public void TestGetByTitle()
        {
            var shoppingList = repository.FindByTitle("SlRepo - Many per user 1", td.userId1);
            Assert.AreEqual("SlRepo - Many per user 1", shoppingList.Title);
        }

        [TestMethod]
        public void TestGetByTitleShouldNotMatchAnotherUsersShoppingLists()
        {
            var shoppingList = repository.FindByTitle("SlRepo - Many per user 2", td.userId1);
            Assert.IsNull(shoppingList);
        }
    }

    public class ShoppingListRepositoryTestData
    {
        public string userId0 { get; set; }
        public string userId1 { get; set; }
        public string userId2 { get; set; }
        public string userId3 { get; set; }
        public string username0 { get; set; }
        public string username1 { get; set; }
        public string username2 { get; set; }
        public string username3 { get; set; }
        public long shoppingListGetId { get; set; }
        public long shoppingListUpdateId { get; set; }
        public long shoppingListDeleteId { get; set; }
    }
}
