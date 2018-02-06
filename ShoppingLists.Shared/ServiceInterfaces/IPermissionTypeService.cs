using System.Collections.Generic;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Shared.ServiceInterfaces
{
    public interface IPermissionTypeService
    {
        IEnumerable<PermissionType> GetAll();
    }
}