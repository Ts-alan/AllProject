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
        ComputerProvider cmng = new ComputerProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);

        Int32 countAll = cmng.Count("ComputerName LIKE '%'");

        DateTime dy = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
           DateTime.Now.Day, 0, 0, 0);

        DateTime dt = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month,
           DateTime.Now.Day, 0, 0, 0);

        Int32 countActiveToday = cmng.Count("ComputerName LIKE '%' " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "RecentActive", false, false));//"ComputerName LIKE '%'");

        lblCountOfCompActiveToday.Text = countActiveToday.ToString();
        lblCountOfCompRegistred.Text = countAll.ToString();

        EventProvider evnt = new EventProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);

        countAll = evnt.Count("EventID > -1");
        countActiveToday = evnt.Count("EventID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "EventTime", false, false));

        lblCountOfEventRegistred.Text = countAll.ToString();
        lblCountOfEventToday.Text = countActiveToday.ToString();

        //Отобразим лишь администратору или оператору информацию по задачам
        if ((Roles.IsUserInRole("Administrator")) || (Roles.IsUserInRole("Operator")))
        {
            TaskProvider tskm = new TaskProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);

            lblCountOfTaskRegistred.Text = tskm.Count("TaskID > -1").ToString();
            lblCountOfTaskTodayIssued.Text = tskm.Count("TaskID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "DateIssued", false, false)).ToString();
            lblCountOfTaskTodayUpdated.Text = tskm.Count("TaskID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "DateUpdated", false, false)).ToString();
            lblCountOfTaskTodayCompleted.Text = tskm.Count("TaskID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "DateComplete", false, false)).ToString();
        }
        Page.Title = Resources.Resource.PageStatisticTitle;
    }

}
