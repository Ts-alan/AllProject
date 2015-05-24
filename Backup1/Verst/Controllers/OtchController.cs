using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Verst.Models;
using MvcApplication.Models;


namespace Verst.Controllers
{
    [PlantAuthorization]
    public class OtchController : Controller
    {
        [HttpGet]
        public ActionResult PribMountReport()
        {
            var result = (from mpartt in FoxRepo.GetTable<Crystal.mparttRow>()
                          join asspr in FoxRepo.GetTable<Crystal.assprRow>()
                           on mpartt.kpr equals asspr.kpr
                          group mpartt by new { mpartt.kpr, asspr.napr, asspr.kmk } into g_mpartt
                          select new SimplePribList()
                          {
                              Kpr = g_mpartt.Key.kpr,
                              Napr = g_mpartt.Key.napr,
                              Nzp = g_mpartt.Sum(x => x.kpls)
                          }).OrderBy(a => a.Kpr);
            return View(result);
            
        }

        [HttpGet]
        public ActionResult GetDataBySelectedPrib(string selPrib)
        {
            TempData["PribK"] = selPrib;
            var result = from asmnp in FoxRepo.GetTable<Crystal.asmnpRow>(filter:"Where Kpr == ?", parameters:new[]{selPrib})
                         join astmr in FoxRepo.GetTable<Crystal.astmrRow>() 
                         on new {asmnp.nop, asmnp.kmk} equals new {astmr.nop, astmr.kmk}
                         join assop in FoxRepo.GetTable<Crystal.assopRow>() 
                         on astmr.kop equals assop.kop
                         group asmnp by new {asmnp.nop, assop.naop} into gResult
                         let sumPso = gResult.Sum(x=>x.pso3)
                         let sumSdo = gResult.Sum(x=>x.sdo3)
                         let sumBrk = gResult.Sum(x=>x.brk3)
                         let sumNzpo = gResult.Sum(x=>x.nzpo)
                           select new MonthReportData
                               {
                                   Nop = gResult.Key.nop,
                                   Naop = gResult.Key.naop,
                                   In = sumPso.ToString(),
                                   Out = sumSdo.ToString(),
                                   Brk = sumBrk.ToString(),
                                   OutPer = ((sumSdo != 0 ? (sumSdo / (sumSdo + sumBrk)) : 0)*100).ToString("F2"),
                                   Nzpf = ((sumNzpo + sumPso) - sumSdo - sumBrk).ToString("F0")
                               };
            return View(result.Where(x=>x.Brk!="0" || x.In!="0" || x.Out!="0"|| x.Nzpf!="0").OrderBy(x=>int.Parse(x.Nop)));
        }

        [HttpGet]
        public ActionResult PribMounthRepordBrak(string nop)
        {
            var kpr = (string)TempData.Peek("PribK");
            var d2 = DateTime.Now;
            var d1 = DateTime.Parse(("01." + d2.Month + "." + d2.Year));
            object date1 = d1;
            object date2 = d2;
            var tempMpartn = FoxRepo.GetTable<Crystal.mpartnRow>(filter: "WHERE BETWEEN(Datbr,?,?) AND Kpr==? AND Nop == ?", parameters: new[] { date1, date2, kpr, nop});
            var tempAmpartn = FoxRepo.GetTable<Crystal.mpartnRow>(isArchived: true, filter: "WHERE BETWEEN(Datbr,?,?) AND Kpr == ? AND Nop == ?", parameters: new[] { date1, date2, kpr, nop });

            var result = from table in tempAmpartn.Concat(tempMpartn)
                         join asbrak in FoxRepo.GetTable<Crystal.asbrakRow>()
                             on table.kprb equals asbrak.kprb
                         join astob in FoxRepo.GetTable<Crystal.astobRow>()
                         on new { table.kus, table.kgt } equals new { astob.kus, astob.kgt }
                         group table by new { table.nprt, asbrak.naprb, astob.snaobor, table.datbr } into gResult
                         select new MonthReportDataBrak
                             {
                                 Nprt = gResult.Key.nprt,
                                 Nzp = gResult.Count().ToString(),
                                 Naprb = gResult.Key.naprb,
                                 Npls = gResult.Select(x=>x.npls.Substring(11,2)),
                                 Ust = gResult.Key.snaobor,
                                 Date = gResult.Key.datbr.ToShortDateString(),
                             };
            
            return View(result);
        }

