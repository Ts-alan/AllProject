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

using ARM2_dbcontrol.Diagram;
using ARM2_dbcontrol.Filters;
using Filters.Primitive;

public partial class Visual : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }
    // Visual.aspx
    // —траница Visual.aspx отображает продажи по категори€м в трех различных представлени€х: 
    // кругова€ диаграмма (Pie Chart), гистограмма (Bar Graph) и таблица (Tabular).
    protected string _styleSheet;

    private void Page_Load(object sender, System.EventArgs e)
    {
        Page.Title = Resources.Resource.Diagram;

        if (!IsPostBack)
        {
            if (Session["ViewSelectIndex"] != null)
                drpChartType.SelectedIndex = (int)Session["ViewSelectIndex"];

            InitFields();

            // вызов событи€ дл€ вывода представлени€ по умолчанию
            drpChartType_SelectedIndexChanged(this, new System.EventArgs());

        }

    }

    protected override void InitFields()
    {
        ddlStatisticType.Items.Add(Resources.Resource.StatCountEvent);
        ddlStatisticType.Items.Add(Resources.Resource.StatCountEventToday);
        ddlStatisticType.Items.Add(Resources.Resource.StatCountVirus);
        ddlStatisticType.Items.Add(Resources.Resource.StatCountVirusToday);

        ddlStatisticType.Items.Add(Resources.Resource.StatVirusTotalTop);
        ddlStatisticType.Items.Add(Resources.Resource.StatVirusTopMonth);
        ddlStatisticType.Items.Add(Resources.Resource.StatVirusTopToDay);

        drpChartType.Items.Add(new ListItem(Resources.Resource.Diagram, "Pie"));
        drpChartType.Items.Add(new ListItem(Resources.Resource.Bar, "Bar"));
        drpChartType.Items.Add(new ListItem(Resources.Resource.Table, "Table"));

        lblType.Text = Resources.Resource.Type;
        lblAppearance.Text = Resources.Resource.Appearance;

    }

    // ћетод BindGrid запрашивает список  по категори€м
    // и затем прив€зывает эти данные к SalesByCategoryGrid
    private void BindGrid(VisualReportCollection data)
    {
        SalesByCategoryGrid.Visible = true;
        SalesByCategoryGrid.DataSource = data;
        SalesByCategoryGrid.DataBind();
    }


    private VisualReportCollection GetVirusStat(DateTime dt)
    {
        //”берем гиперссылки
        ((HyperLinkColumn)SalesByCategoryGrid.Columns[0]).DataNavigateUrlField = "";

        return VisualReport.GetVirusStat(
          ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString,
          ConfigurationManager.AppSettings["VirusFoundEvent"], dt);
    }

    // ќбработчик событий drpChartType_SelectedIndexChanged получает данные измен€ет вид представлени€.
    protected void drpChartType_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        String sqlQuery = String.Empty;
        //говнокод
        // извлечение данных
        VisualReportCollection visualReport = new VisualReportCollection();
        if (Request.QueryString["Mode"] == null)
        {
            //!-OPTM ѕростор дл€ оптимизации
            switch (ddlStatisticType.SelectedIndex)
            {
                case 6:
                    DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    visualReport = GetVirusStat(dt);
                    break;

                case 5:
                    visualReport = GetVirusStat(DateTime.Now.AddMonths(-1));
                    break;

                case 4:
                    visualReport = GetVirusStat(new DateTime(1900, 1, 1));
                    break;

                case 3: // вирусов за день
                    sqlQuery = "EventID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0),
                                     new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 0), "EventTime", false, false)
                                     + PrimitiveFilterHelper.GenerateSqlForTextValue(ConfigurationManager.AppSettings["VirusFoundEvent"], "EventName", false, false);
                    break;
                case 2://всего вирусов
                    sqlQuery = "EventID > -1 " + PrimitiveFilterHelper.GenerateSqlForTextValue(ConfigurationManager.AppSettings["VirusFoundEvent"], "EventName", false, false);
                    break;
                case 1: // событий за день
                    sqlQuery = "EventID > -1 " + PrimitiveFilterHelper.GenerateSqlForRangeDateTimeValue(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0),
                                     new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 0), "EventTime", false, false);
                    break;
                case 0://всего событий
                default:
                    sqlQuery = "EventID > -1";
                    break;
            }
            //ммм.. костыли..
            if (ddlStatisticType.SelectedIndex < 4)
            {
                visualReport = VisualReport.GetEvent(
                        ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString,
                        sqlQuery);
                ((HyperLinkColumn)SalesByCategoryGrid.Columns[0]).DataNavigateUrlField = "CategoryName";
            }
        }
        else
        {
            /*
            ddlStatisticType.Visible = false;
            
            string table = Request.QueryString["Mode"];
            try
            {
                switch (table)
                {
                    case "Events":
                        filter = (EventFilterEntity)Session["CurrentEventFilter"];
                        
                        break;
                    default:
                        //ѕо идее, этого эксепшна пользователь не увидит..
                        throw new Exception("Cannot load diagram type!");
                }

              

                if(filter==null)
                    throw new Exception("Filter to diagramm is null");
                lblType.Text = filter.FilterName;

                filter.GenerateSQLWhereStatement();

                if ((Session["TempGroupEvents"] != null)&&(table=="Events"))
                {
                    filter.GetSQLWhereStatement =
                        ((FilterEntity)Session["TempGroup"]).GetSQLWhereStatement;
                }


                visualReport = VisualReport.GetEvent(
                        ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString,
                        filter.GetSQLWhereStatement);

            }
            catch
            {
                Response.Redirect("Visual.aspx");
            }
             */
        }

        if (drpChartType.SelectedItem.Value != "Table")
        {
            StringBuilder xValues = new StringBuilder();
            StringBuilder yValues = new StringBuilder();
            int rowCount = visualReport.Count;
            int i = 1;

            // преобразование данных в формат с разделителем-зап€той
            foreach (VisualReport dr in visualReport)
            {
                xValues.Append(dr.CategoryName);
                yValues.Append(dr.Sales);
                if (i < rowCount)
                {
                    xValues.Append("|");
                    yValues.Append("|");
                }
                i++;
            }

            if ((xValues.ToString() == "") && (yValues.ToString() == ""))
                ChartImage.ImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/Disabled.gif";
            else
                // прив€зка генератора изображений к изображени
                ChartImage.ImageUrl = "ChartGenerator.aspx?" +
                    "xValues=" + xValues.ToString() +
                    "&yValues=" + yValues.ToString() +
                    "&ChartType=" + drpChartType.SelectedItem.Value.ToLower();

            ChartImage.Visible = true;
            SalesByCategoryGrid.Visible = false;
        }
        else
        {

            BindGrid(visualReport);
            ChartImage.Visible = false;

        }

        Session["ViewSelectIndex"] = drpChartType.SelectedIndex;
    }
    protected void ddlStatisticType_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpChartType_SelectedIndexChanged(this, new System.EventArgs());
    }
}
