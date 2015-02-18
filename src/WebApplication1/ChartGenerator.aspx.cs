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

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using VirusBlokAda.CC.Diagram;

public partial class ChartGenerator : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // установка типа возвращаемых данных - графический формат png
        Response.ContentType = "image/png";

        string xValues, yValues, chartType;

        // ѕолучение входных параметров из строки запроса
        chartType = Request.QueryString["chartType"];
        xValues = Request.QueryString["xValues"];
        yValues = Request.QueryString["yValues"];


        if (chartType == null)
            chartType = "";



        if (xValues != null && yValues != null)
        {
            Color bgColor;


            bgColor = Color.FromArgb(255, 253, 244);

            Bitmap StockBitMap;
            MemoryStream memStream = new MemoryStream();

            switch (chartType)
            {
                case "bar":
                    BarGraph bar = new BarGraph(bgColor);

                    bar.VerticalLabel = "";
                    bar.VerticalTickCount = 5;
                    bar.ShowLegend = true;
                    bar.ShowData = true;
                    bar.Height = 400;
                    bar.Width = 700;

                    bar.CollectDataPoints(xValues.Split("|".ToCharArray()), yValues.Split("|".ToCharArray()));
                    StockBitMap = bar.Draw();
                    break;
                default:
                    PieChart pc = new PieChart(bgColor);

                    pc.CollectDataPoints(xValues.Split("|".ToCharArray()), yValues.Split("|".ToCharArray()));

                    StockBitMap = pc.Draw();

                    break;
            }

            // ¬озврат растрового потока клиенту
            StockBitMap.Save(memStream, ImageFormat.Png);
            memStream.WriteTo(Response.OutputStream);
        }


    }
}
