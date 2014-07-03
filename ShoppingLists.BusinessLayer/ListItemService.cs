using System;
using System.Data;
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
    public class ListItemService
    {
        private IUnitOfWork uow;
        private IListItemRepository repository;
        private ShoppingListPermissionHelper permissionsHelper;
        private Timestamper<ListItem> timestamper;

        public ListItemService(IUnitOfWork uow, IListItemRepository repository, ShoppingListPermissionHelper permissionsHelper, Timestamper<ListItem> timestamper)
        {
            this.uow = uow;
            this.repository = repository;
            this.permissionsHelper = permissionsHelper;
            this.timestamper = timestamper;
        }

        public ListItem Create(string description, int quantity, long shoppingListId, string userId)
        {
            permissionsHelper.Check(userId, Permissions.AddListItems, shoppingListId); // Don't allow if user does not have permission to add ListItems.
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new EmptyStringException("description");
            }
            if (quantity == 0)
            {
                throw new OutOfRangeException("quantity", quantity.GetType());
            }
            if (repository.GetByDescription(description, shoppingListId) != null)
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
            timestamper.Create(listItem, userId);
            return listItem;
        }

        public ListItem Pick(long listItemId, long shoppingListId, string userId)
        {
            permissionsHelper.Check(userId, Permissions.PickOrUnpickListItems, shoppingListId); // Don't allow if user does not have permission to pick/unpick ListItems.
            var listItem = repository.Get(listItemId);
            if (listItem == null)
            {
                throw new EntityNotFoundException(typeof(ListItem), listItemId);
            }
            if (listItem.ShoppingListId != shoppingListId)
            {
                throw new NotRelatedException(typeof(ShoppingList), shoppingListId, typeof(ListItem), listItemId); // Don't allow if the ListItem does not belong to the ShoppingList.
            }
            if (listItem.StatusId != Statuses.Picked)
            { // Don't bother with the update if the Status is already "Picked".
                listItem.StatusId = Statuses.Picked;
                timestamper.Update(listItem, userId);
            }
            return listItem;
        }

        public ListItem Unpick(long listItemId, long shoppingListId, string userId)
        {
            permissionsHelper.Check(userId, Permissions.PickOrUnpickListItems, shoppingListId); // Don't allow if user does not have permission to pick/unpick ListItems.
            var listItem = repository.Get(listItemId);
            if (listItem.ShoppingListId != shoppingListId)
            {
                throw new NotRelatedException(typeof(ShoppingList), shoppingListId, typeof(ListItem), listItemId); // Don't allow if the ListItem does not belong to the ShoppingList.
            }
            if (listItem.StatusId != Statuses.NotPicked)
            { // Don't bother with the update if the Status is already "Not picked".
                listItem.StatusId = Statuses.NotPicked;
                timestamper.Update(listItem, userId);
            }
            return listItem;
        }

        public ListItem Update(string description, int quantity, long listItemId, long shoppingListId, string userId)
        {
            permissionsHelper.Check(userId, Permissions.EditListItems, shoppingListId); // Don't allow if user does not have permission to change ListItems descriptions.
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new EmptyStringException("description");
            }
            if (quantity == 0)
            {
                throw new OutOfRangeException("quantity", quantity.GetType());
            }
            var listItem = repository.Get(listItemId);
            if (listItem == null)
            {
                throw new EntityNotFoundException(typeof(ListItem), listItemId);
            }
            if (listItem.ShoppingListId != shoppingListId)
            {
                throw new NotRelatedException(typeof(ShoppingList), shoppingListId, typeof(ListItem), listItemId); // Don't allow if the ListItem does not belong to the ShoppingList.
            }
            var existingListItem = repository.GetByDescription(description, shoppingListId);
            if (existingListItem != null && existingListItem.Id != listItemId)
            {
                throw new ListItemAlreadyExistsException(description, shoppingListId); // Don't allow if a ListItem with the same description already exists.
            }
            listItem.Description = description;
            listItem.Quantity = quantity;
            timestamper.Update(listItem, userId);
            return listItem;
        }

        public void Delete(long listItemId, long shoppingListId, string userId)
        {
            permissionsHelper.Check(userId, Permissions.RemoveListItems, shoppingListId); // Don't allow if user does not have permission to remove ListItems.
            var listItem = repository.Get(listItemId);
            if (listItem == null)
            {
                throw new EntityNotFoundException(typeof(ListItem), listItemId);
            }
            if (listItem.ShoppingListId != shoppingListId)
            {
                throw new NotRelatedException(typeof(ShoppingList), shoppingListId, typeof(ListItem), listItemId); // Don't allow if the ListItem does not belong to the ShoppingList.
            }
            repository.Delete(listItemId);
        }
    }
}
