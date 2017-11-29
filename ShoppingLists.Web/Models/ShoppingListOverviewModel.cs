using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Web.Models
{
    public class ShoppingListOverviewModel : IEncodable
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Creator { get; set; }

        public ShoppingListOverviewModel()
        {
        }

        public ShoppingListOverviewModel(ShoppingList shoppingList)
        {
            Id = shoppingList.Id;
            Title = shoppingList.Title;
            Creator = shoppingList.Creator.Username;
            if (string.IsNullOrEmpty(Creator))
            {
                throw new ApplicationException("Creator can't be blank.");
            }
        }

        public void Encode()
        {
            Title = EncodingHelper.Encode(Title);
            Creator = EncodingHelper.Encode(Creator);
        }

        public override string ToString()
        {
            return string.Format("ShoppingListOverviewModel: Id={0}, Title={1}, Creator={2}", Id, Title, Creator);
        }
    }
}