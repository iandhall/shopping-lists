using System;
using System.Linq;
using Dapper;
using LightInject;
using LightInject.Mocking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;
using ShoppingLists.Shared.RepositoryInterfaces;

namespace ShoppingLists.Tests.DataAccessLayer
{
    [TestClass]
    public class ListItemRepositoryTests
    {
        private IUnitOfWork _uow;
        private IListItemRepository _repository;
        private static ServiceContainer _container;
        private Scope _scope;
        private static ListItemRepositoryTestData _td;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            // Create test data.
            _td = TestUtils.InitialiseTestData<ListItemRepositoryTestData>(@"DataAccessLayer\sql\InitListItemRepositoryTestData.sql");

            // Init DI.
            _container = TestUtils.GetDiContainer();
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
            _scope = _container.BeginScope();
            _uow = _container.GetInstance<IUnitOfWork>();

            //_container.Register<UserIdProvider>(f => new UserIdProvider(_td.userId0));
            var userContextMock = new Mock<IUserContext>();
            userContextMock.SetupGet(s => s.UserId).Returns((Guid?)Guid.NewGuid());
            _container.StartMocking<IUserContext>(() => userContextMock.Object);

            _repository = _container.GetInstance<IListItemRepository>();
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            _scope.Dispose(); // Disposes the Di container, scope and IDisposable implementors like the UnitOfWork.
        }

        [TestMethod]
        public void TestGet()
        {
            var listItem = _repository.Get(_td.listItemGetId);
            _uow.Complete();
            Assert.AreEqual(_td.listItemGetId, listItem.Id);
        }
        
        [TestMethod]
        public void TestGetByDescription()
        {
            var listItem = _repository.FindByDescription("LiRepo - To be matched by description.", _td.shoppingListGetId);
            Assert.AreEqual(_td.listItemGetByDescId, listItem.Id);
        }

        [TestMethod]
        public void TestUnpickAllListItems()
        {
            _repository.UnpickAllListItems(_td.shoppingListUnpickAllId);
            _uow.Complete();
            _uow.Dispose();
            
            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(0, con.Query<int>("select count(1) from ListItems where ShoppingListId = @id and StatusId = @statusId", new { id = _td.shoppingListUnpickAllId, statusId = Statuses.Picked }).First());
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
