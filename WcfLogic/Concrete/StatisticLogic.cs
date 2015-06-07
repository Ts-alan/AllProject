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

        public IEnumerable<GetAnalysis1> GetAnalysis(string fdate, string sdate, string indnumb)
        {
            IEnumerable<GetAnalysis1> res = bl.GetAnalysis(fdate,sdate,indnumb);
            return res;
        }


        public IEnumerable<GetAnalysisDiag> GetAnalysisDiag(string[] kpr, string input)
        {
            IEnumerable<GetAnalysisDiag> res = bl.GetAnalysisDiag(kpr, input);
            return res;
        }

        public ParetoData GetParetoData(string fdate, string sdate, string indnumb)
        {
            ParetoData res = bl.GetParetoData(fdate,sdate,indnumb);
            return res;
        }

        public IEnumerable<ParetoDiag> GetParetoDiag(string[] kpr, string[] kuch)
        {
            IEnumerable<ParetoDiag> res = bl.GetParetoDiag(kpr,kuch);
            return res;
        }
    }
}