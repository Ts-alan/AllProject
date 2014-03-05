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
using ARM2_dbcontrol.Filters;
using VirusBlokAda.RemoteOperations.Wmi;
using VirusBlokAda.RemoteOperations.Common;

using Tasks.Common;
using Tasks.Entities;

public partial class Controls_UninstallTaskOptions : System.Web.UI.UserControl, ITaskOptions, ITaskOptionsHelper<UninstallTaskEntity>
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
            return tskUninstall.ClientID;
        }
    }
    public Type TaskType
    {
        get { return typeof(UninstallTaskEntity); }
    }
    #endregion

    #region ITaskOptionsHelper
    public void LoadTaskEntity(UninstallTaskEntity entity)
    {

        ddlProduct.SelectedIndex = entity.SelectedIndex;
    }

    public UninstallTaskEntity SaveTaskEntity(UninstallTaskEntity oldEntity, out bool changed)
    {
        UninstallTaskEntity entity = new UninstallTaskEntity();

        entity.SelectedIndex = ddlProduct.SelectedIndex;
        changed = !oldEntity.Equals(entity);
        return entity;
    }

    public UninstallTaskEntity ConvertTaskEntity(TaskEntity entity)
    {
        UninstallTaskEntity processEntity = entity as UninstallTaskEntity;
        if ((entity as UninstallTaskEntity) == null)
        {
            processEntity = new UninstallTaskEntity();
        }
        return processEntity;
    }
    #endregion

   /* 
    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
        rbtnlProviders.Items.Add(Resources.Resource.WMI);
        rbtnlProviders.Items.Add(Resources.Resource.RemoteService);
        rbtnlProviders.SelectedIndex = 0;
    }

    public bool DoRestart
    {
        get 
        {
            return cbRebootAfterInstall.Checked;
        }
    }

    public RemoteMethodsEnum Provider
    {
        get 
        {
            if (rbtnlProviders.SelectedValue== Resources.Resource.RemoteService)
            {
                return RemoteMethodsEnum.RemoteService;
            }
            return RemoteMethodsEnum.Wmi;
        }
    }

    public TimeSpan PollingTime
    {
        get 
        {
            return new TimeSpan(0, 0, 10);

            //if (String.IsNullOrEmpty(tbPollingTime.Text))
            //{
            //    return new TimeSpan(0, 0, 10);
            //}
            //else
            //{
            //    return new TimeSpan(0, 0, int.Parse(tbPollingTime.Text));
            //} 
        }
    }

    public TimeSpan Timeout
    {
        get
        {
            return new TimeSpan(0, 10, 0);
        }
    }

    public bool ValidateFields()
    {
        if (String.IsNullOrEmpty(tboxLogin.Text))
        {
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.Login);
        }
        if (String.IsNullOrEmpty(tboxPassword.Text))
        {
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.PasswordLabelText);
        }
        if (String.IsNullOrEmpty(tboxDomain.Text))
        {
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.DomainName);
        }
        return true;
    }*/

}
