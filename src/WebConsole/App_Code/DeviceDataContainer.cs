using System;
using System.Collections.Generic;
using System.Web;
using VirusBlokAda.Vba32CC.Policies.Devices;
using ARM2_dbcontrol.DataBase;
using System.Configuration;
using VirusBlokAda.Vba32CC.Policies;
using VirusBlokAda.Vba32CC.Policies.Devices.Policy;

/// <summary>
/// Summary description for DeviceDataContainer
/// </summary>
public static class DeviceDataContainer
{
    public static List<Device> Get(String where, String SortExpression, Int32 maximumRows, Int32 startRowIndex)
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

        list = PoliciesState.GetDevicesList((Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows, where, orderBy);

        return list;
    }

    public static Int32 Count(String where)
    {
        Int32 count = 0;

        count = PoliciesState.GetDeviceCount(where);

        return count;
    }
    public static List<DevicePolicy> GetUnknown(String where, String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        List<DevicePolicy> list = new List<DevicePolicy>();
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
            //System.Reflection.PropertyInfo prop = typeof(DevicePolicy).GetProperty(parts[0]);
            //if (prop == null)
            //{
            //    throw new Exception("No property '" + parts[0] + "' in DevicePolicy");
            //}

            orderBy = String.Format("{0} {1}", parts[0], descending ? "DESC" : "ASC");
        }

        list = PoliciesState.GetUnknownDevicesList((Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows, where, orderBy);

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
    public static Int32 CountUnknown(String where)
    {
        Int32 count = 0;

        count = PoliciesState.GetUnknownDeviceCount(where);

        return count;
    }
    public static PolicyProvider PoliciesState
    {
        get
        {
            PolicyProvider provider = HttpContext.Current.Application["PoliciesState"] as PolicyProvider;
            if (provider == null)
            {
                provider = new PolicyProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Application["PoliciesState"] = provider;
            }

            return provider;
        }
    }

}