        [HttpGet]
        public ActionResult PribMounthRepordNzp(string nop)
        {
            var kpr = (string)TempData.Peek("PribK");
            var tempMpartt = FoxRepo.GetTable<Crystal.mparttRow>(filter: "WHERE Kpr==? AND nop == ?", parameters: new[] { kpr, nop });
            var result = tempMpartt.Select(
                    x =>new MonthReportDataNzp
                        {
                            Nprt = x.nprt,
                            Date = x.dato.isNullDate() != true ? x.dato.ToShortDateString() : x.dath.ToShortDateString(),
                            Kpls = x.kpls.ToString(),
                            DaysOnOp = x.dato.isNullDate() != true ? (DateTime.Now - x.dato).TotalDays.ToString("F0") : (DateTime.Now - x.dath).TotalDays.ToString("F0")
                        });
            return View(result);
        }

        [HttpGet]
        public ActionResult NzpPrib() //Формируем список приборов
        {
            var pribL = (from mpartt in FoxRepo.GetTable<Crystal.mparttRow>(filter:"WHERE nop!='0000'")
                       join asspr in FoxRepo.GetTable<Crystal.assprRow>()
                        on mpartt.kpr equals asspr.kpr
                         join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                          on new { mpartt.kmk, mpartt.nop } equals new { astmr.kmk, astmr.nop }
                         join assop in FoxRepo.GetTable<Crystal.assopRow>(filter: "WHERE pfl!='5'")
                         on astmr.kop equals assop.kop
                       group mpartt by new {mpartt.kpr, asspr.napr} into g_mpartt
                       select new NzpPribList
                       {
                           Kpr = g_mpartt.Key.kpr,
                           Napr = g_mpartt.Key.napr,
                           Nzp = g_mpartt.Sum(a=> a.kpls)
                       }).OrderBy(a=>a.Kpr);
            return View(pribL);
        }

        [HttpGet]
        public ActionResult GetNzpPrib(string kpr) //Выводим данные по запросу из списка
        {
            ViewBag.kpr = kpr;
            var data = (from mpartt in FoxRepo.GetTable<Crystal.mparttRow>(filter: "WHERE Kpr==?", parameters: new[] {kpr})
                       join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                        on new{mpartt.kmk, mpartt.nop} equals new{astmr.kmk, astmr.nop}
                        join assop in FoxRepo.GetTable<Crystal.assopRow>(filter: "WHERE pfl!='5'")
                        on astmr.kop equals assop.kop
                       group mpartt by new{ mpartt.nop, assop.naop} into g_mpartt 
                       select new NzpPrib
                       {
                           Nop = g_mpartt.Key.nop,
                           Naop = g_mpartt.Key.naop,
                           Nzp = g_mpartt.Sum(a=> a.kpls).ToString()
                       }).OrderBy(a => a.Nop);
            return View(data);
        }

        [HttpGet]
        public ViewResult GetFromNzp(string kpr, string nop)//Раскрываем НЗП
        {
            var tempAsabon = FoxRepo.GetTable<Crystal.asabonRow>();
            //ViewBag.observed = (new Verst.Models.SiteAccountsTableAdapters.ObservedBatchesTableAdapter()).GetData();
            var data = (from mpartt in FoxRepo.GetTable<Crystal.mparttRow>()//(filter: "WHERE Kpr==? AND nop == ?", parameters: new[] { kpr, nop })
                       where mpartt.kpr == kpr && mpartt.nop == nop
                        
                       select new Par
                       {
                           Nprt = mpartt.nprt,
                           Dato = mpartt.dato.GetFoxDate() != "" ? mpartt.dato.GetFoxDate() : mpartt.dath.GetFoxDate(),
                           Timo = FoxRepo.GetFoxTime(mpartt.timo) != "" ? FoxRepo.GetFoxTime(mpartt.timo) : FoxRepo.GetFoxTime(mpartt.timh),
                           Datf = "",
                           Kpls = mpartt.kpls.ToString(),
                           Abon = FoxRepo.GetFioByTabNumber(mpartt.owner, tempAsabon),
                           Dath=mpartt.dath.GetFoxDate(),
                           Timh=FoxRepo.GetFoxTime(mpartt.timh),
                           kpr = kpr
                       }).ToList();
            return View(data);
        }

