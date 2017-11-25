using System;
using System.Web.Helpers;
using System.Web.Mvc;
using NLog;

namespace ShoppingLists.Web
{
    // Validates the anti forgery token in the request headers against the one in the cookie.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MvcValidateAntiForgeryHeaderAttribute : FilterAttribute, IAuthorizationFilter
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public void OnAuthorization(AuthorizationContext authorizationContext)
        {
            var request = authorizationContext.RequestContext.HttpContext.Request;
            var cookieValue = "";
            var headerValue = "";
            try
            {
                cookieValue = request.Cookies[AntiForgeryConfig.CookieName] != null
                    ? request.Cookies[AntiForgeryConfig.CookieName].Value
                    : null;
                headerValue = request.Headers["__RequestVerificationToken"];
            }
            finally
            {
                AntiForgery.Validate(cookieValue, headerValue);
            }
        }
    }
}