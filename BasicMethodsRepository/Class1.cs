using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using CrystalDataSetLib;
namespace BasicMethodsRepository
{
    public static class Conslg
    {
        public static IHtmlString Log(this HtmlHelper helper, string items) //Хелпер генерации console.log() в режиме дебага с проверкой на поддержку браузера
        {
#if DEBUG
            return MvcHtmlString.Create(string.Format("if (window.console) {{ console.log({0});}};", items));
#else
            return new HtmlString("");
#endif
        }
    }

    public enum Plants { plant43 = 3, plant47 = 4, plant12 = 12, plant20 = 20, bms, plant_none };

    public static class FoxRepo
    {
        public static readonly DateTime FoxNullDate = new DateTime(1899, 12, 30);
        public const string CONNECTION_STRING = "_ConnectionString";

        public static Plants GetPlant(Plants plant = Plants.plant_none)
        {

            return Plants.plant43;
        }

        /// <summary>
        /// Преобразование строки в перечисление. 
        /// </summary>
        /// <param name="prodNumber"></param>
        /// <returns></returns>
        public static Plants GetPlantFromNumber(string prodNumber)
        {
            var tst = ((Plants)int.Parse(prodNumber));
            return tst;
        }

        public static string GetConnection(Plants plant)
        {
            string expected_ConnectionString = plant.ToString().ToUpper() + CONNECTION_STRING;
            System.Configuration.ConnectionStringSettings connectionStringSettings = System.Configuration.ConfigurationManager.ConnectionStrings[expected_ConnectionString];

            return connectionStringSettings.ConnectionString;
        }



        public static IEnumerable<T> GetTable<T>(bool useCache = false, bool isArchived = false, bool ShowDeleted = false, Plants plant = Plants.plant_none, string filter = "", params Object[] parameters) where T : global::System.Data.DataRow
        {
            Plants pl;//Добавлено
            pl = plant == Plants.plant_none ? GetPlant(plant) : plant;
            string plant_name = pl.ToString();
            /* old incorrect code
            Plants pl = GetPlant(plant);
            string plant_name = pl.ToString();*/
            Type t = typeof(T);
            string db_name = t.Name.Substring(0, t.Name.LastIndexOf("Row"));

            DataTable tbl;

            switch (db_name)
            {
                case "mpartn": tbl = new CrystalDS.mpartnDataTable(); break;
                case "asbrak": tbl = new CrystalDS.asbrakDataTable(); break;
                case "askor": tbl = new CrystalDS.askorDataTable(); break;
                case "asmnp": tbl = new CrystalDS.asmnpDataTable(); break;
                case "astob": tbl = new CrystalDS.astobDataTable(); break;
                case "asspr": tbl = new CrystalDS.assprDataTable(); break;
                case "assufl": tbl = new CrystalDS.assuflDataTable(); break;
                case "asmbn": tbl = new CrystalDS.asmbnDataTable(); break;
                case "askuch": tbl = new CrystalDS.askuchDataTable(); break;
                case "as_wop": tbl = new CrystalDS.as_wopDataTable(); break;
                case "mprxop": tbl = new CrystalDS.mprxopDataTable(); break;
                case "astmr": tbl = new CrystalDS.astmrDataTable(); break;
                case "asks": tbl = new CrystalDS.asksDataTable(); break;
                case "assop": tbl = new CrystalDS.assopDataTable(); break;
                case "asabon": tbl = new CrystalDS.asabonDataTable(); break;
                case "mpartt": tbl = new CrystalDS.mparttDataTable(); break;
                case "mcompl": tbl = new CrystalDS.mcomplDataTable(); break;
                case "mcopl": tbl = new CrystalDS.mparttDataTable(); break;
                case "askrm": tbl = new CrystalDS.askrmDataTable(); break;
                case "asscp": tbl = new CrystalDS.asscpDataTable(); break;
                case "askprob": tbl = new CrystalDS.askprobDataTable(); break;
                case "asparam": tbl = new CrystalDS.asparamDataTable(); break;
                case "askprb": tbl = new CrystalDS.askprbDataTable(); break;
                case "mkrist": tbl = new CrystalDS.mkristDataTable(); break;
                case "asob_nal": tbl = new CrystalDS.asob_nalDataTable(); break;
                case "assbrn": tbl = new CrystalDS.assbrnDataTable(); break;
                case "asmkt": tbl = new CrystalDS.asmktDataTable(); break;
                case "asgsm": tbl = new CrystalDS.asgsmDataTable(); break;
                default: throw new InvalidCastException(string.Format("FoxRepo.GetTable don't know {0}", db_name));
            }

            if (isArchived)
                db_name = "a" + db_name;

            bool canCached = (isArchived && filter == "") || useCache;

            if (canCached)
            {
                Object data = HttpContext.Current.Cache.Get(plant_name + "_" + db_name);

                if (data != null) return (IEnumerable<T>)data;
            }

            using (IDbConnection dbConn = new OleDbConnection(GetConnection(pl)))
            {
                IDbCommand dbCmd = dbConn.CreateCommand();
                string selectCommand = "select * from " + db_name;
                if (filter != "")
                {
                    selectCommand += (" " + filter);
                }
                dbCmd.CommandText = selectCommand;

                if (parameters != null)
                    foreach (var par in parameters)
                    {
                        dbCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("", par));
                    }

                if (ShowDeleted) dbConn.ShowDeleted();
                dbConn.Open();
                tbl.Load(dbCmd.ExecuteReader(), LoadOption.OverwriteChanges, (s, e) => { e.Continue = true; });

                dbConn.Close();
            }

