using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions 
{
    public class UserNotFoundException : ServiceValidationException
    {
        public UserNotFoundException(string userId) : base(
            "User with Id {0} not found.",
            userId
        ) { }
    }
}