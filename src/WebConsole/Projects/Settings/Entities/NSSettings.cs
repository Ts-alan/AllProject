using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Entities
{
    public class NSSettings
    {
        #region Fields

        private String _MailServer;
        private String _MailFrom;
        private String _MailDisplayName;

        private String _JabberServer;
        private String _JabberPassword;
        private String _JabberFromJID;

        private Int32? _TimeLimit;
        private Int32? _UseFlowAnalysis;
        private Int32? _Limit;

        private Int32? _LocalHearthLimit;
        private Int32? _LocalHearthTimeLimit;

        private Int32? _GlobalEpidemyCompCount;
        private Int32? _GlobalEpidemyLimit;
        private Int32? _GlobalEpidemyTimeLimit;
                
        private Boolean _ReRead;
        
        #endregion

        #region Properties

        public String MailServer
        {
            get { return _MailServer; }
            set { _MailServer = value; }
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

        public Boolean ReRead
        {
            get { return _ReRead; }
            set { _ReRead = value; }
        }

        #endregion
        
        #region Constructors

        public NSSettings() { }

        #endregion
    }
}
