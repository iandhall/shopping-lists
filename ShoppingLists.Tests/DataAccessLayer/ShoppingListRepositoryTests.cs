using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Tests.DataAccessLayer
{
    [TestClass]
    public class ShoppingListRepositoryTests
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
        public void ShouldRetriveAShoppingListAndPopulateListItemsWhenIncludeListItemsParameterIsTrue()
        {
            var testDataKeys = CreateTestData();

            using (var dbContext = TestUtils.CreateDbContext(_connection, new MockUserContext(testDataKeys.UserId)))
            using (IUnitOfWork unitOfWork = new EfUnitOfWork(dbContext))
            {
                var shoppingListRepository = new ShoppingListRepository(dbContext);
                var shoppingList = shoppingListRepository.Get(testDataKeys.ShoppingListId, includeListItems: true);
                Assert.AreEqual(2, shoppingList.ListItems.Count);
            }
        }

        [TestMethod]
        public void ShouldRetriveAShoppingListWithoutPopulatingListItemsWhenIncludeListItemsParameterIsNotSpecified()
        {
            //Assert.IsNull(shoppingList.ListItems);
            //Effort always applies DbSet.Includes
        }

        [TestMethod]
        public void ShouldRetriveAShoppingListAndPopulateCreatorWhenIncludeCreatorParameterIsTrue()
        {
            //Assert.IsNotNull(shoppingList.Creator);
            //Effort always applies DbSet.Includes
        }

        [TestMethod]
        public void ShouldRetriveAShoppingListWithoutPopulatingCreatorWhenIncludeCreatorParameterIsNotSpecified()
        {
            //Assert.IsNull(shoppingList.Creator);
            //Effort always applies DbSet.Includes
        }

        [TestMethod]
        public void ShouldFindAllShoppingListsForTheGivenCreator()
        {
            var testDataKeys = CreateTestData();

            using (var dbContext = TestUtils.CreateDbContext(_connection))
            using (IUnitOfWork unitOfWork = new EfUnitOfWork(dbContext))
            {
                var shoppingListRepository = new ShoppingListRepository(dbContext);
                var shoppingLists = shoppingListRepository.FindAllForUser(testDataKeys.UserId).ToList();
                Assert.AreEqual(testDataKeys.UserId, shoppingLists[0].CreatorId);
                Assert.AreEqual(testDataKeys.UserId, shoppingLists[1].CreatorId);
            }
        }

        [TestMethod]
        public void ShouldFindAShoppingListForTheGivenTitleAndCreator()
        {
            var testDataKeys = CreateTestData();

            using (var dbContext = TestUtils.CreateDbContext(_connection, new MockUserContext(testDataKeys.UserId)))
            using (IUnitOfWork unitOfWork = new EfUnitOfWork(dbContext))
            {
                var shoppingListRepository = new ShoppingListRepository(dbContext);
                var shoppingList = shoppingListRepository.FindByTitle("Shopping list to find", testDataKeys.UserId);
                Assert.AreEqual(testDataKeys.Marker, shoppingList.Title);
                Assert.AreEqual(testDataKeys.UserId, shoppingList.CreatorId.ToString());
            }
        }

        private TestDataKeys CreateTestData()
        {
            var testDataKeys = new TestDataKeys();

            testDataKeys.Marker = "Shopping list to find";
            var userId = TestUtils.NewId();
            using (var dbContext = TestUtils.CreateDbContext(_connection, new MockUserContext(userId)))
            {
                dbContext.ShoppingLists.AddRange(new List<ShoppingList>
                {
                    new ShoppingList { Title = TestUtils.GetTestShoppingListTitle() },
                    new ShoppingList { Title = TestUtils.GetTestShoppingListTitle() },
                    new ShoppingList { Title = TestUtils.GetTestShoppingListTitle() },
                    new ShoppingList { Title = TestUtils.GetTestShoppingListTitle() }
                });
                dbContext.SaveChanges();
            }

            testDataKeys.UserId = TestUtils.NewId();
            using (var dbContext = TestUtils.CreateDbContext(_connection, new MockUserContext(testDataKeys.UserId)))
            {
                var shoppingList = new ShoppingList { Title = testDataKeys.Marker };
                dbContext.ShoppingLists.Add(shoppingList);
                dbContext.ShoppingLists.Add(new ShoppingList { Title = TestUtils.GetTestShoppingListTitle() });
                dbContext.SaveChanges();
                testDataKeys.ShoppingListId = shoppingList.Id;

                dbContext.ListItems.Add(new ListItem { ShoppingListId = testDataKeys.ShoppingListId });
                dbContext.ListItems.Add(new ListItem { ShoppingListId = testDataKeys.ShoppingListId });
                dbContext.SaveChanges();
            }

            userId = TestUtils.NewId();
            using (var dbContext = TestUtils.CreateDbContext(_connection, new MockUserContext(userId)))
            {
                dbContext.ShoppingLists.AddRange(new List<ShoppingList>
                {
                    new ShoppingList { Title = TestUtils.GetTestShoppingListTitle() },
                    new ShoppingList { Title = TestUtils.GetTestShoppingListTitle() },
                    new ShoppingList { Title = TestUtils.GetTestShoppingListTitle() },
                });
                dbContext.SaveChanges();
            }

            return testDataKeys;
        }

        public class TestDataKeys
        {
            public string UserId { get; set; }
            public long ShoppingListId { get; set; }
            public string Marker { get; set; }
        }
    }
}