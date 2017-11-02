using System;
using System.Threading;
using NLog;

namespace ShoppingLists.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            // See Owin Startup class.
        }

        protected void Application_Error(object sender, EventArgs args)
        {
            Exception ex = Server.GetLastError();
            if (ex is ThreadAbortException) return;
            _log.Error(ex);
        }

        //protected void Application_BeginRequest(object sender, EventArgs args) {
        //    _log.Debug(Request.Url.ToString());
        //}
    }
}
