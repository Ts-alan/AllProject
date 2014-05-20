using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace ARM2_dbcontrol.Tasks.ConfigureFileCleanerCleaningTemplate
{
    [Serializable]
    public class SingleCleaningTemplate : IConfigureTask
    {
        #region Fields

        private String _Name;
        private Boolean _IsActive;
        private List<FileCleanerTemplate> _Templates;

        private XmlSerializer serializer;
        
        #endregion

        #region Properties
        

        public String Name
        {
            get{ return _Name; }
            set{ _Name = value; }
        }
        public Boolean IsActive
        { get; set; }
        public List<FileCleanerTemplate> Templates
        {
            get { return _Templates; }
            set { _Templates = value; }
        }
     

        #endregion

        #region Constructor

        public SingleCleaningTemplate()
        {
            _Name="";
            _Templates=new List<FileCleanerTemplate>();
            IsActive = false;
            serializer = new XmlSerializer(this.GetType());
        }

        public SingleCleaningTemplate(String name)
        {
            _Name = name;
            _Templates = new List<FileCleanerTemplate>();
            IsActive = false;
            serializer = new XmlSerializer(this.GetType());
        }
        public SingleCleaningTemplate(String name,List<FileCleanerTemplate> templates)
        {
            _Name = name;
            _Templates = templates;
            IsActive = false;
            serializer = new XmlSerializer(this.GetType());
        }
        public SingleCleaningTemplate(SingleCleaningTemplate sct)
        {
            _Name = sct.Name;
            _Templates = sct.Templates;
            IsActive = sct.IsActive;
            serializer = new XmlSerializer(this.GetType());
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

            SingleCleaningTemplate task;
            using (TextReader reader = new StringReader(Xml))
            {
                task = (SingleCleaningTemplate)serializer.Deserialize(reader);
            }
            this._IsActive = task.IsActive;
            this._Templates = task.Templates;
            this._Name = task.Name;      
        }

        public String GetTask()
        {
            StringBuilder task = new StringBuilder(256);

            task.Append("<key>" + Name + "</key>");
            task.Append("<val>");
            for (Int32 i = 0; i < _Templates.Count; i++)
            {
                task.AppendFormat("{0}|{1}", _Templates[i].Path, _Templates[i].FileName);
                if (i != _Templates.Count - 1)
                {
                    task.Append("|");
                }
            }
            task.Append("</val>");
            return task.ToString();
        }
        #endregion

        #region IConfigureTask Members


        public void LoadFromRegistry(string reg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public struct FileCleanerTemplate
    {
        public String Path;
        public String FileName;
    }
}