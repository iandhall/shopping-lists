using System.Web.Mvc;
using NLog;

namespace ShoppingLists.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "ShoppingLists");
            }
            else
            {
                return View();
            }
        }

        public ActionResult About()
        {
            return View();
        }
    }
}