        public ActionResult NzpFlit() //Список приборов на фотолитографию
        {
            var pribL = (from mpartt in FoxRepo.GetTable<Crystal.mparttRow>()
                              join asspr in FoxRepo.GetTable<Crystal.assprRow>()
                               on mpartt.kpr equals asspr.kpr
                              group mpartt by new { mpartt.kpr, asspr.napr, asspr.kmk } into g_mpartt
                              select new NzpFLitList
                              {
                                  Kpr = g_mpartt.Key.kpr,
                                  Napr = g_mpartt.Key.napr,
                                  Kmk = g_mpartt.Key.kmk,
                                  Nzp = g_mpartt.Sum(x=>x.kpls)
                              }).OrderBy(a => a.Kpr);
            return View(pribL);
        }

        public ViewResult GetNzpFLit(string kpr, string kmk) //Выводим данные по запросу из списка
        {
            string nnop = "0001";
            int old_nbl, nbl;
            var data = from astmr in FoxRepo.GetTable<Crystal.astmrRow>()
            join assop in FoxRepo.GetTable<Crystal.assopRow>() on astmr.kop equals assop.kop
            where astmr.kmk==kmk && assop.pfl != "5"
            orderby astmr.nop
            select new { pfl = assop.pfl, nop = astmr.nop, naop = assop.naop };
            
            var blks = new List<Blocks>();
            
            nbl=0;
            old_nbl = -1;
            
            foreach(var tab in data)
            {
                if (old_nbl != nbl) nnop = tab.nop;
                old_nbl = nbl;
                if (tab.pfl.Trim() != "5")
                {
                    if (tab.pfl.Trim() == "1")
                    {
                        nbl++;
                        blks.Add(new Blocks { Pfl = tab.pfl, FNop = nnop, LNop = tab.nop, BlockN = nbl.ToString() });
                    }
                }
             }

            var tool = from holl in data
                       join bl in blks on holl.nop equals bl.LNop
                       select new { naop = holl.naop, fop = bl.LNop };

            TempData["Blocks"] = blks;
            var tabl = from mpartt in FoxRepo.GetTable<Crystal.mparttRow>()
                       join ss in data on mpartt.nop equals ss.nop
                       let nbloka = blks.Where(a => int.Parse(a.FNop) <= int.Parse(mpartt.nop) && int.Parse(a.LNop) >= int.Parse(mpartt.nop) && mpartt.kmk == kmk && mpartt.kpr == kpr)
                       select new { pfl = ss.pfl, prt = mpartt, nbl = nbloka.Count() > 0 ? nbloka.Single().BlockN : "0" };

            var result = from t in tabl
                         join haf in blks on t.nbl equals haf.BlockN
                         join hex in tool on haf.LNop equals hex.fop
                         group t by new { t.nbl, haf.LNop, haf.BlockN, hex.naop, haf.Pfl } into g_tabl
                         select new NzpFLit
                         {
                             Pfl = g_tabl.Key.Pfl,
                             Nfl = g_tabl.Key.nbl,
                             Nop = g_tabl.Key.LNop,
                             Naop = g_tabl.Key.naop,
                             KolPr = g_tabl.Count().ToString(),
                             Nzp = g_tabl.Sum(a => a.prt.kpls).ToString(),
                             Kmk = kmk,
                             Kpr = kpr
                         };
            return View(result.OrderBy(x=>int.Parse(x.Nfl)));
        }

