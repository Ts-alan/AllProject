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

using ARM2_dbcontrol.Generation;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Filters;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;

/// <summary>
/// Задача настройки карантина
/// </summary>
public partial class Controls_TaskConfigureQuarantine : System.Web.UI.UserControl, ITask
{
    private static TaskConfigureQuarantine quarantine;

    protected void Page_Init(object sender, EventArgs e)
    {

        if (!Page.IsPostBack || quarantine == null)
        {
            InitFields();

            InitFieldsJournalEvent(quarantine.journalEvent);
        }
        else
        {
            InitFieldsJournalEvent(quarantine.journalEvent);
            LoadQuarantine();
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
        quarantine = new TaskConfigureQuarantine(GetEvents());
    }

    private void SetEnabled()
    {
        JournalEventTable.Enabled = _enabled;
    }
    private String[] GetEvents()
    {
        String[] s = { "JE_QTN_ADD", "JE_QTN_DELETE", "JE_QTN_RESTORE"};
        return s;
    }
    public Boolean ValidateFields()
    {
       /* Validation vld = new Validation("");
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
       
            /*vld.Value = tboxUserName.Text;
            if (!vld.CheckStringValue())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.CongLdrUserName);
        }

        if (cboxMaintenancePeriod.Checked)
        {
            vld.Value = tboxServicePeriod.Text;
            if (!vld.CheckPositiveInteger())
                throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                    + Resources.Resource.CongQtnMaintenancePeriod);

            if (cboxMaximumQuarantineSize.Checked)
            {
               vld.Value = tboxMaxSize.Text;
               if (!vld.CheckPositiveInteger())
                   throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                       + Resources.Resource.CongQtnMaximumQuarantineSize);
            }

            if (cboxMaximumStorageTime.Checked)
            {
                vld.Value =  tboxMaximumStorageTime.Text;
                if (!vld.CheckPositiveInteger())
                    throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.CongQtnMaximumStorageTime);
            }
        }*/

        return true;
    }

    public void LoadQuarantine()
    {
        LoadJournalEvent(quarantine.journalEvent);
    }
    /// <summary>
    /// Загружаем состояние карантина
    /// </summary>
    /// <param name="task">сохраненное состояние</param>
    public void LoadState(TaskUserEntity task)
    {
        if (task == null || task.Type != TaskType.ConfigureQuarantine)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
        quarantine.LoadFromXml(task.Param);
        LoadQuarantine();
    }
    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ConfigureQuarantine;
        SaveJournalEvents();
        ValidateFields();
        task.Param = quarantine.SaveToXml();

        return task;
    }

    public String BuildTask()
    {
        return quarantine.GetTask();
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

            if ((JournalEventTable.Rows[i + 1].Cells[1].Controls[0] as CheckBox).Checked == false)
            {
                je.Events[i].EventFlag ^= EventJournalFlags.WindowsJournal;
            }
            if ((JournalEventTable.Rows[i + 1].Cells[2].Controls[0] as CheckBox).Checked == false)
            {
                je.Events[i].EventFlag ^= EventJournalFlags.LocalJournal;
            }
            if ((JournalEventTable.Rows[i + 1].Cells[3].Controls[0] as CheckBox).Checked == false)
            {
                je.Events[i].EventFlag ^= EventJournalFlags.CCJournal;
            }
        }
        quarantine.journalEvent = je;
    }
    #endregion
}
