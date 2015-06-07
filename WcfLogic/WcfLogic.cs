using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crystal.BusinessLogic;
using Crystal.DomainModel;

using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WcfLogic
{
    public partial class WcfLogics : ICrystalLogic
    {
  
        public Plants RepoPlant;
        public WcfLogic.ServiceReference1.DataClient bl;

        MessageHeader PutPlantNumberToService(Plants plant)
        {
            MessageHeader<int> customHeaderPlantID =
              new MessageHeader<int>((int)plant);
            MessageHeader untypedHeaderPlantID =
              customHeaderPlantID.GetUntypedHeader("PlantID", "CustomHeader");
            return untypedHeaderPlantID;
        }

        public WcfLogics(Plants plant)
        {
            RepoPlant = plant;
            bl = new WcfLogic.ServiceReference1.DataClient();
            new OperationContextScope(bl.InnerChannel);
            OperationContext.Current.OutgoingMessageHeaders.Add(PutPlantNumberToService(plant));

        }
      
        public IEnumerable<PartSearch> PrtSearch(string query)
        {
            var res = bl.PrtSearch(query);
            return res;
        }
         

        public string GetProductionNumber()
        {
            return bl.GetProductionNumber();
        }

        
        public DateTime GetMonthStartDate()
        {
            var res = bl.GetMonthStartDate();
            return res;
        }

        public TransistorViewModel Production()
        {
            var res = bl.Production();
            return res;
        }

        public BatchInfo PartInfo(string NPRT)
        {
            BatchInfo vModel = bl.PartInfo(NPRT);
            return vModel;
        }

        


        public ProductionInfoModel GetProductionSummary()
        {
            ProductionInfoModel vModel = bl.GetProductionSummary();
            return vModel;
        }



    }
}