        public ViewResult GetFromNzpFlit(string nfl, string kmk, string kpr)//Получаем блок и выводим нзп по операциям
        {
            var someblocks = (IEnumerable<Blocks>)TempData.Peek("Blocks");

            var part = from assop in FoxRepo.GetTable<Crystal.assopRow>()
                       where assop.pfl != "5"
                       join astmr in FoxRepo.GetTable<Crystal.astmrRow>() on assop.kop equals astmr.kop
                       join mpartt in FoxRepo.GetTable<Crystal.mparttRow>() on new { astmr.nop, astmr.kmk } equals new { mpartt.nop, mpartt.kmk }
                       where mpartt.kmk == kmk && mpartt.kpr == kpr
                       from kt in someblocks
                       where kt.BlockN == nfl
                       where int.Parse(mpartt.nop) >= int.Parse(kt.FNop) && int.Parse(mpartt.nop) <= int.Parse(kt.LNop)
                       group mpartt by new { mpartt.nop, mpartt.nprt, mpartt.kpls, assop.naop, mpartt.dato, mpartt.dath, mpartt.timo, mpartt.timh } into g_mpartt
                       select new FlitPartNzp
                       {
                           Nprt = g_mpartt.Key.nprt,
                           Naop = g_mpartt.Key.naop,
                           Dato = g_mpartt.Key.dato.GetFoxDate() !="" ? g_mpartt.Key.dato.GetFoxDate():g_mpartt.Key.dath.GetFoxDate(),
                           Timo = g_mpartt.Key.timo.GetFoxTime() != ""? g_mpartt.Key.timo.GetFoxTime() : g_mpartt.Key.timh.GetFoxTime(),
                           Nop = g_mpartt.Key.nop,
                           Nzp = g_mpartt.Key.kpls.ToString()
                       };

            var part1 = from az in part
                        group az by new { az.Nop, az.Naop } into xz
                        select new FlitOprNzp { Nop = xz.Key.Nop, Naop = xz.Key.Naop, Nzp = xz.Sum(a => int.Parse(a.Nzp)).ToString() };
            ViewBag.kmk = kmk;
            TempData["Parts"] = part;
            return View(part1.OrderBy(a => int.Parse(a.Nop)));
        }

        public ViewResult GetFromNzpFlit2(string nop) //От операции получаем партии
        {
            var someparts = (IEnumerable<FlitPartNzp>)TempData.Peek("Parts");
            return View(someparts.Where(x=>x.Nop==nop));
        }

        public ActionResult DayReports()
        {
            var uchList = from uch in FoxRepo.GetTable<Crystal.askuchRow>()
                          select new UchSelect{ Kuch = uch.kuch, Nauch = uch.nauch};
            ViewBag.Kuch = uchList.OrderBy(x=>x.Kuch);
            return View();
        }

