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
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;

/// <summary>
/// Задача настройки Монитора
/// </summary>
public partial class Controls_TaskConfigureMonitor : System.Web.UI.UserControl,ITask
{
    private static TaskConfigureMonitor monitor;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack || monitor==null)
        {
            InitFields();
        }
        InitFieldsJournalEvent(monitor.journalEvent);
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

        if (monitor == null)
        {
            monitor = new TaskConfigureMonitor(GetEvents());
            monitor.Vba32CCUser = Anchor.GetStringForTaskGivedUser();
        }
        else
        {
            monitor.Clear();
        }

        UpdateSettings();
    }

    private void SetEnabled()
    {
        cboxMonitorOn.Enabled = _enabled;
        tboxMonitorFileExtensions.Enabled = tboxMonitorFilesExcluded.Enabled = _enabled;
        lbtnMonitorFileExtensionReset.Enabled = _enabled;
        lboxExcludedPath.Enabled = _enabled;
        lbtnPathAdd.Enabled = lbtnPathChange.Enabled = lbtnPathDelete.Enabled = _enabled;
        ddlInfectedActions1.Enabled = ddlInfectedActions2.Enabled = ddlInfectedActions3.Enabled = _enabled;
        ddlSuspiciousActions1.Enabled = ddlSuspiciousActions2.Enabled = _enabled;
        chkInfectedSaveCopy.Enabled = chkSuspiciousSaveCopy.Enabled = _enabled;
        JournalEventTable.Enabled = _enabled;
    }

    private String[] GetEvents()
    {
        String[] s = { "JE_VMT_START", "JE_VMT_STOP", "JE_VMT_APPLIED_SETTINGS_OK", "JE_VMT_APPLIED_SETTINGS_FAILED", 
                         "JE_VMT_CHECK_RESULT_SUSPECTED", "JE_VMT_CHECK_RESULT_INFECTED", "JE_VMT_ACTION_BLOCK", 
                         "JE_VMT_ACTION_CURE", "JE_VMT_ACTION_DELETE", "JE_VMT_ACTION_NONE" };
        return s;
    }

    public Boolean ValidateFields()
    {
        return true;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task == null || task.Type != TaskType.ConfigureMonitor)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
        
        monitor.LoadFromXml(task.Param);
        
        UpdateSettings();
        LoadJournalEvent(monitor.journalEvent);        
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ConfigureMonitor;

        SaveJournalEvents();
        SaveSettings();
        ValidateFields();
        task.Param = monitor.SaveToXml();

        return task;
    }

    public String BuildTask()
    {
        SaveSettings();
        SaveJournalEvents();
        return monitor.GetTask();
    }

    #region Settings

    public void UpdateSettings()
    {
        cboxMonitorOn.Checked = monitor.MONITOR_ON;

        tboxMonitorFileExtensions.Text = monitor.FileExtensions;
        tboxMonitorFilesExcluded.Text = monitor.FileExtensionsExcluded;

        ddlInfectedActions1.SelectedIndex = (Int32)monitor.InfectedAction1;
        ddlInfectedActions2.SelectedIndex = (Int32)monitor.InfectedAction2;
        ddlInfectedActions3.SelectedIndex = (Int32)monitor.InfectedAction3;

        ddlSuspiciousActions1.SelectedIndex = (Int32)monitor.SuspiciousAction1;
        ddlSuspiciousActions2.SelectedIndex = (Int32)monitor.SuspiciousAction2;

        chkInfectedSaveCopy.Checked = monitor.IsSaveInfectedToQuarantine;
        chkSuspiciousSaveCopy.Checked = monitor.IsSaveSuspiciousToQuarantine;

        ExcludedPathUpdateData();
    }

    public void SaveSettings()
    {
        monitor.MONITOR_ON = cboxMonitorOn.Checked;

        monitor.FileExtensions = tboxMonitorFileExtensions.Text;
        monitor.FileExtensionsExcluded = tboxMonitorFilesExcluded.Text;

        monitor.InfectedAction1 = (MonitorInfectedActions)ddlInfectedActions1.SelectedIndex;
        monitor.InfectedAction2 = (MonitorInfectedActions)ddlInfectedActions2.SelectedIndex;
        monitor.InfectedAction3 = (MonitorInfectedActions)ddlInfectedActions3.SelectedIndex;

        monitor.SuspiciousAction1 = (MonitorSuspiciousActions)ddlSuspiciousActions1.SelectedIndex;
        monitor.SuspiciousAction2 = (MonitorSuspiciousActions)ddlSuspiciousActions2.SelectedIndex;


        monitor.IsSaveInfectedToQuarantine = chkInfectedSaveCopy.Checked;
        monitor.IsSaveSuspiciousToQuarantine = chkSuspiciousSaveCopy.Checked;
    }

    private void ExcludedPathUpdateData()
    {
        lboxExcludedPath.Items.Clear();
        foreach (String str in monitor.ExcludingFoldersAndFilesDelete)
        {
            lboxExcludedPath.Items.Add(str);
        }
    }

    protected void lbtnAddExcludedDialogApply_Click(object sender, EventArgs e)
    {
        monitor.ExcludingFoldersAndFilesDelete.Add(tboxAddExcludedDialogPath.Text);

        ExcludedPathUpdateData();
    }

    protected void lbtnPathDelete_Click(object sender, EventArgs e)
    {
        Int32 index = lboxExcludedPath.SelectedIndex;
        if (index >= 0)
        {
            monitor.ExcludingFoldersAndFilesDelete.RemoveAt(index);
            ExcludedPathUpdateData();
        }
    }

    protected void lbtnPathChangeHidden_Click(object sender, EventArgs e)
    {
        Int32 index = lboxExcludedPath.SelectedIndex;
        if (index >= 0)
        {
            monitor.ExcludingFoldersAndFilesDelete[index] = tboxAddExcludedDialogPath.Text;
            ExcludedPathUpdateData();
        }
    } 

    #endregion 

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

    private TableRow GenerateRow(SingleJournalEvent ev, Int32 rowNo)
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
        for (Int32 i = 0; i < JournalEventTable.Rows.Count - 1; i++)
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
        monitor.journalEvent = je;
    }

    #endregion
}
