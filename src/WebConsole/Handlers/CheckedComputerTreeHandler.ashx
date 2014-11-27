<%@ WebHandler Language="C#" Class="CheckedComputerTreeHandler" %>

using System;
using System.Web;
using System.Configuration;
using VirusBlokAda.CC.JSON;
using System.Collections.Generic;
using VirusBlokAda.CC.DataBase;

public class CheckedComputerTreeHandler : IHttpHandler {

    private List<TreeNodeJSONEntity> tree = new List<TreeNodeJSONEntity>();
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        GroupProvider provider = new GroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        List<Group> list = provider.GetGroups();

        //with groups
        Int32 index = 0;
        while (NextGroup(list, null, ref index))
        {
            tree.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[index], false, false, true));
            RecursiveAddChildren(tree[tree.Count - 1], list, index, provider);
            index++;
        }

        //without group
        tree.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(new Group(0, ResourceControl.GetStringForCurrentCulture("ComputersWithoutGroups"), "", null), false, true, true));
        foreach (ComputersEntity comp in provider.GetComputersWithoutGroup())
        {
            tree[tree.Count - 1].Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, false, false, true));
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

    /// <summary>
    /// Удаление пустых вершин
    /// </summary>
    /// <param name="node">Удаляемая вершина</param>
    /// <param name="parentNode">Родительская вершина </param>
    /// <returns></returns>
    private Boolean DeleteEmptyNodes(TreeNodeJSONEntity node, TreeNodeJSONEntity parentNode)
    {
        if (node.Children==null) return false;

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
    
    /// <summary>
    /// Рекурсивно добавляет дочерние узлы
    /// </summary>
    /// <param name="node">Исходная вершина</param>
    /// <param name="list">список групп </param>
    /// <param name="indexList">индекс </param>
    /// <param name="provider">провайдер групп </param>
    private void RecursiveAddChildren(TreeNodeJSONEntity node, List<Group> list, Int32 indexList, GroupProvider provider)
    {
        //Groups
        Int32 i = 0;
        while (NextGroup(list, list[indexList].ID, ref i))
        {
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[i], false, true, true));
            RecursiveAddChildren(node.Children[node.Children.Count - 1], list, i, provider);
            i++;
        }
        //Comps
        foreach (ComputersEntity comp in provider.GetComputersByGroup(list[indexList].ID))
        {
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, false, false, true));
        }
    }
    
    /// <summary>
    /// Поиск подгрупп
    /// </summary>
    /// <param name="list">список групп </param>
    /// <param name="parentID">ID группы </param>
    /// <param name="index">индекс начала поиска </param>
    /// <returns>true, если подгруппа найдена</returns>
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