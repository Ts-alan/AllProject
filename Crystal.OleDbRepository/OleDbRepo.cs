using Crystal.BusinessLogic;
using CrystalDataSetLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using System.Web.Mvc;
namespace Crystal.VFPOleDBLogic
{
    public class VFPOleDbRepo : CrystalRepository
    {
        MemoryCache memory = MemoryCache.Default;

        public IEnumerable<T> GetTable<T>(bool useCache = false, bool isArchived = false, bool ShowDeleted = false, Plants plant = Plants.plant_none, string filter = "", params Object[] parameters) where T : global::System.Data.DataRow {
            Plants pl = ((plant == Plants.plant_none) ? RepoPlant : plant);
            string plant_name = pl.ToString();
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
                case "mrest": tbl = new CrystalDS.mrestDataTable(); break;
                case "mcompl": tbl = new CrystalDS.mcomplDataTable(); break;
                default: throw new InvalidCastException(string.Format("FoxRepo.GetTable don't know {0}", db_name));
            }

            if (isArchived)
                db_name = "a" + db_name;

            bool canCached = (isArchived && filter == "") || useCache;

            if (canCached)
            {
                //Object data = HttpContext.Current.Cache.Get(plant_name + "_" + db_name);
                memory.Set(db_name.ToString(), db_name, null, null);
                var data = memory.Get(plant_name + "_" + db_name);
                if (data != null) return (IEnumerable<T>)data;
            }


            using (IDbConnection dbConn = GetConnection(pl))
            {
                IDbCommand dbCmd = dbConn.CreateCommand();

                //string cols = "";

                //for (int i = 0; i < tbl.Columns.Count; i++)
                //{
                //    cols += (String.IsNullOrEmpty(cols) ? " " : ", ") + tbl.Columns[i];
                //}

                //string selectCommand = "select" + cols + " from " + db_name;

                string selectCommand = "select * from " + db_name;

                if (filter != "")
                {
                    selectCommand += (" " + filter);
                }
                dbCmd.CommandText = selectCommand;

                if (parameters != null)
                    foreach (var par in parameters)
                    {
                        //if (par is String)
                        //{
                        //    dbCmd.Parameters.AddWithValue("", "[" + par + "]");
                        //}
                        //else
                            dbCmd.Parameters.Add(new OleDbParameter("", par));
                    }

                //if (ShowDeleted) dbConn.ShowDeleted();
                dbConn.Open();
                //if (db_name == "amprxop")
                //{
                //    var cmd = new OleDbCommand("EXECSCRIPT([USE amprxop IN 0]+CHR(13)+CHR(10)+[SET ORDER TO TAG NPRT IN amprxop])" ,dbConn);
                //    //cmd.CommandText = "EXECSCRIPT(SET ORDER TO 1)";
                //    cmd.ExecuteNonQuery();
                //}
                tbl.Load(dbCmd.ExecuteReader(), LoadOption.OverwriteChanges, (s, e) => { e.Continue = true; });

                dbConn.Close();
            }

            IEnumerable<T> res = (tbl as global::System.Data.TypedTableBase<T>).AsEnumerable().ToList();
            if (canCached)
                memory.Set(plant_name + "_" + db_name, res, new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(180) });
            //HttpContext.Current.Cache.Insert(plant_name + "_" + db_name, res, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(180));
            return res;
        }


        public IDbConnection GetConnection(Plants plant) {
            string expected_ConnectionString = plant.ToString().ToUpper() + CONNECTION_STRING;
            System.Configuration.ConnectionStringSettings connectionStringSettings = System.Configuration.ConfigurationManager.ConnectionStrings[expected_ConnectionString];
            if (connectionStringSettings == null)
            {
                throw new System.Configuration.ConfigurationErrorsException(string.Format("Error ConnectionString in Web.config\nExpected: {0}", expected_ConnectionString));
            }
            return new OleDbConnection(connectionStringSettings.ConnectionString);
        }
        public string GetFioByTabNumber(string tn, IEnumerable<CrystalDS.asabonRow> abons = null) {
            if (tn.Trim() == "") return "";
            if (abons == null) abons = GetTable<CrystalDS.asabonRow>();
            var abonent = abons.Where(a => a.tbn == tn);
            return abonent.Count() > 0 ? abonent.First().fio : "уволен (" + tn + ")";
        }


        public int WorkDaysBetween(DateTime? start_date, DateTime? end_date, IEnumerable<CrystalDS.askrmRow> calend) {
            return calend.Count(a => a.dm > start_date && a.dm <= end_date);
        }



    }
}
