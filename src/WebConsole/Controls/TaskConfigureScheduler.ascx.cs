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
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Service.Vba32NS;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using VirusBlokAda.CC.Common.Xml;
using Newtonsoft.Json;

public partial class Controls_TaskConfigureScheduler : System.Web.UI.UserControl, ITask
{
    private static TaskConfigureScheduler scheduler;
    private bool _hideHeader = false;

    public bool HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private bool _enabled = true;

    public bool Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitFields();
            LoadTableInfo();
        }
    }

    #region ITask Members

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;

        if (scheduler == null)
        {
            scheduler = new TaskConfigureScheduler();
            scheduler.Vba32CCUser = Anchor.GetStringForTaskGivedUser();
        }
        else scheduler.SchedulerTasksList.Clear();    
    }

    public bool ValidateFields()
    {
        return true;
    }

    private bool ValidateRule()
    {               
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        GetCurrentSchedulerTasks();
        
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ConfigureSheduler;

        ValidateFields();
        task.Param = scheduler.SaveToXml();
        return task;
    }
    
    private void GetCurrentSchedulerTasks()
    {
        scheduler.SchedulerTasksList.Clear();
        String jsonTableState = hdnSchedulerTableState.Value;
        List<SchedulerTaskFromJSON> SchedulerTasksList = new List<SchedulerTaskFromJSON>();
        SchedulerTasksList = JsonConvert.DeserializeObject(jsonTableState, SchedulerTasksList.GetType()) as List<SchedulerTaskFromJSON>;
        foreach (SchedulerTaskFromJSON jsonTask in SchedulerTasksList)
        {
            scheduler.SchedulerTasksList.Add(ConvertJSONTaskToScheduler(jsonTask));
        }
    }

    public String BuildTask()
    {
        return scheduler.GetTask();
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task == null || task.Type != TaskType.ConfigureSheduler)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        scheduler.LoadFromXml(task.Param);
        LoadTableInfo();
    }

    private void LoadTableInfo()
    {
        List<SchedulerTaskFromJSON> TaskList = new List<SchedulerTaskFromJSON>();
        foreach (SchedulerTask task in scheduler.SchedulerTasksList)
        {
            TaskList.Add(ConvertToJSONTask(task));
        }
        String json = JsonConvert.SerializeObject(TaskList);

        hdnSchedulerTableState.Value = json;
    }

    #endregion

    #region JSONConvert
    
    private SchedulerTask ConvertJSONTaskToScheduler(SchedulerTaskFromJSON jsonTask)
    {
        SchedulerTask task = new SchedulerTask();
        task.Type = (ActionTypeEnum)jsonTask.TaskType;
        task.Period = (PeriodicityEnum)jsonTask.TaskPeriod;
        task.TaskDateTime = DateTime.Parse(jsonTask.TaskDateTime);
        task.IsConsideringSystemLoad = jsonTask.IsConsideringSystemLoad;

        return task;
    }

    private SchedulerTaskFromJSON ConvertToJSONTask(SchedulerTask task)
    {
        SchedulerTaskFromJSON jsonTask = new SchedulerTaskFromJSON();
        jsonTask.TaskType = (Int32)task.Type;
        jsonTask.TaskPeriod = (Int32)task.Period;
        jsonTask.TaskDateTime = task.TaskDateTime.ToString("dd.MM.yyyy hh:mm");
        jsonTask.IsConsideringSystemLoad = task.IsConsideringSystemLoad;

        return jsonTask;
    }

    public struct SchedulerTaskFromJSON
    {
        public Int32 TaskType;
        public Int32 TaskPeriod;
        public String TaskDateTime;
        public Boolean IsConsideringSystemLoad;
    }

    #endregion
}