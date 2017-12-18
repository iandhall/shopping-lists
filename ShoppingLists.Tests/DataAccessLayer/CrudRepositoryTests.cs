using System;
using System.Linq;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Tests.DataAccessLayer
{
    [TestClass]
    public class CrudRepositoryTests
    {
        [TestMethod]
        public void Create_ShouldSetTheCreatorId()
        {
            var userId = Guid.NewGuid().ToString();
            var newShoppingListId = Create(userId);

            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(
                    userId,
                    con.Query<string>(
                        "select CreatorId from ShoppingLists where Id = @Id",
                        new { Id = newShoppingListId })
                        .Single(),
                    true);
            }
        }

        [TestMethod]
        public void Create_ShouldSetTheCreatedDate()
        {
            var newShoppingListId = Create(Guid.NewGuid().ToString());

            using (var con = TestUtils.GetConnection())
            {
                Assert.AreNotEqual(
                    default(DateTime),
                    con.Query<DateTime>(
                        "select CreatedDate from ShoppingLists where Id = @id",
                        new { id = newShoppingListId })
                        .Single());
            }
        }

        [TestMethod]
        public void Update_ShouldSetTheAmenderId()
        {
            var userId = Guid.NewGuid().ToString();
            var newShoppingListId = Update(userId);

            using (var con = TestUtils.GetConnection())
            {
                Assert.AreEqual(
                    userId,
                    con.Query<string>(
                        "select AmenderId from ShoppingLists where Id = @Id",
                        new { Id = newShoppingListId })
                        .Single(),
                    true);
            }
        }

        [TestMethod]
        public void Update_ShouldSetTheAmendedDate()
        {
            var newShoppingListId = Update(Guid.NewGuid().ToString());

            using (var con = TestUtils.GetConnection())
            {
                Assert.AreNotEqual(
                    default(DateTime),
                    con.Query<DateTime>(
                        "select AmendedDate from ShoppingLists where Id = @id",
                        new { id = newShoppingListId })
                        .Single());
            }
        }

        private long Create(string userId)
        {
            var dbContext = new ShoppingListsDbContext(new MockUserContext(userId));
            var repository = new ShoppingListRepository(dbContext);

            var shoppingList = new ShoppingList { Title = TestUtils.GetTestTitle() };
            repository.Create(shoppingList);
            dbContext.SaveChanges();

            return shoppingList.Id;
        }

        private long Update(string userId)
        {
            // Create initial record
            long shoppingListId = 0;
            using (var con = TestUtils.GetConnection())
            {
                shoppingListId = con.Query<long>(
                    @"insert into ShoppingLists (Title, CreatorId, CreatedDate)
                    values (@Title, @CreatorId, @CreatedDate);
                    SELECT CAST(SCOPE_IDENTITY() as bigint);",
                    new { Title = TestUtils.GetTestTitle(), CreatorId = Guid.NewGuid().ToString(), CreatedDate = DateTime.Now }
                ).Single();
            }

            var dbContext = new ShoppingListsDbContext(new MockUserContext(userId));
            var repository = new ShoppingListRepository(dbContext);

            // Perform the update
            var shoppingList = default(ShoppingList);
            shoppingList = repository.Get(shoppingListId);
            shoppingList.Title = shoppingList.Title;
            repository.Update(shoppingList);
            dbContext.SaveChanges();

            return shoppingListId;
        }
    }
}
