using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crystal.BusinessLogic;
using Crystal.DomainModel;

namespace Crystal.Models
{

    public class BatchWatcherService
    {

        public static string TrackedHtml = @"<span><div class='Tracked StarInDisplay' title='Партия отслеживается' rel='tooltip' style='display: inline-block'  nprt='{0}' onClick='Tracker($(this),{1})'></div></span>";
        public static string CanTrackedHtml = @"<span><div class='StarInHide' title='Отслеживать партию' rel='tooltip' style='display: inline-block'  nprt='{0}' onClick='Tracker($(this),{1})'></div></span>";
        public static ITrackingService Service;

        //Конструктор
        static BatchWatcherService()
        {
#if DEBUG
            Service = new BatchTrackingService(new WcfLogic.BatchDataManagerWCF());
#else
            Service = new BatchTrackingService(new EFContext.BatchDataManagerEF());
#endif            
        }

       public static void AddToTrackingList(string nprt, string pNum, int userId)
        {
            Service.AddToTrackingList(new TrackedBatch() { BatchNumber = nprt, ProdNumber = pNum, UserId = userId });
        }
        
        public static void RemoveFromTrackingList(string nprt, string pNum, int userId)
        {
            Service.RemoveFromTrackingList(new TrackedBatch() { BatchNumber = nprt, ProdNumber = pNum, UserId = userId });
        }
        
        public static IEnumerable<TrackedBatch> GetBatchesForUser(int userId)
        {
            return Service.GetUserTrackedItems(userId).Cast<TrackedBatch>();
        }

        public static bool CheckIfAlreadyTracking(string nprt, string prodN, int userId)
        {
            return Service.IsTracked(new TrackedBatch() { BatchNumber = nprt, ProdNumber = prodN, UserId = userId });
        }
    }

    #region Хэлпер для генерации ссылки на добавление/снятие
    public static class BatchWatcherHelpers
    {
        public static IHtmlString TrackThisBatch(this HtmlHelper helper, string nprt, string prodN, int userId)
        {
            if (BatchWatcherService.CheckIfAlreadyTracking(nprt, prodN, userId))
            {
                return MvcHtmlString.Create(string.Format(BatchWatcherService.TrackedHtml, nprt, prodN));  
            }
            return MvcHtmlString.Create(string.Format(BatchWatcherService.CanTrackedHtml, nprt, prodN));
        }
    }
    #endregion
    
}