using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Tasks.Common;

/// <summary>
/// Task panel control: container that hosts child SimpleTask and CustomizableTask controls,
/// and gives the ability to assign selected tasks
/// </summary>
[ParseChildren(true)]
[PersistChildren(false)]
public partial class Controls_TaskPanel : System.Web.UI.UserControl
{
    #region LifeCycle
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        CreateTasks(base.DesignMode);
        //this control got switch that toggles if panel is minimazed or not
        //we need to initialize it
        CollapsiblePanelSwitch1.Initialize(divDetails);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScripts();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //enabled/disabled assigntask buttons
        SetEnabledAssignTaskButtons(ActiveTask() != null);
    }

    private void RegisterScripts()
    {
        Page.ClientScript.RegisterClientScriptInclude("JQuery", @"js/jQuery/jquery-1.3.2.js");
    }
    #endregion

    #region Template
    //standart code to create templated control
    //in this case allowing us to put controls inside <TasksTemplate> tag of TaskPanel control.
    private ITemplate _tasksTemplate;
    private Control _tasksTemplateContainer;

    [
        TemplateInstance(TemplateInstance.Single),
        PersistenceMode(PersistenceMode.InnerProperty),
        Browsable(false)
    ]
    public ITemplate TasksTemplate
    {
        get
        {
            return _tasksTemplate;
        }
        set
        {
            if (!base.DesignMode)
            {
                if (_tasksTemplateContainer != null)
                {
                    throw new InvalidOperationException(String.Format("The TasksTemplate of CompositeTaskPanel with ID '{0}' cannot be set after the template has been instantiated or the tasks template container has been created."
                    , ID));
                }
                _tasksTemplate = value;
            }
            else
            {
                _tasksTemplate = value;
                CreateTasks(true);
            }
        }

    }

    [Browsable(false)]
    public Control TasksTemplateContainer
    {
        get
        {
            if (_tasksTemplateContainer == null)
            {
                _tasksTemplateContainer = CreateTasksTemplateContainer();
                AddTasksTemplateContainer();
            }
            return this._tasksTemplateContainer;
        }
    }

    protected virtual Control CreateTasksTemplateContainer()
    {
        return new Control();
    }

    private void AddTasksTemplateContainer()
    {
        TasksPlaceHolder.Controls.Add(_tasksTemplateContainer);
    }

    private void ClearTasksTemplateContainer()
    {
        TasksPlaceHolder.Controls.Clear();
    }

    private void CreateTasks(bool recreate)
    {
        if (recreate)
        {
            TasksTemplateContainer.Controls.Clear();
            _tasksTemplateContainer = null;
            ClearTasksTemplateContainer();
        }
        if (_tasksTemplateContainer == null)
        {
            _tasksTemplateContainer = CreateTasksTemplateContainer();
            if (_tasksTemplate != null)
            {
                _tasksTemplate.InstantiateIn(_tasksTemplateContainer);
            }
            AddTasksTemplateContainer();
        }
        else if (TasksTemplate != null)
        {
            throw new InvalidOperationException(String.Format("Cannot instantiate the TasksTemplate in the Init event when the TasksTemplateContainer was already created manually in CompositeTaskPanel with ID '{0}'.",
                ID));
        }
    }
    #endregion

    #region Events
    //Event that is raised by taskpanel control on task assignment
    [
       Category("Action"),
       Description("Task assign event"),
    ]
    public event EventHandler<TaskEventArgs> TaskAssign;
    protected void OnTaskAssign(string xml, bool assignToAll)
    {
        EventHandler<TaskEventArgs> temp = TaskAssign;
        if (temp != null)
        {
            temp(this, new TaskEventArgs(xml, assignToAll));
        }
    }
    #endregion

    #region Tasks
    //signalizes that tasks where enumerated
    private bool enumeratedTasks = false;

    //find all child controls that implement ITask interface
    //don't enumerate if it was already done
    protected void EnumerateTasks()
    {
        EnumerateTasks(false);
    }
    //find all child controls that implement ITask interface
    //if force = false don't enumerate? if it was already done 
    protected void EnumerateTasks(bool force)
    {
        if (enumeratedTasks && !force)
        {
            return;
        }
        tasks = new List<ITask>();
        if (TasksPlaceHolder.Controls != null)
        {
            EnumerateTasksRecursive(TasksPlaceHolder.Controls);
        }
        enumeratedTasks = true;
    }

    //recursive function that helps to enumerate all child controls that implement ITask
    protected void EnumerateTasksRecursive(ControlCollection controls)
    {
        foreach (Control next in controls)
        {
            if (next.Controls != null)
            {
                EnumerateTasksRecursive(next.Controls);
            }
            ITask task = next as ITask;
            if (task != null)
            {
                tasks.Add(task);
            }
        }
    }

    private List<ITask> tasks = null;
    #endregion

    #region Helper Methods
    //returns xml string for active task
    private string ActiveTaskXml()
    {
        ITask activeTask = ActiveTask();
        if (activeTask == null)
        {
            throw new InvalidOperationException(String.Format(
                "CompositeTaskPanel {0} is trying to assign task when no task is active.", ClientID));
        }
        return activeTask.GetXmlString();
    }

    //returns active ITask if any
    private ITask ActiveTask()
    {
        EnumerateTasks();
        foreach (ITask task in tasks)
        {
            if (task.IsActive())
            {
                return task;
            }
        }
        return null;
    }
    #endregion

    #region Buttons
    protected void lbtnAssignTask_Click(object sender, EventArgs e)
    {
        OnTaskAssign(ActiveTaskXml(), false);
    }

    protected void lbtnAssignTaskToAll_Click(object sender, EventArgs e)
    {
        OnTaskAssign(ActiveTaskXml(), true);
    }

    //enable/disable assign task buttons
    private void SetEnabledAssignTaskButtons(bool enabled)
    {
        if (enabled)
        {
            divAssignTask.Attributes["class"] = "Button";
            divAssignTaskToAll.Attributes["class"] = "Button";
            lbtnAssignTask.OnClientClick = "";
            lbtnAssignTaskToAll.OnClientClick = "";
        }
        else
        {
            divAssignTask.Attributes["class"] = "ButtonDisabled";
            divAssignTaskToAll.Attributes["class"] = "ButtonDisabled";
            lbtnAssignTask.OnClientClick = "return false;";
            lbtnAssignTaskToAll.OnClientClick = "return false;";
        }
    }
    #endregion
}