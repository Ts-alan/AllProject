using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class PParserProvider
    {
        public const String ProviderName = "PParserProvider";

        private PParserManager pMngr;
        private EventsFlowManager eMngr;

        public PParserProvider(String connectionString)
        {
            InitManagers(connectionString);
        }

        private void InitManagers(String connectionString)
        {
            pMngr = new PParserManager(connectionString);
            eMngr = new EventsFlowManager(connectionString);
        }

        #region EventsFlowManager properties

        #region Local hearth property
        
        /// <summary>
        /// Временной порог срабатывания на очаг
        /// </summary>
        public Int32 LocalHearthTimeLimit
        {
            get { return eMngr.LocalHearthTimeLimit; }
            set { eMngr.LocalHearthTimeLimit = value; }
        }
        
        /// <summary>
        /// Порог срабатывания на очаг
        /// </summary>
        public Int32 LocalHearthLimit
        {
            get { return eMngr.LocalHearthLimit; }
            set { eMngr.LocalHearthLimit = value; }
        }

        /// <summary>
        /// Сигнализирует о необходимости отсылки сообщения
        /// об очаге заражения
        /// </summary>
        public Boolean IsNeedSendLocalHearthWarning
        {
            get { return eMngr.IsNeedSendLocalHearthWarning; }
            set { eMngr.IsNeedSendLocalHearthWarning = value; }
        }

        #endregion

        #region Global epidemy property
        /// <summary>
        /// Временной порог срабатывания глоб. эпидемии
        /// </summary>
        public Int32 GlobalEpidemyTimeLimit
        {
            get { return eMngr.GlobalEpidemyTimeLimit; }
            set { eMngr.GlobalEpidemyTimeLimit = value; }
        }
        
        /// <summary>
        /// Порог срабатывания глоб. эпидемии
        /// </summary>
        public Int32 GlobalEpidemyLimit
        {
            get { return eMngr.GlobalEpidemyLimit; }
            set { eMngr.GlobalEpidemyLimit = value; }
        }

        /// <summary>
        /// Минимальное количество компов, удовлетворяющих критериям эпидемии
        /// </summary>
        public Int32 GlobalEpidemyCompCount
        {
            get { return eMngr.GlobalEpidemyCompCount; }
            set { eMngr.GlobalEpidemyCompCount = value; }
        }

        public String GlobalEpidemyCompList
        {
            get { return eMngr.GlobalEpidemyCompList; }
            set { eMngr.GlobalEpidemyCompList = value; }
        }

        #endregion

        #region Other events

        /// <summary>
        /// Порог блокировки
        /// </summary>
        public Int32 Limit
        {
            get { return eMngr.Limit; }
            set { eMngr.Limit = value; }
        }

        /// <summary>
        /// Временной порог
        /// </summary>
        public Int32 TimeLimit
        {
            get { return eMngr.TimeLimit; }
            set { eMngr.TimeLimit = value; }
        }

        #endregion

        #endregion

        #region Methods

        #region PParserManager

        /// <summary>
        /// Delete process info
        /// </summary>
        /// <param name="computer_name"></param>
        public void DeleteProcessInfo(String computer_name)
        {
            pMngr.DeleteProcessInfo(computer_name);
        }

        /// <summary>
        /// Insert component settings by computer name
        /// </summary>
        /// <param name="computer_name"></param>
        /// <param name="component_name"></param>
        /// <param name="settings"></param>
        public void InsertComponentSettings(String computer_name, String component_name, String settings)
        {
            pMngr.InsertComponentSettings(computer_name, component_name, settings);
        }

        /// <summary>
        /// Insert component state
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="componentName"></param>
        /// <param name="componentState"></param>
        /// <param name="version"></param>
        public void InsertComponentState(String computerName, String componentName, String componentState, String version)
        {
            pMngr.InsertComponentState(computerName, componentName, componentState, version);
        }

        /// <summary>
        /// Insert event
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="eventName"></param>
        /// <param name="eventTime"></param>
        /// <param name="componentName"></param>
        public void InsertEvent(EventsEntity ev, Int16 licenseCount)
        {
            //A device has been inserted. 
            //We must check whether the device registered in the database

            //!!! -- проверить атрибут VDD_INSERTED!
            if (ev.EventName == "JE_VDD_AUDIT_USB")
            {
                pMngr.OnDeviceInsert(ev, licenseCount);
                pMngr.ModifyDeviceEvent(ev);
            }

            pMngr.InsertEvent(ev);
        }

        /// <summary>
        /// Insert event without notify
        /// </summary>
        /// <param name="ev">EventsEntity</param>
        public void InsertEventWithoutNotify(EventsEntity ev)
        {
            pMngr.InsertEvent(ev);
        }

        /// <summary>
        /// Insert process info
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="processName"></param>
        /// <param name="memorySize"></param>
        public void InsertProcessInfo(String computerName, String processName, Int32 memorySize)
        {
            pMngr.InsertProcessInfo(computerName, processName, memorySize);
        }

        /// <summary>
        /// Insert system info
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="licenceCount"></param>
        public void InsertSystemInfo(ComputersEntity comp, Int16 licenceCount)
        {
            pMngr.InsertSystemInfo(comp, licenceCount);
        }

        /// <summary>
        /// Insert task state
        /// </summary>
        /// <param name="task"></param>
        public void InsertTaskState(TaskEntity task)
        {
            pMngr.InsertTaskState(task);
        }

        /// <summary>
        /// Insert component settings by MAC address
        /// </summary>
        /// <param name="macAddress"></param>
        /// <param name="componentName"></param>
        /// <param name="settings"></param>
        public void InsertSettings(String macAddress, String componentName, String settings)
        {
            pMngr.InsertSettings(macAddress, componentName, settings);
        }

        /// <summary>
        /// Get computer IP-address by computer name
        /// </summary>
        /// <param name="computerName"></param>
        /// <returns></returns>
        public String GetComputerIPAddress(String computerName)
        {
            return pMngr.GetComputerIPAddress(computerName);
        }

        /// <summary>
        /// Get event type notify
        /// </summary>
        /// <param name="event_name"></param>
        /// <returns></returns>
        public Boolean GetEventTypeNotify(String event_name)
        {
            return pMngr.GetEventTypeNotify(event_name);
        }

        #endregion

        #region EventsFlowManager

        /// <summary>
        /// Сигнализирует об очаге заражения
        /// </summary>
        /// <param name="currEvent"></param>
        /// <returns></returns>
        public Boolean IsLocalHearth(EventsEntity currEvent)
        {
            return eMngr.IsLocalHearth(currEvent);
        }

        /// <summary>
        /// Сигнализирует об эпидемии
        /// Необходимо проверить еще на разность рабстанций
        /// </summary>
        /// <param name="currEvent"></param>
        /// <returns></returns>
        public Boolean IsGlobalEpidemy(EventsEntity currEvent)
        {
            return eMngr.IsGlobalEpidemy(currEvent);
        }

        /// <summary>
        /// Определяет, находится ли поток уведомлений в допустимых пределах
        /// </summary>
        /// <param name="currEvent"></param>
        /// <returns></returns>
        public Boolean FlowAnalysis(EventsEntity currEvent)
        {
            return eMngr.FlowAnalysis(currEvent);
        }

        #endregion

        #endregion
    }
}