        public ViewResult GetByUch(string kuch)
        {
            DateTime thisDay = DateTime.Today;

            var result = (from astob in FoxRepo.GetTable<Crystal.astobRow>()
                          where astob.kuch == kuch
                          join mprxop in FoxRepo.GetTable<Crystal.mprxopRow>()
                              on new {astob.kgt, astob.kus} equals new {mprxop.kgt, mprxop.kus}
                              where mprxop.dato.ToShortDateString() == thisDay.ToShortDateString()
                              group mprxop by mprxop.kmk into g_mprxop
                          select new ReportsDay{ Nzp = g_mprxop.Sum(x=>x.kpls).ToString(), Income = "", Processed = "", TrCof = ""});

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
            ViewBag.Prn = (from asparam in FoxRepo.GetTable<Crystal.asparamRow>()
                           where asparam.identp.Trim() == "FACTORY"
                           select asparam.valp).First().Trim();
            DateTime d1 = DateTime.Parse(fdate);
            DateTime d2 = DateTime.Parse(sdate);
            ViewBag.CrystalsDate1 = d1;
            ViewBag.CrystalsDate2 = d2;

            if (HttpContext.Session != null) HttpContext.Session.Add("__CrystalsDate1", d1);
            if (HttpContext.Session != null) HttpContext.Session.Add("__CrystalsDate2", d2);

            var mkristTable = FoxRepo.GetTable<Crystal.mkristRow>(filter: "WHERE BETWEEN(datop,?,?)", parameters: new[] { d1 as object, d2 as object });
            var amkristTable = FoxRepo.GetTable<Crystal.mkristRow>(isArchived: true, filter: "WHERE BETWEEN(datop,?,?)", parameters: new[] { d1 as object, d2 as object });

          var result = from asspr in FoxRepo.GetTable<Crystal.assprRow>()
                       join mkrist in mkristTable.Where(a => a.godn > 0)
                       on asspr.kpr equals mkrist.kpr into zapr
                       join amkrist in amkristTable.Where(a=>a.godn>0)
                       on asspr.kpr equals amkrist.kpr into zapr1
                       let sum_pr = zapr.Sum(a => a.pkrist) + zapr1.Sum(b => b.pkrist)
                       let sum_gd = zapr.Sum(a => a.godn) + zapr1.Sum(b => b.godn)
                       where sum_pr>0 && sum_gd>0
                       select new Crystals { Kpr = asspr.kpr, Napr = asspr.napr, 
                           Pkrist = sum_pr.ToString(), 
                           Godn = sum_gd.ToString(), 
                           ProcVux = ((sum_gd/sum_pr)*100).ToString("F2") };
            return View(result);
        }
        [HttpGet]
        public ActionResult CrystalsByMonth(string month, string year)
        {
            DateTime date1 = DateTime.Parse(("01." + month + "." + year));
            DateTime date2 = DateTime.Parse(DateTime.DaysInMonth(int.Parse(year), int.Parse(month)).ToString() + "." + month + "." + year);

            if (HttpContext.Session != null) HttpContext.Session.Add("__CrystalsDate1", date1);
            if (HttpContext.Session != null) HttpContext.Session.Add("__CrystalsDate2", date2);

            var mkristTable = FoxRepo.GetTable<Crystal.mkristRow>(filter: "WHERE BETWEEN(datop,?,?)", parameters: new[] { date1 as object, date2 as object });
            var amkristTable = FoxRepo.GetTable<Crystal.mkristRow>(isArchived: true, filter: "WHERE BETWEEN(datop,?,?)", parameters: new[] { date1 as object, date2 as object });

            var result = from asspr in FoxRepo.GetTable<Crystal.assprRow>()
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
        
        [HttpGet]
        public ActionResult CrystalsByMonth2()
        {
            object d1=null,d2=null;

            if (HttpContext.Session != null)
            {
                d1 = HttpContext.Session["__CrystalsDate1"];
                d2 = HttpContext.Session["__CrystalsDate2"];
            }

            var mkristTable = FoxRepo.GetTable<Crystal.mkristRow>(filter: "WHERE BETWEEN(datop,?,?)", parameters: new[] { d1, d2});
            var amkristTable = FoxRepo.GetTable<Crystal.mkristRow>(isArchived:true, filter: "WHERE BETWEEN(datop,?,?)", parameters: new[] { d1 , d2 });
            

            var result = from asabon in FoxRepo.GetTableSql<Crystal.asabonRow>(sql: "select Tbn, Fio from asabon group by Tbn, Fio")
                                join mkrist in mkristTable
                                on asabon.tbn equals mkrist.owner into zapr
                                join amkrist in amkristTable
                                on asabon.tbn equals amkrist.owner into zapr1
                                let sum_pr = zapr.Sum(a => a.pkrist) + zapr1.Sum(b => b.pkrist)
                                let sum_gd = zapr.Sum(a => a.godn) + zapr1.Sum(b => b.godn)
                                where sum_pr > 0 && sum_gd > 0
                                select new CrystalsSecond
                                {
                                    Fio = asabon.fio,
                                    Pkrist = sum_pr.ToString(),
                                    Godn = sum_gd.ToString(),
                                    ProcVux = ((sum_gd / sum_pr) * 100).ToString("F2")
                                };
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public ActionResult Defect()
        {
            return View();
        }
       
        public ActionResult DefectsDistr(string fdate, string sdate)
        {
            ViewBag.Prn = (from asparam in FoxRepo.GetTable<Crystal.asparamRow>()
                           where asparam.identp.Trim() == "FACTORY"
                           select asparam.valp).First().Trim();
            DateTime d1 = DateTime.Parse(fdate);
            DateTime d2 = DateTime.Parse(sdate);
           TempData["d1"] = d1;
           TempData["d2"] = d2;

           var ampartnTable = from ampartn in FoxRepo.GetTable<Crystal.mpartnRow>(isArchived: true, filter: "WHERE BETWEEN(datbr,?,?)", parameters: new[] { d1 as object, d2 as object })
                              join astmr in FoxRepo.GetTable<Crystal.astmrRow>() on new { ampartn.nop, ampartn.kmk } equals new { astmr.nop, astmr.kmk }
                              join assop in FoxRepo.GetTable<Crystal.assopRow>() on astmr.kop equals assop.kop
                              where assop.pfl != "5"
                              select ampartn;
           var mpartnTable = from mpartn in FoxRepo.GetTable<Crystal.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?)", parameters: new[] { d1 as object, d2 as object })
                             join astmr in FoxRepo.GetTable<Crystal.astmrRow>() on new { mpartn.nop, mpartn.kmk } equals new { astmr.nop, astmr.kmk }
                             join assop in FoxRepo.GetTable<Crystal.assopRow>() on astmr.kop equals assop.kop
                             where assop.pfl != "5"
                             select mpartn;

          var result = from asspr in FoxRepo.GetTable<Crystal.assprRow>()
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
            return View(result.Where(x=>x.Nzp!=0).OrderBy(x=>x.Kpr));
        }
        public ActionResult DefectsDistrFor2Plant(string fdate, string sdate)
        {
            DateTime d1 = DateTime.Parse(fdate);
            DateTime d2 = DateTime.Parse(sdate);
            ViewBag.task = "DefectsDistrFor2Plant";
            TempData["d11"] = d1;
            TempData["d21"] = d2;
            var ampartnTable =from ampartn in FoxRepo.GetTable<Crystal.mpartnRow>(isArchived: true, filter: "WHERE BETWEEN(datbr,?,?)", parameters: new[] { d1 as object, d2 as object })
                              join astmr in FoxRepo.GetTable<Crystal.astmrRow>() on new { ampartn.nop, ampartn.kmk } equals new { astmr.nop, astmr.kmk }
                              join assop in FoxRepo.GetTable<Crystal.assopRow>() on astmr.kop equals assop.kop
                              where assop.pfl == "5" 
                              select ampartn;
            var mpartnTable =from mpartn in FoxRepo.GetTable<Crystal.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?)", parameters: new[] { d1 as object, d2 as object })
                             join astmr in FoxRepo.GetTable<Crystal.astmrRow>() on new { mpartn.nop, mpartn.kmk } equals new { astmr.nop, astmr.kmk }
                             join assop in FoxRepo.GetTable<Crystal.assopRow>() on astmr.kop equals assop.kop
                             where assop.pfl == "5"
                             select mpartn;
            var result = from asspr in FoxRepo.GetTable<Crystal.assprRow>()
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
            return View("DefectsDistr",result.Where(x => x.Nzp != 0).OrderBy(x => x.Kpr));
            
        }
        [HttpGet]
        public ViewResult DefectsDistrOperations(string kpr)
        {   
            
            var d1 = TempData.Peek("d1");
            var d2 = TempData.Peek("d2");

            var ampartnTable = FoxRepo.GetTable<Crystal.mpartnRow>(isArchived:true, filter: "WHERE BETWEEN(datbr,?,?) AND kpr == ?", parameters: new[] { d1, d2, kpr });
            var mpartnTable = FoxRepo.GetTable<Crystal.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?) AND kpr == ?", parameters: new[] { d1, d2, kpr });

