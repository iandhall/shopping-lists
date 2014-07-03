using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;
using LogForMe;

namespace ShoppingLists.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootbox.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            var bundle = new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.mapping-latest.js",
                "~/Scripts/knockout.validation.js"
            );
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                      "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                      "~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/ajaxhelper").Include(
                      "~/Scripts/ajax-helper.js"));

            bundles.Add(new ScriptBundle("~/bundles/shoppinglistshow").Include(
                      "~/Scripts/ShoppingList/Show/ShoppingListModel.js",
                      "~/Scripts/ShoppingList/Show/ListItemModel.js",
                      "~/Scripts/ShoppingList/Show/ListItemEditModel.js",
                      "~/Scripts/ShoppingList/Show/ShoppingListHub.js"));

            bundles.Add(new ScriptBundle("~/bundles/shoppinglistindex").Include(
                      "~/Scripts/ShoppingList/Index/MyShoppingListsModel.js",
                      "~/Scripts/ShoppingList/Index/ShoppingListOverviewModel.js"));

            bundles.Add(new ScriptBundle("~/bundles/shoppinglistshare").Include(
                      "~/Scripts/ShoppingList/Share/SharingModel.js",
                      "~/Scripts/ShoppingList/Share/UserModel.js",
                      "~/Scripts/ShoppingList/Share/UserPermissionsEditModel.js",
                      "~/Scripts/ShoppingList/Share/PermissionModel.js"));

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }

    public class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}
