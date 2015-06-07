using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Crystal.BusinessLogic;
using Crystal.DomainModel;
using NLog;
using Crystal.Models;
using System.Linq;
using CrystalDataSetLib;
using Crystal.VFPOleDBLogic;
//using Equipment = MvcApplication.Models.Equipment;
using MvcApplication.Models;
using System.Data.Entity.Core;
using System.Web.Caching;
namespace Crystal.Controllers
{
    [PlantAuthorization]
    public class FacilityController : Controller
    {
        ICrystalLogic bl = BusinessLogicFactory.GetBL();
        //VFPOleDbRepo FoxRepo;
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public ActionResult Details(string KGT, string KUS)
        {
            
            var model = bl.GetEquipmentDetail(KGT, KUS);
          
            ViewBag.prodN = bl.GetProductionNumber();
            ViewBag.userId = Crystal.Authentication.AuthService.GetUserKeyHashCode(User);
            _log.Info("Пользователь [{0}] просматривает данные по установке [{1}] на производстве №{2}.", User.Identity.Name, model.ShortName, ViewBag.prodN);
            return View(model);
        }

        public ActionResult GetPrib(string KGT, string KUS, int Depth)
        {
            var data = bl.GetPrib(KGT, KUS, Depth);
            return View(data);
        }

        public ActionResult AreaNzp()
        {
            ViewBag.Prn = bl.GetProductionNumber();
            var result = bl.AreaNzp();

            ViewBag.results = result;
            TempData["Table"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult AreaNzpBrakGraf()
        {
            var nauch = ((IEnumerable<NzpUch>)TempData.Peek("Table")).Select(x => x.Kuch).ToList();
            var brk = ((IEnumerable<NzpUch>)TempData.Peek("Table")).Select(x => int.Parse(x.Brk)).ToList();
            return Json(new { Nauch = nauch, Brk = brk }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartInfo(string NPRT)
        {
            BatchInfo result;
            try
            {
                result = bl.PartInfo(NPRT);
            }

            catch (System.ServiceModel.FaultException<ExceptionDetail> ex)
            {
                if (ex.Detail.Type == typeof(ObjectNotFoundException).FullName)
                {
                    ViewBag.ErrorText = ex.Message;
                    return PartialView("ServiceError");
                }
                throw ex;
            }
            ViewBag.prodN = bl.GetProductionNumber();
            ViewBag.userId = Crystal.Authentication.AuthService.GetUserKeyHashCode(User);
            Session["BatchInfo_" + NPRT] = result;
            _log.Info("Пользователь [{0}] просматривает данные по партии [{1}] на производстве №{2}.", User.Identity.Name, NPRT, ViewBag.prodN);
            return View(result);
        }



        public ActionResult McomplInfo(string nop, string nprt, Guid id, string mcompl)
        {
            //var tempAM = FoxRepo.GetTable<Crystal.mpartnRow>(isArchived: true);
            //var tempM = FoxRepo.GetTable<Crystal.mpartnRow>();
            //var Tem = tempM.Concat(tempAM);
            //var result = from mcompl in FoxRepo.GetTable<Crystal.mcomplRow>()
            //             where mcompl.nop == nop && mcompl.nprt == nprt

            //             select new ComplData
            //             {
            //                 Kpls = mcompl.kpls.ToString(),
            //                 Nprtr = mcompl.nprtr,
            //                 Kplr = mcompl.kplr.ToString(),
            //                 Datr = mcompl.datr.ToString(),
            //                 Timr = mcompl.timr,
            //                 Fio = mcompl.fio
            //             };
            var result = ((BatchInfo)Session["BatchInfo_" + nprt]).RouteData.Single(a => a.Nop == nop && a.Id == id).ComplData.Where(a => a.Pcompl == mcompl);
            return View(result);
        }

        public ActionResult Downtimes(string KGT, string KUS)
        {
            var result = bl.Downtimes(KGT, KUS);
            _log.Info("Пользователь [{0}] просматривает данные по простоям установки [{1}] на производстве №{2}.", User.Identity.Name, KGT + ":" + KUS, (int)FoxRepo.GetPlant());
            return View(result);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetBrak(string nop, string nprt, Guid id)
        {
            var result = ((BatchInfo)Session["BatchInfo_" + nprt]).RouteData.Single(a => a.Nop == nop && a.Id==id ).BrakInfo;
            return View(result);
        }
        public ActionResult MZagrsBrig()
        {
            ViewBag.Action = "MZagrBrig";
            ViewBag.Controller = "Facility";
            ViewBag.Title = "Отчет о работе по бригадам";
            return View("DateForm");
        }
        public ActionResult MZagrBrig(string fdate, string sdate, string indnumb)
        {
            ViewBag.Period = FoxRepo.GetPeriod(fdate, sdate, indnumb);
            ViewBag.Prn = bl.GetProductionNumber();
            DateTime d1 = DateTime.Parse(fdate);
            DateTime d2 = DateTime.Parse(sdate);

            object date1 = d1, date2 = d2;
            TempData["Date1"] = d1;
            TempData["Date2"] = d2;
            var Result = bl.MZagrBrig(d1, d2);
            
            ViewBag.result = (from test in Result
                          group test by new { test.Numb, test.Kuch }
                              into zz
                              select new LoadMth
                              {
                                  Numb = zz.Key.Numb,
                                  Kuch = zz.Key.Kuch,
                                  Kpls = zz.Sum(a => int.Parse(a.Kpls)).ToString(),
                                  RepH = zz.Sum(a => decimal.Parse(a.RepH)).ToString("F2"),
                                  KrObr = (zz.Sum(a => a.KrObr) / zz.Count())

                              }).OrderBy(a => a.Numb);
            TempData["Result"] = Result;
            return View();
        }

        public ActionResult MZagrBrigNumb(string numb)
        {
            IEnumerable<LoadMth> res = (IEnumerable<LoadMth>)TempData.Peek("Result");
            var res1 = res.Where(a => a.Numb == numb);
            return View(res1);
        }

        public ActionResult MZagrs()
        {
            ViewBag.Action = "EquipmentLoad";
            ViewBag.Controller = "Facility";
            ViewBag.Title = "Загрузка оборудования";
            return View("DateForm");
        }

        public ActionResult EquipmentLoad(string fdate, string sdate, string indnumb)
        {
            ViewBag.Period = FoxRepo.GetPeriod(fdate, sdate, indnumb);
            ViewBag.Prn = bl.GetProductionNumber();
            DateTime d1 = DateTime.Parse(fdate);
            DateTime d2 = DateTime.Parse(sdate);
            //var culture_RU = CultureInfo.CreateSpecificCulture("ru-RU");
            //DateTime d1 = DateTime.ParseExact(fdate, "dd-MM-yyyy", culture_RU);//DateTime.Parse(fdate);//, "ddMMYYYY", culture_US);//culture_US);
            //DateTime d2 = DateTime.ParseExact(sdate, "dd-MM-yyyy", culture_RU); //DateTime.Parse(sdate);

            var data = bl.GetEquipmentLoads(d1, d2);

            var reasons = data.ToDictionary(a=>a.EquipmentUniqCode, b=>b.Repairs);
            //ViewBag.reas = ((IEnumerable<LoadMth>)ViewBag.Result).Average(a => decimal.Parse(a.KrObr)).ToString("F3");
            var viewCode = Guid.NewGuid();
            Session["reasons"+viewCode.ToString()] = reasons;
            //HttpContext.Cache.Add("reasons"+viewCode.ToString(), reasons, null, 

            ViewData.Model = data;
            ViewBag.AvgEfficient = data.Average(a => a.EquipmentEfficiency);
            ViewBag.ViewCode = viewCode.ToString();
            

            return View();
        }

        public ActionResult RepairReasons(string EquipmentCode, string ViewCode)
        {
            var reasons = (Dictionary<string, EquipmentLoad.RepairReason[]>)Session["reasons"+ViewCode];
            if (string.IsNullOrEmpty(EquipmentCode))
            {
                ViewData.Model = from reason in reasons.SelectMany(a => a.Value)
                                 group reason by new { reason.ReasonCode, reason.ReasonName } into g_reason
                                 select new EquipmentLoad.RepairReason
                                 {
                                     ReasonName = g_reason.Key.ReasonName,
                                     RepairCount = g_reason.Sum(a => a.RepairCount),
                                     DurationH = g_reason.Sum(a => a.DurationH)
                                 };
            }
            else
            {
                ViewData.Model = reasons[EquipmentCode];
            }
            return PartialView();
        }

        //public ActionResult MZagr(string fdate, string sdate, string indnumb)
        //{
        //    ViewBag.Period = FoxRepo.GetPeriod(fdate, sdate, indnumb);
        //    ViewBag.Prn = (from asparam in FoxRepo.GetTable<CrystalDS.asparamRow>()
        //                   where asparam.identp.Trim() == "FACTORY"
        //                   select asparam.valp).First().Trim();
        //    DateTime d1 = DateTime.Parse(fdate);
        //    DateTime d2 = DateTime.Parse(sdate);

        //    object date1 = d1, date2 = d2;
        //    TempData["Date1"] = d1;
        //    TempData["Date2"] = d2;

        //    int hrsMonth = FoxRepo.WorkDaysInMonth(DateTime.Now) * 24;
        //    var tempMprxop = FoxRepo.GetTable<CrystalDS.mprxopRow>(filter: "WHERE BETWEEN(dath,?,?)", parameters: new[] { date1, date2 });
        //    var tempAsmbn = FoxRepo.GetTable<CrystalDS.asmbnRow>(filter: "WHERE (EMPTY(Dop)) OR BETWEEN(Dop,?,?)", parameters: new[] { date1, date2 }); //.Where(a=> a.kgt == "18" && a.kus == "11");

        //    ViewBag.Result = (from astob in FoxRepo.GetTable<CrystalDS.astobRow>()
        //                      join askuch in FoxRepo.GetTable<CrystalDS.askuchRow>()
        //                          on astob.kuch equals askuch.kuch
        //                      join mprxop in tempMprxop
        //                         on new { astob.kgt, astob.kus } equals new { mprxop.kgt, mprxop.kus } into proc_parts
        //                      join mpartn in FoxRepo.GetTable<CrystalDS.mpartnRow>()
        //                         on new { astob.kgt, astob.kus } equals new { mpartn.kgt, mpartn.kus } into proc_brak
        //                      join asmbn in tempAsmbn
        //                        on new { astob.kgt, astob.kus } equals new { asmbn.kgt, asmbn.kus } into eq_repairs
        //                      let reps = eq_repairs.Sum(asmbn => FoxRepo.HoursBetweenForMZagr(asmbn.dop, asmbn.top, asmbn.dnp, asmbn.tnp))
        //                      select new LoadMth
        //                      {
        //                          Kuch = askuch.nauch,
        //                          Nobr = astob.snaobor,
        //                          Kpls = proc_parts.Sum(a => a.kpls).ToString(),
        //                          Brk = proc_brak.Count().ToString(),
        //                          Nprt = proc_parts.Count().ToString(),
        //                          RepH = reps.ToString("F2"),
        //                          KrObr = ((hrsMonth - reps) / hrsMonth).ToString("F3")
        //                      }).OrderBy(x => x.Kuch);

        //    var reas = from askprob in FoxRepo.GetTable<CrystalDS.askprobRow>()
        //               join asmbn in tempAsmbn
        //               on askprob.kpp equals asmbn.kpp
        //               group asmbn by new { askprob.napr } into zapr
        //               let reps = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp))
        //               select new Reasons
        //               {
        //                   Napr = zapr.Key.napr,
        //                   SumN = zapr.Select(a => a.kpp).Count().ToString(),
        //                   Hrs = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp)).ToString("F2"),
        //                   KrObr = ((hrsMonth - reps) / hrsMonth).ToString("F3"),
        //               };
        //    ViewBag.reas = ((IEnumerable<LoadMth>)ViewBag.Result).Average(a => decimal.Parse(a.KrObr)).ToString("F3");
        //    ViewBag.Result2 = reas.OrderByDescending(a => a.Hrs);
            
        //    return View();
        //}

        //[HttpGet]
        //public ViewResult MzagrFor(string month, string year)
        //{

        //    DateTime d1 = DateTime.Parse("1." + month + "." + year), d2 = DateTime.Parse(DateTime.DaysInMonth(int.Parse(year), int.Parse(month)).ToString() + "." + month + "." + year);
        //    TempData["Date1"] = d1;
        //    TempData["Date2"] = d2;
        //    int WorkHrsMonth = FoxRepo.WorkDaysInMonth(d1) * 24;

        //    var t_mprxop = FoxRepo.GetTable<CrystalDS.mprxopRow>(filter: "WHERE BETWEEN(Dath,?,?)", parameters: new[] { d1 as object, d2 as object });
        //    var t_asmbn = FoxRepo.GetTable<CrystalDS.asmbnRow>(filter: "WHERE (EMPTY(Dop)) OR BETWEEN(Dop,?,?)", parameters: new[] { d1 as object, d2 as object });

        //    ViewBag.Result = (from astob in FoxRepo.GetTable<CrystalDS.astobRow>()
        //                      join askuch in FoxRepo.GetTable<CrystalDS.askuchRow>()
        //                          on astob.kuch equals askuch.kuch
        //                      join mprxop in t_mprxop
        //                         on new { astob.kgt, astob.kus } equals new { mprxop.kgt, mprxop.kus } into proc_parts
        //                      join mpartn in FoxRepo.GetTable<CrystalDS.mpartnRow>()
        //                         on new { astob.kgt, astob.kus } equals new { mpartn.kgt, mpartn.kus } into proc_brak
        //                      join asmbn in t_asmbn
        //                        on new { astob.kgt, astob.kus } equals new { asmbn.kgt, asmbn.kus } into eq_repairs
        //                      let reps = eq_repairs.Sum(asmbn => FoxRepo.HoursBetweenForMZagr(asmbn.dop, asmbn.top, asmbn.dnp, asmbn.tnp))
        //                      select new LoadMth
        //                      {
        //                          Kuch = askuch.nauch,
        //                          Nobr = astob.snaobor,
        //                          Kpls = proc_parts.Sum(a => a.kpls).ToString(),
        //                          Brk = proc_brak.Count().ToString(),
        //                          Nprt = proc_parts.Count().ToString(),
        //                          RepH = reps.ToString("F2"),
        //                          KrObr = ((WorkHrsMonth - reps) / WorkHrsMonth).ToString("F2")
        //                      }).OrderBy(x => x.Kuch);

        //    var reas = from askprob in FoxRepo.GetTable<CrystalDS.askprobRow>()
        //               join asmbn in FoxRepo.GetTable<CrystalDS.asmbnRow>()
        //               on askprob.kpp equals asmbn.kpp
        //               where asmbn.dop <= d2 && asmbn.dop >= d1
        //               group asmbn by new { askprob.napr } into zapr
        //               let reps = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp))
        //               select new Reasons
        //               {
        //                   Napr = zapr.Key.napr,
        //                   SumN = zapr.Select(a => a.kpp).Count().ToString(),
        //                   Hrs = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp)).ToString("F2"),
        //                   KrObr = ((WorkHrsMonth - reps) / WorkHrsMonth).ToString("F2"),
        //               };

        //    ViewBag.reas = ((IEnumerable<LoadMth>)ViewBag.Result).Average(a => decimal.Parse(a.KrObr)).ToString("F2");
        //    ViewBag.Result2 = reas.OrderByDescending(a => a.Hrs);
        //    return View();
        //}

        //public ViewResult ReasonsByUst(string Nobr)
        //{
        //    var d1 = (DateTime)TempData.Peek("Date1");
        //    var d2 = (DateTime)TempData.Peek("Date2");
        //    int hrsMonth = FoxRepo.WorkDaysInMonth(d1) * 24;

        //    var t_asmbn = FoxRepo.GetTable<CrystalDS.asmbnRow>(filter: "WHERE (EMPTY(Dop)) OR BETWEEN(Dop,?,?)", parameters: new[] { d1 as object, d2 as object });
        //    var t_askprob = FoxRepo.GetTable<CrystalDS.askprobRow>();

        //    var reas = from asmbn in t_asmbn
        //               join astob in FoxRepo.GetTable<CrystalDS.astobRow>()
        //                on new { asmbn.kus, asmbn.kgt } equals new { astob.kus, astob.kgt }
        //               where astob.snaobor == Nobr
        //               group asmbn by new { kpp = asmbn.kpp } into zapr
        //               let reps = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp))
        //               let askprb = t_askprob.SingleOrDefault(a => a.kpp == zapr.Key.kpp)
        //               select new Reasons
        //               {
        //                   Napr = askprb == null ? "" : askprb.napr,
        //                   SumN = zapr.Select(a => a.kpp).Count().ToString(),
        //                   Hrs = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp)).ToString("F2"),
        //                   KrObr = ((hrsMonth - reps) / hrsMonth).ToString("F2"),
        //               };
        //    return View(reas);
        //}

        [HttpGet]
        public ActionResult PrtSearch()
        {
            //Session["PageTitle"] = "Поиск партии";
            return View();
        }

        [HttpPost]
        public ActionResult PrtSearch(string query)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "");
                return View("PrtSearchBag", query);
            }



            ViewBag.Nprt = query.PadRight(6);
            ViewBag.userId = Crystal.Authentication.AuthService.GetUserKeyHashCode(User); // System.Web.Security.Membership.GetUser(HttpContext.User.Identity.Name, false).ProviderUserKey.GetHashCode();
            var result = bl.PrtSearch(query);
                         
            return View("PrtSearchResult", result);
        }

        public ActionResult PrtSearchBag()
        {

            return View();
        }

        public JsonResult SearchRes(string query)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            var result = bl.SearchRes(query);
            var ss = result;
            string json = serializer.Serialize(ss);
            return Json(json);
        }

        public ActionResult StandEquipment()
        {
            var result = bl.StandEquipment();
            _log.Info("Пользователь [{0}] просматривает данные по установкам в ремонте на производстве №{1}.", User.Identity.Name, (int)FoxRepo.GetPlant());
            return View(result);
        }
        //public ActionResult OutputStand(string kgt, string kus)
        //{
        //    var gg = (IEnumerable<StandTemp>)TempData.Peek("OutputStand");
        //    var result = from temp in gg.Where(a => a.kgt == kgt && a.kus == kus)
        //                 join askprob in FoxRepo.GetTable<CrystalDS.askprobRow>()
        //                 on temp.kpp equals askprob.kpp into zabr1
        //                 from item in zabr1.DefaultIfEmpty()
        //                 select new
        //                     {
        //                         Kpp = (item == null ? "ещё не определена" : item.napr)
        //                     }

        //    ;
        //    ViewBag.Napr = result.Select(a => a.Kpp);
        //    return View();
        //}
    }
}
