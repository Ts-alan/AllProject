using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Tasks.Attributes;

namespace VirusBlokAda.CC.Tasks.Entities
{
    [TaskEntity("task")]
    public class UninstallTaskEntity : TaskEntity
    { 
        public UninstallTaskEntity() : base("Uninstall")
        {
        
        }




        private int _selectedIndex;
        [TaskEntityInt32Property("SelectedIndex")]
        public Int32 SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            String version = String.Empty;
            String cmd = String.Empty;
 /*           switch (SelectedIndex)
            {
                case 0:
                    version = Vba32MsiStorage.GetVba32VersionByOSVersion(osVersion);
                    break;
                case 1:
                    version = Vba32VersionInfo.Vba32RemoteConsoleScanner;
                    break;
                case 2:
                    version = Vba32VersionInfo.Vba32Antivirus;
                    break;
            }

            cmd= String.Format("msiexec.exe /x \"{0}\" /q /norestart", Vba32VersionInfo.GetGuid(version));*/
            StringBuilder content = new StringBuilder();
            content.Append("<TaskCreateProcess>");
            content.AppendFormat(@"<CommandLine>{0}</CommandLine>",cmd );
            content.Append(@"</TaskCreateProcess>");

            return content.ToString();
        }
    }
}
