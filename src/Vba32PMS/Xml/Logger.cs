using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Diagnostics;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService.Xml
{
    /// <summary>
    /// Логгирует в файл на диске.
    /// </summary>
    internal static class Logger
    {
        #region Fields

        private static String path = String.Empty;
        private static Encoding encoding = Encoding.Default;
        private static Boolean append = true;
        private static String user = String.Empty;
        private static LogLevel loggingLevel = LogLevel.Debug;

        #endregion

        #region Property

        public static String Path
        {
            get { return path; }
            set { path = value; }
        }

        public static Boolean Append
        {
            get { return append; }
            set { append = value; }
        }

        public static Encoding EncodingText
        {
            get { return encoding; }
            set { encoding = value; }
        }

        public static String User
        {
            get { return user; }
            set { user = value; }
        }

        public static LogLevel LoggingLevel
        {
            get { return loggingLevel; }
            set { loggingLevel = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Write text to logfile
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <returns></returns>
        private static Boolean Write(String text, LogLevel logLevel)
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

        public static void Fatal(String errorMessage)
        {
            Write(errorMessage, LogLevel.Fatal);
            EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, EventLogEntryType.Error);
        }

        public static void Error(String errorMessage)
        {
            Write(errorMessage, LogLevel.Error);
            EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, EventLogEntryType.Error);
        }

        public static void Warning(String errorMessage)
        {
            Write(errorMessage, LogLevel.Warning);
            EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, EventLogEntryType.Warning);
        }

        public static void Info(String errorMessage)
        {
            Write(errorMessage, LogLevel.Info);
        }

        public static void Debug(String errorMessage)
        {
            Write(errorMessage, LogLevel.Debug);
        }

        public static void Trace(String errorMessage)
        {
            Write(errorMessage, LogLevel.Trace);
        }

        #endregion

    }
}
