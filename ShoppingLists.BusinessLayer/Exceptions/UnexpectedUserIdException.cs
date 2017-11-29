using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class UnexpectedUserIdException : ServiceException
    {
        public UnexpectedUserIdException(string expectedUserId, string unexpectedUserId) : base(
            "Unexpected userId detected: {0}. Expected {1}.",
            expectedUserId,
            unexpectedUserId
        ) { }
    }
}