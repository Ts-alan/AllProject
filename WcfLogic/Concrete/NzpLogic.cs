using Crystal.BusinessLogic.Abstract;
using Crystal.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfLogic
{
    public partial class WcfLogics : INzpLogic
    {
        public IEnumerable<MonthReportDataBrak> PribMonthRepordBrak(string nop)
        {
            var res = bl.PribMonthRepordBrak(nop);
            return res;
        }
        public IEnumerable<MonthReportData> GetDataBySelectedPrib(string selPrib)
        {
            var res = bl.GetDataBySelectedPrib(selPrib);
            return res;
        } 
        public IEnumerable<NzpPribList> NzpPrib()
        {
            IEnumerable<NzpPribList> vModel = bl.NzpPrib();
            return vModel;
        }
          public IEnumerable<SimplePribList> PribMonthReport()
          {
              var res = bl.PribMonthReport();
              return res;
          }

        public IEnumerable<NzpPribList> NZPFor2Plant()
        {
            IEnumerable<NzpPribList> vModel = bl.NZPFor2Plant();
            return vModel;
        }

        public IEnumerable<NzpPrib> GetNzpPrib(string kpr)
        {
            IEnumerable<NzpPrib> vModel = bl.GetNzpPrib(kpr);
            return vModel;
        }

        public IEnumerable<NzpPrib> GetNZPFor2Plant(string kpr)
        {
            IEnumerable<NzpPrib> vModel = bl.GetNZPFor2Plant(kpr);
            return vModel;
        }

        public IEnumerable<NzpBatchInfo> GetFromNzp(string kpr, string nop)
        {
            IEnumerable<NzpBatchInfo> vModel = bl.GetFromNzp(kpr, nop);
            return vModel;
        }

        public IEnumerable<NzpFLitList> NzpFlit()
        {
            IEnumerable<NzpFLitList> vModel = bl.NzpFlit();
            return vModel;
        }

        public IEnumerable<NzpFLit> GetNzpFLit(string kpr, string kmk)
        {
            IEnumerable<NzpFLit> vModel = bl.GetNzpFLit(kpr, kmk);
            return vModel;
        }


   

        public IEnumerable<FlitOprNzp> GetFromNzpFlit(string nfl, string kmk, string kpr)
        {
            IEnumerable<FlitOprNzp> vModel = bl.GetFromNzpFlit(nfl, kmk, kpr);
            return vModel;
        }

        public IEnumerable<FlitPartNzp> GetFromNzpFlit2(string nop)
        {
            IEnumerable<FlitPartNzp> vModel = bl.GetFromNzpFlit2(nop);
            return vModel;
        }

        public IEnumerable<PribKplsForDay> PribKplsForDay()
        {
            IEnumerable<PribKplsForDay> vModel = bl.PribKplsForDay();
            return vModel;
        }

        public IEnumerable<GangPribKplsForDay> GangPribKplsForDay(string kpr)
        {
            IEnumerable<GangPribKplsForDay> vModel = bl.GangPribKplsForDay(kpr);
            return vModel;
        }

        public IEnumerable<PribKplsForMonth> PribKplsForMonth()
        {
            IEnumerable<PribKplsForMonth> vModel = bl.PribKplsForMonth();
            return vModel;
        }

        public IEnumerable<DefectsDistributionPrib> DefectsDistr(string fdate, string sdate, string indnumb)
        {
            IEnumerable<DefectsDistributionPrib> vModel = bl.DefectsDistr(fdate, sdate, indnumb);
          
            return vModel;
        }

        public IEnumerable<DefectsDistributionOperations> DefectsDistrOperations(string kpr)
        {
            IEnumerable<DefectsDistributionOperations> vModel = bl.DefectsDistrOperations(kpr);

            return vModel;
        }

        public IEnumerable<DefDistrData> DefectsDistrData(string nop, string kpr, string kmk)
        {
            IEnumerable<DefDistrData> vModel = bl.DefectsDistrData(nop, kpr, kmk);

            return vModel;
        }


        public IEnumerable<DefectsDistributionPrib> DefectsDistrFor2Plant(string fdate, string sdate, string head)
        {
            IEnumerable<DefectsDistributionPrib> vModel = bl.DefectsDistrFor2Plant(fdate,sdate,head);

            return vModel;
        }


        public  IEnumerable<DefectsDistributionOperations> DefectsDistrOperationsFor2Plant(string kpr)
        {
            IEnumerable<DefectsDistributionOperations> vModel =bl.DefectsDistrOperationsFor2Plant(kpr);

            return vModel;
        }
    }
}
