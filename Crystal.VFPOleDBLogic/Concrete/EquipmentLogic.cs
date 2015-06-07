using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crystal.BusinessLogic.Abstract;
using Crystal.DomainModel;
using CrystalDataSetLib;

namespace Crystal.VFPOleDBLogic
{
    public partial class BusinessLogicOleDb : IEquipmentLogic
    {
        public IEnumerable<EquipmentLoad> GetEquipmentLoads(DateTime startDate, DateTime endDate)
        {
            var t_askrm = FoxRepo.GetTable<CrystalDS.askrmRow>();
            int hrsMonth = WorkDaysBetween(startDate, endDate, t_askrm) * 24;
            
            endDate = endDate.AddDays(1).AddMilliseconds(-1);
            
            object date1 = startDate, date2 = endDate;
            var tempMprxop = FoxRepo.GetTable<CrystalDS.mprxopRow>(filter: "WHERE BETWEEN(dath,?,?)",
                                                                   parameters: new[] {date1, date2}).
                                     Concat(FoxRepo.GetTable<CrystalDS.mprxopRow>(isArchived: true,
                                                                                  filter: "WHERE BETWEEN(dath,?,?)",
                                                                                  parameters: new[] {date1, date2}));
            var tempAsmbn = FoxRepo.GetTable<CrystalDS.asmbnRow>(
                filter: "WHERE BETWEEN(Dnp,?,?) OR ( EMPTY(Dop) AND (Dnp <= ?) ) OR ( (Dnp<=?) AND (Dop>=?) )",
                parameters: new[] {date1, date2, date2, date1, date1}).
                                    Concat(FoxRepo.GetTable<CrystalDS.asmbnRow>(isArchived: true,
                                                                                filter:
                                                                                    "WHERE BETWEEN(Dnp,?,?) OR ( EMPTY(Dop) AND (Dnp <= ?) ) OR ( (Dnp<=?) AND (Dop>=?) )",
                                                                                parameters:
                                                                                    new[] {
                                                                                            date1, date2, date2, date1,
                                                                                            date1
                                                                                        }));
                //.Where(a=> a.kgt == "18" && a.kus == "11");

            var tempMpartN = FoxRepo.GetTable<CrystalDS.mpartnRow>(filter: "WHERE BETWEEN(datbr,?,?)",
                                                                   parameters: new[] {date1, date2}).
                                     Concat(FoxRepo.GetTable<CrystalDS.mpartnRow>(isArchived: true,
                                                                                  filter: "WHERE BETWEEN(datbr,?,?)",
                                                                                  parameters: new[] {date1, date2}));


            var tempAskprob = FoxRepo.GetTable<CrystalDS.askprobRow>();

            

            var r1 = (from astob in FoxRepo.GetTable<CrystalDS.astobRow>()
                      join askuch in FoxRepo.GetTable<CrystalDS.askuchRow>()
                          on astob.kuch equals askuch.kuch
                      join mprxop in tempMprxop
                          on new {astob.kgt, astob.kus} equals new {mprxop.kgt, mprxop.kus} into proc_parts
                      join mpartn in tempMpartN
                          on new {astob.kgt, astob.kus} equals new {mpartn.kgt, mpartn.kus} into proc_brak
                      join asmbn in tempAsmbn
                          on new {astob.kgt, astob.kus} equals new {asmbn.kgt, asmbn.kus} into eq_repairs
                      let repairs = (from rep in eq_repairs
                                     group rep by rep.kpp
                                     into g_reps
                                     let reason = tempAskprob.SingleOrDefault(a => a.kpp == g_reps.Key)

                                     select new EquipmentLoad.RepairReason
                                         {
                                             ReasonCode = g_reps.Key,
                                             ReasonName = reason == null ? "" : reason.napr,
                                             RepairCount = g_reps.Count(),
                                             DurationH = g_reps.Sum(a => (CalcDowntime(a, t_askrm, startDate, endDate)).TotalHours)
                                                 //{
                                                 //    var startDowntime = FoxProExtensions.FullDateTime(a.dnp, a.tnp);
                                                 //    var endDowntime = FoxProExtensions.FullDateTime(a.dop, a.top) ?? endDate;
                                                 //    return
                                                 //        (endDowntime > endDate ? endDate : endDowntime).Subtract(
                                                 //            startDowntime < startDate ? startDate : startDowntime.Value)
                                                 //                                                       .TotalHours;
                                                 //})
                                         }).ToArray()
                      let rep_duration = repairs.Sum(a => a.DurationH)
                      select new EquipmentLoad
                          {
                              EquipmentUniqCode = astob.kgt + astob.kus,
                              AreaName = askuch.nauch,
                              EquipmentName = astob.snaobor,
                              PlateCount = Convert.ToInt32(proc_parts.Sum(a => a.kpls)),
                              BrkCount = proc_brak.Count(),
                              BatchCount = proc_parts.Count(),
                              Repairs = repairs,
                              RepairDurationH = rep_duration,
                              EquipmentEfficiency = (hrsMonth - rep_duration)/hrsMonth
                          }).OrderBy(x => x.AreaName);
            return r1;
            //var reas = from askprob in FoxRepo.GetTable<CrystalDS.askprobRow>()
            //           join asmbn in tempAsmbn
            //           on askprob.kpp equals asmbn.kpp
            //           group asmbn by new { askprob.napr } into zapr
            //           let reps = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp))
            //           select new Reasons
            //           {
            //               Napr = zapr.Key.napr,
            //               SumN = zapr.Select(a => a.kpp).Count().ToString(),
            //               Hrs = zapr.Sum(a => FoxRepo.HoursBetweenForMZagr(a.dop, a.top, a.dnp, a.tnp)).ToString("F2"),
            //               KrObr = ((hrsMonth - reps) / hrsMonth).ToString("F3"),
            //           };
            //ViewBag.reas = ((IEnumerable<LoadMth>)ViewBag.Result).Average(a => decimal.Parse(a.KrObr)).ToString("F3");
            //ViewBag.Result2 = reas.OrderByDescending(a => a.Hrs);
        }

