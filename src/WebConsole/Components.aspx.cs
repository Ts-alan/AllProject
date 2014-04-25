using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirusBlokAda.CC.Filters.Composite;
using VirusBlokAda.CC.DataBase;
using VirusBlokAda.CC.Filters.Common;
using VirusBlokAda.CC.Common;

public partial class Components : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

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
