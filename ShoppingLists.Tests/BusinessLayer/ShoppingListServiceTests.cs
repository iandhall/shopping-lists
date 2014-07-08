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
        private ShoppingListService service;
        private static ServiceContainer container;
        private Scope scope;

        public const int shoppingListGetId = 123999;
        public const int shoppingListNotExistingId = 999999;

        public static List<string> userIds = new List<string>() {
            "cfbf134b-705e-4cde-947b-d1c5d6c32062", "e6db5c8b-9793-4a2e-8d11-eccf177af70e", "6dfd3e83-0000-421a-b104-216d4b208ef3", "9zfd3e83-0000-421a-b104-216d4b208ez9" 
        };

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            // Init DI.
            container = TestUtils.GetDiContainer();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
            container.StartMocking<IUnitOfWork>(() => Mock.Of<IUnitOfWork>());
            container.StartMocking<IListItemRepository>(() => Mock.Of<IListItemRepository>());
            container.StartMocking<ICrudRepository<ShoppingList>>(() => Mock.Of<ICrudRepository<ShoppingList>>());
            container.StartMocking<IUserRepository>(() => Mock.Of<IUserRepository>());
            container.StartMocking<ICrudRepository<ShoppingListPermission>>(() => Mock.Of<ICrudRepository<ShoppingListPermission>>());

            scope = container.BeginScope();
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            container.EndMocking<IShoppingListRepository>();
            container.EndMocking<IShoppingListPermissionRepository>();
            container.EndMocking<IUnitOfWork>();
            container.EndMocking<IListItemRepository>();
            container.EndMocking<ICrudRepository<ShoppingList>>();
            container.EndMocking<IUserRepository>();
            container.EndMocking<ICrudRepository<ShoppingListPermission>>();

            scope.Dispose();
        }

        [TestMethod]
        public void TestGetWithoutIncludes()
        {
            var mockShoppingListRepository = new Mock<IShoppingListRepository>(MockBehavior.Strict);
            mockShoppingListRepository.Setup(slr => slr.Get(shoppingListGetId, false, false)).Returns(
                new ShoppingList
                {
                    Id = shoppingListGetId, Title = "Test list to get", CreatorId = userIds[0], CreatedDate = DateTime.Now
                }
            );
            container.StartMocking<IShoppingListRepository>(() => mockShoppingListRepository.Object);
            CreateMockPermission(Permissions.View, shoppingListGetId, userIds[0]);
            service = container.GetInstance<ShoppingListService>();

            var shoppingList = service.Get(shoppingListGetId, userIds[0]);
            Assert.IsNull(shoppingList.ListItems);
        }

        [TestMethod]
        public void TestGetIncludeListItems()
        {
            var mockShoppingListRepository = new Mock<IShoppingListRepository>(MockBehavior.Strict);
            mockShoppingListRepository.Setup(slr => slr.Get(shoppingListGetId, true, false)).Returns(
                new ShoppingList
                {
                    Id = shoppingListGetId, Title = "Test list to get", CreatorId = userIds[0], CreatedDate = DateTime.Now,
                    ListItems = new List<ListItem>()
                    {
                        new ListItem { Description = "Test list item 1", ShoppingListId = shoppingListGetId, CreatorId = userIds[0], CreatedDate = DateTime.Now },
                        new ListItem { Description = "Test list item 2", ShoppingListId = shoppingListGetId, CreatorId = userIds[0], CreatedDate = DateTime.Now }
                    }
                }
            );
            container.StartMocking<IShoppingListRepository>(() => mockShoppingListRepository.Object);
            CreateMockPermission(Permissions.View, shoppingListGetId, userIds[0]);
            service = container.GetInstance<ShoppingListService>();

            var shoppingList = service.Get(shoppingListGetId, userIds[0], includeListItems: true);
            Assert.AreEqual(2, shoppingList.ListItems.Count());
        }

        [TestMethod, ExpectedException(typeof(EntityNotFoundException))]
        public void TestGetEntityNotFound()
        {
            var mockShoppingListRepository = new Mock<IShoppingListRepository>(MockBehavior.Strict);
            mockShoppingListRepository.Setup(slr => slr.Get(shoppingListNotExistingId, false, false)).Returns<ShoppingList>(null);
            container.StartMocking<IShoppingListRepository>(() => mockShoppingListRepository.Object);
            CreateMockPermission(Permissions.View, shoppingListNotExistingId, userIds[0]);
            service = container.GetInstance<ShoppingListService>();

            var shoppingList = service.Get(shoppingListNotExistingId, userIds[0]); // Should throw EntityNotFoundException.
        }

        [TestMethod, ExpectedException(typeof(PermissionNotFoundException))]
        public void TestGetNoPermission()
        {
            var mockShoppingListRepository = new Mock<IShoppingListRepository>(MockBehavior.Strict);
            mockShoppingListRepository.Setup(slr => slr.Get(shoppingListGetId, false, false)).Returns(
                new ShoppingList
                {
                    Id = shoppingListGetId, Title = "Test list to get", CreatorId = userIds[0], CreatedDate = DateTime.Now
                }
            );
            container.StartMocking<IShoppingListRepository>(() => mockShoppingListRepository.Object);
            var mockShoppingListPermissionRepository = new Mock<IShoppingListPermissionRepository>();
            container.StartMocking<IShoppingListPermissionRepository>(() => mockShoppingListPermissionRepository.Object);
            service = container.GetInstance<ShoppingListService>();

            var shoppingList = service.Get(shoppingListGetId, userIds[0]);
        }

        private void CreateMockPermission(Permissions permissionTypeId, long shoppingListId, string userId)
        {
            var mockShoppingListPermissionRepository = new Mock<IShoppingListPermissionRepository>(MockBehavior.Strict);
            mockShoppingListPermissionRepository.Setup(slpr => slpr.Get(Permissions.View, userId, shoppingListId)).Returns(
                new ShoppingListPermission
                {
                    Id = 123, PermissionTypeId = permissionTypeId, UserId = userId, ShoppingListId = shoppingListId, CreatorId = userIds[0], CreatedDate = DateTime.Now
                }
            );
            container.StartMocking<IShoppingListPermissionRepository>(() => mockShoppingListPermissionRepository.Object);
        }
    }
}
