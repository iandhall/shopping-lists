using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInject;

namespace ShoppingLists.Web
{
    interface IHasScope
    {
        Scope Scope { get; set; }
    }
}
