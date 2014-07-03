using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using ShoppingLists.Core.Entities;
using ShoppingLists.BusinessLayer;
using ShoppingLists.Core;
using System.Threading.Tasks;
using ShoppingLists.Web.Models;
using LogForMe;
using LightInject;
using Microsoft.AspNet.Identity;
using ShoppingLists.BusinessLayer.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace ShoppingLists.Web.Hubs
{
    public class ShoppingListHub : Hub, IHasUnitOfWork, IHasScope
    {
        private IUnitOfWork uow;
        private ShoppingListService shoppingListService;
        private ListItemService listItemService;
        private UserService userService;

        // Allows services to be injected via the UnitOfWorkPipelineModule:
        public ShoppingListService ShoppingListService { set { shoppingListService = value; } }
        public ListItemService ListItemService { set { listItemService = value; } }
        public UserService UserService { set { userService = value; } }
        public IUnitOfWork Uow { get { return uow; } set { uow = value; } }
        public Scope Scope { get; set; } // Needed until LightInject gets support for SignalR.

        public ShoppingListHub()
        {
        }

        private string userId
        {
            get
            {
                try
                {
                    return Context.User.Identity.GetUserId();
                }
                catch (Exception ex)
                { // Not logged by ErrorLoggingPipelineModule.
                    Logger.Error(ex);
                    throw;
                }
            }
        }

        public override Task OnConnected()
        {
            try
            {
                Logger.Debug("ConnectionId={0}", Context.ConnectionId);
                return base.OnConnected();
            }
            catch (Exception ex)
            { // Not logged by ErrorLoggingPipelineModule.
                Logger.Error(ex);
                throw;
            }
        }

        public override Task OnReconnected()
        {
            try
            {
                Logger.Debug("ConnectionId={0}", Context.ConnectionId);
                return base.OnReconnected();
            }
            catch (Exception ex)
            { // Not logged by ErrorLoggingPipelineModule.
                Logger.Error(ex);
                throw;
            }
        }

        public override Task OnDisconnected()
        {
            try
            {
                Logger.Debug("ConnectionId={0}", Context.ConnectionId);
                var connection = ConnectionHolder.Get(Context.ConnectionId);
                if (connection == null)
                {
                    return base.OnDisconnected();
                }
                if (connection.UserId != userId)
                {
                    throw new UnexpectedUserIdException(userId, connection.UserId);
                }
                Clients.Group(connection.Group).viewerRemoved(connection.Username);
                Groups.Remove(connection.Id, connection.Group);
                ConnectionHolder.Remove(Context.ConnectionId);
                return base.OnDisconnected();
            }
            catch (Exception ex)
            { // Not logged by ErrorLoggingPipelineModule.
                Logger.Error(ex);
                throw;
            }
        }

        // Exceptions in Hub methods are logged by ErrorLoggingPipelineModule:

        [UnitOfWork]
        public void StartViewingList(long shoppingListId)
        {
            Logger.Debug("shoppingListId={0}, ConnectionId={1}", shoppingListId, Context.ConnectionId);
            string shoppingListIdString = shoppingListId.ToString();
            var user = userService.Get(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            var shoppingList = shoppingListService.Get(shoppingListId, userId); // Attempt to load the ShoppingList to validate that the current user has the appropriate permissions.
            ConnectionHolder.AddOrUpdate(Context.ConnectionId, shoppingListIdString, userId, user.Username); // Add a HubConnection for the current user.
            Groups.Add(Context.ConnectionId, shoppingListIdString); // Add the current user to the group for the ShoppingList.
            Clients.Group(shoppingListIdString).viewerAdded(user.Username); // Inform all clients currently viewing this ShoppingList of the new user that is viewing it.
            Clients.Caller.allCurrentViewersReceived(ConnectionHolder.GetAllUsernamesInGroup(shoppingListIdString)); // Send list of all users viewing this ShoppingList to the caller.
        }

        [UnitOfWork]
        public void Update(ShoppingListOverviewModel shoppingListModel)
        {
            Logger.Debug("userId={0}, {1}", userId, shoppingListModel);
            var shoppingList = shoppingListService.Update(shoppingListModel.Id, shoppingListModel.Title, userId);
            var updatedShoppingListModel = new ShoppingListOverviewModel(shoppingList);
            Clients.Group(shoppingListModel.Id.ToString()).shoppingListUpdated(updatedShoppingListModel);
        }

        [UnitOfWork]
        public void DeleteListItem(ListItemModel listItemModel)
        {
            Logger.Debug("userId={0}, {1}", userId, listItemModel);
            listItemService.Delete(listItemModel.Id, listItemModel.ShoppingListId, userId);
            Clients.Group(listItemModel.ShoppingListId.ToString()).listItemDeleted(listItemModel.Id);
        }

        [UnitOfWork]
        public void UnpickAllListItems(long shoppingListId)
        {
            Logger.Debug("userId={0}, shoppingListId={1}", userId, shoppingListId);
            shoppingListService.UnpickAllListItems(shoppingListId, userId);
            Clients.Group(shoppingListId.ToString()).allListItemsUnpicked();
        }

        [UnitOfWork]
        public void UpdateListItem(ListItemModel listItemModel)
        {
            Logger.Debug("userId={0}, {1}", userId, listItemModel);
            var updatedListItemModel = new ListItemModel(
                listItemService.Update(listItemModel.Description, listItemModel.Quantity, listItemModel.Id, listItemModel.ShoppingListId, userId)
            );
            Clients.Group(listItemModel.ShoppingListId.ToString()).listItemUpdated(updatedListItemModel);
        }

        [UnitOfWork]
        public void CreateListItem(ListItemModel listItemModel)
        {
            Logger.Debug("userId={0}, {1}", userId, listItemModel);
            var newListItemModel = new ListItemModel(
                listItemService.Create(listItemModel.Description, listItemModel.Quantity, listItemModel.ShoppingListId, userId)
            );
            Clients.Group(listItemModel.ShoppingListId.ToString()).listItemCreated(newListItemModel);
        }

        [UnitOfWork]
        public void PickListItem(ListItemModel listItemModel)
        {
            Logger.Debug("userId={0}, {1}", userId, listItemModel);
            var updatedListItemModel = new ListItemModel(
                listItemService.Pick(listItemModel.Id, listItemModel.ShoppingListId, userId)
            );
            Clients.Group(listItemModel.ShoppingListId.ToString()).listItemUpdated(updatedListItemModel);
        }

        [UnitOfWork]
        public void UnpickListItem(ListItemModel listItemModel)
        {
            Logger.Debug("userId={0}, {1}", userId, listItemModel);
            var updatedListItemModel = new ListItemModel(
                listItemService.Unpick(listItemModel.Id, listItemModel.ShoppingListId, userId)
            );
            Clients.Group(listItemModel.ShoppingListId.ToString()).listItemUpdated(updatedListItemModel);
        }

        public void RemoveViewAccess(string userId)
        {
            Logger.Debug("userId={0}", userId);
            Clients.User(userId).viewAccessRemoved();
        }
    }
}
