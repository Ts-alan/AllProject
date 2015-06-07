using Crystal.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crystal.BusinessLogic.Abstract
{
    public interface INzpLogic
    {
        IEnumerable<NzpPribList> NzpPrib();
        IEnumerable<NzpPribList> NZPFor2Plant();
        IEnumerable<NzpPrib> GetNzpPrib(string kpr);
        IEnumerable<NzpPrib> GetNZPFor2Plant(string kpr);
        IEnumerable<NzpBatchInfo> GetFromNzp(string kpr, string nop);
        IEnumerable<NzpFLitList> NzpFlit();
        IEnumerable<NzpFLit> GetNzpFLit(string kpr, string kmk);
        IEnumerable<FlitOprNzp> GetFromNzpFlit(string nfl, string kmk, string kpr);
        IEnumerable<FlitPartNzp> GetFromNzpFlit2(string nop);
        IEnumerable<PribKplsForDay> PribKplsForDay();
        IEnumerable<GangPribKplsForDay> GangPribKplsForDay(string kpr);
        IEnumerable<PribKplsForMonth> PribKplsForMonth();
        IEnumerable<DefectsDistributionPrib> DefectsDistr(string fdate, string sdate, string indnumb);
        IEnumerable<DefectsDistributionOperations> DefectsDistrOperations(string kpr);
        IEnumerable<DefDistrData> DefectsDistrData(string nop, string kpr, string kmk);
        IEnumerable<DefectsDistributionPrib> DefectsDistrFor2Plant(string fdate, string sdate, string head);
        IEnumerable<DefectsDistributionOperations> DefectsDistrOperationsFor2Plant(string kpr);
        IEnumerable<SimplePribList> PribMonthReport();
        IEnumerable<MonthReportData> GetDataBySelectedPrib(string selPrib);
        IEnumerable<MonthReportDataBrak> PribMonthRepordBrak(string nop);
    }

}
