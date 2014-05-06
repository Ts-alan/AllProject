using System;
using System.Collections.Generic;
using System.Web;
using VirusBlokAda.CC.DataBase;
using System.Configuration;
using VirusBlokAda.CC.Common;

public static class TemporaryGroupContainer
{
    public static List<String> GetComputerNameList(InformationListTypes type, String where)
    {
        if (where == String.Empty)
        {
            where = null;
        }
        return DBProviders.TemporaryGroup.GetComputerNameList(type.ToString().ToUpper(), where);
    }
}