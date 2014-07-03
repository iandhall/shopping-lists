using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Core;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.Web.Models
{
    public class ListItemModel : IEncodable
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public long ShoppingListId { get; set; }
        public Statuses StatusId { get; set; }

        public ListItemModel()
        {
        }

        public ListItemModel(ListItem entity)
        {
            Id = entity.Id;
            Description = entity.Description;
            Quantity = entity.Quantity;
            ShoppingListId = entity.ShoppingListId;
            StatusId = entity.StatusId;
        }

        public void Encode()
        {
            Description = EncodingHelper.Encode(Description);
        }

        public override string ToString()
        {
            return string.Format("ListItemModel: Id={0}, Description={1}, Quantity={2}, ShoppingListId={3}, StatusId={4}", Id, Description, Quantity, ShoppingListId, StatusId);
        }
    }
}