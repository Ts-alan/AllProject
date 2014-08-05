using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ARM2_dbcontrol.Service.Vba32NS;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Tasks.ConfigureFileCleaner;
using ARM2_dbcontrol.Tasks.ConfigureFileCleanerCleaningTemplate;
using Newtonsoft.Json;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;

public partial class Controls_TaskConfigureFileCleaner : System.Web.UI.UserControl, ITask
{
    private static TaskConfigureFileCleaner fileCleaner;
    private Int32 FileCleanerRulesCount = 0;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitFields();
        }
        InitFieldsJournalEvent(fileCleaner.journalEvent);
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
        if (fileCleaner == null)
        {
            fileCleaner = new TaskConfigureFileCleaner(GetEvents());
            fileCleaner.Vba32CCUser = Anchor.GetStringForTaskGivedUser();
        }
        else
        {
            fileCleaner.FullProgramList.Clear();
            fileCleaner.journalEvent.ClearEvents();
        }

        FileCleanerUpdateData();
    }

    private void SetEnabled()
    {
        tblFileCleanerMainPanel.Enabled = _enabled;
    }


    public Boolean ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.FileCleaner;
        SaveJournalEvents();
        task.Param = fileCleaner.SaveToXml();
        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task == null || task.Type != TaskType.FileCleaner)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
        fileCleaner.LoadFromXml(task.Param);
        LoadJournalEvent(fileCleaner.journalEvent);
        FileCleanerUpdateData();
    }

    public String BuildTask()
    {
        return fileCleaner.GetTask();
    }

    private String[] GetEvents()
    {
        String[] s = { "JE_VFC_START",
                         "JE_VFC_STOP",
                         "JE_VFC_APPLIED_SETTINGS_OK",
                         "JE_VFC_APPLIED_SETTINGS_FAILED",
                         "JE_VFC_REMOVED"};
        return s;
    }

    #region FileCleaner
    private void FileCleanerUpdateData()
    {
        ProgramListDataList.DataSource = fileCleaner.FullProgramList;
        ProgramListDataList.DataBind();
    }


    protected void AddProgramHiddenButtonClick(object sender, EventArgs e)
    {
        SingleCleaningTemplate rule = new SingleCleaningTemplate();
        rule.IsActive = false;

        String json = Request["__EVENTARGUMENT"];
        List<FileCleanerTemplate> JsonList = JsonConvert.DeserializeObject(json, rule.Templates.GetType()) as List<FileCleanerTemplate>;

        rule.Templates = new List<FileCleanerTemplate>(JsonList);
        rule.Name = AddProgramDialogName.Text;

        fileCleaner.FullProgramList.Add(rule);        
        
        FileCleanerUpdateData();        
    }

    protected void ProgramListDataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            SingleCleaningTemplate item = (SingleCleaningTemplate)e.Item.DataItem;
            

            (e.Item.FindControl("hdnProgramRowNo") as HiddenField).Value = (++FileCleanerRulesCount).ToString();
            (e.Item.FindControl("lblProgramName") as Label).Text = item.Name;

            (e.Item.FindControl("chkProgramChecked") as CheckBox).Checked = ((SingleCleaningTemplate)e.Item.DataItem).IsActive;
            (e.Item.FindControl("hdnProgramJson") as HiddenField).Value = JsonConvert.SerializeObject(item.Templates);

            if (e.Item.ItemIndex % 2 == 0)
                (e.Item.FindControl("trProgramItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRowAlternating");
            else
                (e.Item.FindControl("trProgramItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRow");
        }
    }

    protected void ProgramListDataList_SelectedIndexChanged(object sender, DataListCommandEventArgs e)
    {
        ProgramListDataList.EditItemIndex = e.Item.ItemIndex;
        ProgramListDataList.SelectedIndex = e.Item.ItemIndex;
        FileCleanerUpdateData();
    }

    protected void DeleteProgramButtonClick(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(FileCleanerhdnActiveRowNo.Value);
        if (index != 0)
        {
            fileCleaner.FullProgramList.RemoveAt(index - 1);
            FileCleanerhdnActiveRowNo.Value = Convert.ToString(0);
            FileCleanerUpdateData();
        }
    }

    protected void ChangeProgramRulesHiddenButtonClick(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(FileCleanerhdnActiveRowNo.Value);
           if (index != 0)
           {
               SingleCleaningTemplate rule = fileCleaner.FullProgramList[index - 1];

               String json = Request["__EVENTARGUMENT"];

               List<FileCleanerTemplate> JsonList = JsonConvert.DeserializeObject(json, rule.Templates.GetType()) as List<FileCleanerTemplate>;

               rule.Templates = JsonList;
               rule.Name = AddProgramDialogName.Text;
               fileCleaner.FullProgramList.Insert(index - 1, rule);
               fileCleaner.FullProgramList.RemoveAt(index);

               FileCleanerUpdateData();
           }
    }

    protected void chkProgramChecked_OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = sender as CheckBox;
        int index = Convert.ToInt32((chk.Parent.Controls[3] as HiddenField).Value);
        SingleCleaningTemplate rule = fileCleaner.FullProgramList[index - 1];
        rule.IsActive = chk.Checked;
        fileCleaner.FullProgramList.Insert(index - 1, rule);
        fileCleaner.FullProgramList.RemoveAt(index);
        FileCleanerUpdateData();
    }


    #endregion

    #region JournalEvents

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
        fileCleaner.journalEvent = je;
    }

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

    #endregion


}
