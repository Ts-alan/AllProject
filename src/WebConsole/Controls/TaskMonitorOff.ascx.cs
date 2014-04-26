using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;

public partial class Controls_TaskMonitorOff : System.Web.UI.UserControl, ITask
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    #region ITask Members

    public void InitFields()
    {
    }

    public Boolean ValidateFields()
    {
        return true;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.MonitorOff)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        //ARM2_dbcontrol.Tasks.MonitorOnOff.TaskMonitorOnOff tsk = new ARM2_dbcontrol.Tasks.MonitorOnOff.TaskMonitorOnOff();
        //tsk.LoadFromXml(task.Param);
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.MonitorOff;
        task.Param = GetTaskEntity().SaveToXml();
        return task;
    }

    public String GetTaskForVSIS()
    {
        return GetTaskEntity().GetTaskForVSIS();
    }

    public String GetTaskForLoader()
    {
        return GetTaskEntity().GetTask();
    }

    private TaskMonitorOnOff GetTaskEntity()
    {
        TaskMonitorOnOff task = new TaskMonitorOnOff();

        task.IsMonitorOn = false;
        task.Vba32CCUser = Anchor.GetStringForTaskGivedUser();

        return task;
    }

    #endregion
}