using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirusBlokAda.Vba32CC.DataBase;
using System.Configuration;
using ARM2_dbcontrol.Filters;

public partial class Controls_GroupFiltersMain : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitFields();
        }
    }
    private void InitFields()
    {
        cboxGroupName.Text = Resources.Resource.GroupName;
        cboxDescription.Text = Resources.Resource.Description;

        List<string> terms = new List<string>();
        terms.Add("AND");
        terms.Add("OR");
        terms.Add("NOT");

        try
        {
            ddlTermGroupName.DataSource = terms;
            ddlTermGroupName.DataBind();
        }
        catch
        {
            ddlTermGroupName.SelectedIndex = -1;
            ddlTermGroupName.SelectedValue = null;

            ddlTermGroupName.DataSource = terms;
            ddlTermGroupName.DataBind();
        }

        try
        {
            ddlTermDescription.DataSource = terms;
            ddlTermDescription.DataBind();
        }
        catch
        {
            ddlTermDescription.SelectedIndex = -1;
            ddlTermDescription.SelectedValue = null;

            ddlTermDescription.DataSource = terms;
            ddlTermDescription.DataBind();
        }
    }

    public void LoadFilter(GroupFilterEntity filter)
    {

        if (filter.GroupName != String.Empty)
        {
            cboxGroupName.Checked = true;
            tboxGroupName.Text = filter.GroupName;
            ddlTermGroupName.SelectedValue = filter.TermGroupName;
        }

        if (filter.Description != String.Empty)
        {
            cboxDescription.Checked = true;
            tboxDescription.Text = filter.Description;
            ddlTermDescription.SelectedValue = filter.TermDescription;
        }
    }

    public void LoadFilter(string filterName)
    {
        GroupFilterCollection collection = (GroupFilterCollection)Session["GroupFilters"];
        GroupFilterEntity filter = collection.Get(filterName);

        this.LoadFilter(filter);
    }

    public void GetCurrentStateFilter(ref GroupFilterEntity fltr)
    {
        if (cboxGroupName.Checked)
        {
            fltr.GroupName = tboxGroupName.Text;
            fltr.TermGroupName = ddlTermGroupName.SelectedValue;
        }

        if (cboxDescription.Checked)
        {
            fltr.Description = tboxDescription.Text;

            fltr.TermDescription = ddlTermDescription.SelectedValue;
        }
        
    }


    public void Clear()
    {
        ddlTermGroupName.SelectedIndex = 0;
        ddlTermDescription.SelectedIndex = 0;

        tboxGroupName.Text = String.Empty;
        tboxDescription.Text = String.Empty;
        
        cboxGroupName.Checked = false;
        cboxDescription.Checked = false;
    }
}