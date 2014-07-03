using ShoppingLists.Web.Hubs;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using LightInject;
using Microsoft.AspNet.SignalR;
using ShoppingLists.Web.HubPipelineModules;
using Owin;

namespace ShoppingLists.Web
{
    public partial class Startup
    {
        public void ConfigureSignalr(IAppBuilder app)
        {
            //container.Register<ShoppingListHub>();
            //var resolver = new LightInjectSignalrDependencyResolver(container);

            //var config = new HubConfiguration() {
            //    Resolver = resolver
            //};

            var container = new ServiceContainer(); // SignalR needs it's own DI container as it doesn't use the per request scope that MVC uses.
            GlobalHost.HubPipeline.AddModule(new ErrorLoggingPipelineModule());
            GlobalHost.HubPipeline.AddModule(new ScriptDetectionPipelineModule());
            GlobalHost.HubPipeline.AddModule(new UnitOfWorkPipelineModule(container));
            //app.MapSignalR(new HubConfiguration { EnableDetailedErrors = true });
            app.MapSignalR();
            GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}