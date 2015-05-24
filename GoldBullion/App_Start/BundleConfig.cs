using System.Web;
using System.Web.Optimization;

namespace GoldBullion
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
            

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-datepicker.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/jqplot").Include(
                      "~/Scripts/jquery.jqplot.js",
                      "~/Scripts/jqplot.highlighter.min.js",
                      "~/Scripts/jqplot.cursor.min.js",
                      "~/Scripts/jqplot.dateAxisRenderer.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/datepicker.css",
                       "~/Content/jquery.jqplot.css",
                       "~/Content/Template.css"
                      ));
           
        }
    }
}
