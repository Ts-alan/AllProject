using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crystal.BusinessLogic;
using Crystal.DomainModel;


namespace Crystal.Models
{
    
    public static class DeviceService
    {


        public static string TrackedHtml = @"<span><div class='Tracked StarInDisplay' title='Прибор отслеживается' rel='tooltip'   onClick='Device($(this),""{0}"",{1})'></div></span>";
        public static string CanTrackedHtml = @"<span><div title='Отслеживать прибор' class='StarInHide' rel='tooltip'   onClick='Device($(this),""{0}"",{1})'></div></span>";
        public static ITrackingService Service;

        //public static IDataManager DataManager;

        static DeviceService() 
        {
#if DEBUG
            Service = new DeviceTrackingService(new WcfLogic.DeviceDataManagerWCF());
#else
            Service = new DeviceTrackingService(new EFContext.DeviceDataManagerEF());
#endif
           
        }

        public static void AddToTrackingList( string kpr,string pNum, int userId)
        {
            Service.AddToTrackingList(new TrackedDevice {DeviceNumber = kpr, ProdNumber = pNum, UserId = userId });
        }

        public static void RemoveFromTrackingList(string kpr,string pNum, int userId)
        {
            Service.RemoveFromTrackingList(new TrackedDevice { DeviceNumber = kpr, ProdNumber = pNum, UserId = userId });

        }
        public static IEnumerable<TrackedDevice> GetDevicesForUser(int userId)
        {
            return Service.GetUserTrackedItems(userId).Cast<TrackedDevice>();
        }
        public static bool CheckIfAlreadyTracking(string kpr, string prodN, int userId)
        {
            return Service.IsTracked(new TrackedDevice { DeviceNumber = kpr, ProdNumber = prodN , UserId = userId });
        }
        
    }

    #region Хэлпер для генерации ссылки на добавление/снятие
    public static class DeviceWatcherHelpers
    {
        public static IHtmlString TrackThisDevice(this HtmlHelper helper, string kpr, string prodN, int userId,
                                                 IEnumerable<TrackedDevice> device = null)
        {
            if (DeviceService.CheckIfAlreadyTracking(kpr, prodN, userId))
            {
                return MvcHtmlString.Create(string.Format(DeviceService.TrackedHtml, kpr, prodN));
            }
            return MvcHtmlString.Create(string.Format(DeviceService.CanTrackedHtml, kpr, prodN));
        }
    }
    #endregion
}