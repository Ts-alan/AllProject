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
using System.Web.Services;
using Filters.Common;
using ARM2_dbcontrol.DataBase;
using System.Collections.Generic;

public partial class Visual : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }
    // Visual.aspx
    // Страница Visual.aspx отображает продажи по категориям в трех различных представлениях: 
    // круговая диаграмма (Pie Chart), гистограмма (Bar Graph) и линейный график (Line Chart).
    protected String _styleSheet;

    private void Page_Load(object sender, System.EventArgs e)
    {
        Page.Title = Resources.Resource.Diagram;
        RegisterScript(@"js/json2.js");
        RegisterScript(@"js/jQuery/jquery.loadmask.js");
        RegisterScript(@"js/PageRequestManagerHelper.js");
        RegisterScript(@"js/jQuery/jqplot/jquery.jqplot.js");
        RegisterScript(@"js/jQuery/jqplot/jqplot.barRenderer.js");
        RegisterScript(@"js/jQuery/jqplot/jqplot.categoryAxisRenderer.js");
        RegisterScript(@"js/jQuery/jqplot/jqplot.pieRenderer.js");
        RegisterScript(@"js/jQuery/jqplot/jqplot.pointLabels.js");        
        chartData = "{\"data\": null}";

        if (!IsPostBack)
        {
            if (Session["ViewSelectIndex"] != null)
                InitFields();
        }
    }
    private void Page_LoadComplete(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String where = FilterContainer.GenerateSQL();
            hdnFieldWhere.Value = where;
            HiddenFieldGroupBy.Value = fltType.GroupBy;
        }        
    }
    protected override void InitFields()
    {       
    }
    [WebMethod]
    public static String getData(string where, string groupBy)
    {
        if (where == "" || where == null)
        {
            chartData = "{\"data\": null}";
        }
        else
        {
            VisualReport report = new VisualReport(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
            List<StatisticEntity> dataList = report.GetStatistics(groupBy, where, 15);
            chartData = ConvertDataListToString(dataList);            
        }
        return chartData;
    }
    public static String chartData = "{\"data\": null}";

    protected void FilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(FilterContainer.FindControl("lbtnApply"), typeof(LinkButton), "createChart", "getChartData(\""+e.Where+"\",\""+fltType.GroupBy+"\");", true);
    }

    private static String ConvertDataListToString(List<StatisticEntity> dataList)
    {
        
        String data = "{\"data\":[";
        for (int i = 0; i < dataList.Count; i++)
        {
            data += "[\"" + dataList[i].Name + "\"," + dataList[i].Count.ToString() + "]";
            if (i != dataList.Count - 1) data += ",";
        }
        data += "]}";
        return data;
    }
}