using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching.Configuration;
using Crystal.DomainModel;
using Crystal.BusinessLogic;
using CrystalDataSetLib;
using System.Web.Mvc;
using System.Runtime.Caching;
namespace Crystal.VFPOleDBLogic
{
    public partial class BusinessLogicOleDb : ICrystalLogic
    {
        private VFPOleDbRepo   FoxRepo;
        private MemoryCache memory = MemoryCache.Default;
        
        public BusinessLogicOleDb(Plants plant)
        {
            FoxRepo = new VFPOleDbRepo() { RepoPlant = plant };
        }


        private class Prod2Data
        {
            public decimal Nzp { get; set; }
            public decimal NzpCurrent { get; set; }
            public decimal Defects { get; set; }
        }


        public BatchInfo PartInfo(string NPRT)
        {
            
            IEnumerable<CrystalDS.mcomplRow> tMcompl;
            List<CrystalDS.mprxopRow> tMprxop;
            IEnumerable<CrystalDS.mpartnRow> tMpartn;

            bool archived, isOut = false;

            //var tMpartt = FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "WHERE nprt == ?", parameters: new[] { NPRT });
            var batch =
                FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "WHERE nprt == ?", parameters: new[] { NPRT })
                    .FirstOrDefault();

            if (batch != null)
            {
                archived = false;
                tMcompl = FoxRepo.GetTable<CrystalDS.mcomplRow>(filter: "WHERE nprt == ?", parameters: new[] { NPRT });
                tMprxop = FoxRepo.GetTable<CrystalDS.mprxopRow>(filter: "WHERE nprt == ? ORDER BY dath, nop",
                    parameters: new[] { NPRT }).ToList();
                var nr = new CrystalDS.mprxopDataTable().NewmprxopRow();
                {
                    nr.nprt = batch.nprt;
                    nr.nop = batch.nop;
                    nr.kmk = batch.kmk;
                    nr.dato = batch.dath.isEmptyDate() ? batch.dath : batch.dato;
                    nr.dath = batch.dath;
                    nr.timo = batch.timo;
                    nr.timh = batch.timh;
                    nr.kgt = batch.kgt;
                    nr.kus = batch.kus;
                    nr.kpls = batch.kpls;
                    nr.owner = batch.owner;

                }

                //nr.nprt =
                isOut = ("0000" == nr.nop);

                if (!isOut)
                    tMprxop.Add(nr);

                tMpartn = FoxRepo.GetTable<CrystalDS.mpartnRow>(filter: "WHERE nprt == ?",
                    parameters: new[] { NPRT });
            }
            else
            {
                batch =
                    FoxRepo.GetTable<CrystalDS.mparttRow>(isArchived: true, filter: "WHERE nprt == ?",
                        parameters: new[] { NPRT }).FirstOrDefault();
                if (batch == null)
                {
                    throw new System.Data.ObjectNotFoundException("не найдена партия");
                }
                archived = true;
                isOut = true;
                tMcompl = FoxRepo.GetTable<CrystalDS.mcomplRow>(isArchived: true,
                    filter: "WHERE nprt == ?", parameters: new[] { NPRT });
                tMprxop = FoxRepo.GetTable<CrystalDS.mprxopRow>(isArchived: true,
                    filter: "WHERE nprt == ? ORDER BY dath, nop", parameters: new[] { NPRT }).ToList();
                tMpartn = FoxRepo.GetTable<CrystalDS.mpartnRow>(isArchived: true,
                    filter: "WHERE nprt == ?", parameters: new[] { NPRT });
            }

