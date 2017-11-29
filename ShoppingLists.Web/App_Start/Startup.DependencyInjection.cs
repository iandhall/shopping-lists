using System.Web.Http;
using LightInject;
using Microsoft.AspNet.Identity;
using ShoppingLists.Shared;
using ShoppingLists.Web.Models;

namespace ShoppingLists.Web
{
    public partial class Startup
    {

        public ServiceContainer ConfigureDependencyInjection()
        {
            var container = new ServiceContainer();
            container.ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider(); // Enable scopes across the logical CallContext. This allows Lightinject to work for async calls.

            container.EnablePerWebRequestScope();
            container.RegisterApiControllers();
            container.EnableWebApi(GlobalConfiguration.Configuration);
            container.EnableMvc();

            // Web:
            container.RegisterControllers();
            container.Register(typeof(UserManager<>), new PerScopeLifetime());
            container.Register<IUserStore<AspNetUser>, UserStore>(new PerScopeLifetime());
            container.Register<IUserContext, UserContext>(new PerScopeLifetime());

            // Business and Data Layers have classes which implement ICompositionRoot and set up their own dependencies.

            return container;
        }
    }
}