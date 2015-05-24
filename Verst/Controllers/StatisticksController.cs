using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcApplication.Models;
using NLog;
using Verst.Models;

namespace Verst.Controllers
{
 
    public class StatisticksController : Controller
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        
        public ActionResult ParetoDiag()
        {

            return View();
        }

        
        public ActionResult GetParetoData(string fdate, string sdate)
        {
            _log.Info("Пользователь {0} запросил данные для диаграммы парето за промежуток с {1} по {2}", User.Identity.Name, fdate, sdate);

            var d1 = DateTime.Parse(fdate) as object;
            var d2 = DateTime.Parse(sdate) as object;
            var tempMpartn = FoxRepo.GetTable<Crystal.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?)", parameters: new[] { d1, d2 });
            var tempAmpartn = FoxRepo.GetTable<Crystal.mpartnRow>(isArchived: true, filter: "where between(datbr,?,?)", parameters: new[] { d1, d2 });
            TempData["TempAmpartn"] = tempAmpartn;
            TempData["TempMpartn"] = tempMpartn;

            ViewBag.UchList = FoxRepo.GetTable<Crystal.askuchRow>().Select(x => new UchList { Kuch = x.kuch, Nauch = x.nauch }).OrderBy(x => int.Parse(x.Kuch));

            var ts = from tmp in tempMpartn.Union(tempAmpartn)
                     select new { Kpr = tmp.kpr, };