            string Kmk = (from asspr in FoxRepo.GetTable<Crystal.assprRow>(filter: "WHERE kpr == ?", parameters: new[] { kpr })
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

        var result = from tester in pre_result
                     group tester by tester.nop
                     into gtest
                     join astmr in FoxRepo.GetTable<Crystal.astmrRow>(filter: "WHERE kmk == ?", parameters:new[]{Kmk}) 
                     on gtest.Key equals astmr.nop
                         join assop in FoxRepo.GetTable<Crystal.assopRow>(filter: "WHERE pfl!='5'") 
                     on astmr.kop equals assop.kop 
                     select new DefectsDistributionOperations
                     {
                         Nop = gtest.Key,
                         Naop = assop.naop,
                         Nzp = gtest.Sum(a => a.pls).ToString()
                     };

            ViewBag.Kpr = kpr;
            ViewBag.Kmk = Kmk;
            return View(result.OrderBy(a=>a.Nop));
        }

        public ViewResult DefectsDistrOperationsFor2Plant(string kpr)
        {
            var d1 = TempData.Peek("d11");
            var d2 = TempData.Peek("d21");

            var ampartnTable = FoxRepo.GetTable<Crystal.mpartnRow>(isArchived: true, filter: "WHERE BETWEEN(datbr,?,?) AND kpr == ?", parameters: new[] { d1, d2, kpr });
            var mpartnTable = FoxRepo.GetTable<Crystal.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?) AND kpr == ?", parameters: new[] { d1, d2, kpr });

