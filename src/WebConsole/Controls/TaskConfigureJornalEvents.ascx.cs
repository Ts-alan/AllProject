using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;
using ARM2_dbcontrol.Tasks;

public partial class Controls_TaskConfigureJornalEvents : System.Web.UI.UserControl
{
    private static JournalEvent _journalEvent;

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

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitFields();
        }
        FillJournalEvent();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        JournalEventTable.Enabled = _enabled;
    }

    public void InitFields()
    {
        if (_journalEvent == null)
        {
            _journalEvent = new JournalEvent(GetEvents());
        }
        else
        {
            _journalEvent.ClearEvents();
        }

        HeaderName.Visible = !HideHeader;
    }

    private String[] GetEvents()
    {
        String[] s = { "JE_VDD_APPLIED_CLASSES_RULES_SETTINGS_FAILED",
                         "JE_VDD_APPLIED_CLASSES_RULES_SETTINGS_OK",
                         "JE_VDD_APPLIED_USB_CLASSES_SETTINGS_FAILED",
                         "JE_VDD_APPLIED_USB_CLASSES_SETTINGS_OK",
                         "JE_VDD_APPLIED_USB_DEVICES_SETTINGS_FAILED",
                         "JE_VDD_APPLIED_USB_DEVICES_SETTINGS_OK",
                         "JE_VDD_AUDIT_USB",
                         "JE_VDD_START",
                         "JE_VDD_STOP"
                     };
        return s;
    }

    public Boolean ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.JornalEvents;
        SaveJournalEvents();
        task.Param = _journalEvent.SaveToXml();
        return task;
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
        _journalEvent = je;
    }
    public void LoadState(TaskUserEntity task)
    {
        if (task == null || task.Type != TaskType.JornalEvents)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
        _journalEvent.LoadFromXml(task.Param);
        
        Boolean isChecked = false;
        for (Int32 i = 0; i < _journalEvent.Events.Length; i++)
        {
            for (Int32 j = 0; j < 3; j++)
            {
                switch (j)
                {
                    case 0:
                        isChecked = (_journalEvent.Events[i].EventFlag & EventJournalFlags.WindowsJournal) == EventJournalFlags.WindowsJournal;
                        break;
                    case 1:
                        isChecked = (_journalEvent.Events[i].EventFlag & EventJournalFlags.LocalJournal) == EventJournalFlags.LocalJournal;
                        break;
                    case 2:
                        isChecked = (_journalEvent.Events[i].EventFlag & EventJournalFlags.CCJournal) == EventJournalFlags.CCJournal;
                        break;
                }
                (JournalEventTable.Rows[i + 1].Cells[j + 1].Controls[0] as CheckBox).Checked = isChecked;
            }
        }
    }

    public String BuildTask()
    {
        return _journalEvent.GetTask();
    }

    private void FillJournalEvent()
    {
        if (_journalEvent == null)
            return;

        if (JournalEventTable.Rows.Count == 1)
        {
            for (Int32 i = 0; i < _journalEvent.Events.Length; i++)
            {
                JournalEventTable.Rows.Add(GenerateRow(_journalEvent.Events[i], i));
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


}