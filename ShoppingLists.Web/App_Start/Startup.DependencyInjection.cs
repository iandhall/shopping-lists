using LightInject;
using Microsoft.AspNet.Identity;
using ShoppingLists.Core;
using ShoppingLists.Web.Models;

namespace ShoppingLists.Web
{
    public partial class Startup
    {

        public ServiceContainer ConfigureDependencyInjection()
        {
            var container = new ServiceContainer();

            // Enable scopes across the logical CallContext. This allows Lightinject to work for async calls.
            container.ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider();

            container.RegisterControllers();

            // Web:
            container.Register(typeof(UserManager<>), new PerScopeLifetime());
            container.Register<IUserStore<AspNetUser>, UserStore>(new PerScopeLifetime());
            container.Register<IUserContext, UserContext>(new PerScopeLifetime());

            // Business and Data Layers have classes which implement ICompositionRoot and set up their own dependencies.

            container.EnableMvc();
            return container;
        }
    }
}