using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingLists.Core
{
    public interface IUnitOfWork : IDisposable
    {
        void Complete();
    }
}
