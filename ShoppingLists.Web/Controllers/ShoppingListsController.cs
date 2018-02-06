using System.Web.Mvc;
using NLog;
using ShoppingLists.BusinessLayer;
using ShoppingLists.Shared;
using ShoppingLists.Shared.ServiceInterfaces;
using ShoppingLists.Web.Models;

namespace ShoppingLists.Web.Controllers
{
    [Authorize]
    public class ShoppingListsController : Controller
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private IUserContext _userContext;
        private IShoppingListService _shoppingListService;
        private IUserService _userService;

        public ShoppingListsController(IUserContext userContext, IShoppingListService shoppingListService, IUserService userService)
        {
            _userContext = userContext;
            _shoppingListService = shoppingListService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            var shoppingLists = _shoppingListService.FindAllForCurrentUser();
            return View(new ShoppingListsIndexModel(shoppingLists, _userContext.UserId));
        }

        public ActionResult Show(long id)
        {
            var shoppingList = _shoppingListService.Get(id, includeListItems: true);
            var currentUser = _userService.Get(_userContext.UserId, includePermissions: true, shoppingListId: id);
            return View(new ShoppingListModel(shoppingList, currentUser));
        }

        public ActionResult Share(long id)
        {
            var shoppingList = _shoppingListService.Get(id);
            var users = _userService.GetAllForShoppingList(id);
            var shoppingListSharingModel = new ShoppingListSharingModel(shoppingList, users, _userContext.UserId);
            return View(shoppingListSharingModel);
        }
    }
}
