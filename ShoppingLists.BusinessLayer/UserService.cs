using System;
using System.Collections.Generic;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;
using ShoppingLists.Shared.RepositoryInterfaces;
using ShoppingLists.Shared.ServiceInterfaces;

namespace ShoppingLists.BusinessLayer
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        private IPermissionService _permissionService;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _permissionService = permissionService;
        }

        public User Get(string id, bool includePermissions = false, long? shoppingListId = null)
        {
            var user = _userRepository.Get(id, includePermissions, shoppingListId);
            return user;
        }

        public void Create(User entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.Discriminator = "ApplicationUser";
            _userRepository.Create(entity);
            _unitOfWork.SaveChanges();
        }

        public void Update(User entity)
        {
            _userRepository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public void Delete(string id)
        {
            _userRepository.Delete(id);
            _unitOfWork.SaveChanges();
        }

        public User GetByName(string userName)
        {
            return _userRepository.FindByName(userName);
        }

        public IEnumerable<User> GetAllForShoppingList(long shoppingListId)
        {
            var users = _userRepository.FindAllForShoppingList(shoppingListId);
            return users;
        }

        public void SetPermissions(string userId, long shoppingListId, IEnumerable<long> permissionIds)
        {
            _permissionService.SetAllForUser(userId, shoppingListId, permissionIds);
            _unitOfWork.SaveChanges();
        }
    }
}
