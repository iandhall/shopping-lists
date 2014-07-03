using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace ShoppingLists.Web.CustomModelBinders
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value.AttemptedValue == "" || value.AttemptedValue == "null") return null;
            return new DateTime(long.Parse(value.AttemptedValue), DateTimeKind.Utc);
        }
    }
}