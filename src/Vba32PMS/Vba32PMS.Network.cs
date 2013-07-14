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
                Logger.Debug("SendEventsFromFiles::�������� ��� ������� �� ������");
                Logger.Debug("����� ������..");
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles(filePrefix + "*.xml"))
                {
                    if (isShutdown) return false;
                    Logger.Debug("������ ����: " + file.FullName + ". ������ " + file.Length);
                    if (!SendAllEventsFromFile(file.FullName))
                        retVal = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("SendAllEventsFromFile()::" + ex.Message);
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
                Logger.Debug("SendAllEventsFromFile::�������� ��� ������� �������� �� ����� "+fileName);

                Logger.Debug("��������� ������� ����� � ������� �� �����. ");
                if (!File.Exists(fileName))
                {
                    Logger.Error("SendAllEventsFromFile()::�� ������ ����: " + fileName);
                    return false;
                }

                //��������, � ����� ������ ������������������, ����� ���� �� ������������
                //����� Dictionary<Tkey,TValue>
                Logger.Debug("�������������");
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
                    Logger.Error("SendAllEventsFromFile():: ������ ��� ������� ��������������� ������ �� �����. " + ex.Message + " " + ex.GetType().ToString());
                    File.Delete(fileName);
                    return false;
                }

                if (list.Count == 0)
                {
                    Logger.Debug("������� �� ���������");
                    return true;
                }

                Int32 itemsCount = maxCountToSend;
                while (list.Count > 0)
                {
                    Logger.Debug("������� ������ ���������: " + list.Count);
                    Logger.Debug("������� ������ ���������� �����������: " + itemsCount);
                    if (list.Count < itemsCount)
                    {
                        Logger.Debug("������ ��������� ������ ���������� �����������. ����������..");
                        itemsCount = list.Count;
                    }

                    Logger.Debug("������� ������ ������");
                    EventsToControlAgentXml ggg = new EventsToControlAgentXml(list.GetRange(0, itemsCount), machineName);
                    Logger.Debug("���������� �����");
                    String packet = ggg.Convert();
                    if (packet == String.Empty)
                    {
                        Logger.Debug("����� ������ �����������.. ���������..");
                        itemsCount -= 20;
                        if (itemsCount < 0) itemsCount = 1; //������������, �� ��� ��
                        continue;
                    }

                    Logger.Debug("��������...");
                    String dataSend = EventsSender.SocketSendReceive(server, port, packet);

                    if (dataSend != "OK")
                    {
                        Logger.Debug("�� ���������� ��������: " + dataSend);
                        break;
                    }
                    else
                    {
                        Logger.Debug("��������. ��������� ���� ��������� �������� �������");
                        lastSendDate = DateTime.Now;
                        WriteSettingsToRegistry();
                    }

                    Logger.Debug("������� ������");
                    list.RemoveRange(0, itemsCount);
                    //!--
                    //������. ���������� ������ ������ ���� ������������� � ����
                    if (isShutdown)
                        break;

                    itemsCount = maxCountToSend;
                }

                Logger.Debug("������� ��������� �� ����� " + fileName + " ���������. ������� ������ ���������: " + list.Count.ToString());
                if (list.Count == 0)
                {
                    retVal = true;
                    Logger.Debug("��� ������� �� ����� " + fileName + " ��������. ������� ���.");
                    File.Delete(fileName);
                    if (File.Exists(fileName))
                    {
                        Logger.Error("SendAllEventsFromFile()::���� �� ��������! �������� ��������� �������� ���������!");
                    }
                }
                else
                {
                    Logger.Debug("�������� ������� ��� �������. �������� � ��� �� ����.");
                    ObjectSerializer.ObjToXmlStr(fileName, list);
                    if (!File.Exists(fileName))
                    {
                        Logger.Error("SendAllEventsFromFile()::����� ����� ������������ � ��������� ���!");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("SendAllEventsFromFile()::" + ex.Message +" "+ex.GetType().ToString());
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
                Logger.Debug("Vba32PMS.SendSystemInfo::�������� ����� � ������������� CC ������");

                if (ipAddress == String.Empty)
                    ipAddress = GetIP(machineName);
                StringBuilder build = new StringBuilder(128);
                build.AppendFormat("<SystemInfo><ComputerName>{0}</ComputerName><IPAddress>{1}</IPAddress><ControlCenter>true</ControlCenter></SystemInfo>",
                                    machineName, ipAddress);
                
                String dataSend = EventsSender.SocketSendReceive(server, port, build.ToString());
                if (dataSend != "OK")
                {
                    Logger.Debug("Vba32PMS.SendSystemInfo():: dataSend=" + dataSend);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Vba32PMS.SendSystemInfo()::" + ex.Message);
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
                Logger.Error("Vba32PMS.GetIP()::" + ex.Message);
            }
            return String.Empty;
        }

    }
}