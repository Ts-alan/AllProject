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
        private Boolean DataBaseToXml()
        {
            try
            {
                Logger.Debug("Vba32PMS.DataBaseToXml::�� ���� ������ � xml-����");

                Logger.Debug("����� ��������� �������: " + lastSelectDate);
                DateTime dtFrom = lastSelectDate; //new DateTime(2008, 10, 01);
                DateTime dtTo = DateTime.Now;
                Logger.Debug("��������� ������ ����������");
                String where = "Send = 1 AND " + EventsFilterEntity.DateValue("EventTime", dtFrom, dtTo, "AND");
                Logger.Debug(where);
                Logger.Debug("�������� ������� �� ����");

                List<EventsEntity> list = GetEventsFromDb(connectionString, where, Int32.MaxValue);
                if (list == null)
                    return false;

                Logger.Debug("���������� ��������� � ������ " + list.Count);

                if (list.Count == 0)
                {
                    Logger.Debug("��� ������� ��� ����������. �������");
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
                        Logger.Debug("��������� ����� ����������� ������");
                        List<EventsEntity> oldList = new List<EventsEntity>();
                        try
                        {
                            oldList = ObjectSerializer.XmlStrToObj<List<EventsEntity>>(fileName);
                        }
                        catch
                        {
                            Logger.Error("Vba32PMS.DataBaseToXml()::������� ��� ������� ����� ����������� �������. ���� ����� ������");
                            File.Delete(fileName);
                        }

                        Int32 tempInt = list.Count;
                        list.AddRange(oldList);
                        countToXml = list.Count;

                        if (tempInt + oldList.Count != list.Count)
                        {
                            Logger.Error("Vba32PMS.DataBaseToXml()::����� ���������� ��������� ����� ����������� ������� �� ���������");
                            return false;
                        }
                    }

                    Logger.Debug("Vba32PMS.����������� � ���� " + fileName+ " . ���������� ������� �������������: "+countToXml.ToString());
                    ObjectSerializer.ObjToXmlStr(fileName, list.GetRange(0,countToXml));
                    list.RemoveRange(0, countToXml);
                }

                lastSelectDate = DateTime.Now;
                WriteSettingsToRegistry();
                Logger.Debug("����� ��������� �������: " + lastSelectDate);
                //Logger.Debug("��� ���������������� �����: " + GetFileNameToSave(1000));
            }
            catch(Exception ex)
            {
                Logger.Error("Vba32PMS.DataBaseToXml()::" + ex.Message);                
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
                Logger.Debug("Vba32PMS.GetFileNameToSave::���� ����� ��� ����������");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles(filePrefix + '*'))
                {
                    Logger.Debug("������ ����: " + file.FullName + ". ������ " + file.Length + ". ���������� ������: " + maxFileLength);
                    if (file.Length < maxFileLength)
                        return file.FullName;
                }

                //LogMessage("������ �� �������. C������ �����");
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