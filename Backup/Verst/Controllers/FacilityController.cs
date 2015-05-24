using System;
using System.Collections;
using System.Collections.Generic;

using System.Web.Mvc;
using NLog;
using Verst.Models;
using System.Linq;
using MvcApplication.Models;
using Area = MvcApplication.Models.Area;


namespace Verst.Controllers
{
    [PlantAuthorization]
    public class FacilityController : Controller
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public ActionResult Details(string KGT, string KUS)
        {
           DeatilsModel model = new DeatilsModel { Kgt = KGT, Kus = KUS };
            var eq = new Equipment { kgt = KGT, kus = KUS };
            ViewBag.Equipment = eq.equipment;
            model.OnOperations = (from part in eq.GetStartedParts()
                               join asspr in FoxRepo.GetTable<Crystal.assprRow>()
                                  on part.kpr equals asspr.kpr
                                join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                                  on new { part.kmk, part.nop } equals new { astmr.kmk, astmr.nop }
                                join assop in FoxRepo.GetTable<Crystal.assopRow>()
                                  on astmr.kop equals assop.kop
                                join asabon in FoxRepo.GetTable<Crystal.asabonRow>()
                                  on part.owner equals asabon.tbn into part_abon
                               select new OnOperation
                               {
                                   Kpr = part.kpr,
                                   Napr = asspr.napr,
                                   Nprt = part.nprt,
                                   Dato = part.dato.GetFoxDate(),
                                   Nop = part.nop,
                                   Naop = assop.naop,
                                   Dath = part.dath.GetFoxDate(),
                                   Timh=FoxRepo.GetFoxTime(part.timh) ,
                                   Kpls = double.Parse(part.kpls.ToString()),Fio = part_abon.First().fio
                               }).OrderBy(x=>x.Kpr);

            model.OnLaunch = (from part in eq.GetWaitingParts()
                                 join asspr in FoxRepo.GetTable<Crystal.assprRow>()
                                   on part.kpr equals asspr.kpr
                                 join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                                   on new { part.kmk, part.nop } equals new { astmr.kmk, astmr.nop }
                                 join assop in FoxRepo.GetTable<Crystal.assopRow>()
                                   on astmr.kop equals assop.kop
                                 join asabon in FoxRepo.GetTable<Crystal.asabonRow>()
                                   on part.owner equals asabon.tbn into part_abon
                                select new OnLaunch
                                {
                                    Kpr = part.kpr,
                                    Napr = asspr.napr,
                                    Nprt = part.nprt,
                                    Dato = part.dato.GetFoxDate(),
                                    Timo=FoxRepo.GetFoxTime(part.timo),
                                    Nop = part.nop,
                                    Naop = assop.naop,
                                    Kpls = double.Parse(part.kpls.ToString())
                                }).OrderBy(x=>x.Kpr);
            return View(model);
        }

        public ActionResult GetPrib(string KGT, string KUS, int Depth)
        {
            var eq = new Equipment { kgt = KGT, kus = KUS };
            var data =
               (from part in eq.GetNearParts()
                join asspr in FoxRepo.GetTable<Crystal.assprRow>()
                   on part.batch.kpr equals asspr.kpr
                join assop in FoxRepo.GetTable<Crystal.assopRow>()
                  on part.kop equals assop.kop

                join astmrbatch in FoxRepo.GetTable<Crystal.astmrRow>()
                on new { part.batch.kmk,part.batch.nop} equals new { astmrbatch.kmk,astmrbatch.nop}
                join assopbatch in FoxRepo.GetTable<Crystal.assopRow>()
                on astmrbatch.kop equals assopbatch.kop
                
               
               select new Approaching
               {
                   Kpr = part.batch.kpr,
                   Napr = asspr.napr,
                   Nprt = part.nprt,
                   Kpls = part.batch.kpls.ToString(),
                   From = assopbatch.naop,
                   To = assop.naop,
                   Distance = part.distance
               }).OrderBy(x=>x.Distance);

            if (Depth >= 0)
            {
               var data2 = data.Where(x => x.Distance <= Depth);
                return View(data2);
            }

            return View(data);
        }

