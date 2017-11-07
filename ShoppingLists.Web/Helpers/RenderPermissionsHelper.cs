using System;
using System.Text;
using System.Linq;
using ShoppingLists.Core;
using System.Web.Mvc;

namespace ShoppingLists.Web.Helpers
{
    public static class RenderPermissionsHelper
    {
        public static MvcHtmlString RenderPermissionsJs(this HtmlHelper htmlHelper)
        {
            var output = new StringBuilder(@"var permissions = {");

            foreach (var permission in Enum.GetValues(typeof(Permissions)).Cast<Permissions>())
            {
                output.AppendFormat("{2}\t\t\t\t\t{0}: {1},",
                    Enum.GetName(typeof(Permissions), permission),
                    (int)permission,
                    Environment.NewLine);
            }

            output.AppendFormat("{0}\t\t\t\t}}", Environment.NewLine);
            return MvcHtmlString.Create(output.ToString());
        }
    }
}