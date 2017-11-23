using System.Web;
using Microsoft.AspNet.Identity;
using ShoppingLists.Core;

namespace ShoppingLists.Web
{
    public class UserContext : IUserContext
    {
        public string UserId {
            get {
                return HttpContext.Current.User.Identity.GetUserId();
            }
        }
    }
}
