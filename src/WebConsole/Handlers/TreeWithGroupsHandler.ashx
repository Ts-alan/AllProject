<%@ WebHandler Language="C#" Class="TreeWithGroupsHandler" %>

using System;
using System.Web;
using VirusBlokAda.Vba32CC.Groups;
using System.Configuration;
using VirusBlokAda.Vba32CC.JSON;
using System.Collections.Generic;
using ARM2_dbcontrol.DataBase;
using VirusBlokAda.Vba32CC.JSON.Entities;

public class TreeWithGroupsHandler : IHttpHandler {

    private List<TreeNodeJSONEntity> tree = new List<TreeNodeJSONEntity>();
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        GroupProvider provider = new GroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);        
        List<Group> list = provider.GetGroups();

        Int32 index = 0;
        while (NextGroup(list, null, ref index))
        {
            tree.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[index], null, true, true, false, true, true));
            RecursiveAddChildren(tree[tree.Count - 1], provider, list, index);
            index++;
        }
        
        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(tree));
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

    private void RecursiveAddChildren(TreeNodeJSONEntity node, GroupProvider provider, List<Group> list, Int32 indexList)
    {
        //Groups
        Int32 i = 0;
        while (NextGroup(list, list[indexList].ID, ref i))
        {
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[i], null, true, true, false, true, true));
            RecursiveAddChildren(node.Children[node.Children.Count - 1], provider, list, i);
            i++;
        }
        //Comps
        foreach (ComputersEntity comp in provider.GetComputersByGroup(list[indexList].ID))
        {
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, null, true, false, true, true, true));
        }
    }

    private Boolean NextGroup(List<Group> list, Int32? parentID, ref Int32 index)
    {
        for (Int32 i = index; i < list.Count; i++)
        {
            if (list[i].ParentID == parentID)
            {
                index = i;
                return true;
            }
        }
        return false;
    }
}