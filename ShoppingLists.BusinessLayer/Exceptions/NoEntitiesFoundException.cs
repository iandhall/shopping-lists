using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Shared.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class NoEntitiesFoundException : ServiceException
    {
        public NoEntitiesFoundException(Type entityType) : base(
            "No {0} entity records found in the corresponding database table.",
            entityType.Name
        ) { }
    }
}