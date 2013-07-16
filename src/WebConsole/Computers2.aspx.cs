using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Filters.Common;
using VirusBlokAda.Vba32CC.Groups;
using VirusBlokAda.Vba32CC.JSON.Entities;
using VirusBlokAda.Vba32CC.JSON;
using System.Configuration;
using ARM2_dbcontrol.DataBase;
using VirusBlokAda.Vba32CC.Policies;

/// <summary>
/// Computers page
/// </summary>
public partial class Computers2 : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = Resources.Resource.PageComputersTitle;
        if (!IsPostBack)
        {
            InitFields();
        }
    }
    
    protected override void InitFields()
    {
        List<DDLPair> list = new List<DDLPair>();
        list.Add(new DDLPair(Resources.Resource.Yes, "1"));
        list.Add(new DDLPair(Resources.Resource.No, "0"));
        
        fltControlCenter.DataSource = list;
        fltControlCenter.DataBind();
        fltVBA32Integrity.DataSource = list;
        fltVBA32Integrity.DataBind();
        fltVBA32KeyValid.DataSource = list;
        fltVBA32KeyValid.DataBind();

        //get default policy name
        PolicyProvider provider = PoliciesState;
        try
        {
            hdnDefaultPolicyName.Value = provider.GetDefaultPolicyName();
        }
        catch { }

        //policy list
        list.Clear();
        foreach (String policyName in provider.GetAllPolicyTypesNames())
        {
            list.Add(new DDLPair(policyName, policyName)); 
        }
        fltPolicy.DataSource = list;
        fltPolicy.DataBind();
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

    protected void FilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "treeLoaderScript", "Ext.onReady(function(){ $get('" + btnReload.ClientID + "').onclick(\"" + e.Where + "\"); });", true);
    }
}

/// <summary>
/// Class DropDownListPair
/// </summary>
public class DDLPair
{
    private String text;
    private String value;

    public String Text
    {
        get { return this.text; }
        set { this.text = value; }
    }
    public String Value
    {
        get { return this.value; }
        set { this.value = value; }
    }

    public DDLPair()
    { }
    public DDLPair(String text, String value)
    {
        this.text = text;
        this.value = value;
    }
}