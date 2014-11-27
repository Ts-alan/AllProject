using System;
using System.Collections.Generic;
using System.Web;
using VirusBlokAda.CC.DataBase;
using System.Configuration;

/// <summary>
/// Summary description for InstallTasksDataContainer
/// </summary>

public static class InstallTasksDataContainer
{
    /// <summary>
    /// Получение списка заданий установок
    /// </summary>
    /// <param name="where">условие получения</param>
    /// <param name="SortExpression">выражение сортировки</param>
    /// <param name="maximumRows">максимальное количество рядов</param>
    /// <param name="startRowIndex">индекс начального ряда</param>
    /// <returns>список заданий установок</returns>
    public static List<InstallationTaskEntity> Get(String where, String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        if (maximumRows < 1) 
            return new List<InstallationTaskEntity>();

        String orderBy = "InstallationDate DESC";
        String[] parts = SortExpression.Split(' ');
        Boolean descending = false;

        if (parts.Length > 0 && parts[0] != "")
        {
            if (parts.Length > 1)
            {
                descending = parts[1].ToLower().Contains("desc");
            }
            System.Reflection.PropertyInfo prop = typeof(InstallationTaskEntity).GetProperty(parts[0]);
            if (prop == null)
            {
                throw new Exception("No property '" + parts[0] + "' in InstallationTaskEntity");
            }

            orderBy = String.Format("{0} {1}", parts[0], descending ? "DESC" : "ASC");
        }

        List<InstallationTaskEntity> list = DBProviders.InstallationTask.List(where, orderBy, (Int16)((Int32)((Double)startRowIndex / (Double)maximumRows) + 1), (Int16)maximumRows);
        for (Int32 i = 0; i < list.Count; i++)
        {
            list[i].Status = DatabaseNameLocalization.GetNameForCurrentCulture(list[i].Status);
        }
        return list;
    }
    /// <summary>
    /// подсчет количества заданий установок
    /// </summary>
    /// <param name="where">условие получения</param>
    /// <returns>количество заданий установок</returns>
    public static Int32 Count(String where)
    {
        return DBProviders.InstallationTask.Count(where);
    }
    /// <summary>
    /// получение списка статусов заданий установок
    /// </summary>
    /// <returns>список статусов заданий установок</returns>
    public static List<String> GetStatuses()
    {
        return DBProviders.InstallationTask.GetStatuses();
    }
}