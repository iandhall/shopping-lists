using System.Collections.Generic;

namespace ShoppingLists.Shared.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Discriminator { get; set; }

        public virtual ICollection<Permission> ShoppingListPermissions { get; set; } // The User's ShoppingListPermissions.
    }
}
