using System.Web.Mvc;
using System.Web.Routing;
using NLog;

namespace ShoppingLists.Web.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Log("OnActionExecuting", filterContext.RouteData);
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted", filterContext.RouteData);
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext.RouteData);
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext.RouteData);
            base.OnResultExecuted(filterContext);
        }

        private void Log(string methodName, RouteData routeData)
        {
            _log.Debug("{1}.{2}: {0}", methodName, routeData.Values["controller"], routeData.Values["action"]);
        }
    }
}