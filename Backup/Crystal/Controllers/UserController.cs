using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Profile;
using System.Web.Security;
using MvcApplication.Models;
using NLog;
using Crystal.Models;
using CrystalDataSetLib;
using Crystal.Authentication;
using Crystal.DomainModel;
using Crystal.BusinessLogic;

namespace Crystal.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        //private ProfileBase _currentUserProfile;
        private static Logger _log = LogManager.GetCurrentClassLogger();


        #region UserCabModels

        public class PersonalCabinetModel
        {
            public string UserName { get; set; }
            public string Fio { get; set; }
            public string Unit { get; set; }
            public IEnumerable<ObservedBatch> ObservedBatches { get; set; }
            public IEnumerable<WasDeleted> DeletedBatches { get; set; }
            public IEnumerable<ObservedDevice> ObservedDevices;
        }


        public class WasDeleted
        {
            public string ProdN { get; set; }
            public string BatchNum { get; set; }
        }

        #endregion


        public ActionResult Cabinet()
        {
            ICrystalLogic bl;// = BusinessLogicFactory.GetBL();

            var userId = AuthService.GetUserKeyHashCode(User);//Membership.GetUser(User.Identity.Name);
            _log.Info("Пользователь {0} вошёл в личный кабинет.", User.Identity.Name);
            //_currentUserProfile = ProfileBase.Create(User.Identity.Name);
            var observedBatchesList = BatchWatcherService.GetBatchesForUser(userId).ToList();
            var batchArr = new List<ObservedBatch>();
            var deviceArr = new List<ObservedDevice>();
            var observedDevicesList = DeviceService.GetDevicesForUser(userId).ToList();
            foreach (var obsProd in observedBatchesList.GroupBy(x => x.ProdNumber).Select(x => new { x.Key, NprtArr = x }))
            {
                bl = BusinessLogicFactory.GetBL(FoxRepo.GetPlantFromNumber(obsProd.Key));
                var querry = bl.GetTrackedBatchesDetail(obsProd.NprtArr);
                batchArr.AddRange(querry);

            }

            foreach (var obsDevice in observedDevicesList.GroupBy(x => x.ProdNumber).Select(x => new { x.Key, DeviceArr = x }))
            {
                bl = BusinessLogicFactory.GetBL(FoxRepo.GetPlantFromNumber(obsDevice.Key));
                var querry = bl.GetTrackedDevicesDetail(obsDevice.DeviceArr);
                deviceArr.AddRange(querry);
            }

            //Бредово но должно работать
            IEnumerable<string> deleted = observedBatchesList.Select(f => f.BatchNumber).Except(batchArr.Select(s => s.Nprt));
            IEnumerable<WasDeleted> delBatches = from bList in observedBatchesList
                                                 where deleted.Contains(bList.BatchNumber)
                                                 select new WasDeleted { ProdN = bList.ProdNumber, BatchNum = bList.BatchNumber };

            //Удаляем того чего нету
            CleanBatchesList(ref delBatches, ref userId);

            var result = new PersonalCabinetModel
            {
                ObservedDevices = deviceArr,
                ObservedBatches = batchArr,
                DeletedBatches = delBatches,
                UserName = User.Identity.Name,
                Fio = Crystal.Authentication.AuthService.GetUserProfileProperty(User.Identity.Name, "Fio")?? "", // (string)_currentUserProfile["Fio"] ?? "",
                Unit = Crystal.Authentication.AuthService.GetUserProfileProperty(User.Identity.Name, "Unit")?? "" //(string)_currentUserProfile["Unit"] ?? ""
            };

            ViewBag.userId = Crystal.Authentication.AuthService.GetUserKeyHashCode(User);

            return View(result);
        }

        private void CleanBatchesList(ref IEnumerable<WasDeleted> shouldBeDeleted, ref int userId)
        {
            foreach (var batch in shouldBeDeleted)
            {
                BatchWatcherService.RemoveFromTrackingList(batch.BatchNum, batch.ProdN, userId);
            }
        }
        //private IEnumerable<ObservedDevice> MakeResultDevice(string kpr, string prodN)
        //{
        //    var result = from asspr in FoxRepo.GetTable<CrystalDS.assprRow>(plant: FoxRepo.GetPlantFromNumber(prodN)).Where(x => x.kpr == kpr)
        //                 join mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>(plant: FoxRepo.GetPlantFromNumber(prodN)).Where(x => x.kpr == kpr)
        //                  on asspr.kpr equals mpartt.kpr into g_mpartt
        //                 select new ObservedDevice
        //                 {
        //                     Kpr = asspr.kpr,
        //                     Napr = asspr.napr,
        //                     ProdNumber = prodN,
        //                     Nzp = Convert.ToInt32(g_mpartt.Sum(a => a.kpls)),
        //                     Nnprt = g_mpartt.Select(a => a.nprt).Count()
        //                 };

        //    return result;
        //}
        //private IEnumerable<ObservedBatch> MakeResultBatch(string prodN, IEnumerable<string> nprtArray)
        //{
        //    var preSelect = from mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>(plant: FoxRepo.GetPlantFromNumber(prodN))
        //                    join batches in nprtArray
        //                        on mpartt.nprt equals batches
        //                    join asspr in FoxRepo.GetTable<CrystalDS.assprRow>(plant: FoxRepo.GetPlantFromNumber(prodN))
        //                      on mpartt.kpr equals asspr.kpr
        //                    join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>(plant: FoxRepo.GetPlantFromNumber(prodN))
        //                      on new { mpartt.kmk, mpartt.nop } equals new { astmr.kmk, astmr.nop }
        //                    join assop in FoxRepo.GetTable<CrystalDS.assopRow>(plant: FoxRepo.GetPlantFromNumber(prodN))
        //                      on astmr.kop equals assop.kop
        //                    select new ObservedBatch
        //                    {
        //                        Nprt = mpartt.nprt,
        //                        Napr = asspr.napr,
        //                        Naop = assop.naop,
        //                        Nzp = mpartt.kpls,
        //                        Date = mpartt.dato.isNullDate() ? mpartt.dath.ToShortDateString() + " " + mpartt.timh.GetFoxTime() : mpartt.dato.ToShortDateString() + " " + mpartt.timo.GetFoxTime(),
        //                        ProdNumber = prodN
        //                    };

        //    return preSelect;
        //}
    }
}
