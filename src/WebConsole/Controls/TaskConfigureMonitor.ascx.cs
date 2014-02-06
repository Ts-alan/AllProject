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

using ARM2_dbcontrol.Generation;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Filters;
using System.Collections.Generic;

/// <summary>
/// Задача настройки Монитора
/// </summary>
public partial class Controls_TaskConfigureMonitor : System.Web.UI.UserControl,ITask
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    private String defaultFilters = ARM2_dbcontrol.Tasks.ConfigureMonitor.TaskConfigureMonitor.DefaultFilters;

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


    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
        LoadSource();
    }

    public Boolean ValidateFields()
    {
        Validation vld = new Validation(tboxLogFile.Text);
        if (!vld.CheckFileName())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongLdrKeep);

        if (cboxMaximumSizeLog.Checked)
        {
            vld.Value = tboxMaximumSizeLog.Text;
            if (!vld.CheckSize())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.CongLdrMaximumSizeLog);
        }

        if (cboxMaximumCPUUsege.Checked)
        {
            vld.Value = tboxMaximumCPUUsege.Text;
            if (!vld.CheckPercent())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongMonitorMaximumCPUUsege);
        }

        if (cboxMaximumDiskActivity.Checked)
        {
            vld.Value = tboxMaximumDiskActivity.Text;
            if (!vld.CheckPercent())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongMonitorMaximumDiskActivity);
        }

        if (cboxMaximumDisplacement.Checked)
        {
            vld.Value = tboxMaximumDisplacement.Text;
            if (!vld.CheckSize())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
               + Resources.Resource.CongMonitorMaximumDisplacement);
        }

        if (cboxMinimumBattery.Checked)
        {
            vld.Value = tboxMinimumBattery.Text;
            if (!vld.CheckPercent())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongMonitorMinimumBattery);
        }

        return true;
    }

    private void LoadSource()
    {
        if (ddlInfected.Items.Count != 0) return;
        ddlInfected.Items.Add(Resources.Resource.Block);
        ddlInfected.Items.Add(Resources.Resource.Cure);
        ddlInfected.Items.Add(Resources.Resource.Delete);
        ddlInfected.Items.Add(Resources.Resource.Ask);

        ddlPrevInfected.Items.Add(Resources.Resource.Cure);
        ddlPrevInfected.Items.Add(Resources.Resource.Delete);
        ddlPrevInfected.Items.Add(Resources.Resource.Ask);

        ddlPrevPrevInfected.Items.Add(Resources.Resource.Delete);
        ddlPrevPrevInfected.Items.Add(Resources.Resource.Ask);

        ddlSuspicious.Items.Add(Resources.Resource.Skip);
        ddlSuspicious.Items.Add(Resources.Resource.Block);
        ddlSuspicious.Items.Add(Resources.Resource.Delete);

        ddlPrePrevSuspicious.Items.Add(Resources.Resource.Skip);
        ddlPrePrevSuspicious.Items.Add(Resources.Resource.Delete);

        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Disabled);
        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Optimal);
        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Maximum);
        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Excessive);
    }

    /// <summary>
    /// Build xml
    /// </summary>
    /// <returns></returns>
    private String BuildXml()
    {
        ARM2_dbcontrol.Tasks.ConfigureMonitor.TaskConfigureMonitor task = new ARM2_dbcontrol.Tasks.ConfigureMonitor.TaskConfigureMonitor();
        //Набор файлов
        if (rbScanStandartSet.Checked)
            task.CHECK_MODE = 0;
        if (rbScanSelectedTypes.Checked)
        {
            task.CHECK_MODE = 1;

            String filters = tboxFilterDefined.Text;
            //Если пользователь не задал фильтры, используем набор по умолчанию
            if (String.IsNullOrEmpty(filters))
                filters = defaultFilters;

            task.FILTER_DEFINED = filters;
        }
        if (rbMonitorScanAllTypes.Checked)
        {
            task.CHECK_MODE = 2;
            task.FILTER_EXCLUDE = tboxFilterExclude.Text;
        }

        task.FAST_MODE = cboxScanOnlyNew.Checked ? 1 : 0;
        task.DETECT_RISKWARE = cboxDetectWare.Checked ? 1 : 0;

        //фоновая проверка
        task.IDLE_CHECK = cboxScanInBackGround.Checked ? 1 : 0;
        if (cboxScanInBackGround.Checked)
        {
            task.IsScanInBackGround = true;

            task.IDLE_PROCESSOR = cboxMaximumCPUUsege.Checked ? 1 : 0;
            task.IDLE_DISK = cboxMaximumDiskActivity.Checked ? 1 : 0;
            task.IDLE_MOUSE = cboxMaximumDisplacement.Checked ? 1 : 0;
            task.IDLE_NOTEBOOK = cboxMinimumBattery.Checked ? 1 : 0;
            task.IDLE_USERFILES = cboxScanFilesByUser.Checked ? 1 : 0;
            task.IDLE_AUTORUN = cboxScanStartupFiles.Checked ? 1 : 0;

            if (cboxMaximumCPUUsege.Checked)
                task.IDLE_PROCESSOR_VALUE = Convert.ToInt32(tboxMaximumCPUUsege.Text);
            if (cboxMaximumDiskActivity.Checked)
                task.IDLE_DISK_VALUE = Convert.ToInt32(tboxMaximumDiskActivity.Text);
            if (cboxMaximumDisplacement.Checked)
                task.IDLE_MOUSE_VALUE = Convert.ToInt32(tboxMaximumDisplacement.Text);
            if (cboxMinimumBattery.Checked)
                task.IDLE_NOTEBOOK_VALUE = Convert.ToInt32(tboxMinimumBattery.Text);
        }

        task.NOTIFY = cboxNotifyOfMonitor.Checked ? 1 : 0;

        //лог-файл
        task.REPORT_NAME = tboxLogFile.Text;

        task.LIMIT_REPORT = cboxMaximumSizeLog.Checked ? 1 : 0;
        if (cboxMaximumSizeLog.Checked)
        {
            task.LIMIT_REPORT_VALUE = Convert.ToInt32(tboxMaximumSizeLog.Text);
        }

        task.SHOW_OK = cboxInformationAboutCleanFiles.Checked ? 1 : 0;

        // Действия над инфицированными
        task.InfectedAction1 = ddlInfected.SelectedIndex + 1;
        Int32 index;
        if (ddlInfected.SelectedIndex < 2 && ddlPrevInfected.SelectedIndex == 0)
        {
            if (ddlInfected.SelectedIndex == 0) index = 2;
            else index = 1;
        }
        else index = ddlPrevInfected.SelectedIndex + 2;

        task.InfectedAction2 = index;
        task.InfectedAction3 = ddlPrevPrevInfected.SelectedIndex + 3;

        task.HEURISTIC = ddlHeuristicAnalysis.SelectedIndex;

        if (ddlHeuristicAnalysis.SelectedIndex > 0)
        {
            task.BlockAutorunInf = cboxBlockUSB.Checked ? 1 : 0;
        }

        //Действия над подозрительными
        task.SuspiciousAction1 = ddlSuspicious.SelectedIndex == 2 ? 3 : ddlSuspicious.SelectedIndex;
        task.SuspiciousAction2 = ddlPrePrevSuspicious.SelectedIndex == 0 ? 0 : 3;

        //пути фоновой проверки и список необрабатываемых путей        
        if (ddlExcludingFoldersAndFilesDelete.Items.Count != 0)
        {
            for (Int32 i = 0; i < ddlExcludingFoldersAndFilesDelete.Items.Count; i++)
                task.ExcludingFoldersAndFilesDelete.Add(ddlExcludingFoldersAndFilesDelete.Items[i].Text);
        }

        if (ddlListOfPathToScan.Items.Count != 0)
        {
            for (Int32 i = 0; i < ddlListOfPathToScan.Items.Count; i++)
                task.ListOfPathToScan.Add(ddlListOfPathToScan.Items[i].Text);
        }

        //cbox to dll
        task.InfectedCopy1 = cboxInfectedQuarantine.Checked ? 1 : 0;
        task.InfectedCopy2 = cboxInfectedQua.Checked ? 1 : 0;
        task.InfectedCopy3 = cboxInfectedQuaPrev.Checked ? 1 : 0;
        task.SuspiciousCopy1 = cboxSuspiciousQua.Checked ? 1 : 0;
        task.SuspiciousCopy2 = cboxSuspiciousQuaPrev.Checked ? 1 : 0;

        task.Vba32CCUser = Anchor.GetStringForTaskGivedUser();

        return task.SaveToXml();
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ConfigureMonitor)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        ARM2_dbcontrol.Tasks.ConfigureMonitor.TaskConfigureMonitor tsk = new ARM2_dbcontrol.Tasks.ConfigureMonitor.TaskConfigureMonitor();
        tsk.LoadFromXml(task.Param);

        //cbox
        //cboxAddLog.Checked = pars.GetValue("ADD_TO_REPORT","reg_dword:") == "1" ? true : false;
        cboxScanOnlyNew.Checked = tsk.FAST_MODE == 1;
        cboxDetectWare.Checked = tsk.DETECT_RISKWARE == 1;

        cboxScanInBackGround.Checked = tsk.IDLE_CHECK == 1;
        cboxScanInBackGround.Attributes.Add("onclick", "ChangeScanInBackGround();");
        cboxMaximumCPUUsege.Checked = tsk.IDLE_PROCESSOR == 1;
        cboxMaximumCPUUsege.Attributes.Add("onclick", "ChangeMaximumCPUUsege();");
        cboxMaximumDiskActivity.Checked = tsk.IDLE_DISK == 1;
        cboxMaximumDiskActivity.Attributes.Add("onclick", "ChangeMaximumDiskActivity();");
        cboxMaximumDisplacement.Checked = tsk.IDLE_MOUSE == 1;
        cboxMaximumDisplacement.Attributes.Add("onclick", "ChangeMaximumDisplacement();");
        cboxMinimumBattery.Checked = tsk.IDLE_NOTEBOOK == 1;
        cboxMinimumBattery.Attributes.Add("onclick", "ChangeMinimumBattery();");

        cboxScanFilesByUser.Checked = tsk.IDLE_USERFILES == 1;
        cboxNotifyOfMonitor.Checked = tsk.NOTIFY == 1;
        cboxMaximumSizeLog.Checked = tsk.LIMIT_REPORT == 1;
        cboxInformationAboutCleanFiles.Checked = tsk.SHOW_OK == 1;
        cboxInfectedQuarantine.Checked = tsk.InfectedCopy1 == 1;
        cboxInfectedQua.Checked = tsk.InfectedCopy2 == 1;
        cboxInfectedQuaPrev.Checked = tsk.InfectedCopy3 == 1;
        cboxSuspiciousQua.Checked = tsk.SuspiciousCopy1 == 1;
        cboxSuspiciousQuaPrev.Checked = tsk.SuspiciousCopy2 == 1;
        cboxBlockUSB.Checked = tsk.BlockAutorunInf == 1;

        //tbox
        if (tsk.IDLE_PROCESSOR_VALUE > -1)
            tboxMaximumCPUUsege.Text = tsk.IDLE_PROCESSOR_VALUE.ToString();
        if (tsk.IDLE_DISK_VALUE > -1)
            tboxMaximumDiskActivity.Text = tsk.IDLE_DISK_VALUE.ToString();
        if (tsk.IDLE_MOUSE_VALUE > -1)
            tboxMaximumDisplacement.Text = tsk.IDLE_MOUSE_VALUE.ToString();
        if (tsk.IDLE_NOTEBOOK > -1)
            tboxMinimumBattery.Text = tsk.IDLE_NOTEBOOK_VALUE.ToString();
        tboxLogFile.Text = tsk.REPORT_NAME;
        if (tsk.LIMIT_REPORT_VALUE > -1)
            tboxMaximumSizeLog.Text = tsk.LIMIT_REPORT_VALUE.ToString();
        tboxFilterDefined.Text = tsk.FILTER_DEFINED;
        tboxFilterExclude.Text = tsk.FILTER_EXCLUDE;

        //enable disable textboxes checkboxes

        if (!cboxScanInBackGround.Checked)
        {
            cboxMaximumCPUUsege.InputAttributes.Add("disabled", "true");
            cboxMaximumDiskActivity.InputAttributes.Add("disabled", "true");
            cboxMaximumDisplacement.InputAttributes.Add("disabled", "true");
            cboxMinimumBattery.InputAttributes.Add("disabled", "true");
            cboxScanFilesByUser.InputAttributes.Add("disabled", "true");

            tboxMaximumCPUUsege.Attributes.Add("disabled", "true");
            tboxMaximumDiskActivity.Attributes.Add("disabled", "true");
            tboxMaximumDisplacement.Attributes.Add("disabled", "true");
            tboxMinimumBattery.Attributes.Add("disabled", "true");
        }
        else
        {
            if (!cboxMaximumCPUUsege.Checked)
            {
                tboxMaximumCPUUsege.Attributes.Add("disabled", "true");
            }
            if (!cboxMaximumDiskActivity.Checked)
            {
                tboxMaximumDiskActivity.Attributes.Add("disabled", "true");
            }
            if (!cboxMaximumDisplacement.Checked)
            {
                tboxMaximumDisplacement.Attributes.Add("disabled", "true");
            }
            if (!cboxMinimumBattery.Checked)
            {
                tboxMinimumBattery.Attributes.Add("disabled", "true");
            }
        }


        //ddl
        ddlInfected.SelectedIndex = tsk.InfectedAction1;

        switch (tsk.InfectedAction1)
        {
            case 0:
                ddlPrevInfected.Items.Clear();
                for (Int32 i = 1; i < ddlInfected.Items.Count; i++)
                {
                    ddlPrevInfected.Items.Add(ddlInfected.Items[i].Text);
                }
                ddlPrevInfected.SelectedIndex = 0;
                ddlPrevPrevInfected.SelectedIndex = 0;
                break;
            case 1:
                ddlPrevInfected.Items.Clear();
                for (Int32 i = 0; i < ddlInfected.Items.Count; i++)
                {
                    if (i != 1)
                    {
                        ddlPrevInfected.Items.Add(ddlInfected.Items[i].Text);
                    }
                }
                ddlPrevInfected.SelectedIndex = 0;
                ddlPrevPrevInfected.SelectedIndex = 0;
                break;
            case 2:
                ddlPrevInfected.SelectedIndex = 1;
                ddlPrevPrevInfected.SelectedIndex = 0;

                ddlPrevInfected.Enabled = false;
                ddlPrevPrevInfected.Enabled = false;
                cboxInfectedQua.InputAttributes.Add("disabled", "true");
                cboxInfectedQuaPrev.InputAttributes.Add("disabled", "true");
                break;
            case 3:
                ddlPrevInfected.SelectedIndex = 2;
                ddlPrevPrevInfected.SelectedIndex = 1;

                ddlPrevInfected.Enabled = false;
                ddlPrevPrevInfected.Enabled = false;
                cboxInfectedQua.InputAttributes.Add("disabled", "true");
                cboxInfectedQuaPrev.InputAttributes.Add("disabled", "true");

                cboxInfectedQuarantine.InputAttributes.Add("disabled", "true");
                break;
        }

        if (tsk.InfectedAction1 < 2)
        {
            ddlPrevInfected.SelectedIndex = tsk.InfectedAction2;
            switch (ddlPrevInfected.SelectedIndex)
            {
                case 0:
                    ddlPrevPrevInfected.SelectedIndex = 0;
                    break;
                case 1:
                    ddlPrevPrevInfected.SelectedIndex = 0;
                    ddlPrevPrevInfected.Enabled = false;
                    cboxInfectedQuaPrev.InputAttributes.Add("disabled", "true");
                    break;
                case 2:
                    ddlPrevPrevInfected.SelectedIndex = 1;
                    ddlPrevPrevInfected.Enabled = false;
                    cboxInfectedQuaPrev.InputAttributes.Add("disabled", "true");
                    cboxInfectedQua.InputAttributes.Add("disabled", "true");
                    break;
            }

            if (ddlPrevInfected.SelectedIndex == 0)
            {
                ddlPrevPrevInfected.SelectedIndex = tsk.InfectedAction3;
                if (ddlPrevPrevInfected.SelectedIndex == 1)
                    cboxInfectedQuaPrev.InputAttributes.Add("disabled", "true");
            }
        }


        ddlSuspicious.SelectedIndex = tsk.SuspiciousAction1;

        switch (ddlSuspicious.SelectedIndex)
        {
            case 0:
                ddlPrePrevSuspicious.Enabled = false;
                ddlPrePrevSuspicious.SelectedIndex = 0;
                cboxSuspiciousQuaPrev.InputAttributes.Add("disabled", "true");
                break;
            case 1:
                ddlPrePrevSuspicious.Enabled = true;
                ddlPrePrevSuspicious.SelectedIndex = 0;
                cboxSuspiciousQuaPrev.InputAttributes.Add("disabled", "false");
                break;
            case 2:
                ddlPrePrevSuspicious.Enabled = false;
                ddlPrePrevSuspicious.SelectedIndex = 1;
                cboxSuspiciousQuaPrev.InputAttributes.Add("disabled", "true");
                break;
        }

        ddlPrePrevSuspicious.SelectedIndex = tsk.SuspiciousAction2;

        ddlHeuristicAnalysis.SelectedIndex = tsk.HEURISTIC;

        if (ddlHeuristicAnalysis.SelectedIndex == 0)
        {
            cboxBlockUSB.InputAttributes.Add("disabled", "true");
            cboxSuspiciousQuaPrev.InputAttributes.Add("disabled", "true");
            ddlPrePrevSuspicious.Attributes.Add("disabled", "true");
            cboxSuspiciousQua.InputAttributes.Add("disabled", "true");
            ddlSuspicious.Attributes.Add("disabled", "true");
        }

        //Folders and files excluded from Monitor scanning 
        ddlExcludingFoldersAndFilesDelete.DataSource = tsk.ExcludingFoldersAndFilesDelete;
        ddlExcludingFoldersAndFilesDelete.DataBind();

        //List of paths to scan
        ddlListOfPathToScan.DataSource = tsk.ListOfPathToScan;
        ddlListOfPathToScan.DataBind();

        //rb
        switch (tsk.CHECK_MODE)
        {
            case 0:
                rbScanStandartSet.Checked = true;
                break;
            case 1:
                rbScanSelectedTypes.Checked = true;
                break;
            case 2:
                rbMonitorScanAllTypes.Checked = true;
                break;
        }
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ConfigureMonitor;

        ValidateFields();
        task.Param = BuildXml();

        return task;
    }

    private void ScrollToObj(String controlId)
    {
        if (!Page.ClientScript.IsStartupScriptRegistered("close"))
            Page.ClientScript.RegisterStartupScript(typeof(Page), "close", "document.getElementById('" + controlId + "').scrollIntoView(true);", true);
    }

    protected void lbtnExcludingFoldersAndFilesAdd_Click(object sender, EventArgs e)
    {
        if (tboxExcludingFoldersAndFilesAdd.Text == String.Empty) return;
        String path = tboxExcludingFoldersAndFilesAdd.Text.Trim();
        if ((ddlExcludingFoldersAndFilesDelete.Items.FindByText("-" + path) != null) ||
            (ddlExcludingFoldersAndFilesDelete.Items.FindByText("+" + path) != null))
        {
        }
        else
        {
            ddlExcludingFoldersAndFilesDelete.Items.Add((cboxIncludingSubFolders.Checked ? "+" : "-") + path);
        }
        tboxExcludingFoldersAndFilesAdd.Text = String.Empty;
    }

    protected void lbntnExcludingFoldersAndFilesDelete_Click(object sender, EventArgs e)
    {
        ddlExcludingFoldersAndFilesDelete.Items.Remove(ddlExcludingFoldersAndFilesDelete.SelectedItem);
    }

    protected void lbtnListOfPathToScanAdd_Click(object sender, EventArgs e)
    {
        if (tboxListOfPathToScanAdd.Text == String.Empty) return;
        String path = tboxListOfPathToScanAdd.Text.Trim();
        if ((ddlListOfPathToScan.Items.FindByText("-" + path) != null) ||
            (ddlListOfPathToScan.Items.FindByText("+" + path) != null))
        {
        }
        else
        {
            ddlListOfPathToScan.Items.Add((cboxListOfPathToScanIncludingSubFolders.Checked ? "+" : "-") + path);
        }
        tboxListOfPathToScanAdd.Text = String.Empty;
    }

    protected void lbtnListOfPathToScanDelete_Click(object sender, EventArgs e)
    {
        ddlListOfPathToScan.Items.Remove(ddlListOfPathToScan.SelectedItem);
    }
}
