using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Web.Models
{
    public class UserSharingModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public UserSharingModel()
        {
        }

        public UserSharingModel(User user)
        {
            Id = user.Id;
            UserName = user.Username;
        }
    }
}