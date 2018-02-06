using LightInject;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.BusinessLayer;
using System.Reflection;
using ShoppingLists.Shared.ServiceInterfaces;

[assembly: CompositionRootType(typeof(BusinessCompositionRoot))]

namespace ShoppingLists.BusinessLayer
{
    // Configure dependency injection for the Business Layer.
    public class BusinessCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IPermissionService, PermissionService>(new PerScopeLifetime());
            serviceRegistry.Register<IShoppingListService, ShoppingListService>(new PerScopeLifetime());
            serviceRegistry.Register<IListItemService, ListItemService>(new PerScopeLifetime());
            serviceRegistry.Register<IUserService, UserService>(new PerScopeLifetime());
            serviceRegistry.Register<IPermissionTypeService, PermissionTypeService>(new PerScopeLifetime());

            serviceRegistry.RegisterAssembly(Assembly.GetAssembly(typeof(DataAccessCompositionRoot))); // LightInject needs to be told to scan ShoppingLists.DataAccessLayer for a comp. root.
        }
    }
}
