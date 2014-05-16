using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ARM2_dbcontrol.Tasks.ConfigureIntegrityCheck
{
    [Serializable]
    public class TaskConfigureIntegrityCheck
    {
        #region Fields

        private  String _Type = "IntegrityCheck";
        private List<IntegrityCheckFilesEntity> _Files;
        private List<IntegrityCheckRegistryEntity> _Registries;
        private XmlSerializer serializer;

        #endregion

        #region Properties

        public String Type
        {
            get { return _Type; }
            set { _Type = "IntegrityCheck"; }
        }
    
        public List<IntegrityCheckFilesEntity> Files
        {
            get { return _Files; }
            set { _Files = value; }
        }
        public List<IntegrityCheckRegistryEntity> Registries
        {
            get { return _Registries; }
            set { _Registries = value; }
        }

        #endregion

        #region Constructor

        public TaskConfigureIntegrityCheck()
        {
            serializer = new XmlSerializer(this.GetType());
            _Files = new List<IntegrityCheckFilesEntity>();
            _Registries = new List<IntegrityCheckRegistryEntity>();
        }

        #endregion

        #region Methods
        
        public String SaveToXml()
        {
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, this);
            return sw.ToString();
        }
        
        public void LoadFromXml(String Xml)
        {
            if (String.IsNullOrEmpty(Xml))
                return;
            TaskConfigureIntegrityCheck task;
            using (TextReader reader = new StringReader(Xml))
            {
                task = (TaskConfigureIntegrityCheck)serializer.Deserialize(reader);
            }
            this._Files = task.Files;
            this._Registries = task.Registries;
        }
        
        public String GetTask()
        {
            StringBuilder result = new StringBuilder(512);
            result.Append("<VsisCommand>");
            result.Append("<Args>");

            result.Append(@"<command><arg><key>module-id</key><value>{7C62F84A-A362-4CAA-800C-DEA89110596C}</value></arg>");
            result.Append(@"<arg><key>command</key><value>apply_settings</value></arg>");
            result.Append(@"<arg><key>settings</key><value><config><id>Normal</id><module><id>{B4A234EB-3BAF-4822-9262-2467B035856C}</id>");

            Int32 Index = 0;
            result.Append(@"<param><id>FilesPathes </id><type>stringmap</type><value>");
            for (Int32 i = 0; i < _Files.Count; i++)
            {
                if (!String.IsNullOrEmpty(_Files[i].Template))
                    result.AppendFormat(@"<string><id>{0}</id><key><![CDATA[{1}]]></key><val>{2}</val></string>", Index++, _Files[i].Path, _Files[i].Template);
                else result.AppendFormat(@"<string><id>{0}</id><key><![CDATA[{1}]]></key><val/></string>", Index++, _Files[i].Path);
            }
            result.Append(@"</value>");
            Index = 0;
            result.Append(@"<param><id>RegistryPathes</id><type>stringlist</type><value>");
            for (Int32 i = 0; i < _Registries.Count; i++)
            {
                result.AppendFormat(@"<string><id>{0}</id><val>{1}<val/></string>", Index++, _Registries[i].Path);
            }
            result.Append(@"</value>");
            result.Append(@"</param>");


            result.Append(@"</module></config></value></arg></command>");

            result.Append(@"</Args>");
            result.Append(@"<Async>0</Async>");
            result.Append(@"</VsisCommand>");

            return result.ToString();
        }

        #endregion
    }

    public struct IntegrityCheckFilesEntity
    {
        public String Path;
        public String Template;
    }

    public struct IntegrityCheckRegistryEntity
    {
        public String Path;
    }
}
