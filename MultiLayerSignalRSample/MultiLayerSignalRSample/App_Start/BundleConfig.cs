using System.Web.Optimization;

namespace MultiLayerSignalRSample
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = false;

            // .debug.js, -vsdoc.js and .intellisense.js files 
            // are in BundleTable.Bundles.IgnoreList by default.
            // Clear out the list and add back the ones we want to ignore.
            // Don't add back .debug.js.
            bundles.IgnoreList.Clear();
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*intellisense.js");

            // Modernizr goes separate since it loads first
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/modernizr-{version}.js"));

            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery",
                "//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js")
                .Include("~/Scripts/jquery-{version}.js"));

            // 3rd Party JavaScript files
            bundles.Add(new ScriptBundle("~/bundles/jsextlibs")
                .Include(
                    "~/Scripts/json2.js", // IE7 needs this

                    // SignalR plugins
                    "~/Scripts/jquery.signalR-1.1.0-beta1.js",

                    // jQuery plugins
                    "~/Scripts/TrafficCop.js",
                    "~/Scripts/infuser.js", // depends on TrafficCop

                    // Knockout and its plugins
                    "~/Scripts/knockout-{version}.debug.js",
                    "~/Scripts/koExternalTemplateEngine.js",

                    // Other 3rd party libraries
                    "~/Scripts/underscore.js",
                    "~/Scripts/moment.js",
                    "~/Scripts/sammy-{version}.js",
                    "~/Scripts/lib/amplify.js",
                    "~/Scripts/toastr.js"
                ));

            // All application JS files (except mocks)
            bundles.Add(new ScriptBundle("~/bundles/jsapplibs")
                .IncludeDirectory("~/Scripts/app/", "*.js", searchSubdirectories: false));

            // All CSS files
            bundles.Add(
              new StyleBundle("~/Content/css")
                .Include("~/Content/ie10mobile.css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/bootstrap-responsive.css")
                .Include("~/Content/font-awesome.css")
                .Include("~/Content/toastr.css")
              );
        }
    }
}