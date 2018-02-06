using System.Collections.Generic;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.Shared.RepositoryInterfaces
{
    public interface IPermissionTypeRepository
    {
        IEnumerable<PermissionType> GetAll();
    }
}