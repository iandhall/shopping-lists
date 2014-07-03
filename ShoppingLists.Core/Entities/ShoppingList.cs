using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingLists.Core.Entities
{
    public class ShoppingList : TimestampedEntity
    {
        public string Title { get; set; }

        public virtual ICollection<ListItem> ListItems { get; set; }
        public virtual ICollection<ShoppingListPermission> ShoppingListPermissions { get; set; }
    }
}
