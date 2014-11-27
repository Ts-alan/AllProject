using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using VirusBlokAda.CC.DataBase;

/// <summary>
/// Summary description for DeviceClassDataContainer
/// </summary>
public static class DeviceClassDataContainer
{
    /// <summary>
    /// Получение списка классов устройств
    /// </summary>
    /// <param name="where">условие получения</param>
    /// <param name="SortExpression">выражение сортировки</param>
    /// <param name="maximumRows">максимальное количество рядов</param>
    /// <param name="startRowIndex">индекс начального ряда</param>
    /// <returns>список классов устройств</returns>
    public static List<DeviceClass> Get(String where, String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        List<DeviceClass> list = new List<DeviceClass>();
        if (maximumRows < 1) return list;

        String orderBy = "ClassName ASC";
        String[] parts = SortExpression.Split(' ');
        Boolean descending = false;

        if (parts.Length > 0 && parts[0] != "")
        {
            if (parts.Length > 1)
            {
                descending = parts[1].ToLower().Contains("desc");
            }
            System.Reflection.PropertyInfo prop = typeof(DeviceClass).GetProperty(parts[0]);
            if (prop == null)
            {
                throw new Exception("No property '" + parts[0] + "' in Device");
            }

            orderBy = String.Format("{0} {1}", parts[0], descending ? "DESC" : "ASC");
        }

        list = DBProviders.Policy.GetDeviceClassListByFilter(where, orderBy, (Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows);

        return list;
    }
    /// <summary>
    /// подсчет количества классов устройств
    /// </summary>
    /// <param name="where">условие получения</param>
    /// <returns>количество классов устройств</returns>
    public static Int32 Count(String where)
    {
        return DBProviders.Policy.GetDeviceClassCount(where);
    }
}