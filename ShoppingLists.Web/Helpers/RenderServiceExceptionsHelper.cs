using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ShoppingLists.BusinessLayer.Exceptions;
using System.Web.Mvc;

namespace ShoppingLists.Web.Helpers
{
    public static class RenderServiceExceptionsHelper
    {
        public static MvcHtmlString RenderServiceExceptionsJs(this HtmlHelper htmlHelper)
        {
            var output = new StringBuilder(@"var serviceExceptions = {");

            foreach (var exceptionType in GetServiceExceptionTypes())
            {
                output.AppendFormat("{2}\t\t\t\t\t{0}: \"{1}\",",
                    exceptionType.Name,
                    exceptionType.Name,
                    //exceptionType.GetField("StaticErrorCode").GetValue(null),
                    Environment.NewLine);
            }

            output.AppendFormat("{0}\t\t\t\t}}", Environment.NewLine);
            return MvcHtmlString.Create(output.ToString());
        }

        // Find all Types that extend ServiceValidationException.
        private static IEnumerable<Type> GetServiceExceptionTypes()
        {
            return typeof(ServiceValidationException).Assembly.DefinedTypes.Where(t => t.IsSubclassOf(typeof(ServiceValidationException))).ToList();
        }
    }
}