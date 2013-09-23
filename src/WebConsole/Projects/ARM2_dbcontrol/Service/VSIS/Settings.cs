using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.Vba32CC.Service.VSIS
{
    internal class Settings
    {
        #region Properties

        private vsisLib.Settings _settings;
        internal vsisLib.Settings SettingsClass
        {
            get { return _settings; }
            set { _settings = value; }
        }

        #endregion

        #region Constructors

        internal Settings() { }

        #endregion

        #region Methods

        internal void SetParameter(String module_id, String parameter_id, Object parameter, UInt32 settingsType)
        {
            _settings.SetParameter(module_id, parameter_id, parameter, settingsType);
        }

        /// <summary>
        /// Set update parameters
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="properties"></param>
        /// <param name="settingsType">default value = 1</param>
        /// <param name="isRewrite">true - remove old parameters, false - save old parameters and add new parameters</param>
        internal void SetUpdateParameters(String module_id, UpdateProperties properties, UInt32 settingsType, Boolean isRewrite)
        {
            if (!String.IsNullOrEmpty(properties.AuthorityName))
                _settings.SetParameter(module_id, "AuthorityName", properties.AuthorityName, settingsType);
            else
                _settings.SetParameter(module_id, "AuthorityName", "", settingsType);

            if (!String.IsNullOrEmpty(properties.AuthorityPassword))
                _settings.SetParameter(module_id, "AuthorityPassword", properties.AuthorityPassword, settingsType);
            else
                _settings.SetParameter(module_id, "AuthorityPassword", "", settingsType);


            if (!String.IsNullOrEmpty(properties.ProxyAddress))
                _settings.SetParameter(module_id, "ProxyAddress", properties.ProxyAddress, settingsType);
            else
                _settings.SetParameter(module_id, "ProxyAddress", "", settingsType);

            if (!String.IsNullOrEmpty(properties.ProxyAuthorityName))
                _settings.SetParameter(module_id, "ProxyAuthorityName", properties.ProxyAuthorityName, settingsType);
            else
                _settings.SetParameter(module_id, "ProxyAuthorityName", "", settingsType);

            if (!String.IsNullOrEmpty(properties.ProxyAuthorityPassword))
                _settings.SetParameter(module_id, "ProxyAuthorityPassword", properties.ProxyAuthorityPassword, settingsType);
            else
                _settings.SetParameter(module_id, "ProxyAuthorityPassword", "", settingsType);

            if (properties.ProxyPort > 0)
                _settings.SetParameter(module_id, "ProxyPort", properties.ProxyPort, settingsType);
            else
                _settings.SetParameter(module_id, "ProxyPort", 0, settingsType);

            _settings.SetParameter(module_id, "ProxyType", properties.ProxyType, settingsType);


            if (!String.IsNullOrEmpty(properties.TempFolder))
                _settings.SetParameter(module_id, "TempFolder", properties.TempFolder, settingsType);


            if (properties.UpdatePathes != null && properties.UpdatePathes.Length > 0)
            {
                List<String> pathes = new List<String>(properties.UpdatePathes);
                if (!isRewrite)
                {
                    Object oldPathes;
                    UInt32 oldSettingsType;
                    _settings.GetParameter(module_id, "UpdatePathes", out oldPathes, out oldSettingsType);
                    if (oldPathes != null)
                        pathes.AddRange((String[])oldPathes);

                }
                _settings.SetParameter(module_id, "UpdatePathes", pathes.ToArray(), settingsType);
            }
            //else
            //    _settings.SetParameter(module_id, "UpdatePathes", new String[0], settingsType);


            if (properties.ExpandPathesList != null && properties.ExpandPathesList.Length > 0)
            {
                List<vsisLib.PairString> list = new List<vsisLib.PairString>(properties.ExpandPathesList);
                if (!isRewrite)
                {
                    /*
                    Object oldList;
                    UInt32 oldSettingsType;
                    _settings.GetParameter(module_id, "ExpandPathesList", out oldList, out oldSettingsType);
                    if (oldList != null)
                        list.AddRange((vsisLib.PairString[])oldList);
                    */

                }
                _settings.SetParameter(module_id, "ExpandPathesList", list.ToArray(), settingsType);
            }
            //else
            //    _settings.SetParameter(module_id, "ExpandPathesList", new vsisLib.PairString[0], settingsType);
        }

        /// <summary>
        /// Get update parameters
        /// </summary>
        /// <param name="module_id"></param>
        /// <returns></returns>
        internal UpdateProperties GetUpdateParameters(String module_id)
        {
            UpdateProperties properties = new UpdateProperties();
            Object parameter;
            UInt32 settingsType;
            _settings.GetParameter(module_id, "AuthorityName", out parameter, out settingsType);
            properties.AuthorityName = (String)parameter;
            _settings.GetParameter(module_id, "AuthorityPassword", out parameter, out settingsType);
            properties.AuthorityPassword = (String)parameter;

            _settings.GetParameter(module_id, "ProxyAddress", out parameter, out settingsType);
            properties.ProxyAddress = (String)parameter;
            _settings.GetParameter(module_id, "ProxyAuthorityName", out parameter, out settingsType);
            properties.ProxyAuthorityName = (String)parameter;
            _settings.GetParameter(module_id, "ProxyAuthorityPassword", out parameter, out settingsType);
            properties.ProxyAuthorityPassword = (String)parameter;
            _settings.GetParameter(module_id, "ProxyPort", out parameter, out settingsType);
            if(parameter != null) properties.ProxyPort = (UInt32)parameter;
            _settings.GetParameter(module_id, "ProxyType", out parameter, out settingsType);
            if (parameter != null) properties.ProxyType = (UInt32)parameter;

            _settings.GetParameter(module_id, "TempFolder", out parameter, out settingsType);
            properties.TempFolder = (String)parameter;

            _settings.GetParameter(module_id, "UpdatePathes", out parameter, out settingsType);
            if (parameter != null) properties.UpdatePathes = (String[])parameter;

            //_settings.GetParameter(module_id, "ExpandPathesList", out parameter, out settingsType);
            //if (parameter != null) properties.ExpandPathesList = (vsisLib.PairString[])parameter;
            
            return properties;
        }

        #endregion
    }

    /*
        vsis.exe --parameter-set {D4041472-FEC0-41B5-A133-8AAC758C1006} AuthorityName string fsz_builder@VBADOMAIN   
        vsis.exe --parameter-set {D4041472-FEC0-41B5-A133-8AAC758C1006} AuthorityPassword string builder3

        ::vsis.exe --parameter-set {D4041472-FEC0-41B5-A133-8AAC758C1006} ProxyAuthorityName string proxy_name
        ::vsis.exe --parameter-set {D4041472-FEC0-41B5-A133-8AAC758C1006} ProxyAuthorityPassword string proxy_password

        vsis.exe --parameter-set {D4041472-FEC0-41B5-A133-8AAC758C1006} ProxyType ulong 0
        ::vsis.exe --parameter-set {D4041472-FEC0-41B5-A133-8AAC758C1006} ProxyAddress string proxy.com
        ::vsis.exe --parameter-set {D4041472-FEC0-41B5-A133-8AAC758C1006} ProxyPort ulong 34567

        vsis.exe --parameter-set {D4041472-FEC0-41B5-A133-8AAC758C1006} UpdatePathes stringlist "\\sborka\Vba32\release_usb\update"

        ::vsis.exe --parameter-set {D4041472-FEC0-41B5-A133-8AAC758C1006} TempFolder string C:\Windows\Temp
        vsis.exe --parameter-set {D4041472-FEC0-41B5-A133-8AAC758C1006} ExpandPathesList stringmap WEBCONSOLE "c:\Program Files\Vba32 Control Center\WebConsole"    
     
     */
}
