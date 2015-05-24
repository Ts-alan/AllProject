using System;
using System.Web.Mvc;
using System.Web.Security;
using Verst.Models;

namespace CrystalMonitor.Controllers
{
    [Authorize]
    public class BackEndServicesController : Controller
    {
        [HttpPost]
        public ActionResult StartTracking(string prodN,string nprt,string userName)
        {
            var userid = Membership.GetUser(userName).ProviderUserKey.GetHashCode();
            BatchWatcherService.AddToTrackingList(nprt, prodN, userid);
            return Content("<span><img class='Tracked' src='../../Content/images/star_active.png'  nprt='" + nprt + "' onClick='Tracker($(this)," + prodN + ")'></span>");
        }

        [HttpPost]
        public ActionResult StopTracking(string prodN, string nprt, string userName)
        {
            var userid = Membership.GetUser(userName).ProviderUserKey.GetHashCode();
            BatchWatcherService.RemoveFromTrackingList(nprt, prodN, userid);
            return Content("<span><img src='../../Content/images/star_passive.png' nprt='" + nprt + "' onClick='Tracker($(this)," + prodN + ")'></span>");
        }
    }
}
