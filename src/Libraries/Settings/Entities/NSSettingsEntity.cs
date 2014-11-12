using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.Settings.Entities
{
    public class NSSettingsEntity
    {
        #region Fields

        private String _MailServer = null;
        private Int32 _MailPort = 0;
        private String _MailFrom = null;
        private String _MailDisplayName = null;
        private Boolean _UseMailAuthorization = false;
        private String _MailUsername = null;
        private String _MailPassword = null;
        private Boolean _MailEnableSsl = false;

        private String _JabberServer = null;
        private String _JabberPassword = null;
        private String _JabberFromJID = null;

        private Int32? _TimeLimit = null;
        private Int32? _UseFlowAnalysis = null;
        private Int32? _Limit = null;

        private Int32? _LocalHearthLimit = null;
        private Int32? _LocalHearthTimeLimit = null;

        private Int32? _GlobalEpidemyCompCount = null;
        private Int32? _GlobalEpidemyLimit = null;
        private Int32? _GlobalEpidemyTimeLimit = null;

        private Boolean _XMPPLibrary = false;
                
        private Boolean _ReRead;
        
        #endregion

        #region Properties

        public String MailServer
        {
            get { return _MailServer; }
            set { _MailServer = value; }
        }

        public Int32 MailPort
        {
            get { return _MailPort; }
            set { _MailPort = value; }
        }

        public String MailFrom
        {
            get { return _MailFrom; }
            set { _MailFrom = value; }
        }

        public String MailDisplayName
        {
            get { return _MailDisplayName; }
            set { _MailDisplayName = value; }
        }

        public Boolean UseMailAuthorization
        {
            get { return _UseMailAuthorization; }
            set { _UseMailAuthorization = value; }
        }

        public String MailUsername
        {
            get { return _MailUsername; }
            set { _MailUsername = value; }
        }

        public String MailPassword
        {
            get { return _MailPassword; }
            set { _MailPassword = value; }
        }

        public Boolean MailEnableSsl
        {
            get { return _MailEnableSsl; }
            set { _MailEnableSsl = value; }
        }

        public String JabberServer
        {
            get { return _JabberServer; }
            set { _JabberServer = value; }
        }

        public String JabberPassword
        {
            get { return _JabberPassword; }
            set { _JabberPassword = value; }
        }

        public String JabberFromJID
        {
            get { return _JabberFromJID; }
            set { _JabberFromJID = value; }
        }

        public Int32? TimeLimit
        {
            get { return _TimeLimit; }
            set { _TimeLimit = value; }
        }

        public Int32? UseFlowAnalysis
        {
            get { return _UseFlowAnalysis; }
            set { _UseFlowAnalysis = value; }
        }

        public Int32? Limit
        {
            get { return _Limit; }
            set { _Limit = value; }
        }

        public Int32? LocalHearthLimit
        {
            get { return _LocalHearthLimit; }
            set { _LocalHearthLimit = value; }
        }

        public Int32? LocalHearthTimeLimit
        {
            get { return _LocalHearthTimeLimit; }
            set { _LocalHearthTimeLimit = value; }
        }

        public Int32? GlobalEpidemyCompCount
        {
            get { return _GlobalEpidemyCompCount; }
            set { _GlobalEpidemyCompCount = value; }
        }

        public Int32? GlobalEpidemyLimit
        {
            get { return _GlobalEpidemyLimit; }
            set { _GlobalEpidemyLimit = value; }
        }

        public Int32? GlobalEpidemyTimeLimit
        {
            get { return _GlobalEpidemyTimeLimit; }
            set { _GlobalEpidemyTimeLimit = value; }
        }

        public Boolean XMPPLibrary
        {
            get { return _XMPPLibrary; }
            set { _XMPPLibrary = value; }
        }

        public Boolean ReRead
        {
            get { return _ReRead; }
            set { _ReRead = value; }
        }

        #endregion
        
        #region Constructors

        public NSSettingsEntity() { }

        #endregion

        #region Methods

        public String GenerateXML()
        {
            StringBuilder xml = new StringBuilder(1024);
            xml.Append("<VbaSettings><ControlCenter><Notification>");

            if(!String.IsNullOrEmpty(MailServer))
                xml.AppendFormat("<MailServer type=" + "\"reg_sz\"" + ">{0}</MailServer>", MailServer);
            if (!String.IsNullOrEmpty(MailFrom))
                xml.AppendFormat("<MailFrom type=" + "\"reg_sz\"" + ">{0}</MailFrom>", MailFrom);
            if (!String.IsNullOrEmpty(MailDisplayName))
                xml.AppendFormat("<MailDisplayName type=" + "\"reg_sz\"" + ">{0}</MailDisplayName>", MailDisplayName);

            xml.AppendFormat("<MailPort type=" + "\"reg_dword\"" + ">{0}</MailPort>", MailPort);
            xml.AppendFormat("<MailUsername type=" + "\"reg_sz\"" + ">{0}</MailUsername>", UseMailAuthorization ? MailUsername : "");
            xml.AppendFormat("<MailPassword type=" + "\"reg_sz\"" + ">{0}</MailPassword>", UseMailAuthorization ? MailPassword : "");
            xml.AppendFormat("<MailEnableSsl type=" + "\"reg_dword\"" + ">{0}</MailEnableSsl>", MailEnableSsl ? 1 : 0);

            if (!String.IsNullOrEmpty(JabberServer))
                xml.AppendFormat("<JabberServer type=" + "\"reg_sz\"" + ">{0}</JabberServer>", JabberServer);
            if (!String.IsNullOrEmpty(JabberFromJID))
                xml.AppendFormat("<JabberFromJID type=" + "\"reg_sz\"" + ">{0}</JabberFromJID>", JabberFromJID);
            if (!String.IsNullOrEmpty(JabberPassword))
                xml.AppendFormat("<JabberPassword type=" + "\"reg_sz\"" + ">{0}</JabberPassword>", JabberPassword);

            if (LocalHearthTimeLimit.HasValue)
                xml.AppendFormat("<LocalHearthTimeLimit type=" + "\"reg_dword\"" + ">{0}</LocalHearthTimeLimit>", LocalHearthTimeLimit);
            if (LocalHearthLimit.HasValue)
                xml.AppendFormat("<LocalHearthLimit type=" + "\"reg_dword\"" + ">{0}</LocalHearthLimit>", LocalHearthLimit);
            if (GlobalEpidemyTimeLimit.HasValue)
                xml.AppendFormat("<GlobalEpidemyTimeLimit type=" + "\"reg_dword\"" + ">{0}</GlobalEpidemyTimeLimit>", GlobalEpidemyTimeLimit);
            if (GlobalEpidemyLimit.HasValue)
                xml.AppendFormat("<GlobalEpidemyLimit type=" + "\"reg_dword\"" + ">{0}</GlobalEpidemyLimit>", GlobalEpidemyLimit);
            if (Limit.HasValue)
                xml.AppendFormat("<Limit type=" + "\"reg_dword\"" + ">{0}</Limit>", Limit);
            if (TimeLimit.HasValue)
                xml.AppendFormat("<TimeLimit type=" + "\"reg_dword\"" + ">{0}</TimeLimit>", TimeLimit);
            if (UseFlowAnalysis.HasValue)
                xml.AppendFormat("<UseFlowAnalysis type=" + "\"reg_dword\"" + ">{0}</UseFlowAnalysis>", UseFlowAnalysis);
            if (GlobalEpidemyCompCount.HasValue)
                xml.AppendFormat("<GlobalEpidemyCompCount type=" + "\"reg_dword\"" + ">{0}</GlobalEpidemyCompCount>", GlobalEpidemyCompCount);
            
            xml.AppendFormat("<ReRead type=" + "\"reg_dword\"" + ">{0}</ReRead>", ReRead ? "1" : "0");

            xml.Append("</Notification></ControlCenter></VbaSettings>");

            return xml.ToString();
        }

        #endregion
    }
}
