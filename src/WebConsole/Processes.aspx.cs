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

using ARM2_dbcontrol.Filters;
using VirusBlokAda.CC.DataBase;
using VirusBlokAda.CC.Filters.Composite;
using VirusBlokAda.CC.Filters.Common;
using System.Web.Services;
using VirusBlokAda.CC.Tasks.Service;
using ARM2_dbcontrol.Tasks;

/// <summary>
/// Process list
/// </summary>
public partial class Processes : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //ѕри смене состо€ни€ web-parts страница становитс€ Untitled
        Page.Title = Resources.Resource.PageProcessTitle;
        if (!IsPostBack)
        {
            InitFields();
        }

        Controls_PagerUserControl.AddGridViewExtendedAttributes(GridView1, ObjectDataSource1);
    }

    /// <summary>
    /// Initialization fields
    /// </summary>
    protected override void InitFields()
    {
        lbtnExcel.Text = Resources.Resource.ExportToExcel;
        GridView1.Columns[2].HeaderText = Resources.Resource.MemorySize + '(' + Resources.Resource.Kilobyte + ')';
        GridView1.EmptyDataText = Resources.Resource.EmptyMessage;
        fltMemory.RangeCompareErrorMessage = Resources.Resource.MemorySize + " - " + Resources.Resource.RangeError;
    }

    protected void FilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    {
        GridView1.PageIndex = 0;
        GridView1.Where = e.Where;
        GridView1.DataBind();
    }

    protected void lbtnExcel_Click(object sender, EventArgs e)
    {
        GridView gvExcel = new GridView();

        string where = GridView1.Where;
        if (String.IsNullOrEmpty(where)) where = null;
        gvExcel.DataSource = ProcessDataContainer.Get(where, String.Empty, ProcessDataContainer.Count(where), 1);
        gvExcel.DataBind();

        DataGridToExcel.Export("Processes.xls", gvExcel);
    }

    [WebMethod]
    public static String TerminateProcess(String procName, String compName)
    {
        String service = ConfigurationManager.AppSettings["Service"];
        String connStr = ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString;

        List<Int16> listID = new List<Int16>();
        listID.Add(DBProviders.Computer.GetComputerID(compName));
        String[] ipAddr = PreServAction.GetIPArray(listID, connStr);

        Vba32ControlCenterWrapper control = new Vba32ControlCenterWrapper(service);

        Int64[] taskId = new Int64[1];
        taskId[0] = PreServAction.CreateTask(compName, Resources.Resource.Terminate + " " + Resources.Resource.ActionProcess.ToLower(), GetTaskXML(procName), Anchor.GetStringForTaskGivedUser(), connStr);
        
        control.PacketCreateProcess(taskId, ipAddr, GetCommangLine(procName));

        return Resources.Resource.Terminate + " " + Resources.Resource.ActionProcess.ToLower() + ": " + Resources.Resource.TaskGived;
        //control.GetLastError()
    }

    private static String GetTaskXML(String procName)
    {
        VirusBlokAda.CC.Common.Xml.XmlBuilder xml = new VirusBlokAda.CC.Common.Xml.XmlBuilder("task");

        string str = String.Empty;

        str = GetCommangLine(procName);

        str = str.Replace(" ", "&#160;");
        str = str.Replace("&", "&amp;");

        xml.AddNode("CommandLine", str);
        xml.AddNode("ComSpec", "0");
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "CreateProcess");
        xml.Generate();

        return xml.Result;
    }

    private static String GetCommangLine(String procName)
    {
        return String.Format("taskkill /IM \"{0}\" /F /T", procName);
    }
}