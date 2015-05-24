using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Verst.Models.SiteAccountsTableAdapters;

namespace Verst.Models
{
    //Singleton
    public class BatchWatcherService
    {
        private static BatchWatcherService _batchWatcher;
        private static System.Configuration.ConnectionStringSettings _dbPath;
        private static ObservedBatchesTableAdapter _observedBatches;

        //Конструктор
        private BatchWatcherService()
        {
            _dbPath = System.Configuration.ConfigurationManager.ConnectionStrings["SiteAccounts"];
            _observedBatches = new ObservedBatchesTableAdapter();
        }

        public static BatchWatcherService GetInstance()
        {
            if (_batchWatcher==null)
            {
               return _batchWatcher = new BatchWatcherService(){};
            }
                return _batchWatcher;
        }

        public static void AddToTrackingList(string nprt, string pNum, int userId)
        {
            var rec = _observedBatches.GetData().FirstOrDefault(x => x.BatchNumber == nprt && x.ProdNumber == pNum && x.UserID.GetHashCode() == userId);
            if (rec==null)
            {
                _observedBatches.Insert(userId, nprt, pNum);
                HttpContext.Current.Session["observed"] = null;
            }
        }
        
        public static string RemoveFromTrackingList(string nprt, string pNum, int userId)
        {
            var rec = _observedBatches.GetData().FirstOrDefault(x => x.BatchNumber == nprt && x.ProdNumber == pNum && x.UserID.GetHashCode() == userId);
            if (rec != null)
            {
                _observedBatches.Delete(rec.RecordID);
                HttpContext.Current.Session["observed"] = null;
            }
            return null;
        }
        
        public static IEnumerable<SiteAccounts.ObservedBatchesRow> GetBatchesForUser(int userId)
        {
            return _observedBatches.GetData().Where(x => x.UserID.GetHashCode() == userId);
        }

        public static bool CheckIfAlreadyTracking(string nprt, string prodN, string userName, IEnumerable<Verst.Models.SiteAccounts.ObservedBatchesRow> observed = null)
        {
            if (observed == null)
                observed = (IEnumerable<Verst.Models.SiteAccounts.ObservedBatchesRow>)HttpContext.Current.Session["observed"];
            
            if (observed == null) observed = _observedBatches.GetData();
            
            HttpContext.Current.Session["observed"]  = observed;
            
            var user = System.Web.Security.Membership.GetUser(userName);
            if (user != null)
            {
                var userId = user.ProviderUserKey.GetHashCode();
                var test = observed.FirstOrDefault(x => x.UserID.GetHashCode() == userId && x.ProdNumber == prodN && x.BatchNumber == nprt);
                return test != null;
            }
            return false;
        }
    }

    #region Хэлпер для генерации ссылки на добавление/снятие
    public static class BatchWatcherHelpers
    {
        public static IHtmlString TrackThisBatch(this HtmlHelper helper, string nprt, string prodN, string userName, IEnumerable<Verst.Models.SiteAccounts.ObservedBatchesRow> observed = null)
        {
            if (BatchWatcherService.CheckIfAlreadyTracking(nprt, prodN, userName, observed))
            {
                return MvcHtmlString.Create("<span><img class='Tracked' title='Партия отслеживается' rel='tooltip' src='../../Content/images/star_active.png'  nprt='" + nprt + "' onClick='Tracker($(this)," + prodN + ")'></span>");
            }
            return MvcHtmlString.Create("<span><img title='Отслеживать партию' rel='tooltip' src='../../Content/images/star_passive.png' nprt='" + nprt + "' onClick='Tracker($(this)," + prodN + ")'></span>");
        }

        /// <summary>
        /// Принимает номер из вызова
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IHtmlString TrackThisBatchScript(this HtmlHelper helper, string userName)
        {
            
            return MvcHtmlString.Create("<script>function Tracker($elem, prodN) { var url = '/BackEndServices/StartTracking'; if ($elem.hasClass('Tracked')) { url = '/BackEndServices/StopTracking'; } $.ajax({ url: url, type: 'POST', data: { prodN: prodN, nprt: $elem.attr('nprt'), userName: '" + userName + "' }, success: function (result) { $elem.closest('span').html(result); } }); }</script>");
        }

        /// <summary>
        /// Принимает номер из объявления
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="prodN"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IHtmlString TrackThisBatchScript(this HtmlHelper helper, string prodN, string userName)
        {
            return MvcHtmlString.Create("<script>function Tracker($elem) { var url = '/BackEndServices/StartTracking'; if ($elem.hasClass('Tracked')) { url = '/BackEndServices/StopTracking'; } $.ajax({ url: url, type: 'POST', data: { prodN: '" + prodN + "', nprt: $elem.attr('nprt'), userName: '" + userName + "' }, success: function (result) { $elem.closest('span').html(result); } }); }</script>");
        }
    }
    #endregion
    
}