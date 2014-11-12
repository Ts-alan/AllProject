using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Filters.Common
{
    public static class FilterHelper
    {
        public static String GenerateSqlForSingleTextValue(String value, String nameFieldDB, Boolean useNot)
        {
            String adjustedValue = AdjustSingleTextValue(value);

            if (useNot)
                return "(" + nameFieldDB + " NOT LIKE '" + adjustedValue + "' OR " + nameFieldDB + " IS NULL) ";
            else
                return "(" + nameFieldDB + " LIKE '" + adjustedValue + "' )";
        }

        public static String AdjustSingleTextValue(String value)
        {
            return value.TrimEnd(new Char[] { ' ' }).Replace("%", "[%]").Replace('*', '%');
        }
    }
}
