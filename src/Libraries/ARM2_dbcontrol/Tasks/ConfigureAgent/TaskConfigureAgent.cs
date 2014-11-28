using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using VirusBlokAda.CC.Common.Xml;

namespace ARM2_dbcontrol.Tasks
{
    public class TaskConfigureAgent : IConfigureTask
    {
        #region Fields

        public const String TaskType = "ConfigureAgent";
        private String _Vba32CCUser;
        private String _configFile = String.Empty;

        #endregion

        #region Properties

        public String ConfigFile
        {
            get { return _configFile; }
            set { _configFile = value; }
        }

        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }

        #endregion

        #region Constructors
        public TaskConfigureAgent()
        { }

        public TaskConfigureAgent(String configFile)
        {
            _configFile = configFile;
        }
        #endregion
        
        /// <summary>
        /// Parse config file
        /// </summary>
        /// <param name="ConfigFile"></param>
        /// <param name="args">Set server, publicKey and server mask values</param>
        /// <returns></returns>
        private Boolean ParseConfigFile(String ConfigFile, ref String[] args)
        {
            String fileContent = String.Empty;
            using (StreamReader reader = new StreamReader(ConfigFile))
            {
                fileContent = reader.ReadToEnd();
                reader.Close();
            }

            String publicKey = "PublicKey=";
            String server = "Server=";
            String mask = "ServerMask=";

            Int32 IndexBegin = 0;
            Int32 IndexEnd = 0;
            String EndLine = "\r\n";

            //Get Server
            if (fileContent.IndexOf(server) < 0)
                throw new Exception("Server isn't found.");
            IndexBegin = fileContent.IndexOf(server) + server.Length;
            IndexEnd = fileContent.IndexOf(EndLine, IndexBegin);
            args[0] = fileContent.Substring(IndexBegin, IndexEnd - IndexBegin);

            //Get PublicKey
            if (fileContent.IndexOf(publicKey) < 0)
                throw new Exception("PublicKey isn't found.");
            IndexBegin = fileContent.IndexOf(publicKey) + publicKey.Length;
            IndexEnd = fileContent.IndexOf(EndLine, IndexBegin);
            args[1] = fileContent.Substring(IndexBegin, IndexEnd - IndexBegin);
            if (args[1].Length < 264)
                throw new Exception("PublicKey isn't valid.");
            args[1] = args[1].Substring(0, 264);

            //Get ServerMask
            if (fileContent.IndexOf(mask) > -1)
            {
                IndexBegin = fileContent.IndexOf(mask) + mask.Length;
                IndexEnd = fileContent.IndexOf(EndLine, IndexBegin);
                args[2] = fileContent.Substring(IndexBegin, IndexEnd - IndexBegin);
            }

            if (args[0] == server || args[1] == publicKey)
                return false;

            return true;
        }

        #region IConfigureTask Members
        /// <summary>
        /// Сохранить в xml
        /// </summary>
        /// <returns></returns>
        public String SaveToXml()
        {
            XmlBuilder xml = new XmlBuilder("ConfigureAgent");
            xml.Top = String.Empty;
            xml.AddNode("Vba32CCUser", Vba32CCUser);
            xml.AddNode("Type", TaskType);
            String path = String.Format(@"<{0}>{1}</{0}>", "ConfigPath", ConfigFile);
            xml.AddNode("Content", path + GetTask());
            xml.Generate();

            return xml.Result;
        }
        /// <summary>
        /// Загрузить из Xml
        /// </summary>
        /// <param name="xml">xml</param>
        public void LoadFromXml(String xml)
        {
            XmlTaskParser parser = new XmlTaskParser(xml);

            ConfigFile = parser.GetXmlTagContent("ConfigPath");
        }

        public void LoadFromRegistry(String reg)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get XML for agent
        /// </summary>
        /// <returns></returns>
        public String GetTask()
        {
            StringBuilder result = new StringBuilder(256);
            String[] args = new String[3];
            if (!ParseConfigFile(ConfigFile, ref args))
                throw new Exception("Config file isn't valid.");
            result.Append("<ConfigureWithServer>");
            result.AppendFormat(@"<Server>{0}</Server>", args[0]);
            result.AppendFormat(@"<PublicKey>{0}</PublicKey>", args[1]);
            if (!String.IsNullOrEmpty(args[2]))
                result.AppendFormat(@"<ServerMask>{0}</ServerMask>", args[2]);
            result.Append(@"</ConfigureWithServer>");

            return result.ToString();
        }

        #endregion
    }
}
