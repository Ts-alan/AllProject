using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Tasks.Attributes;

namespace VirusBlokAda.CC.Tasks.Entities
{
    [TaskEntity("task")]
    public class SendFileTaskEntity : TaskEntity
    { 
        public SendFileTaskEntity() : base("SendFile")
        {
        
        }

        public SendFileTaskEntity(string source,string destination,string information)
            : this()
        {
            _sourceFile = source;
            _destinationFile = destination;
            _information = information;
        }

        private string _sourceFile;
        [TaskEntityStringProperty("SourceFile")]
        public string SourceFile
        {
            get { return _sourceFile; }
            set { _sourceFile= value; }
        }
        private string _destinationFile;
        [TaskEntityStringProperty("DestinationFile")]
        public string DestinationFile
        {
            get { return _destinationFile; }
            set { _destinationFile = value; }
        }
        private string _information;
        [TaskEntityStringProperty("Information")]
        public string Information
        {
            get { return _information; }
            set { _information = value; }
        }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            StringBuilder result = new StringBuilder(256);

            result.Append("<TaskSendFile>");
            result.AppendFormat(@"<Source>{0}</Source>",SourceFile);
            result.AppendFormat(@"<Destination>{0}</Destination>", DestinationFile);
            result.Append("</TaskSendFile>");

            return result.ToString();

        }
    }
}
