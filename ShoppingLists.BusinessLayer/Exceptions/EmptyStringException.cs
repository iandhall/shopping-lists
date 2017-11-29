using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class EmptyStringException : ServiceException
    {
        public EmptyStringException(string parameterName) : base(
            "Parameter {0} can't be empty, null or contain only whitespace.",
            parameterName
        ) { }
    }
}