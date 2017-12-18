using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Tests.DataAccessLayer
{
    [TestClass]
    public class CrudRepositoryTests
    {
        private IUnitOfWork _uow;
        private ShoppingListRepository _repository; // Implements CrudRepository<>

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
        }

        [TestCleanup]
        public void AfterEachTest()
        {
        }

        [TestMethod]
        public void Create_ShouldSetTheCreatorId()
        {
            var userId = Guid.NewGuid().ToString();
            var newId = Create(userId);

            Assert.AreEqual(
                userId,
                con.Query<string>("select CreatorId from ShoppingLists where Id = @Id", new { Id = newId }).First(),
                true
            );
        }

        //[TestMethod]
        //public void Create_ShouldSetTheCreatedDate()
        //{
        //    // Arrange:
        //    // Act:
        //    var newId = Create(Guid.NewGuid().ToString());

        //    // Assert:
        //    Assert.IsTrue(newId > 0);
        //    using (var con = TestUtils.GetConnection())
        //    {
        //        Assert.AreNotEqual(
        //            new DateTime(),
        //            con.Query<DateTime>("select CreatedDate from ShoppingLists where Id = @id", new { id = newId }).First()
        //        );
        //    }
        //}

        //[TestMethod]
        //public void Update_ShouldSetTheAmenderId()
        //{
        //    // Arrange:
        //    var userId = Guid.NewGuid().ToString();
        //    var trackingKey = Guid.NewGuid().ToString();

        //    // Act:
        //    var shoppingListId = Update(userId, trackingKey);

        //    // Assert:
        //    using (var con = TestUtils.GetConnection())
        //    {
        //        Assert.AreEqual(
        //            userId,
        //            con.Query<string>(
        //                @"select AmenderId from ShoppingLists where Id = @Id",
        //                new { Id = shoppingListId }
        //            ).First()
        //        );
        //    }
        //}

        //[TestMethod]
        //public void Update_ShouldSetTheAmendedDate()
        //{
        //    // Arrange:
        //    var userId = Guid.NewGuid().ToString();
        //    var trackingKey = Guid.NewGuid().ToString();

        //    // Act:
        //    var shoppingListId = Update(userId, trackingKey);

        //    // Assert:
        //    using (var con = TestUtils.GetConnection())
        //    {
        //        var amemdedDate = con.Query<DateTime?>(
        //            @"select AmendedDate from ShoppingLists where Id = @Id",
        //            new { Id = shoppingListId }
        //        ).First();

        //        Assert.IsNotNull(amemdedDate);
        //        Assert.AreNotEqual(default(DateTime), amemdedDate);
        //    }
        //}

        private ShoppingListRepository GetRepository(string userId)
        {
            return new ShoppingListRepository(
                new ShoppingListsDbContext(
                    new MockUserContext(userId)
                )
            );
        }

        private long Create(string userId)
        {
            var shoppingList = new ShoppingList { Title = GetTestTitle() };
            var repository = GetRepository(userId);
            repository.Create(shoppingList);
            uow.Complete();
            return shoppingList.Id;
        }

        //private long Update(string userId, string trackingKey)
        //{
        //    // Create initial record
        //    long shoppingListId = 0;
        //    using (var con = TestUtils.GetConnection())
        //    {
        //        shoppingListId = con.Query<long>(
        //            @"insert into ShoppingLists (Title, CreatorId, CreatedDate)
        //            values (@Title, @CreatorId, @CreatedDate);
        //            SELECT CAST(SCOPE_IDENTITY() as bigint);",
        //            new { Title = GetTestTitle(), CreatorId = userId, CreatedDate = DateTime.Now }
        //        ).First();
        //    }

        //    _container.Register<UserIdProvider>(f => new UserIdProvider(userId));
        //    _repository = _container.GetInstance<IShoppingListRepository>();

        //    // Perform the update
        //    var shoppingList = default(ShoppingList);
        //    using (var uow = _container.GetInstance<IUnitOfWork>())
        //    {
        //        shoppingList = _repository.Get(shoppingListId);
        //        shoppingList.Title = shoppingList.Title + trackingKey;
        //        _repository.Update(shoppingList);
        //        uow.Complete();
        //    }

        //    return shoppingListId;
        //}

        private string GetTestTitle()
        {
            return "Test Shopping List " + Guid.NewGuid().ToString();
        }
    }
}
