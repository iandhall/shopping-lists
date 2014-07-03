using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInject;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core.RepositoryInterfaces;
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
            serviceRegistry.Register<IShoppingListRepository, ShoppingListRepository>(new PerScopeLifetime());
            serviceRegistry.Register<ICrudRepository<ShoppingList>>(f => f.GetInstance<IShoppingListRepository>());
            serviceRegistry.Register<IListItemRepository, ListItemRepository>(new PerScopeLifetime());
            serviceRegistry.Register<ICrudRepository<ListItem>>(f => f.GetInstance<IListItemRepository>());
            serviceRegistry.Register<IShoppingListPermissionRepository, ShoppingListPermissionRepository>(new PerScopeLifetime());
            serviceRegistry.Register<ICrudRepository<ShoppingListPermission>>(f => f.GetInstance<IShoppingListPermissionRepository>());
            serviceRegistry.Register<IUserRepository, UserRepository>(new PerScopeLifetime());
            serviceRegistry.Register<IPermissionTypeRepository, PermissionTypeRepository>(new PerScopeLifetime());
        }
    }
}