            string Kmk = (from asspr in FoxRepo.GetTable<Crystal.assprRow>(filter: "WHERE kpr == ?", parameters: new[] { kpr })
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

            var result = from tester in pre_result
                         group tester by tester.nop
                             into gtest
                             join astmr in FoxRepo.GetTable<Crystal.astmrRow>(filter: "WHERE kmk == ?", parameters: new[] { Kmk })
                             on gtest.Key equals astmr.nop
                             join assop in FoxRepo.GetTable<Crystal.assopRow>(filter: "WHERE pfl=='5'")
                         on astmr.kop equals assop.kop
                             select new DefectsDistributionOperations
                             {
                                 Nop = gtest.Key,
                                 Naop = assop.naop,
                                 Nzp = gtest.Sum(a => a.pls).ToString()
                             };

            ViewBag.Kpr = kpr;
            ViewBag.Kmk = Kmk;
            return View("DefectsDistrOperations", result.OrderBy(a => a.Nop));
        }

        [HttpGet]
        public ViewResult DefectsDistrData(string nop, string kpr, string kmk)
        {
            var d1 = TempData.Peek("d1");
            var d2 = TempData.Peek("d2");
            var ampartnTable = FoxRepo.GetTable<Crystal.mpartnRow>( isArchived:true, filter: "WHERE BETWEEN(datbr,?,?) AND kpr == ? AND kmk == ? AND nop == ?", parameters: new[] { d1, d2, kpr, kmk, nop });
            var mpartnTable = FoxRepo.GetTable<Crystal.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?) AND kpr == ? AND kmk == ? AND nop == ?", parameters: new[] { d1, d2, kpr, kmk, nop });

            var test = (from t1 in ampartnTable
                        select new {nprt = t1.nprt, npls = t1.npls, Datbr = t1.datbr, kprb = t1.kprb, kgt = t1.kgt}).Union
                       (from t2 in mpartnTable
                        select new {nprt = t2.nprt, npls = t2.npls, Datbr = t2.datbr, kprb = t2.kprb, kgt = t2.kgt});

            var result = from t1 in FoxRepo.GetTable<Crystal.askprbRow>()
                          join t2 in test on new {t1.kprb, t1.kgt} equals new {t2.kprb, t2.kgt}
            select new DefDistrData
                              {
                                  Nprt = t2.nprt,
                                  Npls = t2.npls.Substring(11, 2),
                                  Naprb = t1.naprb,
                                  Datbr = t2.Datbr.ToShortDateString()
                              };
            return View(result.OrderBy(x=>x.Nprt));
        }

        public ActionResult DefectsDistrByMonth(string month, string year)
        {
            DateTime d1 = DateTime.Parse(("01." + month + "." + year));
            DateTime d2 = DateTime.Parse(DateTime.DaysInMonth(int.Parse(year), int.Parse(month)).ToString() + "." + month + "." + year);

            TempData["d1"] = d1;
            TempData["d2"] = d2;

            var ampartnTable = FoxRepo.GetTable<Crystal.mpartnRow>(isArchived: true, filter: "WHERE BETWEEN(datbr,?,?)", parameters: new[] { d1 as object, d2 as object });
            var mpartnTable = FoxRepo.GetTable<Crystal.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?)", parameters: new[] { d1 as object, d2 as object });

            var result = from asspr in FoxRepo.GetTable<Crystal.assprRow>()
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

