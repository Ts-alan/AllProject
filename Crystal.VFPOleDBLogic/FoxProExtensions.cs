using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crystal.BusinessLogic;
using CrystalDataSetLib;

namespace Crystal.VFPOleDBLogic
{
    public static class FoxProExtensions
    {
        public static readonly DateTime FoxNullDate = DateTime.FromOADate(0).Date;

        public static bool isEmptyDate(this DateTime dt)
        {
            return dt == FoxNullDate ? true : false;
        }

        public static DateTime? FullDateTime(DateTime dato, String Timo)
        {
            if (dato.isEmptyDate())
                //throw new ArgumentNullException("dato", "dato is EMPTY in FoxPro");
                return null;
            
            if (Timo.Trim().Length != 4)
                throw new ArgumentOutOfRangeException("Timo", "timo must have 4 chars");

            return new DateTime(year: dato.Year, month: dato.Month, day: dato.Day, hour: int.Parse(Timo.Trim().Substring(0, 2)), minute: int.Parse(Timo.Trim().Substring(2, 2)), second: 0);
        }
        public static string GetFoxTime(this string tim)
        {
            return tim != "    " ? tim.Substring(0, 2) + ":" + tim.Substring(2, 2) : "";//dt.isNullDate() ? "" : dt.ToShortDateString();// dt == DateTime.Parse("30/12/1899") ? true : false;
        }

        public static string GetFoxDate(this DateTime dt)
        {
            return dt.isEmptyDate() ? "" : dt.ToShortDateString();// dt == DateTime.Parse("30/12/1899") ? true : false;
        }

        public static double HoursBetween(DateTime dt_e,DateTime dt_s)
        {
            return dt_e.Subtract(dt_s).TotalHours;
        }



    }

}
