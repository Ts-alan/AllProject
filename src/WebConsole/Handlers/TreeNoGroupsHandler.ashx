<%@ WebHandler Language="C#" Class="TreeNoGroupsHandler" %>

using System;
using System.Web;
using System.Configuration;
using VirusBlokAda.CC.JSON;
using System.Collections.Generic;
using VirusBlokAda.CC.DataBase;

public class TreeNoGroupsHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        GroupProvider provider = new GroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);

        List<TreeNodeJSONEntity> tree = new List<TreeNodeJSONEntity>();

        foreach (ComputersEntity comp in provider.GetComputersWithoutGroup())
        {
            tree.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, null, true, false, true, false, true));
        }

        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(tree));   
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}