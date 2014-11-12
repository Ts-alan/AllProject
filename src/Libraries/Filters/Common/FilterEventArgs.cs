using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Filters.Common
{
    public class FilterEventArgs : EventArgs
    {
        public FilterEventArgs(string _where)
        {
            where = _where;
        }

        private readonly string where;
        public string Where
        {
            get
            {
                return where;
            }
        }
    }
}
