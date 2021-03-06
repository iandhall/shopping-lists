﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingLists.BusinessLayer;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;
using ShoppingLists.Shared.RepositoryInterfaces;
using ShoppingLists.Shared.ServiceInterfaces;

namespace ShoppingLists.Tests.BusinessLayer
{
    [TestClass]
    public class ShoppingListServiceTests
    {
        [TestMethod, ExpectedException(typeof(EntityNotFoundException))]
        public void Get_ShouldThrowEntityNotFoundException_IfTheGivenEntityDoesntExist()
        {
            var userContext = new MockUserContext(NewId());
            var shoppingListRepositoryMock = new Mock<IShoppingListRepository>();
            shoppingListRepositoryMock.Setup(x => x.Get(9999, false, false))
                .Returns(() => null);
            var shoppingListService = CreateShoppingListService(userContext, shoppingListRepositoryMock.Object);
            
            var shoppingList = shoppingListService.Get(9999);
        }

        [TestMethod]
        public void Create_ShouldSetTheTitleToTheNextAvailableDefaultTitle_WhenCalled()
        {
            var userContext = new MockUserContext(NewId());
            var shoppingListRepositoryMock = new Mock<IShoppingListRepository>();
            shoppingListRepositoryMock.Setup(x => x.FindAllForUser(userContext.UserId))
                .Returns(() => new List<ShoppingList>
            {
                new ShoppingList { Title = "Shopping List #1" },
                // "Shopping List #2 is missing from list
                new ShoppingList { Title = "Shopping List #3" },
                new ShoppingList { Title = "Shopping List #5" },
                new ShoppingList { Title = "Shopping List #6" },
                new ShoppingList { Title = "Shopping List #10" },
                new ShoppingList { Title = "Shopping List #20" }
            });
            var shoppingListService = CreateShoppingListService(userContext, shoppingListRepositoryMock.Object);

            var shoppingList = shoppingListService.Create();
            Assert.AreEqual("Shopping List #21", shoppingList.Title);
        }

        [TestMethod, ExpectedException(typeof(EmptyStringException))]
        public void Update_ShouldFail_IfTitleIsNull()
        {
            var userContext = new MockUserContext(NewId());
            var shoppingListRepositoryMock = new Mock<IShoppingListRepository>();
            var shoppingListService = CreateShoppingListService(userContext, shoppingListRepositoryMock.Object);

            shoppingListService.Update(9999, null);
        }

        [TestMethod, ExpectedException(typeof(ShoppingListTitleDuplicateException))]
        public void Update_ShouldFail_IfTitleIsADuplicate()
        {
            var title = "Existing Shopping List";
            var userContext = new MockUserContext(NewId());
            var shoppingListRepositoryMock = new Mock<IShoppingListRepository>();
            shoppingListRepositoryMock.Setup(x => x.FindByTitle(title, userContext.UserId))
                .Returns(() => new ShoppingList {});
            var shoppingListService = CreateShoppingListService(userContext, shoppingListRepositoryMock.Object);

            shoppingListService.Update(9999, title);
        }
        
        [TestMethod, ExpectedException(typeof(EntityNotFoundException))]
        public void Update_ShouldFail_WhenShoppingListNotFound()
        {
            var userContext = new MockUserContext(NewId());
            var shoppingListRepositoryMock = new Mock<IShoppingListRepository>();
            shoppingListRepositoryMock.Setup(x => x.Get(9999, false, false))
                .Returns(() => null);
            var shoppingListService = CreateShoppingListService(userContext, shoppingListRepositoryMock.Object);

            shoppingListService.Update(9999, "Changed title");
        }

        private ShoppingListService CreateShoppingListService(IUserContext userContext, IShoppingListRepository shoppingListRepository)
        {
            return new ShoppingListService(
                Mock.Of<IUnitOfWork>(),
                userContext,
                shoppingListRepository,
                Mock.Of<IPermissionService>(),
                Mock.Of<IListItemRepository>(),
                Mock.Of<IUserService>());
        }

        private string NewId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
