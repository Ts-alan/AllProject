using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using VirusBlokAda.CC.DataBase;

/// <summary>
/// Summary description for DeviceDataContainer
/// </summary>
public static class DeviceDataContainer
{
    public static List<Device> Get(String where, String device_type, String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        List<Device> list = new List<Device>();
        if (maximumRows < 1) return list;

        String orderBy = "SerialNo ASC";
        String[] parts = SortExpression.Split(' ');
        Boolean descending = false;

        if (parts.Length > 0 && parts[0] != "")
        {
            if (parts.Length > 1)
            {
                descending = parts[1].ToLower().Contains("desc");
            }
            System.Reflection.PropertyInfo prop = typeof(Device).GetProperty(parts[0]);
            if (prop == null)
            {
                throw new Exception("No property '" + parts[0] + "' in Device");
            }

            orderBy = String.Format("{0} {1}", parts[0], descending ? "DESC" : "ASC");
        }

        list = DBProviders.Policy.GetDevicesList((Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows, String.Format("TypeName='{0}'{1}", device_type, !String.IsNullOrEmpty(where) ? String.Format(" AND ({0})", where) : ""), orderBy);

        return list;
    }

    public static Int32 Count(String where, String device_type)
    {
        Int32 count = 0;

        count = DBProviders.Policy.GetDeviceCount(String.Format("TypeName='{0}'{1}", device_type, !String.IsNullOrEmpty(where) ? String.Format(" AND ({0})", where) : ""));

        return count;
    }

    public static List<DevicePolicy> GetUnknown(String where, String device_type, String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        List<DevicePolicy> list = new List<DevicePolicy>();
        if (maximumRows < 1) return list;

        if (DeviceTypeExtensions.Get(device_type) != DeviceType.USB)
            return list;
        
        String orderBy = "SerialNo ASC";
        String[] parts = SortExpression.Split(' ');
        Boolean descending = false;

        if (parts.Length > 0 && parts[0] != "")
        {
            if (parts.Length > 1)
            {
                descending = parts[1].ToLower().Contains("desc");
            }
            //System.Reflection.PropertyInfo prop = typeof(DevicePolicy).GetProperty(parts[0]);
            //if (prop == null)
            //{
            //    throw new Exception("No property '" + parts[0] + "' in DevicePolicy");
            //}

            orderBy = String.Format("{0} {1}", parts[0], descending ? "DESC" : "ASC");
        }

        list = DBProviders.Policy.GetUnknownDevicesList((Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows, where, orderBy);

        return list;
    }

    public static object FollowPropertyPath(Type currentType, string path)
    {
       /* Type currentType = value.GetType();*/
        System.Reflection.PropertyInfo property = null;
        foreach (string propertyName in path.Split('.'))
        {
             property = currentType.GetProperty(propertyName);
            currentType = property.PropertyType;
            /*value = property.GetValue(value, null);*/
            
        }
        return property;
    }

    public static Int32 CountUnknown(String where, String device_type)
    {
        Int32 count = 0;

        if (DeviceTypeExtensions.Get(device_type) != DeviceType.USB)
            return count;

        count = DBProviders.Policy.GetUnknownDeviceCount(where);

        return count;
    }
}