            var napr =
                FoxRepo.GetTable<CrystalDS.assprRow>()
                    .Where(a => a.kpr == batch.kpr)
                    .Select(b => b.napr)
                    .FirstOrDefault();
            var tempAsabon = FoxRepo.GetTable<CrystalDS.asabonRow>();
            DateTime? prevDate = DateTime.MinValue; //FoxProExtensions.FoxNullDate;
            var tastob = FoxRepo.GetTable<CrystalDS.astobRow>();
            var tassop = FoxRepo.GetTable<CrystalDS.assopRow>();
            var tAstmr = FoxRepo.GetTable<CrystalDS.astmrRow>();
            var tasbrak = FoxRepo.GetTable<CrystalDS.asbrakRow>();
            var result = (from part in tMprxop
                          join astmr in tAstmr
                              on new { part.kmk, part.nop } equals new { astmr.kmk, astmr.nop }
                          join mpartn in tMpartn
                              on new { nop = part.nop,dato=part.dato ,timo = part.timo } equals new { nop = mpartn.nop ,dato= mpartn.datbr , timo = mpartn.timbr} into brak
                          let brakInfo = (from brk in brak
                                          join asbrak in tasbrak
                                              on brk.kprb equals asbrak.kprb
                                              
                                          select new BrakInfo
                                          {
                                              Naprb = asbrak.naprb,
                                              Npls = brk.npls,
                                              Datbr = brk.datbr,
                                              Timbr = brk.timbr,
                                              Fio = FoxRepo.GetFioByTabNumber(part.owner, tempAsabon)
                                          }).ToArray()

                          let complData =
                              (from mcompl in
                                   tMcompl.Where(
                                       a =>
                                           a.nop == part.nop &&
                                           //((prevDate.Equals(prevDate)) ||
                                            FoxProExtensions.FullDateTime(a.datr, a.timr) >= prevDate &&
                                           (part.dato.isEmptyDate() ||
                                            FoxProExtensions.FullDateTime(a.datr, a.timr) <=
                                            FoxProExtensions.FullDateTime(part.dato, part.timo)))
                               select new ComplData
                               {
                                   Kpls = Convert.ToInt32(mcompl.kpls),
                                   Nprtr = mcompl.nprtr,
                                   Kplr = Convert.ToInt32(mcompl.kplr),
                                   Pcompl = mcompl.pcompl,
                                   Datr = FoxProExtensions.FullDateTime(mcompl.datr, mcompl.timr),
                                   Fio = mcompl.fio,
                               }).ToArray()

                          select new PartRouteData
                          {
                              Id = Guid.NewGuid(),
                              Ex = null,
                              Nop = part.nop,
                              Kop = astmr.kop,
                              Naop = tassop.Where(a => a.kop == astmr.kop).Select(b => b.naop).FirstOrDefault(),
                              SNaobor =
                                  tastob.Where(a => a.kgt == part.kgt && a.kus == part.kus)
                                      .Select(b => b.snaobor)
                                      .FirstOrDefault(),
                              Dath = FoxProExtensions.FullDateTime(part.dath, part.timh),
                              Dato = FoxProExtensions.FullDateTime(part.dato, part.timo),
                              Kpls = Convert.ToInt32(part.kpls),
                              BrakInfo = brakInfo,
                              Fio = FoxRepo.GetFioByTabNumber(part.owner, tempAsabon),
                              ComplData = complData.Where(a =>
                              {
                                  prevDate = FoxProExtensions.FullDateTime(part.dato, part.timo);
                                  return true;
                              }).ToArray(),
                              SumKplsR = complData.Where(a => a.Pcompl == "R").Sum(b => b.Kplr),
                              SumKplsD = complData.Where(a => a.Pcompl == "D").Sum(b => b.Kplr),
                          }).ToArray();
            var LastDate = result.LastOrDefault().Dath ?? result.LastOrDefault().Dato;
            var FirstDate = result.FirstOrDefault().Dath ?? result.FirstOrDefault().Dato;

            var DateEnd = isOut ? LastDate : DateTime.Now;
            var tonr = FoxRepo.WorkDaysBetween(FirstDate, DateEnd,
                FoxRepo.GetTable<CrystalDS.askrmRow>());


            var res = new BatchInfo
            {
                Napr = napr,
                Nprt = NPRT,
                RouteData = result,
                DaysInRoute = tonr,
                Archived = archived
            };

            return res;
        }

