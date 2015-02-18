<%@ WebHandler Language="C#" Class="ComputerPageHandler" %>

using System;
using System.Web;
using System.Configuration;
using VirusBlokAda.CC.JSON;
using System.Collections.Generic;
using VirusBlokAda.CC.DataBase;

public class ComputerPageHandler : IHttpHandler {
    
    private List<TreeNodeJSONEntity> tree = new List<TreeNodeJSONEntity>();
    
    public void ProcessRequest (HttpContext context) {
        String where = context.Request.Params.Get("where");
        String selected = context.Request.Params.Get("selected");
        
        List<String> selectedComps=null;
        if (String.IsNullOrEmpty(selected)) selected = null;
        if (selected != null)
            selectedComps = new List<string>(selected.Split('&'));
        

        if (String.IsNullOrEmpty(where)) where = null;
        GroupProvider provider = new GroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        context.Response.ContentType = "text/plain";
        List<Group> list = provider.GetGroups();

        //with groups
        Int32 index = 0;
        while (NextGroup(list, null, ref index))
        {
            tree.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[index], false, false, false));
            RecursiveAddChildren(tree[tree.Count - 1], list, index, where, provider,selectedComps);
            index++;
        }

        //without group
        bool isSelected = false;
        tree.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(new Group(0, Resources.Resource.ComputersWithoutGroups, Resources.Resource.CompWithoutGroup, null), false,false, false));
        foreach (ComputersEntityEx comp in provider.GetComputersExWithoutGroup(where))
        {
            for (index = 0; index < comp.Components.Count; index++)
            {
                comp.Components[index].ComponentState = DatabaseNameLocalization.GetNameForCurrentCulture(comp.Components[index].ComponentState);
            }
            isSelected = false;
            if (selectedComps != null)
            {
                if (selectedComps.Contains(comp.ComputerName.ToLower()))
                    isSelected = true;
            }
            tree[tree.Count - 1].Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, isSelected, false, false));
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
    /// Удаление вершин
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
    /// Рекурсивно добавление дочерних узлов
    /// </summary>
    /// <param name="node">Исходная вершина</param>
    /// <param name="list">список групп</param>
    /// <param name="indexList">индекс</param>
    /// <param name="where">условие выбора</param>
    /// <param name="provider">провайдер групп</param>
    /// <param name="selectedComps">выбранные компьютеры</param>
    /// <returns></returns>
    private void RecursiveAddChildren(TreeNodeJSONEntity node, List<Group> list, Int32 indexList, String where, GroupProvider provider,List<String> selectedComps)
    {
        //Groups
        Int32 i = 0;
        while (NextGroup(list, list[indexList].ID, ref i))
        {
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(list[i], false,true, false));
            RecursiveAddChildren(node.Children[node.Children.Count - 1], list, i, where, provider,selectedComps);
            i++;
        }
        //Comps
        bool isSelected=false;
        foreach (ComputersEntityEx comp in provider.GetComputersExByGroup(list[indexList].ID, where))
        {
            for (Int16 index = 0; index < comp.Components.Count; index++)
            {
                comp.Components[index].ComponentState = DatabaseNameLocalization.GetNameForCurrentCulture(comp.Components[index].ComponentState);
            }
            isSelected=false;
            if (selectedComps!= null)
            {
                if (selectedComps.Contains(comp.ComputerName.ToLower()))
                    isSelected = true;
            }
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, isSelected,false, false));
        }
    }

    /// <summary>
    /// Поиск подгрупп в списке
    /// </summary>
    /// <param name="list">список групп </param>
    /// <param name="parentID">ID группы </param>
    /// <param name="index">индекс начала поиска</param>
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