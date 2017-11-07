using Microsoft.AspNet.Identity;
using ShoppingLists.Web.Models;
using LightInject;

namespace ShoppingLists.Web
{
    public partial class Startup
    {

        public ServiceContainer ConfigureDependencyInjection()
        {
            var container = new ServiceContainer();
            container.RegisterControllers();

            // Register other services:

            // Business and Data Layers:
            // ...have classes which implement ICompositionRoot and set up their own dependencies.

            // Web:
            container.Register(typeof(UserManager<>));
            container.Register<IUserStore<AspNetUser>, UserStore>();

            container.EnableMvc();
            return container;
        }
    }
}