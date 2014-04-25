using System;
using System.Collections.Generic;
using System.Web;
using VirusBlokAda.CC.DataBase;
using System.Configuration;
using VirusBlokAda.CC.Common;


public static class TemporaryGroupContainer
{
    private static TemporaryGroupProvider provider;

	static TemporaryGroupContainer()
	{
        provider = new TemporaryGroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
	}

    public static List<String> GetComputerNameList(InformationListTypes type, String where)
    {
        if (where == String.Empty)
        {
            where = null;
        }
        return provider.GetComputerNameList(type.ToString().ToUpper(), where);
    }
}