        public TransistorViewModel Production()
        {

            var vModel = new TransistorViewModel();

            DateTime d1 = GetMonthStartDate().Date, d2 = DateTime.Now;

            var t_asmnp = FoxRepo.GetTable<CrystalDS.asmnpRow>();

            decimal nzpoNM = 0, sform = 0, nzpoALL = 0, brak = 0;
            nzpoNM = t_asmnp.Sum(a => a.nzpo);
            sform = t_asmnp.Where(a => a.nop == "0001").Sum(a => a.pso3);
            nzpoALL = t_asmnp.Sum(a => a.pso3 + a.nzpo - a.sdo3 - a.brk3);
            brak = t_asmnp.Sum(a => a.brk3);


            var t_astmr = FoxRepo.GetTable<CrystalDS.astmrRow>();
            var t_assop = FoxRepo.GetTable<CrystalDS.assopRow>();

            vModel.PlantNumber = GetProductionNumber();



            Prod2Data z2_data;
            if (vModel.ShowProd2Data)
            {
                z2_data = (from asmpn in t_asmnp
                           join astmr in t_astmr on new { asmpn.nop, asmpn.kmk } equals new { astmr.nop, astmr.kmk }
                           join assop in t_assop on astmr.kop equals assop.kop
                           where assop.pfl == "5"
                           group asmpn by new { assop.pfl }
                               into gg
                               select
                                   new Prod2Data
                                   {
                                       Nzp = gg.Sum(a => a.nzpo),
                                       NzpCurrent =
                                           gg.Sum(a => a.nzpo) + gg.Sum(a => a.pso3) - gg.Sum(a => a.sdo3) - gg.Sum(a => a.brk3),
                                       Defects = gg.Sum(a => a.brk3)
                                   }).FirstOrDefault();
            }
            else
            {
                z2_data = null;
            }

            decimal zapr1MparttSum = FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "WHERE nop=='0000'").Sum(a => a.kpls);
            decimal zapr1AMparttSum =
                FoxRepo.GetTable<CrystalDS.mparttRow>(isArchived: true, filter: "WHERE BETWEEN(dato,?,?)",
                    parameters: new[] { d1 as object, d2 as object }).Sum(a => a.kpls);

            var dd = new List<AllNzp>
            {
                new AllNzp
                {
                    Nzp = (int) (nzpoNM - (z2_data != null ? z2_data.Nzp : 0)),
                    NzpPr2 = (int) (z2_data != null ? z2_data.Nzp : 0),
                    Sform = (int) sform,
                    NowNzp = (int) (nzpoALL - (z2_data != null ? z2_data.NzpCurrent : 0)),
                    NowNzpPr2 = (int) (z2_data != null ? z2_data.NzpCurrent : 0),
                    KplsPr2 = (int) (z2_data != null ? z2_data.Defects : 0),
                    Kpls = (int) (brak - (z2_data != null ? z2_data.Defects : 0)),
                    Sdo = (int) (zapr1MparttSum + zapr1AMparttSum),
                    KplsForDay = (int) t_asmnp.Sum(a => a.sdo2),
                    KplsForMount = (int) t_asmnp.Sum(a => a.sdo3)
                }
            };

            //var dd = new List<AllNzp>
            //    {
            //        new AllNzp
            //            {
            //                Nzp = 0,
            //                NzpPr2 = 0,
            //                Sform = 0,
            //                NowNzp = 0,
            //                NowNzpPr2 = 0,
            //                KplsPr2 = 0,
            //                Kpls = 0,
            //                Sdo = 0,
            //                KplsForDay=0,
            //                KplsForMount=0 
            //            }
            //    };

            vModel.AllNzps = dd.ToArray();

