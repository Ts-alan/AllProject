using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web.UI;
using MvcApplication.Models;
using NLog;
using Crystal.Models;
using CrystalDataSetLib;
using Crystal.BusinessLogic;
using Crystal.DomainModel;

namespace Crystal.Controllers
{
    [PlantAuthorization]
    public class CyclesController : Controller
    {
        ICrystalLogic bl = BusinessLogicFactory.GetBL();
        private static Logger _log = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public ActionResult Cycle()
        {
            ViewBag.Action = "GetCycle";
            ViewBag.Controller = "Cycles";
            ViewBag.Title = "Циклы";
            return View("DateForm");
        }


        public ViewResult GetCycle(string fdate, string sdate, string indnumb) //Получаем данные по запросу
        {
            _log.Info("Пользователь {0} запросил производственный цикл с:{1} по:{2} на производстве №{3}", User.Identity.Name, fdate, sdate, (int)FoxRepo.GetPlant());

            try
            {
                List<CycleInfo> res;
                res = bl.GetCycle(fdate, sdate, indnumb);
                ViewBag.SumKpls = res.Sum(a => decimal.Parse(a.Kpls));
                ViewBag.Ex = (res.Sum(a => a.Ex) / res.Sum(a => decimal.Parse(a.Kpls))).ToString("F2");
                ViewBag.Ez = (res.Sum(a => a.Ez) / res.Sum(a => decimal.Parse(a.Kpls))).ToString("F2");

                return View(res.OrderBy(x => int.Parse(x.Kpr)));
            }

            catch (Exception ex)
            {
                _log.Error("При выполнении запроса произошла ошибка: {0}!", ex.Message);
                return null;
            }


            
            //try
            //{
               
            //    DateTime d1 = DateTime.Parse(fdate);
            //    DateTime d2 = DateTime.Parse(sdate);
            //    Object dt1 = d1;
            //    Object dt2 = d2;
            //    TempData["dt1"] = dt1;
            //    TempData["dt2"] = dt2;
         
            //    var tempAmpartn = FoxRepo.GetTable<CrystalDS.mpartnRow>(isArchived: true).ToList();
            //    var tempAmpartt = FoxRepo.GetTable<CrystalDS.mparttRow>(isArchived: true,filter: "WHERE BETWEEN(dato,?,?)",
            //        parameters: new[] {dt1, dt2});
            //    var tempMpartt =
            //        FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "where nop == '0000' AND BETWEEN(dato,?,?)",
            //            parameters: new[] {dt1, dt2});
            //    var tempAskrm = FoxRepo.GetTable<CrystalDS.askrmRow>();
            //    var tempAmprxop =
            //        FoxRepo.GetTable<CrystalDS.mprxopRow>(isArchived: true);
            //    var c_tmp_mprxop = ((from ampartt in tempAmpartt
            //                       join amprxop in tempAmprxop
            //            on new {ampartt.kpr, ampartt.nprt} equals new {amprxop.kpr, amprxop.nprt}
            //        group amprxop by new {amprxop.kpr, amprxop.nprt, amprxop.nop}
            //        into g_amprxop
            //        let s_amprxop = g_amprxop.OrderBy(a => a.dath).Last()
            //        select new
            //        {
            //            s_amprxop.kpr,
            //            s_amprxop.nprt,
            //            s_amprxop.nop,
            //            dath = s_amprxop.dath,
            //            dato = s_amprxop.dato,
            //            kpls = s_amprxop.kpls
            //        }).Concat(
            //            from mpartt in tempMpartt
            //            join mprxop in FoxRepo.GetTable<CrystalDS.mprxopRow>()
            //                on new { mpartt.kpr, mpartt.nprt } equals new { mprxop.kpr, mprxop.nprt }
            //            group mprxop by new { mprxop.kpr, mprxop.nprt, mprxop.nop }
            //                into g_mprxop
            //                let s_mprxop = g_mprxop.OrderBy(a => a.dath).Last()
            //                select new
            //                {
            //                    s_mprxop.kpr,
            //                    s_mprxop.nprt,
            //                    s_mprxop.nop,
            //                    dath = s_mprxop.dath,
            //                    dato = s_mprxop.dato,
            //                    kpls = s_mprxop.kpls
            //                }).Distinct()).ToList(); 
               
              
            //    var vr_astmr = (from asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
            //        join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
            //            on asspr.kmk equals astmr.kmk into j_astmr
            //        select new
            //        {
            //            kpr = asspr.kpr,
            //            max_nop = j_astmr.Max(a => a.nop)
            //        }).ToList();

            //    var cycle_data_poln = (from e_mprxop in c_tmp_mprxop
            //        join astmr in vr_astmr
            //            on new {kpr = e_mprxop.kpr, nop = e_mprxop.nop} equals
            //            new {kpr = astmr.kpr, nop = astmr.max_nop}
            //        join s_mprxop in c_tmp_mprxop
            //            on e_mprxop.nprt equals s_mprxop.nprt
            //        where s_mprxop.nop == "0001"
            //        select new cycle_period
            //        {
            //            Kpr = s_mprxop.kpr,
            //            Nprt = s_mprxop.nprt,
            //            dath = s_mprxop.dath,
            //            dato = e_mprxop.dato,
            //        }).ToList();

               
           
            //    var cycle_data_sredn = (from e_mprxop in c_tmp_mprxop
            //                            join asscp in FoxRepo.GetTable<CrystalDS.asscpRow>()
            //                                on new {kpr = e_mprxop.kpr, nop = e_mprxop.nop} equals
            //                                new {kpr = asscp.kpr, nop = asscp.knop}
            //                            join s_mprxop in c_tmp_mprxop
            //                                on e_mprxop.nprt equals s_mprxop.nprt
            //                            where s_mprxop.nop == asscp.nnop
            //                            select new cycle_period
            //                                {
            //                                   Kpr  = s_mprxop.kpr,
            //                                   Nprt = s_mprxop.nprt,
            //                                   dath = s_mprxop.dath,
            //                                   dato = e_mprxop.dato,
            //                                }).ToList();

             

            //    var cycles_poln = (from cd in cycle_data_poln
            //                      group cd by cd.Kpr
            //                      into g_cd
            //                      select
            //                          new
            //                              {
            //                                  kpr = g_cd.Key,
            //                                  cycle_poln =
            //                          g_cd.Average(a => FoxRepo.WorkDaysBetween(a.dath, a.dato, tempAskrm))
            //                              }).ToList();

            //    var cycles_sredn = (from cd in cycle_data_sredn
            //                       group cd by cd.Kpr
            //                       into g_cd
            //                       select
            //                           new
            //                               {
            //                                   kpr = g_cd.Key,
            //                                   cycle_sredn =
            //                           g_cd.Average(a => FoxRepo.WorkDaysBetween(a.dath, a.dato, tempAskrm))
            //                               }).ToList();

            //    var kprs1 = (from mprxop in c_tmp_mprxop
            //                 orderby mprxop.nop
            //                 group mprxop by new {mprxop.kpr, mprxop.nprt}
            //                 into g_mprxop
            //                 select new
            //                     {
            //                         g_mprxop.Key.kpr,
            //                         g_mprxop.Key.nprt,
            //                         sdano = g_mprxop.Last().kpls,
            //                         brak =
            //                     tempAmpartn.Count(a => a.kpr == g_mprxop.Key.kpr && a.nprt == g_mprxop.Key.nprt)
            //                     }).ToList();

            //    var kprs = (from mprxop in kprs1
            //                group mprxop by mprxop.kpr
            //                into g_mprxop
            //                select new
            //                    {
            //                        kpr = g_mprxop.Key,
            //                        sd = g_mprxop.Sum(a => a.sdano),
            //                        brk = g_mprxop.Sum(a => a.brak)
            //                    }
            //               ).ToList();

          
            //    var data = (from prib in kprs
            //               join cp in cycles_poln
            //                   on prib.kpr equals cp.kpr into g_cp
            //               join cs in cycles_sredn
            //                   on prib.kpr equals cs.kpr into g_cs
            //               join asscp in FoxRepo.GetTable<CrystalDS.asscpRow>()
            //                   on prib.kpr equals asscp.kpr
            //               join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
            //                   on prib.kpr equals asspr.kpr
            //               join asmkt in FoxRepo.GetTable<CrystalDS.asmktRow>()
            //                   on asspr.kmk equals asmkt.kmk
            //                let numbfl = String.IsNullOrEmpty(asmkt.numbfl.Trim()) ? 0 : decimal.Parse(asmkt.numbfl)  
            //                let PrCycl = g_cs.Count() > 0 ? g_cs.First().cycle_sredn.ToString("F2") : "0"
            //                let PlCycl = String.IsNullOrEmpty(asscp.pl_cykl.Trim()) ? "0" : asscp.pl_cykl
            //                let PrCyclfl = numbfl == 0 ? "0" : (decimal.Parse(PrCycl) / numbfl).ToString("F2")
            //                let PlCyclfl = numbfl == 0 ? "0" : (decimal.Parse(PlCycl) / numbfl).ToString("F2")
            //               select new CycleInfo()
            //                   {

            //                       Kpr = prib.kpr,
            //                       Napr = asspr.napr,
            //                       Kpls = prib.sd.ToString(),
            //                       Brk = prib.brk.ToString(),
            //                       Proc = ((prib.sd + prib.brk) != 0 ? prib.sd/(prib.sd + prib.brk) : 1).ToString("P"),
            //                       Nop1 = asscp.nnop,
            //                       Nop2 = asscp.knop,
            //                       PlCycl = PlCycl,
            //                       NachOpr = asscp.nnaop,
            //                       KonOpr = asscp.knaop,
            //                       PrCycl = PrCycl,
            //                       FCycl = g_cp.Count() > 0 ? g_cp.First().cycle_poln.ToString("F2") : "n/a",
            //                       Nfl = String.IsNullOrEmpty(asmkt.numbfl.Trim()) ? "0" : asmkt.numbfl,
            //                       PrCyclfl = PrCyclfl,
            //                       PlCyclfl = PlCyclfl,
            //                       Ez = prib.sd*decimal.Parse(PrCyclfl),
            //                       Ex = prib.sd * decimal.Parse(PlCyclfl)
   
            //                   }).ToList();
               
           
          
            //    ViewBag.SumKpls = data.Sum(a => decimal.Parse(a.Kpls));
            //    ViewBag.Ex = (data.Sum(a => a.Ex)/data.Sum(a => decimal.Parse(a.Kpls))).ToString("F2"); 
            //    ViewBag.Ez =(data.Sum(a => a.Ez) / data.Sum(a => decimal.Parse(a.Kpls))).ToString("F2");
            //    TempData["cycleDataPoln"] = cycle_data_poln;
            //    TempData["cycle_data_sredn"] = cycle_data_sredn;
            //    TempData["cycleDataSredn"] = cycle_data_sredn;
             

            //    return View(data.OrderBy(x => int.Parse(x.Kpr)));
                
            //}
            //catch (Exception ex)
            //{
            //    _log.Error("При выполнении запроса произошла ошибка: {0}!", ex.Message);
            //    return null;
            //}
        }

