using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogForMe;

namespace ShoppingLists.BusinessLayer
{
    public class NumericStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            int xInt;
            int yInt;
            bool xIsNumeric = int.TryParse(x, out xInt);
            bool yIsNumeric = int.TryParse(y, out yInt);

            if (xIsNumeric && yIsNumeric)
            {
                if (xInt > yInt)
                {
                    return 1;
                }
                if (xInt < yInt)
                {
                    return -1;
                }
                if (xInt == yInt)
                {
                    return 0;
                }
            }
            if (xIsNumeric && !yIsNumeric)
            {
                return 1;
            }
            if (!xIsNumeric && yIsNumeric)
            {
                return -1;
            }
            return string.Compare(x, y, true);
        }
    }
}
