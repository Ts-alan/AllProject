using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Net;
using System.Diagnostics;

using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;
using Vba32.ControlCenter.PeriodicalMaintenanceService.Network;
using VirusBlokAda.CC.Common.Xml;
using VirusBlokAda.CC.DataBase;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService
{
    /// <summary>
    /// ������ � �����
    /// </summary>
    partial class Vba32PMS
    {
        private Int32 maxCountToSend = 200;

        /// <summary>
        /// �������� ������� �� ������ ������������ ���������
        /// </summary>
        /// <returns></returns>
        private Boolean SendEventsFromFiles()
        {
            Boolean retVal = true;
            try
            {
                LoggerPMS.log.Debug("SendEventsFromFiles::�������� ��� ������� �� ������");
                LoggerPMS.log.Debug("����� ������..");
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles(filePrefix + "*.xml"))
                {
                    if (isShutdown) return false;
                    LoggerPMS.log.Debug("������ ����: " + file.FullName + ". ������ " + file.Length);
                    if (!SendAllEventsFromFile(file.FullName))
                        retVal = false;
                }
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("SendAllEventsFromFile()::" + ex.Message);
                return false;
            }
            return retVal;
        }

        /// <summary>
        /// �������� ��� ������� �� ����� ��������
        /// </summary>
        /// <param name="fileName"></param>
        private Boolean SendAllEventsFromFile(String fileName)
        {
            Boolean retVal = false;
            try
            {
                LoggerPMS.log.Debug("SendAllEventsFromFile::�������� ��� ������� �������� �� ����� "+fileName);

                LoggerPMS.log.Debug("��������� ������� ����� � ������� �� �����. ");
                if (!File.Exists(fileName))
                {
                    LoggerPMS.log.Error("SendAllEventsFromFile()::�� ������ ����: " + fileName);
                    return false;
                }

                //��������, � ����� ������ ������������������, ����� ���� �� ������������
                //����� Dictionary<Tkey,TValue>
                LoggerPMS.log.Debug("�������������");
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
                    LoggerPMS.log.Error("SendAllEventsFromFile():: ������ ��� ������� ��������������� ������ �� �����. " + ex.Message + " " + ex.GetType().ToString());
                    File.Delete(fileName);
                    return false;
                }

                if (list.Count == 0)
                {
                    LoggerPMS.log.Debug("������� �� ���������");
                    return true;
                }

                Int32 itemsCount = maxCountToSend;
                while (list.Count > 0)
                {
                    LoggerPMS.log.Debug("������� ������ ���������: " + list.Count);
                    LoggerPMS.log.Debug("������� ������ ���������� �����������: " + itemsCount);
                    if (list.Count < itemsCount)
                    {
                        LoggerPMS.log.Debug("������ ��������� ������ ���������� �����������. ����������..");
                        itemsCount = list.Count;
                    }

                    LoggerPMS.log.Debug("������� ������ ������");
                    EventsToControlAgentXml ggg = new EventsToControlAgentXml(list.GetRange(0, itemsCount), machineName);
                    LoggerPMS.log.Debug("���������� �����");
                    String packet = ggg.Convert();
                    if (packet == String.Empty)
                    {
                        LoggerPMS.log.Debug("����� ������ �����������.. ���������..");
                        itemsCount -= 20;
                        if (itemsCount < 0) itemsCount = 1; //������������, �� ��� ��
                        continue;
                    }

                    LoggerPMS.log.Debug("��������...");
                    String dataSend = EventsSender.SocketSendReceive(server, port, packet);

                    if (dataSend != "OK")
                    {
                        LoggerPMS.log.Debug("�� ���������� ��������: " + dataSend);
                        break;
                    }
                    else
                    {
                        LoggerPMS.log.Debug("��������. ��������� ���� ��������� �������� �������");
                        lastSendDate = DateTime.Now;
                        WriteSettingsToRegistry();
                    }

                    LoggerPMS.log.Debug("������� ������");
                    list.RemoveRange(0, itemsCount);
                    //!--
                    //������. ���������� ������ ������ ���� ������������� � ����
                    if (isShutdown)
                        break;

                    itemsCount = maxCountToSend;
                }

                LoggerPMS.log.Debug("������� ��������� �� ����� " + fileName + " ���������. ������� ������ ���������: " + list.Count.ToString());
                if (list.Count == 0)
                {
                    retVal = true;
                    LoggerPMS.log.Debug("��� ������� �� ����� " + fileName + " ��������. ������� ���.");
                    File.Delete(fileName);
                    if (File.Exists(fileName))
                    {
                        LoggerPMS.log.Error("SendAllEventsFromFile()::���� �� ��������! �������� ��������� �������� ���������!");
                    }
                }
                else
                {
                    LoggerPMS.log.Debug("�������� ������� ��� �������. �������� � ��� �� ����.");
                    ObjectSerializer.ObjToXmlStr(fileName, list);
                    if (!File.Exists(fileName))
                    {
                        LoggerPMS.log.Error("SendAllEventsFromFile()::����� ����� ������������ � ��������� ���!");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("SendAllEventsFromFile()::" + ex.Message +" "+ex.GetType().ToString());
                return false;
            }
            return retVal;
        }

        /// <summary>
        /// �������� �����, ������� ��������� ����������, ��� ���� ���� �������� �������� CC
        /// </summary>
        /// <returns></returns>
        private Boolean SendSystemInfo()
        {
            try
            {
                LoggerPMS.log.Debug("Vba32PMS.SendSystemInfo::�������� ����� � ������������� CC ������");

                if (ipAddress == String.Empty)
                    ipAddress = GetIP(machineName);
                StringBuilder build = new StringBuilder(128);
                build.AppendFormat("<SystemInfo><ComputerName>{0}</ComputerName><IPAddress>{1}</IPAddress><ControlCenter>true</ControlCenter></SystemInfo>",
                                    machineName, ipAddress);
                
                String dataSend = EventsSender.SocketSendReceive(server, port, build.ToString());
                if (dataSend != "OK")
                {
                    LoggerPMS.log.Debug("Vba32PMS.SendSystemInfo():: dataSend=" + dataSend);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.SendSystemInfo()::" + ex.Message);
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
        private String GetIP(String host)
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
                LoggerPMS.log.Error("Vba32PMS.GetIP()::" + ex.Message);
            }
            return String.Empty;
        }

    }
}