using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class EmptyStringException : ServiceValidationException
    {
        public EmptyStringException(string parameterName) : base(
            "Parameter {0} can't be empty or null.",
            parameterName
        ) { }
    }
}