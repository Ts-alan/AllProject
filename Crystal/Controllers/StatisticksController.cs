using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Crystal.DomainModel;
using MvcApplication.Models;
using NLog;
using Crystal.Models;
using CrystalDataSetLib;
using Crystal.BusinessLogic;
namespace Crystal.Controllers
{

    public class StatisticksController : Controller
    {

        ICrystalLogic bl = BusinessLogicFactory.GetBL();
        private static Logger _log = LogManager.GetCurrentClassLogger();


        public ActionResult ParetoDiag()
        {
            ViewBag.Action = "GetParetoData";
            ViewBag.Controller = "Statisticks";
            ViewBag.Title = "Распределение брака";
            return View("DateForm");
        }


        public ActionResult GetParetoData(string fdate, string sdate, string indnumb)
        {
          
            ViewBag.Period = FoxRepo.GetPeriod(fdate, sdate, indnumb);

            var res = bl.GetParetoData(fdate,sdate,indnumb);

             ViewBag.PribList = res.PribList;
             ViewBag.UchList = res.UchList;
           
            return View(res);
        }
        public ActionResult GetAnalysis(string fdate, string sdate, string indnumb)
        {

            ViewBag.Period = FoxRepo.GetPeriod(fdate, sdate, indnumb);

            ViewBag.Mprox = bl.GetAnalysis(fdate, sdate, indnumb);


            return View();

        }
        public ActionResult GetAnalysisDiag(string[] kpr, string input)
        {

            ViewBag.Prn = bl.GetProductionNumber();

            if (kpr == null)
            {
                return null;
            }
            else
            {

                var kpls = bl.GetAnalysisDiag(kpr, input);


                if (input == "")
                {
                    return View("GetAnalysisDiag", kpls);
                }
                else
                {
                    return View("GetAnalysisDiagTable", kpls);
                }
            }
        }

        [HttpGet]
        public ActionResult GetParetoDiag(string[] kpr, string[] kuch)
        {
          
            _log.Info("Пользователь {0} запросил построение диаграммы парето.", User.Identity.Name);
         
            if (kpr == null && kuch == null)//Если не выбраны приборы/участки -> ничего не делаем
            {
                ViewData.Model = null;
                return View();
            }
            //-------------------------------------------------------------------------------------------------------------------------------
            var viewCode = Guid.NewGuid();
            ViewBag.ViewCode = viewCode.ToString();
            TempData["Message"] = "Диаграмма Парето";

            var result = bl.GetParetoDiag(kpr, kuch);



            var max = result.Sum(x => int.Parse(x.Brk));
            TempData["Max" + viewCode.ToString()] = max.ToString("D0");
            TempData["Result" + viewCode.ToString()] = result;


            ViewData.Model = Url.Action("JsonToParetoDiag");
            return View();
        }

        [HttpGet]
        public ActionResult JsonToParetoDiag(string ViewCode)
        {
            var result = (IEnumerable<ParetoDiag>)TempData.Peek("Result"+ViewCode);
            var max = float.Parse((string)TempData.Peek("Max" + ViewCode));
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
            ViewBag.Action = "GetAnalysis";
            ViewBag.Controller = "Statisticks";
            ViewBag.Title = "Анализ движения";
            return View("DateForm");
        }

    }
}