        public EquipmentDetailModel GetEquipmentDetail(string techGroupCode, string equipmentCode)
        {
            var equipment =
                FoxRepo.GetTable<CrystalDS.astobRow>(filter: "WHERE astob.kgt == ? AND astob.kus == ?",
                                                     parameters: new[] {techGroupCode, equipmentCode}).Single();
            //DeatilsModel model = new DeatilsModel { Kgt = KGT, Kus = KUS };
            // var eq = new Equipment { kgt = KGT, kus = KUS };

            var model = new EquipmentDetailModel
                {
                    TechGroupCode = techGroupCode,
                    Code = equipmentCode,
                    ShortName = equipment.snaobor,
                    StateCode = equipment.ks
                };

            var t =
                FoxRepo.GetTable<CrystalDS.assuflRow>(filter: "WHERE kgt == ? AND kus == ? ",
                                                      parameters: new[] {techGroupCode, equipmentCode})
                       .SingleOrDefault();
            if (t == null)
                model.PhotoLitCode = String.Empty;
            else
                model.PhotoLitCode = t.nuf;


            var t_asabon = FoxRepo.GetTable<CrystalDS.asabonRow>();

            var batchesOnEquipment =
                FoxRepo.GetTable<CrystalDS.mparttRow>(
                    filter: "WHERE mpartt.kgt == ? AND mpartt.kus == ? AND !EMPTY(mpartt.dath) ",
                    parameters: new[] {techGroupCode, equipmentCode});
            model.BatchesOnOperation = (from part in batchesOnEquipment
                                        join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                                            on part.kpr equals asspr.kpr
                                        join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
                                            on new {part.kmk, part.nop} equals new {astmr.kmk, astmr.nop}
                                        join assop in FoxRepo.GetTable<CrystalDS.assopRow>()
                                            on astmr.kop equals assop.kop
                                        select new BatchOnOperation
                                            {
                                                DeviceCode = part.kpr,
                                                DeviceName = asspr.napr,
                                                BatchNumber = part.nprt,
                                                OperationNumber = part.nop,
                                                OperationName = assop.naop,
                                                LaunchDate = FoxProExtensions.FullDateTime(part.dath, part.timh).Value,
                                                PlateCount = Convert.ToInt32(part.kpls),
                                                OperatorFullName = FoxRepo.GetFioByTabNumber(part.owner, t_asabon)
                                            }).OrderBy(x => x.DeviceCode).ToArray();

            var batchesForLaunch =
                (from mpartt in
                     FoxRepo.GetTable<CrystalDS.mparttRow>(filter: "WHERE mpartt.kgt == ? AND !EMPTY(mpartt.dato) ",
                                                           parameters: new[] {techGroupCode})
                 join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
                     on new {kmk = mpartt.kmk, nop = mpartt.nop} equals new {kmk = astmr.kmk, nop = astmr.nop}
                 join as_wop in
                     FoxRepo.GetTable<CrystalDS.as_wopRow>(filter: "WHERE as_wop.kus == ?",
                                                           parameters: new[] {equipmentCode})
                     on new {kgt = mpartt.kgt, kop = astmr.kop} equals new {kgt = as_wop.kgt, kop = as_wop.kop}
                 where ((model.PhotoLitCode.Length == 0) || (model.PhotoLitCode == mpartt.nprt.Substring(0, 2)))
                 select mpartt);

            model.BatchesForLaunch = (from part in batchesForLaunch
                                      join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                                          on part.kpr equals asspr.kpr
                                      join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
                                          on new {part.kmk, part.nop} equals new {astmr.kmk, astmr.nop}
                                      join assop in FoxRepo.GetTable<CrystalDS.assopRow>()
                                          on astmr.kop equals assop.kop
                                      select new BatchForLaunch
                                          {
                                              DeviceCode = part.kpr,
                                              DeviceName = asspr.napr,
                                              BatchNumber = part.nprt,
                                              ArriveDate = FoxProExtensions.FullDateTime(part.dato, part.timo).Value,
                                              OperationNumber = part.nop,
                                              OperationName = assop.naop,
                                              PlateCount = Convert.ToInt32(part.kpls)
                                          }).OrderBy(x => x.DeviceCode).ToArray();
            //ViewBag.prodN = bl.GetProductionNumber();
            //ViewBag.userId = Crystal.Authentication.AuthService.GetUserKeyHashCode(User);
            //return View(model);
            return model;
        }

