using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;

namespace ARM2_dbcontrol.Tasks.ConfigureFirewall
{
    [Serializable]
    public class TaskConfigureFirewall : IConfigureTask
    {
        #region Fields

        private String _Type = "Firewall";
        private List<FirewallRule> _IP4Rules;
        private List<FirewallRule> _IP6Rules;
        private Boolean _firewall_On;
        private FirewallNetworkType _networkType;
        //private static List<FirewallRuleProtocol> RuleProtocols;
        private JournalEvent _journalEvent;

        public const String GUID = "{AAEBBB59-4CD4-478B-A7C6-15F4DAF74078}";
        public static readonly String[] ProtocolsName = {"pnRESERVED","pnICMP","pnIGMP","pnGGP","pnIP","pnST","pnTCP","pnUCL","pnEGP","pnIGP","pnBBN_RCC_MON","pnNVP_II","pnPUP","pnARGUS","pnEMCON","pnXNET","pnCHAOS","pnUDP","pnMUX","pnDCN_MEAS","pnHMP","pnPRM","pnXNS_IDP","pnTRUNK_1","pnTRUNK_2","pnLEAF_1","pnLEAF_2","pnRDP","pnIRTP","pnISO_TP4","pnNETBLT","pnMFE_NSP","pnMERIT_INP","pnSEP","pn3PC","pnIDPR",
                                            "pnXTP","pnDDP","pnIDRP_CMTP","pnTP","pnIL","pnSIP","pnSDRP","pnSIP_SR","pnSIP_FRAG","pnIDRP","pnRSVP","pnGRE","pnMHRP","pnBNA","pnSIPP_ESP","pnSIPP_AH","pnI_NLSP","pnSWIPE","pnNHRP","pnUNASSIGNED","pnUNASSIGNED","pnUNASSIGNED","pnUNASSIGNED","pnUNASSIGNED","pnUNASSIGNED","pnANY_HOST","pnCFTP","pnANY_LOCAL","pnSAT_EXPAK","pnKRYPTOLAN","pnRVD","pnIPPC",
                                            "pnANY_DISTRIBUTED","pnSAT_MON","pnVISA","pnIPCV","pnCPNX","pnCPHB","pnWSN","pnPVP","pnBR_SAT_MON","pnSUN_ND","pnWB_MON","pnWB_EXPAK","pnISO_IP","pnVMTP","pnSECURE_VMTP","pnVINES","pnTTP","pnNSFNET_IGP","pnDGP","pnTCF","pnIGRP","pnOSPFIGP","pnSprite_RPC","pnLARP","pnMTP","pnAX_25","pnIPIP","pnMICP","pnSCC_SP","pnETHERIP","pnENCAP","pnANY_PRIVATE","pnGMTP"};

        private String _Vba32CCUser;
        private XmlSerializer serializer;

        #endregion

        #region Properties

        public String Vba32CCUser
        {
            get { return _Vba32CCUser; }
            set { _Vba32CCUser = value; }
        }

        public String Type
        {
            get { return _Type; }
            set { _Type = "Firewall"; }
        }

        public List<FirewallRule> IP4Rules
        {
            get { return _IP4Rules; }
            set { _IP4Rules = value; }
        }

        public List<FirewallRule> IP6Rules
        {
            get { return _IP6Rules; }
            set { _IP6Rules = value; }
        }

        public JournalEvent journalEvent
        {
            get { return _journalEvent; }
            set { _journalEvent = value; }
        }

        public Boolean firewall_On
        {
            get { return _firewall_On; }
            set { _firewall_On = value; }
        }
        public FirewallNetworkType NetworkType
        {
            get { return _networkType; }
            set { _networkType = value; }
        }
        #endregion

        #region Constructor

        public TaskConfigureFirewall(String[] eventNames)
        {
            _IP4Rules = new List<FirewallRule>();
            _IP6Rules = new List<FirewallRule>();
            _journalEvent = new JournalEvent(eventNames);
            serializer = new XmlSerializer(this.GetType());
        }

        public TaskConfigureFirewall()
        {
            _IP4Rules = new List<FirewallRule>();
            _IP6Rules = new List<FirewallRule>();
            _journalEvent = new JournalEvent();
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

            TaskConfigureFirewall task;
            using (TextReader reader = new StringReader(Xml))
            {
                task = (TaskConfigureFirewall)serializer.Deserialize(reader);
            }

            this._networkType = task._networkType;
            this._firewall_On = task._firewall_On;
            this._IP4Rules = task.IP4Rules;
            this._IP6Rules = task.IP6Rules;
            this._Type = task.Type;
            this._Vba32CCUser = task.Vba32CCUser;
            this._journalEvent = task.journalEvent;
        }

