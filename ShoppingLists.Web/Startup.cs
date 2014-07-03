using Microsoft.Owin;
using Owin;
using LogForMe;
using System.Web.Mvc;
using System;
using LightInject;
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
        public void Configuration(IAppBuilder app)
        {
            if (Path.GetDirectoryName(Logger.FileName).Length == 0)
            {
                Logger.FileName = string.Format("{0}..{1}Logs{1}{2}", HttpContext.Current.Server.MapPath("~"), Path.DirectorySeparatorChar, Logger.FileName);
            }
            Logger.Debug("DataDirectory={0}", AppDomain.CurrentDomain.GetData("DataDirectory"));
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
