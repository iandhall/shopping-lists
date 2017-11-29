using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class EntityNotFoundException : ServiceException
    {
        public EntityNotFoundException(Type entityType, long entityId) : base(
            "{0} with Id {1} not found.",
            entityType.Name,
            entityId
        ) { }
    }
}