using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace ShoppingLists.DataAccessLayer
{
    public static class DataAccessStartup
    {
        public static void Initialise()
        {
            Database.SetInitializer<ShoppingListsDbContext>(null);
        }
    }
}
