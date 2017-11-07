using System.Web;

namespace ShoppingLists.Web
{
    public static class EncodingHelper
    {
        public static string Encode(string text)
        {
            if (text == null) return null;
            return HttpUtility.HtmlEncode(text)
                .Replace("&#39;", "'") // These chars are allowed and aren't encoded:
                .Replace("&amp;", "&")
                .Replace("&#163;", "£")
                .Replace("&#39;", "@");
        }
    }
}