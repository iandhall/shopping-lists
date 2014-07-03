using ShoppingLists.Web.CustomModelBinders;
using System.Web.Mvc;
using System;

namespace ShoppingLists.Web
{
    public partial class Startup
    {
        public void ConfigureModelBinders()
        {
            var binder = new DateTimeModelBinder();
            ModelBinders.Binders.Add(typeof(DateTime), binder);
            ModelBinders.Binders.Add(typeof(DateTime?), binder);
        }
    }
}