using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ARM2_dbcontrol.Tasks.ConfigureAgent
{
    public class TaskConfigureAgent
    {
        private String _configFile = String.Empty;
        public String ConfigFile
        {
            get { return _configFile; }
            set { _configFile = value; }
        }

        #region Constructors
        public TaskConfigureAgent()
        { }

        public TaskConfigureAgent(String configFile)
        {
            _configFile = configFile;
        }
        #endregion

        /// <summary>
        /// Get XML for agent
        /// </summary>
        /// <returns></returns>
        public String BuildTask()
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
            IndexBegin = fileContent.IndexOf(server) + server.Length;
            IndexEnd = fileContent.IndexOf(EndLine, IndexBegin);
            args[0] = fileContent.Substring(IndexBegin, IndexEnd - IndexBegin);

            //Get PublicKey
            IndexBegin = fileContent.IndexOf(publicKey) + publicKey.Length;
            IndexEnd = fileContent.IndexOf(EndLine, IndexBegin);
            args[1] = fileContent.Substring(IndexBegin, IndexEnd - IndexBegin);
            if (args[1].Length < 264)
                throw new Exception("PublicKey isn't valid.");
            args[1] = args[1].Substring(0, 264);

            //Get ServerMask
            IndexBegin = fileContent.IndexOf(mask) + mask.Length;
            IndexEnd = fileContent.IndexOf(EndLine, IndexBegin);
            args[2] = fileContent.Substring(IndexBegin, IndexEnd - IndexBegin);

            if (args[0] == server || args[1] == publicKey)
                return false;

            return true;
        }
    }
}
