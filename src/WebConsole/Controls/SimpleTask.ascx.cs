using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirusBlokAda.CC.Tasks.Simple;
using VirusBlokAda.CC.Tasks.Entities;
using VirusBlokAda.CC.Tasks.Common;

/// <summary>
/// Control that represents simple tasks - task without options
/// </summary>
public partial class Controls_SimpleTask : System.Web.UI.UserControl, ITask
{
    #region Page LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScripts();
    }

    private void RegisterScripts()
    {
        //register helper script
        Page.ClientScript.RegisterClientScriptInclude("TaskHelper", @"js/TaskHelper.js");
    }
    #endregion

    #region Control LifeCycle
    #region Init
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        GenerateStorageName();
    }

    private void GenerateStorageName()
    {
        //generate name of storage where control state is stored
        SimpleTaskStateStorage.StorageName = TaskType.ToString();
    }
    #endregion

    #region Load
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        //load state from storage
        LoadCurrentSimpleTaskState();
        if (!IsPostBack)
        {
            InitFields();
            //first time on this page load from state
            LoadTaskControlsState();
        }
        else
        {
            //save to state
            ChangeCurrentSimpleTaskState();
        }
    }

    private void ChangeCurrentSimpleTaskState()
    {
        //store in state that this task was selected
        currentSimpleTaskState.IsSelected = rbtnUseTask.Checked;
    }

    private void LoadTaskControlsState()
    {
        //load from state if this task was selected
        rbtnUseTask.Checked = currentSimpleTaskState.IsSelected;
    }

    protected void InitFields()
    {
        lblName.Text = TaskName;
    }
    #endregion
    #endregion

    #region ITask
    public bool IsActive()
    {
        //return if this task is active
        return currentSimpleTaskState.IsActive();
    }

    public string GetXmlString()
    {
        //return xml string corresponding to this task
        return currentSimpleTaskState.GetXmlString();
    }

    public string GetTaskXml()
    {
        return currentSimpleTaskState.GetTaskXml();
    }

    public string GetTaskName()
    {
        return currentSimpleTaskState.GetTaskName();
    }
    #endregion

    #region Task State
    private SimpleTaskState currentSimpleTaskState;

    private void LoadCurrentSimpleTaskState()
    {
        //try to load state from storage
        currentSimpleTaskState = SimpleTaskStateStorage.Storage as SimpleTaskState;
        if (currentSimpleTaskState == null)
        {
            //state was not present in storage
            //create new state, and store it
            currentSimpleTaskState = new SimpleTaskState(_taskType);
            SimpleTaskStateStorage.Storage = currentSimpleTaskState;
        }
    }
    #endregion

    #region Public Properties
    private String _taskName = String.Empty;
    //Simple task name
    public String TaskName
    {
        get { return _taskName; }
        set { _taskName = value; }
    }

    //Simple task type
    private SimpleTaskEntityEnum _taskType = SimpleTaskEntityEnum.None;
    public SimpleTaskEntityEnum TaskType
    {
        get { return _taskType; }
        set { _taskType = value; }
    }
    #endregion



}