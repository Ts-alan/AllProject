using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Common.Xml;
using VirusBlokAda.CC.Common;
using System.Xml.Serialization;
using System.IO;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;

namespace ARM2_dbcontrol.Tasks
{
    /// <summary>
    /// Правило проактивной защиты
    /// </summary>
    [Serializable]
    public class TaskConfigureProactive: IConfigureTask
    {
        #region Fields

        private String _TaskType = "ProactiveProtection";

        private ProactiveRule _GeneralRule;
        private List<ProactiveRule> _UserRules;
        private JournalEvent _journalEvent;
        
        private String _Vba32CCUser;
        private XmlSerializer serializer;
        
        #endregion

        #region Properties

        public List<ProactiveRule> UserRules
        {
            get { return _UserRules; }
            set { _UserRules = value; }
        }

        public ProactiveRule GeneralRule
        {
            get { return _GeneralRule; }
            set { _GeneralRule = value; }
        }

        public JournalEvent journalEvent
        {
            get { return _journalEvent; }
            set { _journalEvent = value; }
        }

        public String TaskType
        {
            get { return _TaskType; }
            set { _TaskType = value; }
        }

        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }

        #endregion

        #region Constructors

        public TaskConfigureProactive()
        { }

        public TaskConfigureProactive(String[] eventNames)
        {
            _GeneralRule = new ProactiveRule("");
            _UserRules = new List<ProactiveRule>();            
            _journalEvent = new JournalEvent(eventNames);

            _UserRules.Add(GetDefaultUserRule());

            serializer = new XmlSerializer(this.GetType());
        }

        private ProactiveRule GetDefaultUserRule()
        {
            ProactiveRule defaultRule = new ProactiveRule("<Default>");
            defaultRule.TrustedApplications.Add(@"C:\Windows\explorer.exe");
            defaultRule.TrustedApplications.Add(@"C:\Windows\system32\userinit.exe");
            defaultRule.TrustedApplications.Add(@"C:\Windows\system32\winlogon.exe");

            defaultRule.ProtectedFolders.Add(@"C:");
            return defaultRule;
        }

        #endregion

        #region Methods

        public void Clear()
        {
            _GeneralRule.Clear();
            _UserRules.Clear();
            _UserRules.Add(GetDefaultUserRule());
            _journalEvent.ClearEvents();
        }

        #region IConfigureTask Members

