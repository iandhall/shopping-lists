using System;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public abstract class ServiceException : ApplicationException
    {
        protected string UserMessage;

        public ServiceException(string message, params object[] args) : base(string.Format(message, args)) { }

        public ServiceException(string userMessage, string message, params object[] args) : base(string.Format(message, args))
        {
            UserMessage = userMessage;
        }
    }
}