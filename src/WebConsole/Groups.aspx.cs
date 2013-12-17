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

using System.Text.RegularExpressions;

using Microsoft.Win32;

using ARM2_dbcontrol.DataBase;
using ARM2_dbcontrol.Filters;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Service.TaskAssignment;
using Vba32.ControlCenter.SettingsService;

using VirusBlokAda.Vba32CC.Policies;
using VirusBlokAda.Vba32CC.Policies.General;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using VirusBlokAda.RemoteOperations.MsiInfo;
using VirusBlokAda.RemoteOperations.RemoteInstall;
using VirusBlokAda.RemoteOperations.Common;
using ARM2_dbcontrol.GroupEntities;
using VirusBlokAda.Vba32CC.Groups;


//!-OPTM Вынести выдачу задач в отдельный partial-файл

/// <summary>
/// Computers page
/// </summary>
public partial class Groups : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = Resources.Resource.PageGroupsTitle;
        if (!IsPostBack)
        {
            InitFields();
            Session["GroupSelectAll"] = true;
            Page.Form.Attributes.Add("onkeydown", "OnKeyDown()");
        }
    }

    #region Initialization...

    /// <summary>
    /// Инициализирует сессионные переменные на случай их вылета
    /// </summary>
    private void InitializeSession()
    {
        if (Session["GroupPageSize"] == null)
            Session["GroupPageSize"] = 10;						//page size	

        if (Session["GroupSelectAll"] == null)
            Session["GroupSelectAll"] = true;					//select mode

        if (Session["GroupSorting"] == null)
            Session["GroupSorting"] = "GroupName ASC";		//sort expression

        //ALARMA
        if (Session["GroupFilters"] == null)
            Session["GroupFilters"] = new GroupFilterCollection(Profile.GroupFilters);

        if (Session["TaskUser"] == null)
            Session["TaskUser"] = new TaskUserCollection(Profile.TasksList);

        if (Session["GroupTopControlVisible"] == null)
            Session["GroupTopControlVisible"] = false;

        if (Session["GroupBottomControlVisible"] == null)
            Session["GroupBottomControlVisible"] = false;
    }

    /// <summary>
    /// Init start visibility mode of panel
    /// </summary>
    /// <param name="sessionName"></param>
    /// <param name="div"></param>
    /// <param name="ib"></param>
    private void InitShowPanelImageButton(string sessionName, HtmlGenericControl div, ImageButton ib)
    {
        if ((bool)Session[sessionName])
        {
            div.Style["visibility"] = "visible";
            div.Style["height"] = "auto";
        }
        else
        {
            div.Style["visibility"] = "hidden";
            div.Style["height"] = "0px";
            div.Style["overflow"] = "hidden";
        }

        if (div.Style["visibility"] != "visible")
            ib.ImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/arrow_down.gif";
        else
            ib.ImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/arrow_up.gif";
    }

    /// <summary>
    /// Инициализация полей: лейблы, линки и етк.
    /// </summary>
    protected override void InitFields()
    {
        InitializeSession();

        btnClose.Text = Resources.Resource.Close;

        btnDelete.Text = Resources.Resource.Delete;
        btnDelete.Attributes.Add("onclick",
            "return confirm('" + Resources.Resource.AreYouSureFilter + "');");
        btnEditFilter.Text = Resources.Resource.Edit;


        //Цикл который позволяет определить доступные режимы отображения Web Parts 
        foreach (WebPartDisplayMode displayMode in WebPartManager1.SupportedDisplayModes)
            ddlWebPartState.Items.Add(displayMode.Name);

        lblDefaults.Text = Resources.Resource.Clear;
        //lbtnGiveTask.Text = Resources.Resource.GiveTask;
        lbtnExcel.Text = Resources.Resource.ExportToExcel;

        pcPaging.CurrentPageIndex = 1;
        pcPaging.PageCount = 1;
        pcPaging.PageText = Resources.Resource.Page;
        pcPaging.OfText = Resources.Resource.Of;
        pcPaging.NextText = Resources.Resource.Next;
        pcPaging.PrevText = Resources.Resource.Prev;

        pcPaging.HomeText = Resources.Resource.HomePaging;
        pcPaging.LastText = Resources.Resource.LastPaging;

        pcPagingTop.CurrentPageIndex = 1;
        pcPagingTop.PageCount = 1;
        pcPagingTop.PageText = Resources.Resource.Page;
        pcPagingTop.OfText = Resources.Resource.Of;
        pcPagingTop.NextText = Resources.Resource.Next;
        pcPagingTop.PrevText = Resources.Resource.Prev;

        pcPagingTop.HomeText = Resources.Resource.HomePaging;
        pcPagingTop.LastText = Resources.Resource.LastPaging;

        //Page size init

        if (ddlPageSize.Items.Count == 0)
        {
            ddlPageSize.Items.Add("1");
            ddlPageSize.Items.Add("10");
            ddlPageSize.Items.Add("25");
            ddlPageSize.Items.Add("50");
            ddlPageSize.Items.Add("100");
        }


        btnApplyFilter.Text = Resources.Resource.Apply;
        lbtnFilter.Text = Resources.Resource.Create;
        WebPartManager1.WebParts[0].Title = Resources.Resource.Main;

            WebPartManager1.Zones[0].MinimizeVerb.Text = Resources.Resource.Minimize;
            WebPartManager1.Zones[0].RestoreVerb.Text = Resources.Resource.Restore;
            WebPartManager1.Zones[0].CloseVerb.Text = Resources.Resource.Close;

        UpdateData();

        ddlPageSize.SelectedValue = Convert.ToString(Session["GroupPageSize"]);



        //Init start state of panel
        InitShowPanelImageButton("GroupTopControlVisible", divTop, imbtnTopControl);
        InitShowPanelImageButton("GroupBottomControlVisible", divBottom, imbtnBottomControl);

        List<string> filtersName = new List<string>();
        filtersName.Add(Resources.Resource.TemporaryFilter);
        GroupFilterCollection collection = (GroupFilterCollection)Session["GroupFilters"];
        foreach (GroupFilterEntity filter in collection)
            filtersName.Add(filter.FilterName);

        ddlFilter.DataSource = filtersName;
        ddlFilter.DataBind();


        if (Session["CurrentGroupFilter"] != null)
        {
            GroupFilterEntity filter = (GroupFilterEntity)Session["CurrentGroupFilter"];
            if (filter.FilterName != "")
                ddlFilter.SelectedValue = filter.FilterName;
            else
                ddlFilter.SelectedIndex = 0;

            cmpfltMain.LoadFilter(filter);
        }
        if (ddlFilter.SelectedIndex == 0)
        {
            divEditFilter.Visible = false;
            divDelete.Visible = false;
            divFilterHeader.Attributes["class"] = "GiveButton";
        }
        else divFilterHeader.Attributes["class"] = "GiveButton1";

        if ((!Roles.IsUserInRole("Administrator")) && (!Roles.IsUserInRole("Operator")))
        {
            divBottom.Style["visibility"] = "hidden";
            divBottom.Style["height"] = "0px";
            divTop.Style["overflow"] = "hidden";
            ddlTaskName.Visible = false;
            lbtnGive.Visible = false;
            lbtnSave.Visible = false;
            lbtnDelete.Visible = false;
            imbtnBottomControl.Visible = false;
            lblTaskName.Text = "";
        }
        else
        {
            InitFieldsTask();

        }
    }

    #endregion

    protected void ddlWebPartState_SelectedIndexChanged(object sender, EventArgs e)
    {
        WebPartDisplayMode displayMode;
        //Определяем выбранный режим отображения 
        displayMode = WebPartManager1.SupportedDisplayModes[ddlWebPartState.SelectedValue];
        //Устанавливаем режим отображения 
        WebPartManager1.DisplayMode = displayMode;
    }


    private List<GroupEx> groups;
    /// <summary>
    /// Заполнение таблицы Computers на основании выбранных параметров
    /// </summary>
    void UpdateData()
    {
        InitializeSession();
        using (VlslVConnection conn = new VlslVConnection(
            ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {

            conn.OpenConnection();
            conn.CheckConnectionState(true);
            ///!-OPTM песдетс:)
            ///Получше разобраться, что я тут хотел сделать и почему это так
            if (Session["GroupCheckBoxs"] != null)
            {
                Hashtable htbl = (Hashtable)Session["GroupCheckBoxs"];
                Regex reg = new Regex(@"\$DataList1\$\w+");
                ArrayList list = new ArrayList();
                foreach (string str in Request.Form)
                {
                    if (str == null) continue;
                    Match match = reg.Match(str);
                    if ((match.Success) && (htbl.ContainsKey(match.Value)))
                    {
                        list.Add(htbl[match.Value]);
                    }
                }
                Session["GroupCheckedValues"] = list;
            }

            Session["GroupCheckBoxs"] = new Hashtable();				//list of id checkbox/computersid


            GroupFilterEntity filter = new GroupFilterEntity(); ;

            int? showMode = (int?)Session["GroupShowMode"];
            if (!showMode.HasValue)
                showMode = 0;

            int count = 0;
            groups = new List<GroupEx>();

            GroupManager gmng = new GroupManager(conn);
            //switch mode to data
            switch (showMode)
            {                
                //use filter
                default:
                    if (Session["CurrentGroupFilter"] == null)
                    {
                        filter = new GroupFilterEntity();
                        filter.ComputerName = "*";
                        filter.GenerateSQLWhereStatement();
                    }
                    else
                    {
                        filter = (GroupFilterEntity)Session["CurrentGroupFilter"];
                        filter.GenerateSQLWhereStatement();
                    } 
                    conn.CheckConnectionState(true);
                    count = gmng.Count(filter.GetSQLWhereStatement);
                    groups = gmng.List(filter.GetSQLWhereStatement,
                            Convert.ToString(Session["GroupSorting"]),
                            pcPaging.CurrentPageIndex, Convert.ToInt32(Session["GroupPageSize"]));  
                    
                    ////заглушка
                    //provider = PoliciesState;
                    //policy = new Policy();
                    //count = provider.GetGroupsByPolicyCount(policy);
                    //groups = provider.GetGroupsByPolicyPage(policy, pcPaging.CurrentPageIndex, Convert.ToInt32(Session["GroupPageSize"]),
                    //    Convert.ToString(Session["GroupSorting"]));
                    break;
            }


            int pageSize = Convert.ToInt32(Session["GroupPageSize"]);
            int pageCount = (int)Math.Ceiling((double)count / pageSize);

            lblCount.Text = Resources.Resource.Found + ": " + count.ToString();

            pcPaging.PageCount = pageCount;
            pcPagingTop.PageCount = pageCount;

            Session["GroupsCurrentPageIndex"] = pcPaging.CurrentPageIndex;

            DataList1.DataSource = groups;
            DataList1.DataBind();


            conn.CloseConnection();
        }
    }

    /// <summary>
    /// Изменение размера страницы
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        pcPaging.CurrentPageIndex = 1;
        pcPagingTop.CurrentPageIndex = 1;
        Session["GroupPageSize"] = ddlPageSize.SelectedValue;
        Session["GroupSelectAll"] = true;
        UpdateData();
    }

    /// <summary>
    /// Изменение выбранного фильтра
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFilter.SelectedValue == Resources.Resource.TemporaryFilter)
        {
            cmpfltMain.Clear();
            //btnApplyFilter_Click(this, null);
            Session["CurrentGroupFilter"] = null;
            lblDefaults_Click(this, null);
            divFilterHeader.Attributes["class"] = "GiveButton";
            divEditFilter.Visible = false;
            divDelete.Visible = false;
            return;
        }
        else
        {
            divEditFilter.Visible = true;
            divDelete.Visible = true;
        }

        GroupFilterCollection collection = (GroupFilterCollection)Session["GroupFilters"];

        GroupFilterEntity fltr = new GroupFilterEntity();

        foreach (GroupFilterEntity filter in collection)
        {
            if (filter.FilterName == ddlFilter.SelectedValue)
                fltr = filter;
        }

        fltr.CheckFilters();
        fltr.GenerateSQLWhereStatement();


        if (fltr.GetSQLWhereStatement != String.Empty)
        {
            Session["CurrentGroupFilter"] = fltr;
        }
        else Session["CurrentGroupFilter"] = null;//default*/

        pcPaging.CurrentPageIndex = 1;
        pcPagingTop.CurrentPageIndex = 1;

        cmpfltMain.Clear();

        cmpfltMain.LoadFilter(fltr);
        Session["GroupShowMode"] = 0;
        Session["GroupSelectAll"] = true;
        Session["TempGroupComputers"] = null;
        UpdateData();

    }

    #region DataList ItemDataBound

    private string GetDateString(DateTime dt, string argbColor)
    {
        if (dt != DateTime.MinValue)
            return "<td width=100 bgcolor='" + argbColor + "'>" + dt + "</td>";

        return "<td style=\"text-align:center\" width=100 bgcolor='" + argbColor + "'>-</td>";

    }


    /// <summary>
    /// Привязка данных к таблице Computers
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //Header
        if (e.Item.ItemType == ListItemType.Header)
        {
            string argbColor = DataList1.HeaderStyle.BackColor.Name;

            if ((!Roles.IsUserInRole("Administrator")) && (!Roles.IsUserInRole("Operator")))
            {
                (e.Item.FindControl("lbtnSel") as LinkButton).Visible = false;
            }
            (e.Item.FindControl("lbtnID") as LinkButton).Visible = false;
            (e.Item.FindControl("tdID") as HtmlTableCell).Visible = false;

            (e.Item.FindControl("tdGroupName") as HtmlTableCell).BgColor = argbColor;

            (e.Item.FindControl("tdTotalCount") as HtmlTableCell).BgColor = argbColor;

            (e.Item.FindControl("tdActiveCount") as HtmlTableCell).BgColor = argbColor;

            (e.Item.FindControl("tdParentName") as HtmlTableCell).BgColor = argbColor;

            (e.Item.FindControl("tdDescription") as HtmlTableCell).BgColor = argbColor;


            (e.Item.FindControl("lbtnGroupName") as LinkButton).Text = Resources.Resource.GroupName;
            (e.Item.FindControl("lbtnTotalCount") as LinkButton).Text = Resources.Resource.TotalCount;
            (e.Item.FindControl("lbtnActiveCount") as LinkButton).Text = Resources.Resource.ActiveCount;
            (e.Item.FindControl("lbtnParentName") as LinkButton).Text = Resources.Resource.Parent;
            (e.Item.FindControl("lbtnGroupComment") as LinkButton).Text = Resources.Resource.Description;

            string currentSorting = (string)Session["GroupSorting"];
            string[] name = currentSorting.Split(' ');
            if (name[1] == "ASC")
            {
                (e.Item.FindControl("lbtn" + name[0]) as LinkButton).Text += " \u2193";
            }
            else
            {
                (e.Item.FindControl("lbtn" + name[0]) as LinkButton).Text += " \u2191";
            }
        }
        //Item
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {

            Hashtable htbl = (Hashtable)Session["GroupCheckBoxs"];

            string s;
            string argbColor;
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                argbColor = DataList1.AlternatingItemStyle.BackColor.Name;
            }
            else
            {
                argbColor = DataList1.ItemStyle.BackColor.Name;
            }

            try
            {
                if ((e.Item.ItemIndex + 1) < 10)
                    s = "$DataList1$ctl0" + Convert.ToString(e.Item.ItemIndex + 1);
                else
                    s = "$DataList1$ctl" + Convert.ToString(e.Item.ItemIndex + 1);

                htbl.Add(s, ((GroupEx)e.Item.DataItem).ID);

                //!-OPTM Юзать StringBuilder фиксированной длины было бы лучше с точки
                //зрения быстродействия
                s = "";


                if ((!Roles.IsUserInRole("Administrator")) && (!Roles.IsUserInRole("Operator")))
                {
                    (e.Item.FindControl("check") as CheckBox).Visible = false;
                }

                if (((GroupEx)e.Item.DataItem).TotalCount == 0) (e.Item.FindControl("check") as CheckBox).Enabled = false;

                //s += "<td align='center' width=30>" + ((GroupEx)e.Item.DataItem).ID + "</td>";

                s += "<td align='center' width=100 bgcolor='" + argbColor + "'>" + ((GroupEx)e.Item.DataItem).Name + "</td>";


                    s += "<td align='center' width=100 bgcolor='" + argbColor + "'>" + ((GroupEx)e.Item.DataItem).TotalCount + "</td>";
                    s += "<td align='center' width=100 bgcolor='" + argbColor + "'>" + ((GroupEx)e.Item.DataItem).ActiveCount + "</td>";
                    
                    (e.Item.FindControl("tdParentName") as HtmlTableCell).BgColor = argbColor;
                    (e.Item.FindControl("lbtnParentName") as Label).Text = String.IsNullOrEmpty(((GroupEx)e.Item.DataItem).ParentName) ? "-" : ((GroupEx)e.Item.DataItem).ParentName;



                    (e.Item.FindControl("lbtnGroupComment") as Label).Visible = true;
                    (e.Item.FindControl("lbtnGroupComment") as Label).BackColor = System.Drawing.Color.FromName(argbColor);
                    (e.Item.FindControl("tdDescription") as HtmlTableCell).BgColor = argbColor;
                    string comment = ((GroupEx)e.Item.DataItem).Comment;
                    if (comment == null) comment = String.Empty;
                    (e.Item.FindControl("lbtnGroupComment") as Label).Text = Anchor.FixString(comment, 16);

                    if ((e.Item.FindControl("lbtnGroupComment") as Label).Text == String.Empty)
                        (e.Item.FindControl("lbtnGroupComment") as Label).Text = '[' + Resources.Resource.Empty + ']';



                    if (Session["GroupsCheckedValues"] != null)
                {
                    ArrayList list = (ArrayList)Session["GroupsCheckedValues"];
                    if (list.Contains(((GroupEx)e.Item.DataItem).ID))
                        (e.Item.FindControl("check") as CheckBox).Checked = true;
                }

                e.Item.DataItem = s;
                e.Item.DataBind();
            }
            //!-- вылет при добавлении - грит что есть такие ключи, если есть одинаковые записи
            //!-OPTM Выводить дебаг сообщения или писать в лог
            catch
            {
            }
        }
    }

    #endregion

    #region DataList ItemCommand

    /// <summary>
    /// Item Commands к Data list
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
    {
        InitializeSession();
        switch (e.CommandName)
        {
            case "SortCommand":
                //Sorting data from selected field
                if (Session["GroupSorting"].ToString() == e.CommandArgument + " ASC")
                    Session["GroupSorting"] = e.CommandArgument + " DESC";
                else
                    Session["GroupSorting"] = e.CommandArgument + " ASC";
                DataList1.EditItemIndex = -1;
                UpdateData();
                break;

            case "SelectCommand":
                //Select or deselect all checkboxes in DataList
                bool checkedbox = true;
                if (Session["GroupSelectAll"] != String.Empty)
                {
                    if (((bool)Session["GroupSelectAll"]) == true)
                    {
                        checkedbox = true;
                        (e.CommandSource as LinkButton).Text = "-";
                        Session["GroupSelectAll"] = false;
                    }
                    else
                    {
                        checkedbox = false;
                        (e.CommandSource as LinkButton).Text = "+";
                        Session["GroupSelectAll"] = true;
                    }
                }
                for (int i = 0; i < DataList1.Items.Count; i++)
                {
                    CheckBox cb = (CheckBox)DataList1.Items[i].FindControl("check");
                    if (cb != null && cb.Enabled) { cb.Checked = checkedbox; }
                }
                break;
        }
    }

    #endregion

    #region Paging events
    protected void pcPaging_NextPage(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        pcPaging.CurrentPageIndex = index;
        pcPagingTop.CurrentPageIndex = index;

        DataList1.EditItemIndex = -1;
        Page.MaintainScrollPositionOnPostBack = false;
        Anchor.ScrollToTop(Page);
        Session["GroupSelectAll"] = true;
        UpdateData();
    }

    protected void pcPaging_PrevPage(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        pcPaging.CurrentPageIndex = index;
        pcPagingTop.CurrentPageIndex = index;

        DataList1.EditItemIndex = -1;
        Page.MaintainScrollPositionOnPostBack = false;
        Anchor.ScrollToTop(Page);
        Session["GroupSelectAll"] = true;
        UpdateData();
    }

    protected void pcPaging_HomePage(object sender, EventArgs e)
    {
        pcPaging.CurrentPageIndex = 1;
        pcPagingTop.CurrentPageIndex = 1;

        DataList1.EditItemIndex = -1;
        Page.MaintainScrollPositionOnPostBack = false;
        Anchor.ScrollToTop(Page);
        Session["GroupSelectAll"] = true;
        UpdateData();
    }
    protected void pcPaging_LastPage(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).PageCount;
        pcPaging.CurrentPageIndex = index;
        pcPagingTop.CurrentPageIndex = index;

        DataList1.EditItemIndex = -1;
        Page.MaintainScrollPositionOnPostBack = false;
        Anchor.ScrollToTop(Page);
        Session["GroupSelectAll"] = true;
        UpdateData();
    }
    #endregion

    #region Filter button click

    /// <summary>
    /// Apply click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnApplyFilter_Click(object sender, EventArgs e)
    {
        divFilterHeader.Attributes["class"] = "GiveButton1";
        GroupFilterEntity filter = new GroupFilterEntity();

        cmpfltMain.GetCurrentStateFilter(ref filter);

        filter.CheckFilters();
        filter.GenerateSQLWhereStatement();

        pcPaging.CurrentPageIndex = 1;
        pcPagingTop.CurrentPageIndex = 1;


        if (filter.GetSQLWhereStatement != String.Empty)
            Session["CurrentGroupFilter"] = filter;
        else
            Session["CurrentGroupFilter"] = null;

        ddlFilter.SelectedValue = Resources.Resource.TemporaryFilter;

        Session["GroupShowMode"] = 0;
        UpdateData();
    }

    /// <summary>
    /// Defaults click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lblDefaults_Click(object sender, EventArgs e)
    {
        divFilterHeader.Attributes["class"] = "GiveButton";

        cmpfltMain.Clear();

        ddlFilter.SelectedValue = Resources.Resource.TemporaryFilter;

        divEditFilter.Visible = false;
        divDelete.Visible = false;

        Session["GroupShowMode"] = null;
        Session["CurrentGroupFilter"] = null;
        UpdateData();
    }

    /// <summary>
    /// Create filter click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnFilter_Click(object sender, EventArgs e)
    {
        GroupFilterEntity filter = new GroupFilterEntity();

        cmpfltMain.GetCurrentStateFilter(ref filter);

        filter.GenerateSQLWhereStatement();

        //if (ddlFilter.SelectedValue != Resources.Resource.TemporaryFilter)
        //    filter.FilterName = ddlFilter.SelectedValue;

        Session["CurrentGroupFilter"] = filter;
        
        Response.Redirect("GroupFilters.aspx");
    }


    /// <summary>
    ///  Top control show/hide
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnTopControl_Click(object sender, EventArgs e)
    {
        InitializeSession();

        bool tcVis = (bool)Session["GroupTopControlVisible"];
        Session["GroupTopControlVisible"] = !tcVis;

        InitShowPanelImageButton("GroupTopControlVisible", divTop, imbtnTopControl);


        if (Session["CurrentGroupFilter"] != null)
        {
            GroupFilterEntity filter = (GroupFilterEntity)Session["CurrentGroupFilter"];
            cmpfltMain.LoadFilter(filter);
        }

        UpdateData();

    }


    /// <summary>
    /// Edit filter click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEditFilter_Click(object sender, EventArgs e)
    {
        string selectedFilter = ddlFilter.SelectedValue;
        Response.Redirect("GroupFilters.aspx?Filter=" + selectedFilter);
    }

    /// <summary>
    /// Delete filter button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {

        GroupFilterCollection collection = new GroupFilterCollection(Profile.GroupFilters);

        collection.Delete(ddlFilter.SelectedValue);
        collection = collection.Deserialize();

        Profile.CompFilters = collection.Serialize();
        Session["GroupFilters"] = collection;

        Session["CurrentGroupFilter"] = null;
        //ddlFilter.SelectedIndex = 0;

        InitFields();

        lblDefaults_Click(this, null);

    }

    #endregion

    #region Task Panel methods


    /// <summary>
    /// Initialize fields tasks
    /// </summary>
    private void InitFieldsTask()
    {
        InitializeSession();

        lblTaskName.Text = Resources.Resource.TaskName;

        cboxSelectAll.Text = Resources.Resource.TaskGiveAllGroups;
        cboxSelectAll.Visible = true;

        lbtnGive.Text = Resources.Resource.GiveTask;
        lbtnSave.Text = Resources.Resource.Save;
        lbtnDelete.Text = Resources.Resource.Delete;
        lbtnDelete.Attributes.Add("onclick",
            "return confirm('" + Resources.Resource.AreYouSureTask + "');");


        List<string> taskName = new List<string>();
        taskName.Add(Resources.Resource.Process);
        taskName.Add(Resources.Resource.SendFile);
        taskName.Add(Resources.Resource.TaskNameRunScanner);
        taskName.Add(Resources.Resource.MenuSystemInfo);
        taskName.Add(Resources.Resource.TaskNameListProcesses);
        taskName.Add(Resources.Resource.TaskNameComponentState);
        taskName.Add(Resources.Resource.TaskUninstall);
        taskName.Add(Resources.Resource.TaskAgentSettings);
        taskName.Add(Resources.Resource.CongLdrConfigureLoader);
        taskName.Add(Resources.Resource.CongLdrConfigureMonitor);
        taskName.Add(Resources.Resource.TaskNameConfigureQuarantine);
        //taskName.Add(Resources.Resource.TaskNameConfigureProactiveProtection);
        //taskName.Add(Resources.Resource.ConfigureScheduler);
        //taskName.Add(Resources.Resource.TaskNameConfigureFirewall);
        taskName.Add(Resources.Resource.TaskNameRestoreFileFromQtn);
        taskName.Add(Resources.Resource.TaskNameConfigurePassword);
        taskName.Add(Resources.Resource.TaskChangeDeviceProtect);
        taskName.Add(Resources.Resource.TaskRequestPolicy);

        taskName.Add(Resources.Resource.TaskSeparator);

        taskName.Add(Resources.Resource.TaskNameVba32LoaderEnable);
        taskName.Add(Resources.Resource.TaskNameVba32LoaderDisable);
        taskName.Add(Resources.Resource.TaskNameVba32MonitorEnable);
        taskName.Add(Resources.Resource.TaskNameVba32MonitorDisable);
        taskName.Add(Resources.Resource.TaskNameVba32ProgramAndDataBaseUpdate);


        TaskUserCollection collection = (TaskUserCollection)Session["TaskUser"];
        //Sort by name
        List<string> nameCollection = new List<string>();
        foreach (TaskUserEntity task in collection)
        {
            if (task.Type != TaskType.Firewall && task.Type != TaskType.ConfigureSheduler)
                nameCollection.Add(task.Name);
        }
        nameCollection.Sort();
        //
        foreach (string task in nameCollection)
        {
            taskName.Add(task);
        }

        ddlTaskName.DataSource = taskName;
        ddlTaskName.DataBind();

        if (ddlTaskName.Items.Count == 0)
            lbtnGive.Enabled = false;

        ddlTaskName_SelectedIndexChanged(this, null);

    }


    /// <summary>
    /// Bottom 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnBottomControl_Click(object sender, EventArgs e)
    {
        InitializeSession();
        bool tcVis = (bool)Session["GroupBottomControlVisible"];
        Session["GroupBottomControlVisible"] = !tcVis;
        InitShowPanelImageButton("GroupBottomControlVisible", divBottom, imbtnBottomControl);
        UpdateData();
        ddlTaskName_SelectedIndexChanged(sender, e);
    }

    /// <summary>
    /// Fill computer arrays 
    /// </summary>
    /// <param name="ipAddr"></param>
    /// <param name="compName"></param>
    private bool InitSelectedComputers(ref string[] ipAddr, ref string[] compName, ref string[] vbaVersion)
    {
        bool isAllSelected = cboxSelectAll.Checked;
        List<Int16> list = new List<Int16>();

        if (!isAllSelected)
        {
            try
            {
                if (Session["GroupCheckBoxs"] == null)
                    throw new Exception(Resources.Resource.NoSelected);
            }
            catch (Exception e)
            {
                lblMessage.Text = e.Message;
                mpPicture.Attributes["class"] = "ModalPopupPictureError";
                CorrectPositionModalPopup();
                ModalPopupExtender.Show();
                return false;
            }
            //Получаем коллекцию выбранных ID групп
            Hashtable htbl = (Hashtable)Session["GroupCheckBoxs"];
            Regex reg = new Regex(@"\$DataList1\$\w+");

            foreach (string str in Request.Form)
            {
                Match match = reg.Match(str);
                if ((match.Success) && (htbl.ContainsKey(match.Value)))
                {
                    list.Add(Convert.ToInt16(htbl[match.Value]));
                }
            }

            //Не было ничего выбранно
            try
            {
                if (list.Count == 0)
                    throw new Exception(Resources.Resource.NoSelectedGroups);
            }
            catch (Exception e)
            {
                lblMessage.Text = e.Message;
                mpPicture.Attributes["class"] = "ModalPopupPictureError";
                CorrectPositionModalPopup();
                ModalPopupExtender.Show();
                return false;
            }

            Session["SelectedGroupID"] = list;
        }

        InitializeSession();


        if ((Session["SelectedGroupID"] == null) && (!isAllSelected))
            throw new Exception(Resources.Resource.ErrorCriticalError + ": Session['SelectedGroupID'] == null ");

        //All computers who matches current filter
        if (isAllSelected)
        {
            //!-OPTM Подобный код встречается не раз..
            GroupFilterEntity filter;
            if (Session["CurrentGroupFilter"] == null)
            {
                filter = new GroupFilterEntity();
                filter.GroupName = "*";
                filter.GenerateSQLWhereStatement();
            }
            else
                filter = (GroupFilterEntity)Session["CurrentGroupFilter"];

            //We are using the temporary group
            if (Session["TempGroupComputers"] != null)
            {
                filter.GetSQLWhereStatement =
                    ((FilterEntity)Session["TempGroup"]).GetSQLWhereStatement;
            }


            //give computer list from db..
            using (VlslVConnection conn = new VlslVConnection(
                ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
            {
                GroupManager gmng = new GroupManager(conn);

                conn.OpenConnection();
                conn.CheckConnectionState(true);

                int count = gmng.Count(filter.GetSQLWhereStatement);

                List<GroupEx> groupsList = gmng.List(filter.GetSQLWhereStatement,
                    Convert.ToString(Session["GroupSorting"]), 1, count);

                List<ComputersEntity> compsList = new List<ComputersEntity>();

                

                foreach (GroupEx nextGroup in groupsList)
                {
                    foreach(ComputersEntity nextComp in gmng.GetComputersByGroup(nextGroup.ID))
                    {
                        compsList.Add(nextComp);
                    }
                }

                conn.CloseConnection();
                ipAddr = new string[compsList.Count];
                compName = new string[compsList.Count];
                vbaVersion = new string[compsList.Count];
                for (int i = 0; i < compsList.Count; i++)
                {
                    ipAddr[i] = compsList[i].IPAddress;
                    compName[i] = compsList[i].ComputerName;
                    vbaVersion[i] = compsList[i].Vba32Version;
                }
            }
        }
        else
        {
            using (VlslVConnection conn = new VlslVConnection(
                ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
            {
                GroupManager gmng = new GroupManager(conn);

                conn.OpenConnection();
                conn.CheckConnectionState(true);
                
                List<ComputersEntity> compsList = new List<ComputersEntity>();

                foreach (Int16 next in (List<Int16>)Session["SelectedGroupID"])
                {
                    foreach (ComputersEntity nextComp in gmng.GetComputersByGroup(next))
                    {
                        compsList.Add(nextComp);
                    }
                }
                conn.CloseConnection();
                ipAddr = new string[compsList.Count];
                compName = new string[compsList.Count];
                vbaVersion = new string[compsList.Count];
                for (int i = 0; i < compsList.Count; i++)
                {
                    ipAddr[i] = compsList[i].IPAddress;
                    compName[i] = compsList[i].ComputerName;
                    vbaVersion[i] = compsList[i].Vba32Version;
                }
            }
        }


        if (ipAddr.Length != compName.Length)
            throw new Exception(Resources.Resource.ErrorCriticalError + ": ipAddr.Length!=compName.Length");
        return true;
    }

    private void CorrectPositionModalPopup()
    {
        try
        {
            ModalPopupExtender.X = int.Parse(hdnWidth.Value) / 2 - 200;
            ModalPopupExtender.Y = int.Parse(hdnHeight.Value) / 2 - 60;
        }
        catch
        {
            ModalPopupExtender.X = ModalPopupExtender.Y = 200;
        }
    }

    /// <summary>
    /// Give task button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnGive_Click(object sender, EventArgs e)
    {
        String userName = Anchor.GetStringForTaskGivedUser();
        string[] ipAddr = new string[0];
        string[] compName = new string[0];
        string[] vbaVersion = new string[0];
        if (!InitSelectedComputers(ref ipAddr, ref compName, ref vbaVersion)) return;

        Int64[] taskId = new Int64[compName.Length];

        string service = ConfigurationManager.AppSettings["Service"];
        string connStr = ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString;

        Vba32ControlCenterWrapper control = new Vba32ControlCenterWrapper(service);

        TaskUserEntity task = new TaskUserEntity();
        try
        {
            ARM2_dbcontrol.Generation.XmlBuilder builder = new ARM2_dbcontrol.Generation.XmlBuilder();
            if (tskCreateProcess.Visible == true)
            {
                task = tskCreateProcess.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskCreateProcess.ValidateFields();

                XmlTaskParser xml = new XmlTaskParser(task.Param);
                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }
                //ggg
                string str = Server.HtmlEncode(xml.GetValue(tskCreateProcess.TagCommandLine)).Replace("&#160;", " ").Replace("&amp;", "&");
                str = Server.HtmlDecode(str);

                control.PacketCreateProcess(taskId, ipAddr, str);

            }

            if (tskSendFile.Visible == true)
            {
                task = tskSendFile.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskSendFile.ValidateFields();
                //!--
                XmlTaskParser xml = new XmlTaskParser(task.Param);
                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }
                control.PacketSendFile(taskId, ipAddr, xml.GetValue(tskSendFile.TagSource),
                    xml.GetValue(tskSendFile.TagDestination));

            }

            if (tskSystemInfo.Visible == true)
            {
                task = tskSystemInfo.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskSystemInfo.ValidateFields();

                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }
                control.PacketSystemInfo(taskId, ipAddr);

            }

            if (tskListProcesses.Visible == true)
            {
                task = tskListProcesses.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskListProcesses.ValidateFields();

                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }
                control.PacketListProcesses(taskId, ipAddr);
            }

            if (tskComponentState.Visible == true)
            {
                task = tskComponentState.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskComponentState.ValidateFields();

                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }
                control.PacketComponentState(taskId, ipAddr);
            }


            if (tskConfigureLoader.Visible == true)
            {
                task = tskConfigureLoader.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskConfigureLoader.ValidateFields();

                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }
                string strtask = task.Param.Remove(0, builder.Top.Length);
                string s = @"<Type>ConfigureLoader</Type>";
                strtask = strtask.Replace(s, "");
                control.PacketConfigureSettings(taskId, ipAddr, strtask);
            }

            if (tskConfigureMonitor.Visible == true)
            {
                task = tskConfigureMonitor.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskConfigureMonitor.ValidateFields();
                //!--
                XmlTaskParser xml = new XmlTaskParser(task.Param);
                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }
                string strtask = task.Param;
                string s = @"<Type>ConfigureMonitor</Type>";
                strtask = strtask.Replace(s, "");
                control.PacketConfigureSettings(taskId, ipAddr, strtask);
            }

            if (tskRunScanner.Visible == true)
            {
                task = tskRunScanner.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskRunScanner.ValidateFields();

                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);

                }
                control.PacketCreateProcess(taskId, ipAddr, tskRunScanner.GenerateCommandLine(task));

            }

            if (tskConfigurePassword.Visible == true)
            {
                task = tskConfigurePassword.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskConfigurePassword.ValidateFields();

                XmlTaskParser xml = new XmlTaskParser(task.Param);
                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }

                string strtask = task.Param.Remove(0, builder.Top.Length);
                string s = @"<Type>ConfigurePassword</Type>";
                strtask = strtask.Replace(s, "");
                control.PacketConfigureSettings(taskId, ipAddr, strtask);
            }

            if (tskConfigureQuarantine.Visible == true)
            {
                task = tskConfigureQuarantine.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskConfigureQuarantine.ValidateFields();

                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }
                string strtask = task.Param.Remove(0, builder.Top.Length);
                string s = @"<Type>ConfigureQuarantine</Type>";
                strtask = strtask.Replace(s, "");
                control.PacketConfigureSettings(taskId, ipAddr, strtask);
            }

            if (tskRestoreFileFromQtn.Visible == true)
            {
                task = tskRestoreFileFromQtn.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskRestoreFileFromQtn.ValidateFields();


                XmlTaskParser xml = new XmlTaskParser(task.Param);
                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }

                control.PacketCreateProcess(taskId, ipAddr, tskRestoreFileFromQtn.GetCommandLine);
            }

            if (tskProactiveProtection.Visible == true)
            {
                task = tskProactiveProtection.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskProactiveProtection.ValidateFields();

                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }

                control.PacketCustomAction(taskId, ipAddr, tskProactiveProtection.BuildTask(task));
            }

            if (tskFirewall.Visible == true)
            {
                if ((tskFirewall.FindControl("ddlProfiles") as DropDownList).Items.Count == 0)
                    throw new ArgumentException(Resources.Resource.ErrorNotSelectProfile);
                TaskUserCollection collection = (TaskUserCollection)Session["TaskUser"];
                task = collection.Get(String.Format("Firewall:{0}", (tskFirewall.FindControl("ddlProfiles") as DropDownList).SelectedValue));

                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], Resources.Resource.TaskNameConfigureFirewall + ": " + task.Name.Substring(9), task.Param, userName, connStr);
                }

                //Написать реализацию отправки задачи
                //control.PacketCustomAction(taskId, ipAddr, tskFirewall.BuildTask(@"%VBA32%???", task.Param));
            }

            if (tskConfigureScheduler.Visible == true)
            {
                if ((tskConfigureScheduler.FindControl("ddlProfiles") as DropDownList).Items.Count == 0)
                    throw new ArgumentException(Resources.Resource.ErrorNotSelectProfile);
                TaskUserCollection collection = (TaskUserCollection)Session["TaskUser"];
                task = collection.Get(String.Format("Scheduler: {0}", (tskConfigureScheduler.FindControl("ddlProfiles") as DropDownList).SelectedValue));

                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], Resources.Resource.ConfigureScheduler + ": " + task.Name.Substring(11), task.Param, userName, connStr);
                }

                control.PacketCustomAction(taskId, ipAddr, tskConfigureScheduler.BuildTask(task.Param));
            }

            if (tskChangeDeviceProtect.Visible == true)
            {
                task = tskChangeDeviceProtect.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;

                XmlTaskParser xml = new XmlTaskParser(task.Param);
                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }

                control.PacketCustomAction(taskId, ipAddr, tskChangeDeviceProtect.BuildTask());
            }

            if (tskAgentSettings.Visible == true)
            {
                task = tskAgentSettings.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;

                XmlTaskParser xml = new XmlTaskParser(task.Param);
                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
                }

                control.PacketCustomAction(taskId, ipAddr, tskAgentSettings.BuildTask());
            }

            if (tskUninstall.Visible == true)
            {
                tskUninstall.ValidateFields();
                List<RemoteInstallEntity> list = new List<RemoteInstallEntity>();

                for (int i = 0; i < compName.Length; i++)
                {
                    RemoteInstallEntity r = new RemoteInstallEntity();
                    r.ComputerName = compName[i];
                    r.IP = ipAddr[i];
                    if (vbaVersion[i].Contains(Vba32VersionInfo.Vba32NTS))
                    {
                        r.VbaVersion = Vba32VersionInfo.Vba32NTS;
                    }
                    else if (vbaVersion[i].Contains(Vba32VersionInfo.Vba32NTW))
                    {
                        r.VbaVersion = Vba32VersionInfo.Vba32NTW;
                    }
                    else if (vbaVersion[i].Contains(Vba32VersionInfo.Vba32Vis))
                    {
                        r.VbaVersion = Vba32VersionInfo.Vba32Vis;
                    }
                    else if (vbaVersion[i].Contains(Vba32VersionInfo.Vba32Vista))
                    {
                        r.VbaVersion = Vba32VersionInfo.Vba32Vista;
                    }
                    else
                    {
                        r.VbaVersion = "unknown";
                    }
                    r.Guid = Vba32VersionInfo.GetGuid(r.VbaVersion);
                    if (r.Guid != "unknown")
                    {
                        list.Add(r);
                    }
                }
                Credentials credentials = new Credentials(tskUninstall.Domain, tskUninstall.Login,
                    tskUninstall.Password);
                int maxThreads = 5;
                RemoteInstaller ri = new RemoteInstaller(credentials, maxThreads, ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                ri.UninstallAll(list, tskUninstall.DoRestart);               
                Response.Redirect("~/TasksInstall.aspx");
            }

            if (tskRequestPolicy.Visible == true)
            {
                task = tskRequestPolicy.GetCurrentState();
                task.Name = ddlTaskName.SelectedValue;
                tskRequestPolicy.ValidateFields();

                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], task.Name, tskRequestPolicy.BuildParam(task.Param), userName, connStr);
                }
                control.PacketCustomAction(taskId, ipAddr, task.Param);
            }
        }
        catch (ArgumentException argEx)
        {
            lblMessage.Text = argEx.Message;
            mpPicture.Attributes["class"] = "ModalPopupPictureError";
            CorrectPositionModalPopup();
            ModalPopupExtender.Show();
            return;
        }
        catch
        {
            lblMessage.Text = Resources.Resource.Error + ". " + Resources.Resource.ErrorService;
            mpPicture.Attributes["class"] = "ModalPopupPictureError";
            CorrectPositionModalPopup();
            ModalPopupExtender.Show();
            return;
        }

        if (control.GetLastError() == "")
        {
            lblMessage.Text = task.Name + ": " + Resources.Resource.TaskGived;
            mpPicture.Attributes["class"] = "ModalPopupPictureSuccess";
        }
        else
        {
            lblMessage.Text = Resources.Resource.Error + ". " + control.GetLastError();
            mpPicture.Attributes["class"] = "ModalPopupPictureError";
        }
        CorrectPositionModalPopup();
        ModalPopupExtender.Show();
    }

    /// <summary>
    /// Update Info
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnUpdateInfo_Click(object sender, EventArgs e)
    {
        String userName = Anchor.GetStringForTaskGivedUser();
        string[] ipAddr = new string[0];
        string[] compName = new string[0];
        string[] vbaVersion = new string[0];
        if (!InitSelectedComputers(ref ipAddr, ref compName, ref vbaVersion)) return;

        Int64[] taskId = new Int64[compName.Length];

        string service = ConfigurationManager.AppSettings["Service"];
        string connStr = ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString;

        Vba32ControlCenterWrapper control = new Vba32ControlCenterWrapper(service);

        TaskUserEntity task = new TaskUserEntity();
        try
        {
            ARM2_dbcontrol.Generation.XmlBuilder builder = new ARM2_dbcontrol.Generation.XmlBuilder();

            //tskSystemInfo 
            task = tskSystemInfo.GetCurrentState();
            task.Name = Resources.Resource.TaskNameSystemInfo;

            for (int i = 0; i < compName.Length; i++)
            {
                taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
            }
            control.PacketSystemInfo(taskId, ipAddr);
            //tskListProcesses
            task = tskListProcesses.GetCurrentState();
            task.Name = Resources.Resource.TaskNameListProcesses;

            for (int i = 0; i < compName.Length; i++)
            {
                taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
            }
            control.PacketListProcesses(taskId, ipAddr);
            //tskComponentState
            task = tskComponentState.GetCurrentState();
            task.Name = Resources.Resource.TaskNameComponentState;

            for (int i = 0; i < compName.Length; i++)
            {
                taskId[i] = PreServAction.CreateTask(compName[i], task.Name, task.Param, userName, connStr);
            }
            control.PacketComponentState(taskId, ipAddr);
        }
        catch (ArgumentException argEx)
        {
            lblMessage.Text = argEx.Message;
            mpPicture.Attributes["class"] = "ModalPopupPictureError";
            CorrectPositionModalPopup();
            ModalPopupExtender.Show();
            return;
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            mpPicture.Attributes["class"] = "ModalPopupPictureError";
            CorrectPositionModalPopup();
            ModalPopupExtender.Show();
            return;
        }

        lblMessage.Text = Resources.Resource.UpdateInfo + ": " + Resources.Resource.TaskGived;
        mpPicture.Attributes["class"] = "ModalPopupPictureSuccess";
        CorrectPositionModalPopup();
        ModalPopupExtender.Show();
    }

    /// <summary>
    /// Change task name
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTaskName_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitializeSession();

        string name = ddlTaskName.SelectedValue;

        if (name == Resources.Resource.TaskSeparator)
        {
            lbtnGive.Visible = false;
            lbtnDelete.Visible = false;
            lbtnSave.Visible = false;
            pnlTask.Visible = false;
            cboxSelectAll.Visible = false;
            return;
        }
        else
        {
            lbtnGive.Visible = true;
            lbtnDelete.Visible = true;
            lbtnSave.Visible = true;
            pnlTask.Visible = true;
            cboxSelectAll.Visible = true;
        }
        TaskUserCollection collection;
        TaskUserEntity task = new TaskUserEntity();


        //По умолчанию
        ARM2_dbcontrol.Generation.XmlBuilder xmlBuil = new ARM2_dbcontrol.Generation.XmlBuilder("root");
        //Для тех, у кого потребуется задать какие-то параметры(например загрузить/выгрузить Диспетчер)
        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("root");

        xmlBuil.Generate();

        //switch нельзя юзать из-за того, что ресурсы не константы..
        //!-OPTM Для использования надо  объявить константы.
        if (name == Resources.Resource.Process)
        {
            task.Type = TaskType.CreateProcess;
            task.Name = Resources.Resource.Process;
            task.Param = xmlBuil.Result;
            lbtnDelete.Visible = false;
        }
        else

            if (name == Resources.Resource.SendFile)
            {
                task.Type = TaskType.SendFile;
                task.Name = Resources.Resource.SendFile;
                task.Param = xmlBuil.Result;
                lbtnDelete.Visible = false;
            }
            else

                if (name == Resources.Resource.MenuSystemInfo)
                {
                    task.Type = TaskType.SystemInfo;
                    task.Name = Resources.Resource.MenuSystemInfo;
                    task.Param = xmlBuil.Result;
                    lbtnDelete.Visible = false;
                    lbtnSave.Visible = false;
                }
                else

                    if (name == Resources.Resource.TaskUninstall)
                    {
                        lbtnDelete.Visible = false;
                        lbtnSave.Visible = false;
                        task.Type = TaskType.Uninstall;
                        task.Name = Resources.Resource.TaskUninstall;
                    }
                    else

                        if (name == Resources.Resource.TaskNameListProcesses)
                        {
                            task.Type = TaskType.ListProcesses;
                            task.Name = Resources.Resource.TaskNameListProcesses;
                            task.Param = xmlBuil.Result;
                            lbtnDelete.Visible = false;
                            lbtnSave.Visible = false;
                        }
                        else
                            if (name == Resources.Resource.TaskNameComponentState)
                            {
                                task.Type = TaskType.ComponentState;
                                task.Name = Resources.Resource.TaskNameComponentState;
                                task.Param = xmlBuil.Result;
                                lbtnDelete.Visible = false;
                                lbtnSave.Visible = false;
                            }

                            else

                                if (name == Resources.Resource.CongLdrConfigureLoader)
                                {
                                    task.Type = TaskType.ConfigureLoader;
                                    task.Name = Resources.Resource.CongLdrConfigureLoader;
                                    task.Param = xmlBuil.Result;
                                    lbtnDelete.Visible = false;
                                }
                                else

                                    if (name == Resources.Resource.CongLdrConfigureMonitor)
                                    {
                                        task.Type = TaskType.ConfigureMonitor;
                                        task.Name = Resources.Resource.CongLdrConfigureMonitor;
                                        task.Param = xmlBuil.Result.Remove(0, xmlBuil.Top.Length);
                                        lbtnDelete.Visible = false;
                                    }
                                    else

                                        if (name == Resources.Resource.TaskNameRunScanner)
                                        {
                                            task.Type = TaskType.RunScanner;
                                            task.Name = Resources.Resource.TaskNameRunScanner;
                                            task.Param = xmlBuil.Result;
                                            lbtnDelete.Visible = false;
                                        }
                                        else
                                            if (name == Resources.Resource.TaskNameConfigurePassword)
                                            {
                                                task.Type = TaskType.ConfigurePassword;
                                                task.Name = Resources.Resource.TaskNameConfigurePassword;
                                                task.Param = xmlBuil.Result;
                                                lbtnDelete.Visible = false;
                                                lbtnSave.Visible = false;
                                            }
                                            else
                                                if (name == Resources.Resource.TaskNameVba32LoaderEnable)
                                                {
                                                    task.Type = TaskType.CreateProcess;
                                                    task.Name = Resources.Resource.TaskNameVba32LoaderEnable;

                                                    string str = Resources.Resource.TaskParamVba32LoaderEnable;
                                                    xml.AddNode(tskCreateProcess.TagCommandLine, str);
                                                    xml.AddNode(tskCreateProcess.TagCommandSpec, "0");

                                                    xml.Generate();
                                                    task.Param = xml.Result;
                                                    lbtnDelete.Visible = false;
                                                    lbtnSave.Visible = false;
                                                }
                                                else
                                                    if (name == Resources.Resource.TaskNameVba32LoaderDisable)
                                                    {
                                                        task.Type = TaskType.CreateProcess;
                                                        task.Name = Resources.Resource.TaskNameVba32LoaderDisable;

                                                        string str = Resources.Resource.TaskParamVba32LoaderDisable;
                                                        xml.AddNode(tskCreateProcess.TagCommandLine, str);
                                                        xml.AddNode(tskCreateProcess.TagCommandSpec, "0");

                                                        xml.Generate();
                                                        task.Param = xml.Result;
                                                        lbtnDelete.Visible = false;
                                                        lbtnSave.Visible = false;
                                                    }
                                                    else
                                                        if (name == Resources.Resource.TaskNameVba32MonitorEnable)
                                                        {
                                                            task.Type = TaskType.CreateProcess;
                                                            task.Name = Resources.Resource.TaskNameVba32MonitorEnable;

                                                            string str = Resources.Resource.TaskParamVba32MonitorEnable;
                                                            xml.AddNode(tskCreateProcess.TagCommandLine, str);
                                                            xml.AddNode(tskCreateProcess.TagCommandSpec, "0");

                                                            xml.Generate();
                                                            task.Param = xml.Result;
                                                            lbtnDelete.Visible = false;
                                                            lbtnSave.Visible = false;
                                                        }
                                                        else
                                                            if (name == Resources.Resource.TaskNameVba32MonitorDisable)
                                                            {
                                                                task.Type = TaskType.CreateProcess;
                                                                task.Name = Resources.Resource.TaskNameVba32MonitorDisable;

                                                                string str = Resources.Resource.TaskParamVba32MonitorDisable;
                                                                xml.AddNode(tskCreateProcess.TagCommandLine, str);
                                                                xml.AddNode(tskCreateProcess.TagCommandSpec, "0");

                                                                xml.Generate();
                                                                task.Param = xml.Result;
                                                                lbtnDelete.Visible = false;
                                                                lbtnSave.Visible = false;
                                                            }
                                                            else
                                                                if (name == Resources.Resource.TaskNameVba32ProgramAndDataBaseUpdate)
                                                                {
                                                                    task.Type = TaskType.CreateProcess;
                                                                    task.Name = Resources.Resource.TaskNameVba32ProgramAndDataBaseUpdate;

                                                                    string str = Resources.Resource.TaskParamVba32ProgramAndDataBaseUpdate;
                                                                    xml.AddNode(tskCreateProcess.TagCommandLine, str);
                                                                    xml.AddNode(tskCreateProcess.TagCommandSpec, "0");

                                                                    xml.Generate();
                                                                    task.Param = xml.Result;
                                                                    lbtnDelete.Visible = false;
                                                                    lbtnSave.Visible = false;
                                                                }
                                                                else
                                                                    if (name == Resources.Resource.TaskNameConfigureQuarantine)
                                                                    {
                                                                        task.Type = TaskType.ConfigureQuarantine;
                                                                        task.Name = Resources.Resource.TaskNameConfigureQuarantine;
                                                                        task.Param = xmlBuil.Result;
                                                                        lbtnDelete.Visible = false;
                                                                    }
                                                                    else
                                                                        if (name == Resources.Resource.TaskNameConfigureFirewall)
                                                                        {
                                                                            task.Type = TaskType.Firewall;
                                                                            task.Name = Resources.Resource.TaskNameConfigureFirewall;
                                                                            task.Param = xmlBuil.Result;
                                                                            lbtnDelete.Visible = false;
                                                                            lbtnSave.Visible = false;
                                                                        }
                                                                        else
                                                                            if (name == Resources.Resource.TaskNameConfigureProactiveProtection)
                                                                            {
                                                                                task.Type = TaskType.ProactiveProtection;
                                                                                task.Name = Resources.Resource.TaskNameConfigureProactiveProtection;
                                                                                task.Param = xmlBuil.Result;
                                                                                lbtnDelete.Visible = false;                                                                                
                                                                            }
                                                                            else
                                                                                if (name == Resources.Resource.ConfigureScheduler)
                                                                                {
                                                                                    task.Type = TaskType.ConfigureSheduler;
                                                                                    task.Name = Resources.Resource.ConfigureScheduler;
                                                                                    task.Param = xmlBuil.Result;
                                                                                    lbtnDelete.Visible = false;
                                                                                    lbtnSave.Visible = false;
                                                                                }
                                                                                else
                                                                                    if (name == Resources.Resource.TaskChangeDeviceProtect)
                                                                                    {
                                                                                        task.Type = TaskType.ChangeDeviceProtect;
                                                                                        task.Name = Resources.Resource.TaskChangeDeviceProtect;
                                                                                        task.Param = xmlBuil.Result;
                                                                                        lbtnDelete.Visible = false;
                                                                                        lbtnSave.Visible = false;
                                                                                    }
                                                                                    else
                                                                                        if (name == Resources.Resource.TaskRequestPolicy)
                                                                                        {
                                                                                            task.Type = TaskType.RequestPolicy;
                                                                                            task.Name = Resources.Resource.TaskRequestPolicy;
                                                                                            task.Param = xmlBuil.Result;
                                                                                            lbtnDelete.Visible = false;
                                                                                            lbtnSave.Visible = false;
                                                                                        }
                                                                                        else
                                                                                            if (name == Resources.Resource.TaskAgentSettings)
                                                                                            {
                                                                                                task.Type = TaskType.AgentSettings;
                                                                                                task.Name = Resources.Resource.TaskAgentSettings;
                                                                                                task.Param = xmlBuil.Result;
                                                                                                lbtnDelete.Visible = false;
                                                                                                lbtnSave.Visible = false;
                                                                                            }
                                                                                        else
                                                                                            if (name == Resources.Resource.TaskNameRestoreFileFromQtn)
                                                                                            {
                                                                                                task.Type = TaskType.RestoreFileFromQtn;
                                                                                                task.Name = Resources.Resource.TaskNameRestoreFileFromQtn;


                                                                                                task.Param = xmlBuil.Result;
                                                                                                lbtnDelete.Visible = false;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                //User task
                                                                                                collection = (TaskUserCollection)Session["TaskUser"];
                                                                                                foreach (TaskUserEntity tsk in collection)
                                                                                                {
                                                                                                    if (tsk.Name == name)
                                                                                                        task = tsk;
                                                                                                }
                                                                                                lbtnDelete.Visible = true;
                                                                                            }

        tskCreateProcess.Visible = false;
        tskSendFile.Visible = false;
        tskListProcesses.Visible = false;
        //tskCancelTask.Visible = false;
        tskSystemInfo.Visible = false;
        tskConfigureLoader.Visible = false;
        tskConfigureMonitor.Visible = false;
        tskRunScanner.Visible = false;
        tskComponentState.Visible = false;
        tskConfigurePassword.Visible = false;
        tskConfigureQuarantine.Visible = false;
        tskRestoreFileFromQtn.Visible = false;
        tskProactiveProtection.Visible = false;
        tskFirewall.Visible = false;
        tskChangeDeviceProtect.Visible = false;
        tskRequestPolicy.Visible = false;
        tskConfigureScheduler.Visible = false;
        tskUninstall.Visible = false;
        tskAgentSettings.Visible = false;

        switch (task.Type)
        {
            case TaskType.CreateProcess:

                tskCreateProcess.LoadState(task);
                tskCreateProcess.Visible = true;
                break;

            case TaskType.SendFile:

                tskSendFile.LoadState(task);
                tskSendFile.Visible = true;
                break;

            case TaskType.SystemInfo:

                tskSystemInfo.LoadState(task);
                tskSystemInfo.Visible = true;
                break;


            case TaskType.ListProcesses:

                tskListProcesses.LoadState(task);
                tskListProcesses.Visible = true;
                break;

            case TaskType.ComponentState:

                tskComponentState.LoadState(task);
                tskComponentState.Visible = true;
                break;

            case TaskType.ConfigureLoader:

                tskConfigureLoader.LoadState(task);
                tskConfigureLoader.Visible = true;
                break;

            case TaskType.ConfigureMonitor:

                tskConfigureMonitor.InitFields();
                tskConfigureMonitor.LoadState(task);
                tskConfigureMonitor.Visible = true;

                break;

            case TaskType.RunScanner:

                tskRunScanner.InitFields();
                tskRunScanner.LoadState(task);
                tskRunScanner.Visible = true;
                break;

            case TaskType.ConfigurePassword:

                tskConfigurePassword.LoadState(task);
                tskConfigurePassword.Visible = true;
                break;

            case TaskType.ConfigureQuarantine:

                tskConfigureQuarantine.InitFields();
                tskConfigureQuarantine.LoadState(task);
                tskConfigureQuarantine.Visible = true;
                break;
            case TaskType.RestoreFileFromQtn:

                tskRestoreFileFromQtn.InitFields();
                tskRestoreFileFromQtn.LoadState(task);
                tskRestoreFileFromQtn.Visible = true;
                break;
            case TaskType.ProactiveProtection:

                tskProactiveProtection.InitFields();
                tskProactiveProtection.LoadState(task);
                tskProactiveProtection.Visible = true;
                break;
            case TaskType.Firewall:

                tskFirewall.InitFields();
                tskFirewall.Visible = true;
                break;

            case TaskType.ConfigureSheduler:

                tskConfigureScheduler.InitFields();
                tskConfigureScheduler.Visible = true;

                break;
            case TaskType.ChangeDeviceProtect:

                tskChangeDeviceProtect.InitFields();
                tskChangeDeviceProtect.Visible = true;
                break;
            case TaskType.RequestPolicy:

                tskRequestPolicy.LoadState(task);
                tskRequestPolicy.Visible = true;
                break;
            case TaskType.Uninstall:
                tskUninstall.Visible = true;
                break;
            case TaskType.AgentSettings:
                tskAgentSettings.LoadState(task);
                tskAgentSettings.Visible = true;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Filter Header Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnFilterHeader_Click(object sender, EventArgs e)
    {
        if (divFilterHeader.Attributes["class"] == "GiveButton")
        {
            btnApplyFilter_Click(sender, e);
        }
        else
        {
            lblDefaults_Click(sender, e);
        }
    }

    /// <summary>
    /// Save task click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnSave_Click(object sender, EventArgs e)
    {
        TaskUserCollection collection =
            new TaskUserCollection(Profile.TasksList);

        string name = ddlTaskName.SelectedValue;

        //Отсечь базовые задачи..
        TaskUserEntity task = new TaskUserEntity();
        ARM2_dbcontrol.Generation.XmlBuilder xmlBuil = new ARM2_dbcontrol.Generation.XmlBuilder("root");
        xmlBuil.Generate();

        string editing = "&Mode=Edit";
        //string editing = "";

        if (name == Resources.Resource.Process)
        {
            task.Type = TaskType.CreateProcess;
            task.Name = Resources.Resource.Process;
            task = tskCreateProcess.GetCurrentState();
        }
        else

            if (name == Resources.Resource.SendFile)
            {
                task.Type = TaskType.SendFile;
                task.Name = Resources.Resource.SendFile;
                task = tskSendFile.GetCurrentState();
            }
            else

                if (name == Resources.Resource.MenuSystemInfo)
                {
                    task.Type = TaskType.SystemInfo;
                    task.Name = Resources.Resource.MenuSystemInfo;
                    task = tskSystemInfo.GetCurrentState();
                }
                else

                    if (name == Resources.Resource.TaskNameListProcesses)
                    {
                        task.Type = TaskType.ListProcesses;
                        task.Name = Resources.Resource.TaskNameListProcesses;
                        task = tskListProcesses.GetCurrentState();
                    }
                    else
                        if (name == Resources.Resource.TaskNameComponentState)
                        {
                            task.Type = TaskType.ComponentState;
                            task.Name = Resources.Resource.TaskNameComponentState;
                            task = tskComponentState.GetCurrentState();
                        }

                        else

                            if (name == Resources.Resource.CongLdrConfigureLoader)
                            {
                                task.Type = TaskType.ConfigureLoader;
                                task.Name = Resources.Resource.CongLdrConfigureLoader;
                                task = tskConfigureLoader.GetCurrentState();

                            }
                            else

                                if (name == Resources.Resource.CongLdrConfigureMonitor)
                                {
                                    task.Type = TaskType.ConfigureMonitor;
                                    task.Name = Resources.Resource.CongLdrConfigureMonitor;
                                    task = tskConfigureMonitor.GetCurrentState();
                                }
                                else
                                    if (name == Resources.Resource.TaskNameConfigureProactiveProtection)
                                    {
                                        task = tskProactiveProtection.GetCurrentState();
                                        tskProactiveProtection.ValidateFields();
                                    }
                                    else
                                        if (name == Resources.Resource.TaskNameRunScanner)
                                        {
                                            task.Type = TaskType.RunScanner;
                                            task.Name = Resources.Resource.TaskNameRunScanner;
                                            task = tskRunScanner.GetCurrentState();
                                        }
                                        else
                                            if (name == Resources.Resource.TaskNameConfigureQuarantine)
                                            {
                                                task.Type = TaskType.ConfigureQuarantine;
                                                task.Name = Resources.Resource.TaskNameConfigureQuarantine;
                                                task = tskConfigureQuarantine.GetCurrentState();
                                            }

                                            else
                                                if (name == Resources.Resource.TaskNameRestoreFileFromQtn)
                                                {
                                                    //!OPTM---Строчка, возможно, излишняя, т.к. тип устанавливается в
                                                    //GetCurrentState
                                                    task.Type = TaskType.RestoreFileFromQtn;
                                                    task.Name = Resources.Resource.TaskNameRestoreFileFromQtn;
                                                    task = tskRestoreFileFromQtn.GetCurrentState();
                                                }

                                                else
                                                {
                                                    task = collection.Get(name);
                                                    //editing = "";
                                                }

        //
        string type = task.Type.ToString();
        Session["CurrentGroupUserTask"] = task;

        Response.Redirect("TaskCreate.aspx?Type=" + type + editing);

    }

    /// <summary>
    /// Delete task click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        TaskUserCollection collection =
            new TaskUserCollection(Profile.TasksList);

        collection.Delete(ddlTaskName.SelectedValue);
        collection = collection.Deserialize();

        Profile.TasksList = collection.Serialize();
        Session["TaskUser"] = collection;


        InitFields();

        lblMessage.Text = Resources.Resource.TaskDeleted;
        mpPicture.Attributes["class"] = "ModalPopupPictureSuccess";
        CorrectPositionModalPopup();
        ModalPopupExtender.Show();
    }

    #endregion

    protected void lbtnExcel_Click(object sender, EventArgs e)
    {
        GridView gvExcel = new GridView();

        InitializeSession();
        using (VlslVConnection conn = new VlslVConnection(
            ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            //заглушка

            GroupManager gmng = new GroupManager(conn);

            conn.OpenConnection();
            conn.CheckConnectionState(true);


            GroupFilterEntity filter;
            if (Session["CurrentGroupFilter"] == null)
            {
                filter = new GroupFilterEntity();
                filter.GroupName = "*";
                filter.GenerateSQLWhereStatement();
            }
            else
                filter = (GroupFilterEntity)Session["CurrentGroupFilter"];

            
            int count = gmng.Count(filter.GetSQLWhereStatement);


            gvExcel.DataSource = gmng.List(filter.GetSQLWhereStatement,
                Convert.ToString(Session["GroupSorting"]), 1, count);
            gvExcel.DataBind();

            conn.CloseConnection();
        }

        DataGridToExcel.Export("Groups.xls", gvExcel);

    }
}
