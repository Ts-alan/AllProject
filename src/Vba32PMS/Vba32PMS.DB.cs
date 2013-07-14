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
        private List<EventsEntity> GetEventsFromDb(String connStr, String sortExpression, Int32 count)
        {
            Logger.Debug("Vba32PMS.GetEventsFromDb()::�������� ������� �� ���� ������");
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
                Logger.Error("Vba32PMS.GetEventsFromDb()::������ ��� ������� �������� ������ �� ��: " + ex.Message);
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
                Logger.Error("Vba32PMS.GenerateConnectionString()::������ ��� ������������ ������ �����������: " + ex.Message);
            }
            return str;
        }

        /// <summary>
        /// ������ ������ ������ c Delivery �� DeliveryTimeout
        /// </summary>
        /// <param name="connStr">������ �����������</param>
        /// <returns></returns>
        private Boolean CheckDeliveryState(String connStr)
        {
            Logger.Debug("Vba32PMS.CheckDeliveryState()::������ ������ �������� �����");
            DateTime dtTo = DateTime.Now;
            try
            {
                dtTo = dtTo.AddMinutes(-1);
            }
            catch (Exception ex)
            {
                Logger.Error("Vba32PMS.CheckDeliveryState()::������ ��� ������������ ������-�������: " + ex.Message);
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
                Logger.Error("Vba32PMS.CheckDeliveryState()::������ ��� ������� � ��: " + ex.Message);
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
            Logger.Debug("Vba32PMS.ClearOldEvents()::������� ������ �������");
            DateTime dtTo = DateTime.Now;
            try
            {
                Int32 tmp = 0 - daysToDelete;
                dtTo = dtTo.AddDays(tmp);
            }
            catch (Exception ex)
            {
                Logger.Error("Vba32PMS.ClearOldEvents()::������ ��� ������������ ������-�������: " + ex.Message);
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
                Logger.Error("Vba32PMS.ClearOldEvents()::������ ��� ������� � ��: " + ex.Message);
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
            Logger.Debug("Vba32PMS.ClearOldTasks()::������� ������ ������");
            DateTime dtTo = DateTime.Now;
            try
            {
                Int32 tmp = 0 - taskDaysToDelete;
                dtTo = dtTo.AddDays(tmp);
            }
            catch (Exception ex)
            {
                Logger.Error("Vba32PMS.ClearOldTasks()::������ ��� ������������ ������-�������: " + ex.Message);
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
                Logger.Error("Vba32PMS.ClearOldTasks()::������ ��� ������� � ��: " + ex.Message);
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
            Logger.Debug("Vba32PMS.CompressDB()::������ ���� ������");
           
            try
            {
                EventsManager.ShrinkDataBase(connStr, 10);
            }
            catch (Exception ex)
            {
                Logger.Error("Vba32PMS.CompressDB()::������ ��� ������� � ��: " + ex.Message);
                return false;
            }
            return true;
        }

    }
}