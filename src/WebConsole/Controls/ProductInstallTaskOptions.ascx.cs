using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirusBlokAda.CC.Tasks.Common;
using VirusBlokAda.CC.Tasks.Entities;

public partial class Controls_ProductInstallTaskOptions : System.Web.UI.UserControl, ITaskOptions, ITaskOptionsHelper<InstallProductTaskEntity>
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
            return tskInstallProduct.ClientID;
        }
    }

    public Type TaskType
    {
        get { return typeof(InstallProductTaskEntity); }
    }
    #endregion

    #region ITaskOptionsHelper
    public void LoadTaskEntity(InstallProductTaskEntity entity)
    {
        tboxArguments.Text = entity.CommandLine;
        ddlProduct.SelectedIndex = entity.SelectedIndex;
    }

    public InstallProductTaskEntity SaveTaskEntity(InstallProductTaskEntity oldEntity, out bool changed)
    {
        InstallProductTaskEntity entity = new InstallProductTaskEntity();
        entity.CommandLine = tboxArguments.Text;
        entity.SelectedIndex=ddlProduct.SelectedIndex;
        changed = !oldEntity.Equals(entity);
        return entity;
    }

    public InstallProductTaskEntity ConvertTaskEntity(TaskEntity entity)
    {
        InstallProductTaskEntity processEntity = entity as InstallProductTaskEntity;
        if ((entity as InstallProductTaskEntity) == null)
        {
            processEntity = new InstallProductTaskEntity();
        }
        return processEntity;
    }
    #endregion

}