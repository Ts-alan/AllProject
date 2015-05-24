using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCP.Controllers
{
    public class DialogsController : Controller
    {
        // GET: Dialogs
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Confirm()
        {
            return View();
        }
    }
}