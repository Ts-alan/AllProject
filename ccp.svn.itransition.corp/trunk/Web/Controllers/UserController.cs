using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCP.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Profile()
        {
            return PartialView();
        }

        public ActionResult Login()
        {
            return PartialView();
        }
    }
}