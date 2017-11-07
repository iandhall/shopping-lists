using System;
using System.Collections.Generic;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core;
using ShoppingLists.Core.RepositoryInterfaces;

namespace ShoppingLists.BusinessLayer
{
    public class UserService
    {
        private IUnitOfWork uow;
        private IUserRepository repository;
        private ShoppingListPermissionHelper permissionHelper;

        public UserService(IUnitOfWork uow, IUserRepository repository, ShoppingListPermissionHelper permissionHelper)
        {
            this.uow = uow;
            this.repository = repository;
            this.permissionHelper = permissionHelper;
        }

        public User Get(string id, bool includePermissions = false, long? shoppingListId = null)
        {
            var user = repository.Get(id, includePermissions, shoppingListId);
            return user;
        }

        public void Create(User entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.Discriminator = "ApplicationUser";
            repository.Create(entity);
        }

        public void Update(User entity)
        {
            repository.Update(entity);
        }

        public void Delete(string id)
        {
            repository.Delete(id);
        }

        public User GetByName(string userName)
        {
            return repository.FindByName(userName);
        }

        public IEnumerable<User> GetAllForShoppingList(long shoppingListId)
        {
            var users = repository.FindAllForShoppingList(shoppingListId);
            return users;
        }

        public void SetPermissions(string userId, long shoppingListId, IEnumerable<long> permissionIds)
        {
            permissionHelper.SetAllForUser(userId, shoppingListId, permissionIds);
        }
    }
}
