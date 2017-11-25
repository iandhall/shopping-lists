using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using NLog;

namespace ShoppingLists.Web
{
    public class ValidateApiModelAttribute : ActionFilterAttribute
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private bool _checkNull;

        public ValidateApiModelAttribute(bool checkNull = true)
        {
            _checkNull = checkNull;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (_checkNull && actionContext.ActionArguments.ContainsValue(null))
            {
                throw new ApplicationException(
                    string.Format("Model is null {0}.", GetActionMethodName(actionContext))
                );
            }

            if (!actionContext.ModelState.IsValid)
            {
                throw new ApplicationException(
                    string.Format("Invalid model {0}:{1}", GetActionMethodName(actionContext), Environment.NewLine + "\t")
                    + string.Join(Environment.NewLine + "\t", actionContext.ModelState.Values.SelectMany(v => v.Errors).Select(GetModelErrorMessage))
                );
            }

            base.OnActionExecuting(actionContext);
        }

        private string GetActionMethodName(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.ControllerDescriptor.ControllerType.Name
                + "." + actionContext.ActionDescriptor.ActionName + "()";
        }

        private string GetModelErrorMessage(ModelError modelError)
        {
            return modelError.ErrorMessage != null
                ? modelError.ErrorMessage
                : modelError.Exception.Message;
        }
    }
}