        public IEnumerable<string> SearchRes(string query)
        {
            var result =
                FoxRepo.GetTable<CrystalDS.mparttRow>(filter: string.Format("WHERE nprt LIKE \"{0}\" ", query + "%"))
                       .Select(a => a.nprt)
                       .OrderBy(b => b.Substring(0, 5));

            return result;
        }

        public IEnumerable<Downtimes> Downtimes(string KGT, string KUS)
        {
            var t_asabon = FoxRepo.GetTable<CrystalDS.asabonRow>();
            DateTime now = DateTime.Now;
            //var eq = new Equipment { kgt = KGT, kus = KUS };

            var equipment =
                FoxRepo.GetTable<CrystalDS.astobRow>(filter: "WHERE astob.kgt == ? AND astob.kus == ?",
                                                     parameters: new[] {KGT, KUS}).Single();
            //DeatilsModel model = new DeatilsModel { Kgt = KGT, Kus = KUS };
            // var eq = new Equipment { kgt = KGT, kus = KUS };

            //var model = new EquipmentDetailModel
            //{
            //    TechGroupCode = KGT,
            //    Code = KUS,
            //    ShortName = equipment.snaobor,
            //    StateCode = equipment.ks
            //};

            var downtimes = FoxRepo.GetTable<CrystalDS.asmbnRow>(filter: "WHERE asmbn.kgt == ? AND asmbn.kus == ?",
                                                                 parameters: new[] {KGT, KUS});
            var t_askrm = FoxRepo.GetTable<CrystalDS.askrmRow>();
            
            var result = (from asmbn in downtimes
                          join askprob in FoxRepo.GetTable<CrystalDS.askprobRow>()
                              on asmbn.kpp equals askprob.kpp into svar
                          from myvar in svar.DefaultIfEmpty()
                          let Dop = FoxProExtensions.FullDateTime(asmbn.dop, asmbn.top) ?? DateTime.Now
                          let Dnp = FoxProExtensions.FullDateTime(asmbn.dnp, asmbn.tnp).Value
                          select new Downtimes
                              {
                                  Nobr = equipment.snaobor,
                                  Dnp = FoxProExtensions.FullDateTime(asmbn.dnp, asmbn.tnp),
                                  Dop = FoxProExtensions.FullDateTime(asmbn.dop, asmbn.top),
                                  Napr = myvar == null ? "" : myvar.napr,
                                  Iopn = FoxRepo.GetFioByTabNumber(asmbn.iopn, t_asabon),
                                  Iopo = FoxRepo.GetFioByTabNumber(asmbn.iopo, t_asabon),
                                  Hrs =
                                    CalcDowntime(asmbn, t_askrm, Dnp, Dop).TotalHours.ToString("F2")
                              });
            //}).Where(a => a.Dop.Trim() == "" || DateTime.Parse(a.Dop).Month == now.Month);
            return result;
        }


