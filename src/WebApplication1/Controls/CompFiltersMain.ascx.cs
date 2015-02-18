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

public partial class Controls_CompFiltersMain : System.Web.UI.UserControl
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
        cboxComputerName.Text = Resources.Resource.ComputerName;
        cboxIPAddress.Text = Resources.Resource.IPAddress;
        cboxLatestMalware.Text = Resources.Resource.LatestMalware;
        cboxUserLogin.Text = Resources.Resource.UserLogin;
        cboxDescription.Text = Resources.Resource.Description;

        List<string> terms = new List<string>();
        terms.Add("AND");
        terms.Add("OR");
        terms.Add("NOT");


        try
        {
            ddlTermComputerName.DataSource = terms;
            ddlTermComputerName.DataBind();
        }
        catch
        {
            ddlTermComputerName.SelectedIndex = -1;
            ddlTermComputerName.SelectedValue = null;
            ddlTermComputerName.DataSource = terms;
            ddlTermComputerName.DataBind();
        }

        try
        {
            ddlTermIPAddress.DataSource = terms;
            ddlTermIPAddress.DataBind();
        }
        catch
        {
            ddlTermIPAddress.SelectedIndex = -1;
            ddlTermIPAddress.SelectedValue = null;
            ddlTermIPAddress.DataSource = terms;
            ddlTermIPAddress.DataBind();
        }

        try
        {
            ddlTermLatestMalware.DataSource = terms;
            ddlTermLatestMalware.DataBind();
        }
        catch
        {
            ddlTermLatestMalware.SelectedIndex = -1;
            ddlTermLatestMalware.SelectedValue = null;
            ddlTermLatestMalware.DataSource = terms;
            ddlTermLatestMalware.DataBind();
        }

        try
        {
            ddlTermUserLogin.DataSource = terms;
            ddlTermUserLogin.DataBind();
        }
        catch
        {
            ddlTermUserLogin.SelectedIndex = -1;
            ddlTermUserLogin.SelectedValue = null;
            ddlTermUserLogin.DataSource = terms;
            ddlTermUserLogin.DataBind();
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

    public void LoadFilter(CompFilterEntity filter)
    {

        if (filter.ComputerName != String.Empty)
        {
            cboxComputerName.Checked = true;
            tboxComputerName.Text = filter.ComputerName;
            ddlTermComputerName.SelectedValue = filter.TermComputerName;
        }

        if (filter.IPAddress != String.Empty)
        {
            cboxIPAddress.Checked = true;
            tboxIPAddress.Text = filter.IPAddress;
            ddlTermIPAddress.SelectedValue = filter.TermIPAddress;
        }

        if (filter.LatestMalware != String.Empty)
        {
            cboxLatestMalware.Checked = true;
            tboxLatestMalware.Text = filter.LatestMalware;
            ddlTermLatestMalware.SelectedValue = filter.TermLatestMalware;
        }

        if (filter.UserLogin != String.Empty)
        {
            cboxUserLogin.Checked = true;
            tboxUserlogin.Text = filter.UserLogin;
            ddlTermUserLogin.SelectedValue = filter.TermUserLogin;
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
        CompFilterCollection collection = (CompFilterCollection)Session["CompFilters"];
        CompFilterEntity filter = collection.Get(filterName);

        this.LoadFilter(filter);
    }

    public void GetCurrentStateFilter(ref CompFilterEntity fltr)
    {
        //CompFilterEntity fltr = new CompFilterEntity();

        if (cboxComputerName.Checked)
        { 
            fltr.ComputerName = tboxComputerName.Text;
            fltr.TermComputerName = ddlTermComputerName.SelectedValue;
        }

        if (cboxIPAddress.Checked)
        {
            fltr.IPAddress = tboxIPAddress.Text;
            fltr.TermIPAddress = ddlTermIPAddress.SelectedValue;
        }

        if (cboxLatestMalware.Checked)
        { 
            fltr.LatestMalware = tboxLatestMalware.Text;
            fltr.TermLatestMalware = ddlTermLatestMalware.SelectedValue;
        }

        if (cboxUserLogin.Checked) 
        { 
            fltr.UserLogin = tboxUserlogin.Text;
            fltr.TermUserLogin = ddlTermUserLogin.SelectedValue;
        }

        if (cboxDescription.Checked)
        {
            fltr.Description = tboxDescription.Text;
            fltr.TermDescription = ddlTermDescription.SelectedValue;
        }
       
        //return fltr;
    }

    public void Clear()
    {
        cboxComputerName.Checked = false;
        cboxIPAddress.Checked = false;
        cboxLatestMalware.Checked = false;
        cboxUserLogin.Checked = false;
        cboxDescription.Checked = false;

        tboxComputerName.Text = String.Empty;
        tboxDescription.Text = String.Empty;
        tboxIPAddress.Text = String.Empty;
        tboxLatestMalware.Text = String.Empty;
        tboxUserlogin.Text = String.Empty;

        ddlTermComputerName.SelectedIndex = 0;
        ddlTermDescription.SelectedIndex = 0;
        ddlTermIPAddress.SelectedIndex = 0;
        ddlTermLatestMalware.SelectedIndex = 0;
        ddlTermUserLogin.SelectedIndex = 0;
    }


    

}
