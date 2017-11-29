using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Web.Models
{
    public class ShoppingListModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public ICollection<ListItemModel> ListItems { get; set; }
        public IEnumerable<int> CurrentUserPermissions { get; set; }
        public string CurrentUsername { get; set; }

        public ShoppingListModel()
        {
        }

        public ShoppingListModel(ShoppingList entity, User currentUser)
        {
            Id = entity.Id;
            Title = entity.Title;
            CurrentUsername = currentUser.Username;
            if (entity.ListItems != null)
            {
                ListItems = entity.ListItems.Select(li => new ListItemModel(li)).ToList();
            }
            CurrentUserPermissions = currentUser.ShoppingListPermissions.Select(slp => (int)slp.PermissionTypeId).ToList();
        }

        public override string ToString()
        {
            return string.Format("ShoppingListModel: Id={0}, Title={1}", Id, Title);
        }
    }
}