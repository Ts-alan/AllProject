using System;
using System.Diagnostics;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Verst.Controllers;
using Verst.Models;
using Plants = MvcApplication.Models.Plants;
using NLog;

namespace Verst
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
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BatchWatcherService.GetInstance();

#if DEBUG

#else
                var proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = @"C:\Iis_start.bat";
                proc.Start();
            #endif
            //proc.WaitForExit();
        }

       /* protected void Application_Error()
        {
            //Пока работаем в дебаге
            if (HttpContext.Current.IsDebuggingEnabled && WebConfigurationManager.AppSettings["EnableCustomErrorPage"].Equals("false"))
            {
                LogManager.GetCurrentClassLogger().Error("Unhandled exception: ", Server.GetLastError());
            }
            else
            {
                try
                {
                    var exception = Server.GetLastError();
                    var httpException = exception as HttpException;
                    Response.Clear();
                    Server.ClearError();
                    var routeData = new RouteData();
                    routeData.Values["controller"] = "Errors";
                    routeData.Values["action"] = "General";
                    routeData.Values["exception"] = exception;

                    Response.StatusCode = 500;
                    if (httpException != null)
                    {
                        Response.StatusCode = httpException.GetHttpCode();
                        switch (Response.StatusCode)
                        {
                            case 403:
                                routeData.Values["action"] = "Http403";
                                break;
                            case 404:
                                routeData.Values["action"] = "Http404";
                                break;
                        }
                    }


                    IController errorsController = new ErrorController();
                    var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
                    errorsController.Execute(rc);
                }
                catch (Exception ex)
                {
                    //Если вызов контроллера не удаётся - отображаем статическую страницу
                    LogManager.GetCurrentClassLogger().Fatal("failed to display error page, fallback to HTML error: ", ex);
                    Response.TransmitFile("~/StError.html");
                    throw;
                }
                
            }
        }*/
    }
}