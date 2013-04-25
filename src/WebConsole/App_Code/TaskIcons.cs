using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Static class that provides path to images used by task controls.
/// </summary>
public static class TaskIcons
{
    public static string OptionsEnabled
    {
        get { return ImagesPath + "cog_enabled.png"; }
    }

    public static string OptionsEnabledLoading
    {
        get { return ImagesPath + "cog_enabled_loading.png"; }
    }

    public static string OptionsDisabled
    {
        get { return ImagesPath + "cog_disabled.png"; }
    }

    public static string OptionsDisabledLoading
    {
        get { return ImagesPath + "cog_disabled_loading.png"; }
    }

    public static string EraseDisabled
    {
        get { return ImagesPath + "erase_disabled.jpg"; }
    }

    public static string Erase
    {
        get { return ImagesPath + "erase.jpg"; }
    }

    public static string Delete
    {
        get { return ImagesPath + "delete.gif"; }
    }

    private static string ImagesPath
    {
        get { return String.Format("App_Themes/{0}/Images/", 
            HttpContext.Current.Profile.GetPropertyValue("Theme")); }
    }
}