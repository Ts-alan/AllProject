using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Tasks.Attributes;
using VirusBlokAda.CC.Tasks.Common;
using System.Xml;
using System.Reflection;
using System.Web;
using VirusBlokAda.CC.Common;
using System.Xml.Serialization;

namespace VirusBlokAda.CC.Tasks.Entities
{
    [XmlInclude(typeof(CreateProcessTaskEntity)), XmlInclude(typeof(ConfigureLoaderTaskEntity)), XmlInclude(typeof(ConfigurePasswordTaskEntity)), XmlInclude(typeof(UninstallTaskEntity)),
    XmlInclude(typeof(RestoreFileFromQtnTaskEntity)), XmlInclude(typeof(InstallProductTaskEntity)), XmlInclude(typeof(ChangeDeviceProtectTaskEntity)),XmlInclude(typeof(SendFileTaskEntity))]
    public abstract class TaskEntity
    {
        #region Properties
        private string _type;
        [TaskEntityStringProperty("Type")]
        public string Type
        {
            get { return _type; }
        }
        #endregion

        #region Constructor
        public TaskEntity(string type)
        {
            _type = type;
        }

        protected TaskEntity()
        { 
        }
        #endregion

        #region Public Methods
        public string ToXmlString()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = CreateXmlWriter(sb);
            try
            {

                writer.WriteStartDocument();

                writer.WriteStartElement(RootElementTag());

                foreach (StringPair next in PropertyElements())
                {
                    writer.WriteStartElement(next.Name);
                    writer.WriteString(next.Value);
                    writer.WriteEndElement();
                }

                writer.WriteStartElement(Vba32CCUserElementTag());
                writer.WriteString(Vba32CCUserElementValue());
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }
            finally
            {
                writer.Close();
            }
            return sb.ToString();
        }

        public abstract string ToTaskXml();

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            else
            {
                PropertyInfo[] properties = GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    object prop1 = property.GetValue(this, null);
                    object prop2 = property.GetValue(obj, null);
                    if (prop1 == null)
                    {
                        if (prop2 != null)
                        {
                            return false;
                        }
                        continue;
                    }                    
                    if (!prop1.Equals(prop2))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Instance Private Methods
        private TaskEntityAttribute TaskEntityAttribute()
        {
            Type type = this.GetType();
            Attribute attribute = Attribute.GetCustomAttribute(type, typeof(TaskEntityAttribute));
            if (attribute == null)
            {
                throw new Exception(String.Format("{0} not marked with TaskEntity attribute", type.Name));
            }
            return (attribute as TaskEntityAttribute);
        }

        private string RootElementTag()
        {
            return TaskEntityAttribute().root;
        }
        private string Vba32CCUserElementTag()
        {
            return "Vba32CCUser";
        }

        private string Vba32CCUserElementValue()
        {
            return GetUserInfo();
        }

        private List<StringPair> PropertyElements()
        {
            List<StringPair> elements = new List<StringPair>();

            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                StringPair element = PropertyElement(property);
                if (element != null)
                {
                    elements.Add(element);
                }
            }
            return elements;
        }

        private StringPair PropertyElement(PropertyInfo property)
        {
            TaskEntityPropertyAttribute tAttribute = GetPropertyAttribute(property);
            if (tAttribute == null)
            {
                return null;
            }

            if (!PropertyElementDependsOnTrueProperty(tAttribute.dependOnTrueProperty))
            {
                return null;
            }

            StringPair element = new StringPair();
            element.Name = tAttribute.name;

            string sValue = null;
            if (tAttribute is TaskEntityStringPropertyAttribute)
            {
                if (!StringElementValue(tAttribute as TaskEntityStringPropertyAttribute, property,
                    out sValue))
                {
                    return null;
                }
            }
            else if (tAttribute is TaskEntityBooleanPropertyAttribute)
            {
                BooleanElementValue(tAttribute as TaskEntityBooleanPropertyAttribute, property,
                    out sValue);
            }
            else if (tAttribute is TaskEntityInt32PropertyAttribute)
            {
                Int32ElementValue(tAttribute as TaskEntityInt32PropertyAttribute, property,
                    out sValue);
            }

            element.Value = Format(sValue, tAttribute.format);

            return element;
        }

