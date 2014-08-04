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
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;
using ARM2_dbcontrol.Tasks.ConfigureScanner;

/// <summary>
/// «адача запуска консольного сканера
/// </summary>
public partial class Controls_TaskConfigureScanner : System.Web.UI.UserControl, ITask
{
    TaskConfigureScanner scanner; 
    protected void Page_Init(object sender, EventArgs e)
    {

        if (!Page.IsPostBack || scanner == null)
        {
            InitFields();

            InitFieldsJournalEvent(scanner.journalEvent);
        }
        else
        {
            InitFieldsJournalEvent(scanner.journalEvent);
            LoadScanner();
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



    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
        SetEnabled();
        if (scanner == null)
        {
            scanner = new TaskConfigureScanner(GetEvents());
            scanner.Vba32CCUser = Anchor.GetStringForTaskGivedUser();
        }
      
    }

    private void SetEnabled()
    {
        tboxScannerFileExtensions.Enabled = _enabled;
        tboxScannerFilesExcluded.Enabled = _enabled;
        chkScannerCache.Enabled = chkScannerFindPotential.Enabled = chkScannerFindVirusInstalls.Enabled = _enabled;
        chkScannerMaxSize.Enabled = chkScannerScanArchives.Enabled = chkScannerScanMail.Enabled = chkScannerTrustAuthenCode.Enabled = _enabled;
        chkSuspiciousSaveCopy.Enabled = chkInfectedSaveCopy.Enabled = _enabled;
        ddlInfectedActions.Enabled = ddlInfectedCases.Enabled = _enabled;
        ddlSuspiciousActions.Enabled = ddlSuspiciousCases.Enabled = _enabled;
        tboxScannerMaxSize.Enabled = _enabled;
    }
    private String[] GetEvents()
    {
        String[] s = { "JE_VAS_ACTION_BLOCK",
                        "JE_VAS_ACTION_CURE",
                        "JE_VAS_ACTION_DELETE",
                        "JE_VAS_ACTION_NONE",
                        "JE_VAS_INFECTED",
                        "JE_VAS_START",
                        "JE_VAS_STOP",
                        "JE_VAS_SUSPICIOUS"};
        return s;
    }

    public Boolean ValidateFields()
    {
        //Validation vld = new Validation(tboxSet.Text);
        //if(cboxSet.Checked)
        //{
        //    if (!vld.CheckStringToExtension())
        //        throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        //        + Resources.Resource.CongScannerSet);
        //}

        //if (cboxAddArch.Checked)
        //{
        //    vld.Value = tboxAddArch.Text;
        //    if (!vld.CheckStringToExtension())
        //        throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        //        + Resources.Resource.CongScannerAdd);
        //}

        //if (cboxExclude.Checked)
        //{
        //    vld.Value = tboxExclude.Text;
        //    if (!vld.CheckStringToExtension())
        //        throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        //        + Resources.Resource.CongScannerExclude);
        //}

        //vld.Value = tboxCheckObjects.Text;
        //if (!vld.CheckPath())
        //    throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        //    + Resources.Resource.CongMonitorObjects);

        //if (cboxIsArchiveSize.Checked)
        //{
        //    vld.Value = tboxArchiveSize.Text;
        //    if (!vld.CheckSize())
        //        throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        //    + Resources.Resource.CongScannerArchivesSize);
        //}

        //if (rbRemove.Checked)
        //{
        //    vld.Value = tboxRemove.Text;
        //    if (!vld.CheckPath())
        //        throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        //    + Resources.Resource.CongScannerRemovePathToInfected);
        //}

        //if (cboxKeep.Checked)
        //{
        //    vld.Value = tboxKeep.Text;
        //    if (!vld.CheckPath())
        //        throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        //    + Resources.Resource.CongScannerKeep);
        //}

        //if (cboxSaveInfectedToReport.Checked)
        //{
        //    vld.Value = tboxSaveInfectedToReport.Text;
        //    if (!vld.CheckPath())
        //        throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        //    + Resources.Resource.CongScannerSaveInfectedToReport);
        //}

        //vld.Value = tboxPathToScanner.Text;
        //if (!vld.CheckPath())
        //    throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
        //+ Resources.Resource.CongScannerPathToScanner);

        return true;
    }

    public void LoadScanner()
    {
        tboxScannerFileExtensions.Text = scanner.FileExtensions;
        tboxScannerFilesExcluded.Text = scanner.FileExtensionsExcluded;
        chkScannerCache.Checked = scanner.IsEnableCache;
        chkScannerScanArchives.Checked = scanner.IsCheckArchives;
        chkScannerScanMail.Checked = scanner.IsCheckMail;
        chkScannerFindPotential.Checked = scanner.IsFindPotential;
        chkScannerFindVirusInstalls.Checked = scanner.IsFindVirusInstalls;
        chkScannerMaxSize.Checked = scanner.IsArchivesMaxSize;
        chkScannerTrustAuthenCode.Checked = scanner.IsAuthenticode;
        tboxScannerMaxSize.Text = scanner.ArchiveMaxSize.ToString();
        ddlInfectedActions.SelectedIndex = (int)scanner.InfectedAction;
        ddlInfectedCases.SelectedIndex = (int)scanner.InfectedCases;
        ddlSuspiciousActions.SelectedIndex = (int)scanner.SuspiciousAction;
        ddlSuspiciousCases.SelectedIndex = (int)scanner.SuspiciousCases;
        chkInfectedSaveCopy.Checked = scanner.IsSaveInfectedToQuarantine;
        chkSuspiciousSaveCopy.Checked = scanner.IsSaveSuspiciousToQuarantine;

        LoadJournalEvent(scanner.journalEvent);
        
    }
    public void SaveScanner()
    {
        scanner.FileExtensions = tboxScannerFileExtensions.Text;
        scanner.FileExtensionsExcluded = tboxScannerFilesExcluded.Text;
        scanner.IsEnableCache = chkScannerCache.Checked;
        scanner.IsCheckArchives = chkScannerScanArchives.Checked;
        scanner.IsCheckMail = chkScannerScanMail.Checked;
        scanner.IsFindPotential = chkScannerFindPotential.Checked;
        scanner.IsFindVirusInstalls = chkScannerFindVirusInstalls.Checked;
        scanner.IsArchivesMaxSize = chkScannerMaxSize.Checked;
        scanner.IsAuthenticode = chkScannerTrustAuthenCode.Checked;
        if (scanner.IsArchivesMaxSize)
        {
            scanner.ArchiveMaxSize = Int32.Parse(tboxScannerMaxSize.Text);
        }
        scanner.InfectedAction = (ScannerActions)ddlInfectedActions.SelectedIndex  ;
        scanner.InfectedCases = (ScannerActions)ddlInfectedCases.SelectedIndex;
        scanner.SuspiciousAction = (ScannerActions)ddlSuspiciousActions.SelectedIndex;
        scanner.SuspiciousCases = (ScannerActions)ddlSuspiciousCases.SelectedIndex;
        scanner.IsSaveInfectedToQuarantine = chkInfectedSaveCopy.Checked;
        scanner.IsSaveSuspiciousToQuarantine = chkSuspiciousSaveCopy.Checked;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task == null || task.Type != TaskType.ConfigureScanner)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);     
        scanner.LoadFromXml(task.Param);
        LoadScanner();
    }  

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ConfigureScanner;
        SaveJournalEvents();
        SaveScanner();
        ValidateFields();
        task.Param = scanner.SaveToXml();

        return task;
    }

    public String BuildTask()
    {
        return scanner.GetTask();
    }


    #region JournalEvents

    private void InitFieldsJournalEvent(JournalEvent _events)
    {
        if (_events == null)
            return;

        if (JournalEventTable.Rows.Count == 1)
        {
            for (Int32 i = 0; i < _events.Events.Length; i++)
            {
                JournalEventTable.Rows.Add(GenerateRow(_events.Events[i], i));
            }
        }
    }

    private void LoadJournalEvent(JournalEvent _events)
    {
        if (_events == null)
            return;

        Boolean isChecked = false;
        for (Int32 i = 0; i < _events.Events.Length; i++)
        {
            for (Int32 j = 0; j < 3; j++)
            {
                switch (j)
                {
                    case 0:
                        isChecked = (_events.Events[i].EventFlag & EventJournalFlags.WindowsJournal) == EventJournalFlags.WindowsJournal;
                        break;
                    case 1:
                        isChecked = (_events.Events[i].EventFlag & EventJournalFlags.LocalJournal) == EventJournalFlags.LocalJournal;
                        break;
                    case 2:
                        isChecked = (_events.Events[i].EventFlag & EventJournalFlags.CCJournal) == EventJournalFlags.CCJournal;
                        break;
                }
                (JournalEventTable.Rows[i + 1].Cells[j + 1].Controls[0] as CheckBox).Checked = isChecked;
            }
        }
    }

    private TableRow GenerateRow(SingleJournalEvent ev, int rowNo)
    {
        String eventName = ev.EventName;
        EventJournalFlags val = ev.EventFlag;

        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.Attributes.Add("align", "center");
        Label l = new Label();
        l.Text = eventName;
        cell.Controls.Add(l);
        row.Cells.Add(cell);
        for (Int32 i = 0; i < 3; i++)
        {
            cell = new TableCell();
            CheckBox chk = new CheckBox();
            chk.Checked = false;
            cell.Controls.Add(chk);
            cell.Attributes.Add("align", "center");
            row.Cells.Add(cell);
        }

        return row;
    }

    private void SaveJournalEvents()
    {
        JournalEvent je = new JournalEvent(GetEvents());
        for (int i = 0; i < JournalEventTable.Rows.Count - 1; i++)
        {

            if ((JournalEventTable.Rows[i + 1].Cells[1].Controls[0] as CheckBox).Checked == true)
            {
                je.Events[i].EventFlag |= EventJournalFlags.WindowsJournal;
            }
            if ((JournalEventTable.Rows[i + 1].Cells[2].Controls[0] as CheckBox).Checked == true)
            {
                je.Events[i].EventFlag |= EventJournalFlags.LocalJournal;
            }
            if ((JournalEventTable.Rows[i + 1].Cells[3].Controls[0] as CheckBox).Checked == true)
            {
                je.Events[i].EventFlag |= EventJournalFlags.CCJournal;
            }
        }
        scanner.journalEvent = je;
    }
    #endregion
}