        public ActionResult AreaNzp()
        {
            ViewBag.Prn = (from asparam in FoxRepo.GetTable<Crystal.asparamRow>()
                           where asparam.identp.Trim() == "FACTORY"
                           select asparam.valp).First().Trim();
            var result = (from astob in FoxRepo.GetTable<Crystal.astobRow>()
                          join as_wop in FoxRepo.GetTable<Crystal.as_wopRow>()
                              on new { astob.kgt, astob.kus } equals new { as_wop.kgt, as_wop.kus }
                          join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                              on as_wop.kop equals astmr.kop
                          join asmnp in FoxRepo.GetTable<Crystal.asmnpRow>()
                              on new { astmr.kmk, astmr.nop } equals new { asmnp.kmk, asmnp.nop }
                          group asmnp by astob.kuch into asnmp_data1
                          let asmnp_data = asnmp_data1.Distinct()
                          let Pso = asmnp_data.Sum(a => a.pso3)
                          let Sdo = asmnp_data.Sum(a => a.sdo3)
                          let Brk = asmnp_data.Sum(a => a.brk3)
                          let Kpls = Pso + asmnp_data.Sum(a => a.nzpo) - Sdo - Brk 
                          select new NzpUch
                               {
                                   Kuch = new Area() { kuch = asnmp_data1.Key }.area.nauch,
                                   Pso = Pso.ToString(),
                                   Sdo = Sdo.ToString(),
                                   Brk = Brk.ToString(),
                                   Kpls = Kpls.ToString(),
                                   Percs = (Sdo+Brk)!=0?(100*Sdo/(Sdo+Brk)).ToString("F2"):"100"
                               }); 

            ViewBag.results = result;
            TempData["Table"] = result;
            return View();
        }

