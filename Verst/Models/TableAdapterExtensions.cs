using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Data;
using Verst.Models;

namespace MvcApplication.Models
{
    public enum Plants { plant43 = 3, plant47 = 4, plant12 = 12, plant20 = 20, bms, plant_none };

    public static class FoxRepo
    {
        public const string CONNECTION_STRING = "_ConnectionString";

        public static Plants GetPlant(Plants plant = Plants.plant_none)
        {
            string plant_name = plant.ToString();
            if (plant == Plants.plant_none)
                plant_name = HttpContext.Current.Request.RequestContext.RouteData.Values["plant"] as string;

            // Преобразование строки в перечисление. 
            Plants pl = Plants.plant_none;
            if (Enum.IsDefined(typeof(Plants), plant_name))
                pl = (Plants)Enum.Parse(typeof(Plants), plant_name, true);
            else
                throw new InvalidCastException(string.Format("Error PlantName\nCheck Plants enum for {0}", plant_name));
            return pl;
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
            if (connectionStringSettings == null)
            {
                throw new System.Configuration.ConfigurationErrorsException(string.Format("Error ConnectionString in Web.config\nExpected: {0}", expected_ConnectionString));
            }
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
                case "mpartn": tbl = new Crystal.mpartnDataTable(); break;
                case "asbrak": tbl = new Crystal.asbrakDataTable(); break;
                case "askor": tbl = new Crystal.askorDataTable(); break;
                case "asmnp": tbl = new Crystal.asmnpDataTable(); break;
                case "astob": tbl = new Crystal.astobDataTable(); break;
                case "asspr": tbl = new Crystal.assprDataTable(); break;
                case "assufl": tbl = new Crystal.assuflDataTable(); break;
                case "asmbn": tbl = new Crystal.asmbnDataTable(); break;
                case "askuch": tbl = new Crystal.askuchDataTable(); break;
                case "as_wop": tbl = new Crystal.as_wopDataTable(); break;
                case "mprxop": tbl = new Crystal.mprxopDataTable(); break;
                case "astmr": tbl = new Crystal.astmrDataTable(); break;
                case "asks": tbl = new Crystal.asksDataTable(); break;
                case "assop": tbl = new Crystal.assopDataTable(); break;
                case "asabon": tbl = new Crystal.asabonDataTable(); break;
                case "mpartt": tbl = new Crystal.mparttDataTable(); break;
                case "askrm": tbl = new Crystal.askrmDataTable(); break;
                case "asscp": tbl = new Crystal.asscpDataTable(); break;
                case "askprob": tbl = new Crystal.askprobDataTable(); break;
                case "asparam": tbl = new Crystal.asparamDataTable(); break;
                case "askprb": tbl = new Crystal.askprbDataTable(); break;
                case "mkrist": tbl = new Crystal.mkristDataTable(); break;
                case "asob_nal": tbl = new Crystal.asob_nalDataTable(); break;
                case "assbrn": tbl = new Crystal.assbrnDataTable(); break; 
                default: throw new InvalidCastException("Error TableRow");
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

        public static IEnumerable<T> GetTableSql<T>(bool useCache = false, bool isArchived = false, bool ShowDeleted = false, Plants plant = Plants.plant_none, string sql = "", params Object[] parameters) where T : global::System.Data.DataRow
        {
            Plants pl = GetPlant(plant);
            string plant_name = pl.ToString();
            Type t = typeof(T);
            string db_name = t.Name.Substring(0, t.Name.LastIndexOf("Row"));

            DataTable tbl;

            switch (db_name)
            {
                case "mpartn": tbl = new Crystal.mpartnDataTable(); break;
                case "asbrak": tbl = new Crystal.asbrakDataTable(); break;
                case "askor": tbl = new Crystal.askorDataTable(); break;
                case "asmnp": tbl = new Crystal.asmnpDataTable(); break;
                case "astob": tbl = new Crystal.astobDataTable(); break;
                case "asspr": tbl = new Crystal.assprDataTable(); break;
                case "assufl": tbl = new Crystal.assuflDataTable(); break;
                case "asmbn": tbl = new Crystal.asmbnDataTable(); break;
                case "askuch": tbl = new Crystal.askuchDataTable(); break;
                case "as_wop": tbl = new Crystal.as_wopDataTable(); break;
                case "mprxop": tbl = new Crystal.mprxopDataTable(); break;
                case "astmr": tbl = new Crystal.astmrDataTable(); break;
                case "asks": tbl = new Crystal.asksDataTable(); break;
                case "assop": tbl = new Crystal.assopDataTable(); break;
                case "asabon": tbl = new Crystal.asabonDataTable(); break;
                case "mpartt": tbl = new Crystal.mparttDataTable(); break;
                case "askrm": tbl = new Crystal.askrmDataTable(); break;
                case "asscp": tbl = new Crystal.asscpDataTable(); break;
                case "askprob": tbl = new Crystal.askprobDataTable(); break;
                case "asparam": tbl = new Crystal.asparamDataTable(); break;
                default: throw new InvalidCastException("Error TableRow");
            }

            if (isArchived)
                db_name = "a" + db_name;

            bool canCached = (isArchived && sql == "") || useCache;

            if (canCached)
            {
                Object data = HttpContext.Current.Cache.Get(plant_name + "_" + db_name);
                if (data != null) return (IEnumerable<T>)data;
            }

            using (IDbConnection dbConn = new OleDbConnection(GetConnection(pl)))
            {
                IDbCommand dbCmd = dbConn.CreateCommand();

                dbCmd.CommandText = sql;

                if (parameters != null)
                    foreach (var par in parameters)
                    {
                        dbCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("", par));
                    }

                if (ShowDeleted) dbConn.ShowDeleted();
                dbConn.Open();
                tbl.Load(dbCmd.ExecuteReader());
                dbConn.Close();
            }

            IEnumerable<T> res = (tbl as global::System.Data.TypedTableBase<T>).AsEnumerable().ToList();
            if (canCached)
                HttpContext.Current.Cache.Insert(plant_name + "_" + db_name, res, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(180));
            return res;
        }

