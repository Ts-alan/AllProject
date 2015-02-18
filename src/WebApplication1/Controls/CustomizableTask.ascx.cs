using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirusBlokAda.CC.Tasks.Entities;
using VirusBlokAda.CC.Tasks.Customizable;
using VirusBlokAda.CC.Tasks.Common;
using System.Threading;
using System.Text;

/// <summary>
/// Control Customizable Task - control for tasks with options
/// !two many hiddenfields/other elements are passed to TaskOptionsDialog
/// !use JSON instead
/// </summary>
public partial class Controls_CustomizableTask : System.Web.UI.UserControl, ITask
{
    #region Page LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScripts();
    }

    private void RegisterScripts()
    {
        Page.ClientScript.RegisterClientScriptInclude("TaskHelper", @"js/TaskHelper.js");
        if (!IsPostBack)
        {
            //Add to TaskOptionsDialog 
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(),
                String.Format("CustomizableTask_{0}_Startup", ClientID),
                String.Format("TaskOptionsDialog.add_TaskOptionsControl('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}' );", 
                    ibtnCustomize.ClientID, TaskOptions.DivOptionsClientID, btnHiddenApply.ClientID, 
                    btnHiddenSave.ClientID, btnHiddenSaveAs.ClientID, hfSaveAsName.ClientID, hfUsedNames.ClientID, 
                    hfRestrictedNames.ClientID, ddlTasks.ClientID, hfTemporaryTaskKey.ClientID), true);
        }
       
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
        //set storage name for state 
        CustomizableTaskStateStorage.StorageName = ID;
    }
    #endregion

    #region Load
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        //load state from storage
        LoadCurrentCustomizableTaskState();
        if (!IsPostBack)
        {
            InitFields();
            InitHFTemproraryTaskKey();
            InitHFTemproraryTaskIsClear();
            //first time on this page load from state
            LoadTaskControlsState();
        }
        else 
        {
            //save to state
            ChangeCurrentSimpleTaskState();
        }
    }

    protected void InitFields()
    {
        lblName.Text = TaskName;        
    }

    private void InitHFTemproraryTaskKey()
    {
        hfTemporaryTaskKey.Value = currentCustomizableTaskState.GetTemproraryTaskKey();
    }

    private void LoadTaskControlsState()
    {
        //load checkbox and dropdown list values from state
        rbtnUseTask.Checked = currentCustomizableTaskState.IsSelected;
        InitStateDependantControls();
        ddlTasks.SelectedValue = currentCustomizableTaskState.SelectedTaskKey;
    }

    private void ChangeCurrentSimpleTaskState()
    {
        //save checkbox and dropdown list values to state
        currentCustomizableTaskState.IsSelected = rbtnUseTask.Checked;
        currentCustomizableTaskState.SelectedTaskKey = ddlTasks.SelectedValue;
    }
    #endregion

    #region PreRender
    protected override void OnPreRender(EventArgs e)
    {       
        base.OnPreRender(e);
        SetImageUrls();
        SetJavaScriptEvents();
    }

    private void SetImageUrls()
    {
        //set images for Delete and Customize image buttons depending on selected task
        //and if temporary task is selected and its options are not set
        if (ddlTasks.SelectedValue == currentCustomizableTaskState.GetTemproraryTaskKey())
        {            
            if (currentCustomizableTaskState.IsTemporaryTaskClear)
            {
                ibtnCustomize.ImageUrl = "~/" + TaskIcons.OptionsDisabled;
                ibtnDelete.ImageUrl = "~/" + TaskIcons.EraseDisabled;
                ibtnDelete.OnClientClick = "function () { return false; };";
            }
            else {
                ibtnCustomize.ImageUrl = "~/" + TaskIcons.OptionsEnabled;
                ibtnDelete.ImageUrl = "~/" + TaskIcons.Erase;
                ibtnDelete.OnClientClick = String.Empty;
            }
        }
        else {
            ibtnCustomize.ImageUrl = "~/" + TaskIcons.OptionsEnabled;
            ibtnDelete.ImageUrl = "~/" + TaskIcons.Delete;
            ibtnDelete.OnClientClick = String.Empty;
        }        
        
    }

    private void SetJavaScriptEvents()
    {        
        ddlTasks.Attributes.Add("onchange", String.Format("ddlTasks_{0}_OnChange()", ClientID));
        rbtnUseTask.Attributes.Add("onclick", String.Format("rbtnUseTask_{0}_OnClick()", ClientID));
    }
    #endregion
    #endregion

    #region Task State
    private CustomizableTaskState currentCustomizableTaskState;

    private void LoadCurrentCustomizableTaskState()
    {
        //try to load state from storage
        currentCustomizableTaskState = CustomizableTaskStateStorage.Storage as CustomizableTaskState;
        if (currentCustomizableTaskState == null)
        {
            //state was not present in storage
            //create new state, and store it
            currentCustomizableTaskState = CustomizableTaskState.Load(TaskOptions.TaskType);
            CustomizableTaskStateStorage.Storage = currentCustomizableTaskState;
        }
    }
    #endregion

    #region Public Properties
    private String _taskName = String.Empty;
    public String TaskName
    {
        get { return _taskName; }
        set { _taskName = value; }
    }

    private String _taskOptionsID;
    public String TaskOptionsID
    {
        get { return _taskOptionsID; }
        set { _taskOptionsID = value; }
    }
    #endregion

    #region Private Properties
    private Control _taskOptionsControl;
    private Control TaskOptionsControl
    {
        get
        {
            //get TaskOptions control corresponding to this CustomizableTask
            if (_taskOptionsControl == null)
            {
                if (String.IsNullOrEmpty(_taskOptionsID))
                {
                    throw new HttpException(String.Format(
                        "TaskOptionsID property of CustomizableTask control {0} is null or empty.", ID));
                }
                _taskOptionsControl = ControlsExtensions.FindControl(this, _taskOptionsID);
                if (_taskOptionsControl == null)
                {
                    throw new HttpException(String.Format(
                        "Can't find corresponding TaskOptionsID {0} control", _taskOptionsID));
                }
            }
            return _taskOptionsControl;
        }
    }

    private ITaskOptions _taskOptions;
    protected ITaskOptions TaskOptions
    {
        get
        {
            //get ITaskOptions corresponding to this CustomizableTask
            if (_taskOptions == null)
            {
                _taskOptions = TaskOptionsControl as ITaskOptions;
                if (_taskOptions == null)
                {
                    throw new HttpException(String.Format(
                        "Corresponding TaskOptionsID {0} control does not implement ITaskOptions interface",
                        _taskOptionsID));
                }
            }
            return _taskOptions;
        }
    }


    #endregion

    #region Buttons
    protected void ibtnCustomize_Click(object sender, ImageClickEventArgs e)
    {
        //make TaskOptions control visible
        TaskOptionsControl.Visible = true;
        //load values in it
        TaskOptions.LoadTaskEntity(currentCustomizableTaskState.GetSelectedTask());
    }

    protected void ibtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlTasks.SelectedValue == currentCustomizableTaskState.GetTemproraryTaskKey())
        {
            //clear temporary task
            currentCustomizableTaskState.ClearTemporaryTask();
            InitHFTemproraryTaskIsClear();
        }
        else
        {
            //delete selected task
            currentCustomizableTaskState.DeleteStoredTask(currentCustomizableTaskState.SelectedTaskKey);
            InitStateDependantControls();
            //select temporary
            currentCustomizableTaskState.SelectedTaskKey = currentCustomizableTaskState.GetTemproraryTaskKey();
            ddlTasks.SelectedValue = currentCustomizableTaskState.GetTemproraryTaskKey();
        }
    }

    protected void btnHiddenApply_Click(object sender, EventArgs e)
    {
        // on apply if options were changed we want to store them in temporary task
        bool changed;
        TaskEntity entity = TaskOptions.SaveTaskEntity(currentCustomizableTaskState.GetSelectedTask(),
            out changed);
        if (changed)
        {
            currentCustomizableTaskState.SetTemporaryTask(entity);
            currentCustomizableTaskState.SelectedTaskKey =
                currentCustomizableTaskState.GetTemproraryTaskKey();
            InitStateDependantControls();
            ddlTasks.SelectedValue = currentCustomizableTaskState.GetTemproraryTaskKey();
            InitHFTemproraryTaskIsClear();
        }
    }

    protected void btnHiddenSave_Click(object sender, EventArgs e)
    {
        //on save if options were changed we store them in responsive name
        bool changed;
        TaskEntity entity = TaskOptions.SaveTaskEntity(currentCustomizableTaskState.GetSelectedTask(),
            out changed);
        if (changed)
        {
            currentCustomizableTaskState.SetStoredTask(ddlTasks.SelectedValue, entity);
        }

    }

    protected void btnHiddenSaveAs_Click(object sender, EventArgs e)
    {
        //on save as we create new task with given name and store there options
        string saveName = hfSaveAsName.Value;
        bool changed;
        TaskEntity entity = TaskOptions.SaveTaskEntity(null, out changed);
        currentCustomizableTaskState.SetStoredTask(saveName, entity);
        InitStateDependantControls();
        currentCustomizableTaskState.SelectedTaskKey = saveName;
        ddlTasks.SelectedValue = saveName;
    }
    #endregion

    #region Controls
    private void InitDDLTasks()
    {
        //clear drop down list
        ddlTasks.Items.Clear();
        //add temporary task to drop down
        ddlTasks.Items.Add(new ListItem(Resources.Resource.TemporaryTask,
            currentCustomizableTaskState.GetTemproraryTaskKey()));
        //add all names tasks to drop down
        foreach (string name in currentCustomizableTaskState.GetStoredTaskKeys())
        {
            ddlTasks.Items.Add(name);
        }
    }

    private void InitHFSpecificNames()
    {
        //don't allow saving as with name equal to TemproraryTaskKey and value stored in 
        //Resources.Resource.TemporaryTask
        hfRestrictedNames.Value = String.Format("['{0}', '{1}']", Resources.Resource.TemporaryTask, 
            currentCustomizableTaskState.GetTemproraryTaskKey());
        //ask to rewrite if name is already used
        if (currentCustomizableTaskState.GetStoredTaskKeys().Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            bool isFirst = true;
            foreach (string name in currentCustomizableTaskState.GetStoredTaskKeys())
            {
                if (!isFirst)
                {
                    sb.Append(", ");
                }
                else
                {
                    isFirst = false;
                }
                sb.AppendFormat("'{0}'", name);
            }
            sb.Append("]");
            hfUsedNames.Value = sb.ToString();
        }
    }

    private void InitHFTemproraryTaskIsClear()
    {
        hfTemporaryTaskIsClear.Value = currentCustomizableTaskState.IsTemporaryTaskClear ? "1" : "0";
    }

    private void InitStateDependantControls()
    {
        InitDDLTasks();
        InitHFSpecificNames();
    }
    #endregion

    #region ITask
    public bool IsActive()
    {
        //return if this task is active
        return currentCustomizableTaskState.IsActive();
    }

    public string GetXmlString()
    {
        //return xml string corresponding to this task
        return currentCustomizableTaskState.GetXmlString();
    }

    public string GetTaskXml()
    {
        return currentCustomizableTaskState.GetTaskXml();
    }

    public string GetTaskName()
    {
        return currentCustomizableTaskState.GetTaskName();
    }
    #endregion

}