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
        private Boolean DataBaseToXml()
        {
            try
            {
                Logger.Debug("Vba32PMS.DataBaseToXml::Из базы данных в xml-файл");

                Logger.Debug("Время последней выборки: " + lastSelectDate);
                DateTime dtFrom = lastSelectDate; //new DateTime(2008, 10, 01);
                DateTime dtTo = DateTime.Now;
                Logger.Debug("Формируем строку фильтрации");
                String where = "Send = 1 AND " + EventsFilterEntity.DateValue("EventTime", dtFrom, dtTo, "AND");
                Logger.Debug(where);
                Logger.Debug("Получаем события из базы");

                List<EventsEntity> list = GetEventsFromDb(connectionString, where, Int32.MaxValue);
                if (list == null)
                    return false;

                Logger.Debug("Количество элементов в списке " + list.Count);

                if (list.Count == 0)
                {
                    Logger.Debug("Нет записей для сохранения. Выходим");
                    lastSelectDate = DateTime.Now;
                    WriteSettingsToRegistry();
                    return true;
                }

                while (list.Count > 0)
                {
                    //Порции отсылки
                    Int32 countToXml = 15000;
                    if (list.Count < countToXml)
                        countToXml = list.Count;

                    //Имя файла для отправки
                    String fileName = GetFileNameToSave();

                    //Может засунем в существующий
                    if (File.Exists(fileName))
                    {
                        Logger.Debug("Извлекаем ранее сохраненные данные");
                        List<EventsEntity> oldList = new List<EventsEntity>();
                        try
                        {
                            oldList = ObjectSerializer.XmlStrToObj<List<EventsEntity>>(fileName);
                        }
                        catch
                        {
                            Logger.Error("Vba32PMS.DataBaseToXml()::Попытка при считать ранее сохраненные события. Файл будет удален");
                            File.Delete(fileName);
                        }

                        Int32 tempInt = list.Count;
                        list.AddRange(oldList);
                        countToXml = list.Count;

                        if (tempInt + oldList.Count != list.Count)
                        {
                            Logger.Error("Vba32PMS.DataBaseToXml()::Общее количество элементов после объединения списков не совпадает");
                            return false;
                        }
                    }

                    Logger.Debug("Vba32PMS.Сериализуем в файл " + fileName+ " . Количество событий сериализуется: "+countToXml.ToString());
                    ObjectSerializer.ObjToXmlStr(fileName, list.GetRange(0,countToXml));
                    list.RemoveRange(0, countToXml);
                }

                lastSelectDate = DateTime.Now;
                WriteSettingsToRegistry();
                Logger.Debug("Время последней выборки: " + lastSelectDate);
                //Logger.Debug("Имя сгенерированного файла: " + GetFileNameToSave(1000));
            }
            catch(Exception ex)
            {
                Logger.Error("Vba32PMS.DataBaseToXml()::" + ex.Message);                
                return false;
            }
            return true;
        }

        /// <summary>
        /// Возвращает имя файла для сохранения
        /// </summary>
        /// <returns></returns>
        private String GetFileNameToSave()
        {
            String newFileName;
            try
            {
                Logger.Debug("Vba32PMS.GetFileNameToSave::Ищем файлы для сохранения");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles(filePrefix + '*'))
                {
                    Logger.Debug("Найден файл: " + file.FullName + ". Размер " + file.Length + ". Допустимый размер: " + maxFileLength);
                    if (file.Length < maxFileLength)
                        return file.FullName;
                }

                //LogMessage("Ничего не найдено. Cоздать новый");
                newFileName = path + filePrefix + Guid.NewGuid() + ".xml";
            }
            catch(Exception ex)
            {
                Logger.Error("Vba32PMS.GetFileNameToSave()::" + ex.Message);
                return null;
            }
            return newFileName;
        }
    }
    
}