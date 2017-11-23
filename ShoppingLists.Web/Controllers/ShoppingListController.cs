using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NLog;
using ShoppingLists.BusinessLayer;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.Core;
using ShoppingLists.Web.Models;

namespace ShoppingLists.Web.Controllers
{
    [Authorize]
    public class ShoppingListController : Controller
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private IUserContext _userContext;
        private ShoppingListService _shoppingListService;
        private UserService _userService;
        private PermissionTypeService _permissionService;

        public ShoppingListController(IUserContext userContext, ShoppingListService shoppingListService, UserService userService, PermissionTypeService permissionService)
        {
            _userContext = userContext;
            _shoppingListService = shoppingListService;
            _userService = userService;
            _permissionService = permissionService;
        }

        public ActionResult Index()
        {
            var shoppingLists = _shoppingListService.FindAllForCurrentUser();
            return View(new MyShoppingListsModel(shoppingLists, _userContext.UserId));
        }

        public ActionResult Show(long id)
        {
            var shoppingList = _shoppingListService.Get(id, includeListItems: true);
            var currentUser = _userService.Get(_userContext.UserId, includePermissions: true, shoppingListId: id);
            return View(new ShoppingListModel(shoppingList, currentUser));
        }

        public ActionResult Create()
        {
            var shoppingList = _shoppingListService.Create();
            if (shoppingList == null) return RedirectToAction("Index");
            return RedirectToAction("Show", new { id = shoppingList.Id });
        }

        public ActionResult Share(long id)
        {
            var shoppingList = _shoppingListService.Get(id);
            var users = _userService.GetAllForShoppingList(id);
            var shoppingListSharingModel = new ShoppingListSharingModel(shoppingList, users, _userContext.UserId);
            return View(shoppingListSharingModel);
        }

        [HttpPost]
        public JsonResult Delete(long id)
        {
            _log.Debug("shoppingListId={0}", id);
            _shoppingListService.Delete(id);
            return Json("");
        }

        [HttpPost]
        public JsonResult Ignore(long id)
        {
            _log.Debug("shoppingListId={0}", id);
            _shoppingListService.Ignore(id);
            return Json("");
        }

        [HttpPost]
        public JsonResult ShareWithUser(long shoppingListId, string username)
        {
            var userToShareWith = _userService.GetByName(username);
            if (userToShareWith == null) throw new UserNotFoundException(username);
            _shoppingListService.ShareWithUser(shoppingListId, userToShareWith.Id);
            var userModel = new UserSharingModel(userToShareWith);
            return Json(userModel);
        }

        [HttpPost]
        public JsonResult RemoveSharingUser(long shoppingListId, UserSharingModel userSharingModel)
        {
            _shoppingListService.RemoveSharingUser(shoppingListId, userSharingModel.Id);
            return Json("");
        }

        [HttpPost]
        public JsonResult GetDefaultPermissions()
        {
            var availablePermissions = _permissionService.GetAll();
            IEnumerable<PermissionModel> permissionModels = availablePermissions.Where(p => p.Id != Permissions.View).Select(p => new PermissionModel(p));
            return Json(permissionModels);
        }

        [HttpPost]
        public JsonResult GetPermissionsForUser(long shoppingListId, string permissionsUserId, bool shouldGetDefaultPermissions)
        {
            var availablePermissions = _permissionService.GetAll().Where(p => p.Id != Permissions.View);
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
            return Json(permissionModels);
        }

        [HttpPost]
        public JsonResult SetPermissionsForUser(long shoppingListId, string permissionsUserId, ICollection<long> selectedPermissionIds)
        {
            if (selectedPermissionIds == null) selectedPermissionIds = new List<long>();
            selectedPermissionIds.Add((long)Permissions.View);
            _userService.SetPermissions(permissionsUserId, shoppingListId, selectedPermissionIds);
            return Json("");
        }
    }
}
