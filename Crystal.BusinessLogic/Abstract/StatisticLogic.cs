using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crystal.DomainModel;

namespace Crystal.BusinessLogic.Abstract
{
    public interface IStatisticLogic
    {
       IEnumerable<GetAnalysis1> GetAnalysis(string fdate, string sdate, string indnumb);
       IEnumerable<GetAnalysisDiag> GetAnalysisDiag(string[] kpr, string input);
       ParetoData GetParetoData(string fdate, string sdate, string indnumb);
       IEnumerable<ParetoDiag> GetParetoDiag(string[] kpr, string[] kuch);
    }
}
