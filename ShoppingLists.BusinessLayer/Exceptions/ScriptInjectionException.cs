using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class ScriptInjectionException : ServiceValidationException
    {
        public ScriptInjectionException(object arg) : base(
            "Potential script injection detected for Hub method argument: {0}",
            arg
        ) { }
    }
}