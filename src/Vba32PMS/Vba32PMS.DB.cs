using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.Data.SqlClient;

using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;
using VirusBlokAda.CC.DataBase;
using VirusBlokAda.CC.Tasks.Service;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService
{
    /// <summary>
    /// Работа с базой данных
    /// </summary>
    partial class Vba32PMS
    {
        /// <summary>
        /// Возвращает из базы данных заданный набор данных
        /// </summary>
        /// <param name="connectionStrings">Строка подключения</param>
        /// <param name="sortExpression">Выражение Where</param>
        /// <param name="count">количество возвращаемых значений</param>
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
        /// Генерирует строку подключения к базе данных
        /// </summary>
        /// <param name="server">Сервер БД</param>
        /// <param name="user">Пользователь БД</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Строка подключения</returns>
        private String GenerateConnectionString(String server, String user, String password)
        {
            String str = String.Empty;
            try
            {
                SqlConnectionStringBuilder connStr = new SqlConnectionStringBuilder();
                connStr.UserID = user;
                connStr.Password = password;
                connStr.DataSource = server;
                connStr.PersistSecurityInfo = false;
                connStr.InitialCatalog = "vbaControlCenterDB";
                str = connStr.ConnectionString;
            
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.GenerateConnectionString():: " + ex.Message);
            }
            return str;
        }

        /// <summary>
        /// Меняет статус задачи c Delivery на DeliveryTimeout
        /// </summary>
        /// <param name="connStr">Строка подключения</param>
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
        /// Очищает базу от старых и ненужных событий
        /// </summary>
        /// <param name="connStr">Строка подключения</param>
        /// <returns></returns>
        private Boolean ClearOldEvents(String connStr)
        {
            LoggerPMS.log.Debug("Vba32PMS.ClearOldEvents():: Delete old events.");
            DateTime dtTo = DateTime.Now;
            try
            {
                Int32 tmp = 0 - daysToDelete;
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
        /// Очищает базу от старых и ненужных задач
        /// !OPTM-- методы близнецы, стоит их как-нить отрефакторить
        /// </summary>
        /// <param name="connStr">Строка подключения</param>
        /// <returns></returns>
        private Boolean ClearOldTasks(String connStr)
        {
            LoggerPMS.log.Debug("Vba32PMS.ClearOldTasks():: Delete old tasks.");
            DateTime dtTo = DateTime.Now;
            try
            {
                Int32 tmp = 0 - taskDaysToDelete;
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
        /// Очищаем базу от старых компьютеров и компьютеров с нулевым IP адресом
        /// </summary>
        /// <param name="connectionString"></param>
        private Boolean ClearOldComputers(String connStr)
        {
            LoggerPMS.log.Debug("Vba32PMS.ClearOldComputers():: Delete old computers.");
            DateTime dtTo = DateTime.Now;
            try
            {                
                Int32 tmp = 0 - compDaysToDelete;
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
        /// Сжатие базы данных
        /// </summary>
        /// <param name="connStr">Строка подключения</param>
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
        /// Отправка задач на конфигурирование агента
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