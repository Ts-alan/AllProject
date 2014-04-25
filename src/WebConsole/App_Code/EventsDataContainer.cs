using System;
using System.Collections.Generic;
using System.Web;
using VirusBlokAda.CC.DataBase;
using System.Configuration;

/// <summary>
/// Summary description for EventsDataContainer
/// </summary>

public static class EventsDataContainer
{
    public static List<EventsEntity> Get(String where, String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        List<EventsEntity> list = new List<EventsEntity>();
        if (maximumRows < 1) return list;

        String orderBy = "EventTime DESC";
        String[] parts = SortExpression.Split(' ');
        Boolean descending = false;

        if (parts.Length > 0 && parts[0] != "")
        {
            if (parts.Length > 1)
            {
                descending = parts[1].ToLower().Contains("desc");
            }
            System.Reflection.PropertyInfo prop = typeof(EventsEntity).GetProperty(parts[0]);
            if (prop == null)
            {
                throw new Exception("No property '" + parts[0] + "' in EventsEntity");
            }

            orderBy = String.Format("{0} {1}", parts[0], descending ? "DESC" : "ASC");
        }

        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            EventsManager db = new EventsManager(conn);
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
            EventsManager db = new EventsManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            count = db.Count(where);

            conn.CloseConnection();
        }

        return count;
    }

    #region Notification

    public static List<EventTypesEntity> GetForNotification(String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        List<EventTypesEntity> list = new List<EventTypesEntity>();
        if (maximumRows < 1) return list;

        String orderBy = "EventName DESC";
        String[] parts = SortExpression.Split(' ');
        Boolean descending = false;

        if (parts.Length > 0 && parts[0] != "")
        {
            if (parts.Length > 1)
            {
                descending = parts[1].ToLower().Contains("desc");
            }
            System.Reflection.PropertyInfo prop = typeof(EventTypesEntity).GetProperty(parts[0]);
            if (prop == null)
            {
                throw new Exception("No property '" + parts[0] + "' in EventsEntity");
            }

            orderBy = String.Format("{0} {1}", parts[0], descending ? "DESC" : "ASC");
        }

        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            EventTypesManager db = new EventTypesManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            list = db.List("EventName like '%'", orderBy, (Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows);

            conn.CloseConnection();
        }

        return list;
    }

    public static Int32 CountForNotification()
    {
        Int32 count = 0;

        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            EventTypesManager db = new EventTypesManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            count = db.Count("EventName like '%'");

            conn.CloseConnection();
        }

        return count;
    }

    public static Boolean UpdateNotify(EventTypesEntity ent)
    {
        Boolean isSuccess = false;
        using (VlslVConnection conn = new VlslVConnection(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            EventTypesManager db = new EventTypesManager(conn);

            conn.OpenConnection();
            conn.CheckConnectionState(true);
            
            db.UpdateNotify(ent);
            isSuccess = true;
            conn.CloseConnection();
        }

        return isSuccess;
    }

    #endregion
}