using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingLists.Web.Hubs
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : Attribute
    {
    }
}