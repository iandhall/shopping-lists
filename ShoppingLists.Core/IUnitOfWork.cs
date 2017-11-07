using System;

namespace ShoppingLists.Core
{
    public interface IUnitOfWork : IDisposable
    {
        void Complete();
    }
}
