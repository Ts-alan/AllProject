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

using System.Text;
using System.Collections.Generic;

using ARM2_dbcontrol.Generation;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Filters;

/// <summary>
/// Задача настройки Диспетчера
/// </summary>
public partial class Controls_TaskConfigureLoader : System.Web.UI.UserControl,ITask
{
    private static TaskConfigureLoader loader;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack ||loader==null)
        {
            InitFields();
        }
    }

    private Boolean _hideHeader = false;
    public Boolean HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    private Boolean _enabled = true;
    public Boolean Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }
    
    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;
        SetEnabled();

  /*      if (loader == null)
        {
            loader = new TaskConfigureLoader();
            loader.Vba32CCUser = Anchor.GetStringForTaskGivedUser();
        }
        else
        {
            loader.Clear();
        }
        if (loader.UPDATE_FOLDER_LIST == null)
        {
            loader.UPDATE_FOLDER_LIST = new List<string>();
        }*/
        loader = new TaskConfigureLoader();
        PathUpdateData();
    }

    private void SetEnabled()
    {
        lboxUpdatePathes.Enabled = _enabled;
        lbtnUpdateMoveUP.Enabled = lbtnUpdateMoveDown.Enabled = _enabled;
        lbtnUpdateAdd.Enabled = lbtnUpdateChange.Enabled = lbtnUpdateDelete.Enabled = _enabled;
       // lbtnUpdateAdd.Attributes["disabled"] = (!_enabled).ToString();
        cboxAuthorizationEnabled.Enabled = cboxProxyAuthorizationEnabled.Enabled = cboxProxyEnabled.Enabled = _enabled;
        ddlProxyType.Enabled = _enabled;
        tboxProxyAddress.Enabled = tboxProxyPort.Enabled = tboxProxyAuthorizationPassword.Enabled = tboxProxyAuthorizationUserName.Enabled = _enabled;
        tboxAuthorizationPassword.Enabled = tboxAuthorizationUserName.Enabled = _enabled;

    }

    public Boolean ValidateFields()
    {
        return true;
    }
   
    private void LoadLoader()
    {
        cboxProxyEnabled.Checked = loader.PROXY_USAGE;
        ddlProxyType.SelectedValue=((Int32)loader.PROXY_TYPE).ToString();
        tboxProxyAddress.Text = loader.PROXY_ADDRESS;
        tboxProxyPort.Text = loader.PROXY_PORT.ToString();
        tboxAuthorizationPassword.Text = loader.AUTH_PASSWORD;
        tboxAuthorizationPassword.Attributes["Value"] = loader.AUTH_PASSWORD;
        tboxAuthorizationUserName.Text = loader.AUTH_USER;
        tboxProxyAuthorizationPassword.Text = loader.PROXY_PASSWORD;
        tboxProxyAuthorizationPassword.Attributes["Value"] = loader.PROXY_PASSWORD;
        tboxProxyAuthorizationUserName.Text = loader.PROXY_USER;
        cboxAuthorizationEnabled.Checked = loader.AUTHORIZE;
        cboxProxyAuthorizationEnabled.Checked = loader.PROXY_AUTHORIZE;

        UpdateEnabledControls();
        PathUpdateData();
    }

    public void SaveLoader()
    {
        loader.AUTHORIZE = cboxAuthorizationEnabled.Checked;
        loader.AUTH_PASSWORD = tboxAuthorizationPassword.Text;
        loader.AUTH_USER = tboxAuthorizationUserName.Text;

        loader.PROXY_USAGE = cboxProxyEnabled.Checked;
        loader.PROXY_TYPE = (ProxyType)Int32.Parse(ddlProxyType.SelectedValue);
        loader.PROXY_ADDRESS = tboxProxyAddress.Text;
        loader.PROXY_PORT = Convert.ToInt32(tboxProxyPort.Text);

        loader.PROXY_AUTHORIZE = cboxProxyAuthorizationEnabled.Checked;
        loader.PROXY_USER = tboxProxyAuthorizationUserName.Text;
        loader.PROXY_PASSWORD = tboxProxyAuthorizationPassword.Text;
    }

    private void UpdateEnabledControls()
    {
        tboxProxyAddress.Enabled = cboxProxyEnabled.Checked;
        tboxProxyPort.Enabled = cboxProxyEnabled.Checked;
        ddlProxyType.Enabled = cboxProxyEnabled.Checked;
        cboxProxyAuthorizationEnabled.Enabled = cboxProxyEnabled.Checked;
        tboxProxyAuthorizationPassword.Enabled = cboxProxyEnabled.Checked && cboxProxyAuthorizationEnabled.Checked;
        tboxProxyAuthorizationUserName.Enabled = cboxProxyEnabled.Checked && cboxProxyAuthorizationEnabled.Checked;

        tboxAuthorizationPassword.Enabled = cboxAuthorizationEnabled.Checked;
        tboxAuthorizationUserName.Enabled = cboxAuthorizationEnabled.Checked;
    }

    /// <summary>
    /// Load state
    /// </summary>
    /// <param name="task"></param>
    public void LoadState(TaskUserEntity task)
    {
        if (task == null || task.Type != TaskType.ConfigureLoader)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);
        loader.LoadFromXml(task.Param);
        LoadLoader();
    }

    /// <summary>
    /// Get current state
    /// </summary>
    /// <returns></returns>
    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ConfigureLoader;

        SaveLoader();
        ValidateFields();
        task.Param = loader.SaveToXml();

        return task;
    }

    public String BuildTask()
    {
        SaveLoader();
        return loader.GetTask();
    }


    #region UpdatePath
    
    private void PathUpdateData()
    {
        lboxUpdatePathes.Items.Clear();
        foreach (String str in loader.UPDATE_FOLDER_LIST)
            lboxUpdatePathes.Items.Add(str);
    }

    protected void lbtnAddUpdatePathDialogApply_Click(object sender, EventArgs e)
    {
        loader.UPDATE_FOLDER_LIST.Add(tboxAddDialogUpdatePath.Text);
        PathUpdateData();
    }

    protected void lbtnUpdateDelete_Click(object sender, EventArgs e)
    {
        Int32 index = lboxUpdatePathes.SelectedIndex;
        if (index >= 0)
        {
            loader.UPDATE_FOLDER_LIST.RemoveAt(index);
            PathUpdateData();
        }
    }

    protected void lbtnUpdatePathChange_Click(object sender, EventArgs e)
    {
        Int32 index = lboxUpdatePathes.SelectedIndex;
        if (index >= 0)
        {
            loader.UPDATE_FOLDER_LIST[index] = tboxAddDialogUpdatePath.Text;
            PathUpdateData();
        }
    }

    protected void lbtnUpdateMoveUP_Click(object sender, EventArgs e)
    {
        Int32 index = lboxUpdatePathes.SelectedIndex;
        String path;
        if (index > 0)
        {
            path = loader.UPDATE_FOLDER_LIST[index];
            loader.UPDATE_FOLDER_LIST[index] = loader.UPDATE_FOLDER_LIST[index - 1];
            loader.UPDATE_FOLDER_LIST[index-1] = path;
            
            PathUpdateData();
        }
    }

    protected void lbtnUpdateMoveDown_Click(object sender, EventArgs e)
    {
        Int32 index = lboxUpdatePathes.SelectedIndex;
        String path;
        if (index >=0 && index < loader.UPDATE_FOLDER_LIST.Count - 1)
        {
            path = loader.UPDATE_FOLDER_LIST[index];
            loader.UPDATE_FOLDER_LIST[index] = loader.UPDATE_FOLDER_LIST[index + 1];
            loader.UPDATE_FOLDER_LIST[index + 1] = path;
            PathUpdateData();
        }
    }
    
    #endregion
}
