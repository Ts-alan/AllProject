using System;
using System.Web.Optimization;

namespace CCP
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular/angular.js",
                        "~/Scripts/angular/angular-route.js",
                        "~/Scripts/angular/angular-resource.js"));

            bundles.Add(new ScriptBundle("~/bundles/ng-grid").Include("~/Scripts/ngGrid/ng-grid.*"));

            bundles.Add(new ScriptBundle("~/bundles/ui-bootstrap").Include("~/Scripts/angular-ui/ui-bootstrap-tpls.*"));

            bundles.Add(new ScriptBundle("~/bundles/applicationCtrl").Include("~/AngularApp/Controllers/applicationCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/contractsCtrl").Include("~/AngularApp/Controllers/CPRs/contractsCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/contractCtrl").Include("~/AngularApp/Controllers/CPRs/contractCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/usersCtrl").Include("~/AngularApp/Controllers/DataAdmin/usersCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/salesPersonsCtrl").Include("~/AngularApp/Controllers/DataAdmin/salesPersonsCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/errorCtrl").Include("~/AngularApp/Controllers/Dialogs/errorCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/confirmCtrl").Include("~/AngularApp/Controllers/Dialogs/confirmCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataAdminCtrl").Include("~/AngularApp/Controllers/DataAdmin/dataAdminCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/userCtrl").Include("~/AngularApp/Controllers/DataAdmin/userCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/salesPersonCtrl").Include("~/AngularApp/Controllers/DataAdmin/salesPersonCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/approvalCtrl").Include("~/AngularApp/Controllers/ApprovalDashboard/approvalCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/myCPRs").Include("~/AngularApp/Controllers/MyCPRs/myContractsCtrl.js"));

            bundles.Add(
                new ScriptBundle("~/bundles/serverErrorDirective").Include(
                    "~/AngularApp/Directives/serverErrorDirective.js"));

            bundles.Add(new ScriptBundle("~/bundles/ng-services").IncludeDirectory("~/AngularApp/Services/", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/ui-router").Include("~/Scripts/angular-ui-router.*"));

            bundles.Add(new ScriptBundle("~/bundles/tabCtrl").Include("~/AngularApp/Controllers/tabCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/appSettings").Include("~/AngularApp/AppSettings.js"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include("~/Scripts/select2/select2.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-ui").Include("~/Scripts/angular-ui.js",
                "~/Scripts/angular-ui-ieshiv.js"));

            bundles.Add(new ScriptBundle("~/bundles/loading-bar").Include("~/Scripts/loading-bar.js"));

            bundles.Add(new ScriptBundle("~/bundles/localStorage").Include("~/Scripts/angular-local-storage.js"));

            bundles.Add(new ScriptBundle("~/bundles/authCtrl").Include("~/AngularApp/Controllers/authCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/less").Include("~/Scripts/less-1.5.1.js"));

            bundles.Add(new StyleBundle("~/Style/css").Include(
                //"~/Content/Style.less",
                "~/Content/bootstrap/bootstrap.css",
                "~/Content/ngGrid/ng-grid.css",
                "~/Content/loading-bar.css"));

            bundles.Add(new StyleBundle("~/Content/select2/css").Include("~/Content/select2/select2.css"));

            BundleTable.EnableOptimizations = true;
            foreach (var bundle in BundleTable.Bundles)
            {
                bundle.Transforms.Clear();
            }
        }
    }
}