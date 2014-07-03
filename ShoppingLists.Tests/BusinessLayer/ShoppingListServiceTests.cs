using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingLists.BusinessLayer;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core;
using ShoppingLists.Core.RepositoryInterfaces;
using ShoppingLists.Tests;
using ShoppingLists.BusinessLayer.Exceptions;
using Moq;
using LightInject;
using LightInject.Mocking;

namespace ShoppingLists.Tests.Services
{
    [TestClass]
    public class ShoppingListServiceTests
    {
        private IUnitOfWork uow;
        private ShoppingListService service;
        private static long getId;
        private static long ignoreId;
        private static long noPermissionsId;
        private ShoppingList shoppingList;
        private static ServiceContainer container;
        private Scope scope;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            getId = 123999;
            new ShoppingList
            {
                Id = getId, Title = "Test list to get", CreatedByUserId = TestData.testUserIds[0], CreatedDate = DateTime.Now,
                ListItems = new List<ListItem>()
                {
                    new ListItem { Description = "Test list item 1", ShoppingListId = getId, CreatedByUserId = TestData.testUserIds[0], CreatedDate = DateTime.Now },
                    new ListItem { Description = "Test list item 2", ShoppingListId = getId, CreatedByUserId = TestData.testUserIds[0], CreatedDate = DateTime.Now },
                    new ListItem { Description = "Test list item 3 to ingore", ShoppingListId = ignoreId, CreatedByUserId = TestData.testUserIds[0], CreatedDate = DateTime.Now }
                }
            };

            // Orphaned EntityPermission (without ShoppingList).
            TestData.Add(new EntityPermission() { PermissionId = Permissions.View, EntityName = "ShoppingList", EntityId = 999, UserId = TestData.testUserIds[0], CreatedByUserId = TestData.testUserIds[0], CreatedDate = DateTime.Now }, con);

            ignoreId = TestData.Add(new ShoppingList { Title = "Test list to ignore", CreatedByUserId = TestData.testUserIds[0], CreatedDate = DateTime.Now }, con);
            noPermissionsId = TestData.Add(new ShoppingList { Title = "Test without permissions", CreatedByUserId = TestData.testUserIds[0], CreatedDate = DateTime.Now }, con, false);

            // Init DI.
            container = TestUtils.GetDiContainer();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            //var mockShoppingListRepository = new Mock<IShoppingListRepository>();
            IShoppingListRepository mockShoppingListRepository = Mock.Of<IShoppingListRepository>(slr => slr.Get = 


            var mockListItemRepository = new Mock<IListItemRepository>();
            var mockCrudRepositoryShoppingList = new Mock<ICrudRepository<ShoppingList>>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockShoppingListPermissionRepository = new Mock<IShoppingListPermissionRepository>();
            var mockCrudRepositoryShoppingListPermission = new Mock<ICrudRepository<ShoppingListPermission>>();

            container.StartMocking<IShoppingListRepository>(() => mockShoppingListRepository.Object);
        }

        [ClassCleanup]
        public void Cleanup()
        {
            container.EndMocking<IUnitOfWork>();
            container.EndMocking<IShoppingListRepository>();
            container.EndMocking<IListItemRepository>();
            container.EndMocking<ICrudRepository<ShoppingList>>();
            container.EndMocking<IUserRepository>();
            container.EndMocking<IShoppingListPermissionRepository>();
            container.EndMocking<ICrudRepository<ShoppingListPermission>>();
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
            scope = container.BeginScope();
            uow = container.GetInstance<EfUnitOfWork>();
            service = container.GetInstance<ShoppingListService>();
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            scope.Dispose();
        }

        [TestMethod]
        public void TestGetIncludeListItems()
        {
            shoppingList = service.Get(getId, TestData.testUserIds[0], includeListItems: true);
            uow.Complete();
            Assert.AreEqual(2, shoppingList.ListItems.Count());
        }

        [TestMethod, ExpectedException(typeof(EntityNotFoundException))]
        public void TestGetEntityNotFound()
        {
            shoppingList = service.Get(999, TestData.testUserIds[0]); // Should throw EntityNotFoundException.
        }

        [TestMethod, ExpectedException(typeof(PermissionNotFoundException))]
        public void TestGetNoPermission()
        {
            shoppingList = service.Get(noPermissionsId, TestData.testUserIds[0]);
        }
    }
}