            ViewBag.PribList = (from t1 in ts.Distinct()
                                join t2 in FoxRepo.GetTable<Crystal.assprRow>() on t1.Kpr equals t2.kpr
                                select new PribList
                                {
                                    Kpr = t1.Kpr,
                                    Napr = t2.napr
                                }).OrderBy(x => int.Parse(x.Kpr));
            return View();
        }
        public ActionResult GetAnalysis(string fdate, string sdate)
        {
            var d1 = DateTime.Parse(fdate) as object;
            var d2 = DateTime.Parse(sdate) as object;
            var Mprox = FoxRepo.GetTable<Crystal.mprxopRow>(filter: "WHERE BETWEEN(dato,?,?)", parameters: new[] { d1, d2 });
            var Amprrox = FoxRepo.GetTable<Crystal.mprxopRow>(isArchived: true, filter: "where between(dato,?,?)", parameters: new[] { d1, d2 });
            var kpr = (from mprox in Mprox.Concat(Amprrox)
                       join asspr in FoxRepo.GetTable<Crystal.assprRow>()
                           on mprox.kpr equals asspr.kpr
                       group mprox by new { mprox.kpr, asspr.napr }
                           into zabr1
                           select new GetAnalysis1
                           {
                               Kpr = zabr1.Key.kpr,
                               Napr = zabr1.Key.napr
                           }).OrderBy(a => a.Kpr);




            var kpls = (from mprox in Mprox.Concat(Amprrox)
                        join astmr in FoxRepo.GetTable<Crystal.astmrRow>()
                        on new {mprox.nop,mprox.kmk} equals new {astmr.nop,astmr.kmk}
                        join assop in FoxRepo.GetTable<Crystal.assopRow>()
                        on astmr.kop equals assop.kop
                        where assop.pfl!="5"
                        group mprox by new { mprox.dato, mprox.kpr } into zabr1
                        orderby zabr1.Key.kpr
                        select new GetAnalysis2
                        {
                            Kpr = zabr1.Key.kpr,
                            Kpls = zabr1.Sum(a => a.kpls).ToString(),
                            Dato = zabr1.Key.dato.ToString()
                        }).OrderBy(a => a.Dato);
            TempData["Mprox"] = kpls;
            TempData["Napr"] = kpr;
            TempData["период1"] = fdate;
            TempData["период2"] = sdate;
            ViewBag.Mprox = kpr;


            return View();

        }
        public ActionResult GetAnalysisDiag(string[] kpr, string input)
        {
            var mprxop = (IEnumerable<GetAnalysis2>)TempData.Peek("Mprox");
            var napr = (IEnumerable<GetAnalysis1>)TempData.Peek("Napr");
            


            if (kpr == null)
            {
                return null;
            }
            else
            {
                if (kpr.Count() == 1)
                {
                    ViewBag.Napr = "Прибор " + napr.Where(a => a.Kpr == kpr[0]).Single().Napr;
                }
                decimal sumnzps =
                    FoxRepo.GetTable<Crystal.mparttRow>().Where(a => kpr.Contains(a.kpr)).Join(FoxRepo.GetTable<Crystal.astmrRow>(), p => new { p.nop, p.kmk }, n => new { n.nop, n.kmk }, (p, n) => new { n.kop, p.kpls }).Join(FoxRepo.GetTable<Crystal.assopRow>(),n=>n.kop,p=>p.kop,(n,p)=>new{n.kpls,p.pfl}).Where(a=>a.pfl!="5").Sum(a => a.kpls);

                

                var kpls = (from n in mprxop
                            where kpr.Contains(n.Kpr)
                            group n by n.Dato
                            into gg

                                select new GetAnalysisDiag
                                {
                                    Dato = gg.Key,
                                    Kpls = gg.Sum(a => int.Parse(a.Kpls)),
                                    Kob =sumnzps!=0? ((decimal)gg.Sum(a => int.Parse(a.Kpls))/sumnzps).ToString("F2"):"0"
                                }).OrderBy(a => DateTime.Parse(a.Dato));


                if (input == "")
                {
                    return View("GetAnalysisDiag", kpls);
                }
                else
                {
                    ViewBag.period1 = TempData.Peek("период1");
                    ViewBag.period2 = TempData.Peek("период2");
                    return View("GetAnalysisDiagTable", kpls);
                }
            }
        }
       
        [HttpGet]
        public ActionResult GetParetoDiag(string[] kpr, string[] kuch)
        {
            _log.Info("Пользователь {0} запросил построение диаграммы парето.", User.Identity.Name);
            var tempData = ((IEnumerable<Crystal.mpartnRow>)TempData.Peek("TempMpartn")).Union(((IEnumerable<Crystal.mpartnRow>)TempData.Peek("TempAmpartn"))).ToList();
            //-------------------------------------------------------------------------------------------------------------------------------
            if (kpr == null && kuch == null)//Если не выбраны приборы/участки -> ничего не делаем
            {
                ViewData.Model = null;
                return View();
            }
            //-------------------------------------------------------------------------------------------------------------------------------
           
          
                TempData["Message"] = "Диаграмма Парето";
                var allUchs = from t in FoxRepo.GetTable<Crystal.astobRow>()
                              where kuch.Contains(t.kuch)
                              select t;

                var allPribs = (from t in tempData
                                where kpr.Contains(t.kpr)
                                select t).ToList();

                var result = (from t1 in allUchs
                              join t2 in allPribs
                              on new { t1.kgt, t1.kus } equals new { t2.kgt, t2.kus }
                              join asbrak in FoxRepo.GetTable<Crystal.asbrakRow>() on t2.kprb equals asbrak.kprb
                              group t1 by new { t2.kprb, asbrak.naprb } into gr
                              select new ParetoDiag
                              {
                                  Naprb = gr.Key.naprb,
                                  Brk = gr.Count().ToString()
                              }).OrderByDescending(x => int.Parse(x.Brk));

                var max = result.Sum(x => int.Parse(x.Brk));
                TempData["Max"] = max.ToString("D0");
                TempData["Result"] = result;

            
            ViewData.Model = Url.Action("JsonToParetoDiag");
            return View();
        }

        [HttpGet]
        public ActionResult JsonToParetoDiag()
        {
            var result = (IEnumerable<ParetoDiag>)TempData.Peek("Result");
            var max = float.Parse((string)TempData.Peek("Max"));
            var l1 = result.Select(x => x.Naprb).ToList();
            var l2 = result.Select(x => int.Parse(x.Brk)).ToList();

            var t = new List<float>();
            float summator = 0;
            foreach (var item in result)
            {
                float temp = (float.Parse(item.Brk) / max) * 100 + summator;
                t.Add(temp);
                summator = temp;
            }
            return Json(new { Naprb = l1, Brk = l2, Accum = t }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Form()
        {
            return View();
        }

    }
}
