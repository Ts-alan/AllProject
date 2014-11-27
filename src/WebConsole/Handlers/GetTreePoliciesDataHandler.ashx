<%@ WebHandler Language="C#" Class="GetTreePoliciesDataHandler" %>

using System;
using System.Web;
using VirusBlokAda.CC.JSON;
using System.Collections.Generic;
using VirusBlokAda.CC.DataBase;
using Newtonsoft.Json;
using System.Configuration;

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
    /// <summary>
    /// Сохранить изменения
    /// </summary>
    /// <param name="policyTreeArray">список вершин дерева политик</param>
    private void SaveChanges(String policyTreeArray)
    {
        List<TreeNodeEntity> policyList = JsonConvert.DeserializeObject<List<TreeNodeEntity>>(policyTreeArray);
        PolicyProvider provider = new PolicyProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        //remove all computers from all policies
        provider.ClearAllPolicy();
        foreach (Policy policy in provider.GetPolicyTypes())
        {
            //add computers into policy
            provider.AddComputersToPolicy(policy, GetComputersByPolicy(policy, policyList));            
        }
    }
    
    /// <summary>
    /// Получение компьютеров по политике
    /// </summary>
    /// <param name="policy">политика</param>
    /// <param name="nodes">список вершин дерева</param>
    /// <returns>список компьютеров</returns>
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