using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Tasks.ProactiveProtection
{
    /// <summary>
    /// ������� ����������� ������
    /// </summary>
    public class RuleProactiveProtection
    {
        private RuleTypeEnum _typeRule;
        public RuleTypeEnum TypeRule
        {
            get { return _typeRule; }
            set { _typeRule = value; }
        }

        public List<string> Applications;
        public List<RegistryRule> RegistryRules;
        public List<FileSystemRule> FileSystemRules;

        #region Constructors
        public RuleProactiveProtection()
        {
            Applications = new List<string>();
            RegistryRules = new List<RegistryRule>();
            FileSystemRules = new List<FileSystemRule>();
        }

        public RuleProactiveProtection(RuleTypeEnum rule)
            : this()
        {
            _typeRule = rule;
        }
        #endregion
    }

    /// <summary>
    /// ������� ��� �������
    /// </summary>
    public struct RegistryRule
    {
        public string eventARM;
        public string Path;
        public string TypeNote;
        public bool Subkeys;
        public bool Log;
    }
    /// <summary>
    /// ������� ��� �������� �������
    /// </summary>
    public struct FileSystemRule
    {
        public string eventARM;
        public string Path;
        public bool AlloRead;
        public bool Subdirs;
        public bool Log;
    }
}
