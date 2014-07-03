using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingLists.DataAccessLayer;

namespace ShoppingLists.BusinessLayer
{
    public static class BusinessStartup
    {
        public static void Initialise()
        {
            DataAccessStartup.Initialise();
        }
    }
}
