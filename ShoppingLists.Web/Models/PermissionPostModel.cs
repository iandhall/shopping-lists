using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Web.Models
{
    public class PermissionPostModel
    {
        public long PermissionId { get; set; }
        public bool Selected { get; set; }
    }
}