using System;
using System.Collections.Generic;
using System.Text;
using vsisLib;

namespace VirusBlokAda.Vba32CC.Service.VSIS
{
    public class VSISWrapper
    {
        #region Properties

        protected ServiceClass _service;
        private UpdateService _updateService;
        private VSIS.Settings _settings;

        #region Guids

        private const String GUID_SettingClass      = "{7C62F84A-A362-4CAA-800C-DEA89110596C}";
        private const String GUID_UpdateClass       = "{D4041472-FEC0-41B5-A133-8AAC758C1006}";
        private const String GUID_SettingInterface  = "{D1E89994-789D-41D7-B5C3-65F8B3894D12}";
        private const String GUID_UpdateInterface   = "{421441B5-54BC-475A-A799-B14DE10C383C}";

        #endregion

        #endregion

        #region Constructors

        public VSISWrapper()
        {
            _service = new ServiceClass();
        }

        #endregion

        #region Methods

        #region Settings

        public void SetUpdateParameters(UpdateProperties properties)
        {
            CreateSettingsInstance();
            _settings.SetUpdateParameters(GUID_UpdateClass, properties, 1, false);
        }

        public UpdateProperties GetUpdateParameters()
        {
            CreateSettingsInstance();
            return _settings.GetUpdateParameters(GUID_UpdateClass);
        }

        /// <summary>
        /// Create Settings class & vsisLib.Settings instance if no exists
        /// </summary>
        protected void CreateSettingsInstance()
        {
            if (_settings == null)
            {
                try
                {
                    _settings = new VSIS.Settings(_service.GetInterface(GUID_SettingClass, GUID_SettingInterface) as vsisLib.Settings);
                }
                catch (Exception e)
                {
                    throw new Exception("Cannot create VSIS.Settings: " + e.Message);
                }
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Create UpdateService class & vsisLib.Update instance if no exists
        /// </summary>
        protected void CreateUpdateInstance()
        {
            if (_updateService == null)
            {
                try
                {
                    _updateService = new VSIS.UpdateService(_service.GetInterface(GUID_UpdateClass, GUID_UpdateInterface) as vsisLib.Update);
                }
                catch (Exception e)
                {
                    throw new Exception("Cannot create VSIS.UpdateService: " + e.Message);
                }
            }
        }

        public void SetUpdateEvents(UpdateEvents events)
        {
            CreateUpdateInstance();
            _updateService.SetEvents(events);
        }

        /// <summary>
        /// Update Vba32ControlCenter
        /// </summary>
        /// <param name="properties">properties for update</param>
        public void Update()
        {
            CreateUpdateInstance();
            _updateService.Update();
        }

        #endregion

        #endregion
    }
}
