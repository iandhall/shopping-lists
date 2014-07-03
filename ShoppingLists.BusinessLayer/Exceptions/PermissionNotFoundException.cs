using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Core;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class PermissionNotFoundException : ServiceValidationException
    {
        public PermissionNotFoundException(Permissions permission, string userId, long shoppingListId) : base(
            "User {0} doesn't have {1} permission for ShoppingList {2}.",
            userId,
            permission.ToString(),
            shoppingListId
        ) { }
    }
}