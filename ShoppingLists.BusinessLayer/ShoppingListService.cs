using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core;
using ShoppingLists.Core.RepositoryInterfaces;
using ShoppingLists.BusinessLayer.Exceptions;

namespace ShoppingLists.BusinessLayer
{
    public class ShoppingListService
    {
        private IUnitOfWork uow;
        private IShoppingListRepository repository;
        private ShoppingListPermissionHelper permissionHelper;
        private Timestamper<ShoppingList> timestamper;
        private IListItemRepository listItemRepository;
        private UserService userService;

        public ShoppingListService(IUnitOfWork uow, IShoppingListRepository repository, ShoppingListPermissionHelper permissionHelper, Timestamper<ShoppingList> timestamper, IListItemRepository listItemRepository, UserService userService)
        {
            this.uow = uow;
            this.repository = repository;
            this.permissionHelper = permissionHelper;
            this.timestamper = timestamper;
            this.listItemRepository = listItemRepository;
            this.userService = userService;
        }

        public ShoppingList Get(long id, string userId, bool includeListItems = false)
        {
            permissionHelper.Check(userId, Permissions.View, id); // Don't allow if this user does not have view access.
            var shoppingList = repository.Get(id, includeListItems);
            if (shoppingList == null)
            {
                throw new EntityNotFoundException(typeof(ShoppingList), id);
            }
            return shoppingList;
        }

        public IEnumerable<ShoppingList> FindAllForUser(string userId)
        {
            return repository.FindAllForUser(userId);
        }

        public ShoppingList Create(string userId)
        {
            var shoppingList = new ShoppingList() { Title = GetNewListTitle(userId) };
            timestamper.Create(shoppingList, userId);
            permissionHelper.Create(userId, Permissions.View, shoppingList.Id);
            permissionHelper.Create(userId, Permissions.Edit, shoppingList.Id);
            permissionHelper.Create(userId, Permissions.Share, shoppingList.Id);
            permissionHelper.Create(userId, Permissions.AddListItems, shoppingList.Id);
            permissionHelper.Create(userId, Permissions.PickOrUnpickListItems, shoppingList.Id);
            permissionHelper.Create(userId, Permissions.Edit, shoppingList.Id);
            permissionHelper.Create(userId, Permissions.RemoveListItems, shoppingList.Id);
            permissionHelper.Create(userId, Permissions.Delete, shoppingList.Id);
            permissionHelper.Create(userId, Permissions.EditListItems, shoppingList.Id);
            return shoppingList;
        }

        private string GetNewListTitle(string userId)
        {
            const string defaultTitle = "Shopping List #";
            int dummy;
            int lastNum = repository.FindByPartialTitleMatch(defaultTitle, userId)
                .Select(sl => sl.Title.Split('#').Last())
                .OrderByDescending(n => n, new NumericStringComparer())
                .Where(n => int.TryParse(n, out dummy))
                .Select(n => int.Parse(n))
                .FirstOrDefault();
            return defaultTitle + ++lastNum;
        }

        public ShoppingList Update(long shoppingListId, string title, string userId)
        {
            permissionHelper.Check(userId, Permissions.Edit, shoppingListId); // Don't allow if this user does not have update access.
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new EmptyStringException("title");
            }
            if (repository.FindByTitle(title, userId) != null)
            {
                throw new ShoppingListTitleDuplicateException(title, userId);
            }
            var shoppingList = repository.Get(shoppingListId, includeCreator: true);
            if (shoppingList == null)
            {
                throw new EntityNotFoundException(typeof(ShoppingList), shoppingListId);
            }
            shoppingList.Title = title;
            timestamper.Update(shoppingList, userId);
            return shoppingList;
        }

        public void Delete(long shoppingListId, string userId)
        {
            permissionHelper.Check(userId, Permissions.Delete, shoppingListId); // Don't allow if this user does not have delete access.
            repository.Delete(shoppingListId);
        }

        public void Ignore(long shoppingListId, string userId)
        {
            permissionHelper.Check(userId, Permissions.View, shoppingListId); // They have to have View permission to ignore the ShoppingList.
            permissionHelper.DeleteAllForUser(shoppingListId, userId);
        }

        public void UnpickAllListItems(long shoppingListId, string userId)
        {
            permissionHelper.Check(userId, Permissions.PickOrUnpickListItems, shoppingListId); // Don't allow if this user does not have delete access.
            listItemRepository.UnpickAllListItems(shoppingListId);
        }

        public void ShareWithUser(long shoppingListId, string userToShareWithId, string userId)
        {
            var shoppingList = repository.Get(shoppingListId);
            if (userToShareWithId == shoppingList.CreatorId)
            {
                throw new ShareWithListCreatorException(userId);
            }
            if (userToShareWithId == userId)
            {
                throw new ShareWithYourselfException(userId);
            }
            permissionHelper.Check(userId, Permissions.Share, shoppingListId); // Don't allow if this user does not have share permissions.
            permissionHelper.CheckAlreadyExists(userToShareWithId, Permissions.View, shoppingListId);
            permissionHelper.Create(userToShareWithId, Permissions.View, shoppingListId);
        }

        public void RemoveSharingUser(long shoppingListId, string sharingUserId, string userId)
        {
            permissionHelper.Check(userId, Permissions.Share, shoppingListId); // Don't allow if this user does not have share permissions.
            permissionHelper.DeleteAllForUser(shoppingListId, sharingUserId);
        }

        public IEnumerable<ShoppingListPermission> GetPermissionsForUser(long shoppingListId, string forUserId, string userId)
        {
            permissionHelper.Check(userId, Permissions.Share, shoppingListId); // Don't allow if this user does not have share permissions.
            return permissionHelper.GetAllForUserAndEntity(forUserId, shoppingListId);
        }
    }
}
