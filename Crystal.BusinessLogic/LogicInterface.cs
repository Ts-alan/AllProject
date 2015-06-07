
using Crystal.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Crystal.BusinessLogic.Abstract;

namespace Crystal.BusinessLogic
{
    public interface ICrystalLogic : ICyclesLogic, INzpLogic, IEquipmentLogic, IUserTrackedLogic,IStatisticLogic
    {
        //ProductionMap GetModuleMap();
        string GetProductionNumber();
        DateTime GetMonthStartDate();
        TransistorViewModel Production();
        BatchInfo PartInfo(string NPRT);
        
        ProductionInfoModel GetProductionSummary();
        IEnumerable<string> SearchRes(string query);
        IEnumerable<PartSearch> PrtSearch(string query);

    }
}
