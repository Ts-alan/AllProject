using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Collections;
using System.Resources;
using System.Globalization;



/// <summary>
/// Summary description for ResourceControl
/// </summary>
public class ResourceControl
{
    private string resourcePath = String.Empty;

    public ResourceControl()
    {
        
    }

    public bool IsExist(string name, string value)
    {

        string gg = (string)HttpContext.GetGlobalResourceObject("Resource", name, new CultureInfo("en-US"));
        if (gg == value) return true;

        gg = (string)HttpContext.GetGlobalResourceObject("Resource", name, new CultureInfo("ru-RU"));
        if (gg == value) return true;

        return false;       
    }

    public static string GetStringForCurrentCulture(string name)
    {
        return (string)HttpContext.GetGlobalResourceObject("Resource", name,
             new System.Globalization.CultureInfo((string)HttpContext.Current.Profile.GetPropertyValue("Culture")));
    }
}
