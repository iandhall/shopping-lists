using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NLog;

namespace ShoppingLists.Web
{
    // Validates the anti forgery token in the request headers against the one in the cookie (API controllers). Is applied to all verbs including GET.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ApiValidateAntiForgeryHeaderAttribute : ActionFilterAttribute
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        [DebuggerHidden]
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            var cookieValue = "";
            var headerValue = "";

            // Compares the __RequestVerificationToken cookie value with the __RequestVerificationToken header field value.
            try
            {
                cookieValue = HttpContext.Current.Request.Cookies[AntiForgeryConfig.CookieName] != null
                    ? HttpContext.Current.Request.Cookies[AntiForgeryConfig.CookieName].Value
                    : null;

                headerValue = request.Headers.GetValues("__RequestVerificationToken").Single();
            }
            finally
            {
                AntiForgery.Validate(cookieValue, headerValue);
                base.OnActionExecuting(actionContext);
            }
        }
    }
}