using LightInject;
using ShoppingLists.Core;
using ShoppingLists.DataAccessLayer;

[assembly: CompositionRootType(typeof(DataAccessCompositionRoot))]

namespace ShoppingLists.DataAccessLayer
{
    // Configure dependency injection for the Data Access Layer.
    public class DataAccessCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<ShoppingListsDbContext>(new PerScopeLifetime());
            serviceRegistry.Register<IUnitOfWork, EfUnitOfWork>(new PerScopeLifetime());
            serviceRegistry.Register<ShoppingListRepository>(new PerScopeLifetime());
            serviceRegistry.Register<ListItemRepository>(new PerScopeLifetime());
            serviceRegistry.Register<ShoppingListPermissionRepository>(new PerScopeLifetime());
            serviceRegistry.Register<UserRepository>(new PerScopeLifetime());
            serviceRegistry.Register<PermissionTypeRepository>(new PerScopeLifetime());
        }
    }
}
