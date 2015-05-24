using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CCP.Controllers;
using CCP.Filters;

namespace CCP
{

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                GlobalFilters.Filters.Add(new IsAngularRouteAttribute());
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }


        //protected void Application_BeginRequest(Object sender, EventArgs e)
        //{
        //     var controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"];
        //     var controllerAction = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
        //     if (controllerName.Equals("StartController")) return;
        //     //if (controllerName.Equals("UserController"&& ))
        //     //if (!filterContext.HttpContext.Request.IsAjaxRequest())
        //     //{
        //     //    filterContext.Result = new RedirectToRouteResult(
        //     //        new RouteValueDictionary 
        //     //       { 
        //     //           { "controller", "Start" }, 
        //     //           { "action", "NgView" } 
        //     //       });
        //     //}
        //}
 
        //protected override void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    var ex = Server.GetLastError().GetBaseException();

        //    var routeData = new RouteData();

        //    if (ex.GetType() == typeof(HttpException))
        //    {
        //        var httpException = (HttpException)ex;
        //        HttpContext.Current.Response.RedirectToRoute(new RouteValueDictionary 
        //            { 
        //                { "controller", "Start" }, 
        //                { "action", "NgView" } 
        //            });
        //    }
        //}
    }
}