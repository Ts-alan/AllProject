using System;
using System.Collections.Generic;
using System.Text;
using Filters.Common;

namespace Filters.TemporaryGroup
{
    public static class TemporaryGroupFilterHelper
    {
        private static String _nameFieldDB = "ComputerName";

        public static String GenerateSqlForComputersList(List<String> computers)
        {
            if (computers == null) return String.Empty;
            if (computers.Count == 0) return String.Empty;
            String result = String.Join("','", computers.ToArray());
            return String.Format("{0} in ('{1}')", _nameFieldDB, result);
        }
    }
}
