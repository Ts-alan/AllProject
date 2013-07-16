using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ARM2_dbcontrol.DataBase;
using ARM2_dbcontrol.Filters;
using VirusBlokAda.Vba32CC.Policies;
using VirusBlokAda.Vba32CC.Policies.General;
using Filters.Primitive;

/// <summary>
/// Info about computer
/// </summary>
public partial class CompInfo : PageBase
{
    protected override void InitFields()
    {

        if (Request.QueryString["CompName"] != null)
        {
            string computerName = Request.QueryString["CompName"];

            Validation vld = new Validation(computerName);
            if (!vld.CheckStringValue())
                throw new ArgumentException(Resources.Resource.ErrorInvalidCompName);
            Page.Title = Resources.Resource.PageCompInfoTitle + ' ' + computerName;
            if ((!Roles.IsUserInRole("Administrator")) && (!Roles.IsUserInRole("Operator")))
            {
                lbtnTasksList.Visible = false;
            }
            WriteCompInfo(computerName);

            WriteComponentState(computerName);

        }
        else
        {
            throw new InvalidOperationException(Resources.Resource.ErrorAccessDenied);
        }
    }
    
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScript(@"js/jQuery/jquery.cookie.js");
        RegisterScript(@"js/DevicesPolicy.js");
        //Validation
        if (!Page.IsPostBack)
        {
            InitFields();
        }
    }

    /// <summary>
    /// Get info from database about computer
    /// </summary>
    /// <param name="computerName"></param>
    private void WriteCompInfo(string computerName)
    {
        ComputersEntity comp;
        using (VlslVConnection conn = new VlslVConnection(
            ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            ComputersManager db = new ComputersManager(conn);
            OSTypesManager os_db = new OSTypesManager(conn);

            conn.OpenConnection();
            conn.CheckConnectionState(true);
            //Get id
            Int16 id = db.GetComputerID(computerName);
            if (id == -1) throw new InvalidOperationException(Resources.Resource.ErrorCompNameNotExist);
            comp = db.Get(id);

            comp.OSName = os_db.GetOSName(comp.OSTypeID);

            conn.CloseConnection();
        }

        lblComputerName.Text = comp.ComputerName;

        bool isControlCenter = comp.ControlCenter || (Environment.MachineName == comp.ComputerName);
        if (isControlCenter)
            imgControlCenter.ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/enabled.gif";
        else
            imgControlCenter.ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/disabled.gif";


        PolicyProvider provider = PoliciesState;
        Policy policy;
        string defaultPolicyName = String.Empty;
        try
        {
            policy = provider.GetPolicyToComputer(comp.ComputerName);
            defaultPolicyName = provider.GetDefaultPolicyName();
        }
        catch
        {
            policy = new Policy();
        }

        lblPolicyName.Text = String.IsNullOrEmpty(policy.Name) ? "-" : policy.Name;
        if (!String.IsNullOrEmpty(defaultPolicyName) && defaultPolicyName == lblPolicyName.Text) lblPolicyName.Text += String.Format(" ({0})", Resources.Resource.DefaultPolicy);

        lblCPU.Text = comp.CPUClock == short.MinValue ? "-" : comp.CPUClock.ToString();
        tboxDescription.Text = comp.Description;
        lbtnSaveDescription.Text = Resources.Resource.Save;
        lblDomainName.Text = comp.DomainName == String.Empty ? "-" : comp.DomainName;
        lblIPAdress.Text = comp.IPAddress;
        DateTime dt = comp.LatestInfected;
        lblLatestInfected.Text = comp.LatestInfected == DateTime.MinValue ? "-" : dt.ToString();
        lblLatestMalware.Text = comp.LatestMalware == String.Empty ? "-" : comp.LatestMalware;

        dt = comp.LatestUpdate;
        lblLatestUpdate.Text = comp.LatestUpdate == DateTime.MinValue ? "-" : dt.ToString();

        lblOSType.Text = comp.OSName;

        lblRAM.Text = comp.RAM == short.MinValue ? "-" : comp.RAM.ToString();
        lblRecentActive.Text = comp.RecentActive.ToString();
        lblUserLogin.Text = comp.UserLogin == String.Empty ? "-" : comp.UserLogin;

        //if (!isControlCenter)
        //{
        if (comp.Vba32Integrity)
            imgVBA32Integrity.ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/enabled.gif";
        else
            imgVBA32Integrity.ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/disabled.gif";

        if (comp.Vba32KeyValid)
            imgKeyValid.ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/enabled.gif";
        else
            imgKeyValid.ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/disabled.gif";
        //}
        //else
        //{
        //    imgVBA32Integrity.Visible = false;
        //    imgKeyValid.Visible = false;
        //}


        lblVBA32Version.Text = comp.Vba32Version == String.Empty ? "-" : comp.Vba32Version;

        divDevices.Attributes.Add("dpc", comp.ID.ToString());
    }

    /// <summary>
    /// Redirect to Events list for this computer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnEventsList_Click(object sender, EventArgs e)
    {
        /*
        if(lblComputerName.Text==String.Empty)
            throw new ArgumentException(Resources.Resource.ErrorInvalidCompName);
      
        EventFilterEntity filter = new EventFilterEntity();
        filter.ComputerName = lblComputerName.Text;
        filter.GenerateSQLWhereStatement();

        Session["CurrentEventFilter"] = filter;
        */
        Response.Redirect("Events.aspx");
    }

    /// <summary>
    /// Redirect to Tasks list for this computer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnTasksList_Click(object sender, EventArgs e)
    {
        /*
        if (lblComputerName.Text == String.Empty)
            throw new ArgumentException(Resources.Resource.ErrorInvalidCompName);
        
        TaskFilterEntity filter = new TaskFilterEntity();
        filter.ComputerName = lblComputerName.Text;
        filter.GenerateSQLWhereStatement();

        Session["CurrentTaskFilter"] = filter;
        */
        Response.Redirect("Task.aspx");
    }
    protected void lbtnComponentsList_Click(object sender, EventArgs e)
    {
        /*
        if (lblComputerName.Text == String.Empty)
            throw new ArgumentException(Resources.Resource.ErrorInvalidCompName);
       
        CmptFilterEntity filter = new CmptFilterEntity();
        filter.ComputerName = lblComputerName.Text;
        filter.GenerateSQLWhereStatement();

        Session["CurrentCmptFilter"] = filter;
        */
        Response.Redirect("Components.aspx");
    }

    protected void WriteComponentState(string compName)
    {
        //TableRow row = new TableRow();
        //TableCell cell = new TableCell();
        //cell.Text = "cell1";
        //row.Cells.Add(cell);
        //tblComponentState.Rows.Add(row);
        List<ComponentsEntity> list;
        ComponentsProvider provider = new ComponentsProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        list = provider.List("ComponentID > -1 " + PrimitiveFilterHelper.GenerateSqlForTextValue(compName, "ComputerName", false, false), "ComponentName ASC", 1, Int16.MaxValue);

        foreach (ComponentsEntity cmpt in list)
        {
            TableRow row = new TableRow();

            TableCell cellName = new TableCell();
            cellName.Text = cmpt.ComponentName;
            row.Cells.Add(cellName);

            TableCell cellState = new TableCell();
            cellState.Text = DatabaseNameLocalization.GetNameForCurrentCulture(cmpt.ComponentState);
            cellState.Attributes.Add("width", "50%");
            cellState.Attributes.Add("align", "left");
            row.Cells.Add(cellState);

            tblComponentState.Rows.Add(row);
        }

    }

    /// <summary>
    /// Redirect to Processes list for this computer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnProcessesList_Click(object sender, EventArgs e)
    {
        /*
        if (lblComputerName.Text == String.Empty)
            throw new ArgumentException(Resources.Resource.ErrorInvalidCompName);

        ProcFilterEntity filter = new ProcFilterEntity();
        filter.ComputerName = lblComputerName.Text;
        filter.GenerateSQLWhereStatement();

        Session["CurrentProcFilter"] = filter;
        */
        Response.Redirect("Processes.aspx");
    }

    protected void lbtnComputersList_Click(object sender, EventArgs e)
    {
        if (lblComputerName.Text == String.Empty)
            throw new ArgumentException(Resources.Resource.ErrorInvalidCompName);

        CompFilterEntity filter = new CompFilterEntity();
        filter.ComputerName = lblComputerName.Text;
        filter.GenerateSQLWhereStatement();

        Session["CurrentCompFilter"] = filter;

        Response.Redirect("Computers.aspx");
    }
    protected void lbtnSaveDescription_Click(object sender, EventArgs e)
    {
        string computerName = lblComputerName.Text;
        string newDescription = tboxDescription.Text;

        Validation vld = new Validation(newDescription);
        if ((!vld.CheckStringToDescription()) && (newDescription != ""))
            throw new ArgumentException(Resources.Resource.ErrorDescription);


        using (VlslVConnection conn = new VlslVConnection(
                  ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            ComputersManager cmng = new ComputersManager(conn);
            conn.OpenConnection();



            short id = cmng.GetComputerID(computerName);
            if (id == 0)
                throw new InvalidOperationException(Resources.Resource.ErrorCompNameNotExist);
            cmng.UpdateDescription(id, newDescription);

            conn.CloseConnection();
        }

        //!-OPTM Как-нить иначе сделать это, что ли..
        Response.Redirect("CompInfo.aspx?CompName=" + computerName);

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
