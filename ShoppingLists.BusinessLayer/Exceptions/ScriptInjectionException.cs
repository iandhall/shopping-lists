using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class ScriptInjectionException : ServiceException
    {
        public ScriptInjectionException(object arg) : base(
            "Potential script injection detected for Hub method argument: {0}",
            arg
        ) { }
    }
}