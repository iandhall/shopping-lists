using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core.RepositoryInterfaces;

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
