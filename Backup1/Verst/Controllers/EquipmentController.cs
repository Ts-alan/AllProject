using System;
using System.Web.Mvc;
using MvcApplication.Models;
using System.Linq;
using NLog;
using Verst.Models;


namespace Verst.Controllers
{
    [PlantAuthorization]
    public class EquipmentController : AsyncController
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        private DateTime _d1, _d2, _d3;

        // Задаём значения
        public EquipmentController()
        {
            _d2 = DateTime.Now;
            _d1 = DateTime.Parse("01." + _d2.Month + "." + _d2.Year);
            _d3 = DateTime.Parse("01." + _d1.Month + "." + (_d1.Year - 1));
        }
        public ActionResult Dynamics()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LoadDynamics()
        {
            _log.Info("Пользователь {0} запросил диаграммы по загрузке оборудования.",User.Identity.Name);
            return View();
        }


        [HttpGet]
        public ActionResult LdPlot()
        {
            ViewBag.Prn = (from asparam in FoxRepo.GetTable<Crystal.asparamRow>()
                           where asparam.identp.Trim() == "FACTORY"
                           select asparam.valp).First().Trim();
            try
            {
                var query = (from dat in FoxRepo.GetTable<Crystal.mprxopRow>(isArchived: true, filter: "where between(Dato,?,?)",
                                                        parameters: new[] { _d1 as object, _d2 as object })
                                                        .Concat(FoxRepo.GetTable<Crystal.mprxopRow>(filter: "where between(Dato,?,?)",
                                                        parameters: new[] { _d1 as object, _d2 as object }))
                            join astob in FoxRepo.GetTable<Crystal.astobRow>()
                             on new { dat.kgt, dat.kus } equals new { astob.kgt, astob.kus }
                            group dat by new{astob.snaobor, astob.kgt} into gRes
                            select new LdPlot { Ust = gRes.Key.snaobor.Trim().Replace("\"", ""), Kpls = gRes.Sum(x => x.kpls), Kgt = gRes.Key.kgt}).OrderBy(x=>int.Parse(x.Kgt));
                return Json(new { Kpls = query.Select(x => x.Kpls).ToList(), Ust = query.Select(x => x.Ust).ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e1)
            {
                _log.Error("Произошла ошибка при построении графика загрузки оборудования за месяц! {0}",e1.Message);
                return null;
            }
        }
       
       
        public ActionResult LdPlotByDate(string fdate, string sdate)
        {
            try
            {
                DateTime d1 = DateTime.Parse(fdate);
                DateTime d2 = DateTime.Parse(sdate);
                _log.Info("Пользователь {0} запросил графика загрузки оборудования", User.Identity.Name);

                var query = (from dat in FoxRepo.GetTable<Crystal.mprxopRow>(isArchived: true, filter: "where between(Dato,?,?)",
                                                        parameters: new[] { d1 as object, d2 as object })
                                                        .Concat(FoxRepo.GetTable<Crystal.mprxopRow>(filter: "where between(Dato,?,?)",
                                                        parameters: new[] { d1 as object, d2 as object }))
                            join astob in FoxRepo.GetTable<Crystal.astobRow>()
                             on new { dat.kgt, dat.kus } equals new { astob.kgt, astob.kus }
                            group dat by new{astob.snaobor, astob.kgt} into gRes
                            select new LdPlot { Ust = gRes.Key.snaobor.Trim().Replace("\"", ""), Kpls = gRes.Sum(x => x.kpls), Kgt = gRes.Key.kgt}).OrderBy(x=>int.Parse(x.Kgt));

                return Json(new { Kpls = query.Select(x => x.Kpls).ToList(), Ust = query.Select(x => x.Ust).ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e1)
            {
                _log.Error("Произошла ошибка при построении графика загрузки оборудования->  {0}",e1.Message);
                return null;
            }
        }

        [HttpGet]
        public ActionResult LdPlotYear()
        {
            try
            {
                var query = (from dat in FoxRepo.GetTable<Crystal.mprxopRow>(isArchived: true, filter: "where between(Dato,?,?)",
                                                        parameters: new[] { _d3 as object, _d2 as object })
                                                        .Concat(FoxRepo.GetTable<Crystal.mprxopRow>(filter: "where between(Dato,?,?)",
                                                        parameters: new[] { _d3 as object, _d2 as object }))
                            join astob in FoxRepo.GetTable<Crystal.astobRow>()
                             on new { dat.kgt, dat.kus } equals new { astob.kgt, astob.kus }
                            group dat by new{astob.snaobor, astob.kgt} into gRes
                            select new LdPlot { Ust = gRes.Key.snaobor.Trim().Replace("\"", ""), Kpls = gRes.Sum(x => x.kpls), Kgt = gRes.Key.kgt}).OrderBy(x=>int.Parse(x.Kgt));

                return Json(new { Kpls = query.Select(x => x.Kpls).ToList(), Ust = query.Select(x => x.Ust).ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e2)
            {
                _log.Error("Произошла ошибка при построении графика загрузки оборудования за год! {0}", e2.Message);
                return null;
            }
        }
    }
}
