using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingLists.Core.Entities
{
    public class ShoppingListPermission : TimestampedEntity
    {
        public Permissions PermissionTypeId { get; set; }
        public string UserId { get; set; }
        public long ShoppingListId { get; set; }

        public PermissionType PermissionType { get; set; } // The Id of this type of Permission (corresponds with the Permissions and the Id of the Permission in the Permissions table).
        public User User { get; set; } // The User to which this ShoppingListPermission belongs.
        public ShoppingList ShoppingList { get; set; } // The ShoppingList with which this ShoppingListPermission is associated.
    }
}
