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
    private Int32 PathAddedCount = 0;
    protected void Page_Init(object sender, EventArgs e)
    {

        if (!Page.IsPostBack || monitor==null)
        {
            InitFields();

            InitFieldsJournalEvent(monitor.journalEvent);
        }
        else
        {
            InitFieldsJournalEvent(monitor.journalEvent);
            LoadMonitor();
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

    private String defaultFilters = TaskConfigureMonitor.DefaultFilters;



    public void InitFields()
    {
        /*if (monitor == null)
        {
            monitor = new TaskConfigureMonitor(GetEvents());
            monitor.Vba32CCUser = Anchor.GetStringForTaskGivedUser();
        }*/
        monitor = new TaskConfigureMonitor(GetEvents());
        ExcludedPathUpdateData();

    }
    private String[] GetEvents()
    {
        String[] s = { "E_VMT_START", "JE_VMT_STOP", "JE_VMT_APPLIED_SETTINGS_OK", "JE_VMT_APPLIED_SETTINGS_FAILED" };
        return s;
    }

    public Boolean ValidateFields()
    {
       /* Validation vld = new Validation(tboxLogFile.Text);
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
        }*/

        return true;
    }

    public void LoadMonitor()
    {
        cboxMonitorOn.Checked = monitor.MONITOR_ON;

        MonitorFileExtensionsTextBox.Text = monitor.FileExtensions;
        MonitorFilesExcludedTextBox.Text = monitor.FileExtensionsExcluded;

        ddlInfectedActions1.SelectedIndex = (int)monitor.InfectedAction1;
        ddlInfectedActions2.SelectedIndex = (int)monitor.InfectedAction2;
        ddlInfectedActions3.SelectedIndex = (int)monitor.InfectedAction3;

        ddlSuspiciousActions1.SelectedIndex = (int)monitor.SuspiciousAction1;
        ddlSuspiciousActions2.SelectedIndex = (int)monitor.SuspiciousAction2;

        chkInfectedSaveCopy.Checked = monitor.IsSaveInfectedToQuarantine;
        chkSuspiciousSaveCopy.Checked = monitor.IsSaveSuspiciousToQuarantine;

        PathAddedCount = 0;
        LoadJournalEvent(monitor.journalEvent);
        ExcludedPathUpdateData();

    }
    public void SaveMonitor()
    {
        monitor.MONITOR_ON = cboxMonitorOn.Checked;

        monitor.FileExtensions = MonitorFileExtensionsTextBox.Text;
        monitor.FileExtensionsExcluded = MonitorFilesExcludedTextBox.Text;

        monitor.InfectedAction1 = (MonitorInfectedActions)ddlInfectedActions1.SelectedIndex;
        monitor.InfectedAction2 = (MonitorInfectedActions)ddlInfectedActions2.SelectedIndex;
        monitor.InfectedAction3 = (MonitorInfectedActions)ddlInfectedActions3.SelectedIndex;

        monitor.SuspiciousAction1 = (MonitorSuspiciousActions)ddlSuspiciousActions1.SelectedIndex;
        monitor.SuspiciousAction2 = (MonitorSuspiciousActions)ddlSuspiciousActions2.SelectedIndex;


        monitor.IsSaveInfectedToQuarantine = chkInfectedSaveCopy.Checked;
        monitor.IsSaveSuspiciousToQuarantine = chkSuspiciousSaveCopy.Checked;

    }




    public void LoadState(TaskUserEntity task)
    {
        if (task == null || task.Type != TaskType.ConfigureMonitor)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
        monitor.LoadFromXml(task.Param);
        LoadMonitor();
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ConfigureMonitor;

        SaveJournalEvents();
        SaveMonitor();
        ValidateFields();
        task.Param = monitor.SaveToXml();

        return task;
    }

    public String BuildTask()
    {
        return monitor.GetTask();
    }

    #region Settings
    private void ExcludedPathUpdateData()
    {
        PathAddedCount = 0;
        ExcludedPathDataList.DataSource = monitor.ExcludingFoldersAndFilesDelete;
        ExcludedPathDataList.DataBind();
    }
    protected void AddExcludedDialogApplyButtonClick(object sender, EventArgs e)
    {
        String path = AddExcludedDialogPath.Text;
        monitor.ExcludingFoldersAndFilesDelete.Add(path);

        ExcludedPathUpdateData();
    }
    protected void PathDeleteButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(PathHdnActiveRowNo.Value);
        if (index != 0)
        {
            monitor.ExcludingFoldersAndFilesDelete.RemoveAt(index - 1);
            PathHdnActiveRowNo.Value = Convert.ToString(0);
            ExcludedPathUpdateData();
        }
    }
    protected void PathChangeButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(PathHdnActiveRowNo.Value);
        if (index != 0)
        {
            String path = monitor.ExcludingFoldersAndFilesDelete[index - 1];
            path = AddExcludedDialogPath.Text;
            monitor.ExcludingFoldersAndFilesDelete.Insert(index - 1, path);
            monitor.ExcludingFoldersAndFilesDelete.RemoveAt(index);
            ExcludedPathUpdateData();
        }
    } 
    protected void ExcludedPathDataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        String path;
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            path = (String)e.Item.DataItem;
            (e.Item.FindControl("PathHdnRowNo") as HiddenField).Value = (++PathAddedCount).ToString();
            (e.Item.FindControl("CongMonExcludedPath") as Label).Text = path;

            if (e.Item.ItemIndex % 2 == 0)
                (e.Item.FindControl("trPathItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRowAlternating");
            else
                (e.Item.FindControl("trPathItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRow");
        }
    }
    protected void ExcludedPathDataList_SelectedIndexChanged(object sender, DataListCommandEventArgs e)
    {
        ExcludedPathDataList.EditItemIndex = e.Item.ItemIndex;
        ExcludedPathDataList.SelectedIndex = e.Item.ItemIndex;
        ExcludedPathUpdateData();
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
          //  chk.Attributes.Add("rowNo", rowNo.ToString());
          //  chk.Attributes.Add("colNo", i.ToString());
          //  chk.AutoPostBack = true;
          //  chk.CheckedChanged += JournalEventChecked;
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
        monitor.journalEvent = je;
    }
    #endregion
}
