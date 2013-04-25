using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.IO;

using Vba32.ControlCenter.PeriodicalMaintenanceService.DataBase;
using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService
{
    /// <summary>
    /// Работа с файлами и объектами
    /// </summary>
    partial class Vba32PMS
    {
        /// <summary>
        /// Данный метод должен извлекать из базы данных необходимую порцию данных
        /// и добавлять ее в файл, не стирая предыдущие, неотосланные записи
        /// </summary>
        private bool DataBaseToXml()
        {
            try
            {
                LogMessage("Vba32PMS.DataBaseToXml::Из базы данных в xml-файл");

                LogMessage("Время последней выборки: " + lastSelectDate);
                DateTime dtFrom = lastSelectDate; //new DateTime(2008, 10, 01);
                DateTime dtTo = DateTime.Now;
                //LogMessage("Формируем строку фильтрации");
                string where = "Send = 1 AND " +
                    EventsFilterEntity.DateValue("EventTime", dtFrom, dtTo, "AND");
                LogMessage(where);
                LogMessage("Получаем события из базы");

                List<EventsEntity> list = GetEventsFromDb(connectionString, where, Int32.MaxValue);
                if (list == null)
                    return false;

                LogMessage("Количество элементов в списке " + list.Count);

                if (list.Count == 0)
                {
                    LogMessage("Нет записей для сохранения. Выходим");
                    //LogMessage("Запишем в реестр дату выборки");
                    lastSelectDate = DateTime.Now;
                    WriteSettingsToRegistry();
                    return true;
                }

                while (list.Count > 0)
                {
                    //Порции отсылки
                    int countToXml = 15000;
                    if (list.Count < countToXml)
                        countToXml = list.Count;

                    //Имя файла для отправки
                    string fileName = GetFileNameToSave();

                    //Может засунем в существующий
                    if (File.Exists(fileName))
                    {
                        LogMessage("Извлекаем ранее сохраненные данные");
                        List<EventsEntity> oldList = new List<EventsEntity>();
                        try
                        {
                            oldList = ObjectSerializer.XmlStrToObj<List<EventsEntity>>(fileName);
                        }
                        catch
                        {
                            LogError("Vba32PMS.DataBaseToXml()::Попытка при считать ранее сохраненные события. Файл будет удален",
                              EventLogEntryType.Error);
                            File.Delete(fileName);
                        }
                            
                        //LogMessage("Количество элементов в старом списке " + oldList.Count);
                        

                        int tempInt = list.Count;
                        list.AddRange(oldList);
                        countToXml = list.Count;

                        //LogMessage("Количество элементов после объединения " + list.Count);

                        if (tempInt + oldList.Count != list.Count)
                        {
                            LogError("Vba32PMS.DataBaseToXml()::Общее количество элементов после объединения списков не совпадает",
                               EventLogEntryType.Error);

                            return false;
                        }
                    }

                    LogMessage("Vba32PMS.Сериализуем в файл " + fileName+ " . Количество событий сериализуется: "+countToXml.ToString());
                    ObjectSerializer.ObjToXmlStr(fileName, list.GetRange(0,countToXml));
                    list.RemoveRange(0, countToXml);
                }

                //LogMessage("Запишем в реестр дату успешной выборки");
                lastSelectDate = DateTime.Now;
                WriteSettingsToRegistry();
                //LogMessage("Время последней выборки: " + lastSelectDate);
                //LogMessage("Имя сгенерированного файла: " + GetFileNameToSave(1000));
            }
            catch(Exception ex)
            {
                LogError("Vba32PMS.DataBaseToXml()::" + ex.Message,
                          EventLogEntryType.Error);
                
                return false;
            }

            return true;
        }

        /// <summary>
        /// Возвращает имя файла для сохранения
        /// </summary>
        /// <returns></returns>
        private string GetFileNameToSave()
        {
            string newFileName;
            try
            {
                LogMessage("Vba32PMS.GetFileNameToSave::Ищем файлы для сохранения");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles(filePrefix + '*'))
                {
                    LogMessage("Найден файл: " + file.FullName + ". Размер " + file.Length + ". Допустимый размер: " + maxFileLength);
                    if (file.Length < maxFileLength)
                        return file.FullName;
                }

                //LogMessage("Ничего не найдено. Cоздать новый");
                newFileName = path + filePrefix + Guid.NewGuid() + ".xml";
            }
            catch(Exception ex)
            {
                LogError("Vba32PMS.GetFileNameToSave()::" + ex.Message,
                        EventLogEntryType.Error);
                
                return null;
            }

            return newFileName;
        }
    }
    
}