        public String GetTask()
        {
            StringBuilder builder = new StringBuilder(512);

            builder.Append(@"<VsisCommand><Args><command><arg><key>module-id</key><value>{7C62F84A-A362-4CAA-800C-DEA89110596C}</value></arg><arg><key>command</key><value>apply_settings</value></arg>");
            builder.AppendFormat(@"<arg><key>settings</key><value><config><id>Normal</id><module><id>{0}</id>", GUID);
            
            builder.AppendFormat(@"<param><id>Enable</id><type>string</type><value>{0}</value></param>", firewall_On ? "On" : "Off");
            builder.AppendFormat(@"<param><id>ActiveRulesSets</id><type>string</type><value>{0}</value></param>", NetworkType.ToString());

            builder.Append(journalEvent.GetTask());
            
            builder.Append("<param><id>IpV4</id><type>stringlist</type><value>");
            for (Int32 i = 0; i < IP4Rules.Count; i++)
            {
                builder.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", i.ToString(), ConvertRuleForTask(IP4Rules[i]));
            }
            builder.Append("</value></param>");
            
            builder.Append("<param><id>IpV6</id><type>stringlist</type><value>");
            for (Int32 i = 0; i < IP6Rules.Count; i++)
            {
                builder.AppendFormat("<string><id>{0}</id><val>{1}</val></string>", i.ToString(), ConvertRuleForTask(IP6Rules[i]));
            }
            builder.Append(@"</value></param></module></config></value></arg></command></Args><Async>0</Async></VsisCommand>");

            return builder.ToString();
        }

        private String ConvertRuleForTask(FirewallRule rule)
        {
            StringBuilder task = new StringBuilder(128);
            task.AppendFormat("{0};", rule.LocalIP);
            task.AppendFormat("{0};", rule.LocalPort);
            task.AppendFormat("{0};", rule.DestinationIP);
            task.AppendFormat("{0};", rule.DestinationPort);

            switch (rule.Protocol)
            {
                case "TCP":
                    task.Append("6;");
                    break;
                case "UDP":
                    task.Append("17;");
                    break;
                default:
                    task.AppendFormat("{0};", rule.Protocol);
                    break;
            }

            UInt32 bitmask = 0x00;
            switch (rule.Rule)
            {
                case RulesEnum.AllowReceiveSend:
                    bitmask += (UInt32)FirewallFlags.AllowSendReceive;
                    break;
                case RulesEnum.AllowSend:
                    bitmask += (UInt32)FirewallFlags.AllowSend;
                    break;
                case RulesEnum.AllowReceive:
                    bitmask += (UInt32)FirewallFlags.AllowReceive;
                    break;
            }

            if (rule.Audit)
                bitmask += (UInt32)FirewallFlags.Audit;

            if (rule.IsTransport)
                bitmask += (UInt32)FirewallFlags.TransportProtocol;

            if (rule.Enable)
                bitmask += (UInt32)FirewallFlags.Enable;

            task.AppendFormat("{0};", bitmask.ToString());
            task.Append(rule.Name);

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
    public enum FirewallNetworkType
    {
        Domain=0,
        Open,
        Private,
        Close
    }
    public struct FirewallRule
    {
        public Boolean Enable;
        public String Name;

        public String LocalIP;
        public String LocalPort;
        public String DestinationIP;
        public String DestinationPort;

        public Boolean Audit;

        public String Protocol;
        public RulesEnum Rule;

        public Boolean IsTransport;
    }

    public enum RulesEnum
    {
        AllowReceive = 0,
        AllowSend,
        AllowReceiveSend,
        DenyAll
    }

    public class FirewallRuleProtocol
    {
        #region Fields

        Int32 _No;
        Boolean _IsChecked;
        String _Name;
        String _FullName;

        #endregion

        #region Properties

        public Int32 No
        {
            get { return _No; }
            set { _No = value; }
        }

        public Boolean IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked = value; }
        }

        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public String FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        #endregion

        #region Constructors

        public FirewallRuleProtocol(Int32 no, Boolean isChecked, String Name, String FullName)
        {
            this._No = no;
            this._IsChecked = isChecked;
            this._Name = Name;
            this._FullName = FullName;
        }

        public FirewallRuleProtocol()
        {
            // TODO: Complete member initialization
        }

        #endregion
    }

    [Flags]
    public enum FirewallFlags
    {
        DenyAll = 0x00,
        AllowSend = 0x01,
        AllowReceive = 0x02,
        AllowSendReceive = AllowSend | AllowReceive,
        Audit = 0x08,
        TransportProtocol = 0x20,
        Enable = 0x40
    }
}