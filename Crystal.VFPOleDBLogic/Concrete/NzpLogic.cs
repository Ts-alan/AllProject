using Crystal.BusinessLogic.Abstract;
using Crystal.DomainModel;
using CrystalDataSetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

///////////Отчет по приборам/////////////////////////////////

namespace Crystal.VFPOleDBLogic
{
    public partial class BusinessLogicOleDb : INzpLogic
    {
        public IEnumerable<NzpPribList> NzpForPrib(IEnumerable<CrystalDS.assopRow> t_assop)
        {

            var data = (from mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "WHERE nop!='0000'")
                        join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                            on mpartt.kpr equals asspr.kpr
                        join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
                            on new { mpartt.kmk, mpartt.nop } equals new { astmr.kmk, astmr.nop }
                        join assop in t_assop
                            on astmr.kop equals assop.kop
                        group mpartt by new { mpartt.kpr, asspr.napr }
                            into g_mpartt
                            select new NzpPribList
                            {
                                Kpr = g_mpartt.Key.kpr,
                                Napr = g_mpartt.Key.napr,
                                Nzp = Convert.ToInt32(g_mpartt.Sum(a => a.kpls)),
                                Diameter = g_mpartt.Key.napr.Substring(0, 3) == "200" ? "200" : "150"

                            }).OrderBy(a => a.Kpr);
            return data;
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
                        }).OrderBy(a => a.Pfl);
            return data;
        }


        public IEnumerable<NzpPribList> NzpPrib() //Формируем список приборов
        {
            var pribL = NzpForPrib(FoxRepo.GetTable<CrystalDS.assopRow>(filter: "WHERE pfl!='5'"));
            //ViewBag.head = head ?? string.Format("НЗП производства № {0}", FoxRepo.GetProductionNumber());
            return pribL;
        }

        public IEnumerable<NzpPribList> NZPFor2Plant() //Формируем список приборов
        {
            var pribL = NzpForPrib(FoxRepo.GetTable<CrystalDS.assopRow>(filter: "WHERE pfl=='5'"));
            //ViewBag.head = head ?? string.Format("НЗП производства № {0}", FoxRepo.GetProductionNumber());
            return pribL;
        }

        public IEnumerable<NzpPrib> GetNZPFor2Plant(string kpr)
        {
            var data = GetNzpForPrib(kpr).Where(a => a.Pfl.Equals("5")).OrderBy(a => a.Nop);
            return data;

        }


        public IEnumerable<NzpPrib> GetNzpPrib(string kpr) //Выводим данные по запросу из списка
        {
            var data = GetNzpForPrib(kpr).Where(a => !a.Pfl.Equals("5")).OrderBy(a => a.Nop);

            return data;
        }

        public IEnumerable<NzpBatchInfo> GetFromNzp(string kpr, string nop)//Раскрываем НЗП
        {
            DateTime now = DateTime.Now;
            var tempAsabon = FoxRepo.GetTable<CrystalDS.asabonRow>();
            var t_askrm = FoxRepo.GetTable<CrystalDS.askrmRow>();
            //ViewBag.observed = (new Crystal.Models.SiteAccountsTableAdapters.ObservedBatchesTableAdapter()).GetData();
            var data = (from mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "WHERE Kpr==? AND nop == ?", parameters: new[] { kpr, nop })
                        //where mpartt.kpr == kpr && mpartt.nop == nop
                        select new NzpBatchInfo
                        {
                            Nprt = mpartt.nprt,
                            Dato = mpartt.dato.isEmptyDate() ? FoxProExtensions.FullDateTime(mpartt.dath, mpartt.timh) : FoxProExtensions.FullDateTime(mpartt.dato, mpartt.timo),
                            Datf = "",
                            Kpls = mpartt.kpls.ToString(),
                            Abon = FoxRepo.GetFioByTabNumber(mpartt.owner, tempAsabon),
                            //Dath = FoxProExtensions.FullDateTime(mpartt.dath,mpartt.timh),
                            kpr = kpr,
                            TimeProc = FoxRepo.WorkDaysBetween(DateTime.Parse(mpartt.dato.GetFoxDate() != "" ? mpartt.dato.GetFoxDate() : mpartt.dath.GetFoxDate()), now.Date, t_askrm)
                        }).ToList();
            return data;
        }

        public IEnumerable<NzpFLitList> NzpFlit() //Список приборов на фотолитографию
        {
            var pribL = (from mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>()
                         join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                          on mpartt.kpr equals asspr.kpr
                         group mpartt by new { mpartt.kpr, asspr.napr, asspr.kmk } into g_mpartt
                         select new NzpFLitList
                         {
                             Kpr = g_mpartt.Key.kpr,
                             Napr = g_mpartt.Key.napr,
                             Kmk = g_mpartt.Key.kmk,
                             Nzp = Convert.ToInt32(g_mpartt.Sum(x => x.kpls))
                         }).OrderBy(a => a.Kpr);
            return pribL;
        }

        public IEnumerable<NzpFLit> GetNzpFLit(string kpr, string kmk) //Выводим данные по запросу из списка
        {
            string nnop = "0001";
            int old_nbl, nbl;
            var data = from astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
                       join assop in FoxRepo.GetTable<CrystalDS.assopRow>() on astmr.kop equals assop.kop
                       where astmr.kmk == kmk && assop.pfl != "5"
                       orderby astmr.nop
                       select new { pfl = assop.pfl, nop = astmr.nop, naop = assop.naop };

            var blks = new List<Blocks>();

            nbl = 0;
            old_nbl = -1;

            foreach (var tab in data)
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

            //TempData["Blocks"] = blks;

            memory.Set("blks", blks, null, null);

            var tabl = from mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>()
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
                             Nzp = Convert.ToInt32(g_tabl.Sum(a => a.prt.kpls)),
                             Kmk = kmk,
                             Kpr = kpr
                             
                            
                         };
            return result.OrderBy(x => int.Parse(x.Nfl));
        }

        public IEnumerable<FlitOprNzp> GetFromNzpFlit(string nfl, string kmk, string kpr)
        {
            var someblocks = (List<Blocks>)memory.Get("blks");

            var part = from assop in FoxRepo.GetTable<CrystalDS.assopRow>()
                       where assop.pfl != "5"
                       join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>() on assop.kop equals astmr.kop
                       join mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>() on new { astmr.nop, astmr.kmk } equals new { mpartt.nop, mpartt.kmk }
                       where mpartt.kmk == kmk && mpartt.kpr == kpr
                       from kt in someblocks
                       where kt.BlockN == nfl
                       where int.Parse(mpartt.nop) >= int.Parse(kt.FNop) && int.Parse(mpartt.nop) <= int.Parse(kt.LNop)
                       group mpartt by new { mpartt.nop, mpartt.nprt, mpartt.kpls, assop.naop, mpartt.dato, mpartt.dath, mpartt.timo, mpartt.timh } into g_mpartt
                       select new FlitPartNzp
                       {
                           Nprt = g_mpartt.Key.nprt,
                           Naop = g_mpartt.Key.naop,
                           Dato = g_mpartt.Key.dato.isEmptyDate() ? g_mpartt.Key.dato : g_mpartt.Key.dath,
                           Timo = FoxProExtensions.GetFoxTime(g_mpartt.Key.timo) != "" ? FoxProExtensions.GetFoxTime(g_mpartt.Key.timo) : FoxProExtensions.GetFoxTime(g_mpartt.Key.timh),
                           Nop = g_mpartt.Key.nop,
                           Nzp = Convert.ToInt32(g_mpartt.Key.kpls),
                           Kpr = kpr
                       };

            var part1 = from az in part
                        group az by new { az.Nop, az.Naop ,az.Kpr} into xz
                        select new FlitOprNzp {Kpr=xz.Key.Kpr, Nop = xz.Key.Nop, Naop = xz.Key.Naop, Nzp = xz.Sum(a => a.Nzp) };

            memory.Set("part1", part, null, null);

            return part1.OrderBy(a => int.Parse(a.Nop));
        }

        public IEnumerable<FlitPartNzp> GetFromNzpFlit2(string nop) //От операции получаем партии
        {
            var someparts = (IEnumerable<FlitPartNzp>)memory.Get("part1");
            return someparts.Where(x => x.Nop == nop);
        }

        public IEnumerable<PribKplsForDay> PribKplsForDay()
        {
            //IEnumerable<PribKplsForDay> vModel = bl.Get(RepoPlant);
            //return vModel;
            var model = (from asmnp in FoxRepo.GetTable<CrystalDS.asmnpRow>()
                         join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                             on asmnp.kpr equals asspr.kpr
                         group asmnp by new { asmnp.kpr, asspr.napr } into zabr1
                         select new PribKplsForDay
                         {
                             Kpr = zabr1.Key.kpr,
                             Napr = zabr1.Key.napr,
                             KplsForDay = zabr1.Sum(a => a.sdo2).ToString()
                         }).OrderBy(a => a.Kpr).Where(a => a.KplsForDay.Trim() != "0");
            return model;
        }

        public IEnumerable<GangPribKplsForDay> GangPribKplsForDay(string kpr)
        {
            var model =
                (from asmnp in FoxRepo.GetTable<CrystalDS.asmnpRow>(filter: "WHERE Kpr==?", parameters: new[] { kpr })
                 join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
                     on new { asmnp.kmk, asmnp.nop } equals new { astmr.kmk, astmr.nop }
                 join assop in FoxRepo.GetTable<CrystalDS.assopRow>()
                     on astmr.kop equals assop.kop
                 select new GangPribKplsForDay
                 {
                     Nop = asmnp.nop,
                     Naop = assop.naop,
                     KplsForNop = asmnp.sdo2.ToString()
                 }).OrderBy(a => a.Nop).Where(a => a.KplsForNop.Trim() != "0");

            return model;
        }

        public IEnumerable<PribKplsForMonth> PribKplsForMonth()
        {

            var start = GetMonthStartDate();
            var block = new List<Temporary>();

            object d1 = start.Date;
            string time = start.ToString("HHmm");
            var t_mprxop =
                FoxRepo.GetTable<CrystalDS.mprxopRow>(filter: "WHERE dato>? or ((dato=?) and timo>?)", parameters: new[] { d1, d1, time }).Concat(FoxRepo.GetTable<CrystalDS.mprxopRow>(isArchived: true, filter: "WHERE dato>? or ((dato=?) and timo>?)", parameters: new[] { d1, d1, time }));

            // Таблица рабочих смен
            var t_asgsm = FoxRepo.GetTable<CrystalDS.asgsmRow>();

            var ToursCount = t_asgsm.Count();

            // TODO: определить смену которая между сутками, а сейчас это последняя смена в списке

            //ViewBag.Count = ToursCount;

            var mprxop_tours = from mprxop in t_mprxop
                               select GetTourData(mprxop, t_asgsm);


            var model = (from mprxop in mprxop_tours
                         group mprxop by new { mprxop.TourDate.Value.Date } into zabr1
                         select new PribKplsForMonth
                         {
                             Date = zabr1.Key.Date,
                             KplsForDay = zabr1.Sum(b => b.Kpls).ToString(),
                             KplsForFirstTour = zabr1.Where(a => a.TourNumber == "1").Sum(b => b.Kpls).ToString(),
                             KplsForSecondTour = zabr1.Where(a => a.TourNumber == "2").Sum(b => b.Kpls).ToString(),
                             KplsForThirdTour = zabr1.Where(a => a.TourNumber == "3").Sum(b => b.Kpls).ToString(),
                         }).OrderBy(a => a.Date);

            return model;
        }


        class MprxopTour
        {
            public DateTime? TourDate { get; set; }
            public string TourNumber { get; set; }
            public int Kpls { get; set; }
        }


        MprxopTour GetTourData(CrystalDS.mprxopRow row, IEnumerable<CrystalDS.asgsmRow> asgsm)
        {
            var ToursCount = asgsm.Count();
            var tour = asgsm.SingleOrDefault(a => String.CompareOrdinal(row.timo, a.tns) >= 0 && String.CompareOrdinal(row.timo, a.tos) < 0);
            return new MprxopTour
            {
                TourDate = FoxProExtensions.FullDateTime(tour == null ? row.dato.AddDays(-1) : row.dato, row.timo),
                TourNumber = tour == null ? ToursCount.ToString() : tour.nsm,
                Kpls = Convert.ToInt32(row.kpls)
            };
        }


        public IEnumerable<DefectsDistributionPrib> DefectsDistr(string fdate, string sdate, string indnumb)
        {
            //ViewBag.Period = FoxRepo.GetPeriod(fdate, sdate, indnumb);
            //ViewBag.Prn = (from asparam in FoxRepo.GetTable<CrystalDS.asparamRow>()
            //               where asparam.identp.Trim() == "FACTORY"
            //               select asparam.valp).First().Trim();
            DateTime d1 = DateTime.Parse(fdate);
            DateTime d2 = DateTime.Parse(sdate);
            Object dt1 = d1;
            Object dt2 = d2;
            memory.Set("dt1", dt1, null, null);   //TempData["dt1"] = dt1;
            memory.Set("dt2", dt2, null, null);
            var result = DefectsDistrForPlant(d1, d2, FoxRepo.GetTable<CrystalDS.assopRow>().Where(a => a.pfl != "5"));
            return result.Where(x => x.Nzp != 0).OrderBy(x => x.Kpr);
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

        public IEnumerable<DefectsDistributionOperations> DefectsDistrOperations(string kpr)
        {

            var d1 = memory.Get("dt1");//var d1 = TempData.Peek("dt1");
            var d2 = memory.Get("dt2");

            
            IEnumerable<DefectsDistributionOperations> result;
            DefectsDistrOperationsForPlant(kpr, d1, d2, FoxRepo.GetTable<CrystalDS.assopRow>(filter: "WHERE pfl!='5'"), out result);
            //ViewBag.Kpr = kpr;
            //ViewBag.Kmk = Kmk;
            return result.OrderBy(a => a.Nop);
        }

        public void DefectsDistrOperationsForPlant(string kpr, object d1, object d2, IEnumerable<CrystalDS.assopRow> t_assop, out IEnumerable<DefectsDistributionOperations> result)
        {
            var ampartnTable = FoxRepo.GetTable<CrystalDS.mpartnRow>(isArchived: true, filter: "WHERE BETWEEN(datbr,?,?) AND kpr == ?", parameters: new[] { d1, d2, kpr });
            var mpartnTable = FoxRepo.GetTable<CrystalDS.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?) AND kpr == ?", parameters: new[] { d1, d2, kpr });

            var Kmk = (from asspr in FoxRepo.GetTable<CrystalDS.assprRow>(filter: "WHERE kpr == ?", parameters: new[] { kpr })
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
                             Nzp = gtest.Sum(a => a.pls).ToString(),
                             Kmk = Kmk
                         };

        }


        public IEnumerable<DefDistrData> DefectsDistrData(string nop, string kpr, string kmk)
        {
            var d1 = memory.Get("dt1");
            var d2 = memory.Get("dt2");
            var ampartnTable = FoxRepo.GetTable<CrystalDS.mpartnRow>(isArchived: true, filter: "WHERE BETWEEN(datbr,?,?) AND kpr == ? AND kmk == ? AND nop == ?", parameters: new[] { d1, d2, kpr, kmk, nop });
            var mpartnTable = FoxRepo.GetTable<CrystalDS.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?) AND kpr == ? AND kmk == ? AND nop == ?", parameters: new[] { d1, d2, kpr, kmk, nop });

            var test = (from t1 in ampartnTable
                        select new { nprt = t1.nprt, npls = t1.npls, Datbr = t1.datbr, kprb = t1.kprb, kgt = t1.kgt }).Union
                       (from t2 in mpartnTable
                        select new { nprt = t2.nprt, npls = t2.npls, Datbr = t2.datbr, kprb = t2.kprb, kgt = t2.kgt });

            var result = from t1 in FoxRepo.GetTable<CrystalDS.askprbRow>()
                         join t2 in test on new { t1.kprb, t1.kgt } equals new { t2.kprb, t2.kgt }
                         select new DefDistrData
                         {
                             Nprt = t2.nprt,
                             Npls = t2.npls.Substring(11, 2),
                             Naprb = t1.naprb,
                             Datbr = t2.Datbr.ToShortDateString()
                         };
            return result.OrderBy(x => x.Nprt);
        }


        public IEnumerable<DefectsDistributionPrib> DefectsDistrFor2Plant(string fdate, string sdate, string head)
        {
            DateTime d1 = DateTime.Parse(fdate);
            DateTime d2 = DateTime.Parse(sdate);
            Object dt1 = d1;
            Object dt2 = d2;
            memory.Set("dt1", dt1, null, null); 
            memory.Set("dt2", dt2, null, null);
            var result = DefectsDistrForPlant(d1, d2, FoxRepo.GetTable<CrystalDS.assopRow>().Where(a => a.pfl == "5"));
            return result.Where(x => x.Nzp != 0).OrderBy(x => x.Kpr);

        }

        public IEnumerable<DefectsDistributionOperations> DefectsDistrOperationsFor2Plant(string kpr)
        {
            var d1 = memory.Get("dt1");//var d1 = TempData.Peek("dt1");
            var d2 = memory.Get("dt2");
            
            IEnumerable<DefectsDistributionOperations> result;
            DefectsDistrOperationsForPlant(kpr, d1, d2, FoxRepo.GetTable<CrystalDS.assopRow>(filter: "WHERE pfl=='5'"), out result);

            return result.OrderBy(a => a.Nop);
        }
        public IEnumerable<SimplePribList> PribMonthReport()
        {
            var result = (from mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>()
                          join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                           on mpartt.kpr equals asspr.kpr
                          group mpartt by new { mpartt.kpr, asspr.napr, asspr.kmk } into g_mpartt
                          select new SimplePribList()
                          {
                              Kpr = g_mpartt.Key.kpr,
                              Napr = g_mpartt.Key.napr,
                              Nzp = Convert.ToInt32(g_mpartt.Sum(x => x.kpls)),
                              Diameter = g_mpartt.Key.napr.Substring(0, 3) == "200" ? "200" : "150"
                          }).OrderBy(a => a.Kpr);
            return result;
        }

        public IEnumerable<MonthReportData> GetDataBySelectedPrib(string selPrib)
        {
            memory.Set("selPrib", selPrib, null, null);
            var result = (from asmnp in
                              FoxRepo.GetTable<CrystalDS.asmnpRow>(filter: "Where Kpr == ?", parameters: new[] {selPrib})
                          join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
                              on new {asmnp.nop, asmnp.kmk} equals new {astmr.nop, astmr.kmk}
                          join assop in FoxRepo.GetTable<CrystalDS.assopRow>()
                              on astmr.kop equals assop.kop
                          group asmnp by new {asmnp.nop, assop.naop}
                          into gResult
                          let sumPso = gResult.Sum(x => x.pso3)
                          let sumSdo = gResult.Sum(x => x.sdo3)
                          let sumBrk = gResult.Sum(x => x.brk3)
                          let sumNzpo = gResult.Sum(x => x.nzpo)
                          select new MonthReportData
                              {
                                  Nop = gResult.Key.nop,
                                  Naop = gResult.Key.naop,
                                  In = sumPso.ToString(),
                                  Out = sumSdo.ToString(),
                                  Brk = sumBrk.ToString(),
                                  OutPer = ((sumSdo != 0 ? (sumSdo/(sumSdo + sumBrk)) : 0)*100).ToString("F2"),
                                  Nzpf = ((sumNzpo + sumPso) - sumSdo - sumBrk).ToString("F0")
                              }).Where(x => x.Brk != "0" || x.In != "0" || x.Out != "0" || x.Nzpf != "0")
                                .OrderBy(x => int.Parse(x.Nop));
            return result;
        }
        public IEnumerable<MonthReportDataBrak> PribMonthRepordBrak(string nop)
        {
            var kpr = memory.Get("selPrib");
             var d2 = DateTime.Now;
             var d1 = DateTime.Parse(("01." + d2.Month + "." + d2.Year));
             object date1 = d1;
             object date2 = d2;
             var tempMpartn = FoxRepo.GetTable<CrystalDS.mpartnRow>(filter: "WHERE BETWEEN(Datbr,?,?) AND Kpr==? AND Nop == ?", parameters: new[] { date1, date2, kpr, nop });
             var tempAmpartn = FoxRepo.GetTable<CrystalDS.mpartnRow>(isArchived: true, filter: "WHERE BETWEEN(Datbr,?,?) AND Kpr == ? AND Nop == ?", parameters: new[] { date1, date2, kpr, nop });

             var result = from table in tempAmpartn.Concat(tempMpartn)
                          join asbrak in FoxRepo.GetTable<CrystalDS.asbrakRow>()
                              on table.kprb equals asbrak.kprb
                          join astob in FoxRepo.GetTable<CrystalDS.astobRow>()
                          on new { table.kus, table.kgt } equals new { astob.kus, astob.kgt }
                          group table by new { table.nprt, asbrak.naprb, astob.snaobor, table.datbr } into gResult
                          select new MonthReportDataBrak
                          {
                              Nprt = gResult.Key.nprt,
                              Nzp = gResult.Count().ToString(),
                              Naprb = gResult.Key.naprb,
                              Npls = gResult.Select(x => x.npls.Substring(11, 2)).ToArray(),
                              Ust = gResult.Key.snaobor,
                              Date = gResult.Key.datbr.ToShortDateString(),
                          };
            return result;

        }


    }
}
