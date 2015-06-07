using System;
using System.Diagnostics;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Crystal.BusinessLogic;
using Crystal.Controllers;
using Crystal.Models;
using NLog;
using Crystal.Authentication;
using Authentication.Membership;
using Authentication.WCF;

namespace Crystal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "BatchInfo", // Route name
                "{plant}/Facility/PartInfo/{nprt}", // URL with parameters
                new { controller = "Facility", action = "PartInfo", nprt = UrlParameter.Optional } // Parameter defaults
                , new { plant = string.Join("|", typeof(Plants).GetEnumNames()) }
            );

            routes.MapRoute(
                "Specialized", // Route name
                "{plant}/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                , new { plant = string.Join("|", typeof(Plants).GetEnumNames()) }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            //"Красивый" путь для пользовательского кабинета
            /*routes.MapRoute(
                "Users", // Route name
                "{controller}/{action}/{username}", // URL with parameters
                new { controller = "User", action = "Cabinet", id = UrlParameter.Optional }
            );*/

            routes.MapRoute(
              "Root",
              "",
              new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }



        protected void Application_Start()
        {
#if DEBUG
            AuthService.mrpService = new MRPServiceWCF();
#else
            AuthService.mrpService = new MRPServiceBase();
#endif
            //AuthService.mrpService = new MRPServiceBase();
            GlobalDiagnosticsContext.Set("logDirectory", System.Configuration.ConfigurationManager.AppSettings["logDirectory"]);
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            LogManager.GetCurrentClassLogger().Info("APPLICATION STARTED");

#if DEBUG

#else
            //var proc = new System.Diagnostics.Process();
            //proc.StartInfo.FileName = System.Configuration.ConfigurationManager.AppSettings["NovellConnection"];
            //proc.Start();//  as string);
            EFContext.ContextHelper.Open();
            NovellUtils.NetWare.ConnectToNovell();
#endif

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                //get the username which we previously set in
                //forms authentication ticket in our login1_authenticate event
                //build a custom identity and custom principal object based on this username
                //CustomIdentitiy identity = new CustomIdentitiy(loggedUser);
                //CustomPrincipal principal = new CustomPrincipal(identity);

                //set the principal to the current context
                HttpContext.Current.User = AuthService.GetPrincipal(HttpContext.Current.User);
                //LogManager.GetCurrentClassLogger().Info(string.Format("{0}-{1}", HttpContext.Current.User.Identity.Name, HttpContext.Current.User.GetHashCode()));
            }
        }
    }
}