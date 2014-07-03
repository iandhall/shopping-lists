using System;
using ShoppingLists.Core.Entities;
using System.Collections.Generic;

namespace ShoppingLists.DataAccessLayer
{
    public interface IPermissionTypeRepository
    {
        IEnumerable<PermissionType> GetAll();
    }
}
