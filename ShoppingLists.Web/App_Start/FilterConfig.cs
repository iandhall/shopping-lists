using System.Web;
using System.Web.Mvc;
using ShoppingLists.Web.Filters;

namespace ShoppingLists.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AntiForgeryAttribute()); // Validates AntiForgeryToken on all POST requests.
            filters.Add(new LogAttribute());
        }
    }
}
