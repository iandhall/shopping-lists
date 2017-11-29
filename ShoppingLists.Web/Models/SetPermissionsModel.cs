using System.Collections.Generic;

namespace ShoppingLists.Web.Models
{
    public class SetPermissionsModel
    {
        public ICollection<long> selectedPermissionIds { get; set; }
    }
}