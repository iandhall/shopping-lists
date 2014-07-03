using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Core;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class PermissionAlreadyExistsException : ServiceValidationException
    {
        public PermissionAlreadyExistsException(Permissions permission, string userId, long shoppingListId) : base(
            "Permission {0} already exists for user {1} on ShoppingList {2}.",
            permission.ToString(),
            userId,
            shoppingListId
        ) { }
    }
}