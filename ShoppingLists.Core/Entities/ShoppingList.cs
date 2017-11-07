using System.Collections.Generic;

namespace ShoppingLists.Core.Entities
{
    public class ShoppingList : TimestampedEntity
    {
        public string Title { get; set; }

        public virtual ICollection<ListItem> ListItems { get; set; }
        public virtual ICollection<ShoppingListPermission> ShoppingListPermissions { get; set; }
    }
}
