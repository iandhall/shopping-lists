using ShoppingLists.Shared.Entities;
using ShoppingLists.Shared;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.Shared.RepositoryInterfaces;
using ShoppingLists.Shared.ServiceInterfaces;

namespace ShoppingLists.BusinessLayer
{
    public class ListItemService : IListItemService
    {
        private IUnitOfWork _unitOfWork;
        private IUserContext _userContext;
        private IListItemRepository _listItemRepository;
        private IPermissionService _permissionService;

        public ListItemService(IUnitOfWork unitOfWork, IUserContext userContext, IListItemRepository repository, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _listItemRepository = repository;
            _permissionService = permissionService;
        }

        public ListItem Create(string description, int quantity, long shoppingListId)
        {
            _permissionService.Check(_userContext.UserId, Permissions.AddListItems, shoppingListId); // Don't allow if user does not have permission to add ListItems.
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new EmptyStringException("description");
            }
            if (quantity == 0)
            {
                throw new OutOfRangeException("quantity", quantity.GetType());
            }
            if (_listItemRepository.FindByDescription(description, shoppingListId) != null)
            {
                throw new ListItemAlreadyExistsException(description, shoppingListId); // Don't allow if a ListItem with the same description already exists.
            }
            var listItem = new ListItem
            {
                ShoppingListId = shoppingListId,
                Description = description,
                Quantity = quantity,
                StatusId = Statuses.NotPicked
            };
            _listItemRepository.Create(listItem);
            _unitOfWork.SaveChanges();
            return listItem;
        }

        public ListItem Pick(long listItemId, long shoppingListId)
        {
            _permissionService.Check(_userContext.UserId, Permissions.PickOrUnpickListItems, shoppingListId); // Don't allow if user does not have permission to pick/unpick ListItems.
            var listItem = _listItemRepository.Get(listItemId);
            if (listItem == null)
            {
                throw new EntityNotFoundException(typeof(ListItem), listItemId);
            }
            if (listItem.ShoppingListId != shoppingListId)
            {
                throw new NotRelatedException(typeof(ShoppingList), shoppingListId, typeof(ListItem), listItemId); // Don't allow if the ListItem does not belong to the ShoppingList.
            }
            if (listItem.StatusId != Statuses.Picked)
            {
                // Don't bother with the update if the Status is already "Picked".
                listItem.StatusId = Statuses.Picked;
                _listItemRepository.Update(listItem);
                _unitOfWork.SaveChanges();
            }
            return listItem;
        }

        public ListItem Unpick(long listItemId, long shoppingListId)
        {
            _permissionService.Check(_userContext.UserId, Permissions.PickOrUnpickListItems, shoppingListId); // Don't allow if user does not have permission to pick/unpick ListItems.
            var listItem = _listItemRepository.Get(listItemId);
            if (listItem.ShoppingListId != shoppingListId)
            {
                throw new NotRelatedException(typeof(ShoppingList), shoppingListId, typeof(ListItem), listItemId); // Don't allow if the ListItem does not belong to the ShoppingList.
            }
            if (listItem.StatusId != Statuses.NotPicked)
            { 
                // Don't bother with the update if the Status is already "Not picked".
                listItem.StatusId = Statuses.NotPicked;
                _listItemRepository.Update(listItem);
                _unitOfWork.SaveChanges();
            }
            return listItem;
        }

        public ListItem Update(string description, int quantity, long listItemId, long shoppingListId)
        {
            _permissionService.Check(_userContext.UserId, Permissions.EditListItems, shoppingListId); // Don't allow if user does not have permission to change ListItems descriptions.
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new EmptyStringException("description");
            }
            if (quantity == 0)
            {
                throw new OutOfRangeException("quantity", quantity.GetType());
            }
            var listItem = _listItemRepository.Get(listItemId);
            if (listItem == null)
            {
                throw new EntityNotFoundException(typeof(ListItem), listItemId);
            }
            if (listItem.ShoppingListId != shoppingListId)
            {
                throw new NotRelatedException(typeof(ShoppingList), shoppingListId, typeof(ListItem), listItemId); // Don't allow if the ListItem does not belong to the ShoppingList.
            }
            var existingListItem = _listItemRepository.FindByDescription(description, shoppingListId);
            if (existingListItem != null && existingListItem.Id != listItemId)
            {
                throw new ListItemAlreadyExistsException(description, shoppingListId); // Don't allow if a ListItem with the same description already exists.
            }
            listItem.Description = description;
            listItem.Quantity = quantity;
            _listItemRepository.Update(listItem);
            _unitOfWork.SaveChanges();
            return listItem;
        }

        public void Delete(long listItemId, long shoppingListId)
        {
            _permissionService.Check(_userContext.UserId, Permissions.RemoveListItems, shoppingListId); // Don't allow if user does not have permission to remove ListItems.
            var listItem = _listItemRepository.Get(listItemId);
            if (listItem == null)
            {
                throw new EntityNotFoundException(typeof(ListItem), listItemId);
            }
            if (listItem.ShoppingListId != shoppingListId)
            {
                throw new NotRelatedException(typeof(ShoppingList), shoppingListId, typeof(ListItem), listItemId); // Don't allow if the ListItem does not belong to the ShoppingList.
            }
            _listItemRepository.Delete(listItemId);
            _unitOfWork.SaveChanges();
        }
    }
}
