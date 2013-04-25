using System;
using System.Collections.Generic;
using System.Web;
using ARM2_dbcontrol.DataBase;
using System.Configuration;

/// <summary>
/// Summary description for ComponentsDataContainer
/// </summary>

public static class ComponentsDataContainer
{
    public static List<ComponentsEntity> Get(String where, String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        List<ComponentsEntity> list = new List<ComponentsEntity>();
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
            System.Reflection.PropertyInfo prop = typeof(ComponentsEntity).GetProperty(parts[0]);
            if (prop == null)
            {
                throw new Exception("No property '" + parts[0] + "' in ComponentsEntity");
            }

            orderBy = String.Format("{0} {1}", parts[0], descending ? "DESC" : "ASC");
        }

        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            ComponentsManager db = new ComponentsManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            list = db.List(where, orderBy, (Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows);

            conn.CloseConnection();
        }

        for (Int32 i = 0; i < list.Count; i++)
        {
            list[i].ComponentState = DatabaseNameLocalization.GetNameForCurrentCulture(list[i].ComponentState);
        }

        return list;
    }

    public static Int32 Count(String where)
    {
        Int32 count = 0;

        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            ComponentsManager db = new ComponentsManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            count = db.Count(where);

            conn.CloseConnection();
        }

        return count;
    }

    public static List<String> GetTypes()
    {
        List<String> list = new List<String>();
        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            ComponentsManager db = new ComponentsManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            foreach (ComponentsEntity ent in db.ListComponentType())
            {
                list.Add(ent.ComponentName);
            }

            conn.CloseConnection();
        }

        return list;
    }

    public static List<String> GetStates()
    {
        List<String> list = new List<String>();
        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            ComponentsManager db = new ComponentsManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            foreach (ComponentsEntity ent in db.ListComponentState())
            {
                list.Add(ent.ComponentState);
            }

            conn.CloseConnection();
        }

        return list;
    }
}