            IEnumerable<T> res = (tbl as global::System.Data.TypedTableBase<T>).AsEnumerable().ToList();
            if (canCached)
                HttpContext.Current.Cache.Insert(plant_name + "_" + db_name, res, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(180));
            return res;
        }




        public static double HoursBetween(DateTime dt_e, string time_e, DateTime dt_s, string time_s)
        {
            DateTime date_e;
            if (dt_e.isNullDate())
            {
                date_e = DateTime.Now;

            }
            else date_e = new DateTime(year: dt_e.Year, month: dt_e.Month, day: dt_e.Day, hour: int.Parse(time_e.Substring(0, 2)), minute: int.Parse(time_e.Substring(2, 2)), second: 0);
            DateTime date_s = new DateTime(year: dt_s.Year, month: dt_s.Month, day: dt_s.Day, hour: int.Parse(time_s.Substring(0, 2)), minute: int.Parse(time_s.Substring(2, 2)), second: 0);
            return date_e.Subtract(date_s).TotalHours;
        }

        public static double HoursBetweenForMZagr(DateTime dt_e, string time_e, DateTime dt_s, string time_s)//dt_s,time_s-начало,dt_e,time_e-конец;
        {
            DateTime date_e;
            DateTime date_s = DateTime.Now;
            if (dt_e.isNullDate())
            {
                date_e = DateTime.Now;
            }
            else
            {
                date_e = new DateTime(year: dt_e.Year, month: dt_e.Month, day: dt_e.Day, hour: int.Parse(time_e.Substring(0, 2)), minute: int.Parse(time_e.Substring(2, 2)), second: 0);
            }

            if (date_e.Month != dt_s.Month)
                date_s = new DateTime(year: date_e.Year, month: date_e.Month, day: date_e.Day, hour: 0, minute: 0, second: 0);
            else
                date_s = new DateTime(year: dt_s.Year, month: dt_s.Month, day: dt_s.Day, hour: int.Parse(time_s.Substring(0, 2)), minute: int.Parse(time_s.Substring(2, 2)), second: 0);

            return date_e.Subtract(date_s).TotalHours;
        }
        public static double HoursBetweenForStandEquipment(DateTime dt_s, string time_s, DateTime dt_e, string time_e, DateTime startmount)//dt_s,time_s-начало,dt_e,time_e-конец,startmount-начало месяца
        {
            DateTime date_e;
            DateTime date_s;


            if (OneColDateTime(dt_s, time_s) > startmount)
                date_s = new DateTime(year: dt_s.Year, month: dt_s.Month, day: dt_s.Day, hour: int.Parse(time_s.Substring(0, 2)), minute: int.Parse(time_s.Substring(2, 2)), second: 0);
            else
                date_s = new DateTime(year: startmount.Year, month: startmount.Month, day: startmount.Day, hour: startmount.Hour, minute: startmount.Minute, second: startmount.Second);

            if (dt_e.isNullDate())
            {
                date_e = DateTime.Now;
            }
            else
            {
                date_e = new DateTime(year: dt_e.Year, month: dt_e.Month, day: dt_e.Day,
                                      hour: int.Parse(time_e.Substring(0, 2)), minute: int.Parse(time_e.Substring(2, 2)),
                                      second: 0);
            }


            return date_e.Subtract(date_s).TotalHours;
        }

        public static int WorkDaysBetween(DateTime start_date, DateTime end_date, IEnumerable<CrystalDS.askrmRow> calend)
        {
            return calend.Count(a => a.dm > start_date && a.dm <= end_date);
        }

        public static DateTime OneColDateTime(DateTime dato, String Timo)
        {
            DateTime DatoTimo;
            if (dato.isNullDate())
            {
                DatoTimo = DateTime.Now;
            }
            else
            {
                DatoTimo = new DateTime(year: dato.Year, month: dato.Month, day: dato.Day, hour: int.Parse(Timo.Trim().Substring(0, 2)), minute: int.Parse(Timo.Trim().Substring(2, 2)), second: 0);
            }
            return DatoTimo;
        }

