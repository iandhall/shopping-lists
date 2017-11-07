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
