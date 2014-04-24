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

using VirusBlokAda.Vba32CC.DataBase;
using ARM2_dbcontrol.Filters;
using Filters.Primitive;

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
        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            ComputersManager cmng = new ComputersManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            Int32 countAll = cmng.Count("ComputerName LIKE '%'");

            DateTime dy = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
               DateTime.Now.Day, 0, 0, 0);

            DateTime dt = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month,
               DateTime.Now.Day, 0, 0, 0);

            Int32 countActiveToday = cmng.Count("ComputerName LIKE '%' " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "RecentActive", false, false));//"ComputerName LIKE '%'");
            conn.CloseConnection();

            lblCountOfCompActiveToday.Text = countActiveToday.ToString();
            lblCountOfCompRegistred.Text = countAll.ToString();

            EventsManager evnt = new EventsManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            countAll = evnt.Count("EventID > -1");
            countActiveToday = evnt.Count("EventID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "EventTime", false, false));
            conn.CloseConnection();

            lblCountOfEventRegistred.Text = countAll.ToString();
            lblCountOfEventToday.Text = countActiveToday.ToString();

            //Отобразим лишь администратору или оператору информацию по задачам
            if ((Roles.IsUserInRole("Administrator")) || (Roles.IsUserInRole("Operator")))
            {
                TaskManager tskm = new TaskManager(conn);
                conn.OpenConnection();
                conn.CheckConnectionState(true);

                lblCountOfTaskRegistred.Text = tskm.Count("TaskID > -1").ToString();
                lblCountOfTaskTodayIssued.Text = tskm.Count("TaskID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "DateIssued", false, false)).ToString();
                lblCountOfTaskTodayUpdated.Text = tskm.Count("TaskID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "DateUpdated", false, false)).ToString();
                lblCountOfTaskTodayCompleted.Text = tskm.Count("TaskID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(dy, dt, "DateComplete", false, false)).ToString();
                conn.CloseConnection();
            }


        }
        Page.Title = Resources.Resource.PageStatisticTitle;
    }

}
