using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ShoppingLists.Core.Entities;
using ShoppingLists.BusinessLayer;
using ShoppingLists.BusinessLayer.Exceptions;
using ShoppingLists.Web.Models;
using ShoppingLists.Web.Filters;
using LogForMe;
using System.Net;
using ShoppingLists.Core;
using System.IO;

namespace ShoppingLists.Web.Controllers
{
    [Authorize, UnitOfWork]
    public class ShoppingListController : Controller, IHasUnitOfWork
    {
        private IUnitOfWork uow;
        private ShoppingListService shoppingListService;
        private UserService userService;
        private PermissionTypesService permissionService;

        public IUnitOfWork Uow { get { return uow; } }

        public ShoppingListController(ShoppingListService shoppingListService, IUnitOfWork uow, UserService userService, PermissionTypesService permissionService)
        {
            this.uow = uow;
            this.shoppingListService = shoppingListService;
            this.userService = userService;
            this.permissionService = permissionService;
        }

        private string userId
        {
            get { return User.Identity.GetUserId(); }
        }

        public ActionResult Index()
        {
            var shoppingLists = shoppingListService.FindAllForUser(userId);
            return View(new MyShoppingListsModel(shoppingLists, userId));
        }

        public ActionResult Show(long id)
        {
            var shoppingList = shoppingListService.Get(id, userId, includeListItems: true);
            var currentUser = userService.Get(userId, includePermissions: true, shoppingListId: id);
            return View(new ShoppingListModel(shoppingList, currentUser));
        }

        public ActionResult Create()
        {
            var shoppingList = shoppingListService.Create(userId);
            if (shoppingList == null) return RedirectToAction("Index");
            return RedirectToAction("Show", new { id = shoppingList.Id });
        }

        public ActionResult Share(long id)
        {
            var shoppingList = shoppingListService.Get(id, userId);
            var users = userService.GetAllForShoppingList(id);
            var shoppingListSharingModel = new ShoppingListSharingModel(shoppingList, users, userId);
            return View(shoppingListSharingModel);
        }

        [HttpPost]
        public JsonResult Delete(long id)
        {
            Logger.Debug("shoppingListId={0}", id);
            shoppingListService.Delete(id, userId);
            return Json("");
        }

        [HttpPost]
        public JsonResult Ignore(long id)
        {
            Logger.Debug("shoppingListId={0}", id);
            shoppingListService.Ignore(id, userId);
            return Json("");
        }

        [HttpPost]
        public JsonResult ShareWithUser(long shoppingListId, string username)
        {
            var userToShareWith = userService.GetByName(username);
            if (userToShareWith == null) throw new UserNotFoundException(username);
            shoppingListService.ShareWithUser(shoppingListId, userToShareWith.Id, userId);
            var userModel = new UserSharingModel(userToShareWith);
            return Json(userModel);
        }

        [HttpPost]
        public JsonResult RemoveSharingUser(long shoppingListId, UserSharingModel userSharingModel)
        {
            shoppingListService.RemoveSharingUser(shoppingListId, userSharingModel.Id, userId);
            return Json("");
        }

        [HttpPost]
        public JsonResult GetDefaultPermissions()
        {
            var availablePermissions = permissionService.GetAll();
            IEnumerable<PermissionModel> permissionModels = availablePermissions.Where(p => p.Id != Permissions.View).Select(p => new PermissionModel(p));
            return Json(permissionModels);
        }

        [HttpPost]
        public JsonResult GetPermissionsForUser(long shoppingListId, string permissionsUserId, bool shouldGetDefaultPermissions)
        {
            var availablePermissions = permissionService.GetAll().Where(p => p.Id != Permissions.View);
            IEnumerable<PermissionModel> permissionModels;
            if (shouldGetDefaultPermissions)
            {
                permissionModels = availablePermissions.Select(p => new PermissionModel(p));
            }
            else
            {
                var entityPermissions = shoppingListService.GetPermissionsForUser(shoppingListId, permissionsUserId, userId);
                permissionModels = availablePermissions.Select(p => new PermissionModel(p, entityPermissions));
            }
            return Json(permissionModels);
        }

        [HttpPost]
        public JsonResult SetPermissionsForUser(long shoppingListId, string permissionsUserId, ICollection<long> selectedPermissionIds)
        {
            if (selectedPermissionIds == null) selectedPermissionIds = new List<long>();
            selectedPermissionIds.Add((long)Permissions.View);
            userService.SetPermissions(permissionsUserId, shoppingListId, selectedPermissionIds);
            return Json("");
        }
    }
}
