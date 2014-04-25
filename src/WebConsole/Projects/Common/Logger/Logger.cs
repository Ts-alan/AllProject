using System;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace VirusBlokAda.CC.Common
{
    /// <summary>
    /// Log to file.
    /// </summary>
    public sealed class Logger
    {
        #region Fields

        private String path;
        private Encoding encoding;
        private Boolean append;
        private String user;
        private LogLevel loggingLevel;

        #endregion
        
        #region Property

        public String Path
        {
            get { return path; }
            set { path = value; }
        }

        public Boolean Append
        {
            get { return append; }
            set { append = value; }
        }

        public Encoding EncodingText
        {
            get { return encoding; }
            set { encoding = value; }
        }

        public String User
        {
            get { return user; }
            set { user = value; }
        }

        public LogLevel LoggingLevel
        {
            get { return loggingLevel; }
            set { loggingLevel = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// File logger
        /// </summary>
        /// <param name="logFile">file name</param>
        public Logger(String path)
            :this(path, LogLevel.Info)
        {
        }

        public Logger(String path, LogLevel level)
            : this(path, level, String.Empty)
        {
        }

        public Logger(String path, LogLevel level, String user)
            : this(path, level, Encoding.Unicode, true, user)
        {
        }

        public Logger(String path, LogLevel level, Encoding encoding, Boolean append, String user)
        {
            this.path = path;
            this.LoggingLevel = level;
            this.encoding = encoding;            
            this.append = append;
            this.user = user;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Write text to logfile
        /// </summary>
        /// <param name="text">Text for writing</param>
        /// <returns>Result operation</returns>
        private Boolean Write(String text, LogLevel logLevel)
        {
            if (logLevel > LoggingLevel) return false;
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(path, append, encoding);
                writer.WriteLine("[{0:G}] {1} : {2}! {3}", DateTime.Now, user, logLevel, text);
                writer.Close();
            }
            catch
            {
                if (writer != null)
                    writer.Close();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Write fatal error to logfile and EventLog
        /// </summary>
        /// <param name="errorMessage">Text for writing</param>
        public void Fatal(String errorMessage)
        {
            Write(errorMessage, LogLevel.Fatal);
            EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, EventLogEntryType.Error);
        }

        /// <summary>
        /// Write error to logfile and EventLog
        /// </summary>
        /// <param name="errorMessage">Text for writing</param>
        public void Error(String errorMessage)
        {
            Write(errorMessage, LogLevel.Error);
            EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, EventLogEntryType.Error);
        }

        /// <summary>
        /// Write warning to logfile
        /// </summary>
        /// <param name="errorMessage">Text for writing</param>
        public void Warning(String errorMessage)
        {
            Write(errorMessage, LogLevel.Warning);
            EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, EventLogEntryType.Warning);
        }

        /// <summary>
        /// Write info to logfile
        /// </summary>
        /// <param name="errorMessage">Text for writing</param>
        public void Info(String errorMessage)
        {
            Write(errorMessage, LogLevel.Info);
        }

        /// <summary>
        /// Write debug info to logfile
        /// </summary>
        /// <param name="errorMessage">Text for writing</param>
        public void Debug(String errorMessage)
        {
            Write(errorMessage, LogLevel.Debug);
        }

        /// <summary>
        /// Write trace info to logfile
        /// </summary>
        /// <param name="errorMessage">Text for writing</param>
        public void Trace(String errorMessage)
        {
            Write(errorMessage, LogLevel.Trace);
        }

        #endregion
    }
}
