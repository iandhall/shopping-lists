using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using NLog;
using ShoppingLists.BusinessLayer;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.Shared;
using ShoppingLists.Shared.ServiceInterfaces;
using ShoppingLists.Web.Models;

namespace ShoppingLists.Web.Api
{
    [RoutePrefix("api/shopping-lists")]
    [Authorize]
    [ApiValidateAntiForgeryHeader]
    public class ShoppingListsController : ApiController
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private IUserContext _userContext;
        private IShoppingListService _shoppingListService;
        private IUserService _userService;
        private IPermissionTypeService _permissionTypeService;

        public ShoppingListsController(IUserContext userContext, IShoppingListService shoppingListService, IUserService userService, IPermissionTypeService permissionTypeService)
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

        [Route("{shoppingListId:long}/share/{username}")]
        [HttpPut]
        public HttpResponseMessage Share(long shoppingListId, string username)
        {
            UserSharingModel userModel = null;
            try
            {
                var userToShareWith = _userService.GetByName(username);
                if (userToShareWith == null) throw new UserNotFoundException(username);
                _shoppingListService.ShareWithUser(shoppingListId, userToShareWith.Id);
                userModel = new UserSharingModel(userToShareWith);
            }
            catch (UserNotFoundException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { userMessage = string.Format("No user called '{0}' was found.", username) });
            }
            catch (ShareWithListCreatorException e)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, new { userMessage = string.Format("Can't share the list with its creator - '{0}'.", username) });
            }
            catch (ShareWithYourselfException e)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, new { userMessage = string.Format("You can't share your list with yourself, {0}.", username) });
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { });
            }

            return Request.CreateResponse(HttpStatusCode.OK, userModel);
        }

        [Route("{shoppingListId:long}/unshare/{sharingUserId:guid}")]
        [HttpPut]
        [ValidateApiModel]
        public void Unshare(long shoppingListId, string sharingUserId)
        {
            _shoppingListService.RemoveSharingUser(shoppingListId, sharingUserId);
        }


        [Route("~/api/permissions/default")]
        [HttpGet]
        public IEnumerable<PermissionModel> GetDefaultPermissions()
        {
            var availablePermissions = _permissionTypeService.GetAll();
            IEnumerable<PermissionModel> permissionModels = availablePermissions.Where(p => p.Id != Permissions.View).Select(p => new PermissionModel(p));
            return permissionModels;
        }

        [Route("{shoppingListId:long}/permissions/{permissionsUserId:guid}/{shouldGetDefaultPermissions:bool}")]
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

        [Route("{shoppingListId:long}/permissions/{permissionsUserId:guid}")]
        [HttpPut]
        public void SetPermissionsForUser(long shoppingListId, string permissionsUserId, [FromBody] SetPermissionsModel setPermissionsModel)
        {
            var selectedPermissionIds = setPermissionsModel.selectedPermissionIds;
            if (selectedPermissionIds == null) selectedPermissionIds = new List<long>();
            selectedPermissionIds.Add((long)Permissions.View);
            _userService.SetPermissions(permissionsUserId, shoppingListId, selectedPermissionIds);
        }
    }
}