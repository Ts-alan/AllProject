using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Web;
using System.Xml.Schema;
using System.Diagnostics;
using System.Security.Cryptography;

namespace VirusBlokAda.RemoteOperations.MsiInfo
{
    public static class Vba32MsiStorage
    {
        private static Dictionary<String, FileEntity> dict = new Dictionary<String, FileEntity>();
        private static readonly String DirPath;
        private static readonly String XsdName = "Vba32Versions.xsd";
        private static readonly String XmlName = "Vba32MSI.xml";
        private static DateTime LastRead;

        static Vba32MsiStorage()
        {
            DirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\");
            Read(); 
        }

        /// <summary>
        /// Get actually data if it's need.
        /// </summary>
        private static void CheckRead()
        {
            if (DateTime.Now.Subtract(LastRead) > new TimeSpan(0, 5, 0))
                Read();
        }

        private static void ValidationEventHandler(Object sender, ValidationEventArgs e)
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

        /// <summary>
        /// Read MSI pathes from xml file.
        /// </summary>
        private static void Read()
        {
            XmlReader reader = null;
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                XmlSchema schema = new XmlSchema();
                schema.SourceUri = Path.Combine(DirPath, XsdName);
                settings.Schemas.Add(schema);
                settings.ValidationType = ValidationType.Schema;
                reader = XmlReader.Create(Path.Combine(DirPath, XmlName), settings);
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                document.Validate(eventHandler);
                XmlNodeList nodes = document.GetElementsByTagName("path");
                dict.Clear();
                foreach (XmlNode next in nodes)
                {
                    String path = next.InnerText;
                    String version = next.Attributes["version"].Value;
                    dict.Add(version, new FileEntity(path, CalculateHash(path)));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading or parsing Vba32MSI.xml " + ex.Message);
            }
            finally
            {
                LastRead = DateTime.Now;
                if (reader != null)
                    reader.Close();
            }
        }


        //public void Write(Dictionary<String, String> _dict)
        //{
        //    Read();
        //    try
        //    {
        //        XmlTextWriter writer = new XmlTextWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\Vba32MSI.xml"), Encoding.UTF8);
                
        //        writer.Formatting = Formatting.Indented;
        //        writer.WriteStartDocument();

        //        writer.WriteStartElement("vba32msi", "Settings");
        //        foreach (KeyValuePair<String, String> pair in _dict)
        //        {
        //            writer.WriteStartElement("path");
        //            writer.WriteAttributeString("version", pair.Key);
        //            writer.WriteString(pair.Value);
        //            writer.WriteEndElement();

        //            String val;
        //            if (dict.TryGetValue(pair.Key, out val))
        //            {
        //                if (!pair.Value.Equals(val))
        //                {
        //                    DeleteFile(dict[pair.Key]);
        //                }
        //            }
        //        }
        //        writer.WriteEndElement();

        //        writer.WriteEndDocument();
        //        writer.Close();

        //        dict = _dict;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Error writing Vba32MSI.xml " + ex.Message);
        //    }
        //}

        /// <summary>
        /// Get MSI path by product name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String GetPathMSI(String name)
        {
            CheckRead();
            FileEntity fEnt;
            if (dict.TryGetValue(name, out fEnt))
            {
                return fEnt.FileName;
            }

            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Get anti-virus version by OS name
        /// </summary>
        /// <param name="osversion"></param>
        /// <returns></returns>
        public static String GetVba32VersionByOSVersion(String osversion)
        {
            String version = String.Empty;
            String osversionl = osversion.ToLower();
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

        public static String GetArgsByOSVersion(Boolean isServer)
        {
            return isServer ? "/qb ADDLOCAL=VSIS,VPP,VMT,VKW,VAS,VGI" : "/qb ADDLOCAL=VSIS,VPP,VMT,VAS,VGI";
        }

        /// <summary>
        /// Delete file.
        /// </summary>
        /// <param name="filename"></param>
        private static void DeleteFile(String filename)
        {
            if (String.IsNullOrEmpty(filename)) return;
            String fullname = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Installs\" + filename);
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

        /// <summary>
        /// Indicates whether the specified file is MSI or not.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Boolean IsMSI(String filename)
        {
            if (String.IsNullOrEmpty(filename)) 
                return false;

            String extension = Path.GetExtension(filename);

            if (".msi".Equals(extension.ToLower()))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculate hash by file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static String CalculateHash(String fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return String.Empty;
            String path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Installs\" + fileName);;
            using (MD5 md5 = MD5.Create())
            {
                Byte[] hash;
                using (FileStream stream = File.OpenRead(path))
                {
                    hash = md5.ComputeHash(stream);
                }

                if (hash == null) 
                    return String.Empty;

                StringBuilder sBuilder = new StringBuilder();
                for (Int32 i = 0; i < hash.Length; i++)
                    sBuilder.Append(hash[i].ToString("x2"));

                return sBuilder.ToString().ToUpper();
            }
        }

        /// <summary>
        /// Get Hash of file by product name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String GetHash(String name)
        {
            CheckRead();
            FileEntity fEnt;
            if (dict.TryGetValue(name, out fEnt))
            {
                return fEnt.Hash;
            }

            else
            {
                return String.Empty;
            }            
        }
    }


    internal class FileEntity
    {
        #region Properties

        private String _fileName = String.Empty;
        public String FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private String _hash = String.Empty;
        public String Hash
        {
            get { return _hash; }
            set { _hash = value; }
        }

        #endregion

        #region Constructors
        
        public FileEntity()
        { }

        public FileEntity(String fileName, String hash)
        {
            this._fileName = fileName;
            this._hash = hash;
        }

        #endregion
    }
}