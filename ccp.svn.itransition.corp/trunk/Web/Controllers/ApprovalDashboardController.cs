using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCP.Controllers
{
    public class ApprovalDashboardController : Controller
    {
        // GET: ApprovalDashboard
        public ActionResult ApprovalDashboard()
        {
            return PartialView();
        }
    }
}