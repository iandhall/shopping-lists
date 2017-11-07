using System.Data.Entity;

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
