using System.Collections.Generic;

namespace ShoppingLists.Shared.Entities
{
    public class ShoppingList : TimestampedEntity
    {
        public string Title { get; set; }

        public virtual ICollection<ListItem> ListItems { get; set; }
        public virtual ICollection<Permission> ShoppingListPermissions { get; set; }
    }
}
