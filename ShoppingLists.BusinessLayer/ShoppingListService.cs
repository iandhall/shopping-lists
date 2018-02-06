using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.Shared.RepositoryInterfaces;
using ShoppingLists.Shared.ServiceInterfaces;

namespace ShoppingLists.BusinessLayer
{
    public class ShoppingListService : IShoppingListService
    {
        private IUnitOfWork _unitOfWork;
        private IUserContext _userContext;
        private IShoppingListRepository _shoppingListRepository;
        private IPermissionService _permissionService;
        private IListItemRepository _listItemRepository;
        private IUserService _userService;

        public ShoppingListService(IUnitOfWork unitOfWork, IUserContext userContext, IShoppingListRepository shoppingListRepository, IPermissionService permissionService, IListItemRepository listItemRepository, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _shoppingListRepository = shoppingListRepository;
            _userContext = userContext;
            _permissionService = permissionService;
            _listItemRepository = listItemRepository;
            _userService = userService;
        }

        public ShoppingList Get(long id, bool includeListItems = false)
        {
            var shoppingList = _shoppingListRepository.Get(id, includeListItems);
            if (shoppingList == null)
            {
                throw new EntityNotFoundException(typeof(ShoppingList), id);
            }
            _permissionService.Check(_userContext.UserId, Permissions.View, id); // Don't allow if this user does not have view access.
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
            _permissionService.Create(_userContext.UserId, Permissions.View, shoppingList.Id);
            _permissionService.Create(_userContext.UserId, Permissions.Edit, shoppingList.Id);
            _permissionService.Create(_userContext.UserId, Permissions.Share, shoppingList.Id);
            _permissionService.Create(_userContext.UserId, Permissions.AddListItems, shoppingList.Id);
            _permissionService.Create(_userContext.UserId, Permissions.PickOrUnpickListItems, shoppingList.Id);
            _permissionService.Create(_userContext.UserId, Permissions.Edit, shoppingList.Id);
            _permissionService.Create(_userContext.UserId, Permissions.RemoveListItems, shoppingList.Id);
            _permissionService.Create(_userContext.UserId, Permissions.Delete, shoppingList.Id);
            _permissionService.Create(_userContext.UserId, Permissions.EditListItems, shoppingList.Id);
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
                .OrderByDescending(n => StringComparisonTools.PadNumbers(n))
                .Where(n => int.TryParse(n, out dummy))
                .Select(n => int.Parse(n))
                .FirstOrDefault();
            return defaultTitle + ++lastNum;
        }

        public ShoppingList Update(long shoppingListId, string title)
        {
            _permissionService.Check(_userContext.UserId, Permissions.Edit, shoppingListId); // Don't allow if this user does not have update access.
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
            _permissionService.Check(_userContext.UserId, Permissions.Delete, shoppingListId); // Don't allow if this user does not have delete access.
            _shoppingListRepository.Delete(shoppingListId);
            _unitOfWork.SaveChanges();
        }

        public void Ignore(long shoppingListId)
        {
            _permissionService.Check(_userContext.UserId, Permissions.View, shoppingListId); // They have to have View permission to ignore the ShoppingList.
            _permissionService.DeleteAllForUser(shoppingListId, _userContext.UserId);
            _unitOfWork.SaveChanges();
        }

        public void UnpickAllListItems(long shoppingListId)
        {
            _permissionService.Check(_userContext.UserId, Permissions.PickOrUnpickListItems, shoppingListId); // Don't allow if this user does not have delete access.
            _listItemRepository.UnpickAllListItems(shoppingListId);
            _unitOfWork.SaveChanges();
        }

        public void ShareWithUser(long shoppingListId, string userToShareWithId)
        {
            var shoppingList = _shoppingListRepository.Get(shoppingListId);
            if (userToShareWithId == _userContext.UserId)
            {
                throw new ShareWithYourselfException(_userContext.UserId);
            }
            if (userToShareWithId == shoppingList.CreatorId)
            {
                throw new ShareWithListCreatorException(_userContext.UserId);
            }
            
            _permissionService.Check(_userContext.UserId, Permissions.Share, shoppingListId); // Don't allow if this user does not have share permissions.
            _permissionService.CheckAlreadyExists(userToShareWithId, Permissions.View, shoppingListId);
            _permissionService.Create(userToShareWithId, Permissions.View, shoppingListId);
            _unitOfWork.SaveChanges();
        }

        public void RemoveSharingUser(long shoppingListId, string sharingUserId)
        {
            _permissionService.Check(_userContext.UserId, Permissions.Share, shoppingListId); // Don't allow if this user does not have share permissions.
            _permissionService.DeleteAllForUser(shoppingListId, sharingUserId);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<Permission> GetPermissionsForUser(long shoppingListId, string forUserId)
        {
            _permissionService.Check(_userContext.UserId, Permissions.Share, shoppingListId); // Don't allow if this user does not have share permissions.
            return _permissionService.GetAllForUserAndEntity(forUserId, shoppingListId);
        }
    }
}
