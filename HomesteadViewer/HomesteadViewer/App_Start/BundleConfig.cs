using System.Web;
using System.Web.Optimization;

namespace HomesteadViewer
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            bundles.Add(new StyleBundle("~/Content/kendo/css")
                .Include("~/Content/kendo/kendo.common.min.css")
                .Include("~/Content/kendo/kendo.common-bootstrap.min.css")
                .Include("~/Content/kendo/kendo.bootstrap.min.css")
                .Include("~/Content/kendo/kendo.dataviz.min.css")
                .Include("~/Content/kendo/kendo.dataviz.metro.min.css")
                .Include("~/Content/plugins/pines-notify/jquery.pnotify.default.css")
            );

            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                            "~/Scripts/jquery.min.js",
                            "~/Scripts/angular.min.js",
                            "~/Scripts/bootstrap.min.js",
                            "~/Scripts/enquire.js",
                            "~/Scripts/jquery.cookie.js",
                            "~/Scripts/jquery.nicescroll.min.js",
                            "~/Scripts/kendo.all.min.js",
                            "~/Scripts/kendo.aspnetmvc.min.js",
                            "~/Scripts/application.js",
                            "~/Scripts/ZeroClipboard.min.js",
                            "~/Scripts/moment.min.js",
                            "~/Scripts/Site.js",
                            "~/Scripts/plugins/pines-notify/jquery.pnotify.min.js"));


            BundleTable.EnableOptimizations = true;
        }
    }
}
