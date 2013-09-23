﻿using System;
using System.Collections.Generic;
using System.Text;
using vsisLib;

namespace VirusBlokAda.Vba32CC.Service.VSIS
{
    public static class VSISWrapper
    {
        #region Properties

        private static ServiceClass _service;
        private static UpdateService _updateService;
        private static VSIS.Settings _settings;

        #region Guids

        private const String GUID_SettingClass      = "{7C62F84A-A362-4CAA-800C-DEA89110596C}";
        private const String GUID_UpdateClass       = "{D4041472-FEC0-41B5-A133-8AAC758C1006}";
        private const String GUID_SettingInterface  = "{D1E89994-789D-41D7-B5C3-65F8B3894D12}";
        private const String GUID_UpdateInterface   = "{421441B5-54BC-475A-A799-B14DE10C383C}";

        #endregion

        #endregion

        #region Constructors

        static VSISWrapper()
        {
            _updateService = new VSIS.UpdateService();
            _settings = new VSIS.Settings();
            Initialize();
        }

        #endregion

        #region Methods

        public static void Initialize()
        {
            try
            {
                _service = new ServiceClass();
            }
            catch (Exception e)
            {
                throw new Exception("Cannot create vsisLib.ServiceClass: " + e.Message);
            }
            
            try
            {
                _updateService.UpdateClass = _service.GetInterface(GUID_UpdateClass, GUID_UpdateInterface) as vsisLib.Update;
            }
            catch (Exception e)
            {
                throw new Exception("Cannot create VSIS.UpdateService: " + e.Message);
            }

            try
            {
                _settings.SettingsClass = _service.GetInterface(GUID_SettingClass, GUID_SettingInterface) as vsisLib.Settings;
            }
            catch (Exception e)
            {
                throw new Exception("Cannot create VSIS.Settings: " + e.Message);
            }
        }

        #region Settings

        public static void SetUpdateParameters(UpdateProperties properties)
        {
            properties.ProxyType = 0;
            properties.ExpandPathesList = new PairString[1];
            properties.ExpandPathesList[0].first = "WEBCONSOLE";
            properties.ExpandPathesList[0].second = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\WebConsole"; //@"D:\Public\Vba32 Control Center\WebConsole\";

            _settings.SetUpdateParameters(GUID_UpdateClass, properties, 1, true);
        }

        public static UpdateProperties GetUpdateParameters()
        {
            return _settings.GetUpdateParameters(GUID_UpdateClass);
        }

        #endregion

        #region Update

        public static void SetUpdateEvents(UpdateEvents events)
        {
            _updateService.SetEvents(events);
        }

        /// <summary>
        /// Update Vba32ControlCenter
        /// </summary>
        /// <param name="properties">properties for update</param>
        public static void Update()
        {
            _updateService.Update();
        }

        public static void UpdateAbort()
        {
            _updateService.IsAbort = true;
        }

        /// <summary>
        /// Get update abort flag
        /// </summary>
        public static Boolean IsUpdateAbort
        {
            get { return _updateService.IsAbort; }
        }

        /// <summary>
        /// Get update alive flag
        /// </summary>
        public static Boolean IsUpdateAlive
        {
            get { return _updateService.IsAlive; }
        }

        /// <summary>
        /// Get last update date
        /// </summary>
        public static DateTime LastUpdate
        {
            get { return _updateService.LastUpdate; }
        }

        /// <summary>
        /// Get update stop reason
        /// </summary>
        public static String UpdateStopReason
        {
            get { return _updateService.StopReason; }
        }

        #endregion

        #endregion
    }
}