        [HttpGet]
        public ActionResult ShowDefects(string kpr)
        {

            IEnumerable<CyclesDefectsInfo> res;
            res = bl.ShowDefects(kpr);
            return View(res);
            //var d1 = TempData.Peek("dt1");
            //var d2 = TempData.Peek("dt2");
            //var tempAMpartt = FoxRepo.GetTable<CrystalDS.mparttRow>(isArchived: true,
            //                                                      filter:
            //                                                          "where nop=='0000' AND kpr ==? AND BETWEEN(Dato,?,?)",
            //                                                      parameters: new[] {kpr, d1, d2});
            //var tempMpartt =
            //    FoxRepo.GetTable<CrystalDS.mparttRow>(
            //        filter: "where nop=='0000' AND kpr ==? AND BETWEEN(Dato,?,?)",
            //        parameters: new[] {kpr, d1, d2});
            //var tempAMprxop = FoxRepo.GetTable<CrystalDS.mprxopRow>(isArchived: true,filter:"where kpr ==? ",parameters: new[] {kpr}).ToList(); 
            //var tempAsabon = FoxRepo.GetTable<CrystalDS.asabonRow>();
            //var tempMpartn = FoxRepo.GetTable<CrystalDS.mpartnRow>();
            //var tempAMpartn = FoxRepo.GetTable<CrystalDS.mpartnRow>(isArchived: true);
            //var tempDMpartt = tempAMpartt.Concat(tempMpartt);
            //var tempDMpartn = tempAMpartn.Concat(tempMpartn);

            //var hex = from assop in FoxRepo.GetTable<CrystalDS.assopRow>()
            //          join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
            //              on assop.kop equals astmr.kop
            //          select new
            //              {
            //                  Nop = astmr.nop,
            //                  Naop = assop.naop,
            //                  Kmk = astmr.kmk
            //              };

            //var brak = from asspr in FoxRepo.GetTable<CrystalDS.assprRow>().Where(a => a.kpr == kpr)
            //           from mpartt in tempDMpartt
            //           join prib in tempDMpartn
            //               on mpartt.nprt  equals  prib.nprt 
            //           join asbrak in FoxRepo.GetTable<CrystalDS.asbrakRow>()
            //               on prib.kprb equals asbrak.kprb
            //           join mprxop in tempAMprxop
            //               on new { prib.nop, prib.kmk, prib.nprt, prib.kpr, prib.datbr.Date }
            //               equals new { mprxop.nop, mprxop.kmk, mprxop.nprt, mprxop.kpr, mprxop.dato.Date }
            //           join dd in hex
            //               on new { p = prib.nop, u = prib.kmk } equals new { p = dd.Nop, u = dd.Kmk } into zapr
            //           from zz in zapr.DefaultIfEmpty()
            //           group prib by
            //               new
            //               {
            //                   prib.nprt,
            //                   prib.datbr,
            //                   prib.timbr,
            //                   mprxop.owner,
            //                   Naop = (zz == null ? string.Empty : zz.Naop),
            //                   asbrak.naprb,
            //                   asspr.napr
            //               }
            //               into f
            //               select new CyclesDefectsInfo
            //               {
            //                   Nprt = f.Key.nprt,
            //                   Kpls = f.Select(a => a.npls).Count().ToString(),
            //                   Naprb = f.Key.naprb,
            //                   Datbr = f.Key.datbr.ToString(),
            //                   Timtbr = f.Key.timbr.ToString(),
            //                   Nop = f.Key.Naop,
            //                   Fio = FoxRepo.GetFioByTabNumber(f.Key.owner, tempAsabon),
            //                   Napr = f.Key.napr

            //               };
            //return View(brak.OrderBy(a => a.Nprt));
        }
        [HttpGet]
        public ActionResult ShowGodn(string kpr)
        {

            IEnumerable<cycle_period> res;
            res = bl.ShowGodn(kpr);
            return View(res);

        //    var d1 = TempData.Peek("dt1");
        //    var d2 = TempData.Peek("dt2");
        //    var data_poln = (IEnumerable<cycle_period>)TempData.Peek("cycleDataPoln");
        //    //var datasredn = (IEnumerable<cycle_period>)TempData.Peek("cycleDataSredn");
        //    var data_sredn = (IEnumerable<cycle_period>)TempData.Peek("cycle_data_sredn");
       
        //    var tempAMpartt = FoxRepo.GetTable<CrystalDS.mparttRow>(isArchived: true,filter:"where nop=='0000' AND kpr ==? AND BETWEEN(Dato,?,?)",parameters: new[] {kpr, d1, d2});
        //    var tempMpartt =FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "where nop=='0000' AND kpr == ? AND BETWEEN(Dato,?,?)",parameters: new[] {kpr, d1, d2});
        //    var tempAMprxop = FoxRepo.GetTable<CrystalDS.mprxopRow>(isArchived: true);
        //    var tempMprxop = FoxRepo.GetTable<CrystalDS.mprxopRow>();
        //    var tempAsabon = FoxRepo.GetTable<CrystalDS.asabonRow>();
        //    var tempMpartn = FoxRepo.GetTable<CrystalDS.mpartnRow>();
        //    var tempAMpartn = FoxRepo.GetTable<CrystalDS.mpartnRow>(isArchived: true);
        //    var tempDMpartt = tempAMpartt.Concat(tempMpartt);
        //    var tempDMprxop = tempAMprxop.Concat(tempMprxop);
        //    var tempDMpartn = tempAMpartn.Concat(tempMpartn);
        //    var tempAskrm = FoxRepo.GetTable<CrystalDS.askrmRow>();


        //    var cycles_poln = (from cd in data_poln
        //                          group cd by new {kpr = cd.Kpr,Nprt=cd.Nprt} into ds
        //                          select new cycle_period
        //                              {
        //                                  Kpr = ds.Key.kpr,
        //                                  Nprt = ds.Key.Nprt,
        //                                  cycle_pln = ds.Average(a => FoxRepo.WorkDaysBetween(a.dath, a.dato, tempAskrm)).ToString()
        //                              }).ToList();
        //    var cycles_sredn = (from cd in data_sredn
        //                        group cd by new { kpr = cd.Kpr, Nprt=cd.Nprt} into ds
        //                      select new cycle_period
        //                      {
        //                          Kpr = ds.Key.kpr,
        //                          Nprt = ds.Key.Nprt,
        //                          cycle_sredn = ds.Average(a => FoxRepo.WorkDaysBetween(a.dath, a.dato, tempAskrm)).ToString()
        //                      }).ToList();

        //    var brak =from asspr in FoxRepo.GetTable<CrystalDS.assprRow>().Where(a=>a.kpr==kpr)
               

        //        from mpartt in tempDMpartt
        //        join mpartn in tempDMpartn
        //        on mpartt.nprt equals mpartn.nprt into zz
        //        join poln in cycles_poln on mpartt.nprt equals poln.Nprt into c_p
        //        join sredn in cycles_sredn on mpartt.nprt equals sredn.Nprt into c_s
        //        select new cycle_period
        //                            {
        //                                Nprt        =mpartt.nprt,
        //                                kpls        =mpartt.kpls.ToString(),// gg.Key.kpls.ToString(),
        //                                Npls        =zz.Select(a=>a.npls).Count().ToString(),// gg.Key.sum_brak.ToString(),
        //                                cycle_pln   = c_p.Any()?c_p.First().cycle_pln:"0",
        //                                cycle_sredn = c_s.Any()?c_s.First().cycle_sredn:"0",
        //                                Napr        = asspr.napr
        //                            };
                
        //           return View(brak.OrderBy(a=>a.Nprt));
        }
    }
}
