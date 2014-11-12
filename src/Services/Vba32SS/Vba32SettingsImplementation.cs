using System;
using VirusBlokAda.CC.Settings.Common;

namespace Vba32.ControlCenter.SettingsService
{
    internal class Vba32SettingsImplementation : MarshalByRefObject, IVba32Settings
    {
        /// <summary>
        /// Ёкспортируемый метод
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public Boolean ChangeRegistry(String xml)
        {
            LoggerSS.log.Info("Vba32SettingsImplementation.ChangeRegistry():: Enter.");
            try
            {
                VirusBlokAda.CC.Settings.SettingsProvider.WriteSettings(xml);
            }
            catch (Exception ex)
            {
                LoggerSS.log.Error("Vba32SettingsImplementation.ChangeRegistry()::" + ex.Message);
                return false;
            }
            return true;
        }
    }
}
