using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCP.Controllers
{
    public class MessageCenterController : Controller
    {
        // GET: MessageCenter
        public ActionResult MessageCenter()
        {
            return PartialView();
        }
    }
}