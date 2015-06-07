using Crystal.BusinessLogic.Abstract;
using Crystal.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfLogic
{
    public partial class WcfLogics : ICyclesLogic
    {
        public List<CycleInfo> GetCycle(string fdate, string sdate, string indnumb)
        {
            List<CycleInfo> vModel = bl.GetCycle(fdate, sdate, indnumb).ToList();
            return vModel;
        }
         
        public IEnumerable<CyclesDefectsInfo> ShowDefects(string kpr)
        {
            IEnumerable<CyclesDefectsInfo> vModel = bl.ShowDefects(kpr);
            return vModel;
        }


        public IEnumerable<cycle_period> ShowGodn(string kpr)
        {

            IEnumerable<cycle_period> vModel = bl.ShowGodn(kpr);
            return vModel;
        }
       
    }
}
