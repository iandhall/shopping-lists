using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class UnexpectedUserIdException : ServiceValidationException
    {
        public UnexpectedUserIdException(string expectedUserId, string unexpectedUserId) : base(
            "Unexpected userId detected: {0}. Expected {1}.",
            expectedUserId,
            unexpectedUserId
        ) { }
    }
}