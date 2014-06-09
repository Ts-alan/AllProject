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
    private Int32 UpdatePathAddedCount = 0;
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack || loader == null)
        {
            InitFields();
        }
        else
        {
            LoadLoader();
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
        loader = new TaskConfigureLoader();
        PathUpdateData();
        UpdatePathHdnActiveRowNo.Value ="0";
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

        UpdatePathAddedCount = 0;
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

        loader.PROXY_AUTHORIZE = cboxAuthorizationEnabled.Checked;
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
        return loader.GetTask();
    }


    #region UpdatePath
    private void PathUpdateData()
    {
        UpdatePathAddedCount = 0;
        CongLdrUpdateDataList.DataSource = loader.UPDATE_FOLDER_LIST;
        CongLdrUpdateDataList.DataBind();
    }
    protected void CongLdrUpdateDataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        String path;
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            path = (String)e.Item.DataItem;
            (e.Item.FindControl("hdnUpdateRowNo") as HiddenField).Value = (++UpdatePathAddedCount).ToString();
            (e.Item.FindControl("CongLdrUpdatePath") as Label).Text = path;

            if (e.Item.ItemIndex % 2 == 0)
                (e.Item.FindControl("trUpdateItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRowAlternating");
            else
                (e.Item.FindControl("trUpdateItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRow");
        }
    }
    protected void CongLdrUpdateDataList_SelectedIndexChanged(object sender, DataListCommandEventArgs e)
    {
        CongLdrUpdateDataList.EditItemIndex = e.Item.ItemIndex;
        CongLdrUpdateDataList.SelectedIndex = e.Item.ItemIndex;
        PathUpdateData();
    }
    protected void AddUpdatePathDialogApplyButtonClick(object sender, EventArgs e)
    {
        String path = AddDialogUpdatePath.Text;
        loader.UPDATE_FOLDER_LIST.Add(path);
        PathUpdateData();
    }
    protected void UpdatePathDeleteButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(UpdatePathHdnActiveRowNo.Value);
        if (index != 0)
        {
            loader.UPDATE_FOLDER_LIST.RemoveAt(index - 1);
            UpdatePathHdnActiveRowNo.Value = Convert.ToString(0);
            PathUpdateData();
        }
    }
    protected void UpdatePathChangeButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(UpdatePathHdnActiveRowNo.Value);
        if (index != 0)
        {
            String path = loader.UPDATE_FOLDER_LIST[index - 1];
            path = AddDialogUpdatePath.Text;
            loader.UPDATE_FOLDER_LIST.Insert(index - 1, path);
            loader.UPDATE_FOLDER_LIST.RemoveAt(index);
            PathUpdateData();
        }
    }
    protected void UpdatePathMoveUpButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(UpdatePathHdnActiveRowNo.Value);
        String path;
        if (index >1)
        {
            path = loader.UPDATE_FOLDER_LIST[index - 1];
            loader.UPDATE_FOLDER_LIST.RemoveAt(index - 1);
            loader.UPDATE_FOLDER_LIST.Insert(index - 2, path);
            
            UpdatePathHdnActiveRowNo.Value = Convert.ToString(index-1);
            PathUpdateData();
        }
    }
    protected void UpdatePathMoveDownButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(UpdatePathHdnActiveRowNo.Value);
        String path;
        if (index !=0 && index<loader.UPDATE_FOLDER_LIST.Count)
        {
            path = loader.UPDATE_FOLDER_LIST[index - 1];
            loader.UPDATE_FOLDER_LIST.RemoveAt(index - 1);
            loader.UPDATE_FOLDER_LIST.Insert(index, path);

            UpdatePathHdnActiveRowNo.Value = Convert.ToString(index+1);
            PathUpdateData();
        }
    }
    #endregion
}
