using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crystal.BusinessLogic.Abstract;
using Crystal.DomainModel;
using CrystalDataSetLib;
namespace Crystal.VFPOleDBLogic
{
    public partial class BusinessLogicOleDb : IStatisticLogic
    {
        public IEnumerable<GetAnalysis1> GetAnalysis(string fdate, string sdate, string indnumb)
        {
            var d1 = DateTime.Parse(fdate) as object;
            var d2 = DateTime.Parse(sdate) as object;
            var Mprox = FoxRepo.GetTable<CrystalDS.mprxopRow>(filter: "WHERE BETWEEN(dato,?,?)", parameters: new[] { d1, d2 });
            var Amprrox = FoxRepo.GetTable<CrystalDS.mprxopRow>(isArchived: true, filter: "where between(dato,?,?)", parameters: new[] { d1, d2 });
            var kpr = (from mprox in Mprox.Concat(Amprrox)
                       join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                           on mprox.kpr equals asspr.kpr
                       group mprox by new { mprox.kpr, asspr.napr }
                           into zabr1
                           select new GetAnalysis1
                           {
                               Kpr = zabr1.Key.kpr,
                               Napr = zabr1.Key.napr
                           }).OrderBy(a => a.Kpr);


            var kpls = (from mprox in Mprox.Concat(Amprrox)
                        join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
                        on new { mprox.nop, mprox.kmk } equals new { astmr.nop, astmr.kmk }
                        join assop in FoxRepo.GetTable<CrystalDS.assopRow>()
                        on astmr.kop equals assop.kop
                        where assop.pfl != "5"
                        group mprox by new { mprox.dato, mprox.kpr } into zabr1
                        orderby zabr1.Key.kpr
                        select new GetAnalysis2
                        {
                            Kpr = zabr1.Key.kpr,
                            Kpls = zabr1.Sum(a => a.kpls).ToString(),
                            Dato = zabr1.Key.dato.ToString()
                        }).OrderBy(a => a.Dato);

            memory.Set("Mprox", kpls, null, null);
            memory.Set("Napr", kpr, null, null);

            return kpr;

        }



        public IEnumerable<GetAnalysisDiag> GetAnalysisDiag(string[] kpr, string input)
        {
            var mprxop = (IEnumerable<GetAnalysis2>)memory.Get("Mprox");
            var napr = (IEnumerable<GetAnalysis1>)memory.Get("Napr");

            decimal sumnzps =
                FoxRepo.GetTable<CrystalDS.mparttRow>().Where(a => kpr.Contains(a.kpr)).Join(FoxRepo.GetTable<CrystalDS.astmrRow>(), p => new { p.nop, p.kmk }, n => new { n.nop, n.kmk }, (p, n) => new { n.kop, p.kpls }).Join(FoxRepo.GetTable<CrystalDS.assopRow>(), n => n.kop, p => p.kop, (n, p) => new { n.kpls, p.pfl }).Where(a => a.pfl != "5").Sum(a => a.kpls);

            var kpls = (from n in mprxop
                        where kpr.Contains(n.Kpr)
                        group n by n.Dato
                            into gg

                            select new GetAnalysisDiag
                            {
                                Dato = gg.Key,
                                Kpls = gg.Sum(a => int.Parse(a.Kpls)),
                                Kob = sumnzps != 0 ? ((decimal)gg.Sum(a => int.Parse(a.Kpls)) / sumnzps).ToString("F2") : "0"
                            }).OrderBy(a => DateTime.Parse(a.Dato));

            return kpls;
        }


        public ParetoData GetParetoData(string fdate, string sdate, string indnumb)
        {
           
            //ViewBag.Period = FoxRepo.GetPeriod(fdate, sdate, indnumb);
            var d1 = DateTime.Parse(fdate) as object;
            var d2 = DateTime.Parse(sdate) as object;
            var tempMpartn = FoxRepo.GetTable<CrystalDS.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?)", parameters: new[] { d1, d2 });
            var tempAmpartn = FoxRepo.GetTable<CrystalDS.mpartnRow>(isArchived: true, filter: "where between(datbr,?,?)", parameters: new[] { d1, d2 });

            memory.Set("TempMpartn", tempMpartn, null, null);
            memory.Set("TempAmpartn", tempAmpartn, null, null);


            var UchList = FoxRepo.GetTable<CrystalDS.askuchRow>().Select(x => new UchList { Kuch = x.kuch, Nauch = x.nauch }).OrderBy(x => int.Parse(x.Kuch)).ToArray();

            var ts = from tmp in tempMpartn.Union(tempAmpartn)
                     select new { Kpr = tmp.kpr, };

            var PribList = (from t1 in ts.Distinct()
                                join t2 in FoxRepo.GetTable<CrystalDS.assprRow>() on t1.Kpr equals t2.kpr
                                select new PribList
                                {
                                    Kpr = t1.Kpr,
                                    Napr = t2.napr
                                }).OrderBy(x => int.Parse(x.Kpr)).ToArray();


            //IEnumerable<ParetoData> res ;
            var res = new ParetoData{PribList=PribList,UchList=UchList};




            return res;
    }

        public IEnumerable<ParetoDiag>  GetParetoDiag(string[] kpr, string[] kuch)
        {
            var tempData = ((IEnumerable<CrystalDS.mpartnRow>)memory.Get("TempMpartn")).Union(((IEnumerable<CrystalDS.mpartnRow>)memory.Get("TempAmpartn"))).ToList();
            
     
            var allUchs = from t in FoxRepo.GetTable<CrystalDS.astobRow>()
                          where kuch.Contains(t.kuch)
                          select t;

            var allPribs = (from t in tempData
                            where kpr.Contains(t.kpr)
                            select t).ToList();

            var result = (from t1 in allUchs
                          join t2 in allPribs
                          on new { t1.kgt, t1.kus } equals new { t2.kgt, t2.kus }
                          join asbrak in FoxRepo.GetTable<CrystalDS.asbrakRow>() on t2.kprb equals asbrak.kprb
                          group t1 by new { t2.kprb, asbrak.naprb } into gr
                          select new ParetoDiag
                          {
                              Naprb = gr.Key.naprb,
                              Brk = gr.Count().ToString()
                          }).OrderByDescending(x => int.Parse(x.Brk));

            var max = result.Sum(x => int.Parse(x.Brk));
            //TempData["Max"] = max.ToString("D0");
            //TempData["Result"] = result;


          
            return result;
        }


        //public ActionResult JsonToParetoDiag()
        //{
        //    var result = (IEnumerable<ParetoDiag>)TempData.Peek("Result");
        //    var max = float.Parse((string)TempData.Peek("Max"));
        //    var l1 = result.Select(x => x.Naprb).ToList();
        //    var l2 = result.Select(x => int.Parse(x.Brk)).ToList();

        //    var t = new List<float>();
        //    float summator = 0;
        //    foreach (var item in result)
        //    {
        //        float temp = (float.Parse(item.Brk) / max) * 100 + summator;
        //        t.Add(temp);
        //        summator = temp;
        //    }
        //    return Json(new { Naprb = l1, Brk = l2, Accum = t }, JsonRequestBehavior.AllowGet);
        //}

    }
}




