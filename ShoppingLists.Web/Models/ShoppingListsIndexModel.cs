using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.Web.Models
{
    public class ShoppingListsIndexModel
    {
        public IEnumerable<ShoppingListOverviewModel> MyLists { get; set; }
        public IEnumerable<ShoppingListOverviewModel> ListsSharedWithMe { get; set; }

        public ShoppingListsIndexModel(IEnumerable<ShoppingList> allListsThatIHaveAccessTo, string userId)
        {
            MyLists = allListsThatIHaveAccessTo.Where(l => l.CreatorId == userId).Select(l => new ShoppingListOverviewModel(l));
            ListsSharedWithMe = allListsThatIHaveAccessTo.Where(l => l.CreatorId != userId).Select(l => new ShoppingListOverviewModel(l));
        }
    }
}