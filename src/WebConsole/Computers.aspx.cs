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


//!-OPTM ������� ������ ����� � ��������� partial-����

/// <summary>
/// Computers page
/// </summary>
public partial class Computers : PageBase
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
        Page.Title = Resources.Resource.PageComputersTitle;
        RegisterScript(@"js/jQuery/jquery-1.3.2.js");
        if (!IsPostBack)
        {
            InitFields();
            Session["CompSelectAll"] = true;
            Page.Form.Attributes.Add("onkeydown", "OnKeyDown()");            
        }
        InitDefaultPolicy();
    }

    #region Initialization...

    /// <summary>
    /// �������������� ���������� ���������� �� ������ �� ������
    /// </summary>
    private void InitializeSession()
    {
        if (Session["CompPageSize"] == null)
            Session["CompPageSize"] = 10;						//page size	

        if (Session["CompSelectAll"] == null)
            Session["CompSelectAll"] = true;					//select mode

        if (Session["CompSorting"] == null)
            Session["CompSorting"] = "ComputerName ASC";		//sort expression

        if (Session["CompFilters"] == null)
            Session["CompFilters"] = new CompFilterCollection(Profile.CompFilters);

        if (Session["TaskUser"] == null)
            Session["TaskUser"] = new TaskUserCollection(Profile.TasksList);

        if (Session["CompTopControlVisible"] == null)
            Session["CompTopControlVisible"] = false;

        if (Session["CompBottomControlVisible"] == null)
            Session["CompBottomControlVisible"] = false;

        if (Session["CompPolicyPanelVisible"] == null)
            Session["CompPolicyPanelVisible"] = false;

        //��������� �������� Settings
        SettingsEntity settings;
        if (Session["Settings"] == null)
        {
            settings = new SettingsEntity();
            try
            {

                settings = settings.Deserialize(Profile.Settings);
            }
            catch
            {
                settings = new SettingsEntity();
            }
            finally
            {
                Session["Settings"] = settings;
            }
        }
        else
        {
            settings = (SettingsEntity)Session["Settings"];
        }
  

        if (Session["TaskUser"] == null)
            Session["TaskUser"] = new TaskUserCollection(Profile.TasksList);
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


    private void InitRights()
    {
        if (Roles.IsUserInRole("Administrator"))
        {
            lbtnDeleteComputers.Text = Resources.Resource.DeleteComputers;
            lbtnDeleteComputers.Attributes.Add("onclick",
                    "return confirm('" + Resources.Resource.AreYouSureComp + "');");

            lbtnDeletePolicy.Attributes.Add("onclick",
                    "return confirm('" + Resources.Resource.AreYouSurePolicy + "');");
        }
        else
        {
            lbtnDeleteComputers.Visible = false;
            divMainPolicy.Visible = false;
            if(!Roles.IsUserInRole("Operator")) divMainTask.Visible = false;
        }
    }

    /// <summary>
    /// ������������� �����: ������, ����� � ���.
    /// </summary>
    protected override void InitFields()
    {

        InitializeSession();

        btnClose.Text = Resources.Resource.Close;

        btnDelete.Text = Resources.Resource.Delete;
        btnDelete.Attributes.Add("onclick",
            "return confirm('" + Resources.Resource.AreYouSureFilter + "');");
        btnEditFilter.Text = Resources.Resource.Edit;


        //���� ������� ��������� ���������� ��������� ������ ����������� Web Parts 
        foreach (WebPartDisplayMode displayMode in WebPartManager1.SupportedDisplayModes)
            ddlWebPartState.Items.Add(displayMode.Name);

        lblDefaults.Text = Resources.Resource.Clear;
        //lbtnGiveTask.Text = Resources.Resource.GiveTask;
        lbtnExcel.Text = Resources.Resource.ExportToExcel;

        //Init Color Options
        InitColorOptions();

        InitRights();

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
        WebPartManager1.WebParts[1].Title = Resources.Resource.Extra;
        WebPartManager1.WebParts[2].Title = Resources.Resource.Date;
        WebPartManager1.WebParts[3].Title = Resources.Resource.Bool;

        for (int i = 0; i < 4; i++)
        {
            WebPartManager1.Zones[i].MinimizeVerb.Text = Resources.Resource.Minimize;
            WebPartManager1.Zones[i].RestoreVerb.Text = Resources.Resource.Restore;
            WebPartManager1.Zones[i].CloseVerb.Text = Resources.Resource.Close;
        }

        InitPoliciesList();
        UpdateData();

        ddlPageSize.SelectedValue = Convert.ToString(Session["CompPageSize"]);



        //Init start state of panel
        InitShowPanelImageButton("CompTopControlVisible", divTop, imbtnTopControl);
        InitShowPanelImageButton("CompBottomControlVisible", divBottom, imbtnBottomControl);
        InitShowPanelImageButton("CompPolicyPanelVisible", divPolicyPanel, lbtnPolicyPanel);
        

        List<string> filtersName = new List<string>();
        filtersName.Add(Resources.Resource.TemporaryFilter);
        CompFilterCollection collection = (CompFilterCollection)Session["CompFilters"];
        foreach (CompFilterEntity filter in collection)
            filtersName.Add(filter.FilterName);

        ddlFilter.DataSource = filtersName;
        ddlFilter.DataBind();


        if (Session["CurrentCompFilter"] != null)
        {
            CompFilterEntity filter = (CompFilterEntity)Session["CurrentCompFilter"];
            if (filter.FilterName != "")
                ddlFilter.SelectedValue = filter.FilterName;
            else
                ddlFilter.SelectedIndex = 0;

            cmpfltMain.LoadFilter(filter);
            cmpfltExtra.LoadFilter(filter);
            cmpfltDate.LoadFilter(filter);
            cmpfltBool.LoadFilter(filter);
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

        lbtnCreatePolicy.Text = Resources.Resource.Create;
        lbtnDeletePolicy.Text = Resources.Resource.Delete;
        lbtnEditPolicy.Text = Resources.Resource.Edit;

        lbtnApplyPolicyToComps.Text = Resources.Resource.ApplyPolicy;
        lbtnRemoveCompsFromPolicy.Text = Resources.Resource.DeleteComputers;
        lbtnShowCompsByPolicy.Text = Resources.Resource.ShowComputers;

        
    }

    private void InitColorOptions()
    {
        ComputerColorOptionsEntity options;
        if (Session["ColorOptions"] == null)
        {
            options = new ComputerColorOptionsEntity();
            try
            {

                options = options.Deserialize(Profile.ComputerColorOptions);
            }
            catch
            {
                ltrlxmlShowValue.Text = "";
                return;
            }
        }
        else
        {
            options = (ComputerColorOptionsEntity)Session["ColorOptions"];
        }

        StringBuilder builder = new StringBuilder();

        builder.Append("<table>");
        if (options.GoodStateColor.Color != "Transparent")
            builder.AppendFormat("<tr><td><div style='background-color: {0}; Width:20px; Height:20px;'></div></td><td><b>{1}</b></td></tr>", options.GoodStateColor.Color, Resources.Resource.GoodState);

        foreach (EPriorityComputerColor prior in options.Priority)
        {
            switch (prior)
            {
                case EPriorityComputerColor.Integrity:
                    if (options.IntegrityColor.Color != "Transparent")
                        builder.AppendFormat("<tr><td><div style='background-color: {0}; Width:20px; Height:20px;'></div></td><td><b>{1}</b></td></tr>", options.IntegrityColor.Color, Resources.Resource.NoIntegrity);

                    break;
                case EPriorityComputerColor.Key:
                    if (options.KeyColor.Color != "Transparent")
                        builder.AppendFormat("<tr><td><div style='background-color: {0}; Width:20px; Height:20px;'></div></td><td><b>{1}</b></td></tr>", options.KeyColor.Color, Resources.Resource.KeyNoValid);

                    break;
                case EPriorityComputerColor.LastActive:
                    builder.AppendFormat("<tr><td colspan='2'><b>{0}:</b></td></tr>", Resources.Resource.LastActive);
                    if (options.LastActiveOption1.Color != "Transparent")
                        builder.AppendFormat("<tr><td><div style='background-color: {0}; Width:20px; Height:20px;'></div></td><td>{1} {2} {3}</td></tr>", options.LastActiveOption1.Color, Resources.Resource.MoreThan, options.LastActiveOption1.Time, options.LastActiveOption1.isHour ? Resources.Resource.Hours : Resources.Resource.Days2);
                    if (options.LastActiveOption2.Color != "Transparent")
                        builder.AppendFormat("<tr><td><div style='background-color: {0}; Width:20px; Height:20px;'></div></td><td>{1} {2} {3}</td></tr>", options.LastActiveOption2.Color, Resources.Resource.MoreThan, options.LastActiveOption2.Time, options.LastActiveOption2.isHour ? Resources.Resource.Hours : Resources.Resource.Days2);
                    if (options.LastActiveOption3.Color != "Transparent")
                        builder.AppendFormat("<tr><td><div style='background-color: {0}; Width:20px; Height:20px;'></div></td><td>{1} {2} {3}</td></tr>", options.LastActiveOption3.Color, Resources.Resource.MoreThan, options.LastActiveOption3.Time, options.LastActiveOption3.isHour ? Resources.Resource.Hours : Resources.Resource.Days2);

                    break;
                case EPriorityComputerColor.LastInfected:
                    builder.AppendFormat("<tr><td colspan='2'><b>{0}:</b></td></tr>", Resources.Resource.LastInfected);
                    if (options.LastInfectionOption1.Color != "Transparent")
                        builder.AppendFormat("<tr><td><div style='background-color: {0}; Width:20px; Height:20px;'></div></td><td>{1} {2} {3}</td></tr>", options.LastInfectionOption1.Color, Resources.Resource.LessThan, options.LastInfectionOption1.Time, options.LastInfectionOption1.isHour ? Resources.Resource.Hours : Resources.Resource.Days2);
                    if (options.LastInfectionOption2.Color != "Transparent")
                        builder.AppendFormat("<tr><td><div style='background-color: {0}; Width:20px; Height:20px;'></div></td><td>{1} {2} {3}</td></tr>", options.LastInfectionOption2.Color, Resources.Resource.LessThan, options.LastInfectionOption2.Time, options.LastInfectionOption2.isHour ? Resources.Resource.Hours : Resources.Resource.Days2);
                    if (options.LastInfectionOption3.Color != "Transparent")
                        builder.AppendFormat("<tr><td><div style='background-color: {0}; Width:20px; Height:20px;'></div></td><td>{1} {2} {3}</td></tr>", options.LastInfectionOption3.Color, Resources.Resource.LessThan, options.LastInfectionOption3.Time, options.LastInfectionOption3.isHour ? Resources.Resource.Hours : Resources.Resource.Days2);

                    break;
                case EPriorityComputerColor.LastUpdate:
                    builder.AppendFormat("<tr><td colspan='2'><b>{0}:</b></td></tr>", Resources.Resource.LastUpdate);
                    if (options.LastUpdateOption1.Color != "Transparent")
                        builder.AppendFormat("<tr><td><div style='background-color: {0}; Width:20px; Height:20px;'></div></td><td>{1} {2} {3}</td></tr>", options.LastUpdateOption1.Color, Resources.Resource.MoreThan, options.LastUpdateOption1.Time, options.LastUpdateOption1.isHour ? Resources.Resource.Hours : Resources.Resource.Days2);
                    if (options.LastUpdateOption2.Color != "Transparent")
                        builder.AppendFormat("<tr><td><div style='background-color: {0}; Width:20px; Height:20px;'></div></td><td>{1} {2} {3}</td></tr>", options.LastUpdateOption2.Color, Resources.Resource.MoreThan, options.LastUpdateOption2.Time, options.LastUpdateOption2.isHour ? Resources.Resource.Hours : Resources.Resource.Days2);
                    if (options.LastUpdateOption3.Color != "Transparent")
                        builder.AppendFormat("<tr><td><div style='background-color: {0}; Width:20px; Height:20px;'></div></td><td>{1} {2} {3}</td></tr>", options.LastUpdateOption3.Color, Resources.Resource.MoreThan, options.LastUpdateOption3.Time, options.LastUpdateOption3.isHour ? Resources.Resource.Hours : Resources.Resource.Days2);

                    break;
            }
        }
        builder.Append("</table>");
        
        ltrlxmlShowValue.Text = builder.ToString();
    }
    #endregion

    protected void ddlWebPartState_SelectedIndexChanged(object sender, EventArgs e)
    {
        WebPartDisplayMode displayMode;
        //���������� ��������� ����� ����������� 
        displayMode = WebPartManager1.SupportedDisplayModes[ddlWebPartState.SelectedValue];
        //������������� ����� ����������� 
        WebPartManager1.DisplayMode = displayMode;
    }


    private List<ComputersEntity> computers;
    /// <summary>
    /// ���������� ������� Computers �� ��������� ��������� ����������
    /// </summary>
    void UpdateData()
    {
        InitializeSession();
        using (VlslVConnection conn = new VlslVConnection(
            ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            ComputersManager cmng = new ComputersManager(conn);

            conn.OpenConnection();
            conn.CheckConnectionState(true);
            ///!-OPTM �������:)
            ///������� �����������, ��� � ��� ����� ������� � ������ ��� ���
            if (Session["CheckBoxs"] != null)
            {
                Hashtable htbl = (Hashtable)Session["CheckBoxs"];
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
                Session["CompsCheckedValues"] = list;
            }

            Session["CheckBoxs"] = new Hashtable();				//list of id checkbox/computersid


            CompFilterEntity filter = new CompFilterEntity(); ;
            
            int? showMode = (int?) Session["CompShowMode"];
            if(!showMode.HasValue)
                showMode = 0;
            
            int count = 0;
            computers = new List<ComputersEntity>();

            //switch mode to data
            switch(showMode)
            {
                    //use temporary group
                case 1:
                    if (Session["TempGroupComputers"] != null)
                    {
                        filter.GetSQLWhereStatement =
                            ((FilterEntity)Session["TempGroup"]).GetSQLWhereStatement;

                        //(tmpGroup.FindControl("mainDiv") as HtmlContainerControl).Attributes.Add("class", "menuSelected");
                        divPolicyMenu.Attributes.Add("class", "menu");
                    }
                    count = cmng.Count(filter.GetSQLWhereStatement);
                    computers = cmng.List(filter.GetSQLWhereStatement,
                            Convert.ToString(Session["CompSorting"]),
                            pcPaging.CurrentPageIndex, Convert.ToInt32(Session["CompPageSize"]));
                    break;
                    //use policy
                case 2:

                    string selectedPolicyName = "";
                    if (ddlPolicyName.SelectedItem != null)
                        selectedPolicyName = ddlPolicyName.SelectedItem.Text;
                    if ((selectedPolicyName == "(undefined)") || (String.IsNullOrEmpty(selectedPolicyName)))
                        break;

                    PolicyProvider provider = PoliciesState;
                    Policy policy = provider.GetPolicyByName(selectedPolicyName);

                    count = provider.GetComputerByPolicyCount(policy);
                    computers = provider.GetComputersByPolicyPage(policy, pcPaging.CurrentPageIndex, Convert.ToInt32(Session["CompPageSize"]),
                        Convert.ToString(Session["CompSorting"]));

                    divPolicyHeader.Attributes["class"] = "GiveButton1";
                    divPolicyMenu.Attributes.Add("class", "menuSelected");
                    //(tmpGroup.FindControl("mainDiv") as HtmlContainerControl).Attributes.Add("class", "menu");
                    break;

                    //use filter
                default:
                    if (Session["CurrentCompFilter"] == null)
                    {
                        filter = new CompFilterEntity();
                        filter.ComputerName = "*";
                        filter.GenerateSQLWhereStatement();
                    }
                    else
                    {
                        filter = (CompFilterEntity)Session["CurrentCompFilter"];
                        //�������...
                        if ((filter.LatestInfectedIntervalIndex != Int32.MinValue) ||
                            (filter.LatestUpdateIntervalIndex != Int32.MinValue) ||
                            (filter.RecentActiveIntervalIndex != Int32.MinValue))
                        {
                            filter.GenerateSQLWhereStatement();
                        }
                    }
                    count = cmng.Count(filter.GetSQLWhereStatement);
                    computers = cmng.List(filter.GetSQLWhereStatement,
                            Convert.ToString(Session["CompSorting"]),
                            pcPaging.CurrentPageIndex, Convert.ToInt32(Session["CompPageSize"]));
                    //(tmpGroup.FindControl("mainDiv") as HtmlContainerControl).Attributes.Add("class", "menu");
                    divPolicyMenu.Attributes.Add("class", "menu");
                    break;
            }

            
            int pageSize = Convert.ToInt32(Session["CompPageSize"]);
            int pageCount = (int)Math.Ceiling((double)count / pageSize);

            lblCount.Text = Resources.Resource.Found + ": " + count.ToString();

            pcPaging.PageCount = pageCount;
            pcPagingTop.PageCount = pageCount;

            Session["ComputersCurrentPageIndex"] = pcPaging.CurrentPageIndex;

            DataList1.DataSource = computers;
            DataList1.DataBind();


            conn.CloseConnection();
        }
    }

    /// <summary>
    /// ��������� ������� ��������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        pcPaging.CurrentPageIndex = 1;
        pcPagingTop.CurrentPageIndex = 1;
        Session["CompPageSize"] = ddlPageSize.SelectedValue;
        Session["CompSelectAll"] = true;
        UpdateData();

    }

    /// <summary>
    /// ��������� ���������� �������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFilter.SelectedValue == Resources.Resource.TemporaryFilter)
        {
            cmpfltMain.Clear();
            cmpfltExtra.Clear();
            cmpfltDate.Clear();
            cmpfltBool.Clear();
            //btnApplyFilter_Click(this, null);
            Session["CurrentCompFilter"] = null;
            lblDefaults_Click(this, null);
            divFilterHeader.Attributes["class"] = "GiveButton";
            divEditFilter.Visible = false;
            divDelete.Visible = false;            
            return;
        }
        else
        {
            if (divPolicyHeader.Attributes["class"] != "GiveButton1")
                divFilterHeader.Attributes["class"] = "GiveButton1";
            divEditFilter.Visible = true;
            divDelete.Visible = true;
        }

        CompFilterCollection collection = (CompFilterCollection)Session["CompFilters"];

        CompFilterEntity fltr = new CompFilterEntity();

        foreach (CompFilterEntity filter in collection)
        {
            if (filter.FilterName == ddlFilter.SelectedValue)
                fltr = filter;
        }

        fltr.CheckFilters();
        fltr.GenerateSQLWhereStatement();


        if (fltr.GetSQLWhereStatement != String.Empty)
        {
            Session["CurrentCompFilter"] = fltr;
        }
        else Session["CurrentCompFilter"] = null;//default*/
       
        pcPaging.CurrentPageIndex = 1;
        pcPagingTop.CurrentPageIndex = 1;

        cmpfltBool.Clear();
        cmpfltDate.Clear();
        cmpfltExtra.Clear();
        cmpfltMain.Clear();

        cmpfltBool.LoadFilter(fltr);
        cmpfltDate.LoadFilter(fltr);
        cmpfltExtra.LoadFilter(fltr);
        cmpfltMain.LoadFilter(fltr);
        if (divPolicyHeader.Attributes["class"] == "GiveButton1") Session["CompShowMode"] = 2;
        else Session["CompShowMode"] = 0;
        Session["CompSelectAll"] = true;
        Session["TempGroupComputers"] = null;
        UpdateData();

    }

    #region DataList ItemDataBound

    private string GetDateString(DateTime dt,string argbColor)
    {
        if (dt != DateTime.MinValue)
            return "<td width=100 bgcolor='" + argbColor + "'>" + dt + "</td>";

        return "<td style=\"text-align:center\" width=100 bgcolor='" + argbColor + "'>-</td>";

    }


    /// <summary>
    /// �������� ������ � ������� Computers
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //Header
        if (e.Item.ItemType == ListItemType.Header)
        {
            if (Session["Settings"] != String.Empty)
            {
                SettingsEntity settings = (SettingsEntity)Session["Settings"];
                string argbColor = DataList1.HeaderStyle.BackColor.Name;

                if ((!Roles.IsUserInRole("Administrator")) && (!Roles.IsUserInRole("Operator")))
                {
                    (e.Item.FindControl("lbtnSel") as LinkButton).Visible = false;
                }

                if (!settings.ShowComputerID)
                {
                    (e.Item.FindControl("lbtnID") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdID") as HtmlTableCell).Visible = false;
                }
                if (!settings.ShowComputerName)
                {
                    (e.Item.FindControl("lbtnComputerName") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdComputerName") as HtmlTableCell).Visible = false;
                }
                (e.Item.FindControl("tdComputerName") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowIPAdress)
                {
                    (e.Item.FindControl("lbtnIPAddress") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdIPAdress") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdIPAdress") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowControlCenter)
                {
                    (e.Item.FindControl("lbtnControlCenter") as LinkButton).Visible = false;         
                    (e.Item.FindControl("tdControlCenter") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdControlCenter") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowCPUClock)
                {
                    (e.Item.FindControl("lbtnCPUClock") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdCPUClock") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdCPUClock") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowDomainName)
                {
                    (e.Item.FindControl("lbtnDomainName") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdDomainName") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdDomainName") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowVba32Integrity)
                {
                    (e.Item.FindControl("lbtnVba32Integrity") as LinkButton).Visible = false;        
                    (e.Item.FindControl("tdIntegrity") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdIntegrity") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowLatestInfected)
                {
                    (e.Item.FindControl("lbtnLatestInfected") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdLatestInfected") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdLatestInfected") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowLatestMalware)
                {
                    (e.Item.FindControl("lbtnLatestMalware") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdLatestMalware") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdLatestMalware") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowLatestUpdate)
                {
                    (e.Item.FindControl("lbtnLatestUpdate") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdLatestUpdate") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdLatestUpdate") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowOSType)
                {
                    (e.Item.FindControl("lbtnOSName") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdOSType") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdOSType") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowRAM)
                {
                    (e.Item.FindControl("lbtnRAM") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdRAM") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdRAM") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowRecentActive)
                {
                    (e.Item.FindControl("lbtnRecentActive") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdRecentActive") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdRecentActive") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowUserLogin)
                {
                    (e.Item.FindControl("lbtnUserLogin") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdUserLogin") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdUserLogin") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowVba32KeyValid)
                {
                    (e.Item.FindControl("lbtnVba32KeyValid") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdKeyValid") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdKeyValid") as HtmlTableCell).BgColor = argbColor;
                if (!settings.ShowVba32Version)
                {
                    (e.Item.FindControl("lbtnVba32Version") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdVersion") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdVersion") as HtmlTableCell).BgColor = argbColor;

                if (!settings.ShowPolicyName)
                {
                    (e.Item.FindControl("lbtnPolicyName") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdPolicyName") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdPolicyName") as HtmlTableCell).BgColor = argbColor;

                if (!settings.ShowDescription)
                {
                    (e.Item.FindControl("lbtnDescription") as LinkButton).Visible = false;
                    (e.Item.FindControl("tdDescription") as HtmlTableCell).Visible = false;

                }
                (e.Item.FindControl("tdDescription") as HtmlTableCell).BgColor = argbColor;

                (e.Item.FindControl("lbtnComputerName") as LinkButton).Text = Resources.Resource.ComputerName;
                (e.Item.FindControl("lbtnIPAddress") as LinkButton).Text = Resources.Resource.IPAddress;
                (e.Item.FindControl("lbtnControlCenter") as LinkButton).Text = Resources.Resource.ControlCenter;
                (e.Item.FindControl("lbtnCPUClock") as LinkButton).Text = Resources.Resource.CPU + '(' + Resources.Resource.Herz + ')'; ;
                (e.Item.FindControl("lbtnDomainName") as LinkButton).Text = Resources.Resource.DomainName;
                (e.Item.FindControl("lbtnVba32Integrity") as LinkButton).Text = Resources.Resource.VBA32Integrity;
                (e.Item.FindControl("lbtnLatestInfected") as LinkButton).Text = Resources.Resource.LatestInfected;
                (e.Item.FindControl("lbtnLatestMalware") as LinkButton).Text = Resources.Resource.LatestMalware;
                (e.Item.FindControl("lbtnLatestUpdate") as LinkButton).Text = Resources.Resource.LatestUpdate;
                (e.Item.FindControl("lbtnOSName") as LinkButton).Text = Resources.Resource.OSType;
                (e.Item.FindControl("lbtnRAM") as LinkButton).Text = Resources.Resource.RAM + '(' + Resources.Resource.Megabyte + ')'; ;
                (e.Item.FindControl("lbtnRecentActive") as LinkButton).Text = Resources.Resource.RecentActive;
                (e.Item.FindControl("lbtnUserLogin") as LinkButton).Text = Resources.Resource.UserLogin;
                (e.Item.FindControl("lbtnVba32KeyValid") as LinkButton).Text = Resources.Resource.VBA32KeyValid;
                (e.Item.FindControl("lbtnVba32Version") as LinkButton).Text = Resources.Resource.VBA32Version;
                (e.Item.FindControl("lbtnPolicyName") as LinkButton).Text = Resources.Resource.Policy;
                (e.Item.FindControl("lbtnDescription") as LinkButton).Text = Resources.Resource.Description;

                string currentSorting = (string)Session["CompSorting"];
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
        }
        //Item
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {

            Hashtable htbl = (Hashtable)Session["CheckBoxs"];
           
            string s;
            string argbColor = GetComputerColor((ComputersEntity)e.Item.DataItem);

            if(argbColor == "Transparent")
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

                htbl.Add(s, ((ComputersEntity)e.Item.DataItem).ID);

                //!-OPTM ����� StringBuilder ������������� ����� ���� �� ����� � �����
                //������ ��������������
                s = "";
                if (Session["Settings"] != String.Empty)
                {
                    string strBool;
                    SettingsEntity settings = (SettingsEntity)Session["Settings"];

                    if ((!Roles.IsUserInRole("Administrator")) && (!Roles.IsUserInRole("Operator")))
                    {
                        (e.Item.FindControl("check") as CheckBox).Visible = false;
                    }
                    
                    if (settings.ShowComputerID) s += "<td width=30>" + ((ComputersEntity)e.Item.DataItem).ID + "</td>";
                    
                    string compName = ((ComputersEntity)e.Item.DataItem).ComputerName;
                    if (settings.ShowComputerName)
                    {
                        (e.Item.FindControl("tdCompName") as HtmlTableCell).BgColor = argbColor;
                        (e.Item.FindControl("aCompName") as HtmlAnchor).Visible = true;
                        (e.Item.FindControl("aCompName") as HtmlAnchor).InnerText = compName;  
                        (e.Item.FindControl("aCompName") as HtmlAnchor).HRef =
                             "CompInfo.aspx?CompName=" + compName;
                    }
                    else
                    {
                        (e.Item.FindControl("tdCompName") as HtmlTableCell).Visible = false;
                    }
                    if (settings.ShowIPAdress) s += "<td width=100 bgcolor='"+argbColor+"'>" + ((ComputersEntity)e.Item.DataItem).IPAddress.TrimEnd() + "</td>";
                    if (settings.ShowControlCenter)
                    {   
                        if ((((ComputersEntity)e.Item.DataItem).ControlCenter)||(compName == Environment.MachineName))
                            strBool = "enabled.gif";
                        else
                            strBool = "disabled.gif";
                        s += "<td width=100 align=center bgcolor='" + argbColor + "'><img src='" +
                            Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/" + strBool + "'></img></td>";
                    }

                    bool isControlCenter = ((ComputersEntity)e.Item.DataItem).ControlCenter;

                    if (settings.ShowCPUClock)
                    {
                        short cpu = ((ComputersEntity)e.Item.DataItem).CPUClock;
                        s += "<td style=\"text-align:center\" width=100 bgcolor='" + argbColor + "'>" + (cpu==short.MinValue ?"-": cpu.ToString()) + "</td>";
                    }
                    
                    if (settings.ShowDomainName)
                        s += "<td width=100 align=center bgcolor='" + argbColor + "'>" + (((ComputersEntity)e.Item.DataItem).DomainName ==String.Empty ? "-" : ((ComputersEntity)e.Item.DataItem).DomainName.TrimEnd()) + "</td>";

                    if (settings.ShowVba32Integrity)
                    {
                        if ((!isControlCenter)||(compName!= Environment.MachineName))
                        {
                            if (((ComputersEntity)e.Item.DataItem).Vba32Integrity)
                                strBool = "enabled.gif";
                            else
                                strBool = "disabled.gif";
                            s += "<td width=100 align=center bgcolor='" + argbColor + "'><img src='"
                                + Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/" + strBool + "'></img></td>";
                        }
                        else
                            s += "<td width=100 align=center bgcolor='" + argbColor + "'>-</td>";

                    }
                    if (settings.ShowLatestInfected)
                    {
                        s += GetDateString(((ComputersEntity)e.Item.DataItem).LatestInfected,argbColor);
                     //   s += "<td width=100 bgcolor='" + argbColor + "'>" + ((ComputersEntity)e.Item.DataItem).LatestInfected + "</td>";
                    }
                    
                    if (settings.ShowLatestMalware)
                        s += "<td width=100 align=center bgcolor='" + argbColor + "'>" + Anchor.FixString(((ComputersEntity)e.Item.DataItem).LatestMalware != String.Empty?
                            ((ComputersEntity)e.Item.DataItem).LatestMalware.TrimEnd() : "-", 12, '.') + "</td>";

                    if (settings.ShowLatestUpdate)
                    {
                        s += GetDateString(((ComputersEntity)e.Item.DataItem).LatestUpdate,argbColor);
                    }
                    
                    if (settings.ShowOSType)
                        s += "<td width=100 align=center bgcolor='" + argbColor + "'>" + ((ComputersEntity)e.Item.DataItem).OSName + "</td>";

                    if (settings.ShowRAM)
                    {
                        short ram = ((ComputersEntity)e.Item.DataItem).RAM;
                        s += "<td style=\"text-align:center\" width=100 bgcolor='" + argbColor + "'>" + (ram!=short.MinValue? ram.ToString() : "-") + "</td>";
                    }

                    if (settings.ShowRecentActive)
                    {
                      
                        s += GetDateString(((ComputersEntity)e.Item.DataItem).RecentActive, argbColor);

                    }
                    if (settings.ShowUserLogin)
                        s += "<td width=100 align=center bgcolor='" + argbColor + "'>" + (((ComputersEntity)e.Item.DataItem).UserLogin ==String.Empty? "-" : ((ComputersEntity)e.Item.DataItem).UserLogin.TrimEnd()) + "</td>";


                    if (settings.ShowVba32KeyValid)
                    {
                        if ((!isControlCenter)||(compName!= Environment.MachineName))
                        {
                            if (((ComputersEntity)e.Item.DataItem).Vba32KeyValid)
                                strBool = "enabled.gif";
                            else
                                strBool = "disabled.gif";
                            s += "<td width=100 align=center bgcolor='" + argbColor + "'><img src='"
                                + Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/" + strBool + "'></img></td>";
                        }
                        else
                            s += "<td width=100 align=center bgcolor='" + argbColor + "'>-</td>";
                    }
                    if (settings.ShowVba32Version)
                        s += "<td width=100 align=center bgcolor='" + argbColor + "'>" + (((ComputersEntity)e.Item.DataItem).Vba32Version==String.Empty ? "-" : ((ComputersEntity)e.Item.DataItem).Vba32Version.TrimEnd()) + "</td>";

                    //PolicyName
                    if (settings.ShowPolicyName)
                    {
                        PolicyProvider provider = PoliciesState;
                        Policy policy;
                        try
                        {
                            policy = provider.GetPolicyToComputer(((ComputersEntity)e.Item.DataItem).ComputerName);
                        }
                        catch
                        {
                            policy = new Policy();
                        }

                        (e.Item.FindControl("lbtnPolicyName") as LinkButton).Visible = true;
                        (e.Item.FindControl("lbtnPolicyName") as LinkButton).BackColor = System.Drawing.Color.FromName(argbColor);
                        (e.Item.FindControl("tdPolicyName") as HtmlTableCell).BgColor = argbColor;
                        (e.Item.FindControl("lbtnPolicyName") as LinkButton).Text = String.IsNullOrEmpty(policy.Name) ? "-" : policy.Name;
                    }
                    else
                    {
                        (e.Item.FindControl("tdPolicyName") as HtmlTableCell).Visible = false;
                    }

                    if (settings.ShowDescription)
                    {
                        (e.Item.FindControl("lbtnDescription") as Label).Visible = true;
                        (e.Item.FindControl("lbtnDescription") as Label).BackColor = System.Drawing.Color.FromName(argbColor);
                        (e.Item.FindControl("tdDescription") as HtmlTableCell).BgColor = argbColor;
                        (e.Item.FindControl("lbtnDescription") as Label).Text = Anchor.FixString(((ComputersEntity)e.Item.DataItem).Description,16 );

                        if ((e.Item.FindControl("lbtnDescription") as Label).Text == String.Empty)
                            (e.Item.FindControl("lbtnDescription") as Label).Text = '[' + Resources.Resource.Empty + ']';

                    }
                    else
                    {
                        (e.Item.FindControl("tdDescription") as HtmlTableCell).Visible = false;
                    }
                }

                if (Session["CompsCheckedValues"] != null)
                {
                    ArrayList list = (ArrayList)Session["CompsCheckedValues"];
                    if(list.Contains( ((ComputersEntity)e.Item.DataItem).ID))
                        (e.Item.FindControl("check") as CheckBox).Checked = true;
                }

                e.Item.DataItem = s;
                e.Item.DataBind();
            }
//!-- ����� ��� ���������� - ���� ��� ���� ����� �����, ���� ���� ���������� ������
                //!-OPTM �������� ����� ��������� ��� ������ � ���
            catch
            {
            }
        }
    }

    private string GetComputerColor(ComputersEntity computersEntity)
    {
        string color = "Transparent";
        
        ComputerColorOptionsEntity options;
        if (Session["ColorOptions"] == null)
        {
            options = new ComputerColorOptionsEntity();
            try
            {

                options = options.Deserialize(Profile.ComputerColorOptions);
            }
            catch
            {
                return color;
            }
        }
        else
        {
            options = (ComputerColorOptionsEntity)Session["ColorOptions"];
        }
                
        string tmpColor = String.Empty;

        foreach (EPriorityComputerColor priority in options.Priority)
        {
            switch (priority)
            {
                case EPriorityComputerColor.LastUpdate:

                    tmpColor = GetColorByOptions(computersEntity.LatestUpdate, options.LastUpdateOption1, true);
                    if (tmpColor != String.Empty) return tmpColor;
                    tmpColor = GetColorByOptions(computersEntity.LatestUpdate, options.LastUpdateOption2, true);
                    if (tmpColor != String.Empty) return tmpColor;
                    tmpColor = GetColorByOptions(computersEntity.LatestUpdate, options.LastUpdateOption3, true);
                    if (tmpColor != String.Empty) return tmpColor;
                    break;
                case EPriorityComputerColor.LastInfected:

                    tmpColor = GetColorByOptions(computersEntity.LatestInfected, options.LastInfectionOption1, false);
                    if (tmpColor != String.Empty) return tmpColor;
                    tmpColor = GetColorByOptions(computersEntity.LatestInfected, options.LastInfectionOption2, false);
                    if (tmpColor != String.Empty) return tmpColor;
                    tmpColor = GetColorByOptions(computersEntity.LatestInfected, options.LastInfectionOption3, false);
                    if (tmpColor != String.Empty) return tmpColor;
                    break;
                case EPriorityComputerColor.LastActive:

                    tmpColor = GetColorByOptions(computersEntity.RecentActive, options.LastActiveOption1, true);
                    if (tmpColor != String.Empty) return tmpColor;
                    tmpColor = GetColorByOptions(computersEntity.RecentActive, options.LastActiveOption2, true);
                    if (tmpColor != String.Empty) return tmpColor;
                    tmpColor = GetColorByOptions(computersEntity.RecentActive, options.LastActiveOption3, true);
                    if (tmpColor != String.Empty) return tmpColor;
                    break;
                case EPriorityComputerColor.Key:
                    if (!computersEntity.Vba32KeyValid) return options.KeyColor.Color;
                    break;
                case EPriorityComputerColor.Integrity:
                    if (!computersEntity.Vba32Integrity) return options.IntegrityColor.Color;
                    break;
            }
        }

        return options.GoodStateColor.Color;
    }

    private string GetColorByOptions(DateTime dateTime, ColorOption option, bool isGreater)
    {
        if (isGreater)
        {
            if (option.isHour)
            {
                if (TimeSpan.Compare(DateTime.Now.Subtract(dateTime), new TimeSpan(option.Time, 0, 0)) > 0)
                    return option.Color;
            }
            else
            {
                if (TimeSpan.Compare(DateTime.Now.Subtract(dateTime), new TimeSpan(option.Time, 0, 0, 0)) > 0)
                    return option.Color;
            }
        }
        else
        {
            if (option.isHour)
            {
                if (TimeSpan.Compare(DateTime.Now.Subtract(dateTime), new TimeSpan(option.Time, 0, 0)) <= 0)
                    return option.Color;
            }
            else
            {
                if (TimeSpan.Compare(DateTime.Now.Subtract(dateTime), new TimeSpan(option.Time, 0, 0, 0)) <= 0)
                    return option.Color;
            }
        }

        return String.Empty;
    }

    #endregion

    #region DataList ItemCommand

    /// <summary>
    /// Item Commands � Data list
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
                if (Session["CompSorting"].ToString() == e.CommandArgument + " ASC")
                    Session["CompSorting"] = e.CommandArgument + " DESC";
                else
                    Session["CompSorting"] = e.CommandArgument + " ASC";
                DataList1.EditItemIndex = -1;
                UpdateData();
                break;

            case "SelectCommand":
                //Select or deselect all checkboxes in DataList
                bool checkedbox = true;
                if (Session["CompSelectAll"] != String.Empty)
                {
                    if (((bool)Session["CompSelectAll"]) == true)
                    {
                        checkedbox = true;
                        (e.CommandSource as LinkButton).Text = "-";
                        Session["CompSelectAll"] = false;
                    }
                    else
                    {
                        checkedbox = false;
                        (e.CommandSource as LinkButton).Text = "+";
                        Session["CompSelectAll"] = true;
                    }
                }
                for (int i = 0; i < DataList1.Items.Count; i++)
                {
                    CheckBox cb = (CheckBox)DataList1.Items[i].FindControl("check");
                    if (cb != null) { cb.Checked = checkedbox; }
                }
                break;
        }
    }

    protected void lbtnPolicyName_Click(object sender, EventArgs e)
    {
        string PolicyName = (sender as LinkButton).Text;
        if (String.IsNullOrEmpty(PolicyName) || PolicyName == "-") return;
        Response.Redirect(String.Format("PoliciesPage.aspx?Name={0}", PolicyName)); 
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
        Session["CompSelectAll"] = true;
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
        Session["CompSelectAll"] = true;
        UpdateData();
    }

    protected void pcPaging_HomePage(object sender, EventArgs e)
    {
        pcPaging.CurrentPageIndex = 1;
        pcPagingTop.CurrentPageIndex = 1;

        DataList1.EditItemIndex = -1;
        Page.MaintainScrollPositionOnPostBack = false;
        Anchor.ScrollToTop(Page);
        Session["CompSelectAll"] = true;
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
        Session["CompSelectAll"] = true;
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
        try
        {
            if (divPolicyHeader.Attributes["class"] == "GiveButton1") return;
            divFilterHeader.Attributes["class"] = "GiveButton1";
            CompFilterEntity filter = new CompFilterEntity();

            cmpfltMain.GetCurrentStateFilter(ref filter);
            cmpfltDate.GetCurrentStateFilter(ref filter);
            cmpfltExtra.GetCurrentStateFilter(ref filter);
            cmpfltBool.GetCurrentStateFilter(ref filter);
            filter.CheckFilters();
            filter.GenerateSQLWhereStatement();


            pcPaging.CurrentPageIndex = 1;
            pcPagingTop.CurrentPageIndex = 1;


            if (filter.GetSQLWhereStatement != String.Empty)
                Session["CurrentCompFilter"] = filter;
            else
                Session["CurrentCompFilter"] = null;

            ddlFilter.SelectedValue = Resources.Resource.TemporaryFilter;

            Session["CompShowMode"] = 0;
            Session["TempGroupComputers"] = null;
            UpdateData();
        }
        catch (ArgumentException argEx)
        {
            if (argEx.Message.Contains("IPAddress"))
            {
                lblMessage.Text = Resources.Resource.InvalidValue + ": " + Resources.Resource.IPAddress;
            }
            else if (argEx.Message.Contains("RAM"))
            {
                lblMessage.Text = Resources.Resource.InvalidValue + ": " + Resources.Resource.RAM;
            }
            else if (argEx.Message.Contains("CPUClock"))
            {
                lblMessage.Text = Resources.Resource.InvalidValue + ": " + Resources.Resource.CPU;
            }
            else if (argEx.Message.Contains("DateTime"))
            {
                lblMessage.Text = Resources.Resource.InvalidValue + ": " + Resources.Resource.Date;
            }
            else if (argEx.Message.Contains("Description"))
            {
                lblMessage.Text = Resources.Resource.InvalidValue + ": " + Resources.Resource.Description;
            }
            else
            {
                lblMessage.Text = argEx.Message;
            }
            mpPicture.Attributes["class"] = "ModalPopupPictureError";
            ModalPopupExtender.Show();
            return;
        }
        catch
        {
            lblMessage.Text = Resources.Resource.Error + ". " + Resources.Resource.ErrorService;
            mpPicture.Attributes["class"] = "ModalPopupPictureError";
            ModalPopupExtender.Show();
            return;
        }
    }

    /// <summary>
    /// Defaults click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lblDefaults_Click(object sender, EventArgs e)
    {
        divFilterHeader.Attributes["class"] = "GiveButton";
        
        cmpfltBool.Clear();
        cmpfltDate.Clear();
        cmpfltExtra.Clear();
        cmpfltMain.Clear();

        ddlFilter.SelectedValue = Resources.Resource.TemporaryFilter;

        divEditFilter.Visible = false;
        divDelete.Visible = false;

        if (divPolicyHeader.Attributes["class"] == "GiveButton1") Session["CompShowMode"] = 2;
        else Session["CompShowMode"] = null;
        Session["CurrentCompFilter"] = null;
        Session["TempGroupComputers"] = null;        
        UpdateData();

    }

    /// <summary>
    /// Create filter click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnFilter_Click(object sender, EventArgs e)
    {

        try
        {
            CompFilterEntity filter = new CompFilterEntity();

            cmpfltMain.GetCurrentStateFilter(ref filter);
            cmpfltDate.GetCurrentStateFilter(ref filter);
            cmpfltExtra.GetCurrentStateFilter(ref filter);
            cmpfltBool.GetCurrentStateFilter(ref filter);
            filter.GenerateSQLWhereStatement();

            //if (ddlFilter.SelectedValue != Resources.Resource.TemporaryFilter)
            //    filter.FilterName = ddlFilter.SelectedValue;

            Session["CurrentCompFilter"] = filter;


            Response.Redirect("CompFilters.aspx");
        }
        catch (ArgumentException argEx)
        {
            if (argEx.Message.Contains("IPAddress"))
            {
                lblMessage.Text = Resources.Resource.InvalidValue + ": " + Resources.Resource.IPAddress;
            }
            else if (argEx.Message.Contains("RAM"))
            {
                lblMessage.Text = Resources.Resource.InvalidValue + ": " + Resources.Resource.RAM;
            }
            else if (argEx.Message.Contains("CPUClock"))
            {
                lblMessage.Text = Resources.Resource.InvalidValue + ": " + Resources.Resource.CPU;
            }
            else if (argEx.Message.Contains("DateTime"))
            {
                lblMessage.Text = Resources.Resource.InvalidValue + ": " + Resources.Resource.Date;
            }
            else if (argEx.Message.Contains("Description"))
            {
                lblMessage.Text = Resources.Resource.InvalidValue + ": " + Resources.Resource.Description;
            }
            else
            {
                lblMessage.Text = argEx.Message;
            }
            mpPicture.Attributes["class"] = "ModalPopupPictureError";
            ModalPopupExtender.Show();
            return;
        }
        catch
        {
            lblMessage.Text = Resources.Resource.Error + ". " + Resources.Resource.ErrorService;
            mpPicture.Attributes["class"] = "ModalPopupPictureError";
            ModalPopupExtender.Show();
            return;
        }
    }


    /// <summary>
    ///  Top control show/hide
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnTopControl_Click(object sender, EventArgs e)
    {
        InitializeSession();

        bool tcVis = (bool)Session["CompTopControlVisible"];
        Session["CompTopControlVisible"] = !tcVis;

        InitShowPanelImageButton("CompTopControlVisible", divTop, imbtnTopControl);


        if (Session["CurrentCompFilter"] != null)
        {
            CompFilterEntity filter = (CompFilterEntity)Session["CurrentCompFilter"];
            cmpfltMain.LoadFilter(filter);
            cmpfltDate.LoadFilter(filter);
            cmpfltExtra.LoadFilter(filter);
            cmpfltBool.LoadFilter(filter);
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
        Response.Redirect("CompFilters.aspx?Filter=" + selectedFilter);
    }

    /// <summary>
    /// Delete filter button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {

        CompFilterCollection collection = new CompFilterCollection(Profile.CompFilters);

        collection.Delete(ddlFilter.SelectedValue);
        collection = collection.Deserialize();

        Profile.CompFilters = collection.Serialize();
        Session["CompFilters"] = collection;

        Session["CurrentCompFilter"] = null;
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
                
        cboxSelectAll.Text = Resources.Resource.TaskGiveAll;
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
        taskName.Add(Resources.Resource.CongLdrConfigureLoader);
        taskName.Add(Resources.Resource.CongLdrConfigureMonitor);
        taskName.Add(Resources.Resource.TaskNameConfigureQuarantine);
        taskName.Add(Resources.Resource.TaskNameConfigureProactiveProtection);
        taskName.Add(Resources.Resource.ConfigureScheduler);
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
            if (task.Type != TaskType.ProactiveProtection && task.Type != TaskType.Firewall && task.Type != TaskType.ConfigureSheduler)
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
        bool tcVis = (bool)Session["CompBottomControlVisible"];
        Session["CompBottomControlVisible"] = !tcVis;
        InitShowPanelImageButton("CompBottomControlVisible", divBottom, imbtnBottomControl);
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
                if (Session["CheckBoxs"] == null)
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
            //�������� ��������� ��������� ID ������
            Hashtable htbl = (Hashtable)Session["CheckBoxs"];
            Regex reg = new Regex(@"\$DataList1\$\w+");

            foreach (string str in Request.Form)
            {
                Match match = reg.Match(str);
                if ((match.Success) && (htbl.ContainsKey(match.Value)))
                {
                    list.Add(Convert.ToInt16(htbl[match.Value]));
                }
            }

            //�� ���� ������ ��������
            try
            {
                if (list.Count == 0)
                    throw new Exception(Resources.Resource.NoSelectedComputers);
            }
            catch (Exception e)
            {
                lblMessage.Text = e.Message;
                mpPicture.Attributes["class"] = "ModalPopupPictureError";
                CorrectPositionModalPopup();
                ModalPopupExtender.Show();
                return false;
            }

            Session["SelectedID"] = list;
        }

        InitializeSession();


        if ((Session["SelectedID"] == null) && (!isAllSelected))
            throw new Exception(Resources.Resource.ErrorCriticalError + ": Session['SelectedID'] == null ");

        //All computers who matches current filter
        if (isAllSelected)
        {
            //!-OPTM �������� ��� ����������� �� ���..
            CompFilterEntity filter;
            if (Session["CurrentCompFilter"] == null)
            {
                filter = new CompFilterEntity();
                filter.ComputerName = "*";
                filter.GenerateSQLWhereStatement();
            }
            else
                filter = (CompFilterEntity)Session["CurrentCompFilter"];

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
                ComputersManager cmng = new ComputersManager(conn);

                conn.OpenConnection();
                conn.CheckConnectionState(true);

                int count = cmng.Count(filter.GetSQLWhereStatement);

                List<ComputersEntity> compsList = cmng.List(filter.GetSQLWhereStatement,
                    Convert.ToString(Session["CompSorting"]), 1, count);


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
            ipAddr = PreServAction.GetIPArray((List<Int16>)Session["SelectedID"],
                ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);

            compName = PreServAction.GetComputerNameArray((List<Int16>)Session["SelectedID"],
                ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
            vbaVersion = PreServAction.GetVbaVersionArray((List<Int16>)Session["SelectedID"],
                ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
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
                string str = Server.HtmlEncode(xml.GetValue(tskCreateProcess.TagCommandLine)).Replace("&amp;", "&").Replace("&#160;", " ");
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
                if ((tskProactiveProtection.FindControl("ddlProfiles") as DropDownList).Items.Count == 0)
                    throw new ArgumentException(Resources.Resource.ErrorNotSelectProfile);
                TaskUserCollection collection = (TaskUserCollection)Session["TaskUser"];
                task = collection.Get(String.Format("Proactive Protection: {0}", (tskProactiveProtection.FindControl("ddlProfiles") as DropDownList).SelectedValue));

                for (int i = 0; i < compName.Length; i++)
                {
                    taskId[i] = PreServAction.CreateTask(compName[i], Resources.Resource.TaskNameConfigureProactiveProtection + ": " + task.Name.Substring(22), task.Param, userName, connStr);
                }

                control.PacketCustomAction(taskId, ipAddr, tskProactiveProtection.BuildTask(@"%VBA32%Vba32 ProActive\Vba32ProActive.conf", task.Param));
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

                //�������� ���������� �������� ������
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
                ri.MethodType = tskUninstall.Provider;
                ri.UninstallAll(list, tskUninstall.DoRestart);
                lbtnDeleteComputers_Click(null, null);
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
        string userName = Anchor.GetStringForTaskGivedUser();
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
        catch(Exception ex)
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


        //�� ���������
        ARM2_dbcontrol.Generation.XmlBuilder xmlBuil = new ARM2_dbcontrol.Generation.XmlBuilder("root");
        //��� ���, � ���� ����������� ������ �����-�� ���������(�������� ���������/��������� ���������)
        ARM2_dbcontrol.Generation.XmlBuilder xml = new ARM2_dbcontrol.Generation.XmlBuilder("root");
        
        xmlBuil.Generate();

        //switch ������ ����� ��-�� ����, ��� ������� �� ���������..
        //!-OPTM ��� ������������� ����  �������� ���������.
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

                                                string str =  Resources.Resource.TaskParamVba32LoaderEnable;
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
                                                                            lbtnSave.Visible = false;
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

        LoadStateTask(task);
    }

    private void LoadStateTask(TaskUserEntity task)
    {
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
        if (divPolicyHeader.Attributes["class"] == "GiveButton1") return;
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
    /// Policy Header Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnPolicyHeader_Click(object sender, EventArgs e)
    {
        if (ddlPolicyName.Items.Count == 0) return;
        if (divFilterHeader.Attributes["class"] == "GiveButton1") return;
        if (divPolicyHeader.Attributes["class"] == "GiveButton")
        {
            lbtnShowCompsByPolicy_Click(sender, e);
            divPolicyHeader.Attributes["class"] = "GiveButton1";
        }
        else
        {
            lblDefaults_Click(sender, e);
            //tmpGroup.lbtnCancel_Click(sender, e);
            divPolicyHeader.Attributes["class"] = "GiveButton";
        }
 
        pcPaging_HomePage(sender, e);
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

        //������ ������� ������..
        TaskUserEntity task = new TaskUserEntity();
        ARM2_dbcontrol.Generation.XmlBuilder xmlBuil = new ARM2_dbcontrol.Generation.XmlBuilder("root");
        xmlBuil.Generate();

        string editing = "&Mode=Edit";
        //string editing = "";
        try
        {
            if (name == Resources.Resource.Process)
            {
                task.Type = TaskType.CreateProcess;
                task.Name = Resources.Resource.Process;
                task = tskCreateProcess.GetCurrentState();
                tskCreateProcess.ValidateFields();
            }
            else

                if (name == Resources.Resource.SendFile)
                {
                    task.Type = TaskType.SendFile;
                    task.Name = Resources.Resource.SendFile;
                    task = tskSendFile.GetCurrentState();
                    tskSendFile.ValidateFields();
                }
                else

                    if (name == Resources.Resource.MenuSystemInfo)
                    {
                        task.Type = TaskType.SystemInfo;
                        task.Name = Resources.Resource.MenuSystemInfo;
                        task = tskSystemInfo.GetCurrentState();
                        tskSystemInfo.ValidateFields();
                    }
                    else

                        if (name == Resources.Resource.TaskNameListProcesses)
                        {
                            task.Type = TaskType.ListProcesses;
                            task.Name = Resources.Resource.TaskNameListProcesses;
                            task = tskListProcesses.GetCurrentState();
                            tskListProcesses.ValidateFields();
                        }
                        else
                            if (name == Resources.Resource.TaskNameComponentState)
                            {
                                task.Type = TaskType.ComponentState;
                                task.Name = Resources.Resource.TaskNameComponentState;
                                task = tskComponentState.GetCurrentState();
                                tskComponentState.ValidateFields();
                            }

                            else

                                if (name == Resources.Resource.CongLdrConfigureLoader)
                                {
                                    task.Type = TaskType.ConfigureLoader;
                                    task.Name = Resources.Resource.CongLdrConfigureLoader;
                                    task = tskConfigureLoader.GetCurrentState();
                                    tskConfigureLoader.ValidateFields();

                                }
                                else

                                    if (name == Resources.Resource.CongLdrConfigureMonitor)
                                    {
                                        task.Type = TaskType.ConfigureMonitor;
                                        task.Name = Resources.Resource.CongLdrConfigureMonitor;
                                        task = tskConfigureMonitor.GetCurrentState();
                                        tskConfigureMonitor.ValidateFields();
                                    }
                                    else
                                        if (name == Resources.Resource.TaskNameRunScanner)
                                        {
                                            task.Type = TaskType.RunScanner;
                                            task.Name = Resources.Resource.TaskNameRunScanner;
                                            task = tskRunScanner.GetCurrentState();
                                            tskRunScanner.ValidateFields();
                                        }
                                        else
                                            if (name == Resources.Resource.TaskNameConfigureQuarantine)
                                            {
                                                task.Type = TaskType.ConfigureQuarantine;
                                                task.Name = Resources.Resource.TaskNameConfigureQuarantine;
                                                task = tskConfigureQuarantine.GetCurrentState();
                                                tskConfigureQuarantine.ValidateFields();
                                            }

                                            else
                                                if (name == Resources.Resource.TaskNameRestoreFileFromQtn)
                                                {
                                                    //!OPTM---�������, ��������, ��������, �.�. ��� ��������������� �
                                                    //GetCurrentState
                                                    task.Type = TaskType.RestoreFileFromQtn;
                                                    task.Name = Resources.Resource.TaskNameRestoreFileFromQtn;
                                                    task = tskRestoreFileFromQtn.GetCurrentState();
                                                    tskRestoreFileFromQtn.ValidateFields();
                                                }

                                                else
                                                {
                                                    task = collection.Get(name);
                                                    //editing = "";
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

        //
        string type = task.Type.ToString();
        Session["CurrentUserTask"] = task;

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
            ComputersManager cmng = new ComputersManager(conn);

            conn.OpenConnection();
            conn.CheckConnectionState(true);

         
            CompFilterEntity filter;
            if (Session["CurrentCompFilter"] == null)
            {
                filter = new CompFilterEntity();
                filter.ComputerName = "*";
                filter.GenerateSQLWhereStatement();
            }
            else
                filter = (CompFilterEntity)Session["CurrentCompFilter"];

            if (Session["TempGroupComputers"] != null)
            {
                filter.GetSQLWhereStatement =
                    ((FilterEntity)Session["TempGroup"]).GetSQLWhereStatement;
            }

            int count = cmng.Count(filter.GetSQLWhereStatement);


            gvExcel.DataSource = cmng.List(filter.GetSQLWhereStatement,
                Convert.ToString(Session["CompSorting"]), 1, count);
            gvExcel.DataBind();

            conn.CloseConnection();
        }

        DataGridToExcel.Export("Computers.xls", gvExcel);
 
    }

    /// <summary>
    /// ���������� ������ �������� ��������� �����������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnDeleteComputers_Click(object sender, EventArgs e)
    {
        List<Int16> list = new List<Int16>();

        if (Session["CheckBoxs"] == null)
        {
            //throw new Exception(Resources.Resource.NoSelected);
            return;
        }

        //�������� ��������� ��������� ID ������
        Hashtable htbl = (Hashtable)Session["CheckBoxs"];
        Regex reg = new Regex(@"\$DataList1\$\w+");

        foreach (string str in Request.Form)
        {
            Match match = reg.Match(str);
            if ((match.Success) && (htbl.ContainsKey(match.Value)))
            {
                list.Add(Convert.ToInt16(htbl[match.Value]));
            }
        }

        //�� ���� ������ ��������
       // if (list.Count == 0)
       //     throw new Exception(Resources.Resource.NoSelectedComputers);

        Session["SelectedID"] = list;


        string connStr = ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString;

        //if (Session["SelectedID"] == null)
         //   throw new Exception(Resources.Resource.ErrorCriticalError + ": Session['SelectedID'] == null ");

        using (VlslVConnection conn = new VlslVConnection(connStr))
        {
            ComputersManager db = new ComputersManager(conn);
            conn.OpenConnection();
            foreach (Int16 id in list)
            {
                db.Delete(id);
            }
            conn.CloseConnection();
           
        }

        UpdateData();

    }


    #region Policies

    /// <summary>
    /// Return all policy type names
    /// </summary>
    private void InitPoliciesList()
    {
        try
        {
            PolicyProvider provider = PoliciesState;
            List<string> policyList = provider.GetAllPolicyTypesNames();
            policyList.Sort();
            ddlPolicyName.DataSource = policyList;
            ddlPolicyName.DataBind();
        }
        catch
        {
            divMainPolicy.Visible = false;
        }
    }

    protected void lbtnEditPolicy_Click(object sender, EventArgs e)
    {
        string selectedPolicyName = "";
        if (ddlPolicyName.SelectedItem != null)
            selectedPolicyName = ddlPolicyName.SelectedItem.Text;
        if ((selectedPolicyName == "(undefined)") || (String.IsNullOrEmpty(selectedPolicyName)))
            return;

        Response.Redirect("PoliciesPage.aspx?Mode=Edit&Name=" + selectedPolicyName);
    }


    protected void lbtnDeletePolicy_Click(object sender, EventArgs e)
    {
        string selectedPolicyName = "";
        if (ddlPolicyName.SelectedItem != null)
            selectedPolicyName = ddlPolicyName.SelectedItem.Text;
        if ((selectedPolicyName == "(undefined)") || (String.IsNullOrEmpty(selectedPolicyName)))
            return;

        PolicyProvider provider = PoliciesState;
        provider.RemovePolicy(selectedPolicyName);

        Session["CompShowMode"] = null;

        Response.Redirect("Computers.aspx");

    }
    protected void lbtnCreatePolicy_Click(object sender, EventArgs e)
    {
        Response.Redirect("PoliciesPage.aspx?Mode=Create");
    }




    protected void lbtnApplyPolicyToComps_Click(object sender, EventArgs e)
    {
        //get selected policy name
        string selectedPolicyName = ddlPolicyName.SelectedValue;
        if ((selectedPolicyName == "(undefined)") || (String.IsNullOrEmpty(selectedPolicyName)))
            return;

        string[] ipAddr = new string[0];
        string[] compName = new string[0];
        string[] vbaVersion = new string[0];
        if (!InitSelectedComputers(ref ipAddr, ref compName, ref vbaVersion)) return;

        List<string> list = new List<string>();
        //Convert fro array to list
        foreach (string s in compName)
            list.Add(s);


        PolicyProvider provider = PoliciesState;

        //Get selected policy
        Policy policy = provider.GetPolicyByName(selectedPolicyName);

        //Add computers to policy
        provider.AddComputersToPolicy(policy, list);

        lblMessage.Text = selectedPolicyName + ": " + Resources.Resource.PolicyApplied;
        mpPicture.Attributes["class"] = "ModalPopupPictureSuccess";        
        CorrectPositionModalPopup();
        ModalPopupExtender.Show();

    }


    protected void lbtnShowCompsByPolicy_Click(object sender, EventArgs e)
    {
        if (divFilterHeader.Attributes["class"] == "GiveButton1") return;
        //get selected policy name
        string selectedPolicyName = ddlPolicyName.SelectedValue;
        if ((selectedPolicyName == "(undefined)") || (String.IsNullOrEmpty(selectedPolicyName)))
            return;

        if (e != null)
            Session["CompShowMode"] = 2;
        else
            if ((int?)Session["CompShowMode"] != 2) Session["CompShowMode"] = null;
        UpdateData();



        //PolicyProvider provider = PoliciesState;
        //Policy policy = provider.GetPolicyByName(selectedPolicyName);

        // count = provider.GetComputerByPolicyCount(policy);
        //computers = provider.GetComputersByPolicyPage(policy, pcPaging.CurrentPageIndex,Convert.ToInt32(Session["CompPageSize"]),
        //    Convert.ToString(Session["CompSorting"]));

    }


    protected void lbtnRemoveCompsFromPolicy_Click(object sender, EventArgs e)
    {
        //get selected policy name
        string selectedPolicyName = ddlPolicyName.SelectedValue;
        if ((selectedPolicyName == "(undefined)") || (String.IsNullOrEmpty(selectedPolicyName)))
            return;

        string[] ipAddr = new string[0];
        string[] compName = new string[0];
        string[] vbaVersion = new string[0];
        if (!InitSelectedComputers(ref ipAddr, ref compName, ref vbaVersion)) return;

        List<string> list = new List<string>();
        //Convert fro array to list
        foreach (string s in compName)
            list.Add(s);


        PolicyProvider provider = PoliciesState;

        //Get selected policy
        Policy policy = provider.GetPolicyByName(selectedPolicyName);

        //Add computers to policy
        provider.RemoveComputersFromPolicy(policy, list);


        InitFields();
    }

    public PolicyProvider PoliciesState
    {
        get
        {
            PolicyProvider provider = Application["PoliciesState"] as PolicyProvider;
            if (provider == null)
            {
                provider = new PolicyProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                Application["PoliciesState"] = provider;
            }

            return provider;
        }

    }

    protected void lbtnPolicyPanel_Click(object sender, ImageClickEventArgs e)
    {
        InitializeSession();
        
        bool tcVis = (bool)Session["CompPolicyPanelVisible"];
        Session["CompPolicyPanelVisible"] = !tcVis;

        InitShowPanelImageButton("CompPolicyPanelVisible", divPolicyPanel, lbtnPolicyPanel);
        
        
        UpdateData();
    }


    private string GetDefaultPolicyNameFromRegistry()
    {
        string registryControlCenterKeyName;
        RegistryKey key;
        try
        {
            //!-OPTM ������� ����� �������� � App_Code � ����� ���� ���
            if (System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8)
                registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
            else
                registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

            key = Registry.LocalMachine.OpenSubKey(registryControlCenterKeyName); ;

        }
        catch
        {
            return String.Empty;
            //throw new ArgumentException("Registry open 'ControlCenter' key error: " + ex.Message);
        }
       
       return (string)key.GetValue("DefaultPolicy");

    }


    private void InitDefaultPolicy()
    {
        string url = Request.ApplicationPath + "/App_Themes/" + Profile.Theme + "/Images/";
        string defaultPolicy = GetDefaultPolicyNameFromRegistry();
        if (ddlPolicyName.SelectedItem != null)
        {
            if ((String.IsNullOrEmpty(defaultPolicy)) || (defaultPolicy != ddlPolicyName.SelectedItem.Text))
                imbtnIsDefaultPolicy.ImageUrl = url + "disabled.gif";
            else
                imbtnIsDefaultPolicy.ImageUrl = url + "enabled.gif";
        }
        else
        {
            imbtnIsDefaultPolicy.Visible = false;
        }     
    }


    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imbtnIsDefaultPolicy_Click(object sender, ImageClickEventArgs e)
    {
        bool retVal = true;
        
        string currentDefaultPolicy =  GetDefaultPolicyNameFromRegistry();

        string newDefaultPolicyName = 
            currentDefaultPolicy == ddlPolicyName.SelectedItem.Text ? "" : ddlPolicyName.SelectedItem.Text;


        try
        {
            IVba32Settings remoteObject = (Vba32.ControlCenter.SettingsService.IVba32Settings)Activator.GetObject(
                       typeof(Vba32.ControlCenter.SettingsService.IVba32Settings),
                       ConfigurationManager.AppSettings["Vba32SS"]);

            string xml = String.Format("<VbaSettings><ControlCenter>" +
                "<DefaultPolicy type=" + "\"reg_sz\"" + ">{0}</DefaultPolicy>" +
                "</ControlCenter></VbaSettings>", newDefaultPolicyName);

            retVal = remoteObject.ChangeRegistry(xml);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("DefaultPolicy: " +
                ex.Message + " " + Resources.Resource.Vba32SSUnavailable);
        }

        if (!retVal)
            throw new ArgumentException("Reread: Vba32SS return false!");

        PoliciesState.ClearCache();

        InitDefaultPolicy();
    }

    protected void ddlPolicyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbtnShowCompsByPolicy_Click(null, null);
    }

    protected void lbtnPolicyManagement_Click(object sender, EventArgs e)
    {
        Response.Redirect("PoliciesPage.aspx?Mode=Create");
    }

    #endregion




    
}