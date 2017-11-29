using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class ShoppingListTitleDuplicateException : ServiceException
    {
        public ShoppingListTitleDuplicateException(string title, string userId) : base(
            "ShoppingList already exists with the title '{0}' for user {1}.",
            title,
            userId
        ) { }
    }
}