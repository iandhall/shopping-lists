using Dapper;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingLists.Shared.Entities;
using ShoppingLists.Shared.RepositoryInterfaces;
using ShoppingLists.Shared;
using LightInject;

namespace ShoppingLists.Tests.DataAccessLayer
{
    [TestClass]
    public class ShoppingListPermissionRepositoryTests
    {
        private IUnitOfWork _uow;
        private IShoppingListPermissionRepository _repository;
        private static ServiceContainer _container;
        private Scope _scope;
        private static ShoppingListPermissionRepositoryTestData _td;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            // Create test data.
            _td = TestUtils.InitialiseTestData<ShoppingListPermissionRepositoryTestData>(@"DataAccessLayer\sql\InitShoppingListPermissionRepositoryTestData.sql");

            // Init DI.
            _container = TestUtils.GetDiContainer();
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
            _scope = _container.BeginScope();
            _uow = _container.GetInstance<IUnitOfWork>();
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            _scope.Dispose(); // Disposes the Di container, scope and IDisposable implementors like the UnitOfWork.
        }

        [TestMethod]
        public void TestGetByPermissionType()
        {
            // Arrange:
            _repository = _container.GetInstance<IShoppingListPermissionRepository>();

            // Act:
            var shoppingListPermission = _repository.Get(Permissions.Share, _td.userId0, _td.shoppingListGetId);
            _uow.Complete();

            // Assert:
            Assert.AreEqual(_td.slpShareId, shoppingListPermission.Id);
        }
        
        [TestMethod]
        public void TestGetAllForUserAndShoppingList()
        {
            // Arrange:
            _repository = _container.GetInstance<IShoppingListPermissionRepository>();

            // Act:
            var slps = _repository.FindAllForUserAndShoppingList(_td.userId0, _td.shoppingListGetId);

            // Assert:
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
    }
}