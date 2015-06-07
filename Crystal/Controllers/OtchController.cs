using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Crystal.BusinessLogic;
using Crystal.Models;
using Crystal.VFPOleDBLogic;
using MvcApplication.Models;
using CrystalDataSetLib;
using Crystal.DomainModel;
using NLog;


namespace Crystal.Controllers
{
    [PlantAuthorization]
    public class OtchController : Controller
    {
        ICrystalLogic bl = BusinessLogicFactory.GetBL();
        private static Logger _log = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public ActionResult PribMonthReport(string selPrib, bool ind = false)
        {
            _log.Info("Пользователь [{0}] просматривает отчёт по приборам за месяц на производстве №{1}.", User.Identity.Name, (int)FoxRepo.GetPlant());
            ViewBag.prodN = bl.GetProductionNumber();
            ViewBag.ind = ind;
            ViewBag.kpr = selPrib;
            ViewBag.userId = Crystal.Authentication.AuthService.GetUserKeyHashCode(User);
            var result = bl.PribMonthReport();
            return View(result);

        }

        [HttpGet]
        public ActionResult GetDataBySelectedPrib(string selPrib)
        {
            TempData["PribK"] = selPrib;
            var result = bl.GetDataBySelectedPrib(selPrib);
            return View(result);
        }

        [HttpGet]
        public ActionResult  PribMonthRepordBrak(string nop)
        {

            ViewBag.prodN = bl.GetProductionNumber();
            ViewBag.userId = Crystal.Authentication.AuthService.GetUserKeyHashCode(User);
            var result = bl.PribMonthRepordBrak(nop);

            return View(result);
        }

        //[HttpGet]
        //public ActionResult PribMonthRepordNzp(string nop)
        //{
        //    var kpr = (string)TempData.Peek("PribK");
        //    var tempMpartt = FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "WHERE Kpr==? AND nop == ?", parameters: new[] { kpr, nop });
        //    var result = tempMpartt.Select(
        //            x => new MonthReportDataNzp
        //                {
        //                    Nprt = x.nprt,
        //                    Date = x.dato.isNullDate() != true ? x.dato.ToShortDateString() : x.dath.ToShortDateString(),
        //                    Kpls = x.kpls.ToString(),
        //                    DaysOnOp = x.dato.isNullDate() != true ? (DateTime.Now - x.dato).TotalDays.ToString("F0") : (DateTime.Now - x.dath).TotalDays.ToString("F0")
        //                });
        //    return View(result);
        //}

        [HttpGet]
        public ActionResult NzpPrib(string head, string Kpr, bool ind = false) //Формируем список приборов
        {
            ViewBag.kpr = Kpr;
            ViewBag.ind = ind;
            IEnumerable<NzpPribList> res;
            res = bl.NzpPrib();
            ViewBag.prodN = bl.GetProductionNumber();
            ViewBag.userId = Crystal.Authentication.AuthService.GetUserKeyHashCode(User);
            ViewBag.head = head ?? string.Format("НЗП производства № {0}", bl.GetProductionNumber());
            _log.Info("Пользователь [{0}] просматривает НЗП по приборам за месяц на производстве №{1}.", User.Identity.Name, (int)FoxRepo.GetPlant());
            return View(res);
        }

        public IEnumerable<NzpPrib> GetNzpForPrib(string kpr)
        {

            var t_assop = FoxRepo.GetTable<CrystalDS.assopRow>(); //filter: "WHERE pfl!='5'"
            var t_mpartt = FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "WHERE kmk!='000' AND Kpr==?", parameters: new[] { kpr });

            var kmk = t_mpartt.First().kmk;

            var t_astmr = FoxRepo.GetTable<CrystalDS.astmrRow>(filter: "WHERE kmk==?", parameters: new[] { kmk });

