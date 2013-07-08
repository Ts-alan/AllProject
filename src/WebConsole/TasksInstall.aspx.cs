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
using ARM2_dbcontrol.DataBase;
using ARM2_dbcontrol.Service.TaskAssignment;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ARM2_dbcontrol.Tasks;
using Filters.Composite;
using Filters.Common;
using ARM2_dbcontrol.Common;

public partial class TasksInstall : PageBase
{
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
        fltVBA32Version.DataSource = InstallTasksDataContainer.GetVba32Versions();
        fltVBA32Version.DataBind();

        lbtnConfigure.Text = Resources.Resource.Attach;
    }

    protected void FilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    {
        GridView1.PageIndex = 0;
        GridView1.Where = e.Where;
        GridView1.DataBind();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            InstallationTaskEntity _task = (InstallationTaskEntity)e.Row.DataItem;

            if (_task.Status != DatabaseNameLocalization.GetNameForCurrentCulture("Success"))
                (e.Row.Cells[0].FindControl("cboxIsSelected") as CheckBox).InputAttributes.Add("disabled", "true");
        }
    }

    /// <summary>
    /// Configure agent
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnConfigure_Click(object sender, EventArgs e)
    {
        List<String> list = new List<String>();
        String val;
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if ((row.Cells[0].FindControl("cboxIsSelected") as CheckBox).Checked)
                {
                    val = row.Cells[2].Text;

                    if (!list.Contains(val))
                        list.Add(val);

                    (row.Cells[0].FindControl("cboxIsSelected") as CheckBox).Checked = false;
                }
            }
        }
        
        (GridView1.HeaderRow.Cells[0].FindControl("cboxSelectAll") as CheckBox).Checked = false;

        if (list.Count == 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = Resources.Resource.NoSelected;
            return;
        }

        String service = ConfigurationManager.AppSettings["Service"];
        Vba32ControlCenterWrapper control = new Vba32ControlCenterWrapper(service);

        foreach (String ip in list)
        {
            try
            {
                control.DefaultConfigureAgent(ip);
            }
            catch { }
        }

        GridView1.DataBind();
    }

    protected void AutoUpdateControl1_AutoUpdate(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
}