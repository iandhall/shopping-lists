using LightInject;

namespace ShoppingLists.Web
{
    interface IHasScope
    {
        Scope Scope { get; set; }
    }
}
