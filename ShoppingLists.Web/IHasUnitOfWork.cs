using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingLists.Core;

namespace ShoppingLists.Web
{
    interface IHasUnitOfWork
    {
        IUnitOfWork Uow { get; }
    }
}
