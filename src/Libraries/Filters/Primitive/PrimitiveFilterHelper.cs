using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using VirusBlokAda.CC.Filters.Common;

namespace VirusBlokAda.CC.Filters.Primitive
{
    public static class PrimitiveFilterHelper
    {
        public static String GenerateSqlForTextValue(String value, String nameFieldDB, Boolean useOr, Boolean useNot)
        {
            String[] splitted = value.Split(new Char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            for (Int32 i = 0; i < splitted.Length; i++)
            {
                splitted[i] = FilterHelper.GenerateSqlForSingleTextValue(splitted[i], nameFieldDB, useNot);
            }
            String result = String.Join(useNot ? "AND" : "OR", splitted);
            if (String.IsNullOrEmpty(result)) return String.Empty;
            return (useOr ? "OR" : "AND") + "(" + result + ")";
        }

        public static String GenerateSqlForIpValue(String value, String nameFieldDB, Boolean useOr, Boolean useNot)
        {
            String[] splitted = value.Split(new Char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            for (Int32 i = 0; i < splitted.Length; i++)
            {
                if (splitted[i].Contains("-"))
                {
                    splitted[i] = GenerateSqlForIpRangeValue(splitted[i], nameFieldDB, useNot);
                }
                else
                {
                    splitted[i] = FilterHelper.GenerateSqlForSingleTextValue(splitted[i], nameFieldDB, useNot);
                }
            }
            String result = String.Join(useNot ? "AND" : "OR", splitted);
            if (String.IsNullOrEmpty(result)) return String.Empty;
            return (useOr ? "OR" : "AND") + "(" + result + ")";
        }

        private static String GenerateSqlForIpRangeValue(String value, String nameFieldDB, Boolean useNot)
        {
            Int32 delim = value.IndexOf('-');
            String sStart = value.Substring(0, delim);
            String sEnd = value.Substring(delim + 1);
            Int64 lStart = ConvertIpToLong(sStart);
            Int64 lEnd = ConvertIpToLong(sEnd);

            if (lStart > lEnd)
            {
                Int64 temp = lStart;
                lStart = lEnd;
                lEnd = temp;
            }
            return String.Format("{0}(dbo.ConvertIPToBigInt([{1}]) BETWEEN {2} AND {3})", useNot ? "NOT " : "",
                nameFieldDB, lStart, lEnd);
        }


        private static Int64 ConvertIpToLong(String ip)
        { 
            Regex ipRegex = new Regex("(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)");
            if (!ipRegex.IsMatch(ip))
            {
                throw new ArgumentException("ConvertIpToLong: submited ip is in incorrect format.");
            }
            Int32 startIndex;
            Int32 endIndex;
            String temp;
            String final = "";

            startIndex = 0;

            for (Int32 i = 0; i < 4; i++)
            {
                endIndex = ip.IndexOf('.', startIndex);
                if (endIndex == -1)
                {
                    endIndex = ip.Length;
                }

                temp = "00" + ip.Substring(startIndex, endIndex - startIndex);
                final += temp.Substring(temp.Length - 3);
                startIndex = endIndex + 1;
            }

            return Int64.Parse(final); 
        }

        public static String GenerateSqlForRangeValue(Range range, String nameFieldDB, Boolean useOr, Boolean useNot)
        {
            String result;
            if (useNot)
                result = "(" + nameFieldDB + " NOT BETWEEN '" + range.Start + "' AND '" + range.Stop + "') ";
            else
                result = "(" + nameFieldDB + " BETWEEN '" + range.Start + "' AND '" + range.Stop + "') ";
            
            return (useOr ? "OR" : "AND") + result;
        }

        public static String GenerateSqlForRangeDateTimeValue(DateTime start, DateTime stop, String NameFieldDB, Boolean useOr, Boolean useNot)
        {
            String result = String.Empty;
            if (start > stop)
                throw new Exception("StartDateTime must be less than StopDateTime.");
            if (useNot)
                result = "(" + NameFieldDB + " < " + String.Format("CAST('{0:yyyy}-{0:MM}-{0:dd}T{0:HH}:{0:mm}:{0:ss}' AS SMALLDATETIME)", start)
                    + " OR " + NameFieldDB + " > " + String.Format("CAST('{0:yyyy}-{0:MM}-{0:dd}T{0:HH}:{0:mm}:{0:ss}' AS SMALLDATETIME)", stop) + ") ";
            else
                result = "(" + NameFieldDB + " >= " + String.Format("CAST('{0:yyyy}-{0:MM}-{0:dd}T{0:HH}:{0:mm}:{0:ss}' AS SMALLDATETIME)", start)
                   + " AND " + NameFieldDB + " <= " + String.Format("CAST('{0:yyyy}-{0:MM}-{0:dd}T{0:HH}:{0:mm}:{0:ss}' AS SMALLDATETIME)", stop) + ") ";
            return (useOr ? "OR" : "AND") + result;
        }
    }
}
