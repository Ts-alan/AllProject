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

using System.Text;
using System.Collections.Generic;

using ARM2_dbcontrol.Generation;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Filters;

/// <summary>
/// Задача настройки Диспетчера
/// </summary>
public partial class Controls_TaskConfigureLoader : System.Web.UI.UserControl,ITask
{
    private string passPrefix = "HER!%&$";


    //CheckBox cboxScanMemory = new CheckBox();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();

        InitClientFields();

        ChangeEnabledControl();
    }

    private void ChangeEnabledControl()
    {
        Tabs.Enabled = _enabled;
        cboxScanMemory.Disabled = !_enabled;
        cboxScanBoot.Disabled = !_enabled;
        cboxScanBootFloppy.Disabled = !_enabled;
        cboxMaximumSizeLog.Disabled = !_enabled;

        if (!cboxScanMemory.Checked)
        {
            rbMode.Enabled = true;
            rbMode.Attributes.Add("disabled", "true");
        }
        if (!cboxScanBoot.Checked)
        {
            cboxScanBootFloppy.Disabled = true;
        }
    }

    private bool _hideHeader = false;

    public bool HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private bool _enabled = true;

    public bool Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    public void InitClientFields()
    {
        //Scan mode 
        if (!String.IsNullOrEmpty(hdnScanMemory.Value))
            cboxScanMemory.Checked =
            Boolean.Parse(hdnScanMemory.Value);
        bool mode = cboxScanMemory.Checked;
        if (!mode)
            rbMode.Attributes.Add("disabled", mode.ToString());
        else
            rbMode.Attributes.Remove("disabled");

        //scan boot
        if (!String.IsNullOrEmpty(hdnScanBoot.Value))
            cboxScanBoot.Checked =
            Boolean.Parse(hdnScanBoot.Value);

        mode = cboxScanBoot.Checked;
        if (!mode)
            cboxScanBootFloppy.Attributes.Add("disabled", mode.ToString());
        else
            cboxScanBootFloppy.Attributes.Remove("disabled");


        //log size
        if (!String.IsNullOrEmpty(hdnMaximumSizeLog.Value))
            cboxMaximumSizeLog.Checked =
            Boolean.Parse(hdnMaximumSizeLog.Value);

        mode = cboxMaximumSizeLog.Checked;
        if (!mode)
            tboxMaximumSizeLog.Attributes.Add("disabled", mode.ToString());
        else
            tboxMaximumSizeLog.Attributes.Remove("disabled");

        GetClientState();
    }


    public void InitFields()
    {
        if (rbMode.Items.Count == 0)
            LoadSource();

        if (HideHeader) HeaderName.Visible = false;

        tabPanel1.HeaderText = Resources.Resource.CongLdrInitialization;
        tabPanel2.HeaderText = Resources.Resource.CongLdrReportFile;
        tabPanel3.HeaderText = Resources.Resource.CongLdrUpdate;

        //lbtnInitialization.Text = Resources.Resource.CongLdrInitialization;
        //lbtnReportFile.Text = Resources.Resource.CongLdrReportFile;
        //lbtnUpdate.Text = Resources.Resource.CongLdrUpdate;

        lblCongLdrUserName.Text = Resources.Resource.CongLdrUserName;
        lblCongLdrPassword.Text = Resources.Resource.CongLdrPassword;
        lblCongLdrAddress.Text = Resources.Resource.CongLdrAddress;
        lblCongLdrPort.Text = Resources.Resource.CongLdrPort;

        tboxLogFile.Text = "Vba32Ldr.log";

    }

     /// <summary>
    /// Build xml with settings
    /// </summary>
    /// <returns></returns>
    private string BuildXml()
    {
        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("loader");

        //cb
        xml.AddNode("AUTO_START", cboxLaunchLoaderAtStart.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("MONITOR_AUTO_START", cboxEnableMonitorAtStart.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("PROTECT_LOADER", cboxProtectProcess.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("SHOW_WINDOW", cboxDisplayLoadingProgress.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("AUTO_CHECK_MEMORY", cboxScanMemory.Checked ? "reg_dword:1" : "reg_dword:0");

        if (cboxScanMemory.Checked)
            xml.AddNode("CHECK_MEMORY_MODE", "reg_dword:" + Convert.ToString(rbMode.SelectedIndex));
        
        xml.AddNode("AUTO_CHECK_BOOT", cboxScanBoot.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("AUTO_CHECK_BOOT_FLOPPY", cboxScanBootFloppy.Checked ? "reg_dword:1" : "reg_dword:0");
        //xml.AddNode("AUTO_CHECK_AUTORUN", cboxScanStartupFiles.Checked ? "reg_dword:1" : "reg_dword:0");
        //xml.AddNode("LOG", cboxKeep.Checked ? "reg_dword:1" : "reg_dword:0");
        //xml.AddNode("LOG_ADD", cboxAddLog.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("LOG_LIMIT", cboxMaximumSizeLog.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("SOUND", cboxSoundWarning.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("ANIMATION", cboxTrayIcon.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("UPDATE_TIME", cboxTimeIntervals.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("UPDATE_INTERACTIVE", cboxInteractive.Checked ? "reg_dword:1" : "reg_dword:0");
        xml.AddNode("PROXY_USAGE", cboxUseProxyServer.Checked ? "reg_dword:2" : "reg_dword:0");
        xml.AddNode("PROXY_AUTHORIZE", cboxUseAccount.Checked ? "reg_dword:1" : "reg_dword:0");

        //xml.AddNode("LANGUAGE", "reg_sz:" + ddlLanguage.SelectedValue);

        //tb
        //if (cboxKeep.Checked)
        //{
            xml.AddNode("LOG_NAME", "reg_sz:"+tboxLogFile.Text);
            if (cboxMaximumSizeLog.Checked)
            {
                xml.AddNode("LOG_LIMIT_VALUE", "reg_dword:"+tboxMaximumSizeLog.Text);
            }  
       // }

        if (tboxTimeIntervals.Text != String.Empty)
        {
            xml.AddNode("UPDATE_TIME_VALUE", "reg_dword:" + tboxTimeIntervals.Text);
        }


        //!--
        //if ((tboxPath.Text != String.Empty)&&(lboxUpdateList.Items.Count<1))
        //{
        //    xml.AddNode("UPDATE_FOLDER", "reg_sz:" + tboxPath.Text);
        //}
        //else
        //{
            if (lboxUpdateList.Items.Count > 0)
            {
                xml.AddNode("UPDATE_FOLDER", "reg_sz:" + lboxUpdateList.Items[0].Text);
                if (lboxUpdateList.Items.Count > 1)
                {
                    string str = "";
                    for (int i = 1; i < lboxUpdateList.Items.Count; i++)
                    {
                        str += lboxUpdateList.Items[i].Text + '?';
                    }
                    str += '?';
                    xml.AddNode("UPDATE_FOLDER_LIST", "reg_multi_sz:" + str);
                }
            }
       // }

        if (cboxUseProxyServer.Checked)
        {
            xml.AddNode("PROXY_ADDRESS", "reg_sz:" + tboxAddress.Text);
            xml.AddNode("PROXY_PORT", "reg_dword:" + tboxPort.Text);
        }

        if (cboxUseAccount.Checked)
        {

            xml.AddNode("PROXY_USER", "reg_sz:" + tboxUserName.Text);

            if (!tboxPassword.Text.Contains(passPrefix))
            {
                //Шифруем пароль
                byte[] bytes = Encoding.Unicode.GetBytes(tboxPassword.Text);

                byte xorValue = 0xAA;
                byte delta = 0x1;

                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] ^= xorValue;
                    delta = Convert.ToByte(delta % 3 + 1);
                    xorValue += delta;
                }
                xml.AddNode("PROXY_PASSWORD", "reg_binary:" + Anchor.ConvertToDumpString(bytes));
            }
            else
            {
                string str = tboxPassword.Text.Replace(passPrefix, "");
                xml.AddNode("PROXY_PASSWORD", str);
            }
        }

        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "ConfigureLoader");

        xml.Generate();

        return xml.Result;

    }

    public bool ValidateFields()
    {
        Validation vld = new Validation(tboxLogFile.Text);
        //if (cboxKeep.Checked)
        //{
            if (!vld.CheckFileName())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.Name);

            if (cboxMaximumSizeLog.Checked)
            {
                vld.Value = tboxMaximumSizeLog.Text;
                if (!vld.CheckSize())
                    throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.CongLdrMaximumSizeLog);
            }
        //}

        if (cboxTimeIntervals.Checked)
        {
            vld.Value = tboxTimeIntervals.Text;
            if (!vld.CheckPositiveInteger())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongLdrTimeIntervals);
            /*vld.Value = tboxPath.Text;
            if (!vld.CheckPath())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.CongLdrPath);*/

        }

        if (cboxUseProxyServer.Checked)
        {
            vld.Value = tboxAddress.Text;
            if (!vld.CheckPath())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.CongLdrAddress);

            vld.Value = tboxPort.Text;
            if (!vld.CheckSize())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.CongLdrPort);
        }

        if (cboxUseAccount.Checked)
        {
            vld.Value = tboxUserName.Text;
            if (!vld.CheckStringToFilter())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.CongLdrUserName);
        }



        if((cboxScanMemory.Checked)&& (rbMode.SelectedIndex == -1))
            throw new ArgumentException("Scan mode is incorrect");

        //vld.Value = tboxPassword.Text;
        // if (!vld.CheckStringToTask())
        //    throw new ArgumentException("Password is incorrect");
        

        return true;

    }

    /// <summary>
    /// Get current state
    /// </summary>
    /// <returns></returns>
    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();

        task.Type = TaskType.ConfigureLoader;

        ARM2_dbcontrol.Generation.XmlBuilder xml =
            new ARM2_dbcontrol.Generation.XmlBuilder("task");

        GetClientState();

        ValidateFields();

        task.Param = BuildXml();

        return task;
    }

    private void GetClientState()
    {
        lboxUpdateList.Items.Clear();
        foreach (string str in hdnBlockItems.Value.Split(';'))
        {
            if (str != String.Empty && str != null)
                lboxUpdateList.Items.Add(str);
        }

        if (!String.IsNullOrEmpty(hdnScanMemory.Value))
            cboxScanMemory.Checked = Boolean.Parse(hdnScanMemory.Value);

    }

   

    private void LoadSource()
    {
        rbMode.Items.Add(Resources.Resource.CongLdrScanMemoryModeFast);
        rbMode.Items.Add(Resources.Resource.CongLdrScanMemoryModeFull);
        rbMode.Items.Add(Resources.Resource.CongLdrScanMemoryModeExcessive);

        if(rbMode.SelectedIndex==-1)
            rbMode.SelectedIndex = 0;


        //ddlLanguage.Items.Add("English");
        //ddlLanguage.Items.Add("Русский");
    }

    /// <summary>
    /// Load state
    /// </summary>
    /// <param name="task"></param>
    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ConfigureLoader)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        XmlTaskParser pars = new XmlTaskParser(task.Param);

        cboxLaunchLoaderAtStart.Checked = pars.GetValue("AUTO_START", "reg_dword:") == "1" ? true : false;
        cboxEnableMonitorAtStart.Checked = pars.GetValue("MONITOR_AUTO_START", "reg_dword:") == "1" ? true : false;
        cboxProtectProcess.Checked = pars.GetValue("PROTECT_LOADER", "reg_dword:") == "1" ? true : false;
        cboxDisplayLoadingProgress.Checked = pars.GetValue("SHOW_WINDOW", "reg_dword:") == "1" ? true : false;
        cboxScanMemory.Checked = pars.GetValue("AUTO_CHECK_MEMORY", "reg_dword:") == "1" ? true : false;
        cboxScanBoot.Checked = pars.GetValue("AUTO_CHECK_BOOT", "reg_dword:") == "1" ? true : false;
        cboxScanBootFloppy.Checked = pars.GetValue("AUTO_CHECK_BOOT_FLOPPY", "reg_dword:") == "1" ? true : false;
        //cboxScanStartupFiles.Checked = pars.GetValue("AUTO_CHECK_AUTORUN", "reg_dword:") == "1" ? true : false;
        //cboxKeep.Checked = pars.GetValue("LOG", "reg_dword:") == "1" ? true : false;
        //cboxAddLog.Checked = pars.GetValue("LOG_ADD", "reg_dword:") == "1" ? true : false;
        cboxMaximumSizeLog.Checked = pars.GetValue("LOG_LIMIT", "reg_dword:") == "1" ? true : false;
        cboxSoundWarning.Checked = pars.GetValue("SOUND", "reg_dword:") == "1" ? true : false;
        cboxTrayIcon.Checked = pars.GetValue("ANIMATION", "reg_dword:") == "1" ? true : false;
        cboxTimeIntervals.Checked = pars.GetValue("UPDATE_TIME", "reg_dword:") == "1" ? true : false;
        cboxInteractive.Checked = pars.GetValue("UPDATE_INTERACTIVE", "reg_dword:") == "1" ? true : false;
        cboxUseProxyServer.Checked = pars.GetValue("PROXY_USAGE", "reg_dword:") == "2" ? true : false;
        cboxUseAccount.Checked = pars.GetValue("PROXY_AUTHORIZE", "reg_dword:") == "1" ? true : false;

        if (rbMode.Items.Count == 0)
        {
            LoadSource();
        }
        if (pars.GetValue("CHECK_MEMORY_MODE", "reg_dword:") != String.Empty)
            rbMode.SelectedIndex = Convert.ToInt32(pars.GetValue("CHECK_MEMORY_MODE", "reg_dword:"));
        else
            rbMode.SelectedIndex = 0;

        //if (pars.GetValue("LANGUAGE", "reg_sz:") != String.Empty)
        //    ddlLanguage.SelectedValue = pars.GetValue("LANGUAGE","reg_sz:");
        //else
        //    ddlLanguage.SelectedIndex = 0;

        string logName = pars.GetValue("LOG_NAME", "reg_sz:");
        
        if(!String.IsNullOrEmpty(logName))
            tboxLogFile.Text = logName;
        
        tboxMaximumSizeLog.Text = pars.GetValue("LOG_LIMIT_VALUE", "reg_dword:");
        tboxTimeIntervals.Text = pars.GetValue("UPDATE_TIME_VALUE", "reg_dword:");


        lboxUpdateList.Items.Clear();
        string primarySource = pars.GetValue("UPDATE_FOLDER", "reg_sz:");
        if (!String.IsNullOrEmpty(primarySource))
            lboxUpdateList.Items.Add(primarySource);
        


        string updateSourceList = pars.GetValue("UPDATE_FOLDER_LIST", "reg_multi_sz:");
        if (!String.IsNullOrEmpty(updateSourceList))
        {
            string[] splitted = updateSourceList.Split('?');
            foreach(string str in splitted)
                if(!String.IsNullOrEmpty(str))
                    lboxUpdateList.Items.Add(str);
        }

        hdnBlockItems.Value = "";
        foreach (ListItem item in lboxUpdateList.Items)
        {
            hdnBlockItems.Value += item.Text + ";";
        }
 
        
        tboxAddress.Text = pars.GetValue("PROXY_ADDRESS", "reg_sz:");
        tboxPort.Text = pars.GetValue("PROXY_PORT", "reg_dword:");
        tboxUserName.Text = pars.GetValue("PROXY_USER", "reg_sz:");
        
        //tboxPassword.Text = pars.GetValue("PROXY_PASSWORD", "reg_binary:");
        tboxPassword.Text = pars.GetValue("PROXY_PASSWORD", "reg_binary:");
        tboxPassword.Attributes.Add("Value", passPrefix + tboxPassword.Text);

    }

   /* protected void lbtnInitialization_Click(object sender, EventArgs e)
    {
        tblInitialization1.Visible = true;
        tblLoaderStartup.Visible = true;

        tblAccessToUpdate.Visible = false;
        
        tblReportFile1.Visible = false;
        tblReportFile2.Visible = false;
        tblUpdate.Visible = false;
        //Anchor.ScrollToObj(lbtnInitialization.ClientID, Page);
    }
   
    protected void lbtnReportFile_Click(object sender, EventArgs e)
    {
        tblReportFile1.Visible = true;
        tblReportFile2.Visible = true;

        tblAccessToUpdate.Visible = false;
        tblInitialization1.Visible = false;
        tblLoaderStartup.Visible = false;
        tblUpdate.Visible = false;
        //Anchor.ScrollToObj(lbtnReportFile.ClientID, Page);

    }
    protected void lbtnUpdate_Click(object sender, EventArgs e)
    {
        tblUpdate.Visible = true;
        tblAccessToUpdate.Visible = true;

        tblInitialization1.Visible = false;
        tblReportFile1.Visible = false;
        tblReportFile2.Visible = false;
        tblLoaderStartup.Visible = false;
        //Anchor.ScrollToObj(lbtnUpdate.ClientID, Page);
    }*/

    #region Update-click handlers

    protected void lbtnUpdateAdd_Click(object sender, EventArgs e)
    {

        /*if ((tboxPath.Text != String.Empty) &&
            (lboxUpdateList.Items.FindByText(tboxPath.Text) == null))
            lboxUpdateList.Items.Add(tboxPath.Text);
        tboxPath.Text = "";*/
    }
    protected void lbtnUpdateRemove_Click(object sender, EventArgs e)
    {
        if (lboxUpdateList.SelectedIndex != -1)
            lboxUpdateList.Items.RemoveAt(lboxUpdateList.SelectedIndex);
    }
    protected void lbtnUpdateUp_Click(object sender, EventArgs e)
    {
        int index = lboxUpdateList.SelectedIndex;

        if((index == 0) ||(index == -1))
            return;
        
        string previous = lboxUpdateList.Items[index - 1].Text;
        lboxUpdateList.Items[index - 1].Text = lboxUpdateList.Items[index].Text;
        lboxUpdateList.Items[index].Text = previous;

        if (index > 0) lboxUpdateList.SelectedIndex--;
      
    }
    protected void lbtnUpdateDown_Click(object sender, EventArgs e)
    {
        int index = lboxUpdateList.SelectedIndex;

        if ((index == lboxUpdateList.Items.Count-1) || (index == -1))
            return;

        string previous = lboxUpdateList.Items[index + 1].Text;
        lboxUpdateList.Items[index + 1].Text = lboxUpdateList.Items[index].Text;
        lboxUpdateList.Items[index].Text = previous;

        if (index < lboxUpdateList.Items.Count - 1) lboxUpdateList.SelectedIndex++;
      
    }
    #endregion
}
