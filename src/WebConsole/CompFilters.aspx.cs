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

using System.Globalization;

using ARM2_dbcontrol.Filters;

/// <summary>
/// Computer filter page
/// </summary>
public partial class CompFilters : System.Web.UI.Page
{

    protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.MasterPageFile = Profile.MasterPage;
        Page.Theme = Profile.Theme;
    }

    protected override void InitializeCulture()
    {
        System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo(Profile.Culture);
        base.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = Resources.Resource.PageComputersFilterTitle;
        if (!IsPostBack)
        {
            InitFields();
            //Editing mode
            if (Request.QueryString["Filter"] != null)
            {
                cmpfltBool.Clear();
                cmpfltExtra.Clear();
                cmpfltMain.Clear();

                cmpfltBool.LoadFilter(Request.QueryString["Filter"]);
                cmpfltExtra.LoadFilter(Request.QueryString["Filter"]);
                cmpfltMain.LoadFilter(Request.QueryString["Filter"]);

                tboxSaveAs.Text = Request.QueryString["Filter"];
            }
            else
            {
                if (Session["CurrentCompFilter"] != null)
                {
                    CompFilterEntity filter = (CompFilterEntity)Session["CurrentCompFilter"];

                    cmpfltBool.Clear();
                    cmpfltExtra.Clear();
                    cmpfltMain.Clear();

                    cmpfltBool.LoadFilter(filter);
                    cmpfltExtra.LoadFilter(filter);
                    cmpfltMain.LoadFilter(filter);
                }
            }
        }
    }
  

    /// <summary>
    /// Initialization fields
    /// </summary>
    private void InitFields()
    {

        btnSaveAs.Text = Resources.Resource.Save;

        CompFilterCollection collection;
        List<string> filtersName = new List<string>();

        if (Session["CompFilters"] == null)
        {
            collection = new CompFilterCollection(Profile.CompFilters);
        }
        else
        {
            collection = (CompFilterCollection)Session["CompFilters"];
        }

        lblMessage.Visible = false;
    }

    /// <summary>
    /// Save as button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveAs_Click(object sender, EventArgs e)
    {
       //Init filter object
        CompFilterEntity filter = new CompFilterEntity();
        cmpfltMain.GetCurrentStateFilter(ref filter);
        cmpfltExtra.GetCurrentStateFilter(ref filter);
        cmpfltBool.GetCurrentStateFilter(ref filter);


        filter.FilterName = tboxSaveAs.Text;

        ResourceControl resctrl =
           new ResourceControl();

        if (resctrl.IsExist("TemporaryFilter", filter.FilterName))
            throw new ArgumentException(Resources.Resource.ErrorIncorrectFilterName);

        Validation vld = new Validation(filter.FilterName);
        if (!vld.CheckStringFilterName())
            throw new ArgumentException(Resources.Resource.ErrorIncorrectFilterName);


                       
        CompFilterCollection collection = (CompFilterCollection)Session["CompFilters"];


        filter.GenerateSQLWhereStatement();

            //Editing mode
        if (Request.QueryString["Filter"] != null)
        {
            //old filter name
            if (Request.QueryString["Filter"] == filter.FilterName)
            {
                collection.Update(filter);
                collection = collection.Deserialize();

            }
            //user change filter name
            else
            {
                collection.Add(filter);
            }
        }
        //Add mode
        else
        {
            if (collection.Get(filter.FilterName).FilterName != String.Empty)
                throw new InvalidOperationException(Resources.Resource.ErrorExistingFilter);

            collection.Add(filter);
        }

            Profile.CompFilters = collection.Serialize();
            Session["CompFilters"] = collection;

            InitFields();

            Session["CurrentCompFilter"] = filter;

            Response.Redirect("Computers.aspx");

    }
}
