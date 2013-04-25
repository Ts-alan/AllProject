using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace VirusBlokAda.RemoteOperations.MsiInfo
{
    public static class Vba32VersionInfo
    {
        private static Dictionary<string, string> dict = new Dictionary<string, string>();

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Debug.WriteLine("Error validating Vba32Versions.xml " + e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Debug.WriteLine("Warning validating Vba32Versions.xml " + e.Message);
                    break;
            }

        }

        static Vba32VersionInfo()
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                XmlSchema schema = new XmlSchema();
                schema.SourceUri = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\Vba32Versions.xsd");
                settings.Schemas.Add(schema);
                settings.ValidationType = ValidationType.Schema;
                XmlReader reader = XmlReader.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\Vba32Versions.xml"), settings);
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                document.Validate(eventHandler);
                XmlNodeList nodes = document.GetElementsByTagName("version");
                foreach (XmlNode next in nodes)
                {
                    string version = next.InnerText;
                    string guid = next.Attributes["guid"].Value;
                    dict.Add(version, guid);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading or parsing Vba32Versions.xml " + ex.Message);
            }

        }
        public static string GetGuid(string version)
        {
            string guid;
            if (dict.TryGetValue(version, out guid))
            {
                return guid;
            }

            else
            {
                return "unknown";
            }
        }

        #region Versions

        public static readonly string Vba32NTW = "Vba32 WinNT Workstation";
        public static readonly string Vba32NTS = "Vba32 WinNT Server";
        public static readonly string Vba32Vista = "Vba32 for Windows Vista";
        public static readonly string Vba32Vis = "Vba32 for Windows Server 2008";
        public static readonly String Vba32RemoteConsoleScanner = "Vba32 Remote Console Scanner";

        #endregion
    }

}