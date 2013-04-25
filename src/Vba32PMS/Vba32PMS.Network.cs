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
    /// Работа с сетью
    /// </summary>
    partial class Vba32PMS
    {
        private int maxCountToSend = 200;

        /// <summary>
        /// Отсылает события из файлов определенной сигнатуры
        /// </summary>
        /// <returns></returns>
        private bool SendEventsFromFiles()
        {
            bool retVal = true;
            try
            {
                LogMessage("SendEventsFromFiles::Отсылаем все события из файлов");
                LogMessage("Поиск файлов..");
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles(filePrefix + "*.xml"))
                {
                    if (isShutdown) return false;
                    LogMessage("Найден файл: " + file.FullName + ". Размер " + file.Length);
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
        /// Отсылает все события из файла порциями
        /// </summary>
        /// <param name="fileName"></param>
        private bool SendAllEventsFromFile(string fileName)
        {
            bool retVal = false;
            try
            {
                LogMessage("SendAllEventsFromFile::Отсылаем все события порциями из файла "+fileName);

                //LogMessage("Проверяем наличие файла с данными на диске. ");
                if (!File.Exists(fileName))
                {
                    LogError("SendAllEventsFromFile()::Не найден файл: " + fileName,
                            EventLogEntryType.Error);
                    return false;
                }

                //Возможно, с точки зрения производительности, лучше было бы использовать
                //класс Dictionary<Tkey,TValue>
                LogMessage("Десериализуем");
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
                    LogError("SendAllEventsFromFile():: Ошибка при попытке десериализовать данные из файла. " + ex.Message + " " + ex.GetType().ToString(),
                   EventLogEntryType.Error);

                    File.Delete(fileName);

                    return false;
                }

                if (list.Count == 0)
                {
                    LogMessage("Отсылка не требуется");
                    return true;
                }

                int itemsCount = maxCountToSend;
                while (list.Count > 0)
                {
                    LogMessage("Текущий размер коллекции: " + list.Count);
                    LogMessage("Текущий размер количества отсылаемого: " + itemsCount);
                    if (list.Count < itemsCount)
                    {
                        LogMessage("Размер коллекции меньше количества отсылаемого. Исправляем..");
                        itemsCount = list.Count;
                    }

                    LogMessage("Получим порцию данных");
                    EventsToControlAgentXml ggg = new EventsToControlAgentXml(list.GetRange(0, itemsCount), machineName);
                    LogMessage("Сформируем пакет");
                    string packet = ggg.Convert();
                    if (packet == String.Empty)
                    {
                        LogMessage("Пакет больше допустимого.. Уменьшаем..");
                        itemsCount-=20;
                        if (itemsCount < 0) itemsCount = 1; //Маловероятно, но все же
                        continue;
                    }

                    LogMessage("Отсылаем...");
                    string dataSend = EventsSender.SocketSendReceive(server, port, packet);

                    if (dataSend != "OK")
                    {
                        LogMessage("Не получилось отослать: " + dataSend);
                        break;
                    }
                    else
                    {
                        Debug.Write("Отослано. Сохраняем дату последней успешной отсылки");
                        lastSendDate = DateTime.Now;
                        WriteSettingsToRegistry();
                    }

                    LogMessage("Удаляем записи");
                    list.RemoveRange(0, itemsCount);
                    //!--
                    //Выйдем. Оставшиеся данные должны быть сериализованы в файл
                    if (isShutdown)
                        break;


                    itemsCount = maxCountToSend;
                }

                LogMessage("Отсылка сообщений из файла " + fileName + " закончена. Текущий размер коллекции: " + list.Count.ToString());
                if (list.Count == 0)
                {
                    retVal = true;
                    LogMessage("Все события из файла " + fileName + " отосланы. Удаляем его.");
                    File.Delete(fileName);
                    if (File.Exists(fileName))
                    {
                        LogError("SendAllEventsFromFile()::Файл не удалился! Возможна повторная отправка сообщений!",
                            EventLogEntryType.Error);
                    }
                }
                else
                {
                    LogMessage("Остались события для отсылки. Сохраним в тот же файл.");
                    ObjectSerializer.ObjToXmlStr(fileName, list);
                    if (!File.Exists(fileName))
                    {
                        LogError("SendAllEventsFromFile()::Файла после сериализации с событиями нет!",
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
        /// Посылает пакет, который позволяет установить, что этот комп является дочерним CC
        /// </summary>
        /// <returns></returns>
        private bool SendSystemInfo()
        {
            try
            {
                LogMessage("Vba32PMS.SendSystemInfo::Посылаем пакет с установленным CC флагом");

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
        /// Используется для получения IP-адреса компьютера, на котором запускается
        /// </summary>
        /// <param name="host">NETBIOS имя. Если передать String.Empty 
        /// берется для локального. Но есть нюанс на Win 2003 Server</param>
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