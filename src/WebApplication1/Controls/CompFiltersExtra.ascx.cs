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
using System.Collections.Generic;

using ARM2_dbcontrol.Filters;
using VirusBlokAda.CC.DataBase;

public partial class Controls_CompFiltersExtra : System.Web.UI.UserControl
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
        cboxCPU.Text = Resources.Resource.CPU + '(' + Resources.Resource.Herz + ')'; ;
        cboxDomainName.Text = Resources.Resource.DomainName;
        cboxOStype.Text = Resources.Resource.OSType;
        cboxRam.Text = Resources.Resource.RAM + '('+ Resources.Resource.Megabyte+')';
        cboxVba32Version.Text = Resources.Resource.VBA32Version;

          
        List<string> terms = new List<string>();
        terms.Add("AND");
        terms.Add("OR");
        terms.Add("NOT");

        try
        {
            ddlTermCpu.DataSource = terms;
            ddlTermCpu.DataBind();
        }
        catch
        {
            ddlTermCpu.SelectedIndex = -1;
            ddlTermCpu.SelectedValue = null;
            ddlTermCpu.DataSource = terms;
            ddlTermCpu.DataBind();
        }

        try
        {
            ddlTermDomainName.DataSource = terms;
            ddlTermDomainName.DataBind();

        }
        catch
        {
            ddlTermDomainName.SelectedIndex = -1;
            ddlTermDomainName.SelectedValue = null;
            ddlTermDomainName.DataSource = terms;
            ddlTermDomainName.DataBind();

        }

        try
        {
            ddlTermOSType.DataSource = terms;
            ddlTermOSType.DataBind();
        }
        catch
        {
            ddlTermOSType.SelectedIndex = -1;
            ddlTermOSType.SelectedValue = null;
            ddlTermOSType.DataSource = terms;
            ddlTermOSType.DataBind();
        }

        try
        {
            ddlTermRam.DataSource = terms;
            ddlTermRam.DataBind();

        }
        catch
        {
            ddlTermRam.SelectedIndex = -1;
            ddlTermRam.SelectedValue = null;
            ddlTermRam.DataSource = terms;
            ddlTermRam.DataBind();

        }

        try
        {
            ddlTermVba32Version.DataSource = terms;
            ddlTermVba32Version.DataBind();

        }
        catch
        {
            ddlTermVba32Version.SelectedIndex = -1;
            ddlTermVba32Version.SelectedValue = null;
            ddlTermVba32Version.DataSource = terms;
            ddlTermVba32Version.DataBind();

        }

    }

    public void LoadFilter(CompFilterEntity filter)
    {
        if (filter.Cpu != String.Empty)
        {
            cboxCPU.Checked = true;
            string[] splitted = filter.Cpu.Split('-');
            tboxCPU.Text = splitted[0];
            tboxCPU2.Text = splitted[1];
            ddlTermCpu.SelectedValue = filter.TermCpu;
        }

        if (filter.DomainName != String.Empty)
        {
            cboxDomainName.Checked = true;
            tboxDomainName.Text = filter.DomainName;
            ddlTermDomainName.SelectedValue = filter.TermDomainName;
        }

        if (filter.Ram != String.Empty)
        {
            cboxRam.Checked = true;
            string[] splitted = filter.Ram.Split('-');
            tboxRam.Text = splitted[0];
            tboxRam2.Text = splitted[1];
            ddlTermRam.SelectedValue = filter.TermRam;
        }

        if (filter.VbaVersion != String.Empty)
        {
            cboxVba32Version.Checked = true;
            tboxVba32Version.Text = filter.VbaVersion;
            ddlTermVba32Version.SelectedValue = filter.TermVbaVersion;
        }

        if (filter.OsType != String.Empty)
        {
            cboxOStype.Checked = true;
            tboxOStype.Text = filter.OsType;
            ddlTermOSType.SelectedValue = filter.TermOsType;
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

        if (cboxCPU.Checked) 
        {
            fltr.Cpu = tboxCPU.Text + '-' + tboxCPU2.Text;
            fltr.TermCpu = ddlTermCpu.SelectedValue;
        }

        if (cboxDomainName.Checked) 
        { 
            fltr.DomainName = tboxDomainName.Text;
            fltr.TermDomainName = ddlTermDomainName.SelectedValue;
        }

        if (cboxOStype.Checked)
        { 
            fltr.OsType = tboxOStype.Text;
            fltr.TermOsType = ddlTermOSType.SelectedValue;
        }

        if (cboxRam.Checked) 
        { 
            fltr.Ram = tboxRam.Text+'-'+tboxRam2.Text;
            fltr.TermRam = ddlTermRam.SelectedValue;
        }

        if (cboxVba32Version.Checked) 
        { 
            fltr.VbaVersion = tboxVba32Version.Text;
            fltr.TermVbaVersion = ddlTermVba32Version.SelectedValue;
        }

        //return fltr;
    }

    public void Clear()
    {
        cboxCPU.Checked = false;
        cboxDomainName.Checked = false;
        cboxOStype.Checked = false;
        cboxRam.Checked = false;
        cboxVba32Version.Checked = false;


        tboxCPU.Text = String.Empty;
        tboxCPU2.Text = String.Empty;
        tboxDomainName.Text = String.Empty;
        tboxRam.Text = String.Empty;
        tboxRam2.Text = String.Empty;
        tboxVba32Version.Text = String.Empty;
        tboxOStype.Text = String.Empty;
        
        ddlTermCpu.SelectedIndex = 0;
        ddlTermDomainName.SelectedIndex = 0;
        ddlTermOSType.SelectedIndex = 0;
        ddlTermRam.SelectedIndex = 0;
        ddlTermVba32Version.SelectedIndex = 0;
    }


}
