using System.Web.Mvc;
using CCP.Filters;

namespace CCP.Controllers
{
    public class CPRController : Controller
    {
        public ActionResult CPRs()
        {
            return PartialView();
        }

        public ActionResult CPR()
        {
            return PartialView();
        }
      
    }
}
