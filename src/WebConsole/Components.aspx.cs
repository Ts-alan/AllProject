using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Filters.Composite;
using ARM2_dbcontrol.DataBase;
using Filters.Common;
using ARM2_dbcontrol.Common;

public partial class Components : PageBase
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ExportToExcel1.InitializeExportToExcel(GridView1, InformationListTypes.Components);
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
        fltComponentState.DataSource = ComponentsDataContainer.GetStates();
        fltComponentState.DataBind();
        fltComponentName.DataSource = ComponentsDataContainer.GetTypes();
        fltComponentName.DataBind();
    }

    protected void FilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    {
        GridView1.PageIndex = 0;
        GridView1.Where = e.Where;
        GridView1.DataBind();
    }
}
