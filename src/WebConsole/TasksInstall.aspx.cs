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
using ARM2_dbcontrol.Filters;
using VirusBlokAda.Vba32CC.DataBase;
using ARM2_dbcontrol.Service.TaskAssignment;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ARM2_dbcontrol.Tasks;
using Filters.Composite;
using Filters.Common;
using VirusBlokAda.Vba32CC.Common;

public partial class TasksInstall : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ExportToExcel1.InitializeExportToExcel(GridView1, InformationListTypes.TasksInstall);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitFields();
        }
        Controls_PagerUserControl.AddGridViewExtendedAttributes(GridView1, ObjectDataSource1);
    }

    protected override void InitFields()
    {
        fltStatus.DataSource = InstallTasksDataContainer.GetStatuses();
        fltStatus.DataBind();
        fltTaskType.DataSource = InstallTasksDataContainer.GetTaskTypes();
        fltTaskType.DataBind();
    }

    protected void FilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    {
        GridView1.PageIndex = 0;
        GridView1.Where = e.Where;
        GridView1.DataBind();
    }

    protected void AutoUpdateControl1_AutoUpdate(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
}