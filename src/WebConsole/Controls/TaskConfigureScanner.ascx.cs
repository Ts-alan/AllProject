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

/// <summary>
/// «адача запуска консольного сканера
/// </summary>
public partial class Controls_TaskConfigureScanner : System.Web.UI.UserControl, ITask
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();

        ChangeEnabledControl();
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

    private void ChangeEnabledControl()
    {
        Tabs.Enabled = _enabled;
    }

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;

        LoadSource();

        tabPanel1.HeaderText = Resources.Resource.Actions;
        tabPanel2.HeaderText = Resources.Resource.CongMonitorObjects;
        tabPanel3.HeaderText = Resources.Resource.CongMonitorReport;
        tabPanel4.HeaderText = Resources.Resource.CongScannerAdditional;
        tabPanel5.HeaderText = Resources.Resource.CongScannerRemote;

        lblExclude.Text = Resources.Resource.CongScannerExclude;
        lblSet.Text = Resources.Resource.CongScannerSet;
        lblAddArch.Text = Resources.Resource.CongScannerAdd;

        cboxMultyThreading.Text = " " + Resources.Resource.EnableDisableMultithreading;
        cboxShowProgressScan.Text = " " + Resources.Resource.ShowProgressScan;
    }

    private void LoadSource()
    {
        if (ddlHeuristicAnalysis.Items.Count != 0) return;
        if ((!rbDelete.Checked) && (!rbRemove.Checked) && (!rbRename.Checked) && (!rbSkip.Checked))
            rbSkip.Checked = true;

        if ((!rbDeleteSusp.Checked) && (!rbRemoveSusp.Checked) && (!rbRenameSusp.Checked) && (!rbSkipSusp.Checked))
            rbSkipSusp.Checked = true;

        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Disabled);
        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Optimal);
        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Maximum);
        ddlHeuristicAnalysis.Items.Add(Resources.Resource.Excessive);
        ddlHeuristicAnalysis.SelectedIndex = 2;

        ddlMode.Items.Add(Resources.Resource.Fast);
        ddlMode.Items.Add(Resources.Resource.Safe);
        ddlMode.Items.Add(Resources.Resource.Excessive);

        ddlCountThread.Items.Add("Auto");
        for (Int32 i = 2; i < 16; i++)
        {
            ddlCountThread.Items.Add(i.ToString());
        }
    }

    public Boolean ValidateFields()
    {
        Validation vld = new Validation(tboxSet.Text);
        if(cboxSet.Checked)
        {
            if (!vld.CheckStringToExtension())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongScannerSet);
        }

        if (cboxAddArch.Checked)
        {
            vld.Value = tboxAddArch.Text;
            if (!vld.CheckStringToExtension())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongScannerAdd);
        }

        if (cboxExclude.Checked)
        {
            vld.Value = tboxExclude.Text;
            if (!vld.CheckStringToExtension())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                + Resources.Resource.CongScannerExclude);
        }

        vld.Value = tboxCheckObjects.Text;
        if (!vld.CheckPath())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
            + Resources.Resource.CongMonitorObjects);

        if (cboxIsArchiveSize.Checked)
        {
            vld.Value = tboxArchiveSize.Text;
            if (!vld.CheckSize())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
            + Resources.Resource.CongScannerArchivesSize);
        }

        if (rbRemove.Checked)
        {
            vld.Value = tboxRemove.Text;
            if (!vld.CheckPath())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
            + Resources.Resource.CongScannerRemovePathToInfected);
        }

        if (cboxKeep.Checked)
        {
            vld.Value = tboxKeep.Text;
            if (!vld.CheckPath())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
            + Resources.Resource.CongScannerKeep);
        }

        if (cboxSaveInfectedToReport.Checked)
        {
            vld.Value = tboxSaveInfectedToReport.Text;
            if (!vld.CheckPath())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
            + Resources.Resource.CongScannerSaveInfectedToReport);
        }

        vld.Value = tboxPathToScanner.Text;
        if (!vld.CheckPath())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        + Resources.Resource.CongScannerPathToScanner);

        return true;
    }

    private String BuildXml()
    {
        return GetTaskEntity().SaveToXml();        
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ConfigureScanner)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        TaskConfigureScanner tsk = new TaskConfigureScanner();
        tsk.LoadFromXml(task.Param);

        cboxCheckArchives.Checked = tsk.IsCheckArchives;
        cboxCheckMacros.Checked = tsk.IsCheckMacros;
        cboxCheckMail.Checked = tsk.IsCheckMail;
        cboxCheckMemory.Checked = tsk.IsCheckMemory;
        cboxCleanFiles.Checked = tsk.IsCleanFiles;
        cboxCure.Checked = tsk.IsCheckCure;
        cboxCureBoot.Checked = tsk.IsCheckCureBoot;
        cboxDeleteArchives.Checked = tsk.IsDeleteArchives;
        cboxDeleteMail.Checked = tsk.IsDeleteMail;
        cboxDetectAdware.Checked = tsk.IsDetectAdware;
        cboxEnableCach.Checked = tsk.IsEnableCach;
        cboxExclude.Checked = tsk.IsExclude;
        cboxSaveInfectedToQuarantine.Checked = tsk.IsSaveInfectedToQuarantine;
        cboxSaveInfectedToReport.Checked = tsk.IsSaveInfectedToReport;
        cboxSaveSusToQuarantine.Checked = tsk.IsSaveSusToQuarantine;
        cboxScanBootSectors.Checked = tsk.IsScanBootSectors;
        cboxScannerSFX.Checked = tsk.IsSFX;
        cboxScanStartup.Checked = tsk.IsScanStartup;
        cboxSet.Checked = tsk.IsSet;
        cboxKeep.Checked = tsk.IsKeep;
        cboxAdd.Checked = tsk.IsAdd;
        cboxAddArch.Checked = tsk.IsAddArch;
        cboxAddInf.Checked = tsk.IsAddInf;
        cboxUpdate.Checked = tsk.IsUpdateBase;
        cboxAuthenticode.Checked = tsk.IsAuthenticode;

        cboxIsArchiveSize.Checked = tsk.IsUncheckLargeArchives;
        tboxCheckObjects.Text = tsk.CheckObjects;        
        tboxKeep.Text = tsk.Keep;
        tboxPathToScanner.Text = tsk.PathToScanner;
        tboxRemove.Text = tsk.Remove;
        tboxSaveInfectedToReport.Text = tsk.SaveInfectedToReport;
        tboxArchiveSize.Text = tsk.ArchiveSize;

        tboxAddArch.Text = tsk.AddArch;
        tboxExclude.Text = tsk.Exclude;
        tboxSet.Text = tsk.Set;

        switch(tsk.IfCureChecked)
        {
            case 1: 
                rbSkip.Checked = true;
                break;
            case 2:
                rbDelete.Checked = true;
                break;
            case 3:
                rbRename.Checked = true;
                break;
            case 4:
                rbRemove.Checked = true;
                break;
        }

        switch (tsk.SuspiciousMode)
        {
            case 1:
                rbSkipSusp.Checked = true;
                break;
            case 2:
                rbDeleteSusp.Checked = true;
                break;
            case 3:
                rbRenameSusp.Checked = true;
                break;
            case 4:
                rbRemoveSusp.Checked = true;
                break;
        }

        ddlHeuristicAnalysis.SelectedIndex = tsk.HeuristicAnalysis;
        ddlMode.SelectedIndex = tsk.Mode;

        cboxMultyThreading.Checked = tsk.IsMultyThreading;
        if (tsk.IsMultyThreading)
        {
            ddlCountThread.SelectedIndex = tsk.MultyThreading - 1;
            ddlCountThread.Enabled = true;
            lblCountThread.Enabled = true;
        }
        else
        {
            ddlCountThread.SelectedIndex = 0;
            ddlCountThread.Enabled = false;
            lblCountThread.Enabled = false;
        }

        cboxShowProgressScan.Checked = tsk.IsShowScanProgress;

        cboxRemoteServer.Checked = tsk.IsRemoteServerEnabled;
        cboxRemoteClient.Checked = tsk.IsRemoteClientEnabled;
        tboxRemoteAddress.Text = tsk.RemoteClientAddress;
        tboxRemoteAddress.Enabled = tsk.IsRemoteClientEnabled;
    }

    public String GenerateCommandLine(TaskUserEntity task)
    {
        if (task.Type != TaskType.ConfigureScanner)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        TaskConfigureScanner tsk = new TaskConfigureScanner();
        tsk.LoadFromXml(task.Param);

        return tsk.GetTask();
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ConfigureScanner;

        ValidateFields();
        task.Param = BuildXml();

        return task;
    }

    private TaskConfigureScanner GetTaskEntity()
    {
        TaskConfigureScanner task = new TaskConfigureScanner();

        task.CheckObjects = tboxCheckObjects.Text;
        task.IsCheckMemory = cboxCheckMemory.Checked;        
        task.IsScanBootSectors = cboxScanBootSectors.Checked;
        task.IsScanStartup = cboxScanStartup.Checked;
        task.IsCheckArchives = cboxCheckArchives.Checked;
        task.IsCheckMail = cboxCheckMail.Checked;
        task.IsDetectAdware = cboxDetectAdware.Checked;
        task.IsSFX = cboxScannerSFX.Checked;
        task.IsUpdateBase = cboxUpdate.Checked;

        task.Mode = ddlMode.SelectedIndex;
        task.HeuristicAnalysis = ddlHeuristicAnalysis.SelectedIndex;
        task.IsSet = cboxSet.Checked;
        if (cboxSet.Checked)
            task.Set = tboxSet.Text;
        task.IsAddArch = cboxAddArch.Checked;
        if (cboxAddArch.Checked)
            task.AddArch = tboxAddArch.Text;
        task.IsExclude = cboxExclude.Checked;
        if (cboxExclude.Checked)
            task.Exclude = tboxExclude.Text;
        task.IsUncheckLargeArchives = cboxIsArchiveSize.Checked;
        if (cboxIsArchiveSize.Checked)
            task.ArchiveSize = tboxArchiveSize.Text;

        task.IsCheckCure = cboxCure.Checked;
        Int32 index = 0;
        if (rbSkip.Checked) index = 1;
        if (rbDelete.Checked) index = 2;
        if (rbRename.Checked) index = 3;
        if (rbRemove.Checked) index = 4;
        task.IfCureChecked = index;

        if (rbSkipSusp.Checked) index = 1;
        if (rbDeleteSusp.Checked) index = 2;
        if (rbRenameSusp.Checked) index = 3;
        if (rbRemoveSusp.Checked) index = 4;
        task.SuspiciousMode = index;

        task.IsCheckCureBoot = cboxCureBoot.Checked;
        task.IsDeleteArchives = cboxDeleteArchives.Checked;
        task.IsDeleteMail = cboxDeleteMail.Checked;
        task.IsSaveInfectedToQuarantine = cboxSaveInfectedToQuarantine.Checked;
        task.IsSaveSusToQuarantine = cboxSaveSusToQuarantine.Checked;
        task.Remove = tboxRemove.Text;

        task.IsKeep = cboxKeep.Checked;
        if (cboxKeep.Checked)
            task.Keep = tboxKeep.Text;
        task.IsAdd = cboxAdd.Checked;
        task.IsCleanFiles = cboxCleanFiles.Checked;
        task.IsSaveInfectedToReport = cboxSaveInfectedToReport.Checked;
        if (cboxSaveInfectedToReport.Checked)
            task.SaveInfectedToReport = tboxSaveInfectedToReport.Text;
        task.IsAddInf = cboxAddInf.Checked;

        task.IsEnableCach = cboxEnableCach.Checked;
        task.IsAuthenticode = cboxAuthenticode.Checked;
        task.IsCheckMacros = cboxCheckMacros.Checked;
        task.PathToScanner = tboxPathToScanner.Text;
        task.IsMultyThreading = cboxMultyThreading.Checked;
        if (cboxMultyThreading.Checked)
        {
            task.MultyThreading = ddlCountThread.SelectedIndex + 1;
        }
        task.IsShowScanProgress = cboxShowProgressScan.Checked;

        task.IsRemoteServerEnabled = cboxRemoteServer.Checked;
        task.IsRemoteClientEnabled = cboxRemoteClient.Checked;
        if (task.IsRemoteClientEnabled)
            task.RemoteClientAddress = tboxRemoteAddress.Text;
        
        task.Vba32CCUser = Anchor.GetStringForTaskGivedUser();

        return task;
    }

    public String GetTaskForVSIS()
    {
        return GetTaskEntity().GetTaskForVSIS();
    }
}
