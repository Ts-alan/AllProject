using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
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
    internal sealed class EventsFlowManager
    {
        #region Local hearth property
        private Int32 _localHearthTimeLimit = 10;
        /// <summary>
        /// ��������� ����� ������������ �� ����
        /// </summary>
        public Int32 LocalHearthTimeLimit
        {
            get { return _localHearthTimeLimit; }
            set { _localHearthTimeLimit = value; }
        }
        private Int32 _localHearthLimit = 10;
        /// <summary>
        /// ����� ������������ �� ����
        /// </summary>
        public Int32 LocalHearthLimit
        {
            get { return _localHearthLimit; }
            set { _localHearthLimit = value; }
        }

        private Boolean _isNeedSendLocalHearthWarning = false;
        /// <summary>
        /// ������������� � ������������� ������� ���������
        /// �� ����� ���������
        /// </summary>
        public Boolean IsNeedSendLocalHearthWarning
        {
            get { return _isNeedSendLocalHearthWarning; }
            set { _isNeedSendLocalHearthWarning = value; }
        }

        #endregion

        #region Global epidemy property
        private Int32 _globalEpidemyTimeLimit = 10;
        /// <summary>
        /// ��������� ����� ������������ ����. ��������
        /// </summary>
        public Int32 GlobalEpidemyTimeLimit
        {
            get { return _globalEpidemyTimeLimit; }
            set { _globalEpidemyTimeLimit = value; }
        }
        private Int32 _globalEpidemyLimit = 10;
        /// <summary>
        /// ����� ������������ ����. ��������
        /// </summary>
        public Int32 GlobalEpidemyLimit
        {
            get { return _globalEpidemyLimit; }
            set { _globalEpidemyLimit = value; }
        }

        private Int32 _globalEpidemyCompCount = 5;
        /// <summary>
        /// ����������� ���������� ������, ��������������� ��������� ��������
        /// </summary>
        public Int32 GlobalEpidemyCompCount
        {
            get { return _globalEpidemyCompCount; }
            set { _globalEpidemyCompCount = value; }
        }

        private String _epidemyCompList = "";

        public String GlobalEpidemyCompList
        {
            get { return _epidemyCompList; }
            set { _epidemyCompList = value; }
        }

        

        /*private Boolean _isNeedSendGlobalEpidemyWarning = false;
        /// <summary>
        /// ������������� � ������������� ������� ����������� 
        /// �� ��������
        /// </summary>
        public Boolean IsNeedSendGlobalEpidemyWarning
        {
            get { return _isNeedSendGlobalEpidemyWarning; }
            set { _isNeedSendGlobalEpidemyWarning = value; }
        }*/


        #endregion

        #region Other events

        private Int32 _limit = 10;
        /// <summary>
        /// ����� ����������
        /// </summary>
        public Int32 Limit
        {
            get { return _limit; }
            set { _limit = value; }
        }

        private Int32 _timeLimit = 10;
        /// <summary>
        /// ��������� �����
        /// </summary>
        public Int32 TimeLimit
        {
            get { return _timeLimit; }
            set { _timeLimit = value; }
        }

        #endregion

        private readonly String connectionString;

        public EventsFlowManager(String connectionString)
        {
            this.connectionString = connectionString;
        }

        #region Methods

        /// <summary>
        /// ������������� �� ����� ���������
        /// </summary>
        /// <param name="currEvent"></param>
        /// <returns></returns>
        internal Boolean IsLocalHearth(EventsEntity currEvent)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetEventsCountByComputer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EventTime", currEvent.EventTime.AddMinutes(0 - this.LocalHearthTimeLimit));
                cmd.Parameters.AddWithValue("@ComputerName", currEvent.ComputerName);
                cmd.Parameters.AddWithValue("@EventName", currEvent.EventName);

                con.Open();

                Int32 result = 0;
                result = (Int32)cmd.ExecuteScalar();

                this.IsNeedSendLocalHearthWarning = result == this.LocalHearthLimit;
                return result >= this.LocalHearthLimit;
            }
        }
        
        /// <summary>
        /// ������������� �� ��������
        /// ���������� ��������� ��� �� �������� ����������
        /// </summary>
        /// <param name="currEvent"></param>
        /// <returns></returns>
        internal Boolean IsGlobalEpidemy(EventsEntity currEvent)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetEventsCountByComment", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EventTime", currEvent.EventTime.AddMinutes(0 - this.GlobalEpidemyTimeLimit));
                cmd.Parameters.AddWithValue("@Comment", currEvent.Comment);
                cmd.Parameters.AddWithValue("@EventName", currEvent.EventName);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                Int32 sum = 0;
                Int32 compCount = 0;
                while (reader.Read())
                {
                    Int32 c = reader.GetInt32(1);
                    sum += c;
                    GlobalEpidemyCompList += String.Format("{0}({1}),", reader.GetString(0), c);

                    compCount++;
                }
                reader.Close();

                if (compCount >= this.GlobalEpidemyCompCount)
                {
                    return sum >= this.GlobalEpidemyLimit;
                }

                return false;
            }
        }
        
        /// <summary>
        /// ����������, ��������� �� ����� ����������� � ���������� ��������
        /// </summary>
        /// <param name="currEvent"></param>
        /// <returns></returns>
        internal Boolean FlowAnalysis(EventsEntity currEvent)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetEventsCountByName", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EventTime", currEvent.EventTime.AddMinutes(0 - this.TimeLimit));
                cmd.Parameters.AddWithValue("@EventName", currEvent.EventName);

                con.Open();
                return (Int32)cmd.ExecuteScalar() <= this.Limit;
            }
        }

        #endregion
    }
}
