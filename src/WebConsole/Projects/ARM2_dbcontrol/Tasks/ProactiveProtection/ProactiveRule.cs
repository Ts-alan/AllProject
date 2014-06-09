using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Tasks
{
    [Serializable]
    public class ProactiveRule
    {
        #region Fields

        private String _RuleName;

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

        public String RuleName
        {
            get { return _RuleName; }
            set { _RuleName = value; }
        }

        #endregion

        #region Constructors

        public ProactiveRule()
        { }

        public ProactiveRule(String ruleName)
        {
            _RuleName = ruleName;

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

        public ProactiveRule Clone(String newName)
        {
            ProactiveRule clone = new ProactiveRule(newName);
            clone.TrustedApplications = new List<String>(this.TrustedApplications);
            clone.ReadOnlyFiles = new List<String>(this.ReadOnlyFiles);
            clone.ReadOnlyFolders = new List<String>(this.ReadOnlyFolders);
            clone.ReadOnlyRegistryKeys = new List<String>(this.ReadOnlyRegistryKeys);
            clone.ProtectedFiles = new List<String>(this.ProtectedFiles);
            clone.ProtectedFolders = new List<String>(this.ProtectedFolders);
            clone.ProtectedRegistryKeys = new List<String>(this.ProtectedRegistryKeys);
            return clone;
        }

        public void Clear()
        {
            this.TrustedApplications.Clear();
            
            this.ReadOnlyFiles.Clear();
            this.ReadOnlyFolders.Clear();
            this.ReadOnlyRegistryKeys.Clear();
            this.ReadOnlyRegistryValues.Clear();
            
            this.ProtectedApplications.Clear();
            this.ProtectedFiles.Clear();
            this.ProtectedFolders.Clear();
            this.ProtectedRegistryKeys.Clear();
            this.ProtectedRegistryValues.Clear();

            this.ExcludedFiles.Clear();
            this.ExcludedFolders.Clear();
        }

        #endregion
    }
}
