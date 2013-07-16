using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Text.RegularExpressions;

using ARM2_dbcontrol.Filters;
using ARM2_dbcontrol.DataBase;
using ARM2_dbcontrol.Service.TaskAssignment;
using ARM2_dbcontrol.Tasks;
using Filters.Composite;
using Filters.Common;
using ARM2_dbcontrol.Common;

/// <summary>
/// Task list page
/// </summary>
public partial class Task : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ExportToExcel1.InitializeExportToExcel(GridView1, InformationListTypes.Tasks);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((!Roles.IsUserInRole("Administrator")) && (!Roles.IsUserInRole("Operator")))
        {
            Response.Redirect("Default.aspx");
        }

        if (!IsPostBack)
        {
            InitFields();
        }

        Controls_PagerUserControl.AddGridViewExtendedAttributes(GridView1, ObjectDataSource1);
    }

    /// <summary>
    /// Initialize fields
    /// </summary>
    protected override void InitFields()
    {
        lblMessage.Visible = false;        
        lbtnCancel.Text = Resources.Resource.CancelButtonText;

        fltState.DataSource = TasksDataContainer.GetTaskStates();
        fltState.DataBind();
    }

    protected void FilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    {
        GridView1.PageIndex = 0;
        GridView1.Where = e.Where;
        GridView1.DataBind();
    }

    /// <summary>
    /// Cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnCancel_Click(object sender, EventArgs e)
    {
        List<Int64> list = new List<Int64>();
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if ((row.Cells[0].FindControl("cboxIsSelected") as CheckBox).Checked)
                {
                    try
                    {
                        list.Add(Int64.Parse((row.Cells[8].FindControl("lblTaskID") as Literal).Text));
                    }
                    catch { }
                }
            }
        }

        if (list.Count == 0)
        {
            lblMessage.Visible = true;
            lblMessage.Text = Resources.Resource.NoSelected;
            return;
        }
        
        string service = ConfigurationManager.AppSettings["Service"];
        string connStr = ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString;

        Vba32ControlCenterWrapper control = new Vba32ControlCenterWrapper(service);
        
        string[] ipAddr = PreServAction.GetIPArrayByTaskID(list.ToArray(), connStr);

        try
        {
            control.PacketCancelTask(list.ToArray(), ipAddr);
        }
        catch
        {
            lblMessage.Visible = true;
            lblMessage.Text = Resources.Resource.Error + ". " + Resources.Resource.ErrorService;
            return;
        }

        lblMessage.Visible = true;
        if (control.GetLastError() == "")
            lblMessage.Text = Resources.Resource.CancelTask;
        else
            lblMessage.Text = Resources.Resource.Error + ". " + control.GetLastError();

        GridView1.DataBind();
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TaskEntityShow _task = (TaskEntityShow)e.Row.DataItem;

            //уточнить список для отмены
            List<String> list = new List<String>();
            list.Add(DatabaseNameLocalization.GetNameForCurrentCulture("Execution"));
            list.Add(DatabaseNameLocalization.GetNameForCurrentCulture("Delivery"));            

            if (!list.Contains(_task.TaskState))
                (e.Row.Cells[0].FindControl("cboxIsSelected") as CheckBox).InputAttributes.Add("disabled", "true");            
        }
    }

    #region Autoupdate
    protected void AutoUpdateControl1_AutoUpdate(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }    
    #endregion
}
