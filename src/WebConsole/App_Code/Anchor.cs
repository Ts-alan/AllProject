using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// Сервисный класс, предоставляет различные методы для обслуживания
/// бизнес-логики веб-приложения
/// </summary>
public static class Anchor
{
    public static string GetStringForTaskGivedUser()
    {
        return String.Format("{0} ({1} {2}) {3}", 
            HttpContext.Current.Profile.UserName,
            HttpContext.Current.Profile.GetPropertyValue("FirstName"), 
            HttpContext.Current.Profile.GetPropertyValue("LastName"),
            HttpContext.Current.Request.UserHostAddress);
    }

    public static string ConvertToDumpString(byte[] data)
    {
        string str = String.Empty;
        foreach (byte b in data)
        {
            str += b.ToString("X2");
        }
        return str;
    }

    /// <summary>
    /// Обрамляет строку в тег font с заданным цветом
    /// </summary>
    /// <param name="str">Исходная строка</param>
    /// <param name="color">Цвет текса</param>
    /// <returns></returns>
    public static string SetColorToString(string str, string color)
    {
        return String.Format("<font color={1}>{0}</font>",str,color);
    }

   /* public static SettingsEntity GetSettings()
    {
        
        SettingsEntity settings;
        if (HttpContext.Current.Session["Settings"] == null)
        {
            settings = new SettingsEntity();
            try
            {
                
                settings = settings.Deserialize((string)HttpContext.Current.Profile.GetPropertyValue("Settings"));
            }
            catch
            {
                settings = new SettingsEntity();
            }
            finally
            {
                HttpContext.Current.Session["Settings"] = settings;
            }
        }
        else
        {
            settings = (SettingsEntity)HttpContext.Current.Session["Settings"];
        }

        return settings;
    }*/



    public static void ScrollToObj(string controlId, Page _Page)
    {
        if (!_Page.ClientScript.IsStartupScriptRegistered("obj"))
            _Page.ClientScript.RegisterStartupScript(typeof(Page), "obj", "document.getElementById('" + controlId + "').scrollIntoView(true);", true);
    }

    public static void ScrollToTop(Page _Page)
    {
        if (!_Page.ClientScript.IsStartupScriptRegistered("top"))
            _Page.ClientScript.RegisterStartupScript(typeof(Page), "top", "window.scrollTo(0,0);", true);
    }

    public static string FixString(string source, int size)
    {
        string dest = String.Empty;
        int counter = 0;
        for (int i = 0; i < source.Length; i++)
        {
            if ((source[i] != ' '))
                counter++;
            else
                counter = 0;
            dest += source[i];
            if ((i % size == 0)&&(i!=0)&&(counter>=size))
                dest += "<br>"; 

        }
            return dest;
    }

    public static string FixString(string source, int size, char symbol)
    {
        string dest = String.Empty;
        int counter = 0;
        bool change = false;
        for (int i = 0; i < source.Length; i++)
        {
            counter++;
            if (counter > size)
                change = true;

            dest += source[i];
            if ((change) && (source[i] == symbol))
            {
                dest += "<br>";
                change = false;
                counter = 0;
            }         
        }
        return dest;
    }

    public static string FromBase64String(string source)
    {
        byte[] bs = Convert.FromBase64String(source);
        return Encoding.UTF8.GetString(bs);
    }

    public static string GetCommentFromSerial(string serial)
    {
        string comment = "";
        try
        {
            comment = FixString(
            HttpContext.Current.Server.HtmlEncode(FromBase64String(serial)), 30);
        }
        catch
        {
        }
        return comment;
    }


    public static List<string> GetDateIntervals()
    {
        List<string> list = new List<string>();
        list.Add("1 " + Resources.Resource.Minute);
        list.Add("5 " + Resources.Resource.Minutes);
        list.Add("15 " + Resources.Resource.Minutes);
        list.Add("30 " + Resources.Resource.Minutes);
        list.Add("1 " + Resources.Resource.Hour2);
        list.Add("3 " + Resources.Resource.Hours);
        list.Add("6 " + Resources.Resource.Hours);
        list.Add("12 " + Resources.Resource.Hours);
        list.Add("1 " + Resources.Resource.Day2);
        list.Add("2 " + Resources.Resource.Days2);
        list.Add("3 " + Resources.Resource.Days2);
        list.Add("1 " + Resources.Resource.Week2);
        list.Add("2 " + Resources.Resource.Weeks2);
        list.Add("3 " + Resources.Resource.Weeks2);
        list.Add("1 " + Resources.Resource.Month2);
        list.Add("2 " + Resources.Resource.Months2);
        list.Add("3 " + Resources.Resource.Months2);
        list.Add("6 " + Resources.Resource.Months2);
        list.Add("1 " + Resources.Resource.Year);

        return list;
    }

    public static string GetMd5Hash(string input)
    {
        // Create a new instance of the MD5CryptoServiceProvider object.
        MD5 md5Hasher = MD5.Create();

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
}