        public IEnumerable<Approaching> GetPrib(string techGroupCode, string equipmentCode, int Depth)
        {
            var equipment =
                FoxRepo.GetTable<CrystalDS.astobRow>(filter: "WHERE astob.kgt == ? AND astob.kus == ?",
                                                     parameters: new[] {techGroupCode, equipmentCode}).Single();
            //DeatilsModel model = new DeatilsModel { Kgt = KGT, Kus = KUS };
            // var eq = new Equipment { kgt = KGT, kus = KUS };

            var model = new EquipmentDetailModel
                {
                    TechGroupCode = techGroupCode,
                    Code = equipmentCode,
                    ShortName = equipment.snaobor,
                    StateCode = equipment.ks
                };


            var t =
                FoxRepo.GetTable<CrystalDS.assuflRow>(filter: "WHERE kgt == ? AND kus == ? ",
                                                      parameters: new[] {techGroupCode, equipmentCode})
                       .SingleOrDefault();
            if (t == null)
                model.PhotoLitCode = String.Empty;
            else
                model.PhotoLitCode = t.nuf;



            var t_astmr = FoxRepo.GetTable<CrystalDS.astmrRow>();

            var res = from mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>()
                      join astmr in t_astmr
                          on mpartt.kmk equals astmr.kmk
                      join as_wop in
                          FoxRepo.GetTable<CrystalDS.as_wopRow>(filter: "WHERE (as_wop.kgt == ? AND as_wop.kus == ?)",
                                                                parameters: new[] {techGroupCode, equipmentCode}
                          )
                          on astmr.kop equals as_wop.kop
                      where int.Parse(mpartt.nop) < int.Parse(astmr.nop)
                            && ((model.PhotoLitCode.Length == 0) || (model.PhotoLitCode == mpartt.nprt.Substring(0, 2)))
                      select new BatchNear
                          {
                              nprt = mpartt.nprt,
                              distance =
                                  t_astmr.Count(
                                      a =>
                                      a.kmk == mpartt.kmk && int.Parse(a.nop) > int.Parse(mpartt.nop) &&
                                      int.Parse(a.nop) <= int.Parse(astmr.nop)),
                              nop = astmr.nop,
                              kop = astmr.kop,
                              kpr = mpartt.kpr,
                              kmk = mpartt.kmk,
                              kpls = mpartt.kpls.ToString()
                          };

            var NearParts = (from batchNear in res
                             where batchNear.distance <= 10
                             select batchNear);


            //var eq = new Equipment { kgt = KGT, kus = KUS };
            var data =
                (from part in NearParts
                 join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                     on part.kpr equals asspr.kpr
                 join assop in FoxRepo.GetTable<CrystalDS.assopRow>()
                     on part.kop equals assop.kop

                 join astmrbatch in FoxRepo.GetTable<CrystalDS.astmrRow>()
                     on new {part.kmk, part.nop} equals new {astmrbatch.kmk, astmrbatch.nop}
                 join assopbatch in FoxRepo.GetTable<CrystalDS.assopRow>()
                     on astmrbatch.kop equals assopbatch.kop


                 select new Approaching
                     {
                         Kpr = part.kpr,
                         Napr = asspr.napr,
                         Nprt = part.nprt,
                         Kpls = part.kpls.ToString(),
                         From = assopbatch.naop,
                         To = assop.naop,
                         Distance = part.distance
                     }).OrderBy(x => x.Distance);

            if (Depth >= 0)
            {
                var data2 = data.Where(x => x.Distance <= Depth);
                return data2;
            }

            return data;
        }

