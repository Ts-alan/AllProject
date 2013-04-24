using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace Vba32CC
{
    /// <summary>
    /// �������������� ���������������� ��������� ������ �����������
    /// LocalHearthTimeLimit - ��������� ����� ������������ ����� ��������� 
    /// LocalHearthLimit - ����� ������������ ����� ���������
    /// GlobalEpidemyTimeLimit - ��������� ����� ������������ ��������
    /// GlobalEpidemyLimit - ����� ������������ ��������
    /// GlobalEpidemyCompCount - ����������� ���������� ������, ��������������� ��������� ��������
    /// Limit - �����-������������ �������
    /// TimeLimit - ��������� �����-������������ �������
    /// </summary>
    class EventsFlowManager
    {

        #region Local hearth property
        private int _localHearthTimeLimit = 10;
        /// <summary>
        /// ��������� ����� ������������ �� ����
        /// </summary>
        public int LocalHearthTimeLimit
        {
            get { return _localHearthTimeLimit; }
            set { _localHearthTimeLimit = value; }
        }
        private int _localHearthLimit = 10;
        /// <summary>
        /// ����� ������������ �� ����
        /// </summary>
        public int LocalHearthLimit
        {
            get { return _localHearthLimit; }
            set { _localHearthLimit = value; }
        }

        private bool _isNeedSendLocalHearthWarning = false;
        /// <summary>
        /// ������������� � ������������� ������� ���������
        /// �� ����� ���������
        /// </summary>
        public bool IsNeedSendLocalHearthWarning
        {
            get { return _isNeedSendLocalHearthWarning; }
            set { _isNeedSendLocalHearthWarning = value; }
        }

        #endregion

        #region Global epidemy property
        private int _globalEpidemyTimeLimit = 10;
        /// <summary>
        /// ��������� ����� ������������ ����. ��������
        /// </summary>
        public int GlobalEpidemyTimeLimit
        {
            get { return _globalEpidemyTimeLimit; }
            set { _globalEpidemyTimeLimit = value; }
        }
        private int _globalEpidemyLimit = 10;
        /// <summary>
        /// ����� ������������ ����. ��������
        /// </summary>
        public int GlobalEpidemyLimit
        {
            get { return _globalEpidemyLimit; }
            set { _globalEpidemyLimit = value; }
        }

        private int _globalEpidemyCompCount = 5;
        /// <summary>
        /// ����������� ���������� ������, ��������������� ��������� ��������
        /// </summary>
        public int GlobalEpidemyCompCount
        {
            get { return _globalEpidemyCompCount; }
            set { _globalEpidemyCompCount = value; }
        }

        private string _epidemyCompList = "";

        public string GlobalEpidemyCompList
        {
            get { return _epidemyCompList; }
            set { _epidemyCompList = value; }
        }

        

        /*private bool _isNeedSendGlobalEpidemyWarning = false;
        /// <summary>
        /// ������������� � ������������� ������� ����������� 
        /// �� ��������
        /// </summary>
        public bool IsNeedSendGlobalEpidemyWarning
        {
            get { return _isNeedSendGlobalEpidemyWarning; }
            set { _isNeedSendGlobalEpidemyWarning = value; }
        }*/


        #endregion

        #region Other events

        private int _limit = 10;
        /// <summary>
        /// ����� ����������
        /// </summary>
        public int Limit
        {
            get { return _limit; }
            set { _limit = value; }
        }

        private int _timeLimit = 10;
        /// <summary>
        /// ��������� �����
        /// </summary>
        public int TimeLimit
        {
            get { return _timeLimit; }
            set { _timeLimit = value; }
        }

        #endregion


        SqlConnection connection = null;


        public EventsFlowManager(SqlConnection conn)
        {
            this.connection = conn;
        }


        /// <summary>
        /// ������������� �� ����� ���������
        /// </summary>
        /// <param name="currEvent"></param>
        /// <returns></returns>
        public bool IsLocalHearth(EventsEntity currEvent)
        {
            int result = 0;

            /*string query = "SELECT COUNT(*) FROM Events " +
            "INNER JOIN Computers ON Computers.[ID]=Events.ComputerID " +
            "INNER JOIN EventTypes ON EventTypes.[ID] = Events.[EventID] " +
            "WHERE EventTime > @EventTime AND ComputerName = @ComputerName AND EventName = @EventName";
            */
            SqlCommand command = new SqlCommand("GetEventsCountByComputer", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EventTime",
                currEvent.EventTime.AddMinutes(0 - this.LocalHearthTimeLimit));
            command.Parameters.AddWithValue("@ComputerName", currEvent.Computer);
            command.Parameters.AddWithValue("@EventName", currEvent.Event);

            result = (int)command.ExecuteScalar();

            this.IsNeedSendLocalHearthWarning = result == this.LocalHearthLimit;
            return result >= this.LocalHearthLimit;
        }
        /// <summary>
        /// ������������� �� ��������
        /// ���������� ��������� ��� �� �������� ����������
        /// </summary>
        /// <param name="currEvent"></param>
        /// <returns></returns>
        public bool IsGlobalEpidemy(EventsEntity currEvent)
        {
            System.Diagnostics.Debug.WriteLine("IsGlobalEpidemy called");

            /*string query = "SELECT ComputerName, Count(*) FROM Events " +
            "INNER JOIN EventTypes ON EventTypes.[ID] = Events.[EventID] " +
            "INNER JOIN Computers ON Computers.[ID] = Events.[ComputerID] " +
            "WHERE EventTime > @EventTime AND Comment = @Comment AND EventName = @EventName "+
            "GROUP BY ComputerName";*/

            SqlCommand command = new SqlCommand("GetEventsCountByComment", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EventTime",
                currEvent.EventTime.AddMinutes(0 - this.GlobalEpidemyTimeLimit));
            command.Parameters.AddWithValue("@Comment", currEvent.Comment);
            command.Parameters.AddWithValue("@EventName", currEvent.Event);

            //result = (int)command.ExecuteScalar();
            SqlDataReader reader = command.ExecuteReader();

            int sum = 0;
            int compCount = 0;
            while (reader.Read())
            {
                int c = reader.GetInt32(1);
                sum += c;
                GlobalEpidemyCompList += String.Format("{0}({1}),", reader.GetString(0),c);
                
                compCount++;
            }
            reader.Close();
            System.Diagnostics.Debug.WriteLine(String.Format("(compCount) {0} >= {1} (this.GlobalEpidemyCompCount); sum={2}. EpidemyLimit={3}",
                compCount,GlobalEpidemyCompCount,sum,GlobalEpidemyLimit));
            if (compCount >= this.GlobalEpidemyCompCount)
            {
                return sum >= this.GlobalEpidemyLimit;
            }

            return false;
        }
        /// <summary>
        /// ����������, ��������� �� ����� ����������� � ���������� ��������
        /// </summary>
        /// <param name="currEvent"></param>
        /// <returns></returns>
        public bool FlowAnalysis(EventsEntity currEvent)
        {

            /*string query = "SELECT COUNT(*) FROM Events " +
            "INNER JOIN EventTypes ON EventTypes.[ID] = Events.[EventID] " +
            "WHERE EventTime > @EventTime AND EventName = @EventName";*/

            SqlCommand command = new SqlCommand("GetEventsCountByName", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EventTime",
                currEvent.EventTime.AddMinutes(0 - this.TimeLimit));
            command.Parameters.AddWithValue("@EventName", currEvent.Event);

            return (int)command.ExecuteScalar() <= this.Limit;
        }
    }
}
