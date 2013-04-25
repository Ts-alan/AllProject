using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Tasks.Firewall
{
    /// <summary>
    /// ������� ��������� ��������
    /// </summary>
    public class RuleConfigureFirewall
    {
        private bool _modeInvisibility;
        public bool ModeInvisibility
        {
            get { return _modeInvisibility; }
            set { _modeInvisibility = value; }
        }

        private bool _antivirusMonitoring;
        public bool AntivirusMonitoring
        {
            get { return _antivirusMonitoring; }
            set { _antivirusMonitoring = value; }
        }

        public List<FirewallRule> Rules;
        public List<FriendlyIPRule> FriendlyIP;

        
        public RuleConfigureFirewall()
        {
            _modeInvisibility = false;
            _antivirusMonitoring = false;
            Rules = new List<FirewallRule>();
            FriendlyIP = new List<FriendlyIPRule>();
        }        
    }

    /// <summary>
    /// ������� ��� ��������
    /// </summary>
    public struct FirewallRule
    {
        public bool isActivate;
        public bool isAllow;
        public string application;
        public string comment;

        public bool isSubnetwork;
        public string ip;        
        public string subnetwork_mask;

        public Protocols protocol;
        public Directions direction;
        public string localPorts;
        public string remotePorts;

        public string eventARM;        
    }

    /// <summary>
    /// ������������� IP-�����
    /// </summary>
    public struct FriendlyIPRule
    {
        public string ip;
        public string comment;
    }

    /// <summary>
    /// ������������ ����������
    /// </summary>
    public enum Protocols
    { 
        Any = 0,
        TCP,
        UDP        
    }
    /// <summary>
    /// ������������ �����������
    /// </summary>
    public enum Directions
    {
        Any = 0,
        In,
        Out        
    }
}
