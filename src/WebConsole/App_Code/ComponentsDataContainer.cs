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
    /// ��������� ������ �����������
    /// </summary>
    /// <param name="where">������� ���������</param>
    /// <param name="SortExpression">��������� ����������</param>
    /// <param name="maximumRows">������������ ���������� �����</param>
    /// <param name="startRowIndex">������ ���������� ����</param>
    /// <returns>������ �����������</returns>
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
    /// ������� ���������� �����������
    /// </summary>
    /// <param name="where">������� ���������</param>
    /// <returns></returns>
    public static Int32 Count(String where)
    {
        return DBProviders.Component.Count(where);
    }
    /// <summary>
    /// ��������� ����� �����������
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
    ///  ��������� ��������� �����������
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