        public String SaveToXml()
        {
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, this);
            return sw.ToString();
        }

        public String GetTask()
        {
            StringBuilder result = new StringBuilder(1024);

            result.Append("<VsisCommand><Args><command><arg><key>module-id</key><value>{7C62F84A-A362-4CAA-800C-DEA89110596C}</value></arg><arg><key>command</key><value>apply_settings</value></arg><arg><key>settings</key><value>");            
            result.Append("<config><id>Normal</id><module><id>{7B7D499C-541E-4971-BFD5-286A78CAE649}</id>");
            result.Append(journalEvent.GetTask());

            Int32 id = 0;
            #region General rule

            result.Append("<param><id>TrustedApplications</id><type>stringlist</type>");
            if (GeneralRule.TrustedApplications.Count == 0)
                result.Append("<value/>");
            else
            {
                result.Append("<value>");
                foreach (String str in GeneralRule.TrustedApplications)
                {
                    result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                }
                result.Append("</value>");
            }
            result.Append("</param>");

            result.Append("<param><id>ProtectedApplications</id><type>stringlist</type>");
            if (GeneralRule.ProtectedApplications.Count == 0)
                result.Append("<value/>");
            else
            {
                id = 0;
                result.Append("<value>");
                foreach (String str in GeneralRule.ProtectedApplications)
                {
                    result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                }
                result.Append("</value>");
            }
            result.Append("</param>");

            result.Append("<param><id>ProtectedFiles</id><type>stringlist</type>");
            if (GeneralRule.ProtectedFiles.Count == 0)
                result.Append("<value/>");
            else
            {
                id = 0;
                result.Append("<value>");
                foreach (String str in GeneralRule.ProtectedFiles)
                {
                    result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                }
                result.Append("</value>");
            }
            result.Append("</param>");

            result.Append("<param><id>ReadOnlyFiles</id><type>stringlist</type>");
            if (GeneralRule.ReadOnlyFiles.Count == 0)
                result.Append("<value/>");
            else
            {
                id = 0;
                result.Append("<value>");
                foreach (String str in GeneralRule.ReadOnlyFiles)
                {
                    result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                }
                result.Append("</value>");
            }
            result.Append("</param>");

            result.Append("<param><id>ExcludedFiles</id><type>stringlist</type>");
            if (GeneralRule.ExcludedFiles.Count == 0)
                result.Append("<value/>");
            else
            {
                id = 0;
                result.Append("<value>");
                foreach (String str in GeneralRule.ExcludedFiles)
                {
                    result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                }
                result.Append("</value>");
            }
            result.Append("</param>");

            result.Append("<param><id>ProtectedFolders</id><type>stringlist</type>");
            if (GeneralRule.ProtectedFolders.Count == 0)
                result.Append("<value/>");
            else
            {
                id = 0;
                result.Append("<value>");
                foreach (String str in GeneralRule.ProtectedFolders)
                {
                    result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                }
                result.Append("</value>");
            }
            result.Append("</param>");

            result.Append("<param><id>ReadOnlyFolders</id><type>stringlist</type>");
            if (GeneralRule.ReadOnlyFolders.Count == 0)
                result.Append("<value/>");
            else
            {
                id = 0;
                result.Append("<value>");
                foreach (String str in GeneralRule.ReadOnlyFolders)
                {
                    result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                }
                result.Append("</value>");
            }
            result.Append("</param>");

            result.Append("<param><id>ExcludedFolders</id><type>stringlist</type>");
            if (GeneralRule.ExcludedFolders.Count == 0)
                result.Append("<value/>");
            else
            {
                id = 0;
                result.Append("<value>");
                foreach (String str in GeneralRule.ExcludedFolders)
                {
                    result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                }
                result.Append("</value>");
            }
            result.Append("</param>");

            result.Append("<param><id>ProtectedRegistryKeys</id><type>stringlist</type>");
            if (GeneralRule.ProtectedRegistryKeys.Count == 0)
                result.Append("<value/>");
            else
            {
                id = 0;
                result.Append("<value>");
                foreach (String str in GeneralRule.ProtectedRegistryKeys)
                {
                    result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                }
                result.Append("</value>");
            }
            result.Append("</param>");

            result.Append("<param><id>ReadOnlyRegistryKeys</id><type>stringlist</type>");
            if (GeneralRule.ReadOnlyRegistryKeys.Count == 0)
                result.Append("<value/>");
            else
            {
                id = 0;
                result.Append("<value>");
                foreach (String str in GeneralRule.ReadOnlyRegistryKeys)
                {
                    result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                }
                result.Append("</value>");
            }
            result.Append("</param>");

            result.Append("<param><id>ProtectedRegistryValues</id><type>stringlist</type>");
            if (GeneralRule.ProtectedRegistryValues.Count == 0)
                result.Append("<value/>");
            else
            {
                id = 0;
                result.Append("<value>");
                foreach (String str in GeneralRule.ProtectedRegistryValues)
                {
                    result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                }
                result.Append("</value>");
            }
            result.Append("</param>");

            result.Append("<param><id>ReadOnlyRegistryValues</id><type>stringlist</type>");
            if (GeneralRule.ReadOnlyRegistryValues.Count == 0)
                result.Append("<value/>");
            else
            {
                id = 0;
                result.Append("<value>");
                foreach (String str in GeneralRule.ReadOnlyRegistryValues)
                {
                    result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                }
                result.Append("</value>");
            }
            result.Append("</param>");
            #endregion

            List<String> users = new List<String>();

            #region User rules

            foreach (ProactiveRule rule in UserRules)
            {
                users.Add(System.Web.HttpUtility.HtmlEncode(rule.RuleName));

                result.AppendFormat("<param><id>{0}|TrustedApplications</id><type>stringlist</type>", System.Web.HttpUtility.HtmlEncode(rule.RuleName));
                if (rule.TrustedApplications.Count == 0)
                    result.Append("<value/>");
                else
                {
                    id = 0;
                    result.Append("<value>");
                    foreach (String str in rule.TrustedApplications)
                    {
                        result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                    }
                    result.Append("</value>");
                }
                result.Append("</param>");

                result.AppendFormat("<param><id>{0}|ProtectedFiles</id><type>stringlist</type>", System.Web.HttpUtility.HtmlEncode(rule.RuleName));
                if (rule.ProtectedFiles.Count == 0)
                    result.Append("<value/>");
                else
                {
                    id = 0;
                    result.Append("<value>");
                    foreach (String str in rule.ProtectedFiles)
                    {
                        result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                    }
                    result.Append("</value>");
                }
                result.Append("</param>");

                result.AppendFormat("<param><id>{0}|ReadOnlyFiles</id><type>stringlist</type>", System.Web.HttpUtility.HtmlEncode(rule.RuleName));
                if (rule.ReadOnlyFiles.Count == 0)
                    result.Append("<value/>");
                else
                {
                    id = 0;
                    result.Append("<value>");
                    foreach (String str in rule.ReadOnlyFiles)
                    {
                        result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                    }
                    result.Append("</value>");
                }
                result.Append("</param>");

                result.AppendFormat("<param><id>{0}|ProtectedFolders</id><type>stringlist</type>", System.Web.HttpUtility.HtmlEncode(rule.RuleName));
                if (rule.ProtectedFolders.Count == 0)
                    result.Append("<value/>");
                else
                {
                    id = 0;
                    result.Append("<value>");
                    foreach (String str in rule.ProtectedFolders)
                    {
                        result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                    }
                    result.Append("</value>");
                }
                result.Append("</param>");

                result.AppendFormat("<param><id>{0}|ReadOnlyFolders</id><type>stringlist</type>", System.Web.HttpUtility.HtmlEncode(rule.RuleName));
                if (rule.ReadOnlyFolders.Count == 0)
                    result.Append("<value/>");
                else
                {
                    id = 0;
                    result.Append("<value>");
                    foreach (String str in rule.ReadOnlyFolders)
                    {
                        result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                    }
                    result.Append("</value>");
                }
                result.Append("</param>");

                result.AppendFormat("<param><id>{0}|ProtectedRegistryKeys</id><type>stringlist</type>", System.Web.HttpUtility.HtmlEncode(rule.RuleName));
                if (rule.ProtectedRegistryKeys.Count == 0)
                    result.Append("<value/>");
                else
                {
                    id = 0;
                    result.Append("<value>");
                    foreach (String str in rule.ProtectedRegistryKeys)
                    {
                        result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                    }
                    result.Append("</value>");
                }
                result.Append("</param>");

                result.AppendFormat("<param><id>{0}|ReadOnlyRegistryKeys</id><type>stringlist</type>", System.Web.HttpUtility.HtmlEncode(rule.RuleName));
                if (rule.ReadOnlyRegistryKeys.Count == 0)
                    result.Append("<value/>");
                else
                {
                    id = 0;
                    result.Append("<value>");
                    foreach (String str in rule.ReadOnlyRegistryKeys)
                    {
                        result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, str);
                    }
                    result.Append("</value>");
                }
                result.Append("</param>");
            }

            #endregion

            #region Users
            id = 0;
            result.Append("<param><id>Users</id><type>stringlist</type><value>");
            foreach (String user in users)
            {
                result.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", id++, user);
            }
            result.Append("</value></param>");
            
            #endregion
        
            result.Append("</module></config>");
            result.Append("</value></arg></command></Args><Async>0</Async></VsisCommand>");

            return result.ToString();
        }

        public void LoadFromXml(String xml)
        {
            if (String.IsNullOrEmpty(xml))
                return;

            TaskConfigureProactive task;
            using (TextReader reader = new StringReader(xml))
            {
                task = (TaskConfigureProactive)serializer.Deserialize(reader);
            }
            this._GeneralRule = task.GeneralRule;
            this._UserRules = task.UserRules;
            this._journalEvent = task.journalEvent;
        }

        public void LoadFromRegistry(string reg)
        {
            throw new NotImplementedException();
        }

        #endregion
        #endregion
    }
}
