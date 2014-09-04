using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingLists.Core.RepositoryInterfaces;
using ShoppingLists.Core;
using ShoppingLists.Core.Entities;
using ShoppingLists.Tests;
using LightInject;
using Dapper;
using System.IO;

namespace ShoppingLists.Tests.DataAccessLayer
{
    [TestClass]
    public class UserRepositoryTests
    {
        private User user;
        private IUnitOfWork uow;
        private IUserRepository repository;
        private static ServiceContainer container;
        private Scope scope;
        private static UserRepositoryTestData td;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            // Create test data.
            td = TestUtils.InitialiseTestData<UserRepositoryTestData>(@"DataAccessLayer\sql\InitUserRepositoryTestData.sql");

            // Init DI.
            container = TestUtils.GetDiContainer();
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
            scope = container.BeginScope();
            uow = container.GetInstance<IUnitOfWork>();
            repository = container.GetInstance<IUserRepository>();
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            scope.Dispose(); // Disposes the Di container, scope and IDisposable implementors like the UnitOfWork.
        }

        [TestMethod]
        public void TestGetWithoutIncludes()
        {
            user = repository.Get(td.userId0);
            uow.Complete();
            Assert.AreEqual(td.userId0, user.Id);
            Assert.IsNull(user.ShoppingListPermissions);
        }
        
        [TestMethod]
        public void TestGetIncludePermissions()
        {
            user = repository.Get(td.userId0, includePermissions: true, shoppingListId: td.shoppingListGetId);
            uow.Complete();
            Assert.AreEqual(3, user.ShoppingListPermissions.Count());
        }

        [TestMethod]
        public void TestGetShouldReturnNullForNonExistentUser()
        {
            user = repository.Get("999999");
            uow.Complete();
            Assert.IsNull(user);
        }
        
        [TestMethod]
        public void TestCreate()
        {
            const string userId = "123123-999999-123123";
            user = new User { Id = userId, Username = "testnewuser", Discriminator = "ApplicationUser" };
            repository.Create(user);
            uow.Complete();
            uow.Dispose();

            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(1, con.Query<int>("select count(1) from Users where Id = @id", new { id = userId }).First());
            }
        }

        [TestMethod]
        public void TestUpdate()
        {
            user = repository.Get(td.userId2);
            user.Username = "TestUserRepoUser2b";
            repository.Update(user);
            uow.Complete();
            uow.Dispose();

            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual("TestUserRepoUser2b", con.Query<string>("select Username from Users where Id = @id", new { id = td.userId2 }).First());
            }
        }

        [TestMethod]
        public void TestDelete()
        {
            repository.Delete(td.userId3);
            uow.Complete();
            uow.Dispose();

            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(0, con.Query<int>("select count(1) from Users where Id = @id", new { id = td.userId3 }).First());
                Assert.AreEqual(0, con.Query<int>("select count(1) from ShoppingListPermissions where UserId = @id", new { id = td.userId3 }).First());
            }
        }
        
        [TestMethod]
        public void TestGetByName()
        {
            user = repository.FindByName("TestUserRepoUser1");
            uow.Complete();
            Assert.AreEqual(td.userId1, user.Id);
        }

        [TestMethod]
        public void TestGetByNameShouldReturnNullIfCaseDifferent()
        {
            user = repository.FindByName("tESTUseRrePOUSeR1");
            uow.Complete();
            Assert.IsNull(user);
        }

        [TestMethod]
        public void TestGetAllForShoppingList()
        {
            var users = repository.FindAllForShoppingList(td.shoppingListGetId);
            uow.Complete();
            Assert.AreEqual(3, users.Count());
        }
    }

    public class UserRepositoryTestData
    {
        public string userId0 { get; set; }
        public string userId1 { get; set; }
        public string userId2 { get; set; }
        public string userId3 { get; set; }
        public long shoppingListGetId { get; set; }
    }
}
