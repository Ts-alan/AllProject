using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    /// <summary>
    /// Организовывает интеллектуальную обработку потока уведомлений
    /// LocalHearthTimeLimit - Временной порог срабатывания очага заражения 
    /// LocalHearthLimit - Порог срабатывания очага заражения
    /// GlobalEpidemyTimeLimit - Временной порог срабатывания эпидемии
    /// GlobalEpidemyLimit - Порог срабатывания эпидемии
    /// GlobalEpidemyCompCount - Минимальное количество компов, удовлетворяющих критериям эпидемии
    /// Limit - Порог-ограничитель событий
    /// TimeLimit - Временной порог-ограничитель событий
    /// </summary>
    internal sealed class EventsFlowManager
    {
        #region Local hearth property
        private Int32 _localHearthTimeLimit = 10;
        /// <summary>
        /// Временной порог срабатывания на очаг
        /// </summary>
        public Int32 LocalHearthTimeLimit
        {
            get { return _localHearthTimeLimit; }
            set { _localHearthTimeLimit = value; }
        }
        private Int32 _localHearthLimit = 10;
        /// <summary>
        /// Порог срабатывания на очаг
        /// </summary>
        public Int32 LocalHearthLimit
        {
            get { return _localHearthLimit; }
            set { _localHearthLimit = value; }
        }

        private Boolean _isNeedSendLocalHearthWarning = false;
        /// <summary>
        /// Сигнализирует о необходимости отсылки сообщения
        /// об очаге заражения
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
        /// Временной порог срабатывания глоб. эпидемии
        /// </summary>
        public Int32 GlobalEpidemyTimeLimit
        {
            get { return _globalEpidemyTimeLimit; }
            set { _globalEpidemyTimeLimit = value; }
        }
        private Int32 _globalEpidemyLimit = 10;
        /// <summary>
        /// Порог срабатывания глоб. эпидемии
        /// </summary>
        public Int32 GlobalEpidemyLimit
        {
            get { return _globalEpidemyLimit; }
            set { _globalEpidemyLimit = value; }
        }

        private Int32 _globalEpidemyCompCount = 5;
        /// <summary>
        /// Минимальное количество компов, удовлетворяющих критериям эпидемии
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
        /// Сигнализирует о необходимости послать уведомление 
        /// об эпидемии
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
        /// Порог блокировки
        /// </summary>
        public Int32 Limit
        {
            get { return _limit; }
            set { _limit = value; }
        }

        private Int32 _timeLimit = 10;
        /// <summary>
        /// Временной порог
        /// </summary>
        public Int32 TimeLimit
        {
            get { return _timeLimit; }
            set { _timeLimit = value; }
        }

        #endregion

        private VlslVConnection database;

        public EventsFlowManager(VlslVConnection conn)
        {
            this.database = conn;
        }

        #region Methods

        /// <summary>
        /// Сигнализирует об очаге заражения
        /// </summary>
        /// <param name="currEvent"></param>
        /// <returns></returns>
        internal Boolean IsLocalHearth(EventsEntity currEvent)
        {
            Int32 result = 0;

            IDbCommand command = database.CreateCommand("GetEventsCountByComputer", true);

            database.AddCommandParameter(command, "@EventTime", DbType.DateTime, currEvent.EventTime.AddMinutes(0 - this.LocalHearthTimeLimit), ParameterDirection.Input);
            database.AddCommandParameter(command, "@ComputerName", DbType.String, currEvent.ComputerName, ParameterDirection.Input);
            database.AddCommandParameter(command, "@EventName", DbType.String, currEvent.EventName, ParameterDirection.Input);

            result = (Int32)command.ExecuteScalar();

            this.IsNeedSendLocalHearthWarning = result == this.LocalHearthLimit;
            return result >= this.LocalHearthLimit;
        }
        
        /// <summary>
        /// Сигнализирует об эпидемии
        /// Необходимо проверить еще на разность рабстанций
        /// </summary>
        /// <param name="currEvent"></param>
        /// <returns></returns>
        internal Boolean IsGlobalEpidemy(EventsEntity currEvent)
        {
            IDbCommand command = database.CreateCommand("GetEventsCountByComment", true);

            database.AddCommandParameter(command, "@EventTime", DbType.DateTime, currEvent.EventTime.AddMinutes(0 - this.GlobalEpidemyTimeLimit), ParameterDirection.Input);
            database.AddCommandParameter(command, "@Comment", DbType.String, currEvent.Comment, ParameterDirection.Input);
            database.AddCommandParameter(command, "@EventName", DbType.String, currEvent.EventName, ParameterDirection.Input);

            SqlDataReader reader = command.ExecuteReader() as SqlDataReader;

            Int32 sum = 0;
            Int32 compCount = 0;
            while (reader.Read())
            {
                Int32 c = reader.GetInt32(1);
                sum += c;
                GlobalEpidemyCompList += String.Format("{0}({1}),", reader.GetString(0),c);
                
                compCount++;
            }
            reader.Close();
            
            if (compCount >= this.GlobalEpidemyCompCount)
            {
                return sum >= this.GlobalEpidemyLimit;
            }

            return false;
        }
        
        /// <summary>
        /// Определяет, находится ли поток уведомлений в допустимых пределах
        /// </summary>
        /// <param name="currEvent"></param>
        /// <returns></returns>
        internal Boolean FlowAnalysis(EventsEntity currEvent)
        {
            IDbCommand command = database.CreateCommand("GetEventsCountByName", true);

            database.AddCommandParameter(command, "@EventTime", DbType.DateTime, currEvent.EventTime.AddMinutes(0 - this.TimeLimit), ParameterDirection.Input);
            database.AddCommandParameter(command, "@EventName", DbType.String, currEvent.EventName, ParameterDirection.Input);

            return (Int32)command.ExecuteScalar() <= this.Limit;
        }

        #endregion
    }
}
