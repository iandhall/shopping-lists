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
            var request = authorizationContext.RequestContext.HttpContext.Request;

            //  Only validate token on POST requests
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                if (request.IsAjaxRequest())
                {
                    // For ajax posts, get the value from the header
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
                else
                {
                    // For form posts, get the value from the form data
                    new ValidateAntiForgeryTokenAttribute().OnAuthorization(authorizationContext);
                }
            }
        }
    }
}