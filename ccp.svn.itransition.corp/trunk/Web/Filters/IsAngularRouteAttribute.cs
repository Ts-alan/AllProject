using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CCP.Controllers;

namespace CCP.Filters
{

    public class IsAngularRouteAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller is StartController || filterContext.Controller is ErrorController) return;
            if(filterContext.Controller is UserController && filterContext.ActionDescriptor.ActionName == "Login") return;
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary 
                    { 
                        { "controller", "Start" }, 
                        { "action", "NgView" } 
                    });
            }
        }
    }
}