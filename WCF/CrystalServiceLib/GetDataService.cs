using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Crystal.BusinessLogic;
using Crystal.DomainModel;
using Crystal.VFPOleDBLogic;
using EFContext;

namespace WcfGetDataLib
{

    [ServiceContract]
    public interface IData : ICrystalLogic
    {
        [OperationContract]
        BatchInfo PartInfo(string NPRT);

        [OperationContract]
        string GetProductionNumber();

        [OperationContract]
        DateTime GetMonthStartDate();

        [OperationContract]
        TransistorViewModel Production();

        [OperationContract]
        IEnumerable<NzpPribList> NzpPrib();

        [OperationContract]
        IEnumerable<NzpPribList> NZPFor2Plant();

        [OperationContract]
        IEnumerable<NzpPrib> GetNzpPrib(string kpr);

        [OperationContract]
        IEnumerable<NzpPrib> GetNZPFor2Plant(string kpr);

        [OperationContract]
        IEnumerable<NzpBatchInfo> GetFromNzp(string kpr, string nop);

        [OperationContract]
        IEnumerable<NzpFLitList> NzpFlit();

        [OperationContract]
        IEnumerable<NzpFLit> GetNzpFLit(string kpr, string kmk);

        [OperationContract]
        ProductionInfoModel GetProductionSummary();

        [OperationContract]
        IEnumerable<FlitOprNzp> GetFromNzpFlit(string nfl, string kmk, string kpr);

        [OperationContract]
        IEnumerable<FlitPartNzp> GetFromNzpFlit2(string nop);

        [OperationContract]
        List<CycleInfo> GetCycle(string fdate, string sdate, string indnumb);

        [OperationContract]
        IEnumerable<CyclesDefectsInfo> ShowDefects(string kpr);

        [OperationContract]
        IEnumerable<cycle_period> ShowGodn(string kpr);

        [OperationContract]
        IEnumerable<EquipmentLoad> GetEquipmentLoads(DateTime startDate, DateTime endDate);

        [OperationContract]
        IEnumerable<PribKplsForDay> PribKplsForDay();

        [OperationContract]
        IEnumerable<GangPribKplsForDay> GangPribKplsForDay(string kpr);

        [OperationContract]
        IEnumerable<PribKplsForMonth> PribKplsForMonth();

        [OperationContract]
        IEnumerable<DefectsDistributionPrib> DefectsDistr(string fdate, string sdate, string indnumb);


        [OperationContract]
        IEnumerable<DefectsDistributionOperations> DefectsDistrOperations(string kpr);

        [OperationContract]
        IEnumerable<DefDistrData> DefectsDistrData(string nop, string kpr, string kmk);

        [OperationContract]
        IEnumerable<DefectsDistributionPrib> DefectsDistrFor2Plant(string fdate, string sdate, string head);

        [OperationContract]
        IEnumerable<DefectsDistributionOperations> DefectsDistrOperationsFor2Plant(string kpr);

        [OperationContract]
        EquipmentDetailModel GetEquipmentDetail(string techGroupCode, string equipmentCode);

        [OperationContract]
        IEnumerable<string> SearchRes(string query);

        [OperationContract]
        IEnumerable<PartSearch> PrtSearch(string query);

        [OperationContract]
        IEnumerable<Downtimes> Downtimes(string KGT, string KUS);


        [OperationContract]
        IEnumerable<ObservedDevice> GetTrackedDevicesDetail(IEnumerable<TrackedDevice> devices);

        [OperationContract]
        IEnumerable<ObservedBatch> GetTrackedBatchesDetail(IEnumerable<TrackedBatch> batches);

        [OperationContract]
        IEnumerable<SimplePribList> PribMonthReport();

        [OperationContract]
        IEnumerable<MonthReportData> GetDataBySelectedPrib(string selPrib);

        [OperationContract]
        IEnumerable<MonthReportDataBrak> PribMonthRepordBrak(string nop);

        [OperationContract]
        IEnumerable<Approaching> GetPrib(string techGroupCode, string equipmentCode, int Depth);