        private void Int32ElementValue(TaskEntityInt32PropertyAttribute attribute, 
            PropertyInfo property, out String sValue)
        {
            CheckInt32Property(property);
            Int32 iValue = (Int32)property.GetValue(this, null);
            sValue = iValue.ToString();
        }

        private void BooleanElementValue(TaskEntityBooleanPropertyAttribute attribute, 
            PropertyInfo property, out String sValue)
        {
            CheckBooleanProperty(property);
            Boolean bValue = (Boolean)property.GetValue(this, null);
            sValue = bValue ? attribute.replaceTrue : attribute.replaceFalse;
        }

        private bool StringElementValue(TaskEntityStringPropertyAttribute attribute, 
            PropertyInfo property, out String sValue)
        {
            CheckStringProperty(property);
            sValue = (String)property.GetValue(this, null);
            return (attribute.allowNullOrEmpty || !String.IsNullOrEmpty(sValue));
        }

        private bool PropertyElementDependsOnTrueProperty(string property)
        {
            if (property == null)
            {
                return true;
            }

            Type type = this.GetType();
            PropertyInfo dependProperty = type.GetProperty(property);
            if (dependProperty == null)
            {
                throw new Exception(String.Format(
                    "Type {0} marked with TaskEntityPropertyAttribute attribute don't have {1} property",
                    type.Name, property));
            }
            bool? dependPropertyValue = dependProperty.GetValue(this, null) as bool?;
            if (dependPropertyValue == null)
            {
                throw new Exception(String.Format(
                    "Type {0} marked with TaskEntityPropertyAttribute attribute property {1} is not of Bool type",
                    type.Name, property));
            }
            return (bool)dependPropertyValue;
        }
        #endregion

        #region Private Static Methods
        private static string GetUserInfo()
        {
            return String.Format("{0} ({1} {2}) {3}",
                HttpContext.Current.Profile.UserName,
                HttpContext.Current.Profile.GetPropertyValue("FirstName"),
                HttpContext.Current.Profile.GetPropertyValue("LastName"),
                HttpContext.Current.Request.UserHostAddress);
        }

        private static TaskEntityPropertyAttribute GetPropertyAttribute(PropertyInfo property)
        {
            Attribute attribute = Attribute.GetCustomAttribute(property, typeof(TaskEntityPropertyAttribute));
            if (attribute == null)
            {
                return null;
            }
            return (attribute as TaskEntityPropertyAttribute);
        }
        private static void CheckInt32Property(PropertyInfo property)
        {
            if (property.PropertyType != typeof(Int32))
            {
                throw new Exception("Property marked with TaskEntityInt32PropertyAttribute is not of Int32 type.");
            }
        }

        private static void CheckBooleanProperty(PropertyInfo property)
        {
            if (property.PropertyType != typeof(Boolean))
            {
                throw new Exception("Property marked with TaskEntityBooleanPropertyAttribute is not of Boolean type.");
            }
        }

        private static void CheckStringProperty(PropertyInfo property)
        {
            if (property.PropertyType != typeof(String))
            {
                throw new Exception("Property marked with TaskEntityStringPropertyAttribute is not of String type.");
            }
        }

        private static XmlWriter CreateXmlWriter(StringBuilder sb)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            settings.CloseOutput = false;
            settings.CheckCharacters = true;

            return XmlWriter.Create(sb, settings);
        }

        private static string Format(string value, string format)
        {
            if (!String.IsNullOrEmpty(format))
            {
                return String.Format(format, value);
            }
            return value;
        }
        #endregion
    }
}
