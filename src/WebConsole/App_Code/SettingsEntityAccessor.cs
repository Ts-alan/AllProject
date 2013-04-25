using System;
using System.Collections.Generic;
using System.Web;
using ARM2_dbcontrol.DataBase;
using System.Web.SessionState;


public static class SettingsEntityAccessor
{
    private static readonly string sessionKey = "Settings";
    public static SettingsEntity GetSettingsEntity()
    {
        HttpSessionState session = HttpContext.Current.Session;
        ProfileCommon profile = (ProfileCommon)HttpContext.Current.Profile;
        SettingsEntity settings;
        if (session[sessionKey] == null)
        {
            settings = new SettingsEntity();
            try
            {
                settings = settings.Deserialize(profile.Settings);
            }
            finally
            {
                session[sessionKey] = settings;
            }
        }
        else
        {
            settings = (SettingsEntity)session[sessionKey];
        }
        return settings;
    }

    public static void SetSettingsEntity(SettingsEntity settings)
    {
        HttpSessionState session = HttpContext.Current.Session;
        ProfileCommon profile = (ProfileCommon)HttpContext.Current.Profile;
        session[sessionKey] = settings;
        profile.Settings = settings.Serialize();
    }
}