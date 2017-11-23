using System;
using Microsoft.AspNet.SignalR;
using ShoppingLists.BusinessLayer;
using ShoppingLists.Core;
using System.Threading.Tasks;
using ShoppingLists.Web.Models;
using NLog;
using LightInject;
using Microsoft.AspNet.Identity;
using ShoppingLists.BusinessLayer.Exceptions;

namespace ShoppingLists.Web.Hubs
{
    public class ShoppingListHub : Hub
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private IUserContext _userContext;
        private ShoppingListService _shoppingListService;
        private ListItemService _listItemService;
        private UserService _userService;

        public ShoppingListHub(IUserContext userContext, ShoppingListService shoppingListService, ListItemService listItemService, UserService userService)
        {
            _userContext = userContext;
            _shoppingListService = shoppingListService;
            _listItemService = listItemService;
            _userService = userService;
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
                    _log.Error(ex);
                    throw;
                }
            }
        }

        public override Task OnConnected()
        {
            try
            {
                _log.Debug("ConnectionId={0}", Context.ConnectionId);
                return base.OnConnected();
            }
            catch (Exception ex)
            { // Not logged by ErrorLoggingPipelineModule.
                _log.Error(ex);
                throw;
            }
        }

        public override Task OnReconnected()
        {
            try
            {
                _log.Debug("ConnectionId={0}", Context.ConnectionId);
                return base.OnReconnected();
            }
            catch (Exception ex)
            { // Not logged by ErrorLoggingPipelineModule.
                _log.Error(ex);
                throw;
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            try
            {
                _log.Debug("ConnectionId={0}", Context.ConnectionId);
                var connection = ConnectionHolder.Get(Context.ConnectionId);
                if (connection == null)
                {
                    return base.OnDisconnected(stopCalled);
                }
                if (connection.UserId != userId)
                {
                    throw new UnexpectedUserIdException(userId, connection.UserId);
                }
                Clients.Group(connection.Group).viewerRemoved(connection.Username);
                Groups.Remove(connection.Id, connection.Group);
                ConnectionHolder.Remove(Context.ConnectionId);
                return base.OnDisconnected(stopCalled);
            }
            catch (Exception ex)
            { // Not logged by ErrorLoggingPipelineModule.
                _log.Error(ex);
                throw;
            }
        }

        // Exceptions in Hub methods are logged by ErrorLoggingPipelineModule:

        public void StartViewingList(long shoppingListId)
        {
            _log.Debug("shoppingListId={0}, ConnectionId={1}", shoppingListId, Context.ConnectionId);
            string shoppingListIdString = shoppingListId.ToString();
            var user = _userService.Get(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            var shoppingList = _shoppingListService.Get(shoppingListId); // Attempt to load the ShoppingList to validate that the current user has the appropriate permissions.
            ConnectionHolder.AddOrUpdate(Context.ConnectionId, shoppingListIdString, userId, user.Username); // Add a HubConnection for the current user.
            Groups.Add(Context.ConnectionId, shoppingListIdString); // Add the current user to the group for the ShoppingList.
            Clients.Group(shoppingListIdString).viewerAdded(user.Username); // Inform all clients currently viewing this ShoppingList of the new user that is viewing it.
            Clients.Caller.allCurrentViewersReceived(ConnectionHolder.GetAllUsernamesInGroup(shoppingListIdString)); // Send list of all users viewing this ShoppingList to the caller.
        }

        public void Update(ShoppingListOverviewModel shoppingListModel)
        {
            _log.Debug("userId={0}, {1}", userId, shoppingListModel);
            var shoppingList = _shoppingListService.Update(shoppingListModel.Id, shoppingListModel.Title);
            var updatedShoppingListModel = new ShoppingListOverviewModel(shoppingList);
            Clients.Group(shoppingListModel.Id.ToString()).shoppingListUpdated(updatedShoppingListModel);
        }

        public void DeleteListItem(ListItemModel listItemModel)
        {
            _log.Debug("userId={0}, {1}", userId, listItemModel);
            _listItemService.Delete(listItemModel.Id, listItemModel.ShoppingListId);
            Clients.Group(listItemModel.ShoppingListId.ToString()).listItemDeleted(listItemModel.Id);
        }

        public void UnpickAllListItems(long shoppingListId)
        {
            _log.Debug("userId={0}, shoppingListId={1}", userId, shoppingListId);
            _shoppingListService.UnpickAllListItems(shoppingListId);
            Clients.Group(shoppingListId.ToString()).allListItemsUnpicked();
        }

        public void UpdateListItem(ListItemModel listItemModel)
        {
            _log.Debug("userId={0}, {1}", userId, listItemModel);
            var updatedListItemModel = new ListItemModel(
                _listItemService.Update(listItemModel.Description, listItemModel.Quantity, listItemModel.Id, listItemModel.ShoppingListId)
            );
            Clients.Group(listItemModel.ShoppingListId.ToString()).listItemUpdated(updatedListItemModel);
        }

        public void CreateListItem(ListItemModel listItemModel)
        {
            _log.Debug("userId={0}, {1}", userId, listItemModel);
            var newListItemModel = new ListItemModel(
                _listItemService.Create(listItemModel.Description, listItemModel.Quantity, listItemModel.ShoppingListId)
            );
            Clients.Group(listItemModel.ShoppingListId.ToString()).listItemCreated(newListItemModel);
        }

        public void PickListItem(ListItemModel listItemModel)
        {
            _log.Debug("userId={0}, {1}", userId, listItemModel);
            var updatedListItemModel = new ListItemModel(
                _listItemService.Pick(listItemModel.Id, listItemModel.ShoppingListId)
            );
            Clients.Group(listItemModel.ShoppingListId.ToString()).listItemUpdated(updatedListItemModel);
        }

        public void UnpickListItem(ListItemModel listItemModel)
        {
            _log.Debug("userId={0}, {1}", userId, listItemModel);
            var updatedListItemModel = new ListItemModel(
                _listItemService.Unpick(listItemModel.Id, listItemModel.ShoppingListId)
            );
            Clients.Group(listItemModel.ShoppingListId.ToString()).listItemUpdated(updatedListItemModel);
        }

        public void RemoveViewAccess(string userId)
        {
            _log.Debug("userId={0}", userId);
            Clients.User(userId).viewAccessRemoved();
        }
    }
}
