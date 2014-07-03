using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using LogForMe;

namespace ShoppingLists.Web.HubPipelineModules
{
    public class ErrorLoggingPipelineModule : HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            Logger.Debug("ErrorLoggingPipelineModule.OnIncomingError");
            Logger.Error(exceptionContext.Error);
            base.OnIncomingError(exceptionContext, invokerContext);
        }
    }
}