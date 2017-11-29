using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class ShareWithListCreatorException : ServiceException
    {
        public ShareWithListCreatorException(string userId) : base(
            "Can't share ShoppingList with list creator {0}.",
            userId
        ) { }
    }
}