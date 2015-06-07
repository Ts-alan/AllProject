using Crystal.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crystal.BusinessLogic.Abstract
{
    public interface ICyclesLogic
    {
        List<CycleInfo> GetCycle(string fdate, string sdate, string indnumb);
        IEnumerable<CyclesDefectsInfo> ShowDefects(string kpr);
        IEnumerable<cycle_period> ShowGodn(string kpr);
    }
}
