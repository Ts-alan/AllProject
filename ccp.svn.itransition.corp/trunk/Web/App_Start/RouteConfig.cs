using System.Web.Mvc;
using System.Web.Routing;

namespace CCP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("{controller}/{action}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Start", action = "NgView", id = UrlParameter.Optional}
                );
        }
    }
}