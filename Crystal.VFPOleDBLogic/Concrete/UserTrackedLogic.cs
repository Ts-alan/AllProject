using Crystal.BusinessLogic.Abstract;
using Crystal.DomainModel;
using CrystalDataSetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crystal.VFPOleDBLogic
{
    public partial class BusinessLogicOleDb : IUserTrackedLogic
    {
        public IEnumerable<ObservedDevice> GetTrackedDevicesDetail(IEnumerable<TrackedDevice> devices)
        {
            var result = from asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                         join device in devices
                                on asspr.kpr equals device.DeviceNumber
                         join mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>()
                          on device.DeviceNumber equals mpartt.kpr into g_mpartt
                         select new ObservedDevice
                         {
                             Kpr = asspr.kpr,
                             Napr = asspr.napr,
                             ProdNumber = device.ProdNumber,
                             Nzp = Convert.ToInt32(g_mpartt.Sum(a => a.kpls)),
                             Nnprt = g_mpartt.Select(a => a.nprt).Count()
                         };

            return result;
        }

        public IEnumerable<ObservedBatch> GetTrackedBatchesDetail(IEnumerable<TrackedBatch> batches)
        {
            var t_askrm = FoxRepo.GetTable<CrystalDS.askrmRow>();
            var preSelect = from mpartt in FoxRepo.GetTable<CrystalDS.mparttRow>()
                            join batch in batches
                                on mpartt.nprt equals batch.BatchNumber
                            join asspr in FoxRepo.GetTable<CrystalDS.assprRow>()
                              on mpartt.kpr equals asspr.kpr
                            join astmr in FoxRepo.GetTable<CrystalDS.astmrRow>()
                              on new { mpartt.kmk, mpartt.nop } equals new { astmr.kmk, astmr.nop }
                            join assop in FoxRepo.GetTable<CrystalDS.assopRow>()
                              on astmr.kop equals assop.kop
                            let LastOperationDate = FoxProExtensions.FullDateTime(mpartt.dato, mpartt.timo) ?? FoxProExtensions.FullDateTime(mpartt.dath, mpartt.timh)
                            select new ObservedBatch
                            {
                                Nprt = mpartt.nprt,
                                Napr = asspr.napr,
                                Naop = assop.naop,
                                Nzp = mpartt.kpls,
                                Date = LastOperationDate,
                                DaysOnOperation = WorkDaysBetween(LastOperationDate.Value, DateTime.Now, t_askrm),
                                ProdNumber = batch.ProdNumber
                            };

            return preSelect;
        }
    }
}
