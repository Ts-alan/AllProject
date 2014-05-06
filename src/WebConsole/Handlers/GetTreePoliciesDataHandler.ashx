<%@ WebHandler Language="C#" Class="GetTreePoliciesDataHandler" %>

using System;
using System.Web;
using VirusBlokAda.CC.JSON;
using System.Collections.Generic;
using VirusBlokAda.CC.DataBase;
using Newtonsoft.Json;

public class GetTreePoliciesDataHandler : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        String policyTreeArray = context.Request.Params.Get("policyTreeArray");        
        context.Response.ContentType = "text/plain";
        try
        {
            SaveChanges(policyTreeArray);
        }
        catch(Exception e)
        {
            context.Response.Write("{\"success\":false,\"data\":{\"result\":\"" + e.Message + "\"}}"); //run fail
            return;             
        }
        context.Response.Write("{\"success\":true,\"data\":{\"result\":\"Success!\"}}"); //run successfully
        
    }
 
    public Boolean IsReusable {
        get {
            return false;
        }
    }
    
    #region Private Metods

    private void SaveChanges(String policyTreeArray)
    {
        List<TreeNodeEntity> policyList = JsonConvert.DeserializeObject<List<TreeNodeEntity>>(policyTreeArray);

        //remove all computers from all policies
        DBProviders.Policy.ClearAllPolicy();
        foreach (Policy policy in DBProviders.Policy.GetPolicyTypes())
        {
            //add computers into policy
            DBProviders.Policy.AddComputersToPolicy(policy, GetComputersByPolicy(policy, policyList));            
        }
    }

    private List<String> GetComputersByPolicy(Policy policy, List<TreeNodeEntity> nodes)
    {
        List<String> list = new List<String>();
        foreach (TreeNodeEntity node in nodes)
        {
            if (node.ParentID == String.Format("Policy_{0}", policy.ID))
                list.Add(node.NodeName);
        }
        return list;
    }

    #endregion
}