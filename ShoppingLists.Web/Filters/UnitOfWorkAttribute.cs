using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
using ShoppingLists.Core;
using ShoppingLists.BusinessLayer.Exceptions;
using System.Net;

namespace ShoppingLists.Web.Filters
{
    public class UnitOfWorkAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // If there's no exception...
            if (filterContext.Exception == null)
            {
                // Only do this for Controllers that have a UnitOfWork.
                if (filterContext.Controller is IHasUnitOfWork)
                {
                    var controller = (IHasUnitOfWork)filterContext.Controller;
                    if (controller.Uow != null)
                    {
                        controller.Uow.Complete(); // Complete the UOW if one has been instantiated, i.e., if either the Action method or the entire Controller has the UnitOfWork attribute applied.
                    }
                }
            }
            else
            {
                // Determine the return type of the action
                string actionName = filterContext.RouteData.Values["action"].ToString();
                Type controllerType = filterContext.Controller.GetType();
                var methods = controllerType.GetMethods().Where(m =>
                    m.Name == actionName
                    && (
                        m.DeclaringType.CustomAttributes.Any(a => a.AttributeType == typeof(UnitOfWorkAttribute)) // The DeclaringType has the UnitOfWorkAttribute...
                        || m.CustomAttributes.Any(a => a.AttributeType == typeof(UnitOfWorkAttribute)) // ... or the method has the UnitOfWorkAttribute.
                    )
                ).ToList();
                if (methods.Count > 1)
                {
                    throw new MultipleActionMethodsException(actionName);
                }
                var returnType = methods.First().ReturnType;

                // If the action that generated the exception returns JSON
                if (returnType.Equals(typeof(JsonResult)))
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    filterContext.Result = new JsonResult()
                    {
                        Data = filterContext.Exception.GetType().Name
                    };
                    filterContext.ExceptionHandled = true;
                }
            }
            base.OnActionExecuted(filterContext);
        }
    }

    public class MultipleActionMethodsException : ApplicationException
    {
        public MultipleActionMethodsException(string actionName) : base(string.Format("Multiple methods with the name {0} and the UnitOfWorkAttribute have been detected.", actionName)) { }
    }
}