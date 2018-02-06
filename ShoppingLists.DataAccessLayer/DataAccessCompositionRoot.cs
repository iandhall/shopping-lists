using LightInject;
using ShoppingLists.Shared;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Shared.RepositoryInterfaces;

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
            serviceRegistry.Register<IShoppingListRepository, ShoppingListRepository>(new PerScopeLifetime());
            serviceRegistry.Register<IListItemRepository, ListItemRepository>(new PerScopeLifetime());
            serviceRegistry.Register<IPermissionRepository, PermissionRepository>(new PerScopeLifetime());
            serviceRegistry.Register<IUserRepository, UserRepository>(new PerScopeLifetime());
            serviceRegistry.Register<IPermissionTypeRepository, PermissionTypeRepository>(new PerScopeLifetime());
        }
    }
}
