using System;
using System.Web.Mvc;
using System.Web.Security;
using Crystal.Models;
using Crystal.Authentication;

namespace CrystalMonitor.Controllers
{
    [Authorize]
    public class BackEndServicesController : Controller
    {
        [HttpPost]
        public ActionResult StartTracking(string prodN,string nprt,string userName = null)
        {
            if (userName == null)
                userName = User.Identity.Name;

            var userId = AuthService.GetUserKeyHashCode(User);// Membership.GetUser(userName).ProviderUserKey.GetHashCode();
            
            BatchWatcherService.AddToTrackingList(nprt, prodN, userId);
            return Content(string.Format(BatchWatcherService.TrackedHtml, nprt, prodN));
        }

        [HttpPost]
        public ActionResult StopTracking(string prodN, string nprt, string userName=null)
        {
            if (userName == null)
                userName = User.Identity.Name;

            var userId = AuthService.GetUserKeyHashCode(User);// Membership.GetUser(userName).ProviderUserKey.GetHashCode();
            BatchWatcherService.RemoveFromTrackingList(nprt, prodN, userId);
            return Content(string.Format(BatchWatcherService.CanTrackedHtml, nprt, prodN));
        }
        [HttpPost]
        public ActionResult StartDevice(string prodN, string kpr, string userName = null)
        {
            if (userName == null)
                userName = User.Identity.Name;

            var userId = AuthService.GetUserKeyHashCode(User);// Membership.GetUser(userName).ProviderUserKey.GetHashCode();
            DeviceService.AddToTrackingList(kpr, prodN, userId);
            return Content(string.Format(DeviceService.TrackedHtml, kpr, prodN));
        }
        [HttpPost]
        public ActionResult StopDevice(string prodN, string kpr, string userName = null)
        {
            if (userName == null)
                userName = User.Identity.Name;

            var userId = AuthService.GetUserKeyHashCode(User);// Membership.GetUser(userName).ProviderUserKey.GetHashCode();
            
            DeviceService.RemoveFromTrackingList(kpr,prodN,userId);
            return Content(string.Format(DeviceService.CanTrackedHtml, kpr, prodN));
        }
    }
}
