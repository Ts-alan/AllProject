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
    /// ��������� ������ ������� ���������
    /// </summary>
    /// <param name="where">������� ���������</param>
    /// <param name="SortExpression">��������� ����������</param>
    /// <param name="maximumRows">������������ ���������� �����</param>
    /// <param name="startRowIndex">������ ���������� ����</param>
    /// <returns>������ ������� ���������</returns>
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
    /// ������� ���������� ������� ���������
    /// </summary>
    /// <param name="where">������� ���������</param>
    /// <returns>���������� ������� ���������</returns>
    public static Int32 Count(String where)
    {
        return DBProviders.InstallationTask.Count(where);
    }
    /// <summary>
    /// ��������� ������ �������� ������� ���������
    /// </summary>
    /// <returns>������ �������� ������� ���������</returns>
    public static List<String> GetStatuses()
    {
        return DBProviders.InstallationTask.GetStatuses();
    }
}