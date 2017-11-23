using System;
using LightInject;
using Microsoft.AspNet.SignalR;
using Owin;
using ShoppingLists.Web.HubPipelineModules;
using ShoppingLists.Web.Hubs;

namespace ShoppingLists.Web
{
    public partial class Startup
    {
        public void ConfigureSignalr(IAppBuilder app, ServiceContainer container)
        {
            container.Register<ShoppingListHub>(new PerScopeLifetime());
            GlobalHost.DependencyResolver.Register(typeof(ShoppingListHub), () =>
            {
                ShoppingListHub hub = null;
                try
                {
                    hub = container.Create<ShoppingListHub>();
                }
                catch (NullReferenceException ex)
                {
                    // Intentional: Ignore this exception
                }
                return hub;
            });

            GlobalHost.HubPipeline.AddModule(new ErrorLoggingPipelineModule());
            GlobalHost.HubPipeline.AddModule(new ScriptDetectionPipelineModule());

            app.MapSignalR();
            GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}