            //--------------------------------------------------------------------------------------------------------------------------------------//
            var t_mpartt1 = FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "Where !EMPTY(dath)");
            var t_mpartt2 = FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "Where !EMPTY(dato)");
            var t_astob = FoxRepo.GetTable<CrystalDS.astobRow>();
            var t_askor = FoxRepo.GetTable<CrystalDS.askorRow>();

            //var t_astmr = FoxRepo.GetTable<CrystalDS.astmrRow>();
            var t_as_wop = FoxRepo.GetTable<CrystalDS.as_wopRow>();

            var t_assufl = FoxRepo.GetTable<CrystalDS.assuflRow>();

            // посчитаем партии готовые к запуску

            var lnch_ready = (from mpartt in t_mpartt2
                              join astob in t_astob
                                  on mpartt.kgt equals astob.kgt
                              join astmr in t_astmr
                                  on new { kmk = mpartt.kmk, nop = mpartt.nop } equals new { kmk = astmr.kmk, nop = astmr.nop }
                              join as_wop in t_as_wop
                                  on new { kgt = mpartt.kgt, kus = astob.kus, kop = astmr.kop } equals
                                  new { kgt = as_wop.kgt, kus = as_wop.kus, kop = as_wop.kop }
                              let nuf = t_assufl.SingleOrDefault(a => a.kgt == as_wop.kgt && a.kus == as_wop.kus)
                              //FoxRepo.GetTableNew<CrystalDS.assuflDataTable>(filter: "WHERE kgt == ? AND kus == ? ", parameters: new[] { kgt, kus }).SingleOrDefault()
                              where ((nuf == null) || (nuf.nuf == mpartt.nprt.Substring(0, 2)))
                              select new
                              {
                                  part = mpartt,
                                  eq = astob
                              }).ToList();


            vModel.ModuleMap = new ProductionMap
            {
                MalfunctionCount = t_astob.Where(a => a.ks == "r").Count(),
                corridors = (from askor in t_askor
                             join astob in t_astob
                                 on askor.kkor equals astob.kkor into g_astob
                             orderby int.Parse(askor.kkor)
                             select new ProductionMap.Corridor
                             {
                                 Code = askor.kkor,
                                 Name = askor.nakor,
                                 equipments =
                                     (from ast in g_astob
                                      join mpartt in t_mpartt1
                                          on new { kgt = ast.kgt, kus = ast.kus } equals
                                          new { kgt = mpartt.kgt, kus = mpartt.kus } into j_mpartt
                                      join mpartt2 in lnch_ready
                                          on new { kgt = ast.kgt, kus = ast.kus } equals
                                          new { kgt = mpartt2.eq.kgt, kus = mpartt2.eq.kus } into j_mpartt2
                                      select new Equipment
                                      {
                                          Code = ast.kus,
                                          TechGroupCode = ast.kgt,
                                          ShortName = ast.snaobor.Trim(),
                                          StateCode = ast.ks,
                                          SumInfo = new Equipment.BatchesInfo
                                          {
                                              Nzp = (int)(j_mpartt.Count() > 0 ? j_mpartt.Sum(a => a.kpls) : 0),
                                              Wait = (int)(j_mpartt2.Count() > 0 ? j_mpartt2.Sum(a => a.part.kpls) : 0)
                                          }

                                      }).ToArray()
                             }

                    ).ToArray()
            };
            return vModel;

        }

        public string GetProductionNumber()
        {
            if (FoxRepo.RepoPlant == Plants.plant_none)
            {
                return "--";
            }
            else
            {
                return FoxRepo.GetTable<CrystalDS.asparamRow>().Single(x => x.identp.Trim() == "FACTORY").valp.Trim();
            }
        }

        private string Get_StartMs()
        {
            return
                FoxRepo.GetTable<CrystalDS.asparamRow>(filter: "WHERE identp == ?", parameters: new[] { "start_ms" })
                    .Single()
                    .valp.Trim();
        }

        public DateTime GetMonthStartDate()
        {
            //string monthStartDate = Get_StartMs();
            return DateTime.ParseExact(Get_StartMs(), "dd/MM/yyyy HHmm",
                System.Globalization.CultureInfo.InvariantCulture);
        }

        public int WorkDaysBetween(DateTime start_date, DateTime end_date, IEnumerable<CrystalDS.askrmRow> calend)
        {
            return calend.Count(a => a.dm >= start_date.Date && a.dm <= end_date.Date);
        }

        public int WorkDaysInMonth(DateTime dt)
        {
            DateTime date_e = new DateTime(year: dt.Year, month: dt.Month, day: DateTime.DaysInMonth(dt.Year, dt.Month));
            DateTime date_s = new DateTime(year: dt.Year, month: dt.Month, day: 1);
            return WorkDaysBetween(date_s, date_e, FoxRepo.GetTable<CrystalDS.askrmRow>());
            //return FoxRepo.GetTable<CrystalDS.askrmRow>().Count(a => a.dm > date_s && a.dm < date_e);
        }

        //public double HoursBetweenForMZagr(DateTime dt_e, string time_e, DateTime dt_s, string time_s)//dt_s,time_s-начало,dt_e,time_e-конец;
        //{
        //    DateTime date_e;
        //    DateTime date_s = DateTime.Now;
        //    if (dt_e.isEmptyDate())
        //    {
        //        date_e = DateTime.Now;
        //    }
        //    else
        //    {
        //        date_e = new DateTime(year: dt_e.Year, month: dt_e.Month, day: dt_e.Day, hour: int.Parse(time_e.Substring(0, 2)), minute: int.Parse(time_e.Substring(2, 2)), second: 0);
        //    }

        //    if (date_e.Month != dt_s.Month)
        //        date_s = new DateTime(year: date_e.Year, month: date_e.Month, day: date_e.Day, hour: 0, minute: 0, second: 0);
        //    else
        //        date_s = new DateTime(year: dt_s.Year, month: dt_s.Month, day: dt_s.Day, hour: int.Parse(time_s.Substring(0, 2)), minute: int.Parse(time_s.Substring(2, 2)), second: 0);

        //    return date_e.Subtract(date_s).TotalHours;
        //}
        

        public ProductionInfoModel GetProductionSummary()
        {
            var t_mpartt = FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "where nop != '0000'");
            var res = new ProductionInfoModel
            {
                BatchCount = t_mpartt.Count(),
                PlastCount = Convert.ToInt32(t_mpartt.Sum(a => a.kpls))
            };
            return res;

        }

        public IEnumerable<PartSearch> PrtSearch(string query)
        {

            var result =
                         from mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>()
                         where mpartt.nprt == query
                         join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
                         on new { mpartt.kmk, mpartt.nop } equals new { astmr.kmk, astmr.nop }
                         join assop in FoxRepo.GetTable<CrystalDS.assopRow>()
                         on astmr.kop equals assop.kop
                         join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                         on new { mpartt.kmk, mpartt.kpr } equals new { asspr.kmk, asspr.kpr }
                         join as_wop in FoxRepo.GetTable<CrystalDS.as_wopRow>()
                         on new { astmr.kop, mpartt.kgt } equals new { as_wop.kop, as_wop.kgt }
                         join astob in FoxRepo.GetTable<CrystalDS.astobRow>()
                         on new { as_wop.kgt, as_wop.kus } equals new { astob.kgt, astob.kus }
                         join askor in FoxRepo.GetTable<CrystalDS.askorRow>()
                         on astob.kkor equals askor.kkor
                         group mpartt by new { mpartt.nprt, mpartt.nop, assop.naop, mpartt.kpr, mpartt.kpls, asspr.napr, astmr.kop, mpartt.timo, mpartt.dato, mpartt.dath, mpartt.timh, askor.nakor, astob.snaobor } into g_mpartt
                         select new PartSearch
                         {
                             Nprt = g_mpartt.Key.nprt,
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

            return result;
        }
        public static double HoursBetweenForMZagr(DateTime dt_e, string time_e, DateTime dt_s, string time_s)//dt_s,time_s-начало,dt_e,time_e-конец;
        {
            DateTime date_e;
            DateTime date_s = DateTime.Now;
            if (dt_e.isEmptyDate())
            {
                date_e = DateTime.Now;
            }
            else
            {
                date_e = new DateTime(year: dt_e.Year, month: dt_e.Month, day: dt_e.Day, hour: int.Parse(time_e.Substring(0, 2)), minute: int.Parse(time_e.Substring(2, 2)), second: 0);
            }

            if (date_e.Month != dt_s.Month)
                date_s = new DateTime(year: date_e.Year, month: date_e.Month, day: date_e.Day, hour: 0, minute: 0, second: 0);
            else
                date_s = new DateTime(year: dt_s.Year, month: dt_s.Month, day: dt_s.Day, hour: int.Parse(time_s.Substring(0, 2)), minute: int.Parse(time_s.Substring(2, 2)), second: 0);

            return date_e.Subtract(date_s).TotalHours;
        }

        
    }

}
