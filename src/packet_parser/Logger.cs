using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

/// <summary>
    /// Логгирует в файл на диске.
    /// Используется для логгирования ошибок.
    /// </summary>
public class Logger
{
    protected string path = String.Empty;
    protected Encoding encoding = Encoding.Unicode;
    protected bool append = true;

    protected string user = String.Empty;
    /// <summary>
    /// File logger
    /// </summary>
    /// <param name="logFile">file name</param>
    public Logger(string path)
    {
        this.path = path;
    }

    public Logger(string path, Encoding encoding)
    {
        this.path = path;
        this.encoding = encoding;
    }

    public Logger(string path, bool append, Encoding encoding)
    {
        this.path = path;
        this.encoding = encoding;
        this.append = append;
    }

    public Logger(string path, bool append, Encoding encoding, string user)
    {
        this.path = path;
        this.encoding = encoding;
        this.append = append;
        this.user = user;
    }

    /// <summary>
    /// Write text to logfile
    /// </summary>
    /// <param name="text">Text to write</param>
    /// <returns></returns>
    public bool Write(string text)
    {
        StreamWriter writer = null;
        try
        {
            writer = new StreamWriter(path, append, encoding);
            writer.WriteLine("[{0:G}] {1} : {2}", DateTime.Now, user, text);
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

    #region Property

    public string Path
    {
        get { return this.path; }
        set { this.path = value; }
    }

    public bool Append
    {
        get { return this.append; }
        set { this.append = value; }
    }

    public Encoding EncodingText
    {
        get { return this.encoding; }
        set { this.encoding = value; }
    }

    public string User
    {
        get { return this.user; }
        set { this.user = value; }
    }

    #endregion

}