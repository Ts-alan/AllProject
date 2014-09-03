using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.DataBase;
using Interop.vsisLib;

namespace VirusBlokAda.Vba32CC.Service.VSIS
{
    public static class VSISWrapper
    {
        #region Properties

        private static ServiceClass _service;
        private static UpdateService _updateService;
        private static VSIS.Settings _settings;

        private static String _ConnectionString = String.Empty;
        public static String ConnectionString
        {
            get { return _ConnectionString; }
            set
            {
                _ConnectionString = value;
                _updateService.ConnectionString = _ConnectionString;
            }
        }

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
            
            //Wait when VSIS worked
            UInt32 status;
            _service.GetPlugStatus(out status);
            while (status != 2)
            {
                System.Threading.Thread.Sleep(100);
                _service.GetPlugStatus(out status);
            }

            try
            {
                _updateService.UpdateClass = _service.GetInterface(GUID_UpdateClass, GUID_UpdateInterface) as Update;
            }
            catch (Exception e)
            {
                throw new Exception("Cannot create VSIS.UpdateService: " + e.Message);
            }

            try
            {
                _settings.SettingsClass = _service.GetInterface(GUID_SettingClass, GUID_SettingInterface) as Interop.vsisLib.Settings;
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
            properties.ExpandPathesList = new PairString[0];
            //properties.ExpandPathesList[0].first = "WEBCONSOLE";
            //String AppPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace(@"file:///", "").Replace(@"/", @"\");
            //properties.ExpandPathesList[0].second = System.IO.Directory.GetParent(AppPath).Parent.FullName;

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
        /// Get last update
        /// </summary>
        public static UpdateEntity GetLastUpdate(UpdateStateEnum state)
        {
            switch (state)
            {
                case UpdateStateEnum.Success:
                    return _updateService.LastSuccess;
                case UpdateStateEnum.Fail:
                    return _updateService.LastFail;
                case UpdateStateEnum.Processing:
                    return _updateService.LastProcessing;
            }

            return null;
        }

        #endregion

        #endregion
    }
}
