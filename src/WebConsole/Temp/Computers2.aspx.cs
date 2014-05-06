using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirusBlokAda.CC.Filters.Common;
using VirusBlokAda.CC.JSON;
using System.Configuration;
using VirusBlokAda.CC.DataBase;
using System.Web.Services;
using VirusBlokAda.CC.Tasks.Common;
using ARM2_dbcontrol.Service.TaskAssignment;
using System.Threading;

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
        try
        {
            hdnDefaultPolicyName.Value = DBProviders.Policy.GetDefaultPolicyName();
        }
        catch { }

        //policy list
        list.Clear();
        foreach (String policyName in DBProviders.Policy.GetAllPolicyTypesNames())
        {
            list.Add(new DDLPair(policyName, policyName));
        }
        fltPolicy.DataSource = list;
        fltPolicy.DataBind();
    }

    protected void FilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "treeLoaderScript", "Ext.onReady(function(){ $get('" + btnReload.ClientID + "').onclick(\"" + e.Where + "\"); });", true);
    }

    #region WebMethods
    [WebMethod]
    public static string GetAdditionalInfo(String id)
    {
        Int16 compID = Convert.ToInt16(id);
        ComputersEntityEx comp = new ComputersEntityEx();
        CompAdditionalInfo info;

        comp = DBProviders.Computer.GetComputerEx(compID);
        comp = new ComputersEntityEx(comp, comp.Group, comp.Policy, DBProviders.Component.GetComponentsPageByComputerID(comp.ID));

        info = new CompAdditionalInfo(comp);
        return Newtonsoft.Json.JsonConvert.SerializeObject(info);
    }
    #endregion

    #region Tasks
    protected void CompositeTaskPanel_TaskAssign(object sender, TaskEventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "treeGetCompsInfo", "Ext.onReady(function(){ $get('" + btnGetCompsInfo.ClientID + "').onclick(\""+e.AssignToAll+"\"); });", true);
        SelectedComputersForTask selectedComps = GetSelectedCompsForTasks(e.AssignToAll);


        Int64[] taskId = new Int64[selectedComps.Names.Count];

        String userName = Anchor.GetStringForTaskGivedUser();
        String service = ConfigurationManager.AppSettings["Service"];
        String connStr = ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString;

        Vba32ControlCenterWrapper control = new Vba32ControlCenterWrapper(service);

        for (int i = 0; i < selectedComps.Names.Count; i++)
        {
            taskId[i] = PreServAction.CreateTask(selectedComps.Names[i], e.TaskName, e.Xml, userName, connStr);
        }
        control.PacketCustomAction(taskId, selectedComps.IpAddresses.ToArray(), e.TaskXml);
    }

    private SelectedComputersForTask GetSelectedCompsForTasks(bool all)
    {
        SelectedComputersForTask selected;
        if (!all)
        {

            selected.Names = new List<string>(hdnSelectedCompsNames.Value.Split('&'));
            selected.IpAddresses = new List<string>(hdnSelectedCompsIP.Value.Split('&'));
        }
        else
        {
            string where = FilterContainer1.GenerateSQL();
            selected = DBProviders.Computer.GetSelectionComputerForTask(where);
        }
        return selected;
    }
    #endregion
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