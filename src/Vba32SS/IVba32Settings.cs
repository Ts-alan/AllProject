using System;
using System.Collections.Generic;
using System.Text;

namespace Vba32.ControlCenter.SettingsService
{
    public interface IVba32Settings
    {
        bool ChangeRegistry(string xml);
    }
}
