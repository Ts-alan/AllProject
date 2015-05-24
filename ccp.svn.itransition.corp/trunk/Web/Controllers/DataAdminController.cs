using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCP.Controllers
{
    public class DataAdminController : Controller
    {
        // GET: Tabs
        public ActionResult DataAdmin()
        {
            return PartialView();
        }

        public ActionResult SalesPersons()
        {
            return PartialView();
        }

        public ActionResult Users()
        {
            return PartialView();
        }

    }
}