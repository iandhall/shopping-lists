using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LightInject;
using Microsoft.AspNet.SignalR.Hubs;
using ShoppingLists.BusinessLayer;
using ShoppingLists.Core;
using ShoppingLists.Web.Hubs;
using LogForMe;
using System.Runtime.InteropServices;
using ShoppingLists.BusinessLayer.Exceptions;

namespace ShoppingLists.Web.HubPipelineModules
{
    // Creates UnitOfWork before each hub method gets called and competes it when the hub method finishes. If an exception occurs, it disposes the UOW causing the database transaction to get rolled back.
    //
    // Requirements:
    //      Hub must implement IHasUnitOfWork.
    //      Hub method must have UnitOfWorkAttribute.
    //      Hub type must be ShoppingListHub (won't be required after LightInject SignalR support).
    //      Hub implements IHasScope (won't be required after LightInject SignalR support).
    //
    // Note: Currently, this module is also reponsible for injecting dependencies into the hub. To do: Configure the SignalR dependency resolver to do this instead.
    //
    public class UnitOfWorkPipelineModule : HubPipelineModule
    {
        private ServiceContainer container;

        public UnitOfWorkPipelineModule(ServiceContainer container)
        {
            this.container = container;
        }

        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            Before(context.Hub, context.MethodDescriptor.Name);
            return base.OnBeforeIncoming(context);
        }

        protected override object OnAfterIncoming(object result, IHubIncomingInvokerContext context)
        {
            After(context.Hub, context.MethodDescriptor.Name);
            return base.OnAfterIncoming(result, context);
        }

        protected override bool OnBeforeConnect(IHub hub)
        {
            Before(hub, "OnConnected");
            return base.OnBeforeConnect(hub);
        }

        protected override void OnAfterConnect(IHub hub)
        {
            After(hub, "OnConnected");
            base.OnAfterConnect(hub);
        }

        protected override bool OnBeforeDisconnect(IHub hub)
        {
            Before(hub, "OnDisconnected");
            return base.OnBeforeDisconnect(hub);
        }

        protected override void OnAfterDisconnect(IHub hub)
        {
            After(hub, "OnDisconnected");
            base.OnAfterDisconnect(hub);
        }

        private void Before(IHub hub, string method)
        {
            if (
                hub is IHasScope
                && hub is ShoppingListHub
                && hub.GetType().GetMethod(method).IsDefined(typeof(UnitOfWorkAttribute), true)
            )
            {
                ((IHasScope)hub).Scope = container.BeginScope(); // Store the DI scope in the hub instance (Not ideal but necessary until LightInject gets SignalR support).
                container.InjectProperties((ShoppingListHub)hub); // Injects required services into the hub.
            }
        }

        private void After(IHub hub, string method)
        {
            if (
                hub is IHasUnitOfWork
                && hub is IHasScope
                && hub.GetType().GetMethod(method).IsDefined(typeof(UnitOfWorkAttribute), true)
            )
            {
                ((IHasUnitOfWork)hub).Uow.Complete(); // Complete the UnitOfWork, committing the transaction as no error should have occured in order to have reached this point.
                ((IHasScope)hub).Scope.Dispose(); // Disposes the current DI scope including the UnitOfWork.
            }
        }

        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            if (
                invokerContext.Hub is IHasScope
                && invokerContext.Hub.GetType().GetMethod(invokerContext.MethodDescriptor.Name).IsDefined(typeof(UnitOfWorkAttribute), true)
            )
            {
                ((IHasScope)invokerContext.Hub).Scope.Dispose(); // Disposes the current DI scope including the UnitOfWork, i.e. without completing the UOW so that the transaction gets rolled back.
            }
            invokerContext.Hub.Clients.Caller.serviceException(exceptionContext.Error.GetType().Name); // Report the error to the client.
            base.OnIncomingError(exceptionContext, invokerContext);
        }
    }
}