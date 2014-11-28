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
        /// <summary>
        /// Сохранение в чьд
        /// </summary>
        /// <returns></returns>
        public String SaveToXml()
        {
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, this);
            return sw.ToString();
        }
        /// <summary>
        /// Загрузка из xml
        /// </summary>
        /// <param name="Xml"></param>
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
        /// <summary>
        /// Выдача задач
        /// </summary>
        /// <returns></returns>
        public String GetTask()
        {

            StringBuilder task = new StringBuilder(256);
            task.Append("<param>");
            task.Append(GetTaskType(FileCleanerTemplateType.File));
            task.Append("</param>");
            task.Append("<param>");
            task.Append(GetTaskType(FileCleanerTemplateType.Registry));
            task.Append("</param>");
            
            return task.ToString();
        }
        /// <summary>
        /// Получение типа задачи
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private String GetTaskType(FileCleanerTemplateType type)
        {
            String ID = "";
            StringBuilder task = new StringBuilder(128);
            if (type == FileCleanerTemplateType.File)
                ID = "CleaningTemplateFiles_" + Name;
            else ID = "CleaningTemplateRegistry_" + Name;

            task.Append("<id>" + ID + "</id>");
            task.Append("<type>stringmap</type><value>");
            Int32 j = 0;
            for (Int32 i = 0; i < _Templates.Count; i++)
            {
                if (_Templates[i].Type == type)
                {
                    task.Append("<string>");
                    task.AppendFormat("<id>{0}</id><key>{1}</key><val>{2}</val>", j, _Templates[i].Path, _Templates[i].FileName);
                    task.AppendFormat("</string>");
                    j++;
                }
            }
            task.Append("</value>");
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
        public FileCleanerTemplateType Type;
    }
    public enum FileCleanerTemplateType
    {
        File,
        Registry
    }
}