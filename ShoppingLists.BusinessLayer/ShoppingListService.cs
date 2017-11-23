using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Core;
using ShoppingLists.Core.Entities;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.DataAccessLayer;

namespace ShoppingLists.BusinessLayer
{
    public class ShoppingListService
    {
        private IUnitOfWork _unitOfWork;
        private IUserContext _userContext;
        private ShoppingListRepository _shoppingListRepository;
        private ShoppingListPermissionHelper _permissionHelper;
        private ListItemRepository _listItemRepository;
        private UserService _userService;

        public ShoppingListService(IUnitOfWork unitOfWork, IUserContext userContext, ShoppingListRepository shoppingListRepository, ShoppingListPermissionHelper permissionHelper, ListItemRepository listItemRepository, UserService userService)
        {
            _unitOfWork = unitOfWork;
            _shoppingListRepository = shoppingListRepository;
            _userContext = userContext;
            _permissionHelper = permissionHelper;
            _listItemRepository = listItemRepository;
            _userService = userService;
        }

        public ShoppingList Get(long id, bool includeListItems = false)
        {
            _permissionHelper.Check(_userContext.UserId, Permissions.View, id); // Don't allow if this user does not have view access.
            var shoppingList = _shoppingListRepository.Get(id, includeListItems);
            if (shoppingList == null)
            {
                throw new EntityNotFoundException(typeof(ShoppingList), id);
            }
            return shoppingList;
        }

        public IEnumerable<ShoppingList> FindAllForCurrentUser()
        {
            return _shoppingListRepository.FindAllForUser(_userContext.UserId);
        }

        public ShoppingList Create()
        {
            var shoppingList = new ShoppingList() { Title = GetNewListTitle() };
            _shoppingListRepository.Create(shoppingList);
            _unitOfWork.SaveChanges(); // Populates shoppingList.Id
            _permissionHelper.Create(_userContext.UserId, Permissions.View, shoppingList.Id);
            _permissionHelper.Create(_userContext.UserId, Permissions.Edit, shoppingList.Id);
            _permissionHelper.Create(_userContext.UserId, Permissions.Share, shoppingList.Id);
            _permissionHelper.Create(_userContext.UserId, Permissions.AddListItems, shoppingList.Id);
            _permissionHelper.Create(_userContext.UserId, Permissions.PickOrUnpickListItems, shoppingList.Id);
            _permissionHelper.Create(_userContext.UserId, Permissions.Edit, shoppingList.Id);
            _permissionHelper.Create(_userContext.UserId, Permissions.RemoveListItems, shoppingList.Id);
            _permissionHelper.Create(_userContext.UserId, Permissions.Delete, shoppingList.Id);
            _permissionHelper.Create(_userContext.UserId, Permissions.EditListItems, shoppingList.Id);
            _unitOfWork.SaveChanges();
            return shoppingList;
        }

        private string GetNewListTitle()
        {
            const string defaultTitle = "Shopping List #";
            int dummy;
            int lastNum = _shoppingListRepository.FindAllForUser(_userContext.UserId)
                .Where(sl => sl.Title.StartsWith(defaultTitle))
                .Select(sl => sl.Title.Split('#').Last())
                .OrderByDescending(n => n, new NumericStringComparer())
                .Where(n => int.TryParse(n, out dummy))
                .Select(n => int.Parse(n))
                .FirstOrDefault();
            return defaultTitle + ++lastNum;
        }

        public ShoppingList Update(long shoppingListId, string title)
        {
            _permissionHelper.Check(_userContext.UserId, Permissions.Edit, shoppingListId); // Don't allow if this user does not have update access.
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new EmptyStringException("title");
            }
            if (_shoppingListRepository.FindByTitle(title, _userContext.UserId) != null)
            {
                throw new ShoppingListTitleDuplicateException(title, _userContext.UserId);
            }
            var shoppingList = _shoppingListRepository.Get(shoppingListId, includeCreator: true);
            if (shoppingList == null)
            {
                throw new EntityNotFoundException(typeof(ShoppingList), shoppingListId);
            }
            shoppingList.Title = title;
            _shoppingListRepository.Update(shoppingList);
            _unitOfWork.SaveChanges();
            return shoppingList;
        }

        public void Delete(long shoppingListId)
        {
            _permissionHelper.Check(_userContext.UserId, Permissions.Delete, shoppingListId); // Don't allow if this user does not have delete access.
            _shoppingListRepository.Delete(shoppingListId);
            _unitOfWork.SaveChanges();
        }

        public void Ignore(long shoppingListId)
        {
            _permissionHelper.Check(_userContext.UserId, Permissions.View, shoppingListId); // They have to have View permission to ignore the ShoppingList.
            _permissionHelper.DeleteAllForUser(shoppingListId, _userContext.UserId);
            _unitOfWork.SaveChanges();
        }

        public void UnpickAllListItems(long shoppingListId)
        {
            _permissionHelper.Check(_userContext.UserId, Permissions.PickOrUnpickListItems, shoppingListId); // Don't allow if this user does not have delete access.
            _listItemRepository.UnpickAllListItems(shoppingListId);
            _unitOfWork.SaveChanges();
        }

        public void ShareWithUser(long shoppingListId, string userToShareWithId)
        {
            var shoppingList = _shoppingListRepository.Get(shoppingListId);
            if (userToShareWithId == shoppingList.CreatorId)
            {
                throw new ShareWithListCreatorException(_userContext.UserId);
            }
            if (userToShareWithId == _userContext.UserId)
            {
                throw new ShareWithYourselfException(_userContext.UserId);
            }
            _permissionHelper.Check(_userContext.UserId, Permissions.Share, shoppingListId); // Don't allow if this user does not have share permissions.
            _permissionHelper.CheckAlreadyExists(userToShareWithId, Permissions.View, shoppingListId);
            _permissionHelper.Create(userToShareWithId, Permissions.View, shoppingListId);
            _unitOfWork.SaveChanges();
        }

        public void RemoveSharingUser(long shoppingListId, string sharingUserId)
        {
            _permissionHelper.Check(_userContext.UserId, Permissions.Share, shoppingListId); // Don't allow if this user does not have share permissions.
            _permissionHelper.DeleteAllForUser(shoppingListId, sharingUserId);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<ShoppingListPermission> GetPermissionsForUser(long shoppingListId, string forUserId)
        {
            _permissionHelper.Check(_userContext.UserId, Permissions.Share, shoppingListId); // Don't allow if this user does not have share permissions.
            return _permissionHelper.GetAllForUserAndEntity(forUserId, shoppingListId);
        }
    }
}
