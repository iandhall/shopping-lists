using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using NLog;
using ShoppingLists.BusinessLayer;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.Core;
using ShoppingLists.Web.Models;

namespace ShoppingLists.Web.Api
{
    [RoutePrefix("api/shopping-lists")]
    [ApiValidateAntiForgeryHeader]
    public class ShoppingListsController : ApiController
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private IUserContext _userContext;
        private ShoppingListService _shoppingListService;
        private UserService _userService;
        private PermissionTypeService _permissionTypeService;

        public ShoppingListsController(IUserContext userContext, ShoppingListService shoppingListService, UserService userService, PermissionTypeService permissionTypeService)
        {
            _userContext = userContext;
            _shoppingListService = shoppingListService;
            _userService = userService;
            _permissionTypeService = permissionTypeService;
        }

        [HttpPost]
        [Route]
        public ShoppingListModel Create()
        {
            var shoppingList = _shoppingListService.Create();
            return new ShoppingListModel(shoppingList, _userService.Get(_userContext.UserId));
        }

        [Route("{id:long}")]
        [HttpDelete]
        public void Delete(long id)
        {
            _log.Debug("shoppingListId={0}", id);
            _shoppingListService.Delete(id);
        }

        [Route("{id:long}/ignore")]
        [HttpPut]
        public void Ignore(long id)
        {
            _log.Debug("shoppingListId={0}", id);
            _shoppingListService.Ignore(id);
        }

        [Route("{id:long}/users/{username}")]
        [HttpPut]
        public UserSharingModel ShareWithUser(long shoppingListId, string username)
        {
            var userToShareWith = _userService.GetByName(username);
            if (userToShareWith == null) throw new UserNotFoundException(username);
            _shoppingListService.ShareWithUser(shoppingListId, userToShareWith.Id);
            var userModel = new UserSharingModel(userToShareWith);
            return userModel;
        }

        [Route("{id:long}/users")]
        [HttpPut]
        [ValidateApiModel]
        public void RemoveSharingUser(long shoppingListId, UserSharingModel userSharingModel)
        {
            _shoppingListService.RemoveSharingUser(shoppingListId, userSharingModel.Id);
        }

        [Route("permissions/default")]
        [HttpGet]
        public IEnumerable<PermissionModel> GetDefaultPermissions()
        {
            var availablePermissions = _permissionTypeService.GetAll();
            IEnumerable<PermissionModel> permissionModels = availablePermissions.Where(p => p.Id != Permissions.View).Select(p => new PermissionModel(p));
            return permissionModels;
        }

        [Route("{id:long}/permissions/{permissionsUserId:long}")]
        [HttpGet]
        public IEnumerable<PermissionModel> GetPermissionsForUser(long shoppingListId, string permissionsUserId, bool shouldGetDefaultPermissions)
        {
            var availablePermissions = _permissionTypeService.GetAll().Where(p => p.Id != Permissions.View);
            IEnumerable<PermissionModel> permissionModels;
            if (shouldGetDefaultPermissions)
            {
                permissionModels = availablePermissions.Select(p => new PermissionModel(p));
            }
            else
            {
                var entityPermissions = _shoppingListService.GetPermissionsForUser(shoppingListId, permissionsUserId);
                permissionModels = availablePermissions.Select(p => new PermissionModel(p, entityPermissions));
            }
            return permissionModels;
        }

        [Route("{id:long}/permissions/{permissionsUserId:long}")]
        [HttpPut]
        public void SetPermissionsForUser(long shoppingListId, string permissionsUserId, ICollection<long> selectedPermissionIds)
        {
            if (selectedPermissionIds == null) selectedPermissionIds = new List<long>();
            selectedPermissionIds.Add((long)Permissions.View);
            _userService.SetPermissions(permissionsUserId, shoppingListId, selectedPermissionIds);
        }
    }
}