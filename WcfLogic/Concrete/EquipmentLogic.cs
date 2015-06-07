using Crystal.BusinessLogic.Abstract;
using Crystal.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfLogic
{
    public partial class WcfLogics : IEquipmentLogic
    {
        public IEnumerable<EquipmentLoad> GetEquipmentLoads(DateTime startDate, DateTime endDate){
            return bl.GetEquipmentLoads(startDate, endDate);
        }

        public IEnumerable<string> SearchRes(string query)
        {
            return bl.SearchRes(query);
        }
        public IEnumerable<LoadMth> MZagrBrig(DateTime fdate, DateTime sdate)
        {
            return bl.MZagrBrig(fdate, sdate);
        }

        public EquipmentDetailModel GetEquipmentDetail(string techGroupCode, string equipmentCode)
        {
            return bl.GetEquipmentDetail(techGroupCode, equipmentCode);
        }


        public IEnumerable<Downtimes> Downtimes(string KGT, string KUS)
        {
            return bl.Downtimes(KGT,KUS);
        }

        public IEnumerable<Approaching> GetPrib(string techGroupCode, string equipmentCode, int Depth)
        {

            return bl.GetPrib(techGroupCode, equipmentCode, Depth);
        }


        public IEnumerable<LdPlot> LdPlotByDate(string fdate, string sdate, string indnumb)
        {
            return bl.LdPlotByDate(fdate, sdate, indnumb); 
        
        }

        public IEnumerable<NzpUch> AreaNzp()
        {
            return bl.AreaNzp(); 
        }

        public IEnumerable<StandEquipment> StandEquipment()
        {
            return bl.StandEquipment();
        }

    }
}
