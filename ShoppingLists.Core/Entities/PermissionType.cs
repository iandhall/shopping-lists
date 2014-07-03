using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingLists.Core.Entities
{
    public class PermissionType
    {
        public Permissions Id { get; set; }
        public string Description { get; set; }
        public bool SelectedDefault { get; set; }
    }
}
