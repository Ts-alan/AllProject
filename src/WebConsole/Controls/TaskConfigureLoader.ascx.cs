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
using ARM2_dbcontrol.Tasks.ConfigureLoader;

/// <summary>
/// Задача настройки Диспетчера
/// </summary>
public partial class Controls_TaskConfigureLoader : System.Web.UI.UserControl,ITask
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();

        InitClientFields();
        ChangeEnabledControl();
    }

    private void ChangeEnabledControl()
    {
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

    private Boolean _hideHeader = false;

    public Boolean HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private Boolean _enabled = true;

    public Boolean Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    public void InitClientFields()
    {
        //Scan mode 
        if (!String.IsNullOrEmpty(hdnScanMemory.Value))
            cboxScanMemory.Checked = Boolean.Parse(hdnScanMemory.Value);
        Boolean mode = cboxScanMemory.Checked;
        if (!mode)
            rbMode.Attributes.Add("disabled", mode.ToString());
        else
            rbMode.Attributes.Remove("disabled");

        //scan boot
        if (!String.IsNullOrEmpty(hdnScanBoot.Value))
            cboxScanBoot.Checked = Boolean.Parse(hdnScanBoot.Value);

        mode = cboxScanBoot.Checked;
        if (!mode)
            cboxScanBootFloppy.Attributes.Add("disabled", mode.ToString());
        else
            cboxScanBootFloppy.Attributes.Remove("disabled");

        //log size
        if (!String.IsNullOrEmpty(hdnMaximumSizeLog.Value))
            cboxMaximumSizeLog.Checked = Boolean.Parse(hdnMaximumSizeLog.Value);

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


        //lbtnInitialization.Text = Resources.Resource.CongLdrInitialization;
        //lbtnReportFile.Text = Resources.Resource.CongLdrReportFile;
        //lbtnUpdate.Text = Resources.Resource.CongLdrUpdate;

        lblCongLdrUserName.Text = Resources.Resource.CongLdrUserName;
        lblCongLdrPassword.Text = Resources.Resource.CongLdrPassword;
        lblCongLdrAddress.Text = Resources.Resource.CongLdrAddress;
        lblCongLdrPort.Text = Resources.Resource.CongLdrPort;
    }

     /// <summary>
    /// Build xml with settings
    /// </summary>
    /// <returns></returns>
    private String BuildXml()
    {
        TaskConfigureLoader task = new TaskConfigureLoader();

        //cb
        task.AUTO_START= cboxLaunchLoaderAtStart.Checked ? 1 : 0;
        task.MONITOR_AUTO_START=cboxEnableMonitorAtStart.Checked ? 1 : 0;
        task.PROTECT_LOADER=cboxProtectProcess.Checked ? 1 : 0;
        task.SHOW_WINDOW=cboxDisplayLoadingProgress.Checked ? 1 : 0;
        task.AUTO_CHECK_MEMORY=cboxScanMemory.Checked ? 1 : 0;

        task.SCAN_USB = cboxScanUSB.Checked ? 1 : 0;

        if (cboxScanMemory.Checked)
            task.CHECK_MEMORY_MODE= rbMode.SelectedIndex;

        task.AUTO_CHECK_BOOT = cboxScanBoot.Checked ? 1 : 0;
        task.AUTO_CHECK_BOOT_FLOPPY = cboxScanBootFloppy.Checked ? 1 : 0;
        task.LOG_LIMIT = cboxMaximumSizeLog.Checked ? 1 : 0;
        task.SOUND = cboxSoundWarning.Checked ? 1 : 0;
        task.ANIMATION = cboxTrayIcon.Checked ? 1 : 0;
        task.UPDATE_TIME = cboxTimeIntervals.Checked ? 1 : 0;
        task.UPDATE_INTERACTIVE = cboxInteractive.Checked ? 1 : 0;
        task.PROXY_USAGE = cboxUseProxyServer.Checked ? 2 : 0;
        task.PROXY_AUTHORIZE = cboxUseAccount.Checked ? 1 : 0;

        task.LOG_NAME = tboxLogFile.Text;
        if (cboxMaximumSizeLog.Checked)
        {
            task.LOG_LIMIT_VALUE = Int32.Parse(tboxMaximumSizeLog.Text);
        }

        if (tboxTimeIntervals.Text != String.Empty)
        {
            task.UPDATE_TIME_VALUE = Int32.Parse(tboxTimeIntervals.Text);
        }

        if (lboxUpdateList.Items.Count > 0)
        {
            task.UPDATE_FOLDER= lboxUpdateList.Items[0].Text;
            if (lboxUpdateList.Items.Count > 1)
            {
                for (Int32 i = 1; i < lboxUpdateList.Items.Count; i++)
                {
                    task.UPDATE_FOLDER_LIST.Add(lboxUpdateList.Items[i].Text);
                }
            }
        }

        if (cboxUseProxyServer.Checked)
        {
            task.PROXY_ADDRESS = tboxAddress.Text;
            task.PROXY_PORT = Int32.Parse(tboxPort.Text);
        }

        if (cboxUseAccount.Checked)
        {
            task.PROXY_USER = tboxUserName.Text;
            task.PROXY_PASSWORD = tboxPassword.Text;
        }

        task.Vba32CCUser = Anchor.GetStringForTaskGivedUser();

        return task.SaveToXml();
    }

    public Boolean ValidateFields()
    {
        Validation vld = new Validation(tboxLogFile.Text);

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

        if (cboxTimeIntervals.Checked)
        {
            vld.Value = tboxTimeIntervals.Text;
            if (!vld.CheckPositiveInteger())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongLdrTimeIntervals);
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



        if ((cboxScanMemory.Checked) && (rbMode.SelectedIndex == -1))
            throw new ArgumentException("Scan mode is incorrect");

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

        GetClientState();
        ValidateFields();

        task.Param = BuildXml();

        return task;
    }

    private void GetClientState()
    {
        lboxUpdateList.Items.Clear();
        foreach (String str in hdnBlockItems.Value.Split(';'))
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
    }

    /// <summary>
    /// Load state
    /// </summary>
    /// <param name="task"></param>
    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ConfigureLoader)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        TaskConfigureLoader tsk = new TaskConfigureLoader();
        tsk.LoadFromXml(task.Param);

        cboxLaunchLoaderAtStart.Checked = tsk.AUTO_START == 1;
        cboxEnableMonitorAtStart.Checked = tsk.MONITOR_AUTO_START == 1;
        cboxProtectProcess.Checked = tsk.PROTECT_LOADER == 1;
        cboxDisplayLoadingProgress.Checked = tsk.SHOW_WINDOW == 1;
        cboxScanMemory.Checked = tsk.AUTO_CHECK_MEMORY == 1;
        cboxScanBoot.Checked = tsk.AUTO_CHECK_BOOT == 1;
        cboxScanBootFloppy.Checked = tsk.AUTO_CHECK_BOOT_FLOPPY == 1;
        cboxScanUSB.Checked = tsk.SCAN_USB == 1;
        cboxMaximumSizeLog.Checked = tsk.LOG_LIMIT == 1;
        cboxSoundWarning.Checked = tsk.SOUND == 1;
        cboxTrayIcon.Checked = tsk.ANIMATION == 1;
        cboxTimeIntervals.Checked = tsk.UPDATE_TIME == 1;
        cboxInteractive.Checked = tsk.UPDATE_INTERACTIVE == 1;
        cboxUseProxyServer.Checked = tsk.PROXY_USAGE == 2;
        cboxUseAccount.Checked = tsk.PROXY_AUTHORIZE == 1;

        if (rbMode.Items.Count == 0)
        {
            LoadSource();
        }
        rbMode.SelectedIndex = tsk.CHECK_MEMORY_MODE;
        
        tboxLogFile.Text = tsk.LOG_NAME;
        tboxMaximumSizeLog.Text = tsk.LOG_LIMIT_VALUE.ToString();
        tboxTimeIntervals.Text = tsk.UPDATE_TIME_VALUE.ToString();


        lboxUpdateList.Items.Clear();
        if (!String.IsNullOrEmpty(tsk.UPDATE_FOLDER))
            lboxUpdateList.Items.Add(tsk.UPDATE_FOLDER);

        foreach (String str in tsk.UPDATE_FOLDER_LIST)
            lboxUpdateList.Items.Add(str);

        hdnBlockItems.Value = "";
        foreach (ListItem item in lboxUpdateList.Items)
        {
            hdnBlockItems.Value += item.Text + ";";
        }

        tboxAddress.Text = tsk.PROXY_ADDRESS;
        tboxPort.Text = tsk.PROXY_PORT.ToString();
        tboxUserName.Text = tsk.PROXY_USER;

        tboxPassword.Text = tsk.PROXY_PASSWORD;
        tboxPassword.Attributes.Add("Value", tboxPassword.Text);
    }

    #region Update-click handlers

    protected void lbtnUpdateAdd_Click(object sender, EventArgs e)
    {
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
