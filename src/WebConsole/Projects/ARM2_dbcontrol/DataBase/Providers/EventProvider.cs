using System;
using System.Collections.Generic;
using System.Data.Common;

namespace VirusBlokAda.CC.DataBase
{
    public class EventProvider
    {
        public const String ProviderName = "EventProvider";

        private EventsManager eventMngr;
        //private EventsFlowManager evFlowMngr;
        private EventTypesManager etMngr;

        #region Constructors
        public EventProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            eventMngr = new EventsManager(connectionString);
            //evFlowMngr = new EventsFlowManager(connection);
            etMngr = new EventTypesManager(connectionString);
        }
        public EventProvider(String connectionString,DbProviderFactory factory)
        {
            InitManagers(connectionString,factory);
        }

        private void InitManagers(String connectionString,DbProviderFactory factory)
        {
            eventMngr = new EventsManager(connectionString,factory);
            //evFlowMngr = new EventsFlowManager(connection);
            etMngr = new EventTypesManager(connectionString,factory);
        }
        public EventProvider(String connectionString,String factoryName)
        {
            InitManagers(connectionString,factoryName);
        }

        private void InitManagers(String connectionString,String factoryName)
        {
            eventMngr = new EventsManager(connectionString,factoryName);
            //evFlowMngr = new EventsFlowManager(connection);
            etMngr = new EventTypesManager(connectionString,factoryName);
        }
        #endregion
        #region Methods

        /// <summary>
        /// Get event page
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <param name="order">Sort query</param>
        /// <param name="page">Page index</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        public List<EventsEntity> List(String where, String order, Int32 page, Int32 size)
        {
            return eventMngr.List(where, order, page, size);
        }

        /// <summary>
        /// Get statistic list
        /// </summary>
        /// <param name="groupBy">GroupBy query</param>
        /// <param name="where">Filter query</param>
        /// <param name="size">Count statistics</param>
        /// <returns></returns>
        public List<StatisticEntity> GetStatistics(String groupBy, String where, Int32 size)
        {
            return eventMngr.GetStatistics(groupBy, where, size);
        }

        /// <summary>
        /// Get count of records with filter
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns></returns>
        public Int32 Count(String where)
        {
            return eventMngr.Count(where);
        }

        /// <summary>
        /// Clear old events
        /// </summary>
        /// <param name="dt">Date</param>
        public void ClearOldEvents(DateTime dt)
        {
            eventMngr.ClearOldEvents(dt);
        }

        //#region EventsFlowManager Methods

        ///// <summary>
        ///// Сигнализирует об очаге заражения
        ///// </summary>
        ///// <param name="currEvent"></param>
        ///// <returns></returns>
        //public Boolean IsLocalHearth(EventsEntity currEvent)
        //{
        //    return evFlowMngr.IsLocalHearth(currEvent);
        //}

        ///// <summary>
        ///// Сигнализирует об эпидемии
        ///// Необходимо проверить еще на разность рабстанций
        ///// </summary>
        ///// <param name="currEvent"></param>
        ///// <returns></returns>
        //public Boolean IsGlobalEpidemy(EventsEntity currEvent)
        //{
        //    return evFlowMngr.IsGlobalEpidemy(currEvent);
        //}

        ///// <summary>
        ///// Определяет, находится ли поток уведомлений в допустимых пределах
        ///// </summary>
        ///// <param name="currEvent"></param>
        ///// <returns></returns>
        //public Boolean FlowAnalysis(EventsEntity currEvent)
        //{
        //    return evFlowMngr.FlowAnalysis(currEvent);
        //}

        //#endregion

        #region EventTypesManager Methods

        /// <summary>
        /// Update color of event in database
        /// </summary>
        /// <param name="eventTypes">entity to update</param>
        public void UpdateColor(EventTypesEntity eventTypes)
        {
            etMngr.UpdateColor(eventTypes);
        }

        /// <summary>
        /// Update 'send' field of event in database
        /// </summary>
        /// <param name="eventTypes">entity to update</param>
        public void UpdateSend(EventTypesEntity eventTypes)
        {
            etMngr.UpdateSend(eventTypes);
        }

        /// <summary>
        /// Update 'send' field of event in database
        /// </summary>
        /// <param name="eventTypes">entity to update</param>
        public void UpdateNoDelete(EventTypesEntity eventTypes)
        {
            etMngr.UpdateNoDelete(eventTypes);
        }

        /// <summary>
        /// Update 'notify' field of event in database
        /// </summary>
        /// <param name="eventTypes">entity to update</param>
        public void UpdateNotify(EventTypesEntity eventTypes)
        {
            etMngr.UpdateNotify(eventTypes);
        }

        /// <summary>
        /// Get event type page
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <param name="order">Sort query</param>
        /// <param name="page">Page index</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        public List<EventTypesEntity> GetEventTypeList(String where, String order, Int16 page, Int16 size)
        {
            return etMngr.List(where, order, page, size);
        }

        /// <summary>
        /// Get count of event types
        /// </summary>
        /// <param name="where">Filter query</param>
        /// <returns>Count of event types</returns>
        public Int32 GetEventTypesCount(String where)
        {
            return etMngr.Count(where);
        }

        #endregion	

        #endregion
    }
}
