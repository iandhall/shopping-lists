using System.Collections.Generic;
using System.Linq;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.DataAccessLayer
{
    public class PermissionTypeRepository
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
