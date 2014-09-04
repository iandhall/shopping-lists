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

namespace ShoppingLists.Tests.DataAccess
{
    [TestClass]
    public class ShoppingListPermissionRepositoryTests
    {
        private IUnitOfWork uow;
        private IShoppingListPermissionRepository repository;
        private static ServiceContainer container;
        private Scope scope;
        private static ShoppingListPermissionRepositoryTestData td;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            // Create test data.
            td = TestUtils.InitialiseTestData<ShoppingListPermissionRepositoryTestData>(@"DataAccessLayer\sql\InitShoppingListPermissionRepositoryTestData.sql");

            // Init DI.
            container = TestUtils.GetDiContainer();
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
            scope = container.BeginScope();
            uow = container.GetInstance<IUnitOfWork>();
            repository = container.GetInstance<IShoppingListPermissionRepository>();
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            scope.Dispose(); // Disposes the Di container, scope and IDisposable implementors like the UnitOfWork.
        }

        [TestMethod]
        public void TestGetByPermissionType()
        {
            var shoppingListPermission = repository.Get(Permissions.Share, td.userId0, td.shoppingListGetId);
            uow.Complete();
            Assert.AreEqual(td.slpShareId, shoppingListPermission.Id);
        }

        [TestMethod]
        public void TestDelete()
        {
            repository.Delete(td.slpDeleteId);
            uow.Complete();
            uow.Dispose();
            
            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(0, con.Query<int>("select count(1) from ShoppingListPermissions where Id = @id", new { id = td.slpDeleteId }).First());
            }
        }

        [TestMethod]
        public void TestCreate()
        {
            var shoppingListPermission = new ShoppingListPermission { PermissionTypeId = Permissions.View, UserId = td.userId3, ShoppingListId = td.shoppingListGetId, CreatorId = td.userId3, CreatedDate = DateTime.Now };
            repository.Create(shoppingListPermission);
            uow.Complete();
            uow.Dispose();
            long newId = shoppingListPermission.Id;

            Assert.IsTrue(newId > 0);
            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(1, con.Query<int>("select count(1) from ShoppingListPermissions where Id = @id", new { id = newId }).First());
            }
        }
        
        [TestMethod]
        public void TestGetAllForUserAndShoppingList()
        {
            var slps = repository.FindAllForUserAndShoppingList(td.userId0, td.shoppingListGetId);

            Assert.AreEqual(3, slps.Count());
        }
    }

    public class ShoppingListPermissionRepositoryTestData
    {
        public string userId0 { get; set; }
        public string userId1 { get; set; }
        public string userId2 { get; set; }
        public string userId3 { get; set; }
        public long shoppingListGetId { get; set; }
        public long slpShareId { get; set; }
        public long slpDeleteId { get; set; }
    }
}
