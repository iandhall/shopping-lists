using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.IntegrationTests.DataAccessLayer
{
    [TestClass]
    public class PermissionRepositoryTests
    {
        private DbConnection _connection;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _connection = Effort.DbConnectionFactory.CreateTransient();
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            _connection.Dispose();
        }

        [TestMethod]
        public void Get_ShouldRetriveAPermission_ForTheSpecifiedPermissionIdUserAndShoppingList()
        {
            var testDataKeys = CreateTestData();

            using (var dbContext = TestUtils.CreateDbContext(_connection, new MockUserContext(testDataKeys.UserId)))
            using (IUnitOfWork unitOfWork = new EfUnitOfWork(dbContext))
            {
                var permissionRepository = new PermissionRepository(dbContext);
                var permission = permissionRepository.Get(Permissions.PickOrUnpickListItems, testDataKeys.UserId, testDataKeys.ShoppingListId);
                Assert.AreEqual(Permissions.PickOrUnpickListItems, permission.PermissionTypeId);
                Assert.AreEqual(testDataKeys.ShoppingListId, permission.ShoppingListId);
                Assert.AreEqual(testDataKeys.UserId, permission.UserId);
            }
        }

        [TestMethod]
        public void FindAllForUserAndShoppingList_ShouldRetieveAllPermissions_ForAGivenUserAndShoppingList()
        {
            var testDataKeys = CreateTestData();

            using (var dbContext = TestUtils.CreateDbContext(_connection, new MockUserContext(testDataKeys.UserId)))
            using (IUnitOfWork unitOfWork = new EfUnitOfWork(dbContext))
            {
                var permissionRepository = new PermissionRepository(dbContext);
                var permissions = permissionRepository.FindAllForUserAndShoppingList(testDataKeys.UserId, testDataKeys.ShoppingListId);
                Assert.AreEqual(4, permissions.Count());
            }
        }

        [TestMethod]
        public void DeleteAllForUserAndShoppingList_ShouldDeleteAllPermissions_ForTheGivenUserAndShoppingList()
        {
            var testDataKeys = CreateTestData();

            using (var dbContext = TestUtils.CreateDbContext(_connection, new MockUserContext(testDataKeys.UserId)))
            using (IUnitOfWork unitOfWork = new EfUnitOfWork(dbContext))
            {
                var permissionRepository = new PermissionRepository(dbContext);
                permissionRepository.DeleteAllForUserAndShoppingList(testDataKeys.ShoppingListId, testDataKeys.UserId);
                unitOfWork.SaveChanges();
            }
            using (var dbContext = TestUtils.CreateDbContext(_connection, new MockUserContext(testDataKeys.UserId)))
            using (IUnitOfWork unitOfWork = new EfUnitOfWork(dbContext))
            {
                var permissionRepository = new PermissionRepository(dbContext);
                var permissions = permissionRepository.FindAllForUserAndShoppingList(testDataKeys.UserId, testDataKeys.ShoppingListId);
                Assert.AreEqual(0, permissions.Count());
            }
        }

        private TestDataKeys CreateTestData()
        {
            var testDataKeys = new TestDataKeys();

            // Some other user's ShoppingList and Permissions
            var userId = TestUtils.NewId();
            using (var dbContext = TestUtils.CreateDbContext(_connection, new MockUserContext(userId)))
            {
                var shoppingList = new ShoppingList { Title = TestUtils.GetTestShoppingListTitle() };
                dbContext.ShoppingLists.Add(shoppingList);
                dbContext.SaveChanges();
                var shoppingListId = shoppingList.Id;
                dbContext.Permissions.AddRange(new List<Permission>
                {
                    new Permission { PermissionTypeId = Permissions.Edit, UserId = userId, ShoppingListId = shoppingListId },
                    new Permission { PermissionTypeId = Permissions.AddListItems, UserId = userId, ShoppingListId = shoppingListId }
                });
                dbContext.SaveChanges();
            }

            // The user of interest's ShoppingLists and Permissions
            testDataKeys.UserId = TestUtils.NewId();
            using (var dbContext = TestUtils.CreateDbContext(_connection, new MockUserContext(testDataKeys.UserId)))
            {
                var shoppingList = new ShoppingList { Title = TestUtils.GetTestShoppingListTitle() };
                dbContext.ShoppingLists.Add(shoppingList);
                dbContext.SaveChanges();
                testDataKeys.ShoppingListId = shoppingList.Id;
                dbContext.Permissions.AddRange(new List<Permission>
                {
                    new Permission { PermissionTypeId = Permissions.Edit, UserId = testDataKeys.UserId, ShoppingListId = testDataKeys.ShoppingListId },
                    new Permission { PermissionTypeId = Permissions.PickOrUnpickListItems, UserId = testDataKeys.UserId, ShoppingListId = testDataKeys.ShoppingListId },
                    new Permission { PermissionTypeId = Permissions.View, UserId = testDataKeys.UserId, ShoppingListId = testDataKeys.ShoppingListId },
                    new Permission { PermissionTypeId = Permissions.AddListItems, UserId = testDataKeys.UserId, ShoppingListId = testDataKeys.ShoppingListId }
                });
                dbContext.SaveChanges();

                shoppingList = new ShoppingList { Title = TestUtils.GetTestShoppingListTitle() };
                dbContext.ShoppingLists.Add(shoppingList);
                dbContext.SaveChanges();
                var shoppingListId2 = shoppingList.Id;
                dbContext.Permissions.AddRange(new List<Permission>
                {
                    new Permission { PermissionTypeId = Permissions.Edit, UserId = testDataKeys.UserId, ShoppingListId = shoppingListId2 },
                    new Permission { PermissionTypeId = Permissions.View, UserId = testDataKeys.UserId, ShoppingListId = shoppingListId2 },
                    new Permission { PermissionTypeId = Permissions.AddListItems, UserId = testDataKeys.UserId, ShoppingListId = shoppingListId2 }
                });
                dbContext.SaveChanges();
            }

            // Yet another user's ShoppingList and Permissions
            userId = TestUtils.NewId();
            using (var dbContext = TestUtils.CreateDbContext(_connection, new MockUserContext(userId)))
            {
                var shoppingList = new ShoppingList { Title = TestUtils.GetTestShoppingListTitle() };
                dbContext.ShoppingLists.Add(shoppingList);
                dbContext.SaveChanges();
                var shoppingListId = shoppingList.Id;
                dbContext.Permissions.AddRange(new List<Permission>
                {
                    new Permission { PermissionTypeId = Permissions.Share, UserId = userId, ShoppingListId = shoppingListId },
                    new Permission { PermissionTypeId = Permissions.Edit, UserId = userId, ShoppingListId = shoppingListId },
                    new Permission { PermissionTypeId = Permissions.AddListItems, UserId = userId, ShoppingListId = shoppingListId }
                });
                dbContext.SaveChanges();
            }

            return testDataKeys;
        }
    }

    public class TestDataKeys
    {
        public string UserId { get; set; }
        public long ShoppingListId { get; set; }
    }
}