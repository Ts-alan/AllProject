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

        List<TreeNodeJSONEntity> tree = new List<TreeNodeJSONEntity>();

        foreach (ComputersEntity comp in DBProviders.Group.GetComputersWithoutGroup())
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