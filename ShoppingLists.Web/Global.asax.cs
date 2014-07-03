using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using LogForMe;

namespace ShoppingLists.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // See Owin Startup class.
        }

        protected void Application_Error(object sender, EventArgs args)
        {
            Exception ex = Server.GetLastError();
            if (ex is ThreadAbortException) return;
            Logger.Error(ex);
        }

        //protected void Application_BeginRequest(object sender, EventArgs args) {
        //    Logger.Debug(Request.Url.ToString());
        //}
    }
}
