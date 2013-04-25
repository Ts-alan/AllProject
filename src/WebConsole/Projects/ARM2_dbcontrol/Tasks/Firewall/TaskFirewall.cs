using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Tasks.Firewall
{
    /// <summary>
    /// Правило настройки файрвола
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
    /// Правило для файрвола
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
    /// Дружественный IP-адрес
    /// </summary>
    public struct FriendlyIPRule
    {
        public string ip;
        public string comment;
    }

    /// <summary>
    /// Перечисление протоколов
    /// </summary>
    public enum Protocols
    { 
        Any = 0,
        TCP,
        UDP        
    }
    /// <summary>
    /// Перечисление направлений
    /// </summary>
    public enum Directions
    {
        Any = 0,
        In,
        Out        
    }
}
