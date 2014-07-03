using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public abstract class ServiceValidationException : ApplicationException
    {
        public ServiceValidationException(string message, params object[] args) : base(string.Format(message, args))
        {
        }
    }
}