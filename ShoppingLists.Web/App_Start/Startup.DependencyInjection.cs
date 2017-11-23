﻿using LightInject;
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
            container.ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider(); // Enable scopes across the logical CallContext. This allows Lightinject to work for async calls.
            container.EnableMvc();
            container.EnablePerWebRequestScope();

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