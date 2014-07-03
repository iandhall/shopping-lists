using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingLists.Core
{
    // Should reflect the Id column of the Permissions table:
    public enum Permissions
    {
        View = 1,
        Edit = 2,
        Share = 3,
        Delete = 4,
        AddListItems = 5,
        PickOrUnpickListItems = 6,
        RemoveListItems = 7,
        EditListItems = 8
    }
}