            var data = (from astmr in t_astmr
                        let ap = t_assop.Single(assop => assop.kop == astmr.kop)
                        let pfl = ap.pfl
                        select new NzpPrib
                        {
                            Nop = astmr.nop,
                            Naop = ap.naop,
                            Nzp = Convert.ToInt32(t_mpartt.Where(m => m.nop == astmr.nop).Sum(a => a.kpls)),
                            Pfl = pfl
                        });
            return data;
        }
        public IEnumerable<NzpPribList> NzpForPrib(IEnumerable<CrystalDS.assopRow> t_assop)
        {

            var data = (from mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "WHERE nop!='0000'")
                        join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                         on mpartt.kpr equals asspr.kpr
                        join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
                        on new { mpartt.kmk, mpartt.nop } equals new { astmr.kmk, astmr.nop }
                        join assop in t_assop
                        on astmr.kop equals assop.kop
                        group mpartt by new { mpartt.kpr, asspr.napr } into g_mpartt
                        select new NzpPribList
                        {
                            Kpr = g_mpartt.Key.kpr,
                            Napr = g_mpartt.Key.napr,
                            Nzp = Convert.ToInt32(g_mpartt.Sum(a => a.kpls)),


                        }).OrderBy(a => a.Kpr);
            return data;
        }

        [HttpGet]
        public ActionResult GetNzpPrib(string kpr) //Выводим данные по запросу из списка
        {
            ViewBag.kpr = kpr;
            var data = bl.GetNzpPrib(kpr);

            return View(data);
        }

        [HttpGet]
        public ViewResult GetFromNzp(string kpr, string nop)//Раскрываем НЗП
        {
            var data = bl.GetFromNzp(kpr, nop);
            ViewBag.prodN = bl.GetProductionNumber();
            ViewBag.userId = Crystal.Authentication.AuthService.GetUserKeyHashCode(User);
            return View(data);
        }

        public ActionResult NzpFlit() //Список приборов на фотолитографию
        {

            ViewBag.userId = Crystal.Authentication.AuthService.GetUserKeyHashCode(User);
            ViewBag.prodN = bl.GetProductionNumber();
            IEnumerable<NzpFLitList> res;
            res = bl.NzpFlit();
            return View(res);
        }

        public ViewResult GetNzpFLit(string kpr, string kmk) //Выводим данные по запросу из списка
        {
            IEnumerable<NzpFLit> res;
            ICrystalLogic bl = BusinessLogicFactory.GetBL();
            res = bl.GetNzpFLit(kpr, kmk);


            //string nnop = "0001";
            //int old_nbl, nbl;
            //var data = from astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
            //           join assop in FoxRepo.GetTable<CrystalDS.assopRow>() on astmr.kop equals assop.kop
            //           where astmr.kmk == kmk && assop.pfl != "5"
            //           orderby astmr.nop
            //           select new { pfl = assop.pfl, nop = astmr.nop, naop = assop.naop };

            //var blks = new List<Blocks>();

            //nbl = 0;
            //old_nbl = -1;

            //foreach (var tab in data)
            //{
            //    if (old_nbl != nbl) nnop = tab.nop;
            //    old_nbl = nbl;
            //    if (tab.pfl.Trim() != "5")
            //    {
            //        if (tab.pfl.Trim() == "1")
            //        {
            //            nbl++;
            //            blks.Add(new Blocks { Pfl = tab.pfl, FNop = nnop, LNop = tab.nop, BlockN = nbl.ToString() });
            //        }
            //    }
            //}

            //var tool = from holl in data
            //           join bl in blks on holl.nop equals bl.LNop
            //           select new { naop = holl.naop, fop = bl.LNop };

            //TempData["Blocks"] = blks;
            //var tabl = from mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>()
            //           join ss in data on mpartt.nop equals ss.nop
            //           let nbloka = blks.Where(a => int.Parse(a.FNop) <= int.Parse(mpartt.nop) && int.Parse(a.LNop) >= int.Parse(mpartt.nop) && mpartt.kmk == kmk && mpartt.kpr == kpr)
            //           select new { pfl = ss.pfl, prt = mpartt, nbl = nbloka.Count() > 0 ? nbloka.Single().BlockN : "0" };

            //var result = from t in tabl
            //             join haf in blks on t.nbl equals haf.BlockN
            //             join hex in tool on haf.LNop equals hex.fop
            //             group t by new { t.nbl, haf.LNop, haf.BlockN, hex.naop, haf.Pfl } into g_tabl
            //             select new NzpFLit
            //             {
            //                 Pfl = g_tabl.Key.Pfl,
            //                 Nfl = g_tabl.Key.nbl,
            //                 Nop = g_tabl.Key.LNop,
            //                 Naop = g_tabl.Key.naop,
            //                 KolPr = g_tabl.Count().ToString(),
            //                 Nzp = g_tabl.Sum(a => a.prt.kpls).ToString(),
            //                 Kmk = kmk,
            //                 Kpr = kpr
            //             };
            return View(res);
        }

        public ViewResult GetFromNzpFlit(string nfl, string kmk, string kpr)//Получаем блок и выводим нзп по операциям
        {
             IEnumerable<FlitOprNzp> res;
            res = bl.GetFromNzpFlit(nfl,kmk,kpr);
            return View(res);
         
            //var someblocks = (IEnumerable<Blocks>)TempData.Peek("Blocks");

            //var part = from assop in FoxRepo.GetTable<CrystalDS.assopRow>()
            //           where assop.pfl != "5"
            //           join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>() on assop.kop equals astmr.kop
            //           join mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>() on new { astmr.nop, astmr.kmk } equals new { mpartt.nop, mpartt.kmk }
            //           where mpartt.kmk == kmk && mpartt.kpr == kpr
            //           from kt in someblocks
            //           where kt.BlockN == nfl
            //           where int.Parse(mpartt.nop) >= int.Parse(kt.FNop) && int.Parse(mpartt.nop) <= int.Parse(kt.LNop)
            //           group mpartt by new { mpartt.nop, mpartt.nprt, mpartt.kpls, assop.naop, mpartt.dato, mpartt.dath, mpartt.timo, mpartt.timh } into g_mpartt
            //           select new FlitPartNzp
            //           {
            //               Nprt = g_mpartt.Key.nprt,
            //               Naop = g_mpartt.Key.naop,
            //               Dato = g_mpartt.Key.dato.GetFoxDate() != "" ? g_mpartt.Key.dato.GetFoxDate() : g_mpartt.Key.dath.GetFoxDate(),
            //               Timo = g_mpartt.Key.timo.GetFoxTime() != "" ? g_mpartt.Key.timo.GetFoxTime() : g_mpartt.Key.timh.GetFoxTime(),
            //               Nop = g_mpartt.Key.nop,
            //               Nzp = g_mpartt.Key.kpls.ToString()
            //           };

            //var part1 = from az in part
            //            group az by new { az.Nop, az.Naop } into xz
            //            select new FlitOprNzp { Nop = xz.Key.Nop, Naop = xz.Key.Naop, Nzp = xz.Sum(a => int.Parse(a.Nzp)).ToString() };
            //ViewBag.kmk = kmk;
            //TempData["Parts"] = part;
            //return View(part1.OrderBy(a => int.Parse(a.Nop)));
        }

        public ViewResult GetFromNzpFlit2(string nop) //От операции получаем партии
        {
            ViewBag.prodN = bl.GetProductionNumber();
            IEnumerable<FlitPartNzp> res;
            res = bl.GetFromNzpFlit2(nop);
            return View(res);
        }

        public ActionResult DayReports()
        {
            var uchList = from uch in FoxRepo.GetTable<CrystalDS.askuchRow>()
                          select new UchSelect { Kuch = uch.kuch, Nauch = uch.nauch };
            ViewBag.Kuch = uchList.OrderBy(x => x.Kuch);
            return View();
        }

        public ViewResult GetByUch(string kuch)
        {
            DateTime thisDay = DateTime.Today;

            var result = (from astob in FoxRepo.GetTable<CrystalDS.astobRow>()
                          where astob.kuch == kuch
                          join mprxop in FoxRepo.GetTable<CrystalDS.mprxopRow>()
                              on new { astob.kgt, astob.kus } equals new { mprxop.kgt, mprxop.kus }
                          where mprxop.dato.ToShortDateString() == thisDay.ToShortDateString()
                          group mprxop by mprxop.kmk into g_mprxop
                          select new ReportsDay { Nzp = g_mprxop.Sum(x => x.kpls).ToString(), Income = "", Processed = "", TrCof = "" });

            IEnumerable<ReportShift> result2 = new[]
                {
                    new ReportShift{Income = "21", Processed = "22", Nzp = "23", TrCof = "24", Shift = "25"}
                };
            ViewBag.forDay = result;
            ViewBag.forShift = result2;
            return View();
        }

        [HttpGet]
        public ViewResult GetAllUch()
        {
            IEnumerable<ReportsDayAll> result1 = new[]
                {
                    new ReportsDayAll{Income = "11", Processed = "12", Nzp = "13", TrCof = "14", Nuch = "some uch"}
                };

            IEnumerable<ReportShiftAll> result2 = new[]
                {
                    new ReportShiftAll{Income = "21", Processed = "22", Nzp = "23", TrCof = "24", Shift = "25", Nuch = "someuch"}
                };
            ViewBag.forDay = result1;
            ViewBag.forShifts = result2;
            return View();
        }
        public ActionResult CrystalForm()
        {
            return View();
        }

        public ActionResult Crystals(string fdate, string sdate)
        {
            ViewBag.Prn = (from asparam in FoxRepo.GetTable<CrystalDS.asparamRow>()
                           where asparam.identp.Trim() == "FACTORY"
                           select asparam.valp).First().Trim();
            DateTime d1 = DateTime.Parse(fdate);
            DateTime d2 = DateTime.Parse(sdate);
            ViewBag.CrystalsDate1 = d1;
            ViewBag.CrystalsDate2 = d2;

            if (HttpContext.Session != null) HttpContext.Session.Add("__CrystalsDate1", d1);
            if (HttpContext.Session != null) HttpContext.Session.Add("__CrystalsDate2", d2);

            var mkristTable = FoxRepo.GetTable<CrystalDS.mkristRow>(filter: "WHERE BETWEEN(datop,?,?)", parameters: new[] { d1 as object, d2 as object });
            var amkristTable = FoxRepo.GetTable<CrystalDS.mkristRow>(isArchived: true, filter: "WHERE BETWEEN(datop,?,?)", parameters: new[] { d1 as object, d2 as object });

            var result = from asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                         join mkrist in mkristTable.Where(a => a.godn > 0)
                         on asspr.kpr equals mkrist.kpr into zapr
                         join amkrist in amkristTable.Where(a => a.godn > 0)
                         on asspr.kpr equals amkrist.kpr into zapr1
                         let sum_pr = zapr.Sum(a => a.pkrist) + zapr1.Sum(b => b.pkrist)
                         let sum_gd = zapr.Sum(a => a.godn) + zapr1.Sum(b => b.godn)
                         where sum_pr > 0 && sum_gd > 0
                         select new Crystals
                         {
                             Kpr = asspr.kpr,
                             Napr = asspr.napr,
                             Pkrist = sum_pr.ToString(),
                             Godn = sum_gd.ToString(),
                             ProcVux = ((sum_gd / sum_pr) * 100).ToString("F2")
                         };
            return View(result);
        }
        [HttpGet]
        public ActionResult CrystalsByMonth(string month, string year)
        {
            DateTime date1 = DateTime.Parse(("01." + month + "." + year));
            DateTime date2 = DateTime.Parse(DateTime.DaysInMonth(int.Parse(year), int.Parse(month)).ToString() + "." + month + "." + year);

            if (HttpContext.Session != null) HttpContext.Session.Add("__CrystalsDate1", date1);
            if (HttpContext.Session != null) HttpContext.Session.Add("__CrystalsDate2", date2);

            var mkristTable = FoxRepo.GetTable<CrystalDS.mkristRow>(filter: "WHERE BETWEEN(datop,?,?)", parameters: new[] { date1 as object, date2 as object });
            var amkristTable = FoxRepo.GetTable<CrystalDS.mkristRow>(isArchived: true, filter: "WHERE BETWEEN(datop,?,?)", parameters: new[] { date1 as object, date2 as object });

            var result = from asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                         join mkrist in mkristTable.Where(a => a.godn > 0)
                         on asspr.kpr equals mkrist.kpr into zapr
                         join amkrist in amkristTable.Where(a => a.godn > 0)
                         on asspr.kpr equals amkrist.kpr into zapr1
                         let sum_pr = zapr.Sum(a => a.pkrist) + zapr1.Sum(b => b.pkrist)
                         let sum_gd = zapr.Sum(a => a.godn) + zapr1.Sum(b => b.godn)
                         where sum_pr > 0 && sum_gd > 0
                         select new Crystals
                         {
                             Kpr = asspr.kpr,
                             Napr = asspr.napr,
                             Pkrist = sum_pr.ToString(),
                             Godn = sum_gd.ToString(),
                             ProcVux = ((sum_gd / sum_pr) * 100).ToString("F2")
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult CrystalsByMonth2()
        //{
        //    object d1 = null, d2 = null;

        //    if (HttpContext.Session != null)
        //    {
        //        d1 = HttpContext.Session["__CrystalsDate1"];
        //        d2 = HttpContext.Session["__CrystalsDate2"];
        //    }

        //    var mkristTable = FoxRepo.GetTable<CrystalDS.mkristRow>(filter: "WHERE BETWEEN(datop,?,?)", parameters: new[] { d1, d2 });
        //    var amkristTable = FoxRepo.GetTable<CrystalDS.mkristRow>(isArchived: true, filter: "WHERE BETWEEN(datop,?,?)", parameters: new[] { d1, d2 });


        //    var result = from asabon in FoxRepo.GetTableSql<CrystalDS.asabonRow>(sql: "select Tbn, Fio from asabon group by Tbn, Fio")
        //                 join mkrist in mkristTable
        //                 on asabon.tbn equals mkrist.owner into zapr
        //                 join amkrist in amkristTable
        //                 on asabon.tbn equals amkrist.owner into zapr1
        //                 let sum_pr = zapr.Sum(a => a.pkrist) + zapr1.Sum(b => b.pkrist)
        //                 let sum_gd = zapr.Sum(a => a.godn) + zapr1.Sum(b => b.godn)
        //                 where sum_pr > 0 && sum_gd > 0
        //                 select new CrystalsSecond
        //                 {
        //                     Fio = asabon.fio,
        //                     Pkrist = sum_pr.ToString(),
        //                     Godn = sum_gd.ToString(),
        //                     ProcVux = ((sum_gd / sum_pr) * 100).ToString("F2")
        //                 };
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Defect()
        {
            ViewBag.Action = "DefectsDistr";
            ViewBag.Controller = "Otch";
            ViewBag.Title = "Отчет по браку за месяц";
            return View("DateForm");

        }

        public ActionResult DefectsDistr(string fdate, string sdate, string indnumb)
        {
            ViewBag.Period = FoxRepo.GetPeriod(fdate, sdate, indnumb);
            ViewBag.Prn = bl.GetProductionNumber();
            DateTime d1 = DateTime.Parse(fdate);
            DateTime d2 = DateTime.Parse(sdate);
            
            var model = bl.DefectsDistr(fdate, sdate, indnumb);
            return View(model);
        }

        public IEnumerable<DefectsDistributionPrib> DefectsDistrForPlant(DateTime d1, DateTime d2, IEnumerable<CrystalDS.assopRow> t_assop)
        {
            var ampartnTable =
                from ampartn in
                    FoxRepo.GetTable<CrystalDS.mpartnRow>(isArchived: true, filter: "WHERE BETWEEN(datbr,?,?)",
                                                        parameters: new[] { d1 as object, d2 as object })
                join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>() on new { ampartn.nop, ampartn.kmk } equals
                    new { astmr.nop, astmr.kmk }
                join assop in t_assop on astmr.kop equals assop.kop
                select ampartn;
            var mpartnTable =
                from mpartn in
                    FoxRepo.GetTable<CrystalDS.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?)",
                                                        parameters: new[] { d1 as object, d2 as object })
                join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>() on new { mpartn.nop, mpartn.kmk } equals
                    new { astmr.nop, astmr.kmk }
                join assop in t_assop on astmr.kop equals assop.kop
                select mpartn;
            var result = from asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                         join ampartn in ampartnTable
                             on asspr.kpr equals ampartn.kpr into zapr1
                         join mpartn in mpartnTable
                             on asspr.kpr equals mpartn.kpr into zapr2
                         let sumNzp = zapr1.Count() + zapr2.Count()
                         select new DefectsDistributionPrib
                             {
                                 Kpr = asspr.kpr,
                                 Napr = asspr.napr,
                                 Nzp = sumNzp
                             };
            return result;
        }

        public ActionResult DefectsDistrFor2Plant(string fdate, string sdate, string head)
        {
            ViewBag.Period = FoxRepo.GetPeriod(fdate, sdate,"2");
            ViewBag.Prn = bl.GetProductionNumber();
            ViewBag.task = "DefectsDistrFor2Plant";
            var result = bl.DefectsDistrFor2Plant(fdate, sdate, head);
            return View("DefectsDistr", result.Where(x => x.Nzp != 0).OrderBy(x => x.Kpr));

        }
        public void DefectsDistrOperationsForPlant(string kpr, object d1, object d2, IEnumerable<CrystalDS.assopRow> t_assop, out string Kmk, out IEnumerable<DefectsDistributionOperations> result)
        {
            var ampartnTable = FoxRepo.GetTable<CrystalDS.mpartnRow>(isArchived: true, filter: "WHERE BETWEEN(datbr,?,?) AND kpr == ?", parameters: new[] { d1, d2, kpr });
            var mpartnTable = FoxRepo.GetTable<CrystalDS.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?) AND kpr == ?", parameters: new[] { d1, d2, kpr });

            Kmk = (from asspr in FoxRepo.GetTable<CrystalDS.assprRow>(filter: "WHERE kpr == ?", parameters: new[] { kpr })
                   select new { kmk = asspr.kmk }).FirstOrDefault().kmk;


            var pre_result = (from mpartn in mpartnTable
                              group mpartn by mpartn.nop
                                  into gMpartn
                                  select new
                                      {
                                          nop = gMpartn.Key,
                                          pls = gMpartn.Count()
                                      }).Concat(from ampartn in ampartnTable
                                                group ampartn by ampartn.nop
                                                    into gaMpartn
                                                    select new
                                                    {
                                                        nop = gaMpartn.Key,
                                                        pls = gaMpartn.Count()
                                                    });
            result = from tester in pre_result
                     group tester by tester.nop
                         into gtest
                         join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>(filter: "WHERE kmk == ?", parameters: new[] { Kmk })
                         on gtest.Key equals astmr.nop
                         join assop in t_assop
                         on astmr.kop equals assop.kop
                         select new DefectsDistributionOperations
                         {
                             Nop = gtest.Key,
                             Naop = assop.naop,
                             Nzp = gtest.Sum(a => a.pls).ToString()
                         };

        }

        [HttpGet]
        public ViewResult DefectsDistrOperations(string kpr)
        {

            //var d1 = TempData.Peek("d1");
            //var d2 = TempData.Peek("d2");

            
            //IEnumerable<DefectsDistributionOperations> result;
            //DefectsDistrOperationsForPlant(kpr, d1, d2, FoxRepo.GetTable<CrystalDS.assopRow>(filter: "WHERE pfl!='5'"), out Kmk, out result);
            ViewBag.Kpr = kpr;
          

            var model = bl.DefectsDistrOperations(kpr);
            return View(model);
        }

        public ViewResult DefectsDistrOperationsFor2Plant(string kpr)
        {
            
            IEnumerable<DefectsDistributionOperations> result;
            result = bl.DefectsDistrOperationsFor2Plant(kpr);
            ViewBag.Kpr = kpr;
            return View("DefectsDistrOperations", result.OrderBy(a => a.Nop));
        }

        [HttpGet]
        public ViewResult DefectsDistrData(string nop, string kpr, string kmk)
        {
          var model = bl.DefectsDistrData(nop,kpr,kmk);
                                       
            return View(model);
        }

        public ActionResult DefectsDistrByMonth(string month, string year)
        {
            DateTime d1 = DateTime.Parse(("01." + month + "." + year));
            DateTime d2 = DateTime.Parse(DateTime.DaysInMonth(int.Parse(year), int.Parse(month)).ToString() + "." + month + "." + year);

            TempData["d1"] = d1;
            TempData["d2"] = d2;

            var ampartnTable = FoxRepo.GetTable<CrystalDS.mpartnRow>(isArchived: true, filter: "WHERE BETWEEN(datbr,?,?)", parameters: new[] { d1 as object, d2 as object });
            var mpartnTable = FoxRepo.GetTable<CrystalDS.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?)", parameters: new[] { d1 as object, d2 as object });

            var result = from asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                         join ampartn in ampartnTable
                         on asspr.kpr equals ampartn.kpr into zapr1
                         join mpartn in mpartnTable
                         on asspr.kpr equals mpartn.kpr into zapr2
                         let sumNzp = zapr1.Count() + zapr2.Count()
                         select new DefectsDistributionPrib
                         {
                             Kpr = asspr.kpr,
                             Napr = asspr.napr,
                             Nzp = sumNzp
                         };
            return View(result.Where(x => x.Nzp != 0).OrderBy(x => x.Kpr));
        }

        public ActionResult NZPFor2Plant(string head, bool ind = false)
        {
            ViewBag.ind = ind;
            ViewBag.timer = "GetNZPFor2Plant";
            IEnumerable<NzpPribList> res;
            res = bl.NZPFor2Plant();
            ViewBag.head = head ?? string.Format("НЗП производства № {0} [пр-во №2]", bl.GetProductionNumber());
            ViewBag.userId = Crystal.Authentication.AuthService.GetUserKeyHashCode(User);
            return View("NzpPrib", res);
        }
        public ActionResult GetNZPFor2Plant(string kpr)
        {
            ViewBag.kpr = kpr;
            ICrystalLogic bl = BusinessLogicFactory.GetBL();
            var data = bl.GetNZPFor2Plant(kpr);
            return View("GetNzpPrib", data);

        }
        public ActionResult PribKplsForDay()
        {
            var model = bl.PribKplsForDay();
            //var model = (from asmnp in FoxRepo.GetTable<CrystalDS.asmnpRow>()
            //             join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
            //                 on asmnp.kpr equals asspr.kpr
            //             group asmnp by new { asmnp.kpr, asspr.napr } into zabr1
            //             select new PribKplsForDay
            //                 {
            //                     Kpr = zabr1.Key.kpr,
            //                     Napr = zabr1.Key.napr,
            //                     KplsForDay = zabr1.Sum(a => a.sdo2).ToString()
            //                 }).OrderBy(a => a.Kpr).Where(a => a.KplsForDay.Trim() != "0");
            return PartialView(model);
        }

        public ActionResult GangPribKplsForDay(string kpr)
        {
            var model = bl.GangPribKplsForDay(kpr);
            //var model =
            //    (from asmnp in FoxRepo.GetTable<CrystalDS.asmnpRow>(filter: "WHERE Kpr==?", parameters: new[] { kpr })
            //     join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
            //         on new { asmnp.kmk, asmnp.nop } equals new { astmr.kmk, astmr.nop }
            //     join assop in FoxRepo.GetTable<CrystalDS.assopRow>()
            //         on astmr.kop equals assop.kop
            //     select new GangPribKplsForDay
            //         {
            //             Nop = asmnp.nop,
            //             Naop = assop.naop,
            //             KplsForNop = asmnp.sdo2.ToString()
            //         }).OrderBy(a => a.Nop).Where(a => a.KplsForNop.Trim() != "0");

            return PartialView(model);
        }


        class MprxopTour
        {
            public DateTime TourDate { get; set; }
            public string TourNumber { get; set; }
            public int Kpls { get; set; }
        }

        MprxopTour GetTourData(CrystalDS.mprxopRow row, IEnumerable<CrystalDS.asgsmRow> asgsm)
        {
            var ToursCount = asgsm.Count();
            var tour = asgsm.SingleOrDefault(a => String.CompareOrdinal(row.timo, a.tns) >= 0 && String.CompareOrdinal(row.timo, a.tos) < 0);
            return new MprxopTour
            {
                TourDate = FoxRepo.OneColDateTime(tour == null ? row.dato.AddDays(-1) : row.dato, row.timo),
                TourNumber = tour == null ? ToursCount.ToString() : tour.nsm,
                Kpls = Convert.ToInt32(row.kpls)
            };
        }

        public ActionResult PribKplsForMonth()
        {

            ViewBag.prodN = bl.GetProductionNumber();
            var model = bl.PribKplsForMonth();
            //var start = FoxRepo.GetMonthStartDate();
            //var block = new List<Temporary>();

            //object d1 = start.Date;
            //string time = start.ToString("HHmm");
            //var t_mprxop =
            //    FoxRepo.GetTable<CrystalDS.mprxopRow>(filter: "WHERE dato>? or ((dato=?) and timo>?)", parameters: new[] { d1, d1, time }).Concat(FoxRepo.GetTable<CrystalDS.mprxopRow>(isArchived: true, filter: "WHERE dato>? or ((dato=?) and timo>?)", parameters: new[] { d1, d1, time }));

            //// Таблица рабочих смен
            //var t_asgsm = FoxRepo.GetTable<CrystalDS.asgsmRow>();

            //var ToursCount = t_asgsm.Count();

            //// TODO: определить смену которая между сутками, а сейчас это последняя смена в списке

            //ViewBag.Count = ToursCount;

            //var mprxop_tours = from mprxop in t_mprxop
            //                   select GetTourData(mprxop, t_asgsm);


            //var model = (from mprxop in mprxop_tours
            //             group mprxop by new { mprxop.TourDate.Date } into zabr1
            //             select new PribKplsForMounth
            //             {
            //                 Date = zabr1.Key.Date.ToString(),
            //                 KplsForDay = zabr1.Sum(b => b.Kpls).ToString(),
            //                 KplsForFirstTour = zabr1.Where(a => a.TourNumber == "1").Sum(b => b.Kpls).ToString(),
            //                 KplsForSecondTour = zabr1.Where(a => a.TourNumber == "2").Sum(b => b.Kpls).ToString(),
            //                 KplsForThirdTour = zabr1.Where(a => a.TourNumber == "3").Sum(b => b.Kpls).ToString(),
            //             }).OrderBy(a => DateTime.Parse(a.Date));

            return View(model);
        }
    }
}
