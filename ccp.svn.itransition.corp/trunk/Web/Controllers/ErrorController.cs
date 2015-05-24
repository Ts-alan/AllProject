using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCP.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult ClientError()
        {
            return View();
        }
    }
}