        public static DateTime FullDateTime(DateTime dato, String Timo)
        {
            if (dato.isNullDate())
                throw new ArgumentNullException("dato", "dato is EMPTY in FoxPro");

            if (Timo.Trim().Length != 4)
                throw new ArgumentOutOfRangeException("Timo", "timo must have 4 chars");

            return new DateTime(year: dato.Year, month: dato.Month, day: dato.Day, hour: int.Parse(Timo.Trim().Substring(0, 2)), minute: int.Parse(Timo.Trim().Substring(2, 2)), second: 0);
        }

        public static string GetFioByTabNumber(string tn, IEnumerable<CrystalDS.asabonRow> abons = null)
        {
            if (tn.Trim() == "") return "";
            if (abons == null) abons = FoxRepo.GetTable<CrystalDS.asabonRow>();
            var abonent = abons.Where(a => a.tbn == tn);
            return abonent.Count() > 0 ? abonent.First().fio : "уволен (" + tn + ")";
        }

        public static int WorkDaysInMonth(DateTime dt)
        {
            DateTime date_e = new DateTime(year: dt.Year, month: dt.Month, day: DateTime.DaysInMonth(dt.Year, dt.Month));
            DateTime date_s = new DateTime(year: dt.Year, month: dt.Month, day: 1);
            return WorkDaysBetween(date_s, date_e, GetTable<CrystalDS.askrmRow>());
            //return FoxRepo.GetTable<Crystal.askrmRow>().Count(a => a.dm > date_s && a.dm < date_e);
        }

        public static string GetFoxTime(string tim)
        {
            return tim != "    " ? tim.Substring(0, 2) + ":" + tim.Substring(2, 2) : "";
        }

        public static string GetProductionNumber()
        {
            return GetTable<CrystalDS.asparamRow>().First(x => x.identp.Trim() == "FACTORY").valp.Trim();
        }
        static string Get_StartMs()
        {
            return FoxRepo.GetTable<CrystalDS.asparamRow>(filter: "WHERE identp == ?", parameters: new[] { "start_ms" }).Single().valp;
        }

        public static DateTime GetMonthStartDate()
        {
            return DateTime.ParseExact(Get_StartMs(), "dd/MM/yyyy HHmm", System.Globalization.CultureInfo.InvariantCulture);
        }
        public static DateTime GetStartDay()
        {
            var startmount = from asparam in FoxRepo.GetTable<CrystalDS.asparamRow>()
                             where asparam.identp.Trim() == "START_SU"
                             select new
                             {
                                 Time = DateTime.ParseExact(asparam.valp.Trim(), "dd/MM/yyyy HHmm", System.Globalization.CultureInfo.InvariantCulture)

                             };
            return startmount.Single().Time;
        }
        public static string GetPeriod(string fdate, string sdate, string indnumb)
        {
            string str = "";
            switch (indnumb)
            {
                case "1":
                    str = "за " + DateTime.Parse(fdate).ToString("dd/MM/yyyy");
                    break;
                case "2":
                    str = "c " + DateTime.Parse(fdate).ToString("dd/MM/yyyy") + " по " + DateTime.Parse(sdate).ToString("dd/MM/yyyy");
                    break;
                case "3":
                    str = "за " + DateTime.Parse(fdate).ToString("MMMM") + " " + DateTime.Parse(fdate).Year + " года";
                    break;
                case "4":
                    switch (DateTime.Parse(fdate).Month)
                    {

                        case 01:
                            str = "за первый квартал " + DateTime.Parse(fdate).Year + " года";
                            break;
                        case 04:
                            str = "за второй квартал " + DateTime.Parse(fdate).Year + " года";
                            break;
                        case 07:
                            str = "за третий квартал " + DateTime.Parse(fdate).Year + " года";
                            break;
                        case 10:
                            str = "за четвертый квартал " + DateTime.Parse(fdate).Year + "года";
                            break;

                    }
                    break;
            }
            return str;

        }
    }

    public static class TableAdapterExtensions
    {
        public static void ShowDeleted(this IDbConnection conn)
        {
            conn.ConnectionString += "DELETED=FALSE;";
        }

        public static bool isNullDate(this DateTime dt)
        {
            return dt == FoxRepo.FoxNullDate ? true : false;
        }

        public static string GetFoxDate(this DateTime dt)
        {
            return dt.isNullDate() ? "" : dt.ToShortDateString();
        }

        public static string GetFoxTime(this string time)
        {
            return time != "    " ? time.Insert(2, ":") : "";
        }
    }
}
