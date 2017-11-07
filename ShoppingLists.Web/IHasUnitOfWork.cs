using ShoppingLists.Core;

namespace ShoppingLists.Web
{
    interface IHasUnitOfWork
    {
        IUnitOfWork Uow { get; }
    }
}
