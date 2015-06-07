using Crystal.BusinessLogic;
using MvcApplication.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crystal.Infrastructure
{
    public class LogActionAttribute : FilterAttribute, IResultFilter
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();
        public string Message { get; set; }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Result is ViewResult)
            {
                string logMessage = (filterContext.Result as ViewResult).ViewBag.log;
                string userName = "[" + filterContext.HttpContext.User.Identity.Name + "]";
                string plantName = "";
                Plants currentPlant;
                if (Enum.TryParse<Plants>(filterContext.HttpContext.Request.RequestContext.RouteData.Values["plant"] as string, true, out currentPlant))
                {
                    plantName = ((int)currentPlant).ToString();
                }
                if (logMessage == null) logMessage = Message;
                var outMessage = logMessage.Replace("{User}", userName).Replace("{Plant}", plantName);
                _log.Info(outMessage);
            }
        }
    }
}