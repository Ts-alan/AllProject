using System;
using System.Collections.Generic;
using System.Web;
using VirusBlokAda.CC.DataBase;
using System.Configuration;

/// <summary>
/// Summary description for ComponentsDataContainer
/// </summary>

public static class ComponentsDataContainer
{
    /// <summary>
    /// ѕолучение списка компонентов
    /// </summary>
    /// <param name="where">условие получени€</param>
    /// <param name="SortExpression">выражение сортировки</param>
    /// <param name="maximumRows">максимальное количество р€дов</param>
    /// <param name="startRowIndex">индекс начального р€да</param>
    /// <returns>список компонентов</returns>
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

        list = DBProviders.Component.List(where, orderBy, (Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows);

        for (Int32 i = 0; i < list.Count; i++)
        {
            list[i].ComponentState = DatabaseNameLocalization.GetNameForCurrentCulture(list[i].ComponentState);
        }

        return list;
    }

    /// <summary>
    /// подсчет количества компонентов
    /// </summary>
    /// <param name="where">условие получени€</param>
    /// <returns></returns>
    public static Int32 Count(String where)
    {
        return DBProviders.Component.Count(where);
    }
    /// <summary>
    /// получение типов компонентов
    /// </summary>
    /// <returns></returns>
    public static List<String> GetTypes()
    {
        List<String> list = new List<String>();
        foreach (ComponentsEntity ent in DBProviders.Component.ListComponentType())
        {
            list.Add(ent.ComponentName);
        }

        return list;
    }
    /// <summary>
    ///  получение состо€ний компонентов
    /// </summary>
    /// <returns></returns>
    public static List<String> GetStates()
    {
        List<String> list = new List<String>();
        foreach (ComponentsEntity ent in DBProviders.Component.ListComponentState())
        {
            list.Add(ent.ComponentState);
        }

        return list;
    }
}