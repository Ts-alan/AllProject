using System;
using System.Collections.Generic;
using System.Web;
using VirusBlokAda.CC.DataBase;
using System.Configuration;

/// <summary>
/// Summary description for DummyDataContainer
/// </summary>

public static class ProcessDataContainer
{
    public static List<ProcessesEntity> Get(String where, String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        List<ProcessesEntity> list = new List<ProcessesEntity>();
        if (maximumRows < 1) return list;

        String orderBy = "ComputerName ASC";
        String[] parts = SortExpression.Split(' ');
        Boolean descending = false;

        if (parts.Length > 0 && parts[0] != "")
        {
            if (parts.Length > 1)
            {
                descending = parts[1].ToLower().Contains("desc");
            }
            System.Reflection.PropertyInfo prop = typeof(ProcessesEntity).GetProperty(parts[0]);
            if (prop == null)
            {
                throw new Exception("No property '" + parts[0] + "' in ProcessesEntity");
            }

            orderBy = String.Format("{0} {1}", parts[0], descending ? "DESC" : "ASC");
        }

        ProcessProvider db = new ProcessProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        list = db.List(where, orderBy, (Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows);

        return list;
    }

    public static Int32 Count(String where)
    {
        ProcessProvider db = new ProcessProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        return db.Count(where);
    }
}