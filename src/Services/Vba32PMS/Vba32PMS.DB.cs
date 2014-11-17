using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;

using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;
using VirusBlokAda.CC.DataBase;
using VirusBlokAda.CC.Tasks.Service;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService
{
    /// <summary>
    /// ������ � ����� ������
    /// </summary>
    partial class Vba32PMS
    {
        /// <summary>
        /// ���������� �� ���� ������ �������� ����� ������
        /// </summary>
        /// <param name="connectionStrings">������ �����������</param>
        /// <param name="sortExpression">��������� Where</param>
        /// <param name="count">���������� ������������ ��������</param>
        /// <returns></returns>
        private List<EventsEntity> GetEventsFromDb(String connStr, String sortExpression, Int32 count)
        {
            LoggerPMS.log.Debug("Vba32PMS.GetEventsFromDb():: Get events from database.");
            List<EventsEntity> list = new List<EventsEntity>();
            try
            {
                EventProvider db = new EventProvider(connStr);
                list = db.List(sortExpression, "EventTime ASC", 1, count);
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.GetEventsFromDb():: " + ex.Message);
                return null;

            }
            return list;
        }

        /// <summary>
        /// ������ ������ ������ c Delivery �� DeliveryTimeout
        /// </summary>
        /// <param name="connStr">������ �����������</param>
        /// <returns></returns>
        private Boolean CheckDeliveryState(String connStr)
        {
            LoggerPMS.log.Debug("Vba32PMS.CheckDeliveryState():: Change state for buzz tasks.");
            DateTime dtTo = DateTime.Now;
            try
            {
                dtTo = dtTo.AddMinutes(-1);
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.CheckDeliveryState():: Format filter string error: " + ex.Message);
                return false;
            }

            try
            {
                TaskProvider db = new TaskProvider(connStr);
                db.ChangeDeliveryState(dtTo);
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.CheckDeliveryState():: " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// ������� ���� �� ������ � �������� �������
        /// </summary>
        /// <param name="connStr">������ �����������</param>
        /// <returns></returns>
        private Boolean ClearOldEvents(String connStr)
        {
            LoggerPMS.log.Debug("Vba32PMS.ClearOldEvents():: Delete old events.");
            DateTime dtTo = DateTime.Now;
            try
            {
                Int32 tmp = 0 - settingsPMS.DaysToDelete.Value;
                dtTo = dtTo.AddDays(tmp);
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.ClearOldEvents():: Format filter string error: " + ex.Message);
                return false;
            }

            try
            {
                EventProvider db = new EventProvider(connStr);
                db.ClearOldEvents(dtTo);
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.ClearOldEvents():: " + ex.Message);
                return false;
            }
            return true;
        }


        /// <summary>
        /// ������� ���� �� ������ � �������� �����
        /// !OPTM-- ������ ��������, ����� �� ���-���� �������������
        /// </summary>
        /// <param name="connStr">������ �����������</param>
        /// <returns></returns>
        private Boolean ClearOldTasks(String connStr)
        {
            LoggerPMS.log.Debug("Vba32PMS.ClearOldTasks():: Delete old tasks.");
            DateTime dtTo = DateTime.Now;
            try
            {
                Int32 tmp = 0 - settingsPMS.TaskDaysToDelete.Value;
                dtTo = dtTo.AddDays(tmp);
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.ClearOldTasks():: Format filter string error: " + ex.Message);
                return false;
            }

            try
            {
                TaskProvider db = new TaskProvider(connStr);
                db.ClearOldTasks(dtTo);
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.ClearOldTasks():: " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// ������� ���� �� ������ ����������� � ����������� � ������� IP �������
        /// </summary>
        /// <param name="connectionString"></param>
        private Boolean ClearOldComputers(String connStr)
        {
            LoggerPMS.log.Debug("Vba32PMS.ClearOldComputers():: Delete old computers.");
            DateTime dtTo = DateTime.Now;
            try
            {                
                Int32 tmp = 0 - settingsPMS.ComputerDaysToDelete.Value;
                dtTo = dtTo.AddDays(tmp);
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.ClearOldComputers():: Format filter string error: " + ex.Message);
                return false;
            }

            try
            {
                ComputerProvider db = new ComputerProvider(connStr);
                db.ClearOldComputers(dtTo);
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.ClearOldComputers():: " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// ������ ���� ������
        /// </summary>
        /// <param name="connStr">������ �����������</param>
        /// <returns></returns>
        private Boolean CompressDB(String connStr)
        {
            LoggerPMS.log.Debug("Vba32PMS.CompressDB():: Shrink database.");

            try
            {
                DataBaseProvider db = new DataBaseProvider(connStr);
                db.ShrinkDataBase(10);
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.CompressDB():: " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// �������� ����� �� ���������������� ������
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private Boolean ConfigureAgent(String connectionString)
        {
            LoggerPMS.log.Debug("Vba32PMS.ConfigureAgent():: Configure arents.");

            LoggerPMS.log.Debug("Vba32PMS.ConfigureAgent():: Get IP-addresses list.");
            List<String> list = null;
            try
            {
                TaskProvider db = new TaskProvider(connectionString);
                list = db.GetIPAddressListForConfigure();
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.ConfigureAgent():: " + ex.Message);
                return false;
            }

            if (list == null || list.Count == 0)
            {
                LoggerPMS.log.Debug("Vba32PMS.ConfigureAgent():: Agents for configure isn't found.");
                return true;
            }

            Vba32ControlCenterWrapper control = new Vba32ControlCenterWrapper("VbaTaskAssignment.Service");
            foreach (String ip in list)
            {
                control.DefaultConfigureAgent(ip);
            }

            return true;
        }

    }
}