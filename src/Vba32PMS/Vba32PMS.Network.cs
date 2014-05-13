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
    /// Работа с сетью
    /// </summary>
    partial class Vba32PMS
    {
        private Int32 maxCountToSend = 200;

        /// <summary>
        /// Отсылает события из файлов определенной сигнатуры
        /// </summary>
        /// <returns></returns>
        private Boolean SendEventsFromFiles()
        {
            Boolean retVal = true;
            try
            {
                LoggerPMS.log.Debug("SendEventsFromFiles:: Send all events from files.");
                LoggerPMS.log.Debug("Files searching...");
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles(filePrefix + "*.xml"))
                {
                    if (isShutdown) return false;
                    LoggerPMS.log.Debug("File is found: " + file.FullName + ". Size " + file.Length);
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
        /// Отсылает все события из файла порциями
        /// </summary>
        /// <param name="fileName"></param>
        private Boolean SendAllEventsFromFile(String fileName)
        {
            Boolean retVal = false;
            try
            {
                LoggerPMS.log.Debug("SendAllEventsFromFile:: Send all events parts from file " + fileName);

                LoggerPMS.log.Debug("Check file existence with date on disk. ");
                if (!File.Exists(fileName))
                {
                    LoggerPMS.log.Error("SendAllEventsFromFile()::File isn't found: " + fileName);
                    return false;
                }

                //Возможно, с точки зрения производительности, лучше было бы использовать
                //класс Dictionary<Tkey,TValue>
                LoggerPMS.log.Debug("Deserialize");
                List<EventsEntity> list = new List<EventsEntity>();
                try
                {
                    //х3 какие ошибки могут возникнуть при десериализации, но я думаю, эти файлы
                    //уже не восстановить. Разводить их также не стоит поэтому в этом случае
                    //мы будем его просто сносить и возвращать false
                    list = ObjectSerializer.XmlStrToObj<List<EventsEntity>>(fileName);
                }
                catch(Exception ex)
                {
                    LoggerPMS.log.Error("SendAllEventsFromFile():: Deserialize error. " + ex.Message + " " + ex.GetType().ToString());
                    File.Delete(fileName);
                    return false;
                }

                if (list.Count == 0)
                {
                    LoggerPMS.log.Debug("Sending isn't needed.");
                    return true;
                }

                Int32 itemsCount = maxCountToSend;
                while (list.Count > 0)
                {
                    LoggerPMS.log.Debug("Current collection size: " + list.Count);
                    LoggerPMS.log.Debug("Current sending items size: " + itemsCount);
                    if (list.Count < itemsCount)
                    {
                        LoggerPMS.log.Debug("Collection size less than sending items size. Fixing...");
                        itemsCount = list.Count;
                    }

                    LoggerPMS.log.Debug("Get date package");
                    EventsToControlAgentXml ggg = new EventsToControlAgentXml(list.GetRange(0, itemsCount), machineName);
                    LoggerPMS.log.Debug("Convert data package");
                    String packet = ggg.Convert();
                    if (packet == String.Empty)
                    {
                        LoggerPMS.log.Debug("Data package size bigger than permissible size. Decrease...");
                        itemsCount -= 20;
                        if (itemsCount < 0) itemsCount = 1; //Маловероятно, но все же
                        continue;
                    }

                    LoggerPMS.log.Debug("Sending...");
                    String dataSend = EventsSender.SocketSendReceive(settingsPMS.Server, settingsPMS.Port.Value, packet);

                    if (dataSend != "OK")
                    {
                        LoggerPMS.log.Debug("Can't send: " + dataSend);
                        break;
                    }
                    else
                    {
                        LoggerPMS.log.Debug("Sended. Saving the latest successful sending date.");
                        settingsPMS.LastSendDate = DateTime.Now;
                        WriteSettingsToRegistry();
                    }

                    LoggerPMS.log.Debug("Delete events.");
                    list.RemoveRange(0, itemsCount);
                    //!--
                    //Выйдем. Оставшиеся данные должны быть сериализованы в файл
                    if (isShutdown)
                        break;

                    itemsCount = maxCountToSend;
                }

                LoggerPMS.log.Debug("Send messages from file " + fileName + " was finished. Current collection size: " + list.Count.ToString());
                if (list.Count == 0)
                {
                    retVal = true;
                    LoggerPMS.log.Debug("All events from file " + fileName + " were sended. Delete file.");
                    File.Delete(fileName);
                    if (File.Exists(fileName))
                    {
                        LoggerPMS.log.Error("SendAllEventsFromFile()::File wasn't deleted! Repeated message sending is possible!");
                    }
                }
                else
                {
                    LoggerPMS.log.Debug("Events for sending is stayed. Save into file.");
                    ObjectSerializer.ObjToXmlStr(fileName, list);
                    if (!File.Exists(fileName))
                    {
                        LoggerPMS.log.Error("SendAllEventsFromFile():: File after serialization is not exist!");
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
        /// Посылает пакет, который позволяет установить, что этот комп является дочерним CC
        /// </summary>
        /// <returns></returns>
        private Boolean SendSystemInfo()
        {
            try
            {
                LoggerPMS.log.Debug("Vba32PMS.SendSystemInfo:: Send package with setting 'CC' flag.");

                if (ipAddress == String.Empty)
                    ipAddress = GetIP(machineName);
                StringBuilder build = new StringBuilder(128);
                build.AppendFormat("<SystemInfo><ComputerName>{0}</ComputerName><IPAddress>{1}</IPAddress><ControlCenter>true</ControlCenter></SystemInfo>",
                                    machineName, ipAddress);
                
                String dataSend = EventsSender.SocketSendReceive(settingsPMS.Server, settingsPMS.Port.Value, build.ToString());
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
        /// Используется для получения IP-адреса компьютера, на котором запускается
        /// </summary>
        /// <param name="host">NETBIOS имя. Если передать String.Empty 
        /// берется для локального. Но есть нюанс на Win 2003 Server</param>
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