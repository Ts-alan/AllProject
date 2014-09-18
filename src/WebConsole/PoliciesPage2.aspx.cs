using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Text;
using System.Web.Services;
using System.Collections.Generic;

using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Filters;
using VirusBlokAda.CC.DataBase;
using ARM2_dbcontrol.Generation;
using Microsoft.Win32;
using VirusBlokAda.CC.Common.Xml;
using VirusBlokAda.CC.Settings.Common;

public partial class _PoliciesPage : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole("Administrator"))
        {
            Response.Redirect("Default.aspx");
        }

        RegisterLink("~/App_Themes/" + Profile.Theme + @"/Groups/Groups.css");
        

        Page.Title = Resources.Resource.PolicySettings;

        string mode = Request.QueryString["Mode"];
        if (!String.IsNullOrEmpty(mode) && (mode == "Create" || mode == "Edit" || mode == "SaveAs"))
        {
            ChangeEnable(true);
        }
        else
        {
            ChangeEnable(false);
        }

        if (!Page.IsPostBack)
            InitFields();
        
        InitDefaultPolicy();
    }

    private void ChangeEnable(Boolean isEnable)
    {
        cblUsedTasks.Enabled = isEnable;
        loader.Enabled = isEnable;
        monitor.Enabled = isEnable;
        quarantine.Enabled = isEnable;
        JornalEvents.Enabled = isEnable;
        cboxRunMonitor.Enabled = isEnable;
    }

    protected override void InitFields()
    {
        lbtnDelete.Attributes.Add("onclick", "return confirm('" + Resources.Resource.AreYouSurePolicy + "');");        
        
        TaskUserEntity task = new TaskUserEntity();
        task.Param = "";

        task.Type = TaskType.ConfigureLoader;
        loader.InitFields();
        loader.LoadState(task);

        task.Type = TaskType.ConfigureMonitor;
        monitor.InitFields();
        monitor.LoadState(task);

        task.Type = TaskType.ConfigureQuarantine;
        quarantine.InitFields();
        quarantine.LoadState(task);

        task.Type = TaskType.JornalEvents;
        JornalEvents.InitFields();

        //get starting params
        string name = Request.QueryString["Name"];
        string mode = Request.QueryString["Mode"];
        if ((!String.IsNullOrEmpty(name)) && (!String.IsNullOrEmpty(mode)) && (mode == "Edit"))
        {
            InitEditPolicy(name);
            tboxPolicyName.Text = name;
            tboxPolicyName.Enabled = false;
            lbtnCancelEditing.Visible = true;
            trTBOX.Visible = true;
            tblPolicies.Visible = false;
            divButtons.Visible = true;
            lbtnCancelEditing.Text = Resources.Resource.CancelButtonText;
        }
        else
            if (!String.IsNullOrEmpty(mode) && mode == "Create")
            {
                trTBOX.Visible = true;
                tblPolicies.Visible = false;
                divButtons.Visible = true;
                lbtnCancelEditing.Visible = true;
                lbtnCancelEditing.Text = Resources.Resource.CancelButtonText;
            }
            else
                if (!String.IsNullOrEmpty(mode) && mode == "SaveAs")
                {
                    InitEditPolicy(name);
                    tboxPolicyName.Text = "";
                    tboxPolicyName.Enabled = true;
                    lbtnCancelEditing.Visible = true;
                    trTBOX.Visible = true;
                    tblPolicies.Visible = false;
                    divButtons.Visible = true;
                    lbtnCancelEditing.Text = Resources.Resource.CancelButtonText;
                }
                else
                {
                    trTBOX.Visible = false;

                    List<string> policyList;
                    try
                    {
                        policyList = DBProviders.Policy.GetAllPolicyTypesNames();
                    }
                    catch
                    {
                        Response.Redirect("ErrorSql.aspx");
                        return;
                    }
                    policyList.Sort();

                    ddlPolicyNames.DataSource = policyList;
                    ddlPolicyNames.DataBind();

                    if (!String.IsNullOrEmpty(name))
                    {
                        ddlPolicyNames.SelectedValue = name;
                    }

                    if (ddlPolicyNames.Items.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(name))
                        {
                            InitEditPolicy(name);
                        }
                        else InitEditPolicy(ddlPolicyNames.SelectedItem.Text);
                    }
                    else
                    {
                        lbtnDelete.Visible = false;
                        lbtnEdit.Visible = false;
                        lbtnSaveAs.Visible = false;
                    }
                    
                }

        lbtnSave.Text = Resources.Resource.Save;

        cblUsedTasks.Items[0].Text = Resources.Resource.SetLoaderSettings;
        cblUsedTasks.Items[1].Text = Resources.Resource.SetMonitorSettings;
        cblUsedTasks.Items[2].Text = Resources.Resource.SetQtnSettings;
        cblUsedTasks.Items[3].Text = Resources.Resource.UseStartupOptionsLoaderAndMonitor;
        cblUsedTasks.Items[4].Text = Resources.Resource.JournalEvents;
    }   
    
    #region Policies creation
    
    private void InitEditPolicy(string policyName)
    {
        //Get policy that should be parsed
        Policy policy = DBProviders.Policy.GetPolicyByName(policyName);
        PolicyParser parser = new PolicyParser(policy.Content);

        //init loader state
        TaskUserEntity loaderTask = new TaskUserEntity();
        loaderTask.Param = VirusBlokAda.CC.Common.Anchor.FromBase64String(parser.GetParam(TaskType.ConfigureLoader.ToString()));
        loaderTask.Type = TaskType.ConfigureLoader;
        loader.InitFields();
        if (!String.IsNullOrEmpty(loaderTask.Param))
        {
            loader.LoadState(loaderTask);
            cblUsedTasks.Items[0].Selected = true;
        }
        else cblUsedTasks.Items[0].Selected = false;

        //init monitor state
        TaskUserEntity monitorTask = new TaskUserEntity();
        monitorTask.Param = VirusBlokAda.CC.Common.Anchor.FromBase64String(parser.GetParam(TaskType.ConfigureMonitor.ToString()));
        monitorTask.Type = TaskType.ConfigureMonitor;
        monitor.InitFields();
        if (!String.IsNullOrEmpty(monitorTask.Param))
        {
            monitor.LoadState(monitorTask);
            cblUsedTasks.Items[1].Selected = true;
        }
        else cblUsedTasks.Items[1].Selected = false;

        //init qtn state
        TaskUserEntity qtnTask = new TaskUserEntity();
        qtnTask.Param = VirusBlokAda.CC.Common.Anchor.FromBase64String(parser.GetParam(TaskType.ConfigureQuarantine.ToString()));
        qtnTask.Type = TaskType.ConfigureQuarantine;
        quarantine.InitFields();
        if (!String.IsNullOrEmpty(qtnTask.Param))
        {
            quarantine.LoadState(qtnTask);
            cblUsedTasks.Items[2].Selected = true;
        }
        else cblUsedTasks.Items[2].Selected = false;

        String tmp = VirusBlokAda.CC.Common.Anchor.FromBase64String(parser.GetParam(TaskType.MonitorOn.ToString()));
        if (!String.IsNullOrEmpty(tmp))
        {

            TaskMonitorOnOff task = new TaskMonitorOnOff();
            task.LoadFromXml(tmp);
            cboxRunMonitor.Checked = task.IsMonitorOn;
            cblUsedTasks.Items[3].Selected = true;            
        }
        else
        {
            cboxRunMonitor.Checked = false;
            cblUsedTasks.Items[3].Selected = false;
        }

        //init journal events state
        TaskUserEntity journalEventTask = new TaskUserEntity();
        journalEventTask.Param = VirusBlokAda.CC.Common.Anchor.FromBase64String(parser.GetParam(TaskType.JornalEvents.ToString()));
        journalEventTask.Type = TaskType.JornalEvents;

        JornalEvents.InitFields();
        if (!String.IsNullOrEmpty(journalEventTask.Param))
        {
            JornalEvents.LoadState(journalEventTask);
            cblUsedTasks.Items[4].Selected = true;
        }
        else cblUsedTasks.Items[4].Selected = false;
    }

    private String GetPolicyString()
    {
        VirusBlokAda.CC.Common.Xml.XmlBuilder xml = new VirusBlokAda.CC.Common.Xml.XmlBuilder();
        StringBuilder sb = new StringBuilder(1024);

        if (cblUsedTasks.Items[0].Selected)
            sb.AppendFormat(@"<{0}>{1}</{0}>", TaskType.ConfigureLoader.ToString(), VirusBlokAda.CC.Common.Anchor.ToBase64String(loader.GetCurrentState().Param));
        if (cblUsedTasks.Items[1].Selected)
            sb.AppendFormat(@"<{0}>{1}</{0}>", TaskType.ConfigureMonitor.ToString(), VirusBlokAda.CC.Common.Anchor.ToBase64String(monitor.GetCurrentState().Param));
        if (cblUsedTasks.Items[2].Selected)
            sb.AppendFormat(@"<{0}>{1}</{0}>", TaskType.ConfigureQuarantine.ToString(), VirusBlokAda.CC.Common.Anchor.ToBase64String(quarantine.GetCurrentState().Param));
        if (cblUsedTasks.Items[3].Selected)
        {
            TaskMonitorOnOff task = new TaskMonitorOnOff();
            task.IsMonitorOn = cboxRunMonitor.Checked;
            sb.AppendFormat(@"<{0}>{1}</{0}>", TaskType.MonitorOn.ToString(), VirusBlokAda.CC.Common.Anchor.ToBase64String(task.SaveToXml()));
        }
        if (cblUsedTasks.Items[4].Selected)
            sb.AppendFormat(@"<{0}>{1}</{0}>", TaskType.JornalEvents.ToString(), VirusBlokAda.CC.Common.Anchor.ToBase64String(JornalEvents.GetCurrentState().Param));

        return sb.ToString();
    }

    protected void lbtnSave_Click(object sender, EventArgs e)
    {
        if (!ValidatePolicy()) return;
        Validation val = new Validation(tboxPolicyName.Text.Replace(" ", ""));
        if (!val.CheckStringValue()) return;
        
        string content = GetPolicyString();
        Policy policy = new Policy(tboxPolicyName.Text,content,"");

        if (Request.QueryString["Mode"] == "Edit")
        {
            policy.ID = DBProviders.Policy.GetPolicyByName(tboxPolicyName.Text).ID;
            DBProviders.Policy.EditPolicy(policy);
        }
        else
            DBProviders.Policy.AddPolicy(policy);

        Response.Redirect("PoliciesPage.aspx?name=" + tboxPolicyName.Text);
    }

    protected void lbtnCancelEditing_Click(object sender, EventArgs e)
    {
        Response.Redirect("PoliciesPage.aspx?name=" + tboxPolicyName.Text);
    }

    /// <summary>
    /// validate task to policy
    /// </summary>
    private bool ValidatePolicy()
    {
        int j = 0;
        for (int i = 0; i < cblUsedTasks.Items.Count; i++)
        {
            if (cblUsedTasks.Items[i].Selected)
                continue;
            j++;
        }
        try
        {
            if (j == cblUsedTasks.Items.Count)
                throw new Exception(Resources.Resource.PolicyEmpty);

            if (cblUsedTasks.Items[0].Selected)
                loader.ValidateFields();

            if (cblUsedTasks.Items[1].Selected)
                monitor.ValidateFields();

            if (cblUsedTasks.Items[2].Selected)
                quarantine.ValidateFields();

            if (String.IsNullOrEmpty(tboxPolicyName.Text))
                throw new ArgumentException(Resources.Resource.ErrorPolicyName);

            string mode = Request.QueryString["Mode"];
            if (mode != "Edit")
            {
                if (DBProviders.Policy.GetPolicyByName(tboxPolicyName.Text).ID != 0)
                    throw new ArgumentException(Resources.Resource.ErrorPolicyExistInCollection);
            }
        }
        catch (ArgumentException argEx)
        {
            lblMessage.Text = argEx.Message;
            mpPicture.Attributes["class"] = "ModalPopupPictureError";
            ModalPopupExtender.Show();
            return false;
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            mpPicture.Attributes["class"] = "ModalPopupPictureError";
            ModalPopupExtender.Show();
            return false;
        }

        return true;
    }
    #endregion

    private void EditPolicy(string policyName)
    {
        Response.Redirect("PoliciesPage.aspx?Mode=Edit&Name=" + policyName.Replace("&", "%26").Replace("#", "%23"));
    }

    private void SaveAsPolicy(string policyName)
    {
        Response.Redirect("PoliciesPage.aspx?Mode=SaveAs&Name=" + policyName.Replace("&", "%26").Replace("#", "%23"));
    }

    protected void imbtnIsDefaultPolicy_Click(object sender, ImageClickEventArgs e)
    {
        bool retVal = true;

        string currentDefaultPolicy = VirusBlokAda.CC.Settings.SettingsProvider.GetDefaultPolicy();

        string newDefaultPolicyName =
            currentDefaultPolicy == ddlPolicyNames.SelectedItem.Text ? "" : ddlPolicyNames.SelectedItem.Text;

        try
        {
            IVba32Settings remoteObject = (IVba32Settings)Activator.GetObject(
                       typeof(IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

            String xml = String.Format("<VbaSettings><ControlCenter>" +
                "<DefaultPolicy type=" + "\"reg_sz\"" + ">{0}</DefaultPolicy>" +
                "</ControlCenter></VbaSettings>", newDefaultPolicyName);

            retVal = remoteObject.ChangeRegistry(xml);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("DefaultPolicy: " +
                ex.Message + " " + Resources.Resource.Vba32SSUnavailable);
        }

        if (!retVal)
            throw new ArgumentException("Reread: Vba32SS return false!");

        DBProviders.Policy.ClearCache();

        InitDefaultPolicy();
    }

    private void InitDefaultPolicy()
    {
        string url = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/";
        string defaultPolicy = VirusBlokAda.CC.Settings.SettingsProvider.GetDefaultPolicy();
        if (ddlPolicyNames.SelectedItem != null)
        {
            if ((String.IsNullOrEmpty(defaultPolicy)) || (defaultPolicy != ddlPolicyNames.SelectedItem.Text))
                imbtnIsDefaultPolicy.ImageUrl = url + "disabled.gif";
            else
                imbtnIsDefaultPolicy.ImageUrl = url + "enabled.gif";
        }
        else
        {
            imbtnIsDefaultPolicy.Visible = false;
        }
    }

    protected void ddlPolicyNames_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitEditPolicy(ddlPolicyNames.SelectedItem.Text);
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        EditPolicy(ddlPolicyNames.SelectedItem.Text);
    }

    protected void lbtnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("PoliciesPage.aspx?Mode=Create");
    }

    protected void lbtnSaveAs_Click(object sender, EventArgs e)
    {
        SaveAsPolicy(ddlPolicyNames.SelectedItem.Text);
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string selectedPolicyName = "";
        if (ddlPolicyNames.SelectedItem != null)
            selectedPolicyName = ddlPolicyNames.SelectedItem.Text;
        if ((selectedPolicyName == "(undefined)") || (String.IsNullOrEmpty(selectedPolicyName)))
            return;

        DBProviders.Policy.RemovePolicy(selectedPolicyName);

        Response.Redirect("PoliciesPage.aspx");
    }
} 