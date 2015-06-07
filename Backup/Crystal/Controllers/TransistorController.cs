using System.Collections.Generic;
using System.Web.Mvc;
using NLog;
using Crystal.Models;
using System.Linq;
using MvcApplication.Models;
using System;
using CrystalDataSetLib;
using Crystal.BusinessLogic;
using Crystal.Infrastructure;
using System.Web;

namespace Crystal.Controllers
{
    [PlantAuthorization]
    [LogAction]
    public class TransistorController : Controller
    {
        ICrystalLogic bl = BusinessLogicFactory.GetBL();
        
        public ActionResult Production()
        {
            var vModel = bl.Production();
            ViewBag.log = "Пользователь {User} просматривает данные по производству №{Plant}.";
            return View(vModel);
        }

        public ActionResult FindBadDates()
        {
            List<string> res = new List<string>();

            var t_mprxop = FoxRepo.GetTable<CrystalDS.mprxopRow>().GroupBy(a => a.nprt);
            var t_mrest = FoxRepo.GetTable<CrystalDS.mrestRow>();
            var t_mpartt = FoxRepo.GetTable<CrystalDS.mparttRow>();


            foreach (var batch in t_mprxop)
            {
                if (t_mpartt.Where(a => a.nprt == batch.Key).Count() == 0)
                    continue;

                var routeData = batch.OrderBy(a => a.nop);
                var firstRec = routeData.FirstOrDefault();
                if (firstRec == null)
                    continue;

                var currDatO = firstRec.dato;
                var currDatH = firstRec.dath;

                foreach (var recMprxop in routeData)
                {
                    if (recMprxop.dato < recMprxop.dath)
                    {
                        res.Add(recMprxop.nprt + " -- ► " + recMprxop.nop +" : "+recMprxop
                            .dato.ToShortDateString());
                        break;
                    }

                    if (recMprxop.dato < currDatO)
                    {
                        var part = recMprxop.nprt;
                        if (t_mrest.Where(a => a.nprt == recMprxop.nprt).Count() > 0)
                            part = part + " **R**";
                        res.Add(part + " -- ▲ " + recMprxop.nop + " : " + currDatO.ToShortDateString());
                        break;
                    }

                    currDatO = recMprxop.dato;
                    currDatH = recMprxop.dath;
                }
            }

            ViewBag.res = res;
            return View();
        }

    }


}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            