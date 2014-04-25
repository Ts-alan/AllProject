using System;
using VirusBlokAda.CC.Common;

internal static class LoggerPP
{
    #region Fields

    private static Logger _logger;

    #endregion

    #region Properties

    public static Logger log
    {
        get { return _logger; }
    }

    public static LogLevel Level
    {
        get { return _logger.LoggingLevel; }
        set { _logger.LoggingLevel = value; }
    }

    public static String User
    {
        get { return _logger.User; }
        set { _logger.User = value; }
    }

    public static String Path
    {
        get { return _logger.Path; }
        set { _logger.Path = value; }
    }

    #endregion

    static LoggerPP()
    {
        String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\";
        _logger = new Logger(path + AppDomain.CurrentDomain.FriendlyName + ".log", LogLevel.Info, "Vba32PP");
    }
}