        public IEnumerable<LdPlot> LdPlotByDate(string fdate, string sdate, string indnumb)
        {


            DateTime d1 = DateTime.Parse(fdate);
            DateTime d2 = DateTime.Parse(sdate);

            var query = (from dat in
                             FoxRepo.GetTable<CrystalDS.mprxopRow>(isArchived: true, filter: "where between(Dato,?,?)",
                                                                   parameters: new[] {d1 as object, d2 as object})
                                    .Concat(FoxRepo.GetTable<CrystalDS.mprxopRow>(filter: "where between(Dato,?,?)",
                                                                                  parameters:
                                                                                      new[] {d1 as object, d2 as object}))
                         join astob in FoxRepo.GetTable<CrystalDS.astobRow>()
                             on new {dat.kgt, dat.kus} equals new {astob.kgt, astob.kus}
                         group dat by new {astob.snaobor, astob.kgt}
                         into gRes
                         select
                             new LdPlot
                                 {
                                     Ust = gRes.Key.snaobor.Trim().Replace("\"", ""),
                                     Kpls = gRes.Sum(x => x.kpls),
                                     Kgt = gRes.Key.kgt
                                 }).OrderBy(x => int.Parse(x.Kgt));

            return query;


        }

        public IEnumerable<NzpUch> AreaNzp()
        {
            var _askuchRow = FoxRepo.GetTable<CrystalDS.askuchRow>();
            var result = (from astob in FoxRepo.GetTable<CrystalDS.astobRow>()
                          join as_wop in FoxRepo.GetTable<CrystalDS.as_wopRow>()
                              on new {astob.kgt, astob.kus} equals new {as_wop.kgt, as_wop.kus}
                          join askuchRow in _askuchRow
                              on astob.kuch equals askuchRow.kuch
                          join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
                              on as_wop.kop equals astmr.kop
                          join asmnp in FoxRepo.GetTable<CrystalDS.asmnpRow>()
                              on new {astmr.kmk, astmr.nop} equals new {asmnp.kmk, asmnp.nop}
                          group asmnp by new {astob.kuch, askuchRow.nauch}
                          into asnmp_data1
                          let asmnp_data = asnmp_data1.Distinct()
                          let Pso = asmnp_data.Sum(a => a.pso3)
                          let Sdo = asmnp_data.Sum(a => a.sdo3)
                          let Brk = asmnp_data.Sum(a => a.brk3)
                          let Kpls = Pso + asmnp_data.Sum(a => a.nzpo) - Sdo - Brk
                          select new NzpUch
                              {
                                  Kuch = asnmp_data1.Key.nauch,
                                  Pso = Pso.ToString(),
                                  Sdo = Sdo.ToString(),
                                  Brk = Brk.ToString(),
                                  Kpls = Kpls.ToString(),
                                  Percs = (Sdo + Brk) != 0 ? (100*Sdo/(Sdo + Brk)).ToString("F2") : "100"
                              });


            return result;
        }

