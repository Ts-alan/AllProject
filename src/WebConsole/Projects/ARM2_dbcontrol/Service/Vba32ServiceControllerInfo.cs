using System;
using System.Data;
using System.Configuration;

using System.ServiceProcess;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using System.Diagnostics;

namespace Vba32.ControlCenter.Service
{
    /// <summary>
    /// ���������� ����� ��������� ��� "��� ������� - ���������"
    /// </summary>
    public class Vba32ServiceControllerInfo
    {
        /// <summary>
        /// ���������� ����� ��������� ��� "��� ������� - ���������" ��������� ���������� ���������
        /// </summary>
        /// <param name="regexp">���������� ���������</param>
        /// <param name="upperNames">������������� � ������������� �������������� ��� ������ ��� �������� � ������� �������</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetServicesInfo(string regexp, bool upperNames)
        {            
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
            dic = new Dictionary<string, string>();
            foreach (ServiceController service in ServiceController.GetServices())
            {
                string serviceName = upperNames ? service.ServiceName.ToUpper() :
                    service.ServiceName;
                Regex ex = new Regex(regexp);
                Match m = ex.Match(serviceName);
                if (m.Success)
                    dic.Add(service.DisplayName, service.Status.ToString());
            }
        }
        catch (Exception ex)
        {
            Debug.Write("Vba32ServiceControllerInfo.GetServicesInfo(): Error:"+ex.Message);
        }

            return dic;
        }

        /// <summary>
        /// ���������� ����� ��������� ��� "��� ������� - ���������" ��������� ��������� ��� ������
        /// </summary>
        /// <param name="names">��������� ���� ��������</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetServicesInfo(List<string> names)
        {
            ARM2_dbcontrol.Generation.Logger logger = new ARM2_dbcontrol.Generation.Logger("C:\\Program Files\\Vba32 Control Center\\WebConsole\\Settings\\vba32services.log");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
            foreach (ServiceController service in ServiceController.GetServices())
            {
                logger.Write(String.Format("{0};  ", service.DisplayName));
                if (names.Contains(service.ServiceName))
                {
                    logger.Write(String.Format("{0} is including.", service.DisplayName));
                    dic.Add(service.DisplayName, service.Status.ToString());
                }
            }
            }
            catch (Exception ex)
            {
                Debug.Write("Vba32ServiceControllerInfo.GetServicesInfo(): Error:" + ex.Message);
                logger.Write(ex.Message);
            }
            return dic;

        }

        /// <summary>
        /// ���������� ��������� ������� �� ��� �����
        /// </summary>
        /// <param name="name">��� �������</param>
        /// <returns>��������� �������</returns>
        public static string GetServiceState(string name)
        {
            try
            {
                foreach (ServiceController service in ServiceController.GetServices())
                {
                    if (name == service.ServiceName)
                        return service.Status.ToString();

                }
            }
            catch (Exception ex)
            {
                Debug.Write("Vba32ServiceControllerInfo.GetServicesInfo(): Error:" + ex.Message);
            }
            return String.Empty;
        }
    }
}
