using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirusBlokAda.Vba32CC.DataBase;
using Filters.Composite;
using System.Drawing;
using Filters.Common;
using VirusBlokAda.Vba32CC.Common;

public partial class Events : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected override void OnInit(EventArgs e)
    {        
        base.OnInit(e);
        ExportToExcel1.InitializeExportToExcel(GridView1, InformationListTypes.Events);
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
        fltComponent.DataSource = ComponentsDataContainer.GetTypes();
        fltComponent.DataBind();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            EventsEntity _event = (EventsEntity)e.Row.DataItem;

            if (!String.IsNullOrEmpty(_event.Color))
            {
                if (_event.Color.ToLower() != "transparent")
                    e.Row.BackColor = Color.FromName(_event.Color);
            }
        }
    }

    protected void FilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    {
        GridView1.PageIndex = 0;
        GridView1.Where = e.Where;
        GridView1.DataBind();
    }

    protected void AutoUpdateControl1_AutoUpdate(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
}