using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class ShareWithYourselfException : ServiceValidationException
    {
        public ShareWithYourselfException(string userId) : base(
            "Can't share ShoppingList with list creator {0}.",
            userId
        ) { }
    }
}