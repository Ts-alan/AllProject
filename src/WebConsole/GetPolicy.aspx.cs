using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using VirusBlokAda.Vba32CC.Policies;

/// <summary>
/// This page return policy to computer
/// </summary>
public partial class GetPolicy : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string compName = Request.QueryString["CompName"];
            string hash = Request.QueryString["Hash"];

            string policyResponse = PoliciesState.GetResponse(compName,
                Request.UserHostAddress, hash);
            
            Response.Write(policyResponse);

        }
        catch (Exception ex)
        {
            //!OPTM - обернуть в тэги xml, понимаемые агентом
            Response.Write(ex.Message);
        }

        try
        {
            Response.Flush();
            Response.End();
        }
        catch { }

    }

    public PolicyProvider PoliciesState
    {
        get
        {
            PolicyProvider provider = Application["PoliciesState"] as PolicyProvider;
            if (provider == null)
            {
                provider = new PolicyProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                Application["PoliciesState"] = provider;
            }

            return provider;
        }
    }

}
