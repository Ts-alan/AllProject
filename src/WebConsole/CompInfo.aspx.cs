using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using VirusBlokAda.CC.DataBase;
using ARM2_dbcontrol.Filters;
using VirusBlokAda.CC.Filters.Primitive;
using ARM2_dbcontrol.Tasks;

using VirusBlokAda.CC.Common.Xml;
using VirusBlokAda.CC.Tasks.Service;

/// <summary>
/// Info about computer
/// </summary>
public partial class CompInfo : PageBase
{
    protected override void InitFields()
    {
        if (Request.QueryString["CompName"] != null)
        {
            String computerName = Request.QueryString["CompName"];

            Validation vld = new Validation(computerName);
            if (!vld.CheckStringValue())
                throw new ArgumentException(Resources.Resource.ErrorInvalidCompName);

            Page.Title = Resources.Resource.PageCompInfoTitle + ' ' + computerName;
            
            //if ((!Roles.IsUserInRole("Administrator")) && (!Roles.IsUserInRole("Operator")))
            //{
            //    lbtnTasksList.Visible = false;
            //}

            WriteCompInfo(computerName);
            InitComponents();
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
    private void WriteCompInfo(String computerName)
    {
        ComputersEntity comp;
        
        //Get id
        Int16 id = DBProviders.Computer.GetComputerID(computerName);
        if (id == -1)
            throw new InvalidOperationException(Resources.Resource.ErrorCompNameNotExist);
        comp = DBProviders.Computer.GetComputer(id);

        lblComputerName.Text = comp.ComputerName;

        Boolean isControlCenter = comp.ControlCenter || (Environment.MachineName == comp.ComputerName);
        if (isControlCenter)
            imgControlCenter.ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/enabled.gif";
        else
            imgControlCenter.ImageUrl = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/disabled.gif";


        Policy policy;
        String defaultPolicyName = String.Empty;
        try
        {
            policy = DBProviders.Policy.GetPolicyToComputer(comp.ComputerName);
            defaultPolicyName = DBProviders.Policy.GetDefaultPolicyName();
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

    private void InitComponents()
    {
        ComponentsEntity cmpt = new ComponentsEntity();
        cmpt.ComponentState = "Not installed";
    }

    protected void WriteComponentState(String compName)
    {
        List<ComponentsEntity> list;
        list = DBProviders.Component.List("ComponentID > -1 " + PrimitiveFilterHelper.GenerateSqlForTextValue(compName, "ComputerName", false, false), "ComponentName ASC", 1, Int16.MaxValue);
                
        foreach (ComponentsEntity cmpt in list)
        {
            AddComponent(cmpt);
        }
    }

    private void AddComponent(ComponentsEntity cmpt)
    {
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(VirusBlokAda.CC.Common.RegularExpressions.GUID);
        if (reg.IsMatch(DatabaseNameLocalization.GetNameForCurrentCulture(cmpt.ComponentName)))
            return;
        String title;
        HtmlGenericControl ctrlH3 = new HtmlGenericControl("h3");
        HtmlGenericControl ctrlA = new HtmlGenericControl("a");
        HtmlGenericControl ctrlSpan = new HtmlGenericControl("span");
        ctrlSpan.InnerText = DatabaseNameLocalization.GetNameForCurrentCulture(cmpt.ComponentName);
        HtmlImage ctrlImg = new HtmlImage();
        ctrlImg.Src = GetImageSrcByState(cmpt.ComponentState, out title);
        ctrlImg.Attributes.Add("title", title);
        ctrlImg.Attributes.Add("style", "margin-right: 10px;");
        ctrlA.Controls.Add(ctrlImg);
        ctrlA.Controls.Add(ctrlSpan);
        ctrlH3.Controls.Add(ctrlA);
        
        divComponents.Controls.Add(ctrlH3);
        divComponents.Controls.Add(new HtmlGenericControl("div"));        
    }

    private void SetComponentAttributes(UserControl ctrl, HtmlImage img, HtmlGenericControl lbl, ComponentsEntity cmpt, TaskType taskType,Button button)
    {
        String title;
        img.Src = GetImageSrcByState(cmpt.ComponentState, out title);
        img.Attributes.Add("title", title);
        if (cmpt.ComponentState == "On" || cmpt.ComponentState == "Off")
        {
            Int16 compID;
            Int16.TryParse(img.Attributes["compid"], out compID);
            if (compID < 1)
                SetErrorMessage(lbl, String.Format(Resources.Resource.ErrorIncorrectValue, "ComputerID"));
            else
            {
                ITask control = ctrl as ITask;
                if (control == null)
                    SetErrorMessage(lbl, String.Format(Resources.Resource.ErrorTaskTypeConflict, cmpt.ComponentName));
                else
                {
                    if (LoadComponentSettings(control, compID, cmpt.ComponentName, taskType, lbl,button))
                        ctrl.Visible = true;
                }
            }
        }
    }

    private Boolean LoadComponentSettings(ITask ctrl, Int16 compID, String ComponentName, TaskType taskType, HtmlGenericControl lbl,Button button)
    {
        //Try load state
        TaskUserEntity task = new TaskUserEntity();
        task.Type = taskType;
        try
        {
            task.Param = DBProviders.Component.GetCurrentSettings(compID, ComponentName);
        }
        catch (Exception e)
        {
            SetErrorMessage(lbl, String.Format(Resources.Resource.ErrorGetSettings, taskType.ToString(), e.Message));
            return false;
        }

        if (String.IsNullOrEmpty(task.Param))
        {
            SetErrorMessage(lbl, Resources.Resource.ErrorEmptySettings);
            return false;
        }
     
        try
        {
            ctrl.LoadState(task);
            button.Visible = true;
            
        }
        catch (Exception e)
        {
            SetErrorMessage(lbl, String.Format(Resources.Resource.ErrorLoad, taskType.ToString(), e.Message));
            return false;
        }

        return true;
    }

    private void SetErrorMessage(HtmlGenericControl lbl, String error)
    {
        if (!String.IsNullOrEmpty(error))
        {
            lbl.InnerText = error;
            lbl.Visible = true;
        }
    }

    private String GetImageSrcByState(String state, out String title)
    {
        title = DatabaseNameLocalization.GetNameForCurrentCulture(state);
        switch (state)
        {
            case "On":
                return "App_Themes/" + Profile.Theme + "/Images/SwitchOn.png";
            case "Off":
                return "App_Themes/" + Profile.Theme + "/Images/SwitchOff.png";
            case "Not installed":
                return "App_Themes/" + Profile.Theme + "/Images/disabled.gif";
        }

        return String.Empty;
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
        String computerName = lblComputerName.Text;
        String newDescription = tboxDescription.Text;

        Validation vld = new Validation(newDescription);
        if ((!vld.CheckStringToDescription()) && (newDescription != ""))
            throw new ArgumentException(Resources.Resource.ErrorDescription);

        Int16 id = DBProviders.Computer.GetComputerID(computerName);
        if (id == 0)
            throw new InvalidOperationException(Resources.Resource.ErrorCompNameNotExist);
        DBProviders.Computer.UpdateDescription(id, newDescription);

        //!-OPTM Как-нить иначе сделать это, что ли..
        Response.Redirect("CompInfo.aspx?CompName=" + computerName);
    }
}