        /// <summary>
        /// NoCacheSupport_AsyncCallsOnly_Plant_Needed
        /// Специальная реализация для асинхронных запросов
        /// </summary>
        public static IEnumerable<T> GetTableLite<T>(bool isArchived = false, bool ShowDeleted = false, Plants plant = Plants.plant_none, string filter = "", params Object[] parameters) where T : global::System.Data.DataRow
        {
            Plants pl = plant;
            string plant_name = pl.ToString();
            Type t = typeof(T);
            string db_name = t.Name.Substring(0, t.Name.LastIndexOf("Row"));

            DataTable tbl;

            switch (db_name)
            {
                case "mpartn": tbl = new Crystal.mpartnDataTable(); break;
                case "asbrak": tbl = new Crystal.asbrakDataTable(); break;
                case "askor": tbl = new Crystal.askorDataTable(); break;
                case "asmnp": tbl = new Crystal.asmnpDataTable(); break;
                case "astob": tbl = new Crystal.astobDataTable(); break;
                case "asspr": tbl = new Crystal.assprDataTable(); break;
                case "assufl": tbl = new Crystal.assuflDataTable(); break;
                case "asmbn": tbl = new Crystal.asmbnDataTable(); break;
                case "askuch": tbl = new Crystal.askuchDataTable(); break;
                case "as_wop": tbl = new Crystal.as_wopDataTable(); break;
                case "mprxop": tbl = new Crystal.mprxopDataTable(); break;
                case "astmr": tbl = new Crystal.astmrDataTable(); break;
                case "asks": tbl = new Crystal.asksDataTable(); break;
                case "assop": tbl = new Crystal.assopDataTable(); break;
                case "asabon": tbl = new Crystal.asabonDataTable(); break;
                case "mpartt": tbl = new Crystal.mparttDataTable(); break;
                case "askrm": tbl = new Crystal.askrmDataTable(); break;
                case "asscp": tbl = new Crystal.asscpDataTable(); break;
                case "askprob": tbl = new Crystal.askprobDataTable(); break;
                case "asparam": tbl = new Crystal.asparamDataTable(); break;
                default: throw new InvalidCastException("Error TableRow");
            }

            if (isArchived)
                db_name = "a" + db_name;

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
                tbl.Load(dbCmd.ExecuteReader());
                dbConn.Close();
            }

