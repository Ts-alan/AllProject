using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

    /// <summary>
    /// Class containing helper functions
    /// </summary>
public static class FileUtility
{
    /// <summary>
    /// Appends terminal backslash to given string
    /// </summary>
    /// <param name="path">String representing path</param>
    /// <returns>String ending with backslash</returns>
    public static string AppendTerminalBackslash(string path)
    {
        if (path.IndexOf(Path.DirectorySeparatorChar, path.Length - 1) == -1)
        {
            return path + Path.DirectorySeparatorChar;
        }
        else
        {
            return path;
        }
    }

    /// <summary>
    /// Create directory, if it doesn't exist
    /// </summary>
    /// <param name="path">Path with directory name</param>
    /// <returns>True if directory was created</returns>
    public static bool CreateDirectory(string path)
    {
        if (Directory.Exists(path))
            return false;
        Directory.CreateDirectory(path);
        return true;
    }
}
