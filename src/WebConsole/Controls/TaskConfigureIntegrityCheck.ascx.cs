using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Service.Vba32NS;
using System.Data;
using System.Text;
using System.Xml;
using System.IO;
using ARM2_dbcontrol.Tasks.ConfigureIntegrityCheck;

public partial class Controls_TaskConfigureIntegrityCheck : System.Web.UI.UserControl, ITask
{
    private static TaskConfigureIntegrityCheck taskIntegrityCheck;

    private Int32 FilesAddedCount = 0;
    private Int32 RegistryAddedCount = 0;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    public void InitFields()
    {
        taskIntegrityCheck = new TaskConfigureIntegrityCheck();
        FilesUpdateData();
        RegistryUpdateData();
    }

    public bool ValidateFields()
    {
        return true;
    }

    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();
        task.Type = TaskType.ConfigureIntegrityCheck;
        task.Param = taskIntegrityCheck.SaveToXml(); 
        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        taskIntegrityCheck.LoadFromXml(task.Param);
        FilesUpdateData();
        RegistryUpdateData();
    }
        
    public String BuildTask()
    {
        return taskIntegrityCheck.GetTask();
    }

    #region Files
    private void FilesUpdateData()
    {
        FilesDataList.DataSource = taskIntegrityCheck.Files;
        FilesDataList.DataBind();
    }
    protected void FilesAddDialogApplyButtonClick(object sender, EventArgs e)
    {

        IntegrityCheckFilesEntity info = new IntegrityCheckFilesEntity();
        info.Path = FilesAddDialogPath.Text;
        info.Template = FilesAddDialogTemplate.Text;
        taskIntegrityCheck.Files.Add(info);
        FilesUpdateData();
    }
    protected void FilesDeleteButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(FileshdnActiveRowNo.Value);
        if (index != 0)
        {
            taskIntegrityCheck.Files.RemoveAt(index - 1);
            FileshdnActiveRowNo.Value = Convert.ToString(0);
            FilesUpdateData();
        }
    }
    protected void FilesChangeButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(FileshdnActiveRowNo.Value);
        if (index != 0)
        {
            IntegrityCheckFilesEntity info = taskIntegrityCheck.Files[index - 1];
            info.Path = FilesAddDialogPath.Text;
            info.Template = FilesAddDialogTemplate.Text;
            taskIntegrityCheck.Files.Insert(index - 1, info);
            taskIntegrityCheck.Files.RemoveAt(index);
            FilesUpdateData();
        }
    } 

    protected void FilesDataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        IntegrityCheckFilesEntity info;
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            info = (IntegrityCheckFilesEntity)e.Item.DataItem;
            (e.Item.FindControl("FileshdnRowNo") as HiddenField).Value = (++FilesAddedCount).ToString();
            (e.Item.FindControl("lblFilesPath") as Label).Text = info.Path;
            (e.Item.FindControl("lblFilesTemplate") as Label).Text = info.Template;           
            if (e.Item.ItemIndex % 2 == 0)
                (e.Item.FindControl("trFilesItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRowAlternating");
            else
                (e.Item.FindControl("trFilesItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRow");
        }
    }

    protected void FilesDataList_SelectedIndexChanged(object sender, DataListCommandEventArgs e)
    {
        FilesDataList.EditItemIndex = e.Item.ItemIndex;
        FilesDataList.SelectedIndex = e.Item.ItemIndex;
        FilesUpdateData();
    }

    #endregion

    #region Registry

    private void RegistryUpdateData()
    {
        RegistryDataList.DataSource = taskIntegrityCheck.Registries;
        RegistryDataList.DataBind();
    }

    protected void RegistryAddDialogApplyButtonClick(object sender, EventArgs e)
    {
       String path = RegistryAddDialogPath.Text;
       IntegrityCheckRegistryEntity reg = new IntegrityCheckRegistryEntity();
       reg.Path = path;
       taskIntegrityCheck.Registries.Add(reg);
       RegistryUpdateData();
    }

    protected void RegistryDeleteButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(RegistryhdnActiveRowNo.Value);
        if (index != 0)
        {
            taskIntegrityCheck.Registries.RemoveAt(index - 1);
            RegistryhdnActiveRowNo.Value = Convert.ToString(0);
            RegistryUpdateData();
        }
    }

    protected void RegistryChangeButtonClick(object sender, EventArgs e)
    {
        Int32 index = Convert.ToInt32(RegistryhdnActiveRowNo.Value);
        if (index != 0)
        {
            IntegrityCheckRegistryEntity reg = taskIntegrityCheck.Registries[index - 1];
            reg.Path = RegistryAddDialogPath.Text;
            taskIntegrityCheck.Registries.Insert(index - 1, reg);
            taskIntegrityCheck.Registries.RemoveAt(index);
            RegistryUpdateData();
        }
    }

    protected void RegistryDataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        IntegrityCheckRegistryEntity reg;
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            reg = (IntegrityCheckRegistryEntity)e.Item.DataItem;
            String info = reg.Path;
            (e.Item.FindControl("RegistryhdnRowNo") as HiddenField).Value = (++RegistryAddedCount).ToString();
            (e.Item.FindControl("lblRegistryPath") as Label).Text = info;
            if (e.Item.ItemIndex % 2 == 0)
                (e.Item.FindControl("trRegistryItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRowAlternating");
            else
                (e.Item.FindControl("trRegistryItem") as System.Web.UI.HtmlControls.HtmlTableRow).Attributes.Add("class", "gridViewRow");
        }
    }

    protected void RegistryDataList_SelectedIndexChanged(object sender, DataListCommandEventArgs e)
    {
        RegistryDataList.EditItemIndex = e.Item.ItemIndex;
        RegistryDataList.SelectedIndex = e.Item.ItemIndex;
        RegistryUpdateData();
    }

    #endregion

}