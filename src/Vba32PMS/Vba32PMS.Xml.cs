using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;
using VirusBlokAda.CC.Common.Xml;
using VirusBlokAda.CC.DataBase;
using VirusBlokAda.CC.Filters.Primitive;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService
{
    /// <summary>
    /// ������ � ������� � ���������
    /// </summary>
    partial class Vba32PMS
    {
        /// <summary>
        /// ������ ����� ������ ��������� �� ���� ������ ����������� ������ ������
        /// � ��������� �� � ����, �� ������ ����������, ������������ ������
        /// </summary>
        private Boolean DataBaseToXml()
        {
            try
            {
                LoggerPMS.log.Debug("Vba32PMS.DataBaseToXml::From database to xml file");

                LoggerPMS.log.Debug("Last selection time: " + lastSelectDate);
                DateTime dtFrom = lastSelectDate; //new DateTime(2008, 10, 01);
                DateTime dtTo = DateTime.Now;
                LoggerPMS.log.Debug("Generate filter string");
                String where = "Send = 1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dtFrom, dtTo, "EventTime", false, false);
                LoggerPMS.log.Debug(where);
                LoggerPMS.log.Debug("Select events from DB");

                List<EventsEntity> list = GetEventsFromDb(connectionString, where, Int32.MaxValue);
                if (list == null)
                    return false;

                LoggerPMS.log.Debug("List count " + list.Count);

                if (list.Count == 0)
                {
                    LoggerPMS.log.Debug("No have items for saving. Exit");
                    lastSelectDate = DateTime.Now;
                    WriteSettingsToRegistry();
                    return true;
                }

                while (list.Count > 0)
                {
                    //������ �������
                    Int32 countToXml = 15000;
                    if (list.Count < countToXml)
                        countToXml = list.Count;

                    //��� ����� ��� ��������
                    String fileName = GetFileNameToSave();

                    //����� ������� � ������������
                    if (File.Exists(fileName))
                    {
                        LoggerPMS.log.Debug("Select saved data");
                        List<EventsEntity> oldList = new List<EventsEntity>();
                        try
                        {
                            oldList = ObjectSerializer.XmlStrToObj<List<EventsEntity>>(fileName);
                        }
                        catch
                        {
                            LoggerPMS.log.Error("Vba32PMS.DataBaseToXml():: Try read saved events. File will be deleted.");
                            File.Delete(fileName);
                        }

                        Int32 tempInt = list.Count;
                        list.AddRange(oldList);
                        countToXml = list.Count;

                        if (tempInt + oldList.Count != list.Count)
                        {
                            LoggerPMS.log.Error("Vba32PMS.DataBaseToXml()::Total element count after union lists is not match.");
                            return false;
                        }
                    }

                    LoggerPMS.log.Debug("Vba32PMS. Serialize to file " + fileName+ " . Count events serialize: "+countToXml.ToString());
                    ObjectSerializer.ObjToXmlStr(fileName, list.GetRange(0,countToXml));
                    list.RemoveRange(0, countToXml);
                }

                lastSelectDate = DateTime.Now;
                WriteSettingsToRegistry();
                LoggerPMS.log.Debug("Last selection time: " + lastSelectDate);
                //LoggerPMS.log.Debug("��� ���������������� �����: " + GetFileNameToSave(1000));
            }
            catch(Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.DataBaseToXml()::" + ex.Message);                
                return false;
            }
            return true;
        }

        /// <summary>
        /// ���������� ��� ����� ��� ����������
        /// </summary>
        /// <returns></returns>
        private String GetFileNameToSave()
        {
            String newFileName;
            try
            {
                LoggerPMS.log.Debug("Vba32PMS.GetFileNameToSave:: Find files for saving.");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles(filePrefix + '*'))
                {
                    LoggerPMS.log.Debug("File is found: " + file.FullName + ". Size " + file.Length + ". Permissible size: " + maxFileLength);
                    if (file.Length < maxFileLength)
                        return file.FullName;
                }

                //LogMessage("������ �� �������. C������ �����");
                newFileName = path + filePrefix + Guid.NewGuid() + ".xml";
            }
            catch(Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.GetFileNameToSave()::" + ex.Message);
                return null;
            }
            return newFileName;
        }
    }
    
}