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
    /// <summary>
    /// Получение списка процессов
    /// </summary>
    /// <param name="where">условие получения</param>
    /// <param name="SortExpression">выражение сортировки</param>
    /// <param name="maximumRows">максимальное количество рядов</param>
    /// <param name="startRowIndex">индекс начального ряда</param>
    /// <returns>список процессов</returns>
    public static List<ProcessesEntity> Get(String where, String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        if (maximumRows < 1) 
            return new List<ProcessesEntity>();

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

        return DBProviders.Process.List(where, orderBy, (Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows);
    }
    /// <summary>
    /// подсчет количества процессов
    /// </summary>
    /// <param name="where">условие получения</param>
    /// <returns>количество процессов</returns>
    public static Int32 Count(String where)
    {
        return DBProviders.Process.Count(where);
    }
}