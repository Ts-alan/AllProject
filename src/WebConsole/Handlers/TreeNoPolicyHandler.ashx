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
        GroupProvider providerGroup = new GroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        List<Group> list = providerGroup.GetGroups();
        TreeNodeJSONEntity gr = TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(new Group(-1, Resources.Resource.NotAssignedExplicitly, Resources.Resource.NotAssignedExplicitly, null), null,true, true);
        
        //with groups
        Int32 index = 0;
        while (NextGroup(list, null, ref index))
        {
            
           gr.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[index], null, true, true));
           RecursiveAddChildren(gr.Children[gr.Children.Count - 1], list, index, providerGroup);
            index++;
        }

        //without group
        gr.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(new Group(0, Resources.Resource.ComputersWithoutGroups, "", null), null, true, true));
        foreach (ComputersEntity comp in providerGroup.GetComputersByGroupAndPolicy(null, null))
        {
            gr.Children[gr.Children.Count - 1].Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, null,true, true));
        }

        //Deleting empty nodes
        for (index = 0; index < gr.Children.Count; index++)
        {
            if (DeleteEmptyNodes(gr.Children[index], gr,gr))
                index--;
        }
        tree.Add(gr);
        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(tree));        
    }

    public Boolean IsReusable
    {
        get
        {
            return false;
        }
    }

    private Boolean DeleteEmptyNodes(TreeNodeJSONEntity node, TreeNodeJSONEntity parentNode,TreeNodeJSONEntity rootNode)
    {
        if (node.Children==null) return false;

        for (Int32 index = node.Children.Count - 1; index >= 0; index--)
        {
            DeleteEmptyNodes(node.Children[index], node,rootNode);             
        }

        if (node.Children.Count == 0)
        {
            if (parentNode != rootNode)
                parentNode.Children.Remove(node);
            else
            {
                rootNode.Children.Remove(node);
                return true;
            }
        }

        return false;
    }

    private void RecursiveAddChildren(TreeNodeJSONEntity node, List<Group> list, Int32 indexList, GroupProvider providerGroup)
    {
        //Groups
        Int32 i = 0;
        while (NextGroup(list, list[indexList].ID, ref i))
        {
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[i], null, true, true));
            RecursiveAddChildren(node.Children[node.Children.Count - 1], list, i, providerGroup);
            i++;
        }
        //Comps
        foreach (ComputersEntity comp in providerGroup.GetComputersByGroupAndPolicy(list[indexList], null))
        {
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, null, true, true));
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