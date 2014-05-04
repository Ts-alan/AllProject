<%@ WebHandler Language="C#" Class="GetTreeDataHandler" %>

using System;
using System.Web;
using VirusBlokAda.CC.DataBase;
using System.Collections.Generic;
using VirusBlokAda.CC.JSON;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
public class GetTreeDataHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        String groupTreeArray = context.Request.Params.Get("groupTreeArray");
        context.Response.ContentType = "text/plain";

        try
        {
            SaveChanges(groupTreeArray);
        }
        catch (Exception e)
        {
            context.Response.Write("{\"success\":false,\"data\":{\"result\":\"" + e.Message +"\"}}"); //run fail
            return;
        }

        context.Response.Write("{\"success\":true,\"data\":{\"result\":\"Success!\"}}"); //run successfully

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    #region Private Metods

    private void SaveChanges(String groupTreeArray)
    {
        List<TreeNodeEntity> groupList = JsonConvert.DeserializeObject<List<TreeNodeEntity>>(groupTreeArray);
        GroupProvider provider = new GroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        List<Group> groups = provider.GetGroups();
        #region Groups
        //Add and rename  groups
        for (Int32 i = groupList.Count - 1; i >= 0; i--)
        {
            Int32 parentID = 0;
            Int32.TryParse(groupList[i].ParentID.Substring(6), out parentID);
            if (!groupList[i].IsLeaf)
            {

                Int32 id;
                if (groupList[i].NodeID.Contains("GroupNew_"))
                {
                    // Add new group                    
                    id = provider.Add(new Group(groupList[i].NodeName, groupList[i].Comment, parentID == 0 ? (Int32?)null : parentID));
                    ChangeParentID(ref groupList, groupList[i].NodeID, String.Format("Group_{0}", id));
                    groupList[i] = new TreeNodeEntity(String.Format("Group_{0}", id), groupList[i].NodeName, groupList[i].ParentID, groupList[i].Comment, groupList[i].IsLeaf);
                }
                else
                {
                    id = Int32.Parse(groupList[i].NodeID.Substring(6));
                    Int32 index = FindGroupIndexByID(groups, id);

                    if (index != -1)
                    {
                        //New name
                        String newGroupName = null;
                        if (groups[index].Name != groupList[i].NodeName)
                        {
                            newGroupName = groupList[i].NodeName;
                        }
                        //New comment
                        String newComment = null;
                       
                        if (!(String.IsNullOrEmpty(groups[index].Comment) &&
                                    String.IsNullOrEmpty(groupList[i].Comment)))
                        {
                            if (groups[index].Comment != groupList[i].Comment)
                            {
                                newComment = groupList[i].Comment;
                            }
                        }
                        //New parentID
                        Int32? newParentID = null;
                        newParentID = parentID;
                        provider.Update(groups[index].Name, newGroupName, newComment, newParentID);
                    }
                    else
                    {
                        throw new Exception("Save on GroupAdministrate: No find group.");
                    }
                    
                }
            }
        }
        //Delete old groups
        foreach (Group group in groups)
        {
            if (FindGroupIndexByID(groupList, group.ID) == -1)
                provider.Delete(group);
        }
        #endregion

        #region Computers
        List<String> list = new List<String>();
        foreach (ChildParentEntity comp in provider.GetComputersWithGroups())
        {
            Int32? newParentID = GetParentID(comp.ChildID, groupList);
            if (newParentID != comp.ParentID)
            {
                //Delete comp from group
                if (newParentID == null)
                {
                    provider.DeleteComputerFromGroup((Int16)comp.ChildID);
                }
                else
                {
                    //Add computer in group
                    if (comp.ParentID == null)
                    {
                        provider.AddComputerInGroup((Int16)comp.ChildID, (Int32)newParentID);
                    }
                    else
                    {
                        provider.MoveComputerBetweenGroups((Int16)comp.ChildID, (Int32)newParentID);
                    }
                }
            }
        }

        #endregion
    }

    private Int32? GetParentID(Int32 compID, List<TreeNodeEntity> groupList)
    {
        //with group
        Int32 ParentId = 0;
        foreach (TreeNodeEntity node in groupList)
        {
            if (node.IsLeaf && node.NodeID == compID.ToString())
            {
                if (Int32.TryParse(node.ParentID.Substring(6), out ParentId))
                    return ParentId;
                else return null;

            }
        }

        //without group
        return null;
    }

    private void ChangeParentID(ref List<TreeNodeEntity> list, String oldID, String newID)
    {
        for (Int32 i = 0; i < list.Count; i++)
        {
            if (list[i].ParentID == oldID)
            {
                list[i] = new TreeNodeEntity(list[i].NodeID, list[i].NodeName, newID, list[i].Comment, list[i].IsLeaf);
            }
        }
    }

    private Int32 FindGroupIndexByID(List<Group> list, Int32 id)
    {
        for (Int32 i = 0; i < list.Count; i++)
        {
            if (list[i].ID == id) return i;
        }

        return -1;
    }

    private int FindGroupIndexByID(List<TreeNodeEntity> list, Int32 id)
    {
        for (Int32 i = 0; i < list.Count; i++)
        {
            if (list[i].NodeID == String.Format("Group_{0}", id)) return i;
        }

        return -1;
    }

        
    #endregion
}