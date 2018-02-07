using System.Collections.Generic;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingLists.BusinessLayer;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.IntegrationTests.BusinessLayer
{
    [TestClass]
    public class ShoppingListServiceTests
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

        [TestMethod, ExpectedException(typeof(PermissionNotFoundException))]
        public void Get_ShouldThrowPermissionNotFoundException_IfTheShoppingListHasNoAssociatedPermissions()
        {
            var userContext = new MockUserContext(TestUtils.NewId());
            long shoppingListId;
            using (var dbContext = TestUtils.CreateDbContext(_connection, userContext))
            {
                var shoppingList = new ShoppingList { Title = "Shopping List #A" };
                dbContext.ShoppingLists.Add(shoppingList);
                dbContext.SaveChanges();
                shoppingListId = shoppingList.Id;
            }

            using (var dbContext = TestUtils.CreateDbContext(_connection, userContext))
            using (IUnitOfWork unitOfWork = new EfUnitOfWork(dbContext))
            {
                var shoppingListService = CreateShoppingListService(userContext, dbContext, unitOfWork);
                var shoppingList = shoppingListService.Get(shoppingListId);
            }
        }
        
        private ShoppingListService CreateShoppingListService(IUserContext userContext, ShoppingListsDbContext dbContext, IUnitOfWork unitOfWork)
        {
            var permissionService = new PermissionService(new PermissionRepository(dbContext));
            return new ShoppingListService(
                unitOfWork,
                userContext,
                new ShoppingListRepository(dbContext),
                permissionService,
                new ListItemRepository(dbContext),
                new UserService(unitOfWork, new UserRepository(dbContext), permissionService)
            );
        }
    }
}
