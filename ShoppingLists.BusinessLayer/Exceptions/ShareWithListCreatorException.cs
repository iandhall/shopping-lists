using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class ShareWithListCreatorException : ServiceValidationException
    {
        public ShareWithListCreatorException(string userId) : base(
            "Can't share ShoppingList with list creator {0}.",
            userId
        ) { }
    }
}