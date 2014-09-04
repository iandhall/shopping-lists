using System;
using ShoppingLists.Core.Entities;
using System.Collections.Generic;

namespace ShoppingLists.Core.RepositoryInterfaces
{
    public interface IPermissionTypeRepository
    {
        IEnumerable<PermissionType> GetAll();
    }
}
