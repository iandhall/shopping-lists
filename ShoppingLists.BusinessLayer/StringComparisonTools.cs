using System.Text.RegularExpressions;

namespace ShoppingLists.BusinessLayer
{
    public static class StringComparisonTools
    {
        public static string PadNumbers(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            else
            {
                return Regex.Replace(input, "[0-9]+", match => match.Value.PadLeft(10, '0'));
            }
        }
    }
}