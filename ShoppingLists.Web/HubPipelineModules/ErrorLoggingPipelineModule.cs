using Microsoft.AspNet.SignalR.Hubs;
using NLog;

namespace ShoppingLists.Web.HubPipelineModules
{
    public class ErrorLoggingPipelineModule : HubPipelineModule
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            _log.Debug("ErrorLoggingPipelineModule.OnIncomingError");
            _log.Error(exceptionContext.Error);
            base.OnIncomingError(exceptionContext, invokerContext);
        }
    }
}