        public IEnumerable<LoadMth> MZagrBrig(DateTime fdate, DateTime sdate)
        {
            object date1 = fdate, date2 = sdate;
            //memory.Set("Date1", fdate, null, null);
            //memory.Set("Date2", sdate, null, null);

            var t_askrm = FoxRepo.GetTable<CrystalDS.askrmRow>();
            int hrsMonth = WorkDaysBetween(fdate, sdate, t_askrm) * 24;
            
            
            //int hrsMonth = WorkDaysInMonth(fdate) * 24;

            var tempMprxop = FoxRepo.GetTable<CrystalDS.mprxopRow>(filter: "WHERE BETWEEN(dath,?,?)",
                                                                   parameters: new[] {date1, date2});
            var tempAsmbn = FoxRepo.GetTable<CrystalDS.asmbnRow>(filter: "WHERE BETWEEN(Dop,?,?)",
                                                                 parameters: new[] {date1, date2});

            var Result = from asob in FoxRepo.GetTable<CrystalDS.asob_nalRow>()
                         join astob in FoxRepo.GetTable<CrystalDS.astobRow>()
                             on new {asob.kgt, asob.kus} equals new {astob.kgt, astob.kus}
                         join assbrn in FoxRepo.GetTable<CrystalDS.assbrnRow>()
                             on new {asob.cex, asob.kbrg} equals new {assbrn.cex, assbrn.kbrg}
                         join mprxop in tempMprxop
                             on new {astob.kgt, astob.kus} equals new {mprxop.kgt, mprxop.kus} into proc_mp
                         join mpartn in FoxRepo.GetTable<CrystalDS.mpartnRow>()
                             on new {asob.kgt, asob.kus} equals new {mpartn.kgt, mpartn.kus} into proc_brak
                         join asmbn in tempAsmbn
                             on new {asob.kgt, asob.kus} equals new {asmbn.kgt, asmbn.kus} into eq_repairs
                         let reps =
                             eq_repairs.Sum(asmbn => HoursBetweenForMZagr(asmbn.dop, asmbn.top, asmbn.dnp, asmbn.tnp))
                         select new LoadMth
                             {
                                 Numb = asob.kbrg,
                                 Kuch = assbrn.nabrg,
                                 Nobr = astob.snaobor,
                                 Kpls = proc_mp.Sum(a => a.kpls).ToString(),
                                 // Brk = proc_brak.Count().ToString(),
                                 //   Nprt = proc_parts.Count().ToString(),
                                 RepH = reps.ToString("F2"),
                                 KrObr = (hrsMonth - reps)/hrsMonth
                             };

         

            return Result;

        }

