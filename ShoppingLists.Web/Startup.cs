using Microsoft.Owin;
using Owin;
using NLog;
using System.Web.Mvc;
using System;
using System.Web.Routing;
using System.Web.Optimization;
using System.Web;
using System.IO;
using ShoppingLists.BusinessLayer;

[assembly: OwinStartupAttribute(typeof(ShoppingLists.Web.Startup))]

namespace ShoppingLists.Web
{
    public partial class Startup
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public void Configuration(IAppBuilder app)
        {
            //if (Path.GetDirectoryName(Logger.FileName).Length == 0)
            //{
            //    Logger.FileName = string.Format("{0}..{1}Logs{1}{2}", HttpContext.Current.Server.MapPath("~"), Path.DirectorySeparatorChar, Logger.FileName);
            //}
            _log.Info("DataDirectory={0}", AppDomain.CurrentDomain.GetData("DataDirectory"));
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ConfigureModelBinders();
            ConfigureAuth(app);
            var container = ConfigureDependencyInjection();
            BusinessStartup.Initialise();
            ConfigureSignalr(app);
        }
    }
}
