using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class PermissionAlreadyExistsException : ServiceException
    {
        public PermissionAlreadyExistsException(Permissions permission, string userId, long shoppingListId) : base(
            "Permission {0} already exists for user {1} on ShoppingList {2}.",
            permission.ToString(),
            userId,
            shoppingListId
        ) { }
    }
}