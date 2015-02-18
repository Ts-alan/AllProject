using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirusBlokAda.CC.Tasks.Common;
using VirusBlokAda.CC.Tasks.Entities;

public partial class Controls_ConfigurePasswordTaskOptions : System.Web.UI.UserControl, ITaskOptions, ITaskOptionsHelper<ConfigurePasswordTaskEntity>
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
            
            return tskConfigurePassword.ClientID;
        }
    }

    public Type TaskType
    {
        get { return typeof(ConfigurePasswordTaskEntity); }
    }
    #endregion

    #region ITaskOptionsHelper
    public void LoadTaskEntity(ConfigurePasswordTaskEntity entity)
    {
        tboxPassword.Text = entity.Password;

    }

    public ConfigurePasswordTaskEntity SaveTaskEntity(ConfigurePasswordTaskEntity oldEntity, out bool changed)
    {
        ConfigurePasswordTaskEntity entity = new ConfigurePasswordTaskEntity();
        entity.Password = tboxPassword.Text;
        changed = !oldEntity.Equals(entity);
        return entity;
    }

    public ConfigurePasswordTaskEntity ConvertTaskEntity(TaskEntity entity)
    {
        ConfigurePasswordTaskEntity processEntity = entity as ConfigurePasswordTaskEntity;
        if ((entity as ConfigurePasswordTaskEntity) == null)
        {
            processEntity = new ConfigurePasswordTaskEntity();
        }
        return processEntity;
    }
    #endregion
}