        [OperationContract]
        IEnumerable<GetAnalysis1> GetAnalysis(string fdate, string sdate, string indnumb);

        [OperationContract]
        IEnumerable<GetAnalysisDiag> GetAnalysisDiag(string[] kpr, string input);

        [OperationContract]
        ParetoData GetParetoData(string fdate, string sdate, string indnumb);

        [OperationContract]
        IEnumerable<ParetoDiag> GetParetoDiag(string[] kpr, string[] kuch);

        [OperationContract]
        IEnumerable<LdPlot> LdPlotByDate(string fdate, string sdate, string indnumb);


        [OperationContract]
        IEnumerable<NzpUch> AreaNzp();
        
        [OperationContract]
        IEnumerable<LoadMth> MZagrBrig(DateTime fdate, DateTime sdate);
        [OperationContract]
        IEnumerable<StandEquipment> StandEquipment();
    }




    public class WcfService : IData
    {
        Plants plant;
        ICrystalLogic bl;
        public WcfService()
        {
            plant = (Plants)OperationContext.Current.IncomingMessageHeaders.GetHeader<int>("PlantID", "CustomHeader");
            bl = new BusinessLogicOleDb(plant);
        }
        public IEnumerable<StandEquipment> StandEquipment()
        {
          return  bl.StandEquipment();
        }
        public IEnumerable<LoadMth> MZagrBrig(DateTime fdate, DateTime sdate)
        {
            return bl.MZagrBrig(fdate, sdate);
        }
        public IEnumerable<MonthReportDataBrak> PribMonthRepordBrak(string nop)
        {
         return   bl.PribMonthRepordBrak(nop);
        }

        public IEnumerable<SimplePribList> PribMonthReport()
        {
            return bl.PribMonthReport();
        }
        public IEnumerable<MonthReportData> GetDataBySelectedPrib(string selPrib)
        {
            return bl.GetDataBySelectedPrib(selPrib);
        }
        public BatchInfo PartInfo(string NPRT)
        {
            return bl.PartInfo(NPRT);
        }

        public string GetProductionNumber()
        {
            return bl.GetProductionNumber();
        }

        public DateTime GetMonthStartDate()
        {
            return bl.GetMonthStartDate();
        }

        public TransistorViewModel Production()
        {
            return bl.Production();
        }

        public IEnumerable<NzpPribList> NzpPrib()
        {
            return bl.NzpPrib();
        }

        public IEnumerable<NzpPribList> NZPFor2Plant()
        {
            return bl.NZPFor2Plant();
        }

        public IEnumerable<NzpPrib> GetNzpPrib(string kpr)
        {
            return bl.GetNzpPrib(kpr);
        }

        public IEnumerable<NzpPrib> GetNZPFor2Plant(string kpr)
        {
            return bl.GetNZPFor2Plant(kpr);
        }

        public IEnumerable<NzpBatchInfo> GetFromNzp(string kpr, string nop)
        {
            return bl.GetFromNzp(kpr, nop);
        }

        public IEnumerable<NzpFLitList> NzpFlit()
        {
            return bl.NzpFlit();
        }

        public IEnumerable<NzpFLit> GetNzpFLit(string kpr, string kmk)
        {
            return bl.GetNzpFLit(kpr, kmk);
        }

        public ProductionInfoModel GetProductionSummary()
        {
            return bl.GetProductionSummary();
        }

        public IEnumerable<FlitOprNzp> GetFromNzpFlit(string nfl, string kmk, string kpr)
        {
            return bl.GetFromNzpFlit(nfl, kmk, kpr);
        }

        public IEnumerable<FlitPartNzp> GetFromNzpFlit2(string nop)
        {
            return bl.GetFromNzpFlit2(nop);
        }

        public List<CycleInfo> GetCycle(string fdate, string sdate, string indnumb)
        {
            return bl.GetCycle(fdate, sdate, indnumb);
        }

