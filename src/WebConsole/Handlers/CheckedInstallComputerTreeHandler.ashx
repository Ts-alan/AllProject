<%@ WebHandler Language="C#" Class="CheckedInstallComputerTreeHandler" %>

using System;
using System.Web;
using VirusBlokAda.CC.DataBase;
using System.Configuration;
using VirusBlokAda.CC.JSON;
using System.Collections.Generic;

public class CheckedInstallComputerTreeHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        InstallationTaskProvider provider = new InstallationTaskProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);

        List<TreeNodeJSONEntity> tree = new List<TreeNodeJSONEntity>();
        tree.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(
            new Group(0, Resources.Resource.Computers, Resources.Resource.Computers, null), false, false, false, false, true, true));

        foreach (String item in provider.GetComputerNames())
        {
            tree[0].Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(item, false, false, false, true, false, true)); 
        }
        
        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(tree));
    }

    public Boolean IsReusable
    {
        get
        {
            return false;
        }
    }
}