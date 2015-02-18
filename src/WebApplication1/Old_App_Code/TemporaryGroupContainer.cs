using System;
using System.Collections.Generic;
using System.Web;
using VirusBlokAda.CC.DataBase;
using System.Configuration;
using VirusBlokAda.CC.Common;

public static class TemporaryGroupContainer
{
    /// <summary>
    /// получение списка имен компьютеров
    /// </summary>
    /// <param name="type">тип информации</param>
    /// <param name="where">условие получения</param>
    /// <returns>список имен компьютеров</returns>
    public static List<String> GetComputerNameList(InformationListTypes type, String where)
    {
        if (where == String.Empty)
        {
            where = null;
        }
        return DBProviders.TemporaryGroup.GetComputerNameList(type.ToString().ToUpper(), where);
    }
}