        public IEnumerable<StandEquipment> StandEquipment()
        {
            int hrsMonth = WorkDaysInMonth(DateTime.Now)*24;
            DateTime now = DateTime.Now;
            DateTime startmount = GetMonthStartDate();
            var t_asabon = FoxRepo.GetTable<CrystalDS.asabonRow>();
            var t_askrm = FoxRepo.GetTable<CrystalDS.askrmRow>(); 
            var resulthours = (from astob in FoxRepo.GetTable<CrystalDS.astobRow>(filter: "WHERE KS='r'")
                               join asmbn in
                                   FoxRepo.GetTable<CrystalDS.asmbnRow>()
                                          .Where(a => (FoxProExtensions.FullDateTime(a.dor, a.tor) ?? DateTime.Now) > startmount)
                                   on new {astob.kgt, astob.kus} equals new {asmbn.kgt, asmbn.kus}

                               select new StandTemp
                                   {
                                       kgt = asmbn.kgt,
                                       kus = asmbn.kus,
                                       kpp = asmbn.kpp,
                                       hours =(FoxProExtensions.FullDateTime(asmbn.dor, asmbn.tor) ?? DateTime.Now).Subtract((FoxProExtensions.FullDateTime(asmbn.dnp, asmbn.tnp).Value>startmount)?FoxProExtensions.FullDateTime(asmbn.dnp, asmbn.tnp).Value:startmount).TotalHours
                                        
                                       // HoursBetweenForStandEquipment(asmbn.dnp, asmbn.tnp, asmbn.dor,
                                             //                                    asmbn.tor, startmount),

                                   });


            var resultmount = from hours in resulthours
                              group hours by new {hours.kgt, hours.kus}
                              into zabr1
                              select new
                                  {
                                      kgt = zabr1.Key.kgt,
                                      kus = zabr1.Key.kus,
                                      totalhours = zabr1.Sum(a => a.hours),
                                      count = zabr1.Count(),
                                      KrObr = ((hrsMonth - zabr1.Sum(a => a.hours))/hrsMonth).ToString("F3")
                                  };

            var result = (from astob in FoxRepo.GetTable<CrystalDS.astobRow>(filter: "WHERE KS='r'")
                          join askor in FoxRepo.GetTable<CrystalDS.askorRow>()
                              on astob.kkor equals askor.kkor
                          join askush in FoxRepo.GetTable<CrystalDS.askuchRow>()
                              on astob.kuch equals askush.kuch
                          join asmbn in FoxRepo.GetTable<CrystalDS.asmbnRow>(filter: "WHERE  TOP=' '")
                              on new {astob.kgt, astob.kus} equals new {asmbn.kgt, asmbn.kus}
                          join hour in resultmount
                              on new {astob.kgt, astob.kus} equals new {hour.kgt, hour.kus}
                          select new StandEquipment
                              {
                                  Nakor = askor.nakor,
                                  Nauch = askush.nauch,
                                  Snaobor = astob.snaobor,
                                  Dnp = FoxProExtensions.FullDateTime(asmbn.dnp, asmbn.tnp),
                                  Fio = FoxRepo.GetFioByTabNumber(asmbn.iopn, t_asabon),
                                  StandHour = CalcDowntime(asmbn, t_askrm, FoxProExtensions.FullDateTime(asmbn.dnp, asmbn.tnp).Value, now).TotalHours.ToString("F2"),
                                      //(now - (FoxProExtensions.FullDateTime(asmbn.dnp, asmbn.tnp).Value)).TotalHours.ToString("F2"),
                                      
                                  StandHourMount = hour.totalhours.ToString("F2"),
                                  StandCount = hour.count.ToString(),
                                  KrObr = hour.KrObr,
                                  Kgt = hour.kgt,
                                  Kus = hour.kus
                              }).OrderBy(a => a.Nauch);
            return result;
        }

        TimeSpan CalcDowntime(CrystalDataSetLib.CrystalDS.asmbnRow asmbnRec, IEnumerable<CrystalDS.askrmRow> calend, DateTime startPeriod, DateTime endPeriod)
        {
            var startDowntime = FoxProExtensions.FullDateTime(asmbnRec.dnp, asmbnRec.tnp).Value;
            var endDowntime = FoxProExtensions.FullDateTime(asmbnRec.dop, asmbnRec.top) ?? endPeriod;

            var startDate = startDowntime < startPeriod ? startPeriod : startDowntime;
            var endDate = endDowntime > endPeriod ? endPeriod : endDowntime;

            var result = TimeSpan.FromDays(WorkDaysBetween(startDate, endDate, calend)).Subtract(startDate.TimeOfDay).Subtract(TimeSpan.FromDays(1).Subtract(endDate.TimeOfDay));


            return result;// endDowntime.Subtract(startDowntime);
        }
    }
}
