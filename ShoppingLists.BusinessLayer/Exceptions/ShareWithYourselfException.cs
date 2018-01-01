using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class ShareWithYourselfException : ServiceException
    {
        public ShareWithYourselfException(string userId) : base(
            "Can't share ShoppingList with list yourself {0}.",
            userId
        ) { }
    }
}