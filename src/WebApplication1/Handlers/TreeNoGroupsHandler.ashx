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
        GroupProvider providerGroup = new GroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        TreeNodeJSONEntity gr = TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(new Group(-1, Resources.Resource.ComputersWithoutGroups, Resources.Resource.ComputersWithoutGroups, null), null, true, true);
        
        foreach (ComputersEntity comp in providerGroup.GetComputersWithoutGroup())
        {
            gr.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, null,false, true));
        }
        tree.Add(gr);
        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(tree));   
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}