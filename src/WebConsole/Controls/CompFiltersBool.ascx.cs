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

public partial class Controls_CompFiltersBool : System.Web.UI.UserControl
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
        cboxControlCenter.Text = Resources.Resource.ControlCenter;
        cboxVba32Integrity.Text = Resources.Resource.VBA32Integrity;
        cboxVba32KeyValid.Text = Resources.Resource.VBA32KeyValid;

        cboxIsControlCenter.Text = Resources.Resource.YesNo;
        cboxIsVba32Integrity.Text = Resources.Resource.YesNo;
        cboxIsVba32KeyValid.Text = Resources.Resource.YesNo;

        List<string> terms = new List<string>();
        terms.Add("AND");
        terms.Add("OR");
        terms.Add("NOT");

        try
        {
            ddlTermControlCenter.DataSource = terms;
            ddlTermControlCenter.DataBind();
        }
        catch
        {
            ddlTermControlCenter.SelectedIndex = -1;
            ddlTermControlCenter.SelectedValue = null;
            ddlTermControlCenter.DataSource = terms;
            ddlTermControlCenter.DataBind();
        }

        try
        {
            ddlTermVba32Integrity.DataSource = terms;
            ddlTermVba32Integrity.DataBind();
        }
        catch
        {
            ddlTermVba32Integrity.SelectedIndex = -1;
            ddlTermVba32Integrity.SelectedValue = null;
            ddlTermVba32Integrity.DataSource = terms;
            ddlTermVba32Integrity.DataBind();
        }

        try
        {
            ddlTermVba32KeyValid.DataSource = terms;
            ddlTermVba32KeyValid.DataBind();
        }
        catch
        {
            ddlTermVba32KeyValid.SelectedIndex = -1;
            ddlTermVba32KeyValid.SelectedValue = null;
            ddlTermVba32KeyValid.DataSource = terms;
            ddlTermVba32KeyValid.DataBind();
        }

    }

    public void LoadFilter(CompFilterEntity filter)
    {

        if (filter.ControlCenter != String.Empty)
        {
            cboxControlCenter.Checked = true;
            if (filter.ControlCenter == "1")
                cboxIsControlCenter.Checked = true;
            else cboxIsControlCenter.Checked = false;

            ddlTermControlCenter.SelectedValue = filter.TermControlCenter;

        }

        if (filter.VbaIntegrity != String.Empty)
        {
            cboxVba32Integrity.Checked = true;
            if (filter.VbaIntegrity == "1")
                cboxIsVba32Integrity.Checked = true;
            else cboxIsVba32Integrity.Checked = false;

            ddlTermVba32Integrity.SelectedValue = filter.TermVbaIntegrity;

        }

        if (filter.VbaKeyValid != String.Empty)
        {
            cboxVba32KeyValid.Checked = true;
            if (filter.VbaKeyValid == "1")
                cboxIsVba32KeyValid.Checked = true;
            else cboxIsVba32KeyValid.Checked = false;

            ddlTermVba32KeyValid.SelectedValue = filter.TermVbaKeyValid;

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
       // CompFilterEntity fltr = new CompFilterEntity();
     
        if (cboxControlCenter.Checked)
        {
            if (cboxIsControlCenter.Checked) fltr.ControlCenter = "1";
            else fltr.ControlCenter = "0";

            fltr.TermControlCenter = ddlTermControlCenter.SelectedValue;

        }
        if (cboxVba32Integrity.Checked)
        {
            if (cboxIsVba32Integrity.Checked) fltr.VbaIntegrity = "1";
            else fltr.VbaIntegrity = "0";

            fltr.TermVbaIntegrity = ddlTermVba32Integrity.SelectedValue;
        }
        if (cboxVba32KeyValid.Checked)
        {
            if (cboxIsVba32KeyValid.Checked) fltr.VbaKeyValid = "1";
            else fltr.VbaKeyValid = "0";

            fltr.TermVbaKeyValid = ddlTermVba32KeyValid.SelectedValue;

        }

        //return fltr;
    }

    public void Clear()
    {
        cboxControlCenter.Checked = false;
        cboxVba32Integrity.Checked = false;
        cboxVba32KeyValid.Checked = false;

        cboxIsControlCenter.Checked = false;
        cboxIsVba32Integrity.Checked = false;
        cboxIsVba32KeyValid.Checked = false;
    }



}
