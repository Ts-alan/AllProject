<%@ WebHandler Language="C#" Class="TreeWithGroupsHandler" %>

using System;
using System.Web;
using System.Configuration;
using VirusBlokAda.CC.JSON;
using System.Collections.Generic;
using VirusBlokAda.CC.DataBase;

public class TreeWithGroupsHandler : IHttpHandler {

    private List<TreeNodeJSONEntity> tree = new List<TreeNodeJSONEntity>();
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        List<Group> list = DBProviders.Group.GetGroups();

        Int32 index = 0;
        while (NextGroup(list, null, ref index))
        {
            tree.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[index], null, true, true, false, true, true));
            RecursiveAddChildren(tree[tree.Count - 1], list, index);
            index++;
        }
        
        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(tree));
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

    private void RecursiveAddChildren(TreeNodeJSONEntity node, List<Group> list, Int32 indexList)
    {
        //Groups
        Int32 i = 0;
        while (NextGroup(list, list[indexList].ID, ref i))
        {
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[i], null, true, true, false, true, true));
            RecursiveAddChildren(node.Children[node.Children.Count - 1], list, i);
            i++;
        }
        //Comps
        foreach (ComputersEntity comp in DBProviders.Group.GetComputersByGroup(list[indexList].ID))
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