using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingLists.Web.Hubs
{
    public class HubConnection
    {
        public string Id { get; set; }
        public string Group { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
    }
}