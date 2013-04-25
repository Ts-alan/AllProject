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
    /// ������ � ������� � ���������
    /// </summary>
    partial class Vba32PMS
    {
        /// <summary>
        /// ������ ����� ������ ��������� �� ���� ������ ����������� ������ ������
        /// � ��������� �� � ����, �� ������ ����������, ������������ ������
        /// </summary>
        private bool DataBaseToXml()
        {
            try
            {
                LogMessage("Vba32PMS.DataBaseToXml::�� ���� ������ � xml-����");

                LogMessage("����� ��������� �������: " + lastSelectDate);
                DateTime dtFrom = lastSelectDate; //new DateTime(2008, 10, 01);
                DateTime dtTo = DateTime.Now;
                //LogMessage("��������� ������ ����������");
                string where = "Send = 1 AND " +
                    EventsFilterEntity.DateValue("EventTime", dtFrom, dtTo, "AND");
                LogMessage(where);
                LogMessage("�������� ������� �� ����");

                List<EventsEntity> list = GetEventsFromDb(connectionString, where, Int32.MaxValue);
                if (list == null)
                    return false;

                LogMessage("���������� ��������� � ������ " + list.Count);

                if (list.Count == 0)
                {
                    LogMessage("��� ������� ��� ����������. �������");
                    //LogMessage("������� � ������ ���� �������");
                    lastSelectDate = DateTime.Now;
                    WriteSettingsToRegistry();
                    return true;
                }

                while (list.Count > 0)
                {
                    //������ �������
                    int countToXml = 15000;
                    if (list.Count < countToXml)
                        countToXml = list.Count;

                    //��� ����� ��� ��������
                    string fileName = GetFileNameToSave();

                    //����� ������� � ������������
                    if (File.Exists(fileName))
                    {
                        LogMessage("��������� ����� ����������� ������");
                        List<EventsEntity> oldList = new List<EventsEntity>();
                        try
                        {
                            oldList = ObjectSerializer.XmlStrToObj<List<EventsEntity>>(fileName);
                        }
                        catch
                        {
                            LogError("Vba32PMS.DataBaseToXml()::������� ��� ������� ����� ����������� �������. ���� ����� ������",
                              EventLogEntryType.Error);
                            File.Delete(fileName);
                        }
                            
                        //LogMessage("���������� ��������� � ������ ������ " + oldList.Count);
                        

                        int tempInt = list.Count;
                        list.AddRange(oldList);
                        countToXml = list.Count;

                        //LogMessage("���������� ��������� ����� ����������� " + list.Count);

                        if (tempInt + oldList.Count != list.Count)
                        {
                            LogError("Vba32PMS.DataBaseToXml()::����� ���������� ��������� ����� ����������� ������� �� ���������",
                               EventLogEntryType.Error);

                            return false;
                        }
                    }

                    LogMessage("Vba32PMS.����������� � ���� " + fileName+ " . ���������� ������� �������������: "+countToXml.ToString());
                    ObjectSerializer.ObjToXmlStr(fileName, list.GetRange(0,countToXml));
                    list.RemoveRange(0, countToXml);
                }

                //LogMessage("������� � ������ ���� �������� �������");
                lastSelectDate = DateTime.Now;
                WriteSettingsToRegistry();
                //LogMessage("����� ��������� �������: " + lastSelectDate);
                //LogMessage("��� ���������������� �����: " + GetFileNameToSave(1000));
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
        /// ���������� ��� ����� ��� ����������
        /// </summary>
        /// <returns></returns>
        private string GetFileNameToSave()
        {
            string newFileName;
            try
            {
                LogMessage("Vba32PMS.GetFileNameToSave::���� ����� ��� ����������");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles(filePrefix + '*'))
                {
                    LogMessage("������ ����: " + file.FullName + ". ������ " + file.Length + ". ���������� ������: " + maxFileLength);
                    if (file.Length < maxFileLength)
                        return file.FullName;
                }

                //LogMessage("������ �� �������. C������ �����");
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