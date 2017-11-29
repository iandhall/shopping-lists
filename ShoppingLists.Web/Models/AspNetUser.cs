using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;
using Microsoft.AspNet.Identity;

namespace ShoppingLists.Web.Models
{
    public class AspNetUser : IUser
    {
        private string userId;
        public string Id
        {
            get { return userId; }
            set { userId = value; }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Discriminator { get; set; }

        // So the BLL and DAL don't need to reference the ASP.NET identity libraries.
        public User GetEntityUser()
        {
            return new User()
            {
                Id = this.Id,
                Username = this.UserName,
                PasswordHash = this.PasswordHash,
                SecurityStamp = this.SecurityStamp,
                Discriminator = this.Discriminator
            };
        }

        public static AspNetUser EntityToAspNetUser(User user)
        {
            if (user == null) return null;
            return new AspNetUser
            {
                Id = user.Id,
                UserName = user.Username,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                Discriminator = user.Discriminator
            };
        }
    }
}