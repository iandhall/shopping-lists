using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
