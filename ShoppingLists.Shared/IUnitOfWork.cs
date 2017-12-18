using System;

namespace ShoppingLists.Shared
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
