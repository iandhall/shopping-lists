using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingLists.Entities
{
    // Should reflect the Id column of the Permissions table:
    public enum Statuses : short
    {
        NotPicked = 1,
        Picked = 2
    }
}
