using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.Web.Models
{
    public class ShoppingListSharingModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<UserSharingModel> Users { get; set; }

        public ShoppingListSharingModel(ShoppingList shoppingList, IEnumerable<User> users, string userId)
        {
            Id = shoppingList.Id;
            Title = shoppingList.Title;
            Users = users
                .Where(u => u.Id != userId && u.Id != shoppingList.CreatorId) // Exclude the current user and the shopping list owner from the list.
                .Select(u => new UserSharingModel(u)).ToList();
        }

        public override string ToString()
        {
            return string.Format("ShoppingListSharingModel: Id={0}, Title={1}", Id, Title);
        }
    }
}