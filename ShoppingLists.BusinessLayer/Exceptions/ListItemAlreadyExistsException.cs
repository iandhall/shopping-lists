﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class ListItemAlreadyExistsException : ServiceException
    {
        public ListItemAlreadyExistsException(string listItemDescription, long shoppingListId) : base(
            "ListItem '{0}' already exists for ShoppingList {1}.",
            listItemDescription,
            shoppingListId
        ) { }
    }
}