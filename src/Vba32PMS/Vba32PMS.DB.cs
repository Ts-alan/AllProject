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
        private List<EventsEntity> GetEventsFromDb(string connStr, string sortExpression, int count)
        {
            LogMessage("Vba32PMS.GetEventsFromDb()::�������� ������� �� ���� ������");
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
                LogError("Vba32PMS.GetEventsFromDb()::������ ��� ������� �������� ������ �� ��: " + ex.Message,
                    EventLogEntryType.Error);
                return null;
            
            }
            return list;
        }

        /// <summary>
        /// ���������� ������ ����������� � ���� ������
        /// </summary>
        /// <param name="server">������ ��</param>
        /// <param name="user">������������ ��</param>
        /// <param name="password">������ ������������</param>
        /// <returns>������ �����������</returns>
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
                LogError("Vba32PMS.GenerateConnectionString()::������ ��� ������������ ������ �����������: " + ex.Message,
                  EventLogEntryType.Error);
            }
            return str;
        }

        /// <summary>
        /// ������ ������ ������ c Delivery �� DeliveryTimeout
        /// </summary>
        /// <param name="connStr">������ �����������</param>
        /// <returns></returns>
        private bool CheckDeliveryState(string connStr)
        {
            LogMessage("Vba32PMS.CheckDeliveryState()::������ ������ �������� �����");
            DateTime dtTo = DateTime.Now;
            try
            {
                dtTo = dtTo.AddMinutes(-1);
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.CheckDeliveryState()::������ ��� ������������ ������-�������: " + ex.Message,
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

                    //����� ��������������� �������� ���������
                    db.ChangeDeliveryState(dtTo);

                    conn.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.CheckDeliveryState()::������ ��� ������� � ��: " + ex.Message,
                    EventLogEntryType.Error);

                return false;
            }
            return true;
        }

        /// <summary>
        /// ������� ���� �� ������ � �������� �������
        /// </summary>
        /// <param name="connStr">������ �����������</param>
        /// <returns></returns>
        private bool ClearOldEvents(string connStr)
        {
            LogMessage("Vba32PMS.ClearOldEvents()::������� ������ �������");
            DateTime dtTo = DateTime.Now;
            try
            {
                int tmp = 0 - daysToDelete;
                dtTo = dtTo.AddDays(tmp);
            }
            catch (Exception ex)
            {

                LogError("Vba32PMS.ClearOldEvents()::������ ��� ������������ ������-�������: " + ex.Message,
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

                    //����� ��������������� �������� ���������
                    db.ClearOldEvents(dtTo);

                    conn.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.ClearOldEvents()::������ ��� ������� � ��: " + ex.Message,
                     EventLogEntryType.Error);

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
        private bool ClearOldTasks(string connStr)
        {
            LogMessage("Vba32PMS.ClearOldTasks()::������� ������ ������");
            DateTime dtTo = DateTime.Now;
            try
            {
                int tmp = 0 - taskDaysToDelete;
                dtTo = dtTo.AddDays(tmp);
            }
            catch (Exception ex)
            {

                LogError("Vba32PMS.ClearOldTasks()::������ ��� ������������ ������-�������: " + ex.Message,
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

                    //����� ��������������� �������� ���������
                    db.ClearOldTasks(dtTo);

                    conn.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.ClearOldTasks()::������ ��� ������� � ��: " + ex.Message,
                     EventLogEntryType.Error);

                return false;
            }
            return true;
        }




        /// <summary>
        /// ������ ���� ������
        /// </summary>
        /// <param name="connStr">������ �����������</param>
        /// <returns></returns>
        private bool CompressDB(string connStr)
        {
            LogMessage("Vba32PMS.CompressDB()::������ ���� ������");
           
            try
            {
                //EventsManager.ShrinkDataBase(connStr, 10);
            }
            catch (Exception ex)
            {
                LogError("Vba32PMS.CompressDB()::������ ��� ������� � ��: " + ex.Message,
                     EventLogEntryType.Error);

                return false;
            }
            return true;
        }

    }
}