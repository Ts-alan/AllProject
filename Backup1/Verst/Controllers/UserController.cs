using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Profile;
using System.Web.Security;
using MvcApplication.Models;
using NLog;
using Verst.Models;

namespace Verst.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ProfileBase _currentUserProfile;
        private static Logger _log = LogManager.GetCurrentClassLogger();

        #region UserCabModels

        public class PersonalCabinetModel
        {
            public string UserName { get; set; }
            public string Fio { get; set; }
            public string Unit { get; set; }
            public IEnumerable<ObservedBatchesModel> ObservedBatches { get; set; }
            public IEnumerable<WasDeleted> DeletedBatches { get; set; }
        }

        public class ObservedBatchesModel
        {
            public string Nprt { get; set; }
            public string Napr { get; set; }
            public string Naop { get; set; }
            public decimal Nzp { get; set; }
            public string Date { get; set; }
            public string ProdNumber { get; set; }
        }

        public class WasDeleted
        {
            public string ProdN { get; set; }
            public string BatchNum { get; set; }
        }

        #endregion
        

        public ActionResult Cabinet()
        {
            var member = Membership.GetUser(User.Identity.Name);
            _log.Info("Пользователь {0} вошёл в личный кабинет.",User.Identity.Name);
            _currentUserProfile = ProfileBase.Create(member.UserName);
            var observed = BatchWatcherService.GetBatchesForUser(member.ProviderUserKey.GetHashCode()).ToList();
            var batchesArr = new List<ObservedBatchesModel>();

            foreach (var obsProd in observed.GroupBy(x => x.ProdNumber).Select(x => new { x.Key, NprtArr = x.Select(s => s.BatchNumber) }))
            {
                var querry = MakeResult(obsProd.Key, obsProd.NprtArr);
                batchesArr.AddRange(querry);
            }

            //Бредово но должно работать
            IEnumerable<string> deleted = observed.Select(f => f.BatchNumber).Except(batchesArr.Select(s => s.Nprt));
            IEnumerable<WasDeleted> delBatches = from bList in observed
                                                 where deleted.Contains(bList.BatchNumber)
                                                 select new WasDeleted { ProdN = bList.ProdNumber, BatchNum = bList.BatchNumber };

            //Удаляем того чего нету
            CleanBatchesList(ref delBatches, ref member);

            var result = new PersonalCabinetModel
            {
                ObservedBatches = batchesArr, 
                DeletedBatches = delBatches ,
                UserName = member.UserName,
                Fio = (string)_currentUserProfile["Fio"]??"",
                Unit = (string)_currentUserProfile["Unit"]??""
            };
            return View(result);
        }

        private void CleanBatchesList(ref IEnumerable<WasDeleted> shouldBeDeleted, ref MembershipUser member)
        {
            foreach (var batch in shouldBeDeleted)
            {
                BatchWatcherService.RemoveFromTrackingList(batch.BatchNum, batch.ProdN, member.ProviderUserKey.GetHashCode());
            }
        }

        private IEnumerable<ObservedBatchesModel> MakeResult(string prodN, IEnumerable<string> nprtArray)
        {
            var preSelect = from mpartt in FoxRepo.GetTable<Crystal.mparttRow>(plant:FoxRepo.GetPlantFromNumber(prodN))
                            join batches in nprtArray
                                on mpartt.nprt equals batches
                             join asspr in FoxRepo.GetTable<Crystal.assprRow>(plant:FoxRepo.GetPlantFromNumber(prodN))
                               on mpartt.kpr equals asspr.kpr
                             join astmr in FoxRepo.GetTable<Crystal.astmrRow>(plant:FoxRepo.GetPlantFromNumber(prodN))
                               on new { mpartt.kmk, mpartt.nop } equals new { astmr.kmk, astmr.nop }
                             join assop in FoxRepo.GetTable<Crystal.assopRow>(plant:FoxRepo.GetPlantFromNumber(prodN))
                               on astmr.kop equals assop.kop
                             select new ObservedBatchesModel
                             {
                                 Nprt = mpartt.nprt,
                                 Napr = asspr.napr,
                                 Naop = assop.naop,
                                 Nzp = mpartt.kpls,
                                 Date = mpartt.dato.isNullDate() ? mpartt.dath.ToShortDateString() + " " + mpartt.timh.GetFoxTime() : mpartt.dato.ToShortDateString() + " " + mpartt.timo.GetFoxTime(),
                                 ProdNumber = prodN
                             };

            return preSelect;
        }
    }

    public static class TableAdapterExtensions2
    {
        public static bool ValidString(this string first, string second)
        {
            if (System.String.Compare(first, second, true, System.Globalization.CultureInfo.CurrentCulture) == 0)
                return true;
            else
                return false;
        }
    }
}