        [HttpGet]
        public ActionResult AreaNzpBrakGraf()
        {
            var nauch = ((IEnumerable<NzpUch>)TempData.Peek("Table")).Select(x => x.Kuch).ToList();
            var brk = ((IEnumerable<NzpUch>)TempData.Peek("Table")).Select(x => int.Parse(x.Brk)).ToList();
            return Json(new {Nauch = nauch, Brk = brk}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartInfo(string NPRT)
        {
            var tempAsabon = FoxRepo.GetTable<Crystal.asabonRow>();
            var tempMprxop = FoxRepo.GetTable<Crystal.mprxopRow>(filter: "WHERE nprt == ? ORDER BY nop", parameters: new[] { NPRT });
            var tempMpartn = FoxRepo.GetTable<Crystal.mpartnRow>(filter: "WHERE nprt == ? ORDER BY nop", parameters: new[] { NPRT });
            if (tempMprxop.Count() < 1)
            {
                tempMprxop = FoxRepo.GetTable<Crystal.mprxopRow>(isArchived: true, filter: "WHERE nprt == ? ORDER BY nop", parameters: new[] { NPRT });
                tempMpartn = FoxRepo.GetTable<Crystal.mpartnRow>(isArchived: true, filter: "WHERE nprt == ? ORDER BY nop", parameters: new[] { NPRT });
            }

            var result = (from part in tempMprxop
                          join asspr in FoxRepo.GetTable<Crystal.assprRow>()
                                on part.kpr equals asspr.kpr
                          join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                                on new { part.kmk, part.nop } equals new { astmr.kmk, astmr.nop }
                          join assop in FoxRepo.GetTable<Crystal.assopRow>()
                                on astmr.kop equals assop.kop
                          join astob in FoxRepo.GetTable<Crystal.astobRow>()
                                on new { part.kgt, part.kus } equals new { astob.kgt, astob.kus }
                          join mpartn in tempMpartn
                                on new { nprt = part.nprt, nop = part.nop } equals new { nprt = mpartn.nprt, nop = mpartn.nop }
                                into part_brak
                             select new PartInfo
                             {
                                 Nop = part.nop,
                                 Nprt = part.nprt,
                                 Napr = asspr.napr,
                                 Kop = astmr.kop,
                                 Naop = assop.naop,
                                 SNaobor = astob.snaobor,
                                 Dath = part.dath.GetFoxDate(),
                                 Timh = FoxRepo.GetFoxTime(part.timh),
                                 Dato = part.dato.GetFoxDate(),
                                 Timo = FoxRepo.GetFoxTime(part.timo),
                                 Kpls = part.kpls.ToString(),
                                 Brak = part_brak.Count().ToString(),
                                 Fio = FoxRepo.GetFioByTabNumber(part.owner, tempAsabon)
                             }).ToList();

            var p_i = result.FirstOrDefault();
            
            if (p_i != null)
            {
                var TOnR = FoxRepo.WorkDaysBetween(DateTime.Parse(result.First().Dath),
                                                   DateTime.Parse(result.Last().Dath),
                                                   FoxRepo.GetTable<Crystal.askrmRow>());
                ViewBag.Nprt = p_i.Nprt;
                ViewBag.Izd = p_i.Napr;
                ViewBag.OnRoute = TOnR;
                ViewBag.result = result;
                //Проверяем наблюдается ли партия
                //ViewBag.WatchedStatus = UserController.PartWatcher.IfWatched(p_i.Nprt) ? "Уже наблюдается" : "Следить за партией";
            }
            else
            {
                ViewBag.Nprt = "";
                ViewBag.Izd = "";
                ViewBag.OnRoute = "";
                ViewBag.result = new List<PartInfo>();
            }
            
            return View();
        }

        public ActionResult Downtimes(string KGT, string KUS)
        {
            var t_asabon = FoxRepo.GetTable<Crystal.asabonRow>();
            DateTime now = DateTime.Now;
            var eq = new Equipment { kgt = KGT, kus = KUS };
            var result = (from asmbn in eq.GetDowntimes()
                         join askprob in FoxRepo.GetTable<Crystal.askprobRow>()
                            on asmbn.kpp equals askprob.kpp into svar
                         from myvar in svar.DefaultIfEmpty()
                         select new Downtimes
                         {
                             Nobr = eq.equipment.snaobor,
                             Dnp = asmbn.dnp.GetFoxDate(),
                             Tnp = FoxRepo.GetFoxTime(asmbn.tnp),
                             Dop = asmbn.dop.GetFoxDate(),
                             Top = FoxRepo.GetFoxTime(asmbn.top),
                             Napr = myvar == null ? "" : myvar.napr,
                             Iopn = FoxRepo.GetFioByTabNumber(asmbn.iopn, t_asabon),
                             Iopo = FoxRepo.GetFioByTabNumber(asmbn.iopo, t_asabon),
                             Hrs = FoxRepo.HoursBetween(asmbn.dop, asmbn.top, asmbn.dnp, asmbn.tnp).ToString("F2")
                         }).Where(a=>DateTime.Parse(a.Dnp).Month==now.Month);
            ViewBag.results = result;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetBrak(string nop, string nprt)
        {
            var tempAM = FoxRepo.GetTable<Crystal.mpartnRow>(isArchived: true);
            var tempM = FoxRepo.GetTable<Crystal.mpartnRow>();
            var Tem = tempM.Concat(tempAM);
            var result = from mpartn in Tem
                      where mpartn.nop == nop && mpartn.nprt == nprt
                         join asbrak in FoxRepo.GetTable<Crystal.asbrakRow>()
                          on mpartn.kprb equals asbrak.kprb
                      select new Defects
                          {
                              Npls = mpartn.npls.Substring(11),
                              Naprb = asbrak.naprb
                          };
            return View(result.OrderBy(x=>x.Npls));
        }
        public ActionResult MZagrsBrig()
        {
            return View();
        }
        public ActionResult MZagrBrig(string fdate, string sdate)
        {
            ViewBag.Prn = (from asparam in FoxRepo.GetTable<Crystal.asparamRow>()
                           where asparam.identp.Trim() == "FACTORY"
                           select asparam.valp).First().Trim();
            DateTime d1 = DateTime.Parse(fdate);
            DateTime d2 = DateTime.Parse(sdate);

            object date1 = d1, date2 = d2;
            TempData["Date1"] = d1;
            TempData["Date2"] = d2;

            int hrsMonth = FoxRepo.WorkDaysInMonth(DateTime.Now) * 24;
            var tempMprxop = FoxRepo.GetTable<Crystal.mprxopRow>(filter: "WHERE BETWEEN(dath,?,?)", parameters: new[] { date1, date2 });
            var tempAsmbn = FoxRepo.GetTable<Crystal.asmbnRow>(filter: "WHERE BETWEEN(Dop,?,?)", parameters: new[] { date1, date2 });

            var Result = from asob in FoxRepo.GetTable<Crystal.asob_nalRow>()
                              join astob in FoxRepo.GetTable<Crystal.astobRow>()
                                  on new { asob.kgt, asob.kus } equals new { astob.kgt, astob.kus }
                              join assbrn in FoxRepo.GetTable<Crystal.assbrnRow>()
                                  on new { asob.cex, asob.kbrg } equals new { assbrn.cex, assbrn.kbrg } 
                              join mprxop in tempMprxop
                                 on new { asob.kgt, asob.kus } equals new { mprxop.kgt, mprxop.kus } into proc_mp
                              join mpartn in FoxRepo.GetTable<Crystal.mpartnRow>()
                                 on new { asob.kgt, asob.kus } equals new { mpartn.kgt, mpartn.kus } into proc_brak
                              join asmbn in tempAsmbn
                                on new { asob.kgt, asob.kus } equals new { asmbn.kgt, asmbn.kus } into eq_repairs
                              let reps = eq_repairs.Sum(asmbn => FoxRepo.HoursBetweenForMZagr(asmbn.dop, asmbn.top, asmbn.dnp, asmbn.tnp))                           
                              select new LoadMth
                              {
                                  Numb = asob.kbrg,
                                  Kuch = assbrn.nabrg,
                              //    Nobr = zz.Key.snaobor,
                                  Kpls = proc_mp.Sum(a=>a.kpls).ToString(),
                                 // Brk = proc_brak.Count().ToString(),
                               //   Nprt = proc_parts.Count().ToString(),
                                  RepH = reps.ToString("F2"),
                                  KrObr = ((hrsMonth - reps) / hrsMonth).ToString("F3")
                              };
            ViewBag.result = (from test in Result
                group test by new {test.Numb, test.Kuch}
                into zz
                select new LoadMth
                {
                    Numb = zz.Key.Numb,
                    Kuch = zz.Key.Kuch,
                    Kpls = zz.Sum(a => int.Parse(a.Kpls)).ToString(),
                    RepH = zz.Sum(a=>decimal.Parse(a.RepH)).ToString("F2"),
                    KrObr = (zz.Sum(a=>decimal.Parse(a.KrObr))/zz.Count()).ToString("F3")
                    
                }).OrderBy(a=>a.Numb);
         /*   var reas = from askprob in FoxRepo.GetTable<Crystal.askprobRow>()
                       join asmbn in FoxRepo.GetTable<Crystal.asmbnRow>(filter: "WHERE BETWEEN(Dop,?,?)", parameters: new[] { date1, date2 })
                       on askprob.kpp equals asmbn.kpp
                       group asmbn by new { askprob.napr } into zapr
                       let reps = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp))
                       select new Reasons
                       {
                           Napr = zapr.Key.napr,
                           SumN = zapr.Select(a => a.kpp).Count().ToString(),
                           Hrs = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp)).ToString("F2"),
                           KrObr = ((hrsMonth - reps) / hrsMonth).ToString("F3"),
                       };
         /*  ViewBag.reas = ((IEnumerable<LoadMth>)ViewBag.Result).Average(a => decimal.Parse(a.KrObr)).ToString("F2");
            ViewBag.Result2 = reas.OrderByDescending(a => a.Hrs);*/
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
            return View();
        }
        
        public ActionResult MZagr(string fdate, string sdate)
        {
            ViewBag.Prn = (from asparam in FoxRepo.GetTable<Crystal.asparamRow>()
                           where asparam.identp.Trim() == "FACTORY"
                           select asparam.valp).First().Trim();
            DateTime d1 = DateTime.Parse(fdate);
            DateTime d2 = DateTime.Parse(sdate);

            object date1 = d1, date2 = d2;
            TempData["Date1"] = d1;
            TempData["Date2"] = d2;

            int hrsMonth = FoxRepo.WorkDaysInMonth(DateTime.Now) * 24;
            var tempMprxop = FoxRepo.GetTable<Crystal.mprxopRow>(filter: "WHERE BETWEEN(dath,?,?)", parameters: new[] { date1, date2 });
            var tempAsmbn = FoxRepo.GetTable<Crystal.asmbnRow>(filter:"WHERE BETWEEN(Dop,?,?)", parameters:new []{date1,date2});

            ViewBag.Result = (from astob in FoxRepo.GetTable<Crystal.astobRow>()
                              join askuch in FoxRepo.GetTable<Crystal.askuchRow>()
                                  on astob.kuch equals askuch.kuch
                              join mprxop in tempMprxop
                                 on new { astob.kgt, astob.kus } equals new { mprxop.kgt, mprxop.kus } into proc_parts
                              join mpartn in FoxRepo.GetTable<Crystal.mpartnRow>()
                                 on new { astob.kgt, astob.kus } equals new { mpartn.kgt, mpartn.kus } into proc_brak
                              join asmbn in tempAsmbn
                                on new { astob.kgt, astob.kus } equals new { asmbn.kgt, asmbn.kus } into eq_repairs
                              let reps = eq_repairs.Sum(asmbn => FoxRepo.HoursBetweenForMZagr(asmbn.dop, asmbn.top, asmbn.dnp, asmbn.tnp))
                              select new LoadMth
                              {
                                  Kuch = askuch.nauch,
                                  Nobr = astob.snaobor,
                                  Kpls = proc_parts.Sum(a => a.kpls).ToString(),
                                  Brk = proc_brak.Count().ToString(),
                                  Nprt = proc_parts.Count().ToString(),
                                  RepH = reps.ToString("F2"),
                                  KrObr = ((hrsMonth - reps) / hrsMonth).ToString("F3")
                              }).OrderBy(x => x.Kuch);

            var reas = from askprob in FoxRepo.GetTable<Crystal.askprobRow>()
                       join asmbn in FoxRepo.GetTable<Crystal.asmbnRow>(filter:"WHERE BETWEEN(Dop,?,?)", parameters:new []{date1,date2})
                       on askprob.kpp equals asmbn.kpp
                       group asmbn by new { askprob.napr } into zapr
                       let reps = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp))
                       select new Reasons
                       {
                           Napr = zapr.Key.napr,
                           SumN = zapr.Select(a => a.kpp).Count().ToString(),
                           Hrs = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp)).ToString("F2"),
                           KrObr = ((hrsMonth - reps) / hrsMonth).ToString("F3"),
                       };
            ViewBag.reas = ((IEnumerable<LoadMth>)ViewBag.Result).Average(a => decimal.Parse(a.KrObr)).ToString("F2");
            ViewBag.Result2 = reas.OrderByDescending(a => a.Hrs);
            return View();
        }

        [HttpGet]
        public ViewResult MzagrFor(string month, string year)
        {
            
            DateTime d1 = DateTime.Parse("1." + month + "." + year), d2 = DateTime.Parse(DateTime.DaysInMonth(int.Parse(year), int.Parse(month)).ToString() + "." + month + "." + year);
            TempData["Date1"] = d1;
            TempData["Date2"] = d2;
            int WorkHrsMonth = FoxRepo.WorkDaysInMonth(d1) * 24;

            var t_mprxop = FoxRepo.GetTable<Crystal.mprxopRow>(filter: "WHERE BETWEEN(Dath,?,?)",parameters:new[]{d1 as object,d2 as object});
            var t_asmbn = FoxRepo.GetTable<Crystal.asmbnRow>(filter: "WHERE BETWEEN(Dop,?,?)", parameters: new[] { d1 as object, d2 as object });

            ViewBag.Result = (from astob in FoxRepo.GetTable<Crystal.astobRow>()
                              join askuch in FoxRepo.GetTable<Crystal.askuchRow>()
                                  on astob.kuch equals askuch.kuch
                              join mprxop in t_mprxop
                                 on new { astob.kgt, astob.kus } equals new { mprxop.kgt, mprxop.kus } into proc_parts
                              join mpartn in FoxRepo.GetTable<Crystal.mpartnRow>()
                                 on new { astob.kgt, astob.kus } equals new { mpartn.kgt, mpartn.kus } into proc_brak
                              join asmbn in t_asmbn
                                on new { astob.kgt, astob.kus } equals new { asmbn.kgt, asmbn.kus } into eq_repairs
                              let reps = eq_repairs.Sum(asmbn => FoxRepo.HoursBetweenForMZagr(asmbn.dop, asmbn.top, asmbn.dnp, asmbn.tnp))
                              select new LoadMth
                              {
                                  Kuch = askuch.nauch,
                                  Nobr = astob.snaobor,
                                  Kpls = proc_parts.Sum(a => a.kpls).ToString(),
                                  Brk = proc_brak.Count().ToString(),
                                  Nprt = proc_parts.Count().ToString(),
                                  RepH = reps.ToString("F2"),
                                  KrObr = ((WorkHrsMonth - reps) / WorkHrsMonth).ToString("F2")
                              }).OrderBy(x => x.Kuch);

            var reas = from askprob in FoxRepo.GetTable<Crystal.askprobRow>()
                       join asmbn in FoxRepo.GetTable<Crystal.asmbnRow>()
                       on askprob.kpp equals asmbn.kpp
                       where asmbn.dop <= d2 && asmbn.dop >= d1
                       group asmbn by new { askprob.napr } into zapr
                       let reps = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp))
                       select new Reasons
                       {
                           Napr = zapr.Key.napr,
                           SumN = zapr.Select(a => a.kpp).Count().ToString(),
                           Hrs = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp)).ToString("F2"),
                           KrObr = ((WorkHrsMonth - reps) / WorkHrsMonth).ToString("F2"),
                       };

            ViewBag.reas = ((IEnumerable<LoadMth>)ViewBag.Result).Average(a => decimal.Parse(a.KrObr)).ToString("F2");
            ViewBag.Result2 = reas.OrderByDescending(a => a.Hrs);
            return View();
        }

        public ViewResult ReasonsByUst(string Nobr)
        {
            var d1 = (DateTime)TempData.Peek("Date1");
            var d2 = (DateTime)TempData.Peek("Date2");
            int hrsMonth = FoxRepo.WorkDaysInMonth(d1) * 24;

            var reas = from askprob in FoxRepo.GetTable<Crystal.askprobRow>()
                       join asmbn in FoxRepo.GetTable<Crystal.asmbnRow>()
                       on askprob.kpp equals asmbn.kpp
                       where asmbn.dop <= d2 && asmbn.dop >= d1
                       join astob in FoxRepo.GetTable<Crystal.astobRow>()
                       on new { asmbn.kus, asmbn.kgt } equals new { astob.kus, astob.kgt }
                       where astob.snaobor == Nobr
                       group asmbn by new { askprob.napr } into zapr
                       let reps = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp))
                       select new Reasons
                       {
                           Napr = zapr.Key.napr,
                           SumN = zapr.Select(a => a.kpp).Count().ToString(),
                           Hrs = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp)).ToString("F2"),
                           KrObr = ((hrsMonth - reps) / hrsMonth).ToString("F2"),
                       };
            return View(reas);
        }

        [HttpGet]
        public ActionResult PrtSearch()
        {
            //Session["PageTitle"] = "Поиск партии";
            return View();
        }

        [HttpPost]
        public ActionResult PrtSearch(PrtToSearch model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "");
                return View("PrtSearchBag", model);
            }



            ViewBag.Nprt = model.Nprt.PadRight(6);

            var result = 
                         from mpartt in FoxRepo.GetTable<Crystal.mparttRow>()
                         where mpartt.nprt.Substring(0, 6) == model.Nprt.PadRight(6)
                         join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                         on new { mpartt.kmk, mpartt.nop } equals new { astmr.kmk, astmr.nop }
                         join assop in FoxRepo.GetTable<Crystal.assopRow>()
                         on astmr.kop equals assop.kop
                         join asspr in FoxRepo.GetTable<Crystal.assprRow>()
                         on new { mpartt.kmk, mpartt.kpr } equals new { asspr.kmk, asspr.kpr }
                         join as_wop in FoxRepo.GetTable<Crystal.as_wopRow>()
                         on new { astmr.kop, mpartt.kgt } equals new { as_wop.kop, as_wop.kgt }
                         join astob in FoxRepo.GetTable<Crystal.astobRow>()
                         on new { as_wop.kgt, as_wop.kus } equals new { astob.kgt, astob.kus }
                         join askor in FoxRepo.GetTable<Crystal.askorRow>()
                         on astob.kkor equals askor.kkor 
                        group mpartt by new {mpartt.nprt, mpartt.nop, assop.naop, mpartt.kpr, mpartt.kpls, asspr.napr, astmr.kop, mpartt.timo, mpartt.dato, mpartt.dath, mpartt.timh, askor.nakor, astob.snaobor} into g_mpartt
                         select new PartSearch
                         {
                             Nprt=g_mpartt.Key.nprt,
                             Kpr = g_mpartt.Key.kpr,
                             Napr = g_mpartt.Key.napr,
                             Nop = g_mpartt.Key.nop,
                             Naop = g_mpartt.Key.naop,
                             Kpls = g_mpartt.Key.kpls.ToString(),
                             Dato = g_mpartt.Key.dato.GetFoxDate(),
                             Timo = g_mpartt.Key.timo.GetFoxTime(),
                             Dath = g_mpartt.Key.dath.GetFoxDate(),
                             Timh = g_mpartt.Key.timh.GetFoxTime(),
                             Nobr = g_mpartt.Key.snaobor,
                             Nakor = g_mpartt.Key.nakor
                         };

            return View("PrtSearchResult", result);
        }

        public ActionResult PrtSearchBag()
        {
            
            return View();
        }
        public ActionResult StandEquipment()
        {
            int hrsMonth = FoxRepo.WorkDaysInMonth(DateTime.Now) * 24;
            DateTime now = DateTime.Now;
            DateTime startmount = FoxRepo.GetStartMount();
            var t_asabon = FoxRepo.GetTable<Crystal.asabonRow>();
        
           var resulthours = (from astob in FoxRepo.GetTable<Crystal.astobRow>(filter: "WHERE KS='r'")
                              join asmbn in FoxRepo.GetTable<Crystal.asmbnRow>().Where(a => FoxRepo.OneColDateTime(a.dor, a.tor) > startmount )
                                  on new {astob.kgt, astob.kus} equals new {asmbn.kgt, asmbn.kus}

                              select new StandTemp
                                  {
                                    kgt=asmbn.kgt,
                                    kus=asmbn.kus,
                                    kpp= asmbn.kpp,
                                    hours = FoxRepo.HoursBetweenForStandEquipment(asmbn.dnp, asmbn.tnp, asmbn.dor, asmbn.tor, startmount),
                                  
                                  });
          
           TempData["OutputStand"] = resulthours;
            var resultmount = from hours in resulthours
                              group hours by new {hours.kgt, hours.kus}
                              into zabr1
                              select new
                                  {
                                      kgt = zabr1.Key.kgt,
                                      kus = zabr1.Key.kus,
                                      totalhours=zabr1.Sum(a=>a.hours),
                                      count=zabr1.Count(),
                                      KrObr = ((hrsMonth - zabr1.Sum(a => a.hours)) / hrsMonth).ToString("F3")
                                  };

            var result = (from astob in FoxRepo.GetTable<Crystal.astobRow>(filter: "WHERE KS='r'")
                         join askor in FoxRepo.GetTable<Crystal.askorRow>()
                         on astob.kkor equals askor.kkor
                         join askush in FoxRepo.GetTable<Crystal.askuchRow>()
                         on astob.kuch equals askush.kuch
                         join asmbn in FoxRepo.GetTable<Crystal.asmbnRow>(filter: "WHERE  TOP=' '")
                         on new { astob.kgt, astob.kus } equals new { asmbn.kgt, asmbn.kus }
                         join hour in resultmount
                         on new {astob.kgt,astob.kus} equals new {hour.kgt,hour.kus}
                         select new StandEquipment
                             {
                                 Nakor = askor.nakor,
                                 Nauch = askush.nauch,
                                 Snaobor = astob.snaobor,
                                 Dnp = asmbn.dnp.ToString("dd.MM.yy"),
                                 Tnp = asmbn.tnp,
                                 Fio = FoxRepo.GetFioByTabNumber(asmbn.iopn, t_asabon),
                                 StandHour = (now - FoxRepo.OneColDateTime(asmbn.dnp, asmbn.tnp)).TotalHours.ToString("F2"),
                                 StandHourMount=hour.totalhours.ToString("F2"),
                                 StandCount=hour.count.ToString(),
                                 KrObr=hour.KrObr,
                                 Kgt = hour.kgt,
                                 Kus = hour.kus
                             }).OrderBy(a=>a.Nauch);
            return View(result);
        }
        public ActionResult OutputStand(string kgt, string kus)
        {
            var gg = (IEnumerable<StandTemp>)TempData.Peek("OutputStand");
            var result = from temp in gg.Where(a => a.kgt == kgt && a.kus == kus)
                         join  askprob in FoxRepo.GetTable<Crystal.askprobRow>()
                         on temp.kpp equals askprob.kpp into zabr1
                         from item in zabr1.DefaultIfEmpty()
                         select new
                             {
                                 Kpp=(item==null?"ещё не определена":item.napr)
                             }
            
            ;
            ViewBag.Napr = result.Select(a=>a.Kpp);
            return View();
        }
        #region Список приборов на производстве

        [HttpGet]
        public ActionResult DevList()
        {
            //Список приборов
            var DevL = from mpartt in FoxRepo.GetTable<Crystal.mparttRow>(filter: "WHERE nop!='0000'")
                       join asspr in FoxRepo.GetTable<Crystal.assprRow>()
                           on mpartt.kpr equals asspr.kpr
                       group mpartt by new { mpartt.kpr, asspr.napr }
                           into g_mpartt
                           orderby int.Parse(g_mpartt.Key.kpr)
                           select new SimplePribList
                           {
                               Kpr = g_mpartt.Key.kpr,
                               Napr = g_mpartt.Key.napr/*,
                    Nzp = g_mpartt.Sum(a => a.kpls)*/
                           };
            return View(DevL);
        }

        [HttpGet]
        public ActionResult DevBatchesList(string kpr)
        {
            var batches = FoxRepo.GetTable<Crystal.mparttRow>(filter: "WHERE kpr==?", parameters: new[] { kpr }).Select(x => x.nprt);
            return View(batches);
        }

        #endregion
    }
}
