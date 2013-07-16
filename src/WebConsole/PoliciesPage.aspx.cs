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
using ARM2_dbcontrol.DataBase;
using ARM2_dbcontrol.Generation;

using VirusBlokAda.Vba32CC.Policies;
using VirusBlokAda.Vba32CC.Policies.General;
using VirusBlokAda.Vba32CC.Policies.Xml;
using Microsoft.Win32;

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

        //RegisterLink("~/App_Themes/" + Profile.Theme + @"/Groups/css/ext-all.css");
        //RegisterLink("~/App_Themes/" + Profile.Theme + @"/Groups/Groups.css");
        

        Page.Title = Resources.Resource.PolicySettings;

        string mode = Request.QueryString["Mode"];
        if (!String.IsNullOrEmpty(mode) && (mode == "Create" || mode == "Edit" || mode == "SaveAs"))
        {
            cblUsedTasks.Enabled = true;
            loader.Enabled = true;
            monitor.Enabled = true;
            quarantine.Enabled = true;
            deviceProtect.Enabled = true;
            cboxRunLoader.Enabled = cboxRunMonitor.Enabled = true;
        }
        else
        {
            cblUsedTasks.Enabled = false;
            loader.Enabled = false;
            monitor.Enabled = false;
            quarantine.Enabled = false;
            deviceProtect.Enabled = false;
            cboxRunLoader.Enabled = cboxRunMonitor.Enabled = false;
        }

        if (!Page.IsPostBack)
            InitFields();
        
        InitDefaultPolicy();
    }


    protected override void InitFields()
    {
        lbtnDelete.Attributes.Add("onclick", "return confirm('" + Resources.Resource.AreYouSurePolicy + "');");        
        btnClose.Text = Resources.Resource.Close;        
        //get starting params
        string name = Request.QueryString["Name"];
        string mode = Request.QueryString["Mode"];
        if ((!String.IsNullOrEmpty(name)) && (!String.IsNullOrEmpty(mode)) && (mode == "Edit"))
        {
            InitEditPolicy(name);
            tboxPolicyName.Text = name;
            tboxPolicyName.Enabled = false;
            divCancelEditing.Visible = true;
            trTBOX.Visible = true;
            tblPolicies.Visible = false;
            divButtons.Visible = true;
            lbtnCancelEditing.Text = Resources.Resource.CancelButtonText;
                        
            if (!cboxRunLoader.Checked) cboxRunMonitor.InputAttributes.Add("disabled", "true");
        }
        else
            if (!String.IsNullOrEmpty(mode) && mode == "Create")
            {
                trTBOX.Visible = true;
                tblPolicies.Visible = false;
                divButtons.Visible = true;
                divCancelEditing.Visible = true;
                lbtnCancelEditing.Text = Resources.Resource.CancelButtonText;
                                
                if (!cboxRunLoader.Checked) cboxRunMonitor.InputAttributes.Add("disabled", "true");
            }
            else
                if (!String.IsNullOrEmpty(mode) && mode == "SaveAs")
                {
                    InitEditPolicy(name);
                    tboxPolicyName.Text = "";
                    tboxPolicyName.Enabled = true;
                    divCancelEditing.Visible = true;
                    trTBOX.Visible = true;
                    tblPolicies.Visible = false;
                    divButtons.Visible = true;
                    lbtnCancelEditing.Text = Resources.Resource.CancelButtonText;

                    if (!cboxRunLoader.Checked) cboxRunMonitor.InputAttributes.Add("disabled", "true");
                }
                else
                {
                    trTBOX.Visible = false;

                    List<string> policyList;
                    try
                    {
                        policyList = PoliciesState.GetAllPolicyTypesNames();
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
                        divDelete.Visible = false;
                        divEdit.Visible = false;
                        divSaveAs.Visible = false;
                    }
                    
                }

        lbtnSave.Text = Resources.Resource.Save;

        cblUsedTasks.Items[0].Text = Resources.Resource.SetLoaderSettings;
        cblUsedTasks.Items[1].Text = Resources.Resource.SetMonitorSettings;
        cblUsedTasks.Items[2].Text = Resources.Resource.SetQtnSettings;
        cblUsedTasks.Items[3].Text = Resources.Resource.UseStartupOptionsLoaderAndMonitor;
    }   
    
    #region Policies creation
    
    private void InitEditPolicy(string policyName)
    {
        //Get policy that should be parsed
        Policy policy;
        try
        {
            policy = PoliciesState.GetPolicyByName(policyName);
        }
        catch
        {
            Response.Redirect("ErrorSql.aspx");
            return;
        }

        PolicyParser parser = new PolicyParser(policy);

        //init loader state
        TaskUserEntity loaderTask = new TaskUserEntity();
        loaderTask.Param = parser.GetParamToLoader();
        loaderTask.Type = TaskType.ConfigureLoader;

        loader.InitFields();
        if (!String.IsNullOrEmpty(loaderTask.Param))
        {
            //we are using this setting
            loader.LoadState(loaderTask);
            cblUsedTasks.Items[0].Selected = true;
        }
        else cblUsedTasks.Items[0].Selected = false;

        //init monitor state
        TaskUserEntity monitorTask = new TaskUserEntity();
        monitorTask.Param = parser.GetParamToMonitor();
        monitorTask.Type = TaskType.ConfigureMonitor;

        monitor.InitFields();
        if (!String.IsNullOrEmpty(monitorTask.Param))
        {
            //we are using this setting
            monitor.LoadState(monitorTask);
            cblUsedTasks.Items[1].Selected = true;
        }
        else cblUsedTasks.Items[1].Selected = false;

        //init qtn state
        TaskUserEntity qtnTask = new TaskUserEntity();
        qtnTask.Param = parser.GetParamToQtn();
        qtnTask.Type = TaskType.ConfigureQuarantine;

        quarantine.InitFields();
        if (!String.IsNullOrEmpty(qtnTask.Param))
        {
            //we are using this setting
            quarantine.LoadState(qtnTask);
            cblUsedTasks.Items[2].Selected = true;
        }
        else
        {
            cblUsedTasks.Items[2].Selected = false;
            TaskUserEntity taskQuarantine = new TaskUserEntity();
            taskQuarantine.Type = TaskType.ConfigureQuarantine;

            ARM2_dbcontrol.Generation.XmlBuilder xmlBuil = new ARM2_dbcontrol.Generation.XmlBuilder("root");
            xmlBuil.Generate();
            taskQuarantine.Param = xmlBuil.Result;

            quarantine.LoadState(taskQuarantine);
        }

        List<string> list = parser.GetTaskCreateProcessContentNodes();
        if (list.Count > 0)
            cblUsedTasks.Items[3].Selected = true;
        else
        {
            cboxRunLoader.Checked = false;
            cboxRunMonitor.Checked = false;
            cblUsedTasks.Items[3].Selected = false;
        }
        foreach (string str in list)
        {

            if (str == GetEncodedString(Resources.Resource.TaskParamVba32LoaderDisable))
                cboxRunLoader.Checked = false;

            if (str == GetEncodedString(Resources.Resource.TaskParamVba32LoaderEnable))
                cboxRunLoader.Checked = true;

            if (str == GetEncodedString(Resources.Resource.TaskParamVba32MonitorDisable))
                cboxRunMonitor.Checked = false;

            if (str == GetEncodedString(Resources.Resource.TaskParamVba32MonitorEnable))
                cboxRunMonitor.Checked = true;

        }

        //init Device Protect state
        TaskUserEntity protectTask = new TaskUserEntity();
        protectTask.Param = parser.GetParamToDeviceProtect();
        protectTask.Type = TaskType.DailyDeviceProtect;

        deviceProtect.InitFields();
        if (!String.IsNullOrEmpty(protectTask.Param))
        {
            //we are using this setting
            deviceProtect.LoadState(protectTask);
            cblUsedTasks.Items[3].Selected = true;
        }
    }

    private string GetPolicyString()
    {
        TaskUserEntity taskLoader = loader.GetCurrentState();
        TaskUserEntity taskMonitor = monitor.GetCurrentState();
        TaskUserEntity taskQtn = quarantine.GetCurrentState();
        String taskProtect = deviceProtect.BuildTask();

        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder();


        StringBuilder sb = new StringBuilder(1024);

        if(cblUsedTasks.Items[0].Selected)
            sb.AppendFormat(@"<Task><Content><TaskConfigureSettings>{0}</TaskConfigureSettings></Content></Task>", taskLoader.Param.Replace(xml.Top, ""));
        if (cblUsedTasks.Items[1].Selected)
            sb.AppendFormat(@"<Task><Content><TaskConfigureSettings>{0}</TaskConfigureSettings></Content></Task>", taskMonitor.Param.Replace(xml.Top, ""));
        if (cblUsedTasks.Items[2].Selected)
            sb.AppendFormat(@"<Task><Content><TaskConfigureSettings>{0}</TaskConfigureSettings></Content></Task>", taskQtn.Param.Replace(xml.Top, ""));        
            

        //Run status of Monitor and Loader
        if (cblUsedTasks.Items[3].Selected)
        {
            //Device Protect
            sb.AppendFormat(@"<Task><Content><TaskCustomAction><Options>{0}</Options></TaskCustomAction></Content></Task>", taskProtect);
            //Loader
            string runLoader;
            string typeLoader;
            if (cboxRunLoader.Checked)
            {
                runLoader = GetEncodedString(Resources.Resource.TaskParamVba32LoaderEnable);
                typeLoader = "LoaderLoad";
            }
            else
            {
                runLoader = GetEncodedString(Resources.Resource.TaskParamVba32LoaderDisable);
                typeLoader = "LoaderUnload";
            }


            sb.AppendFormat(@"<Task><Content><TaskCustomAction><Options><NamedCreateProcess><Name>{0}</Name><TaskCreateProcess><CommandLine>{1}</CommandLine></TaskCreateProcess></NamedCreateProcess></Options></TaskCustomAction></Content></Task>",
            typeLoader, runLoader);

            //sb.AppendFormat(@"<Task><Content><TaskCreateProcess><CommandLine>{0}</CommandLine></TaskCreateProcess></Content></Task>",
            //runLoader);

            //Monitor
            if (cboxRunLoader.Checked)
            {
                string runMonitor;
                string typeMonitor;
                if (cboxRunMonitor.Checked)
                {
                    runMonitor = GetEncodedString(Resources.Resource.TaskParamVba32MonitorEnable);
                    typeMonitor = "MonitorEnable";
                }
                else
                {
                    runMonitor = GetEncodedString(Resources.Resource.TaskParamVba32MonitorDisable);
                    typeMonitor = "MonitorDisable";
                }

                sb.AppendFormat(@"<Task><Content><TaskCustomAction><Options><NamedCreateProcess><Name>{0}</Name><TaskCreateProcess><CommandLine>{1}</CommandLine></TaskCreateProcess></NamedCreateProcess></Options></TaskCustomAction></Content></Task>",
           typeMonitor, runMonitor);


               // sb.AppendFormat(@"<Task><Content><TaskCreateProcess><CommandLine>{0}</CommandLine></TaskCreateProcess></Content></Task>",
               //     runMonitor);
            }
        }
        return sb.ToString();
    }

    private string GetEncodedString(string str)
    {
        str = str.Replace("&#160;"," ");
        return Server.HtmlDecode(Server.HtmlEncode(str));
    }

   
    protected void lbtnSave_Click(object sender, EventArgs e)
    {
        if (!ValidatePolicy()) return;
        Validation val = new Validation(tboxPolicyName.Text.Replace(" ", ""));
        if (!val.CheckStringValue()) return;

        PolicyProvider provider = PoliciesState;

        string content = GetPolicyString();
        Policy policy = new Policy(tboxPolicyName.Text,content,"");

        if (Request.QueryString["Mode"] == "Edit")
        {
            policy.ID = PoliciesState.GetPolicyByName(tboxPolicyName.Text).ID;
            provider.EditPolicy(policy);
        }
        else
            provider.AddPolicy(policy);

        /*
        lblMessage.Text = Resources.Resource.PolicySaved;
        mpPicture.Attributes["class"] = "ModalPopupPictureSuccess";
        CorrectPositionModalPopup();
        ModalPopupExtender.Show();
        */
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
                if (PoliciesState.GetPolicyByName(tboxPolicyName.Text).ID != 0)
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

    public static PolicyProvider PoliciesState
    {
        get
        {
            PolicyProvider provider = HttpContext.Current.Application["PoliciesState"] as PolicyProvider;
            if (provider == null)
            {
                provider = new PolicyProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Application["PoliciesState"] = provider;
            }

            return provider;
        }
    }

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

        string currentDefaultPolicy = GetDefaultPolicyNameFromRegistry();

        string newDefaultPolicyName =
            currentDefaultPolicy == ddlPolicyNames.SelectedItem.Text ? "" : ddlPolicyNames.SelectedItem.Text;


        try
        {
            Vba32.ControlCenter.SettingsService.IVba32Settings remoteObject = (Vba32.ControlCenter.SettingsService.IVba32Settings)Activator.GetObject(
                       typeof(Vba32.ControlCenter.SettingsService.IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

            string xml = String.Format("<VbaSettings><ControlCenter>" +
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

        PoliciesState.ClearCache();

        InitDefaultPolicy();
    }

    private string GetDefaultPolicyNameFromRegistry()
    {
        string registryControlCenterKeyName;
        RegistryKey key;
        try
        {
            //!-OPTM Вынести такую проверку в App_Code и юзать один код
            if (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8)
                registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
            else
                registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

            key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName); ;

        }
        catch
        {
            //throw new ArgumentException("Registry open 'ControlCenter' key error: " + ex.Message);
            return String.Empty;
        }

        return (string)key.GetValue("DefaultPolicy");

    }

    private void InitDefaultPolicy()
    {
        string url = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/";
        string defaultPolicy = GetDefaultPolicyNameFromRegistry();
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

        PolicyProvider provider = PoliciesState;
        provider.RemovePolicy(selectedPolicyName);

        Response.Redirect("PoliciesPage.aspx");
    }
} 