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

        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            ProcessesManager db = new ProcessesManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            list = db.List(where, orderBy, (Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows);

            conn.CloseConnection();
        }

        return list;
    }

    public static Int32 Count(String where)
    {
        Int32 count = 0;

        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            ProcessesManager db = new ProcessesManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            count = db.Count(where);

            conn.CloseConnection();
        }

        return count;
    }
}