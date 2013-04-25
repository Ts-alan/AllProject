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

//Note that ToolkitScriptManager must be added to host page. There is no work around. 
//Pager goes through whole lifecycle 2 times on GridView Load and DataBinding.
//When trying to register ScriptManager dynamically unstable errors can occur.
public partial class Controls_PagerUserControl : System.Web.UI.UserControl
{
    #region LifeCycle
    protected override void OnInit(EventArgs e)
    {
        RecursiveFindAssociatedGridView(this as Control);
        AllwaysShowPaging();
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion

    #region Always Show Paging
    private void AllwaysShowPaging()
    {
        AssociatedGridView.PreRender += new EventHandler(delegate
            (object sender, EventArgs e)
            {
                GridViewRow pagerRow = (GridViewRow)AssociatedGridView.BottomPagerRow;
                if (pagerRow != null && pagerRow.Visible == false)
                {
                    pagerRow.Visible = true;
                }

                pagerRow = (GridViewRow)AssociatedGridView.TopPagerRow;
                if (pagerRow != null && pagerRow.Visible == false)
                {
                    pagerRow.Visible = true;
                }
            });
    }

    #endregion

    #region Watch Associated GridView Properties

    public static void AddGridViewExtendedAttributes(GridView gridView, IDataSource dataSourceObject)
    {
        WatchTotalCount(gridView, dataSourceObject);
        WatchShowingItems(gridView);
    }

    private static void WatchTotalCount(GridView gridView, IDataSource dataSourceObject)
    {
        ObjectDataSource ods = (dataSourceObject as ObjectDataSource);
        if (ods != null)
        {
            ods.Selected += new ObjectDataSourceStatusEventHandler(
                delegate(object sender, ObjectDataSourceStatusEventArgs e)
                {
                    int? rowCount = e.ReturnValue as int?;
                    if (rowCount != null)
                    {
                        int totalCount = rowCount.Value;
                        gridView.Attributes.Add(totalCountKey, totalCount.ToString());
                    }
                });
            return;
        }
        SqlDataSource sds = (dataSourceObject as SqlDataSource);
        if (sds != null)
        {
            sds.Selected += new SqlDataSourceStatusEventHandler(
                delegate(object sender, SqlDataSourceStatusEventArgs e)
                {
                    int totalCount = e.AffectedRows;
                    gridView.Attributes.Add(totalCountKey, totalCount.ToString());
                });
            return;
        }
    }

    private static void WatchShowingItems(GridView gridView)
    {
        gridView.DataBound += new EventHandler(
            delegate(object sender, EventArgs e)
            {
                int showingFirst = gridView.PageIndex * gridView.PageSize + 1;
                int showingLast = showingFirst + gridView.Rows.Count - 1;
                gridView.Attributes.Add(showingFirstKey, showingFirst.ToString());
                gridView.Attributes.Add(showingLastKey, showingLast.ToString());
            });
    }
    #endregion

    #region Associated GridView
    private GridView AssociatedGridView;
    private void RecursiveFindAssociatedGridView(Control control)
    {
        Control parentControl = control.NamingContainer;
        if (parentControl == null)
        {
            String errorInfo = "This control is PagerTemplate. Can't find associated gridview.";
            throw new HttpException(errorInfo);
        }
        AssociatedGridView = parentControl as GridView;
        if (AssociatedGridView == null)
        {
            RecursiveFindAssociatedGridView(parentControl);
        }
    }
    #endregion

    #region InitializeProperties
    private void InitializeImages()
    {
        FirstImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/page-first.gif";
        PreviousImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/page-prev.gif";
        NextImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/page-next.gif";
        LastImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/page-last.gif";
        FirstImageUrlDisabled = "~/App_Themes/" + Profile.Theme + "/Images/page-first-disabled.gif";
        PreviousImageUrlDisabled = "~/App_Themes/" + Profile.Theme + "/Images/page-prev-disabled.gif";
        NextImageUrlDisabled = "~/App_Themes/" + Profile.Theme + "/Images/page-next-disabled.gif";
        LastImageUrlDisabled = "~/App_Themes/" + Profile.Theme + "/Images/page-last-disabled.gif";
    }

    private void InitializeText()
    {
        GoText = Resources.Resource.Go;
        TotalMessage = " " + Resources.Resource.TotalMessage;
        EmptyMessage = Resources.Resource.EmptyMessage;
        DisplayMessage = Resources.Resource.DisplayMessage;
        ItemsPerPageMessage = Resources.Resource.ItemsPerPageMessage;
    }

    private void InitializeCss()
    {
        LinkButtonCssClass = "pagingNavigation";
    }

    private bool initializedProperties = false;

    private void InitializeProperties()
    {
        if (initializedProperties) return;
        InitializeImages();
        InitializeText();
        InitializeCss();
        initializedProperties = true;
    }
    #endregion

    #region Navigation Prerender
    protected void lbtnFirst_PreRender(object sender, EventArgs e)
    {
        if (AssociatedGridView.PageIndex == 0)
        {
            lbtnFirst.Enabled = false;
            imgFirst.ImageUrl = FirstImageUrlDisabled;
        }
        else
        {
            lbtnFirst.Enabled = true;
            imgFirst.ImageUrl = FirstImageUrl;
        }
    }

    protected void lbtnPrev_PreRender(object sender, EventArgs e)
    {
        if (AssociatedGridView.PageIndex == 0)
        {
            lbtnPrev.Enabled = false;
            imgPrev.ImageUrl = PreviousImageUrlDisabled;
        }
        else
        {
            lbtnPrev.Enabled = true;
            imgPrev.ImageUrl = PreviousImageUrl;
        }
    }

    protected void lbtnNext_PreRender(object sender, EventArgs e)
    {
        if (AssociatedGridView.PageIndex >= AssociatedGridView.PageCount - 1)
        {
            lbtnNext.Enabled = false;
            imgNext.ImageUrl = NextImageUrlDisabled;
        }
        else
        {
            lbtnNext.Enabled = true;
            imgNext.ImageUrl = NextImageUrl;
        }
    }

    protected void lbtnLast_PreRender(object sender, EventArgs e)
    {
        if (AssociatedGridView.PageIndex >= AssociatedGridView.PageCount - 1)
        {
            lbtnLast.Enabled = false;
            imgLast.ImageUrl = LastImageUrlDisabled;
        }
        else
        {
            lbtnLast.Enabled = true;
            imgLast.ImageUrl = LastImageUrl;
        }
    }

    protected void lbtnGo_PreRender(object sender, EventArgs e)
    {
        if (AssociatedGridView.PageCount == 0)
        {
            lbtnGo.Enabled = false;
        }
        else
        {
            lbtnGo.Enabled = true;
        }
    }

    protected void tbPage_PreRender(object sender, EventArgs e)
    {
        if (AssociatedGridView.PageCount == 0)
        {
            tbPage.Text = "0";
        }
        else
        {
            tbPage.Text = (AssociatedGridView.PageIndex + 1).ToString();
        }
    }

    protected void lblTotalPages_PreRender(object sender, EventArgs e)
    {
        if (AssociatedGridView.PageCount != 0)
        {
            if (!String.IsNullOrEmpty(TotalMessage))
            {
                lblTotalPages.Text = String.Format(TotalMessage, AssociatedGridView.PageCount);
            }
        }
    }
    #endregion

    #region Navigation Properties
    private string FirstImageUrl;
    private string PreviousImageUrl;
    private string NextImageUrl;
    private string LastImageUrl;
    private string FirstImageUrlDisabled;
    private string PreviousImageUrlDisabled;
    private string NextImageUrlDisabled;
    private string LastImageUrlDisabled;
    private string LinkButtonCssClass;
    private string TotalMessage;
    private string GoText;

    private string DisplayMessage;
    private string EmptyMessage;
    #endregion

    #region Navigation Initialize
    protected void lbtnNav_Init(object sender, EventArgs e)
    {
        InitializeProperties();
        LinkButton lbtnNav = sender as LinkButton;
        lbtnNav.CssClass = LinkButtonCssClass;
        lbtnNav.Attributes.Add("onmouseover", String.Format("$get('{0}').className='{1}Hover'", lbtnNav.ClientID, LinkButtonCssClass));
        lbtnNav.Attributes.Add("onmouseout", String.Format("$get('{0}').className='{1}'", lbtnNav.ClientID, LinkButtonCssClass));
        (lbtnNav.Controls[0] as Image).Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
    }

    protected void lbtnGo_Init(object sender, EventArgs e)
    {
        InitializeProperties();
        lblGo.Text = GoText;
        lblGo.Style.Add(HtmlTextWriterStyle.Color, "Navy");
        lbtnGo.CssClass = LinkButtonCssClass;
        lbtnGo.Attributes.Add("onmouseover", String.Format("$get('{0}').className='{1}Hover'", lbtnGo.ClientID, LinkButtonCssClass));
        lbtnGo.Attributes.Add("onmouseout", String.Format("$get('{0}').className='{1}'", lbtnGo.ClientID, LinkButtonCssClass));
        lblGo.ForeColor = System.Drawing.Color.White;
        lblGo.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
        lbtnGo.Style.Add(HtmlTextWriterStyle.Width, "60px");
    }

    protected void btnHiddenGo_Init(object sender, EventArgs e)
    {
        btnHiddenGo.Width = Unit.Pixel(1);
        btnHiddenGo.Height = Unit.Pixel(1);
        btnHiddenGo.Style.Add(HtmlTextWriterStyle.BorderStyle, "none");
        btnHiddenGo.Style.Add(HtmlTextWriterStyle.Padding, "0");
        btnHiddenGo.Style.Add(HtmlTextWriterStyle.Margin, "0");
        btnHiddenGo.Style.Add(HtmlTextWriterStyle.Overflow, "hidden");
        btnHiddenGo.Style.Add(HtmlTextWriterStyle.BackgroundColor, "transparent");
        //if (String.IsNullOrEmpty(Page.Form.DefaultButton))
        //{
        //    Page.Form.DefaultButton = btnHiddenGo.UniqueID;
        //}
        //btnHiddenGo.Style.Add(HtmlTextWriterStyle.Display, "none");
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        IButtonControl btn = (sender as IButtonControl);
        int index = int.MaxValue;
        if (Int32.TryParse(tbPage.Text, out index))
        {
            if (index <= 0)
            {
                index = 1;
            }
            else if (index > AssociatedGridView.PageCount)
            {
                index = AssociatedGridView.PageCount;
            }
        }
        btn.CommandArgument = index.ToString();
    }

    protected void tbPage_Init(object sender, EventArgs e)
    {
        tbPage.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
        tbPage.Style.Add(HtmlTextWriterStyle.Width, "40px");
        tbPage.Attributes.Add("onkeypress", "javascript:return WebForm_FireDefaultButton(event, '" + btnHiddenGo.ClientID + "');");
    }

    protected void lblTotalPages_Init(object sender, EventArgs e)
    {
        lblTotalPages.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
        lblTotalPages.Style.Add(HtmlTextWriterStyle.Color, "Black");
        lblTotalPages.ForeColor = System.Drawing.Color.White;
    }

    protected void divControl_Init(object sender, EventArgs e)
    {
        divControl.Style.Add(HtmlTextWriterStyle.MarginTop, "2px");
        divControl.Style.Add(HtmlTextWriterStyle.MarginLeft, "5px");
        divControl.Style.Add("float", "left");
    }
    #endregion

    #region Displayed Items Prerender
    protected void lblDisplayedItems_PreRender(object sender, EventArgs e)
    {        
        if (TotalCount == 0)
        {
            lblDisplayedItems.Text = EmptyMessage;
        }
        else
        {
            lblDisplayedItems.Text = String.Format(DisplayMessage, ShowingFirst,
                ShowingLast, TotalCount);
        }
    }
    #endregion

    #region Displayed Items Properties
    private static string totalCountKey = "TotalCount";
    private static string showingFirstKey = "ShowingFirst";
    private static string showingLastKey = "ShowingLast";
    private int TotalCount
    {
        get
        {
            string sTotalCount = AssociatedGridView.Attributes[totalCountKey];
            int totalCount = 0;
            if (sTotalCount != null)
            {
                Int32.TryParse(sTotalCount, out totalCount);
            }
            return totalCount;
        }
    }

    private int ShowingFirst
    {
        get
        {
            string sShowingFirst = AssociatedGridView.Attributes[showingFirstKey];
            int showingFirst = 0;
            if (sShowingFirst != null)
            {
                Int32.TryParse(sShowingFirst, out showingFirst);
            }
            return showingFirst;
        }
    }
    private int ShowingLast
    {
        get
        {
            string sShowingLast = AssociatedGridView.Attributes[showingLastKey];
            int showingLast = 0;
            if (sShowingLast != null)
            {
                Int32.TryParse(sShowingLast, out showingLast);
            }
            return showingLast;
        }
    }
    #endregion

    #region Displayed Items Initialize
    protected void lblDisplayedItems_Init(object sender, EventArgs e)
    {
        lblDisplayedItems.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
        lblDisplayedItems.Style.Add(HtmlTextWriterStyle.Color, "Black");
        lblDisplayedItems.ForeColor = System.Drawing.Color.White;
    }
    #endregion

    #region Page Size Properties
    private string ItemsPerPageMessage;
    #endregion

    #region Page Size Initialize
    protected void lblItemsPerPage_Init(object sender, EventArgs e)
    {
        InitializeProperties();
        lblItemsPerPage.Style.Add(HtmlTextWriterStyle.Color, "Black");
        lblItemsPerPage.ForeColor = System.Drawing.Color.White;
        lblItemsPerPage.Text = ItemsPerPageMessage;
    }
    #endregion

    #region Page Size Prerender
    protected void ddlPageSize_PreRender(object sender, EventArgs e)
    {
        string pageSize = AssociatedGridView.PageSize.ToString();
        ListItem li = ddlPageSize.Items.FindByValue(pageSize);
        if (li == null)
        {
            li = new ListItem(pageSize, pageSize);
            ddlPageSize.Items.Add(li);
        }
        ddlPageSize.SelectedValue = pageSize;
    }
    #endregion

    #region Page Size Events
    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        int pageSize = Int32.Parse((sender as DropDownList).SelectedValue);
        AssociatedGridView.PageIndex = 0;
        AssociatedGridView.PageSize = pageSize;
    }
    #endregion
}