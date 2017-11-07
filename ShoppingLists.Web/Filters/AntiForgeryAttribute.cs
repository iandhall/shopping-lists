using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ShoppingLists.Web.Filters
{
    public class AntiForgeryAttribute : IAuthorizationFilter
    {

        // Adds ValidateAntiForgeryTokenAttribute to POST requests.
        public void OnAuthorization(AuthorizationContext authorizationContext)
        {
            //if (!authorizationContext.RequestContext.HttpContext.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase)) return;
            //new ValidateAntiForgeryTokenAttribute().OnAuthorization(authorizationContext);


            var request = authorizationContext.RequestContext.HttpContext.Request;

            //  Only validate POSTs
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                //  Ajax POSTs and normal form posts have to be treated differently when it comes
                //  to validating the AntiForgeryToken
                if (request.IsAjaxRequest())
                {
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                    var cookieValue = antiForgeryCookie != null
                        ? antiForgeryCookie.Value
                        : null;

                    AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute().OnAuthorization(authorizationContext);
                }
            }
        }
    }
}