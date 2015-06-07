using Crystal.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crystal.BusinessLogic.Abstract
{
    public interface IEquipmentLogic
    {
        IEnumerable<EquipmentLoad> GetEquipmentLoads(DateTime startDate, DateTime endDate);
        EquipmentDetailModel GetEquipmentDetail(string techGroupCode, string equipmentCode);
        IEnumerable<Downtimes> Downtimes(string KGT, string KUS);
        IEnumerable<Approaching> GetPrib(string techGroupCode, string equipmentCode, int Depth);
        IEnumerable<LdPlot> LdPlotByDate(string fdate, string sdate, string indnumb);
        IEnumerable<NzpUch> AreaNzp();
        IEnumerable<LoadMth> MZagrBrig(DateTime fdate, DateTime sdate);
        IEnumerable<StandEquipment> StandEquipment();
    }
}
