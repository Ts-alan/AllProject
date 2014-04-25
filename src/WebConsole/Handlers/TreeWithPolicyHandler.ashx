<%@ WebHandler Language="C#" Class="TreeWithPolicyHandler" %>

using System;
using System.Web;
using System.Configuration;
using VirusBlokAda.CC.JSON;
using System.Collections.Generic;
using VirusBlokAda.CC.DataBase;

public class TreeWithPolicyHandler : IHttpHandler
{
    public int idCount = 0;
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        GroupProvider provider = new GroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        PolicyProvider providerPolicy = new PolicyProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        List<TreeNodeJSONEntity> tree = new List<TreeNodeJSONEntity>();
        List<Policy> policyList = providerPolicy.GetPolicyTypes();
        TreeNodeJSONEntity policyNode;
        String policyId;
        foreach (Policy policy in policyList)
        {
            policyId = policy.ID.ToString();
            policyNode = BuildPolicyNode(policy, providerPolicy.GetComputersByPolicyPage(policy, 1, Int16.MaxValue, null), provider);
            UpdateGroupId(policyNode.Children, policyId);
            tree.Add(policyNode);
  
        }
        
        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(tree));
    }
    private void UpdateGroupId(List<TreeNodeJSONEntity> tree, String policyId)
    {
        if (tree.Count == 0)
        {
            return;
        }
        else
        {
            foreach (TreeNodeJSONEntity node in tree)
            {
                if (node.Id.Contains("Group"))
                {
                    node.Id = node.Id +"__"+ policyId;
                    UpdateGroupId(node.Children, policyId);
                }
            }
        }
    }
    public Boolean IsReusable {
        get {
            return false;
        }
    }

    private TreeNodeJSONEntity BuildPolicyNode(Policy policy, List<ComputersEntity> comps, GroupProvider provider)
    {
        List<Group> list;
        PolicyBranchOfTree policyBranch = new PolicyBranchOfTree(policy);

        foreach (ComputersEntity comp in comps)
        {
            list = provider.GetGroupListByComputerID(comp.ID);
            if (list.Count == 0)
            {
                policyBranch.AddComputer(comp);
            }
            else
            {
               
                policyBranch.AddBranch(BuildBranch(comp, list.Count - 1, list));
            }
        }
        //PolicyBranch is created.

        TreeNodeJSONEntity node = TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(policy, null, false, true, false, true, true);
        node.Children = RecursiveConvertToTreeNodeJSON(policyBranch.Branches, new List<ComputersEntity>());
        if (policyBranch.Computers.Count != 0)
        {
            node.Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(new Group(0, Resources.Resource.ComputersWithoutGroups, "", null), null, true, false, false, true, true));
            node.Children[node.Children.Count - 1].Children = new List<TreeNodeJSONEntity>();
            foreach (ComputersEntity comp in policyBranch.Computers)
            {
                node.Children[node.Children.Count-1].Children.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, null, true, false, true, true, true));
            }
        }
        
        return node;
    }

    private BranchOfTree BuildBranch(ComputersEntity comp, Int32 index, List<Group> list)
    {
        BranchOfTree branch = new BranchOfTree(list[index]);
        if (index == 0)
        {
            branch.AddComputer(comp);
        }
        else
        {
            branch.AddBranch(BuildBranch(comp, index - 1, list));
        }
        return branch;
    }

    private List<TreeNodeJSONEntity> RecursiveConvertToTreeNodeJSON(List<BranchOfTree> branches, List<ComputersEntity> computers)
    {
        List<TreeNodeJSONEntity> list = new List<TreeNodeJSONEntity>();
        foreach (ComputersEntity comp in computers)
        {
            list.Add(TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(comp, null, true, false, true, true, true)); 
        }

        foreach (BranchOfTree branch in branches)
        {
            TreeNodeJSONEntity tmpNode = TreeJSONEntityConverter.ConvertToTreeNodeJsonEntity(branch.Root, null, true, false, false, true, true);
            tmpNode.Children = RecursiveConvertToTreeNodeJSON(branch.ChildrenBranchs, branch.Computers);
            list.Add(tmpNode);
        }

        return list;         
    }

}