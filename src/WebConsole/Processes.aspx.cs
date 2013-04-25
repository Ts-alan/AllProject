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
using ARM2_dbcontrol.DataBase;
using Filters.Composite;
using Filters.Common;

/// <summary>
/// Process list
/// </summary>
public partial class Processes : PageBase
{
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
}