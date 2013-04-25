using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Web;
using System.Xml.Schema;
using System.Diagnostics;

namespace VirusBlokAda.RemoteOperations.MsiInfo
{
    public class Vba32MsiStorage
    {
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
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

        public void Read()
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                XmlSchema schema = new XmlSchema();
                schema.SourceUri = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\Vba32Versions.xsd");
                settings.Schemas.Add(schema);
                settings.ValidationType = ValidationType.Schema;
                XmlReader reader = XmlReader.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\Vba32MSI.xml"), settings);
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                document.Validate(eventHandler);
                XmlNodeList nodes = document.GetElementsByTagName("path");
                foreach (XmlNode next in nodes)
                {
                    string path = next.InnerText;
                    string version = next.Attributes["version"].Value;
                    dict.Add(version, path);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading or parsing Vba32MSI.xml " + ex.Message);
            }
        }

        public void Write(Dictionary<string, string> _dict)
        {
            Read();
            try
            {
                XmlTextWriter writer = new XmlTextWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\Vba32MSI.xml"), Encoding.UTF8);
                
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();

                writer.WriteStartElement("vba32msi", "Settings");
                foreach (KeyValuePair<string, string> pair in _dict)
                {
                    writer.WriteStartElement("path");
                    writer.WriteAttributeString("version", pair.Key);
                    writer.WriteString(pair.Value);
                    writer.WriteEndElement();

                    string val;
                    if (dict.TryGetValue(pair.Key, out val))
                    {
                        if (!pair.Value.Equals(val))
                        {
                            DeleteFile(dict[pair.Key]);
                        }
                    }
                }
                writer.WriteEndElement();

                writer.WriteEndDocument();
                writer.Close();

                dict = _dict;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error writing Vba32MSI.xml " + ex.Message);
            }
        }

        public string GetPathMSI(string version)
        {
            string path;
            if (dict.TryGetValue(version, out path))
            {
                return path;
            }

            else
            {
                return String.Empty;
            }
        }

        public static string GetVba32VersionByOSVersion(string osversion)
        {
            string version = String.Empty;
            string osversionl = osversion.ToLower();
            if (osversionl.Contains("windows"))
            {
                if (osversionl.Contains("server"))
                {
                    if (osversionl.Contains("2008"))
                        version = Vba32VersionInfo.Vba32Vis;
                    else version = Vba32VersionInfo.Vba32NTS;
                }
                else
                {
                    if (osversionl.Contains("vista") || osversionl.Contains(" 7 "))
                        version = Vba32VersionInfo.Vba32Vista;
                    else version = Vba32VersionInfo.Vba32NTW;
                }
            }

            return version;
        }

        private void DeleteFile(string filename)
        {
            if (String.IsNullOrEmpty(filename)) return;
            string fullname = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Downloads\" + filename);
            try
            {
                FileInfo TheFile = new FileInfo(fullname);
                if (TheFile.Exists)
                {
                    TheFile.Delete();
                }
            }
            catch { }
        }

        public static bool IsMSI(string filename)
        {
            if (String.IsNullOrEmpty(filename)) return false;

            string extension = Path.GetExtension(filename);

            if (".msi".Equals(extension.ToLower()))
            {
                return true;
            }

            return false;
        }
    }
}