        public ActionResult NZPFor2Plant()
        {
            ViewBag.timer = "GetNZPFor2Plant";
             var pribL = (from mpartt in FoxRepo.GetTable<Crystal.mparttRow>(filter: "WHERE nop!='0000'")
                         join asspr in FoxRepo.GetTable<Crystal.assprRow>()
                          on mpartt.kpr equals asspr.kpr
                         join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                         on new { mpartt.kmk,mpartt.nop} equals new { astmr.kmk,astmr.nop}
                         join assop in FoxRepo.GetTable<Crystal.assopRow>(filter: "WHERE pfl=='5'")
                         on astmr.kop equals assop.kop
                         group mpartt by new { mpartt.kpr, asspr.napr } into g_mpartt
                         select new NzpPribList
                         {
                             Kpr = g_mpartt.Key.kpr,
                             Napr = g_mpartt.Key.napr,
                             Nzp = g_mpartt.Sum(a => a.kpls)
                         }).OrderBy(a => a.Kpr);
            return View("NzpPrib", pribL);
        }
        public ActionResult GetNZPFor2Plant(string kpr)
        {
            ViewBag.kpr = kpr;
            var data = (from mpartt in FoxRepo.GetTable<Crystal.mparttRow>(filter: "WHERE Kpr==?", parameters: new[] { kpr })
                        join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                         on new { mpartt.kmk, mpartt.nop } equals new { astmr.kmk, astmr.nop }
                        join assop in FoxRepo.GetTable<Crystal.assopRow>(filter: "WHERE pfl=='5'")
                         on astmr.kop equals assop.kop
                        group mpartt by new { mpartt.nop, assop.naop } into g_mpartt
                        select new NzpPrib
                        {
                            Nop = g_mpartt.Key.nop,
                            Naop = g_mpartt.Key.naop,
                            Nzp = g_mpartt.Sum(a => a.kpls).ToString()
                        }).OrderBy(a => a.Nop);
            return View("getnzpprib",data);
            
        }
         public ActionResult PribKplsForDay()
         {
            var model = (from asmnp in FoxRepo.GetTable<Crystal.asmnpRow>()
                         join asspr in FoxRepo.GetTable<Crystal.assprRow>()
                             on asmnp.kpr equals asspr.kpr
                             group asmnp by new {asmnp.kpr,asspr.napr } into zabr1
                         select new PribKplsForDay
                             {
                                 Kpr = zabr1.Key.kpr,
                                 Napr = zabr1.Key.napr,
                                 KplsForDay=zabr1.Sum(a=>a.sdo2).ToString()
                             }).OrderBy(a=>a.Kpr).Where(a=>a.KplsForDay.Trim()!="0");
             return PartialView(model);
         }
         public ActionResult GangPribKplsForDay(string kpr)
         {
             var model =
                 (from asmnp in FoxRepo.GetTable<Crystal.asmnpRow>(filter: "WHERE Kpr==?", parameters: new[] {kpr})
                 join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                     on new {asmnp.kmk, asmnp.nop} equals new {astmr.kmk, astmr.nop}
                 join assop in FoxRepo.GetTable<Crystal.assopRow>()
                     on astmr.kop equals assop.kop
                 select new GangPribKplsForDay
                     {
                         Nop = asmnp.nop,
                         Naop = assop.naop,
                         KplsForNop = asmnp.sdo2.ToString()
                     }).OrderBy(a=>a.Nop).Where(a=>a.KplsForNop.Trim()!="0");

             return PartialView(model);
        }

        public ActionResult PribKplsForMount()
        {
            var start = FoxRepo.GetStartMount();
            var data =
                (FoxRepo.GetTable<Crystal.mprxopRow>()).Concat(FoxRepo.GetTable<Crystal.mprxopRow>(isArchived: true))
                    .Where(a => FoxRepo.OneColDateTime(a.dato, a.timo) >start );
            var firsttour = data;
            var model = from mprxop in data
                        group mprxop by new {FoxRepo.OneColDateTime(mprxop.dato, mprxop.timo).Day}
                        into zabr1
                            select new PribKplsForMount
                            {
                                Date = zabr1.Key.ToString()
                            };

            return PartialView(model);
        }

        //public ActionResult test_jqGrid()
        //{
        //    ViewBag.Prn = (from asparam in FoxRepo.GetTable<Crystal.asparamRow>()
        //                   where asparam.identp.Trim() == "FACTORY"
        //                   select asparam.valp).First().Trim();
        //    var flitList = new List<NzpFLitList>();
        //    flitList.AddRange(from mpartt in FoxRepo.GetTable<Crystal.mparttRow>()
        //                      join asspr in FoxRepo.GetTable<Crystal.assprRow>()
        //                       on mpartt.kpr equals asspr.kpr
        //                      group mpartt by new { mpartt.kpr, asspr.napr, asspr.kmk } into g_mpartt
        //                      select new NzpFLitList
        //                      {
        //                          Kpr = g_mpartt.Key.kpr,
        //                          Napr = g_mpartt.Key.napr,
        //                          Kmk = g_mpartt.Key.kmk,
        //                          Nzp = g_mpartt.Sum(x => x.kpls)
        //                      });

        //    ViewBag.FlitList = JsonConvert.SerializeObject(flitList);
        //    return View();
        //}
    }
}
