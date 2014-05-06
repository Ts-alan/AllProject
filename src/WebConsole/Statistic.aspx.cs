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

using VirusBlokAda.CC.DataBase;
using ARM2_dbcontrol.Filters;
using VirusBlokAda.CC.Filters.Primitive;

/// <summary>
/// Statistic page
/// </summary>
public partial class Statistic : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitFields();
        }
    }

    /// <summary>
    /// Initialization fields
    /// </summary>
    protected override void InitFields()
    {
        //Service information:
        Int32 countAll = DBProviders.Computer.Count("ComputerName LIKE '%'");

        DateTime dy = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
           DateTime.Now.Day, 0, 0, 0);

        DateTime dt = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month,
           DateTime.Now.Day, 0, 0, 0);

        Int32 countActiveToday = DBProviders.Computer.Count("ComputerName LIKE '%' " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "RecentActive", false, false));//"ComputerName LIKE '%'");

        lblCountOfCompActiveToday.Text = countActiveToday.ToString();
        lblCountOfCompRegistred.Text = countAll.ToString();

        countAll = DBProviders.Event.Count("EventID > -1");
        countActiveToday = DBProviders.Event.Count("EventID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "EventTime", false, false));

        lblCountOfEventRegistred.Text = countAll.ToString();
        lblCountOfEventToday.Text = countActiveToday.ToString();

        //��������� ���� �������������� ��� ��������� ���������� �� �������
        if ((Roles.IsUserInRole("Administrator")) || (Roles.IsUserInRole("Operator")))
        {
            lblCountOfTaskRegistred.Text = DBProviders.Task.Count("TaskID > -1").ToString();
            lblCountOfTaskTodayIssued.Text = DBProviders.Task.Count("TaskID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "DateIssued", false, false)).ToString();
            lblCountOfTaskTodayUpdated.Text = DBProviders.Task.Count("TaskID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "DateUpdated", false, false)).ToString();
            lblCountOfTaskTodayCompleted.Text = DBProviders.Task.Count("TaskID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "DateComplete", false, false)).ToString();
        }
        Page.Title = Resources.Resource.PageStatisticTitle;
    }

}
