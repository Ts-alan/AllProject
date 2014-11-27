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
    /// <summary>
    /// Получение списка событий
    /// </summary>
    /// <param name="where">условие получения</param>
    /// <param name="SortExpression">выражение сортировки</param>
    /// <param name="maximumRows">максимальное количество рядов</param>
    /// <param name="startRowIndex">индекс начального ряда</param>
    /// <returns>список событий</returns>
    public static List<EventsEntity> Get(String where, String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        if (maximumRows < 1) 
            return new List<EventsEntity>();

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

        return DBProviders.Event.List(where, orderBy, (Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows);
    }
    /// <summary>
    /// подсчет количества событий
    /// </summary>
    /// <param name="where">условие получения</param>
    /// <returns>количество событий</returns>
    public static Int32 Count(String where)
    {
        return DBProviders.Event.Count(where);
    }

    #region Notification
    /// <summary>
    /// Получение списка типов событий
    /// </summary>
    /// <param name="where">условие получения</param>
    /// <param name="SortExpression">выражение сортировки</param>
    /// <param name="maximumRows">максимальное количество рядов</param>
    /// <param name="startRowIndex">индекс начального ряда</param>
    /// <returns>список типов событий</returns>
    public static List<EventTypesEntity> GetForNotification(String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        if (maximumRows < 1) 
            return new List<EventTypesEntity>();

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

        return DBProviders.Event.GetEventTypeList("EventName like '%'", orderBy, (Int16)((Int32)((Double)startRowIndex / (Double)maximumRows) + 1), (Int16)maximumRows);
    }
    /// <summary>
    /// подсчет количества типов событий
    /// </summary>
    /// <param name="where">условие получения</param>
    /// <returns>количество типов событий</returns>
    public static Int32 CountForNotification()
    {
        return DBProviders.Event.GetEventTypesCount("EventName like '%'");
    }

    public static Boolean UpdateNotify(EventTypesEntity ent)
    {
        Boolean isSuccess = false;
        try
        {
            DBProviders.Event.UpdateNotify(ent);
            isSuccess = true;
        }
        catch { }

        return isSuccess;
    }

    #endregion
}