        public IEnumerable<CyclesDefectsInfo> ShowDefects(string kpr)
        {
            return bl.ShowDefects(kpr);
        }

        public IEnumerable<cycle_period> ShowGodn(string kpr)
        {
            return bl.ShowGodn(kpr);
        }

        public IEnumerable<EquipmentLoad> GetEquipmentLoads(DateTime startDate, DateTime endDate)
        {
            return bl.GetEquipmentLoads(startDate, endDate);
        }

        public IEnumerable<PribKplsForDay> PribKplsForDay()
        {
            return bl.PribKplsForDay();
        }

        public IEnumerable<GangPribKplsForDay> GangPribKplsForDay(string kpr)
        {
            return bl.GangPribKplsForDay(kpr);
        }


        public IEnumerable<PribKplsForMonth> PribKplsForMonth()
        {

            return bl.PribKplsForMonth();
        }

        public IEnumerable<DefectsDistributionPrib> DefectsDistr(string fdate, string sdate, string indnumb)
        {
            return bl.DefectsDistr(fdate, sdate, indnumb);
        }


        public IEnumerable<DefectsDistributionOperations> DefectsDistrOperations(string kpr)
        {
            return bl.DefectsDistrOperations(kpr);
        }

        public IEnumerable<DefDistrData> DefectsDistrData(string nop, string kpr, string kmk)
        {
            return bl.DefectsDistrData(nop, kpr, kmk);
        }


        public IEnumerable<DefectsDistributionPrib> DefectsDistrFor2Plant(string fdate, string sdate, string head)
        {
            return bl.DefectsDistrFor2Plant(fdate, sdate, head);
        }

        public IEnumerable<DefectsDistributionOperations> DefectsDistrOperationsFor2Plant(string kpr)
        {
            return bl.DefectsDistrOperationsFor2Plant(kpr);
        }


        public EquipmentDetailModel GetEquipmentDetail(string techGroupCode, string equipmentCode)
        {
            return bl.GetEquipmentDetail(techGroupCode, equipmentCode);
        }

        public IEnumerable<Downtimes> Downtimes(string KGT, string KUS)
        {
            var data = bl.Downtimes(KGT, KUS);
            return data;
        }


        public IEnumerable<string> SearchRes(string query)
        {
            return bl.SearchRes(query);
        }
        public IEnumerable<PartSearch> PrtSearch(string query)
        {
            return bl.PrtSearch(query);
        }


        public IEnumerable<ObservedDevice> GetTrackedDevicesDetail(IEnumerable<TrackedDevice> devices)
        {
            return bl.GetTrackedDevicesDetail(devices);
        }
        public IEnumerable<ObservedBatch> GetTrackedBatchesDetail(IEnumerable<TrackedBatch> batches)
        {
            return bl.GetTrackedBatchesDetail(batches);
        }


        public IEnumerable<Approaching> GetPrib(string techGroupCode, string equipmentCode, int Depth)
        {
            return bl.GetPrib(techGroupCode, equipmentCode, Depth);
        }

        public IEnumerable<GetAnalysis1> GetAnalysis(string fdate, string sdate, string indnumb)
        {
            return bl.GetAnalysis(fdate, sdate, indnumb);
        }


        public IEnumerable<GetAnalysisDiag> GetAnalysisDiag(string[] kpr, string input)
        {

            return bl.GetAnalysisDiag(kpr,input);
        }

        public ParetoData GetParetoData(string fdate, string sdate, string indnumb)
        {
            return bl.GetParetoData(fdate,sdate,indnumb);
        }


        public IEnumerable<ParetoDiag> GetParetoDiag(string[] kpr, string[] kuch)
        {
            return bl.GetParetoDiag(kpr, kuch);
        }


        public IEnumerable<LdPlot> LdPlotByDate(string fdate, string sdate, string indnumb)
        {
            return bl.LdPlotByDate(fdate, sdate,indnumb);
        }

        public IEnumerable<NzpUch> AreaNzp()
        {
            return bl.AreaNzp();
        }
    }

}

