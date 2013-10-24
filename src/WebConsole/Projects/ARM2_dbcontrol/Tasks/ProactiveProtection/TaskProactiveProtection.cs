using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Tasks.ProactiveProtection
{
    /// <summary>
    /// Правило проактивной защиты
    /// </summary>
    public class TaskConfigureProactive
    {
        #region Fields

        public const String TaskType = "ProactiveProtection";

        private List<String> _TrustedApplications;
        private List<String> _ProtectedApplications;
        private List<String> _ProtectedFiles;
        private List<String> _ProtectedFolders;
        private List<String> _ReadOnlyFiles;
        private List<String> _ReadOnlyFolders;
        private List<String> _ExcludedFiles;
        private List<String> _ExcludedFolders;
        private List<String> _ProtectedRegistryKeys;
        private List<String> _ProtectedRegistryValues;
        private List<String> _ReadOnlyRegistryKeys;
        private List<String> _ReadOnlyRegistryValues;

        private String _Vba32CCUser;
        
        #endregion

        #region Properties

        public List<String> ReadOnlyRegistryValues
        {
            get { return _ReadOnlyRegistryValues; }
            set { _ReadOnlyRegistryValues = value; }
        }

        public List<String> ReadOnlyRegistryKeys
        {
            get { return _ReadOnlyRegistryKeys; }
            set { _ReadOnlyRegistryKeys = value; }
        }

        public List<String> ProtectedRegistryValues
        {
            get { return _ProtectedRegistryValues; }
            set { _ProtectedRegistryValues = value; }
        }

        public List<String> ProtectedRegistryKeys
        {
            get { return _ProtectedRegistryKeys; }
            set { _ProtectedRegistryKeys = value; }
        }

        public List<String> ExcludedFolders
        {
            get { return _ExcludedFolders; }
            set { _ExcludedFolders = value; }
        }

        public List<String> ExcludedFiles
        {
            get { return _ExcludedFiles; }
            set { _ExcludedFiles = value; }
        }

        public List<String> ReadOnlyFolders
        {
            get { return _ReadOnlyFolders; }
            set { _ReadOnlyFolders = value; }
        }

        public List<String> ReadOnlyFiles
        {
            get { return _ReadOnlyFiles; }
            set { _ReadOnlyFiles = value; }
        }

        public List<String> ProtectedFolders
        {
            get { return _ProtectedFolders; }
            set { _ProtectedFolders = value; }
        }

        public List<String> ProtectedFiles
        {
            get { return _ProtectedFiles; }
            set { _ProtectedFiles = value; }
        }

        public List<String> ProtectedApplications
        {
            get { return _ProtectedApplications; }
            set { _ProtectedApplications = value; }
        }

        public List<String> TrustedApplications
        {
            get { return _TrustedApplications; }
            set { _TrustedApplications = value; }
        }
        
        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }

        #endregion

        #region Constructors

        public TaskConfigureProactive()
        {
            _TrustedApplications = new List<String>();
            _ProtectedApplications = new List<String>();

            _ReadOnlyFiles = new List<String>();
            _ProtectedFiles = new List<String>();
            _ExcludedFiles = new List<String>();

            _ReadOnlyFolders = new List<String>();
            _ProtectedFolders = new List<String>();
            _ExcludedFolders = new List<String>();

            _ReadOnlyRegistryKeys = new List<String>();
            _ProtectedRegistryKeys = new List<String>();

            _ReadOnlyRegistryValues = new List<String>();
            _ProtectedRegistryValues = new List<String>();
        }

        #endregion

        #region Methods

        public String SaveToXml()
        {
            StringBuilder sb = new StringBuilder(512);
            sb.Append(@"<proactive>");
            sb.Append(GetXmlContent());
            sb.AppendFormat("<{0}>{1}</{0}>", "Vba32CCUser", Vba32CCUser);
            sb.AppendFormat("<{0}>{1}</{0}>", "Type", TaskType);
            sb.Append(@"</proactive>");
            return sb.ToString();
        }

        public String BuildTaskXml()
        {
            StringBuilder result = new StringBuilder(256);

            result.Append("<SetRegistrySettings>");
            result.AppendFormat(@"<Common><RegisterPath>{0}</RegisterPath><IsDeleteOld>0</IsDeleteOld></Common>",
                        @"HKLM\SOFTWARE\Vba32\Loader\Proactive");
            result.Append(@"<Settings>");
            result.Append(GetXmlContent());
            result.Append(@"</Settings></SetRegistrySettings>");

            return result.ToString();
        }

        private String GetXmlContent()
        {
            StringBuilder result = new StringBuilder(512);
            StringBuilder sb = new StringBuilder();

            foreach (String str in TrustedApplications)
            {
                sb.AppendFormat("{0}{1}", str, '\0');
            }
            if (sb.Length > 0) sb.Append('\0');
            result.AppendFormat(@"<{0}>reg_sz:{1}</{0}>", "TrustedApplications", TaskHelper.ToBase64String(sb.ToString()));

            sb = new StringBuilder();
            foreach (String str in ProtectedApplications)
            {
                sb.AppendFormat("{0}{1}", str, '\0');
            }
            if (sb.Length > 0) sb.Append('\0');
            result.AppendFormat(@"<{0}>reg_sz:{1}</{0}>", "ProtectedApplications", TaskHelper.ToBase64String(sb.ToString()));

            sb = new StringBuilder();
            foreach (String str in ReadOnlyFiles)
            {
                sb.AppendFormat("{0}{1}", str, '\0');
            }
            if (sb.Length > 0) sb.Append('\0');
            result.AppendFormat(@"<{0}>reg_sz:{1}</{0}>", "ReadOnlyFiles", TaskHelper.ToBase64String(sb.ToString()));

            sb = new StringBuilder();
            foreach (String str in ProtectedFiles)
            {
                sb.AppendFormat("{0}{1}", str, '\0');
            }
            if (sb.Length > 0) sb.Append('\0');
            result.AppendFormat(@"<{0}>reg_sz:{1}</{0}>", "ProtectedFiles", TaskHelper.ToBase64String(sb.ToString()));

            sb = new StringBuilder();
            foreach (String str in ExcludedFiles)
            {
                sb.AppendFormat("{0}{1}", str, '\0');
            }
            if (sb.Length > 0) sb.Append('\0');
            result.AppendFormat(@"<{0}>reg_sz:{1}</{0}>", "ExcludedFiles", TaskHelper.ToBase64String(sb.ToString()));

            sb = new StringBuilder();
            foreach (String str in ReadOnlyFolders)
            {
                sb.AppendFormat("{0}{1}", str, '\0');
            }
            if (sb.Length > 0) sb.Append('\0');
            result.AppendFormat(@"<{0}>reg_sz:{1}</{0}>", "ReadOnlyFolders", TaskHelper.ToBase64String(sb.ToString()));

            sb = new StringBuilder();
            foreach (String str in ProtectedFolders)
            {
                sb.AppendFormat("{0}{1}", str, '\0');
            }
            if (sb.Length > 0) sb.Append('\0');
            result.AppendFormat(@"<{0}>reg_sz:{1}</{0}>", "ProtectedFolders", TaskHelper.ToBase64String(sb.ToString()));

            sb = new StringBuilder();
            foreach (String str in ExcludedFolders)
            {
                sb.AppendFormat("{0}{1}", str, '\0');
            }
            if (sb.Length > 0) sb.Append('\0');
            result.AppendFormat(@"<{0}>reg_sz:{1}</{0}>", "ExcludedFolders", TaskHelper.ToBase64String(sb.ToString()));

            sb = new StringBuilder();
            foreach (String str in ReadOnlyRegistryKeys)
            {
                sb.AppendFormat("{0}{1}", str, '\0');
            }
            if (sb.Length > 0) sb.Append('\0');
            result.AppendFormat(@"<{0}>reg_sz:{1}</{0}>", "ReadOnlyRegistryKeys", TaskHelper.ToBase64String(sb.ToString()));

            sb = new StringBuilder();
            foreach (String str in ProtectedRegistryKeys)
            {
                sb.AppendFormat("{0}{1}", str, '\0');
            }
            if (sb.Length > 0) sb.Append('\0');
            result.AppendFormat(@"<{0}>reg_sz:{1}</{0}>", "ProtectedRegistryKeys", TaskHelper.ToBase64String(sb.ToString()));

            sb = new StringBuilder();
            foreach (String str in ReadOnlyRegistryValues)
            {
                sb.AppendFormat("{0}{1}", str, '\0');
            }
            if (sb.Length > 0) sb.Append('\0');
            result.AppendFormat(@"<{0}>reg_sz:{1}</{0}>", "ReadOnlyRegistryValues", TaskHelper.ToBase64String(sb.ToString()));

            sb = new StringBuilder();
            foreach (String str in ProtectedRegistryValues)
            {
                sb.AppendFormat("{0}{1}", str, '\0');
            }
            if (sb.Length > 0) sb.Append('\0');
            result.AppendFormat(@"<{0}>reg_sz:{1}</{0}>", "ProtectedRegistryValues", TaskHelper.ToBase64String(sb.ToString()));

            return result.ToString();
        }

        public void LoadFromXml(String xml)
        {
            XmlTaskParser pars = new XmlTaskParser(xml);

            if (pars.GetValue("TrustedApplications", "reg_sz:") != String.Empty)
            {
                foreach (String str in TaskHelper.FromBase64String(pars.GetValue("TrustedApplications", "reg_sz:")).Split(new Char[] { '\0' }))
                {
                    if (!String.IsNullOrEmpty(str))
                        _TrustedApplications.Add(str);
                }
            }

            if (pars.GetValue("ProtectedApplications", "reg_sz:") != String.Empty)
            {
                foreach (String str in TaskHelper.FromBase64String(pars.GetValue("ProtectedApplications", "reg_sz:")).Split(new Char[] { '\0' }))
                {
                    if (!String.IsNullOrEmpty(str))
                        _ProtectedApplications.Add(str);
                }
            }

            if (pars.GetValue("ReadOnlyFiles", "reg_sz:") != String.Empty)
            {
                foreach (String str in TaskHelper.FromBase64String(pars.GetValue("ReadOnlyFiles", "reg_sz:")).Split(new Char[] { '\0' }))
                {
                    if (!String.IsNullOrEmpty(str))
                        _ReadOnlyFiles.Add(str);
                }
            }

            if (pars.GetValue("ProtectedFiles", "reg_sz:") != String.Empty)
            {
                foreach (String str in TaskHelper.FromBase64String(pars.GetValue("ProtectedFiles", "reg_sz:")).Split(new Char[] { '\0' }))
                {
                    if (!String.IsNullOrEmpty(str))
                        _ProtectedFiles.Add(str);
                }
            }

            if (pars.GetValue("ExcludedFiles", "reg_sz:") != String.Empty)
            {
                foreach (String str in TaskHelper.FromBase64String(pars.GetValue("ExcludedFiles", "reg_sz:")).Split(new Char[] { '\0' }))
                {
                    if (!String.IsNullOrEmpty(str))
                        _ExcludedFiles.Add(str);
                }
            }

            if (pars.GetValue("ReadOnlyFolders", "reg_sz:") != String.Empty)
            {
                foreach (String str in TaskHelper.FromBase64String(pars.GetValue("ReadOnlyFolders", "reg_sz:")).Split(new Char[] { '\0' }))
                {
                    if (!String.IsNullOrEmpty(str))
                        _ReadOnlyFolders.Add(str);
                }
            }

            if (pars.GetValue("ProtectedFolders", "reg_sz:") != String.Empty)
            {
                foreach (String str in TaskHelper.FromBase64String(pars.GetValue("ProtectedFolders", "reg_sz:")).Split(new Char[] { '\0' }))
                {
                    if (!String.IsNullOrEmpty(str))
                        _ProtectedFolders.Add(str);
                }
            }

            if (pars.GetValue("ExcludedFolders", "reg_sz:") != String.Empty)
            {
                foreach (String str in TaskHelper.FromBase64String(pars.GetValue("ExcludedFolders", "reg_sz:")).Split(new Char[] { '\0' }))
                {
                    if (!String.IsNullOrEmpty(str))
                        _ExcludedFolders.Add(str);
                }
            }

            if (pars.GetValue("ReadOnlyRegistryKeys", "reg_sz:") != String.Empty)
            {
                foreach (String str in TaskHelper.FromBase64String(pars.GetValue("ReadOnlyRegistryKeys", "reg_sz:")).Split(new Char[] { '\0' }))
                {
                    if (!String.IsNullOrEmpty(str))
                        _ReadOnlyRegistryKeys.Add(str);
                }
            }

            if (pars.GetValue("ProtectedRegistryKeys", "reg_sz:") != String.Empty)
            {
                foreach (String str in TaskHelper.FromBase64String(pars.GetValue("ProtectedRegistryKeys", "reg_sz:")).Split(new Char[] { '\0' }))
                {
                    if (!String.IsNullOrEmpty(str))
                        _ProtectedRegistryKeys.Add(str);
                }
            }

            if (pars.GetValue("ReadOnlyRegistryValues", "reg_sz:") != String.Empty)
            {
                foreach (String str in TaskHelper.FromBase64String(pars.GetValue("ReadOnlyRegistryValues", "reg_sz:")).Split(new Char[] { '\0' }))
                {
                    if (!String.IsNullOrEmpty(str))
                        _ReadOnlyRegistryValues.Add(str);
                }
            }

            if (pars.GetValue("ProtectedRegistryValues", "reg_sz:") != String.Empty)
            {
                foreach (String str in TaskHelper.FromBase64String(pars.GetValue("ProtectedRegistryValues", "reg_sz:")).Split(new Char[] { '\0' }))
                {
                    if (!String.IsNullOrEmpty(str))
                        _ProtectedRegistryValues.Add(str);
                }
            }
        }

        #endregion
        
    }
}
