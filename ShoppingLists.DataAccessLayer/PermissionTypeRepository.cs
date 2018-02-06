using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Shared.Entities;
using ShoppingLists.Shared.RepositoryInterfaces;

namespace ShoppingLists.DataAccessLayer
{
    public class PermissionTypeRepository : IPermissionTypeRepository
    {
        private ShoppingListsDbContext dbContext;

        public PermissionTypeRepository(ShoppingListsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<PermissionType> GetAll()
        {
            return dbContext.PermissionTypes.ToList();
        }
    }
}
