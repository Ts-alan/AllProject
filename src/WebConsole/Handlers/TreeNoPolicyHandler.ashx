<%@ WebHandler Language="C#" Class="TreeNoPolicyHandler" %>

using System;
using System.Web;
using System.Configuration;
using VirusBlokAda.CC.JSON;
using System.Collections.Generic;
using VirusBlokAda.CC.DataBase;

public class TreeNoPolicyHandler : IHttpHandler {

    private List<TreeNodeJSONEntity> tree = new List<TreeNodeJSONEntity>();
    
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        GroupProvider provider = new GroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        List<Group> list = provider.GetGroups();

        //with groups
        Int32 index = 0;
        while (NextGroup(list, null, ref index))
        {
            tree.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[index], null, true, false, false, true, true));
            RecursiveAddChildren(tree[tree.Count - 1], provider, list, index);
            index++;
        }

        //without group
        tree.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(new Group(0, Resources.Resource.ComputersWithoutGroups, "", null), null, true, false, false, true, true));
        foreach (ComputersEntity comp in provider.GetComputersByGroupAndPolicy(null, null))
        {
            tree[tree.Count - 1].Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, null, true, false, true, true, true));
        }

        //Deleting empty nodes
        for (index = 0; index < tree.Count; index++)
        {
            if (DeleteEmptyNodes(tree[index], null))
                index--;
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

    private Boolean DeleteEmptyNodes(TreeNodeJSONEntity node, TreeNodeJSONEntity parentNode)
    {
        if (node.IsLeaf) return false;

        for (Int32 index = node.Children.Count - 1; index >= 0; index--)
        {
            DeleteEmptyNodes(node.Children[index], node);             
        }

        if (node.Children.Count == 0)
        {
            if (parentNode != null)
                parentNode.Children.Remove(node);
            else
            {
                tree.Remove(node);
                return true;
            }
        }

        return false;
    }

    private void RecursiveAddChildren(TreeNodeJSONEntity node, GroupProvider provider, List<Group> list, Int32 indexList)
    {
        //Groups
        Int32 i = 0;
        while (NextGroup(list, list[indexList].ID, ref i))
        {
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[i], null, true, false, false, true, true));
            RecursiveAddChildren(node.Children[node.Children.Count - 1], provider, list, i);
            i++;
        }
        //Comps
        foreach (ComputersEntity comp in provider.GetComputersByGroupAndPolicy(list[indexList], null))
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