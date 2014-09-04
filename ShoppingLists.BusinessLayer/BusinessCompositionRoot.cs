using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInject;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Core;
using ShoppingLists.Core.RepositoryInterfaces;
using ShoppingLists.BusinessLayer;
using System.Reflection;

[assembly: CompositionRootType(typeof(BusinessCompositionRoot))]

namespace ShoppingLists.BusinessLayer
{
    // Configure dependency injection for the Business Layer.
    public class BusinessCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<ShoppingListPermissionHelper>(new PerScopeLifetime());
            serviceRegistry.Register(typeof(Timestamper<>), new PerScopeLifetime());
            serviceRegistry.Register<ShoppingListService>(new PerScopeLifetime());
            serviceRegistry.Register<ListItemService>(new PerScopeLifetime());
            serviceRegistry.Register<UserService>(new PerScopeLifetime());
            serviceRegistry.Register<PermissionTypeService>(new PerScopeLifetime());

            serviceRegistry.RegisterAssembly(Assembly.GetAssembly(typeof(DataAccessCompositionRoot))); // LightInject needs to be told to scan ShoppingLists.DataAccessLayer for a comp. root.
        }
    }
}
