using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Filters.Common;
using Filters.TemporaryGroup;
using ARM2_dbcontrol.Common;
using ARM2_dbcontrol.DataBase;
using System.Configuration;

public partial class Controls_TemporaryGroupFilter : System.Web.UI.UserControl
{
    #region LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitControls();
        }
        PopulateComputersList();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        RegisterScripts();
    }

    private void PopulateComputersList()
    {
        _computersList = TemporaryGroupFilterStorage.Storage as List<String>;
    }

    private void InitControls()
    {
        imgOptionsHideShow.ImageUrl = "~/App_Themes/" + Profile.Theme + "/Images/arrow_down.gif";
    }

    private void RegisterScripts()
    {
        //register jQuery
	Page.ClientScript.RegisterClientScriptInclude("jQuery", @"js/jQuery/jquery-1.10.2.min.js");



        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "TemporaryGroupFilterRegisterClickEvents",
            "TemporaryGroupFilter.RegisterClickEvents();", true);
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        Page.RegisterRequiresControlState(this);
    }
    #endregion

    #region Properties
    #region Data
    private InformationListTypes _informationListType = InformationListTypes.None;
    public InformationListTypes InformationListType
    {
        get { return _informationListType; }
        set { _informationListType = value; }
    }

    private string _where;
    public string Where {
        get { return _where; }
        set { _where = value; }
    }

    private List<string> _computersList;

    public string Computers
    {
        get { return TemporaryGroupFilterHelper.GenerateSqlForComputersList(_computersList); }
    }
    #endregion

    #region Controls
    public bool FilteringOn
    {
        get
        {
            return (divFilterHeader.Attributes["class"] == "GiveButton1");
        }
        set
        {
            divFilterHeader.Attributes["class"] = String.Format("GiveButton{0}", value ? "1" : "");
        }
    }
    #endregion

    #region Events
    [
       Category("Action"),
       Description("Temprorary group filter change event"),
    ]
    public event EventHandler<FilterEventArgs> TemporaryGroupFilterChange;
    protected void OnTemporaryGroupFilterChange(string where)
    {
        EventHandler<FilterEventArgs> temp = TemporaryGroupFilterChange;
        if (temp != null)
        {

            temp(this, new FilterEventArgs(where));
        }
    }
    #endregion
    #endregion

    #region Helper Methods
    protected void ClearFilter()
    {
        OnTemporaryGroupFilterChange(String.Empty);
        FilteringOn = false;
    }

    protected void ApplyFilter()
    {
        string computers = Computers;
        OnTemporaryGroupFilterChange(computers);
        if (!String.IsNullOrEmpty(computers))
        {
            FilteringOn = true;
        }
    }

    protected void AddComputers()
    {
        if (_computersList == null)
        {
            _computersList = new List<string>();
        }
        List<String> newComputers = TemporaryGroupContainer.GetComputerNameList(_informationListType, _where);
        if (newComputers != null)
        {
            _computersList.AddRange(newComputers);
        }
        TemporaryGroupFilterStorage.Storage = _computersList;
    }

    protected void ClearComputers()
    {
        if (_computersList != null)
        {
            _computersList.Clear();
        }
        TemporaryGroupFilterStorage.Storage = _computersList;
    }
    #endregion

    #region Buttons
    protected void lbtnTempGroupHeader_Click(object sender, EventArgs e)
    {
        if (!FilteringOn)
        {
            ApplyFilter();
        }
        else
        {
            ClearFilter();
        }
    }

    protected void lbtnAddToTempGroup_Click(object sender, EventArgs e)
    {
        AddComputers();
    }

    protected void lbtnClearTempGroup_Click(object sender, EventArgs e)
    {
        ClearComputers();
    }

    protected void lbtnApplyTempGroup_Click(object sender, EventArgs e)
    {
        ApplyFilter();
    }

    protected void lbtnCancelTempGroup_Click(object sender, EventArgs e)
    {
        ClearFilter();
    }
    #endregion

    #region ViewState
    protected override void LoadControlState(object savedState)
    {
        object[] ctlState = (object[])savedState;
        base.LoadControlState(ctlState[0]);
        _where = (string)ctlState[1];
    }

    protected override object SaveControlState()
    {
        object[] ctlState = new object[2];
        ctlState[0] = base.SaveControlState();
        ctlState[1] = _where;
        return ctlState;
    }
    #endregion
}