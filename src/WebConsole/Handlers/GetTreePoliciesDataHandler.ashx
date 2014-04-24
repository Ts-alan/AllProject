<%@ WebHandler Language="C#" Class="GetTreePoliciesDataHandler" %>

using System;
using System.Web;
using VirusBlokAda.Vba32CC.JSON;
using VirusBlokAda.Vba32CC.JSON.Entities;
using System.Collections.Generic;
using VirusBlokAda.Vba32CC.Policies;
using VirusBlokAda.Vba32CC.DataBase;
using VirusBlokAda.Vba32CC.Policies.General;
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

        PolicyProvider providerPolicy = HttpContext.Current.Application["PoliciesState"] as PolicyProvider;
        if (providerPolicy == null)
        {
            providerPolicy = new PolicyProvider(System.Configuration.ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
            HttpContext.Current.Application["PoliciesState"] = providerPolicy;
        }
        //remove all computers from all policies
        providerPolicy.ClearAllPolicy();
        foreach (Policy policy in providerPolicy.GetPolicyTypes())
        {
            //add computers into policy
            providerPolicy.AddComputersToPolicy(policy, GetComputersByPolicy(policy, policyList));            
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