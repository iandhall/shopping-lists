using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoppingLists.DataAccessLayer;
using ShoppingLists.Core.Entities;
using ShoppingLists.Core;
using ShoppingLists.BusinessLayer.Exceptions;

namespace ShoppingLists.BusinessLayer
{
    public class PermissionTypesService
    {
        private IUnitOfWork uow;
        private IPermissionTypeRepository repository;

        public PermissionTypesService(IUnitOfWork uow, IPermissionTypeRepository repository)
        {
            this.uow = uow;
            this.repository = repository;
        }

        public IEnumerable<PermissionType> GetAll()
        {
            var permissions = repository.GetAll();
            if (permissions.Count() == 0)
            {
                throw new NoEntitiesFoundException(typeof(PermissionType));
            }
            return permissions;
        }
    }
}
