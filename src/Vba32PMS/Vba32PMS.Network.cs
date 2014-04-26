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
                LoggerPMS.log.Debug("SendEventsFromFiles::Отсылаем все события из файлов");
                LoggerPMS.log.Debug("Поиск файлов..");
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles(filePrefix + "*.xml"))
                {
                    if (isShutdown) return false;
                    LoggerPMS.log.Debug("Найден файл: " + file.FullName + ". Размер " + file.Length);
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
                LoggerPMS.log.Debug("SendAllEventsFromFile::Отсылаем все события порциями из файла "+fileName);

                LoggerPMS.log.Debug("Проверяем наличие файла с данными на диске. ");
                if (!File.Exists(fileName))
                {
                    LoggerPMS.log.Error("SendAllEventsFromFile()::Не найден файл: " + fileName);
                    return false;
                }

                //Возможно, с точки зрения производительности, лучше было бы использовать
                //класс Dictionary<Tkey,TValue>
                LoggerPMS.log.Debug("Десериализуем");
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
                    LoggerPMS.log.Error("SendAllEventsFromFile():: Ошибка при попытке десериализовать данные из файла. " + ex.Message + " " + ex.GetType().ToString());
                    File.Delete(fileName);
                    return false;
                }

                if (list.Count == 0)
                {
                    LoggerPMS.log.Debug("Отсылка не требуется");
                    return true;
                }

                Int32 itemsCount = maxCountToSend;
                while (list.Count > 0)
                {
                    LoggerPMS.log.Debug("Текущий размер коллекции: " + list.Count);
                    LoggerPMS.log.Debug("Текущий размер количества отсылаемого: " + itemsCount);
                    if (list.Count < itemsCount)
                    {
                        LoggerPMS.log.Debug("Размер коллекции меньше количества отсылаемого. Исправляем..");
                        itemsCount = list.Count;
                    }

                    LoggerPMS.log.Debug("Получим порцию данных");
                    EventsToControlAgentXml ggg = new EventsToControlAgentXml(list.GetRange(0, itemsCount), machineName);
                    LoggerPMS.log.Debug("Сформируем пакет");
                    String packet = ggg.Convert();
                    if (packet == String.Empty)
                    {
                        LoggerPMS.log.Debug("Пакет больше допустимого.. Уменьшаем..");
                        itemsCount -= 20;
                        if (itemsCount < 0) itemsCount = 1; //Маловероятно, но все же
                        continue;
                    }

                    LoggerPMS.log.Debug("Отсылаем...");
                    String dataSend = EventsSender.SocketSendReceive(server, port, packet);

                    if (dataSend != "OK")
                    {
                        LoggerPMS.log.Debug("Не получилось отослать: " + dataSend);
                        break;
                    }
                    else
                    {
                        LoggerPMS.log.Debug("Отослано. Сохраняем дату последней успешной отсылки");
                        lastSendDate = DateTime.Now;
                        WriteSettingsToRegistry();
                    }

                    LoggerPMS.log.Debug("Удаляем записи");
                    list.RemoveRange(0, itemsCount);
                    //!--
                    //Выйдем. Оставшиеся данные должны быть сериализованы в файл
                    if (isShutdown)
                        break;

                    itemsCount = maxCountToSend;
                }

                LoggerPMS.log.Debug("Отсылка сообщений из файла " + fileName + " закончена. Текущий размер коллекции: " + list.Count.ToString());
                if (list.Count == 0)
                {
                    retVal = true;
                    LoggerPMS.log.Debug("Все события из файла " + fileName + " отосланы. Удаляем его.");
                    File.Delete(fileName);
                    if (File.Exists(fileName))
                    {
                        LoggerPMS.log.Error("SendAllEventsFromFile()::Файл не удалился! Возможна повторная отправка сообщений!");
                    }
                }
                else
                {
                    LoggerPMS.log.Debug("Остались события для отсылки. Сохраним в тот же файл.");
                    ObjectSerializer.ObjToXmlStr(fileName, list);
                    if (!File.Exists(fileName))
                    {
                        LoggerPMS.log.Error("SendAllEventsFromFile()::Файла после сериализации с событиями нет!");
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
                LoggerPMS.log.Debug("Vba32PMS.SendSystemInfo::Посылаем пакет с установленным CC флагом");

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