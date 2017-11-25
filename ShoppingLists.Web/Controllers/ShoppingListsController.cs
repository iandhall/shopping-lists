using System.Web.Mvc;
using NLog;
using ShoppingLists.BusinessLayer;
using ShoppingLists.Core;
using ShoppingLists.Web.Models;

namespace ShoppingLists.Web.Controllers
{
    [RoutePrefix("shopping-lists")]
    [Authorize]
    public class ShoppingListsController : Controller
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private IUserContext _userContext;
        private ShoppingListService _shoppingListService;
        private UserService _userService;

        public ShoppingListsController(IUserContext userContext, ShoppingListService shoppingListService, UserService userService)
        {
            _userContext = userContext;
            _shoppingListService = shoppingListService;
            _userService = userService;
        }

        [Route]
        public ActionResult Index()
        {
            var shoppingLists = _shoppingListService.FindAllForCurrentUser();
            return View(new ShoppingListsIndexModel(shoppingLists, _userContext.UserId));
        }

        [Route("{id:long}")]
        public ActionResult Show(long id)
        {
            var shoppingList = _shoppingListService.Get(id, includeListItems: true);
            var currentUser = _userService.Get(_userContext.UserId, includePermissions: true, shoppingListId: id);
            return View(new ShoppingListModel(shoppingList, currentUser));
        }

        [Route("{id:long}/share")]
        public ActionResult Share(long id)
        {
            var shoppingList = _shoppingListService.Get(id);
            var users = _userService.GetAllForShoppingList(id);
            var shoppingListSharingModel = new ShoppingListSharingModel(shoppingList, users, _userContext.UserId);
            return View(shoppingListSharingModel);
        }
    }
}
