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
public partial class GroupFilters : System.Web.UI.Page
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
        Page.Title = Resources.Resource.PageGroupsFilterTitle;
        if (!IsPostBack)
        {
            InitFields();
            //Editing mode
            if (Request.QueryString["Filter"] != null)
            {
                cmpfltMain.Clear();
                cmpfltMain.LoadFilter(Request.QueryString["Filter"]);

                tboxSaveAs.Text = Request.QueryString["Filter"];
            }
            else
            {
                if (Session["CurrentGroupFilter"] != null)
                {
                    GroupFilterEntity filter = (GroupFilterEntity)Session["CurrentGroupFilter"];
                    cmpfltMain.Clear();
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

        GroupFilterCollection collection;
        List<string> filtersName = new List<string>();

        if (Session["GroupFilters"] == null)
        {
            collection = new GroupFilterCollection(Profile.GroupFilters);
        }
        else
        {
            collection = (GroupFilterCollection)Session["GroupFilters"];
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
        GroupFilterEntity filter = new GroupFilterEntity();
        cmpfltMain.GetCurrentStateFilter(ref filter);


        filter.FilterName = tboxSaveAs.Text;

        ResourceControl resctrl =
           new ResourceControl();

        if (resctrl.IsExist("TemporaryFilter", filter.FilterName))
            throw new ArgumentException(Resources.Resource.ErrorIncorrectFilterName);

        Validation vld = new Validation(filter.FilterName);
        if (!vld.CheckStringFilterName())
            throw new ArgumentException(Resources.Resource.ErrorIncorrectFilterName);



        GroupFilterCollection collection = (GroupFilterCollection)Session["GroupFilters"];


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

        Profile.GroupFilters = collection.Serialize();
        Session["GroupFilters"] = collection;

        InitFields();

        Session["CurrentGroupFilter"] = filter;

        Response.Redirect("Groups.aspx");

    }
}
