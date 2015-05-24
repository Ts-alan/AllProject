using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCP.Controllers
{
    public class StartController : Controller
    {
        // GET: Start
        public ActionResult NgView()
        {
            return PartialView();
        }
    }
}