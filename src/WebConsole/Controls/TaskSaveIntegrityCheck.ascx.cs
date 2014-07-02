using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;

public partial class Controls_TaskSaveIntegrityCheck : System.Web.UI.UserControl, ITask
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
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

    #region ITask Members

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
        SetEnabled();

        ddlCommand.Items.Clear();
        ddlCommand.Items.Add(new ListItem(Resources.Resource.Save, "save"));
        ddlCommand.Items.Add(new ListItem(Resources.Resource.Check, "check"));

        ddlIntegrity.Items.Clear();
        ddlIntegrity.Items.Add(new ListItem(Resources.Resource.Files, "files"));
        ddlIntegrity.Items.Add(new ListItem(Resources.Resource.Registry, "registry"));
        ddlIntegrity.Items.Add(new ListItem(Resources.Resource.Devices, "devices"));        
    }

    private void SetEnabled()
    {
        ddlCommand.Enabled = _enabled;
        ddlIntegrity.Enabled = _enabled;
    }

    public bool ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.SaveCheckIntegrity;
        task.Param = GetTaskEntity().SaveToXml();
        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.SaveCheckIntegrity)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        TaskSaveIntegrityCheck tsk = new TaskSaveIntegrityCheck();
        tsk.LoadFromXml(task.Param);

        ddlCommand.SelectedValue = tsk.Command;
        ddlIntegrity.SelectedValue = tsk.Integrity;
    }

    private TaskSaveIntegrityCheck GetTaskEntity()
    {
        TaskSaveIntegrityCheck task = new TaskSaveIntegrityCheck();

        task.Command = ddlCommand.SelectedValue;
        task.Integrity = ddlIntegrity.SelectedValue;
        task.Vba32CCUser = Anchor.GetStringForTaskGivedUser();

        return task;
    }

    public String GetTask()
    {
        return GetTaskEntity().GetTask();
    }

    #endregion
}