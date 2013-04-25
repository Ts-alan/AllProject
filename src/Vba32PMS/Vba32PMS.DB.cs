using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.Data.SqlClient;

using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;
using Vba32.ControlCenter.PeriodicalMaintenanceService.DataBase;

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
        private List<EventsEntity> GetEventsFromDb(string connStr, string sortExpression, int count)
        {
            LogMessage("Vba32PMS.GetEventsFromDb()::Получаем события из базы данных");
            List<EventsEntity> list = new List<EventsEntity>();
            try
            {
                using (VlslVConnection conn = new VlslVConnection(connStr))
                {
                    EventsManager db = new EventsManager(conn);
                    conn.OpenConnection();
                    conn.CheckConnectionState(true);

                    list = db.List(sortExpression, "EventTime ASC", 1, count);
                    conn.CloseConnection();
                }
            }
            catch(Exception ex)
            {
                LogError("Vba32PMS.GetEventsFromDb()::Ошибка при попытке получить данные из БД: " + ex.Message,
                    EventLogEntryType.Error);
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
        private string GenerateConnectionString(string server, string user, string password)
        {
            string str = String.Empty;
            try
            {
                //str =
                //    String.Format("packet size=4096;user id={0};password={1};data source={2};persist security info=False;initial catalog=vbaControlCenterDB", user, password, server);

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
                LogError("Vba32PMS.GenerateConnectionString()::Ошибка при формировании строки подключения: " + ex.Message,
                  EventLogEntryType.Error);
            }
            return str;
        }

        /// <summary>
        /// Меняет статус задачи c Delivery на DeliveryTimeout
        /// </summary>
        /// <param name="connStr">Строка подключения</param>
        /// <returns></returns>
        private bool CheckDeliveryState(string connStr)
        {
            LogMessage("Vba32PMS.CheckDeliveryState()::Меняем статус зависших задач");
            DateTime dtTo = DateTime.Now;
            try
            {
                dtTo = dtTo.AddMinutes(-1);
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.CheckDeliveryState()::Ошибка при формировании строки-фильтра: " + ex.Message,
                    EventLogEntryType.Error);
                return false;
            }

            try
            {
                using (VlslVConnection conn = new VlslVConnection(connStr))
                {
                    EventsManager db = new EventsManager(conn);
                    conn.OpenConnection();
                    conn.CheckConnectionState(true);

                    //Вызов соответствующей хранимой процедуры
                    db.ChangeDeliveryState(dtTo);

                    conn.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.CheckDeliveryState()::Ошибка при запросе к БД: " + ex.Message,
                    EventLogEntryType.Error);

                return false;
            }
            return true;
        }

        /// <summary>
        /// Очищает базу от старых и ненужных событий
        /// </summary>
        /// <param name="connStr">Строка подключения</param>
        /// <returns></returns>
        private bool ClearOldEvents(string connStr)
        {
            LogMessage("Vba32PMS.ClearOldEvents()::Удаляем старые события");
            DateTime dtTo = DateTime.Now;
            try
            {
                int tmp = 0 - daysToDelete;
                dtTo = dtTo.AddDays(tmp);
            }
            catch (Exception ex)
            {

                LogError("Vba32PMS.ClearOldEvents()::Ошибка при формировании строки-фильтра: " + ex.Message,
                    EventLogEntryType.Error);
                return false;
            }

            try
            {
                using (VlslVConnection conn = new VlslVConnection(connStr))
                {
                    EventsManager db = new EventsManager(conn);
                    conn.OpenConnection();
                    conn.CheckConnectionState(true);

                    //Вызов соответствующей хранимой процедуры
                    db.ClearOldEvents(dtTo);

                    conn.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.ClearOldEvents()::Ошибка при запросе к БД: " + ex.Message,
                     EventLogEntryType.Error);

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
        private bool ClearOldTasks(string connStr)
        {
            LogMessage("Vba32PMS.ClearOldTasks()::Удаляем старые задачи");
            DateTime dtTo = DateTime.Now;
            try
            {
                int tmp = 0 - taskDaysToDelete;
                dtTo = dtTo.AddDays(tmp);
            }
            catch (Exception ex)
            {

                LogError("Vba32PMS.ClearOldTasks()::Ошибка при формировании строки-фильтра: " + ex.Message,
                    EventLogEntryType.Error);
                return false;
            }

            try
            {
                using (VlslVConnection conn = new VlslVConnection(connStr))
                {
                    EventsManager db = new EventsManager(conn);
                    conn.OpenConnection();
                    conn.CheckConnectionState(true);

                    //Вызов соответствующей хранимой процедуры
                    db.ClearOldTasks(dtTo);

                    conn.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.ClearOldTasks()::Ошибка при запросе к БД: " + ex.Message,
                     EventLogEntryType.Error);

                return false;
            }
            return true;
        }




        /// <summary>
        /// Сжатие базы данных
        /// </summary>
        /// <param name="connStr">Строка подключения</param>
        /// <returns></returns>
        private bool CompressDB(string connStr)
        {
            LogMessage("Vba32PMS.CompressDB()::Сжатие базы данных");
           
            try
            {
                //EventsManager.ShrinkDataBase(connStr, 10);
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.CompressDB()::Ошибка при запросе к БД: " + ex.Message,
                     EventLogEntryType.Error);

                return false;
            }
            return true;
        }

    }
}