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
using VirusBlokAda.CC.DataBase;

/// <summary>
/// Сервисный класс, предоставляет различные методы для обслуживания
/// бизнес-логики веб-приложения
/// </summary>
public static class Anchor
{
    public static String GetStringForTaskGivedUser()
    {
        return String.Format("{0} ({1} {2}) {3}", 
            HttpContext.Current.Profile.UserName,
            HttpContext.Current.Profile.GetPropertyValue("FirstName"), 
            HttpContext.Current.Profile.GetPropertyValue("LastName"),
            HttpContext.Current.Request.UserHostAddress);
    }

    /// <summary>
    /// Обрамляет строку в тег font с заданным цветом
    /// </summary>
    /// <param name="str">Исходная строка</param>
    /// <param name="color">Цвет текса</param>
    /// <returns></returns>
    public static String SetColorToString(String str, String color)
    {
        return String.Format("<font color={1}>{0}</font>",str,color);
    }
    /// <summary>
    ///  Скролл к элементу страницы
    /// </summary>
    /// <param name="controlId">Ид элемента</param>
    /// <param name="_Page">страница</param>
    public static void ScrollToObj(String controlId, Page _Page)
    {
        if (!_Page.ClientScript.IsStartupScriptRegistered("obj"))
            _Page.ClientScript.RegisterStartupScript(typeof(Page), "obj", "document.getElementById('" + controlId + "').scrollIntoView(true);", true);
    }
    /// <summary>
    /// Скролл к вершине страницы
    /// </summary>
    /// <param name="_Page">страница</param>
    public static void ScrollToTop(Page _Page)
    {
        if (!_Page.ClientScript.IsStartupScriptRegistered("top"))
            _Page.ClientScript.RegisterStartupScript(typeof(Page), "top", "window.scrollTo(0,0);", true);
    }
    /// <summary>
    /// Добавление тэга перевода строки
    /// </summary>
    /// <param name="source">исходная строка</param>
    /// <param name="size">длина</param>
    /// <returns>строка с тэгами</returns>
    public static String FixString(String source, int size)
    {
        String dest = String.Empty;
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
    /// <summary>
    /// Добавление тэга перевода строки
    /// </summary>
    /// <param name="source">исходная строка</param>
    /// <param name="size">длина</param>
    /// <param name="symbol">символ, после которого возможен перевод</param>
    /// <returns>строка с тэгами</returns>
    public static String FixString(String source, Int32 size, Char symbol)
    {
        String dest = String.Empty;
        Int32 counter = 0;
        Boolean change = false;
        for (Int32 i = 0; i < source.Length; i++)
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
    /// <summary>
    /// Конвертация комментария
    /// </summary>
    /// <param name="comment">комментарий</param>
    /// <returns>комментарий с тэгами перевода строки</returns>
    public static String ConvertComment(String comment)
    {
        String result = "";
        try
        {
            result = FixString(HttpContext.Current.Server.HtmlEncode(comment), 30);
        }
        catch
        {
        }
        return result;
    }

    /// <summary>
    ///  Получение списка интервалов времени и даты
    /// </summary>
    /// <returns></returns>
    public static List<String> GetDateIntervals()
    {
        List<String> list = new List<String>();
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
}