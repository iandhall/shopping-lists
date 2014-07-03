using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingLists.Web.Helpers
{
    public static class RenderBootstrapGuideHelper
    {
        public static MvcHtmlString RenderBootstrapGuide(this HtmlHelper htmlHelper)
        {
#if DEBUG
            var output = new StringBuilder(@"
<style type=""text/css"">
    .guide { border: 1px dashed black; }
</style>
<div class=""row"">
");

            for (byte col = 1; col <= 12; col++)
            {
                output.AppendLine("\t" + @"<div class=""col-xs-1 guide"">&nbsp;</div>");
            }

            output.AppendLine("</div>");

            return MvcHtmlString.Create(output.ToString());
#else
            return MvcHtmlString.Create("");
#endif
        }
    }
}