            IEnumerable<T> res = (tbl as global::System.Data.TypedTableBase<T>).AsEnumerable().ToList();

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
                if (dt_e.Month != dt_s.Month)
                    date_s = new DateTime(year: dt_e.Year, month: dt_e.Month, day: dt_e.Day, hour: 0, minute: 0, second: 0);
                else
                    date_s = new DateTime(year: dt_s.Year, month: dt_s.Month, day: dt_s.Day, hour: int.Parse(time_s.Substring(0, 2)), minute: int.Parse(time_s.Substring(2, 2)), second: 0);
            }
            return date_e.Subtract(date_s).TotalHours;
        }
        public static double HoursBetweenForStandEquipment(DateTime dt_s, string time_s,DateTime dt_e, string time_e,DateTime startmount)//dt_s,time_s-начало,dt_e,time_e-конец,startmount-начало месяца
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

        public static int WorkDaysBetween(DateTime start_date, DateTime end_date, IEnumerable<Crystal.askrmRow> calend)
        {
            return calend.Count(a => a.dm > start_date && a.dm <= end_date);
        }

        public static DateTime OneColDateTime(DateTime dato, String Timo)
        {
            DateTime DatoTimo;
            if (dato == DateTime.Parse("30/12/1899"))
            {
                DatoTimo = DateTime.Now;
            }
            else
            {
                DatoTimo = new DateTime(year: dato.Year, month: dato.Month, day: dato.Day, hour: int.Parse(Timo.Trim().Substring(0, 2)), minute: int.Parse(Timo.Trim().Substring(2, 2)), second: 0);
            }
            return DatoTimo;
        }

        public static string GetFioByTabNumber(string tn, IEnumerable<Crystal.asabonRow> abons = null)
        {
            if (tn.Trim() == "") return "";
            if (abons == null) abons = FoxRepo.GetTable<Crystal.asabonRow>();
            var abonent = abons.Where(a => a.tbn == tn);
            return abonent.Count() > 0 ? abonent.First().fio : "уволен (" + tn + ")";
        }

        public static int WorkDaysInMonth(DateTime dt)
        {
            DateTime date_e = new DateTime(year: dt.Year, month: dt.Month, day: DateTime.DaysInMonth(dt.Year, dt.Month));
            DateTime date_s = new DateTime(year: dt.Year, month: dt.Month, day: 1);
            return WorkDaysBetween(date_s, date_e, GetTable<Crystal.askrmRow>());
            //return FoxRepo.GetTable<Crystal.askrmRow>().Count(a => a.dm > date_s && a.dm < date_e);
        }

        public static string GetFoxTime(string tim)
        {
            return tim != "    " ? tim.Substring(0, 2) + ":" + tim.Substring(2, 2) : "";//dt.isNullDate() ? "" : dt.ToShortDateString();// dt == DateTime.Parse("30/12/1899") ? true : false;
        }

        public static string GetProductionNumber()
        {
            return GetTable<Crystal.asparamRow>().First(x => x.identp.Trim() == "FACTORY").valp.Trim();
        }
        public static DateTime GetStartMount()
        {
            var startmount = from asparam in FoxRepo.GetTable<Crystal.asparamRow>()
                             where asparam.identp.Trim() == "start_ms"
                             select new
                             {
                                 Time = DateTime.ParseExact(asparam.valp.Trim(), "dd/MM/yyyy HHmm", System.Globalization.CultureInfo.InvariantCulture)

                             };
            return startmount.Single().Time;
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
            return dt == DateTime.Parse("30/12/1899") ? true : false;
        }

        public static string GetFoxDate(this DateTime dt)
        {
            return dt.isNullDate() ? "" : dt.ToShortDateString();// dt == DateTime.Parse("30/12/1899") ? true : false;
        }

        public static string GetFoxTime(this string time)
        {
            return time != "    " ? time.Insert(2, ":") : "";
        }
    }
}