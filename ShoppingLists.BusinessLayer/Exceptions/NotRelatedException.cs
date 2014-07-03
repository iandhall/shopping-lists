using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingLists.Core.Entities;

namespace ShoppingLists.BusinessLayer.Exceptions
{
    public class NotRelatedException : ServiceValidationException
    {
        public NotRelatedException(Type parentEntityType, long parentId, Type childEntityType, long childId) : base(
            "{0} {1} is not related to {2} {3}.",
            childEntityType.Name,
            childId,
            parentEntityType.Name,
            parentId
        ) { }
    }
}