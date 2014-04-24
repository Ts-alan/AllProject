<%@ WebHandler Language="C#" Class="ComputerPageHandler" %>

using System;
using System.Web;
using VirusBlokAda.Vba32CC.Groups;
using System.Configuration;
using VirusBlokAda.Vba32CC.JSON;
using System.Collections.Generic;
using VirusBlokAda.Vba32CC.DataBase;
using VirusBlokAda.Vba32CC.JSON.Entities;

public class ComputerPageHandler : IHttpHandler {
    
    private List<TreeNodeJSONEntity> tree = new List<TreeNodeJSONEntity>();
    
    public void ProcessRequest (HttpContext context) {
        String where = context.Request.Params.Get("where");
    /*    if(!String.IsNullOrEmpty(where))
            where = where.Substring(1, where.Length - 2);*/
        if (String.IsNullOrEmpty(where)) where = null;

        context.Response.ContentType = "text/plain";
        GroupProvider provider = new GroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        List<Group> list = provider.GetGroups();

        //with groups
        Int32 index = 0;
        while (NextGroup(list, null, ref index))
        {
            tree.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[index], false, false, false, false, false, false));
            RecursiveAddChildren(tree[tree.Count - 1], provider, list, index, where);
            index++;
        }

        //without group
        tree.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(new Group(0, Resources.Resource.ComputersWithoutGroups, Resources.Resource.CompWithoutGroup, null), false, false, false, false, false, false));
        foreach (ComputersEntityEx comp in provider.GetComputersExWithoutGroup(where))
        {
            for (index = 0; index < comp.Components.Count; index++)
            {
                comp.Components[index].ComponentState = DatabaseNameLocalization.GetNameForCurrentCulture(comp.Components[index].ComponentState);
            }
            
            tree[tree.Count - 1].Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, false, false, false, true, false, false));
        }

        //Deleting empty nodes
        for (index = 0; index < tree.Count; index++)
        {
            if (DeleteEmptyNodes(tree[index], null))
                index--;
        }

        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(tree)); 
    }
 
    public Boolean IsReusable {
        get {
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

    private void RecursiveAddChildren(TreeNodeJSONEntity node, GroupProvider provider, List<Group> list, Int32 indexList, String where)
    {
        //Groups
        Int32 i = 0;
        while (NextGroup(list, list[indexList].ID, ref i))
        {
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[i], false, false, false, false, true, false));
            RecursiveAddChildren(node.Children[node.Children.Count - 1], provider, list, i, where);
            i++;
        }
        //Comps
        foreach (ComputersEntityEx comp in provider.GetComputersExByGroup(list[indexList].ID, where))
        {
            for (Int16 index = 0; index < comp.Components.Count; index++)
            {
                comp.Components[index].ComponentState = DatabaseNameLocalization.GetNameForCurrentCulture(comp.Components[index].ComponentState);
            }
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, false, false, false, true, false, false));
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