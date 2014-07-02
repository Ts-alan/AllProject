using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Service.Vba32NS;
using System.Data;
using System.Text;
using System.Xml;
using System.IO;
using ARM2_dbcontrol.Tasks.ConfigureIntegrityCheck;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;

public partial class Controls_TaskConfigureIntegrityCheck : System.Web.UI.UserControl, ITask
{
    private static TaskConfigureIntegrityCheck taskIntegrityCheck;

    private Int32 FilesAddedCount = 0;
    private Int32 RegistryAddedCount = 0;

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
            InitFields();

        InitFieldsJournalEvent(taskIntegrityCheck.journalEvent);
    }

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
        SetEnabled();

        taskIntegrityCheck = new TaskConfigureIntegrityCheck(GetEvents());
        FilesUpdateData();
        RegistryUpdateData();
    }

    private void SetEnabled()
    {
        tblFilesUpdatePanel.Enabled = tblRegistryUpdatePanel.Enabled = _enabled;
    }

    private String[] GetEvents()
    {
        String[] s = { "JE_VIC_REGISTRY_CHANGED",
                         "JE_VIC_DEVICES_STATE_SAVE_START",
                         "JE_VIC_FILES_STATE_SAVE_FINISHED",
                         "JE_VIC_REGISTRY_STATE_CHECK_FINISHED",
                         "JE_VIC_REGISTRY_STATE_SAVE_START",
                         "JE_VIC_DEVICE_CAN_NOT_READ",
                         "JE_VIC_REGISTRY_STATE_CHECK_START",
                         "JE_VIC_DEVICES_STATE_SAVE_FINISHED",
                         "JE_VIC_FILES_STATE_CHECK_START",
                         "JE_VIC_FILES_STATE_CHECK_FINISHED",
                         "JE_VIC_FILES_CAN_NOT_OPEN",
                         "JE_VIC_DEVICES_STATE_CHECK_FINISHED",
                         "JE_VIC_REGISTRY_CAN_NOT_OPEN_KEY",
                         "JE_VIC_DEVICES_STATE_CHECK_START",
                         "JE_VIC_FILES_STATE_SAVE_START",
                         "JE_VIC_FILE_CHANGED",
                         "JE_VIC_DEVICE_CHANGED",
                         "JE_VIC_REGISTRY_STATE_SAVE_FINISHED"};
        return s;
    }

    public bool ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ConfigureIntegrityCheck;
        SaveJournalEvents();
        task.Param = taskIntegrityCheck.SaveToXml(); 
        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        taskIntegrityCheck.LoadFromXml(task.Param);
        FilesUpdateData();
        RegistryUpdateData();
        LoadJournalEvent(taskIntegrityCheck.journalEvent);
    }
        
    public String BuildTask()
    {
        return taskIntegrityCheck.GetTask();
    }

    #region JournalEvents

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
        taskIntegrityCheck.journalEvent = je;
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

    #region Files
    private void FilesUpdateData()
    {
        FilesDataList.DataSource = taskIntegrityCheck.Files;
        FilesDataList.DataBind();
    }
    protected void FilesAddDialogApplyButtonClick(object sender, EventArgs e)
    {

        IntegrityCheckFilesEntity info = new IntegrityCheckFilesEntity();
        info.Path = FilesAddDialogPath.Text;
        info.Template = FilesAddDialogTemplate.Text;
        taskIntegrityCheck.Files.Add(info);
        FilesUpdateData();
    }
    protected void FilesDeleteButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(FileshdnActiveRowNo.Value);
        if (index != 0)
        {
            taskIntegrityCheck.Files.RemoveAt(index - 1);
            FileshdnActiveRowNo.Value = Convert.ToString(0);
            FilesUpdateData();
        }
    }
    protected void FilesChangeButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(FileshdnActiveRowNo.Value);
        if (index != 0)
        {
            IntegrityCheckFilesEntity info = taskIntegrityCheck.Files[index - 1];
            info.Path = FilesAddDialogPath.Text;
            info.Template = FilesAddDialogTemplate.Text;
            taskIntegrityCheck.Files.Insert(index - 1, info);
            taskIntegrityCheck.Files.RemoveAt(index);
            FilesUpdateData();
        }
    } 

    protected void FilesDataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        IntegrityCheckFilesEntity info;
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            info = (IntegrityCheckFilesEntity)e.Item.DataItem;
            (e.Item.FindControl("FileshdnRowNo") as HiddenField).Value = (++FilesAddedCount).ToString();
            (e.Item.FindControl("lblFilesPath") as Label).Text = info.Path;
            (e.Item.FindControl("lblFilesTemplate") as Label).Text = info.Template;           
            if (e.Item.ItemIndex % 2 == 0)
                (e.Item.FindControl("trFilesItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRowAlternating");
            else
                (e.Item.FindControl("trFilesItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRow");
        }
    }

    protected void FilesDataList_SelectedIndexChanged(object sender, DataListCommandEventArgs e)
    {
        FilesDataList.EditItemIndex = e.Item.ItemIndex;
        FilesDataList.SelectedIndex = e.Item.ItemIndex;
        FilesUpdateData();
    }

    #endregion

    #region Registry

    private void RegistryUpdateData()
    {
        RegistryDataList.DataSource = taskIntegrityCheck.Registries;
        RegistryDataList.DataBind();
    }

    protected void RegistryAddDialogApplyButtonClick(object sender, EventArgs e)
    {
       String path = RegistryAddDialogPath.Text;
       IntegrityCheckRegistryEntity reg = new IntegrityCheckRegistryEntity();
       reg.Path = path;
       taskIntegrityCheck.Registries.Add(reg);
       RegistryUpdateData();
    }

    protected void RegistryDeleteButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(RegistryhdnActiveRowNo.Value);
        if (index != 0)
        {
            taskIntegrityCheck.Registries.RemoveAt(index - 1);
            RegistryhdnActiveRowNo.Value = Convert.ToString(0);
            RegistryUpdateData();
        }
    }

    protected void RegistryChangeButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(RegistryhdnActiveRowNo.Value);
        if (index != 0)
        {
            IntegrityCheckRegistryEntity reg = taskIntegrityCheck.Registries[index - 1];
            reg.Path = RegistryAddDialogPath.Text;
            taskIntegrityCheck.Registries.Insert(index - 1, reg);
            taskIntegrityCheck.Registries.RemoveAt(index);
            RegistryUpdateData();
        }
    }

    protected void RegistryDataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        IntegrityCheckRegistryEntity reg;
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            reg = (IntegrityCheckRegistryEntity)e.Item.DataItem;
            String info = reg.Path;
            (e.Item.FindControl("RegistryhdnRowNo") as HiddenField).Value = (++RegistryAddedCount).ToString();
            (e.Item.FindControl("lblRegistryPath") as Label).Text = info;
            if (e.Item.ItemIndex % 2 == 0)
                (e.Item.FindControl("trRegistryItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRowAlternating");
            else
                (e.Item.FindControl("trRegistryItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRow");
        }
    }

    protected void RegistryDataList_SelectedIndexChanged(object sender, DataListCommandEventArgs e)
    {
        RegistryDataList.EditItemIndex = e.Item.ItemIndex;
        RegistryDataList.SelectedIndex = e.Item.ItemIndex;
        RegistryUpdateData();
    }

    #endregion

}