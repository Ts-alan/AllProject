using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirusBlokAda.CC.Tasks.Common;
using VirusBlokAda.CC.Tasks.Entities;

/// <summary>
/// Control that shows RestoreFileFromQtn task options(see TestTaskOptions) 
/// !Update markup
/// </summary>
public partial class Controls_RestoreFileFromQtnTaskOptions : System.Web.UI.UserControl, ITaskOptions, ITaskOptionsHelper<RestoreFileFromQtnTaskEntity>
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
            return tskRestoreFileFromQtn.ClientID;
        }
    }

    public Type TaskType
    {
        get { return typeof(RestoreFileFromQtnTaskEntity); }
    }
    #endregion

    #region ITaskOptionsHelper
    public void LoadTaskEntity(RestoreFileFromQtnTaskEntity entity)
    {
        tboxRestoreFileFromQtn.Text = entity.FullPath;
    }

    public RestoreFileFromQtnTaskEntity SaveTaskEntity(RestoreFileFromQtnTaskEntity oldEntity, out bool changed)
    {
        RestoreFileFromQtnTaskEntity entity = new RestoreFileFromQtnTaskEntity();
        entity.FullPath = tboxRestoreFileFromQtn.Text;
        changed = !oldEntity.Equals(entity);
        return entity;
    }

    public RestoreFileFromQtnTaskEntity ConvertTaskEntity(TaskEntity entity)
    {
        RestoreFileFromQtnTaskEntity processEntity = entity as RestoreFileFromQtnTaskEntity;
        if ((entity as RestoreFileFromQtnTaskEntity) == null)
        {
            processEntity = new RestoreFileFromQtnTaskEntity();
        }
        return processEntity;
    }
    #endregion

}