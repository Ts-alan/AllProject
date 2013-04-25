using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tasks.Common;
using Tasks.Entities;

/// <summary>
/// Control that shows CreateProcess task options(see TestTaskOptions) 
/// !Update markup
/// </summary>
public partial class Controls_CreateProcessTaskOptions : System.Web.UI.UserControl, ITaskOptions, ITaskOptionsHelper<CreateProcessTaskEntity>
{
    #region Page LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion

    #region Control LifeCycle
    protected override void OnInit(EventArgs e)
    {
        Visible = false;
        base.OnInit(e);
    }

    #endregion

    #region ITaskOptions
    public void LoadTaskEntity(TaskEntity entity)
    {
        LoadTaskEntity(ConvertTaskEntity(entity));
    }

    public TaskEntity SaveTaskEntity(TaskEntity oldEntity, out bool changed)
    {
        return SaveTaskEntity(ConvertTaskEntity(oldEntity), out changed);
    }

    public string DivOptionsClientID
    {
        get
        {
            return tskCreateProcess.ClientID;
        }
    }

    public Type TaskType
    {
        get { return typeof(CreateProcessTaskEntity); }
    }
    #endregion

    #region ITaskOptionsHelper
    public void LoadTaskEntity(CreateProcessTaskEntity entity)
    {
        tboxCreateProcess.Text = entity.CommandLine;
        cboxCommand.Checked = entity.CommandSpec;
    }

    public CreateProcessTaskEntity SaveTaskEntity(CreateProcessTaskEntity oldEntity, out bool changed)
    {
        CreateProcessTaskEntity entity = new CreateProcessTaskEntity();
        entity.CommandLine = tboxCreateProcess.Text;
        entity.CommandSpec = cboxCommand.Checked;
        changed = !oldEntity.Equals(entity);
        return entity;
    }

    public CreateProcessTaskEntity ConvertTaskEntity(TaskEntity entity)
    {
        CreateProcessTaskEntity processEntity = entity as CreateProcessTaskEntity;
        if ((entity as CreateProcessTaskEntity) == null)
        {
            processEntity = new CreateProcessTaskEntity();
        }
        return processEntity;
    }
    #endregion


}