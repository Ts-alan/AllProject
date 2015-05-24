using System.Collections.Generic;
using System.Web.Mvc;
using NLog;
using Verst.Models;
using System.Linq;
using MvcApplication.Models;
using System;

namespace Verst.Controllers
{
    [PlantAuthorization]
    public class TransistorController : Controller
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public ActionResult Production()
        {
            var vModel = new TransistorViewModel();
            var xx = FoxRepo.GetTable<Crystal.asparamRow>(filter: "WHERE identp == ?", parameters: new[] { "start_ms" })
               .First()
               .valp.Substring(0, 10);//Формируем дату отсчёта

            DateTime d1 = DateTime.Parse(xx), d2 = DateTime.Now;

            var data = FoxRepo.GetTable<Crystal.asmnpRow>();

            decimal nzpoNM = 0, sform = 0, nzpoALL = 0, brak = 0;
            nzpoNM = data.Sum(a => a.nzpo);
            sform = data.Where(a => a.nop == "0001").Sum(a => a.pso3);
            nzpoALL = data.Sum(a => a.pso3 + a.nzpo - a.sdo3 - a.brk3);
            brak = data.Sum(a => a.brk3);

            var z2_data = (from asmpn in FoxRepo.GetTable<Crystal.asmnpRow>()
                        join astmr in FoxRepo.GetTable<Crystal.astmrRow>() on new { asmpn.nop, asmpn.kmk } equals new { astmr.nop, astmr.kmk }
                        join assop in FoxRepo.GetTable<Crystal.assopRow>() on astmr.kop equals assop.kop
                        where assop.pfl == "5"
                        group asmpn by new { assop.pfl } into gg
                        select new { Nzp = gg.Sum(a => a.nzpo), Nzp2tek = gg.Sum(a => a.nzpo) + gg.Sum(a => a.pso3) - gg.Sum(a => a.sdo3) - gg.Sum(a => a.brk3), Brk3 = gg.Sum(a => a.brk3) }).FirstOrDefault();

            decimal zapr1MparttSum = FoxRepo.GetTable<Crystal.mparttRow>(filter:"WHERE nop=='0000'").Sum(a=>a.kpls);
            decimal zapr1AMparttSum = FoxRepo.GetTable<Crystal.mparttRow>(isArchived: true, filter: "WHERE BETWEEN(dato,?,?)", parameters: new[] { d1 as object, d2 as object }).Sum(a => a.kpls);
           
            var dd = new List<AllNzp>
                {
                    new AllNzp
                        {
                            Nzp = (nzpoNM - (z2_data != null ? z2_data.Nzp : 0)).ToString(),
                            NzpPr2 = (z2_data != null ? z2_data.Nzp : 0).ToString(),
                            Sform = sform.ToString(),
                            NowNzp = (nzpoALL - (z2_data != null ? z2_data.Nzp2tek : 0)).ToString(),
                            NowNzpPr2 = (z2_data != null ? z2_data.Nzp2tek : 0).ToString(),
                            KplsPr2 = (z2_data != null ? z2_data.Brk3 : 0).ToString(),
                            Kpls = (brak - (z2_data != null ? z2_data.Brk3 : 0)).ToString(),
                            Sdo = (zapr1MparttSum + zapr1AMparttSum).ToString(),
                            KplsForDay=data.Sum(a=>a.sdo2).ToString(),
                            KplsForMount=data.Sum(a=>a.sdo3).ToString() 
                        }
                };

            vModel.AllNzps = dd;

            //--------------------------------------------------------------------------------------------------------------------------------------//
            var t_mpartt1 = FoxRepo.GetTable<Crystal.mparttRow>(filter:"Where !EMPTY(dath)");
            var t_mpartt2 = FoxRepo.GetTable<Crystal.mparttRow>(filter:"Where !EMPTY(dato)");
            var t_astob = FoxRepo.GetTable<Crystal.astobRow>();
            var t_askor = FoxRepo.GetTable<Crystal.askorRow>();

            var t_astmr = FoxRepo.GetTable<Crystal.astmrRow>();
            var t_as_wop = FoxRepo.GetTable<Crystal.as_wopRow>();

            var t_assufl = FoxRepo.GetTable<Crystal.assuflRow>();

            // посчитаем партии готовые к запуску

            var lnch_ready = (from mpartt in t_mpartt2
                              join astob in t_astob
                                 on mpartt.kgt equals astob.kgt
                              join astmr in t_astmr
                                 on new { kmk = mpartt.kmk, nop = mpartt.nop } equals new { kmk = astmr.kmk, nop = astmr.nop }
                              join as_wop in t_as_wop
                                 on new { kgt = mpartt.kgt, kus = astob.kus, kop = astmr.kop } equals new { kgt = as_wop.kgt, kus = as_wop.kus, kop = as_wop.kop }
                              let nuf = t_assufl.SingleOrDefault(a => a.kgt == as_wop.kgt && a.kus == as_wop.kus) //FoxRepo.GetTableNew<Crystal.assuflDataTable>(filter: "WHERE kgt == ? AND kus == ? ", parameters: new[] { kgt, kus }).SingleOrDefault()
                              where ((nuf == null) || (nuf.nuf == mpartt.nprt.Substring(0, 2)))
                              select new
                              {
                                  part = mpartt,
                                  eq = astob
                              }).ToList();


            vModel.ModuleMaps = (from askor in t_askor
                              join astob in t_astob
                                 on askor.kkor equals astob.kkor into g_astob
                              select new ModuleMap
                              {
                                  Kkor = askor.kkor,
                                  Nakor = askor.nakor,
                                  obor = 
                                  (from ast in g_astob
                                   join mpartt in t_mpartt1
                                      on new { kgt = ast.kgt, kus = ast.kus } equals new { kgt = mpartt.kgt, kus = mpartt.kus } into j_mpartt
                                   join mpartt2 in lnch_ready
                                      on new { kgt = ast.kgt, kus = ast.kus } equals new { kgt = mpartt2.eq.kgt, kus = mpartt2.eq.kus } into j_mpartt2
                                   select new FacilityData
                                   {
                                       astob_data = ast,
                                       Nzp = j_mpartt.Count() > 0 ? j_mpartt.Sum(a => a.kpls) : 0,
                                       Wait = j_mpartt2.Count() > 0 ? j_mpartt2.Sum(a => a.part.kpls) : 0

                                   })
                              });
            vModel.Prn = FoxRepo.GetProductionNumber();

            _log.Info("Пользователь {0} просматривает данные по производству №{1}.", User.Identity.Name, vModel.Prn);
            return View(vModel);
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            