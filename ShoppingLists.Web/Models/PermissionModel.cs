using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Core;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.Web.Models
{
    public class PermissionModel
    {
        public Permissions PermissionTypeId { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }

        public PermissionModel()
        {
        }

        // Create a PermissionModel setting Selected to the Permission default value (called just after a new shared user has been added and no permissions have been set).
        public PermissionModel(PermissionType permissionType)
        {
            PermissionTypeId = permissionType.Id;
            Description = permissionType.Description;
            Selected = permissionType.SelectedDefault;
        }

        // Create a PermissionModel setting Selected to true if a corresponding EntityPermission exitst otherwise false (called when editing an existing shared user's permissions).
        public PermissionModel(PermissionType permissionType, IEnumerable<ShoppingListPermission> entityPermissionToSelect)
        {
            PermissionTypeId = permissionType.Id;
            Description = permissionType.Description;
            var entityPermission = entityPermissionToSelect.Where(slp => slp.PermissionTypeId == permissionType.Id).FirstOrDefault();
            if (entityPermission != null)
            {
                Selected = true;
            }
            else
            {
                Selected = false;
            }
        }
    }
}