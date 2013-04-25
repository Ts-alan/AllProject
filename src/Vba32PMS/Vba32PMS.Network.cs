using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Net;
using System.Diagnostics;

using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;
using Vba32.ControlCenter.PeriodicalMaintenanceService.DataBase;
using Vba32.ControlCenter.PeriodicalMaintenanceService.Network;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService
{
    /// <summary>
    /// ������ � �����
    /// </summary>
    partial class Vba32PMS
    {
        private int maxCountToSend = 200;

        /// <summary>
        /// �������� ������� �� ������ ������������ ���������
        /// </summary>
        /// <returns></returns>
        private bool SendEventsFromFiles()
        {
            bool retVal = true;
            try
            {
                LogMessage("SendEventsFromFiles::�������� ��� ������� �� ������");
                LogMessage("����� ������..");
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles(filePrefix + "*.xml"))
                {
                    if (isShutdown) return false;
                    LogMessage("������ ����: " + file.FullName + ". ������ " + file.Length);
                    if (!SendAllEventsFromFile(file.FullName))
                        retVal = false;
                }
            }
            catch (Exception ex)
            {
                LogError("SendAllEventsFromFile()::" + ex.Message,
                    EventLogEntryType.Error);

                return false;
            }
            return retVal;

        }

        /// <summary>
        /// �������� ��� ������� �� ����� ��������
        /// </summary>
        /// <param name="fileName"></param>
        private bool SendAllEventsFromFile(string fileName)
        {
            bool retVal = false;
            try
            {
                LogMessage("SendAllEventsFromFile::�������� ��� ������� �������� �� ����� "+fileName);

                //LogMessage("��������� ������� ����� � ������� �� �����. ");
                if (!File.Exists(fileName))
                {
                    LogError("SendAllEventsFromFile()::�� ������ ����: " + fileName,
                            EventLogEntryType.Error);
                    return false;
                }

                //��������, � ����� ������ ������������������, ����� ���� �� ������������
                //����� Dictionary<Tkey,TValue>
                LogMessage("�������������");
                List<EventsEntity> list = new List<EventsEntity>();
                try
                {
                    //�3 ����� ������ ����� ���������� ��� ��������������, �� � �����, ��� �����
                    //��� �� ������������. ��������� �� ����� �� ����� ������� � ���� ������
                    //�� ����� ��� ������ ������� � ���������� false
                    list = ObjectSerializer.XmlStrToObj<List<EventsEntity>>(fileName);
                }
                catch(Exception ex)
                {
                    LogError("SendAllEventsFromFile():: ������ ��� ������� ��������������� ������ �� �����. " + ex.Message + " " + ex.GetType().ToString(),
                   EventLogEntryType.Error);

                    File.Delete(fileName);

                    return false;
                }

                if (list.Count == 0)
                {
                    LogMessage("������� �� ���������");
                    return true;
                }

                int itemsCount = maxCountToSend;
                while (list.Count > 0)
                {
                    LogMessage("������� ������ ���������: " + list.Count);
                    LogMessage("������� ������ ���������� �����������: " + itemsCount);
                    if (list.Count < itemsCount)
                    {
                        LogMessage("������ ��������� ������ ���������� �����������. ����������..");
                        itemsCount = list.Count;
                    }

                    LogMessage("������� ������ ������");
                    EventsToControlAgentXml ggg = new EventsToControlAgentXml(list.GetRange(0, itemsCount), machineName);
                    LogMessage("���������� �����");
                    string packet = ggg.Convert();
                    if (packet == String.Empty)
                    {
                        LogMessage("����� ������ �����������.. ���������..");
                        itemsCount-=20;
                        if (itemsCount < 0) itemsCount = 1; //������������, �� ��� ��
                        continue;
                    }

                    LogMessage("��������...");
                    string dataSend = EventsSender.SocketSendReceive(server, port, packet);

                    if (dataSend != "OK")
                    {
                        LogMessage("�� ���������� ��������: " + dataSend);
                        break;
                    }
                    else
                    {
                        Debug.Write("��������. ��������� ���� ��������� �������� �������");
                        lastSendDate = DateTime.Now;
                        WriteSettingsToRegistry();
                    }

                    LogMessage("������� ������");
                    list.RemoveRange(0, itemsCount);
                    //!--
                    //������. ���������� ������ ������ ���� ������������� � ����
                    if (isShutdown)
                        break;


                    itemsCount = maxCountToSend;
                }

                LogMessage("������� ��������� �� ����� " + fileName + " ���������. ������� ������ ���������: " + list.Count.ToString());
                if (list.Count == 0)
                {
                    retVal = true;
                    LogMessage("��� ������� �� ����� " + fileName + " ��������. ������� ���.");
                    File.Delete(fileName);
                    if (File.Exists(fileName))
                    {
                        LogError("SendAllEventsFromFile()::���� �� ��������! �������� ��������� �������� ���������!",
                            EventLogEntryType.Error);
                    }
                }
                else
                {
                    LogMessage("�������� ������� ��� �������. �������� � ��� �� ����.");
                    ObjectSerializer.ObjToXmlStr(fileName, list);
                    if (!File.Exists(fileName))
                    {
                        LogError("SendAllEventsFromFile()::����� ����� ������������ � ��������� ���!",
                            EventLogEntryType.Error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("SendAllEventsFromFile()::" + ex.Message +" "+ex.GetType().ToString(),
                    EventLogEntryType.Error);

                return false;
            }
            return retVal;
        }

        /// <summary>
        /// �������� �����, ������� ��������� ����������, ��� ���� ���� �������� �������� CC
        /// </summary>
        /// <returns></returns>
        private bool SendSystemInfo()
        {
            try
            {
                LogMessage("Vba32PMS.SendSystemInfo::�������� ����� � ������������� CC ������");

                if (ipAddress == String.Empty)
                    ipAddress = GetIP(machineName);
                StringBuilder build = new StringBuilder(128);
                build.AppendFormat("<SystemInfo><ComputerName>{0}</ComputerName><IPAddress>{1}</IPAddress><ControlCenter>true</ControlCenter></SystemInfo>",
                                    machineName, ipAddress);
                
                //string packet = "<SystemInfo><ComputerName>" + machineName + "</ComputerName>" +
                //    "<ControlCenter>true</ControlCenter>" +
                //    "</SystemInfo>";


                string dataSend = EventsSender.SocketSendReceive(server, port, build.ToString());
                if (dataSend != "OK")
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.SendSystemInfo()::" + ex.Message,
                    EventLogEntryType.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// ������������ ��� ��������� IP-������ ����������, �� ������� �����������
        /// </summary>
        /// <param name="host">NETBIOS ���. ���� �������� String.Empty 
        /// ������� ��� ����������. �� ���� ����� �� Win 2003 Server</param>
        /// <returns></returns>
        private string GetIP(string host)
        {
            try
            {
                foreach (IPAddress ip in Dns.GetHostAddresses(host))
                {
                    if (((ip != IPAddress.Any) || (ip != IPAddress.Loopback)) &&
                        (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                        return ip.ToString();

                }
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.GetIP()::" + ex.Message,
                    EventLogEntryType.Error);
            }
            return String.Empty;
        }

    }
}