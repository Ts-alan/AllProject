using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirusBlokAda.RemoteOperations.RemoteScan;
using VirusBlokAda.RemoteOperations.RemoteScan.RemoteInfo;
using VirusBlokAda.RemoteOperations.Common;
using VirusBlokAda.RemoteOperations.Net;
using System.Net;
using System.Web.Security;
using System.Diagnostics;
using System.Text;
using System.Web.UI.HtmlControls;
using VirusBlokAda.RemoteOperations.MsiInfo;
using VirusBlokAda.RemoteOperations.RemoteInstall;
using System.Configuration;
using ARM2_dbcontrol.DataBase;
using System.Web.Services; 

public partial class AsynchLanScan : PageBase
{
    #region Page Life Cycle
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole("Administrator"))
        {
            Response.Redirect("Default.aspx");
        }
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        RegisterScripts();
        RegisterLinks();
        LoadSettingsState();
        if (!Page.IsPostBack)
        {
            InitFields();
        }
        Page.Title = Resources.Resource.PageLanScanTitle;
    }

    private void RegisterScripts()
    {
        RegisterScript(@"js/jQuery/jquery-1.3.2.js");
        RegisterScript(@"js/jQuery/ui.core.js");
        RegisterScript(@"js/jQuery/ui.draggable.js");        
        RegisterScript(@"js/jQuery/ui.dialog.js");
        RegisterScript(@"js/jQuery/ui.tabs.js");
        RegisterScript(@"js/jQuery/jquery.cookie.js");

        RegisterScript(@"js/Timer.js");
        RegisterScript(@"js/jQuery/jquery.progressbar.js");
        RegisterScript(@"js/jQuery/jquery.menu.js");
    }

    private void RegisterLinks()
    {
        //RegisterLink("~/App_Themes/" + Profile.Theme + @"/ui.all.css");
        RegisterLink("~/App_Themes/" + Profile.Theme + @"/jquery.menu.css");
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            AdjustSelection();
            SaveSelection();
        }

        Controls_PagerUserControl.AddGridViewExtendedAttributes(GridView1, ObjectDataSource1);
        if (!IsPostBack)
        {
            SetUpServerTimer();
        }

        if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
        {
            string eventArg = Request["__EVENTARGUMENT"];
            if (eventArg == ResetSortingStr)
            {
                ResetSorting();
            }
            else if (eventArg == SortIsSelectedAscStr)
            {
                SortIsSelectedAsc();
            }
            else if (eventArg == SortIsSelectedDescStr)
            {
                SortIsSelectedDesc();
            }
        }
    }

    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetButtonsAccoringToState();
        SetUpClientTimer();
        SetUpProgressBar();
        SetUpOptions();
        SetUpSelectionOptions();
    }

    protected override void InitFields()
    {
        lbtnAddNew.Text = Resources.Resource.Add;
        lbtnRemove.Text = Resources.Resource.Delete;

        lblDomain.Text = Resources.Resource.DomainName;
        lblLogin.Text = Resources.Resource.Login;
        lblPass.Text = Resources.Resource.PasswordLabelText;

        lblPingCount.Text = Resources.Resource.RequestPacketCount;
        lblPingTimeout.Text = Resources.Resource.Timeout;
        rangePingCount.ErrorMessage = String.Format(Resources.Resource.ValueBetween, 1, 10);
        rangePingTimeout.ErrorMessage = String.Format(Resources.Resource.ValueBetween, 1, 100);
        
        lbtnInstall.Text = Resources.Resource.Install;
        cbRebootAfterInstall.Text = Resources.Resource.RebootAfterInstall;

        lblNTW.Text = Vba32VersionInfo.Vba32NTW;
        lblNTS.Text = Vba32VersionInfo.Vba32NTS;
        lblVISTA.Text = Vba32VersionInfo.Vba32Vista;
        lblVIS.Text = Vba32VersionInfo.Vba32Vis;
        lblRemoteConsoleScanner.Text = Resources.Resource.RemoteConsoleScanner;
        lbtnNTW.Text = Resources.Resource.Load;
        lbtnNTS.Text = Resources.Resource.Load;
        lbtnVis.Text = Resources.Resource.Load;
        lbtnVista.Text = Resources.Resource.Load;
        lbtnRemoteConsoleScanner.Text = Resources.Resource.Load;

        UpdateMSIPathes();

        rbtnlProviders.Items.Add(Resources.Resource.WMI);
        rbtnlProviders.Items.Add(Resources.Resource.RemoteService);
        rbtnlProviders.SelectedIndex = 0;

        ddlInstallProduct.Items.Add(Resources.Resource.Antivirus);
        ddlInstallProduct.Items.Add(Resources.Resource.RemoteConsoleScanner);
    }

    private void UpdateMSIPathes()
    {
        Vba32MsiStorage msiStorage = new Vba32MsiStorage();
        msiStorage.Read();
        lblNTW_MSI.Text = msiStorage.GetPathMSI(Vba32VersionInfo.Vba32NTW);
        lblNTS_MSI.Text = msiStorage.GetPathMSI(Vba32VersionInfo.Vba32NTS);
        lblVIS_MSI.Text = msiStorage.GetPathMSI(Vba32VersionInfo.Vba32Vis);
        lblVISTA_MSI.Text = msiStorage.GetPathMSI(Vba32VersionInfo.Vba32Vista);
        lblRemoteConsoleScanner_MSI.Text = msiStorage.GetPathMSI("RemoteConsoleScanner");
    }
    #endregion

    #region SettingsState
    private void LoadSettingsState()
    {
        tboxLoginCr.Text = login;
        tboxDomainCr.Text = domain;
        rbtnlProviders.SelectedIndex = provider;
        txtPingCount.Text = pingCount.ToString();
        txtPingTimeout.Text = pingTimeout.ToString();
        if (scanComputerList != null)
        {
            foreach (string next in scanComputerList)
            {
                lboxCompIncludeList.Items.Add(next);
            }
        }
    }

    private void SaveSettingsState()
    {
        login = tboxLoginCr.Text;
        domain = tboxDomainCr.Text;
        provider = rbtnlProviders.SelectedIndex;
        pingCount = GetPingCount();
        pingTimeout = GetPingTimeout();
        scanComputerList = new List<string>();
        foreach (ListItem next in lboxCompIncludeList.Items)
            scanComputerList.Add(next.Text);
    }

    private void SaveIpRangesState()
    {
        scanComputerList = new List<string>();
        foreach (ListItem next in lboxCompIncludeList.Items)
            scanComputerList.Add(next.Text);
    }
    #endregion

    #region ClientTimer
    private void SetUpClientTimer()
    {
        string key = "TimerScript";
        StringBuilder script = new StringBuilder();

        if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
        {
            switch (progressType)
            {
                case ProgressType.Clear:
                    script.Append("SetUpTimer(0);");
                    break;
                case ProgressType.Start:
                    script.Append("SetUpTimer(0); StartTimer(); ");
                    break;
                case ProgressType.Resume:
                    script.Append("StartTimer();");
                    break;
                case ProgressType.Stop:
                case ProgressType.Pause:
                    script.Append("StopTimer();");
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (remoteScanner == null)
            {
                script.Append(String.Format("SetUpLabel('{0}'); SetUpTimer('{1}'); ", lblTimer.ClientID,
                    0));
            }
            else
            {
                script.Append(String.Format("SetUpLabel('{0}'); SetUpTimer('{1}'); ", lblTimer.ClientID,
                    remoteScanner.ElapsedTime));
                if (remoteScanner.IsRunning)
                {
                    script.Append("StartTimer();");
                }
            }
        }
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), key, script.ToString(), true);
    }
    #endregion

    #region Progress Buttons
    private enum ProgressType
    {
        None,
        Clear,
        Start,
        Stop,
        Pause,
        Resume
    }

    private ProgressType progressType= ProgressType.None;         
    
    protected void btnPause_Click(object sender, EventArgs e)
    {
        progressType = ProgressType.Pause;
        if (!remoteScanner.IsCompleted)
        {
            remoteScanner.PauseScan();
        }
        Timer1.Enabled = false;
        ReloadGridView();
    }

    protected void btnResume_Click(object sender, EventArgs e)
    {
        progressType = ProgressType.Resume;
        remoteScanner.ContinueScan();
        Timer1.Enabled = true;
    }

    protected void btnStart_Click(object sender, EventArgs e)
    {
        try
        {
            StopScan(true);
            SaveSettingsState();
            ClearScan();
            StartScan();
            progressType = ProgressType.Start;
            Timer1.Enabled = true;
        }
        catch (ArgumentException)
        {
            string key = "StartScanErrorScript";
            string script =
                @"$(document).ready(function () { 
                    alert('" + Resources.Resource.EmptyIPListAfterExclusion + @"');
                });";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), key, script.ToString(), true);
            progressType = ProgressType.Clear;
        }

    }

    protected void btnStop_Click(object sender, EventArgs e)
    {
        progressType = ProgressType.Stop;
        StopScan(false);
        Timer1.Enabled = false;
        ReloadGridView();
    }

    private void SetButtonsAccoringToState()
    {
        ProgressStateEnum state = ProgressStateEnum.Stopped;
        if (remoteScanner != null)
        {
            state = remoteScanner.State;
        }
        switch (state)
        {
            case ProgressStateEnum.Stopped:
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnPause.Enabled = false;
                btnPause.Visible = true;
                btnResume.Enabled = false;
                btnResume.Visible = false;
                break;
            case ProgressStateEnum.Running:
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                btnPause.Enabled = true;
                btnPause.Visible = true;
                btnResume.Enabled = false;
                btnResume.Visible = false;
                break;
            case ProgressStateEnum.Paused:
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                btnPause.Enabled = false;
                btnPause.Visible = false;
                btnResume.Enabled = true;
                btnResume.Visible = true;
                break;
        }
    }
    #endregion

    #region Progress Helper Methods
    private void StartScan()
    {
        remoteScanner = new RemoteScanner(GetCredentials(), 40);
        remoteScanner.PingTimeout = new TimeSpan(0, 0, GetPingTimeout());
        remoteScanner.PingCount = GetPingCount();

        remoteScanner.ScanIPRangeList.AddRange(GetIpRanges());

        remoteScanner.ExcludeIPAddressList = GetComputersListToExclude();
        remoteScanner.MethodType = GetRemoteMethod();

        scanResultDict = new Dictionary<int, RemoteInfoEntityShow>();

        remoteScanner.StartScan();
    }


    private void ClearScan()
    {        
        remoteScanner = null;
        scanResultDict = null;
        ReloadGridView();
    }

    private void StopScan(bool deleteResult)
    {
        if (remoteScanner != null)
        {
            if (!remoteScanner.IsCompleted)
            {
                remoteScanner.StopScan(deleteResult);
            }          
        }
    }
    #endregion

    #region Grid
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                displayedIdsList = new List<int>();                
                break;
            case DataControlRowType.DataRow:
                RemoteInfoEntityShow rie = e.Row.DataItem as RemoteInfoEntityShow;
                Image imgLoader = e.Row.FindControl("imgLoader") as Image;
                imgLoader.ImageUrl = String.Format("~/App_Themes/{0}/Images/{1}", Profile.Theme,
                    rie.IsLoaderPortOpen ? "enabled.gif" : "disabled.gif");
                Image imgAgent = e.Row.FindControl("imgAgent") as Image;
                imgAgent.ImageUrl = String.Format("~/App_Themes/{0}/Images/{1}", Profile.Theme,
                    rie.IsLoaderPortOpen ? "enabled.gif" : "disabled.gif");
                CheckBox cboxIsSelected = e.Row.FindControl("cboxIsSelected") as CheckBox;
                cboxIsSelected.Checked = rie.IsSelected;
                displayedIdsList.Add(rie.Id);
                if (rie.IsDisabled)
                {
                    cboxIsSelected.Enabled = false;
                }
                else
                {
                    cboxIsSelected.Enabled = true;
                }

                if (String.IsNullOrEmpty(rie.OSVersion))
                {
                    String comment = ScanningObjectState.GetComment(rie.IPAddress.ToString());
                    if (String.IsNullOrEmpty(comment))
                        (e.Row.FindControl("lblInformation") as Label).Text = rie.ErrorInfo;
                    else
                    {
                        (e.Row.FindControl("lblInformation") as Label).Text = comment;
                        (e.Row.FindControl("imgComment") as HtmlGenericControl).Attributes.Add("comment", "true");
                    }
                }
                else
                {
                    (e.Row.FindControl("lblInformation") as Label).Text = rie.OSVersion;
                    (e.Row.FindControl("imgComment") as HtmlGenericControl).Attributes.Add("disabled", "true");
                }

                break;
            default:
                break;
        }
    }

    protected void ObjectDataSource1_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        ScanResultDataSource data = new ScanResultDataSource(remoteScanner, scanResultDict);
        e.ObjectInstance = data;
    }

    private void ReloadGridView()
    {
        ObjectDataSource1.DataBind();
        GridView1.DataBind();
    }

    protected void ResetSorting()
    {
        GridView1.Sort("", SortDirection.Ascending);
    }

    protected void SortIsSelectedAsc()
    {
        GridView1.Sort("IsSelected", SortDirection.Ascending);
    }

    protected void SortIsSelectedDesc()
    {
        GridView1.Sort("IsSelected", SortDirection.Descending);
    }

    protected void imgOptions_Init(object sender, EventArgs e)
    {
        (sender as Image).ImageUrl = "~/App_Themes/"+ Profile.Theme + "/Images/cog.png";
    }


    protected void hfSelectOptions_Prerender(object sender, EventArgs e)
    {
        (sender as HiddenField).Value = SelectOptions.None.ToString();
    }

    private enum SelectOptions
    {
        None,
        SelectAll,
        UnselectAll
    }

    protected void AdjustSelection()
    {
        SelectOptions opt = (SelectOptions) Enum.Parse(typeof(SelectOptions), hfSelectOptions.Value);
        switch (opt)
        { 
            case SelectOptions.None:
                break;
            case SelectOptions.SelectAll:
                ChangeSelection(true);
                break;
            case SelectOptions.UnselectAll:
                ChangeSelection(false);
                break;
        }

    }

    protected void ChangeSelection(bool doSelect)
    {
        foreach (RemoteInfoEntityShow rie in scanResultDict.Values)
        { 
            if (displayedIdsList.Contains(rie.Id))
            {
                continue;
            }
            if (!rie.IsDisabled)
            {
                rie.IsSelected = doSelect;
            }
        }
    }

    protected void SaveSelection()
    {
        if (scanResultDict == null || displayedIdsList == null) return;
        int count = (displayedIdsList.Count < GridView1.Rows.Count) ? 
            displayedIdsList.Count : GridView1.Rows.Count;
        for (int i = 0; i < count; i++)
        {
            GridViewRow nextRow = GridView1.Rows[i];
            bool isSelected = (nextRow.FindControl("cboxIsSelected") as CheckBox).Checked;
            int id = displayedIdsList[i];
            scanResultDict[id].IsSelected = isSelected;
        }
    }

    private string selectAllKey = "SelectAll";
    private string totalCountSelectedKey = "TotalCountSelected";
    private string totalCountAvailableKey = "TotalCountAvailable";
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        //Remove selection if not postback
        if (scanResultDict == null) return;

        //Count all available for selection
        int totalCountAvailable = 0;
        //Count selected on all pages items
        int totalCountSelected = 0;

        if (!IsPostBack)
        {

            foreach (RemoteInfoEntityShow next in scanResultDict.Values)
            {
                next.IsSelected = false;
                if (!next.IsDisabled)
                {
                    totalCountAvailable++;
                }
            }
        }
        else
        {
            foreach (RemoteInfoEntityShow next in scanResultDict.Values)
            {
                if (next.IsSelected)
                {
                    totalCountSelected++;
                }
                if (!next.IsDisabled)
                {
                    totalCountAvailable++;
                }
            }
        }
        GridView1.Attributes[totalCountSelectedKey] = totalCountSelected.ToString();
        GridView1.Attributes[totalCountAvailableKey] = totalCountAvailable.ToString();
    }

    #endregion

    #region Storage Properties
    //refactor
    protected Dictionary<string, object> ScanStorageCollection
    {
        get
        {
            if (ScanStorage.Storage == null)
            {
                ScanStorage.Storage = new Dictionary<string, object>();
            }
            return (ScanStorage.Storage as Dictionary<string, object>);
        }
    }

    private string remoteScannerKey = "RemoteScanner";
    protected RemoteScanner remoteScanner
    {
        get
        {
            if (ScanStorageCollection.ContainsKey(remoteScannerKey)) {
                return ScanStorageCollection[remoteScannerKey] as RemoteScanner;
            }
            return null;
        }
        set
        {
            if (!ScanStorageCollection.ContainsKey(remoteScannerKey))
            {
                ScanStorageCollection.Add(remoteScannerKey, null);
            }
            ScanStorageCollection[remoteScannerKey] = value;
        }
    }

    private string scanResultDictKey = "ShowingDict";
    protected Dictionary<int, RemoteInfoEntityShow> scanResultDict
    {
        get
        {
            if (ScanStorageCollection.ContainsKey(scanResultDictKey))
            {
                return ScanStorageCollection[scanResultDictKey] as Dictionary<int, RemoteInfoEntityShow>;
            }
            return null;
        }
        set
        {
            if (!ScanStorageCollection.ContainsKey(scanResultDictKey))
            {
                ScanStorageCollection.Add(scanResultDictKey, null);
            }
            ScanStorageCollection[scanResultDictKey] = value;
        }
    }

    private string displayedIdsListKey = "DisplayedIdsList";
    protected List<int> displayedIdsList
    {
        get
        {
            if (ScanStorageCollection.ContainsKey(displayedIdsListKey))
            {
                return ScanStorageCollection[displayedIdsListKey] as List<int>;
            }
            return null;
        }
        set
        {
            if (!ScanStorageCollection.ContainsKey(displayedIdsListKey))
            {
                ScanStorageCollection.Add(displayedIdsListKey, null);
            }
            ScanStorageCollection[displayedIdsListKey] = value;
        }
    }

    protected Dictionary<string, object> SettingsStorageCollection
    {
        get
        {
            if (SettingsStorage.Storage == null)
            {
                SettingsStorage.Storage = new Dictionary<string, object>();
            }
            return (SettingsStorage.Storage as Dictionary<string, object>);
        }
    }

    private string loginKey = "Login";
    protected string login
    {
        get
        {
            if (SettingsStorageCollection.ContainsKey(loginKey))
            {
                return SettingsStorageCollection[loginKey] as string;
            }
            return String.Empty;
        }
        set
        {
            if (!SettingsStorageCollection.ContainsKey(loginKey))
            {
                SettingsStorageCollection.Add(loginKey, null);
            }
            SettingsStorageCollection[loginKey] = value;
        }
    }

    private string domainKey = "Domain";
    protected string domain
    {
        get
        {
            if (SettingsStorageCollection.ContainsKey(domainKey))
            {
                return SettingsStorageCollection[domainKey] as string;
            }
            return String.Empty;
        }
        set
        {
            if (!SettingsStorageCollection.ContainsKey(domainKey))
            {
                SettingsStorageCollection.Add(domainKey, null);
            }
            SettingsStorageCollection[domainKey] = value;
        }
    }
    private string pingCountKey = "PingCount";
    protected Int32 pingCount
    {
        get
        {
            if (SettingsStorageCollection.ContainsKey(pingCountKey))
            {
                try
                {
                    return Convert.ToInt32(SettingsStorageCollection[pingCountKey]);
                }
                catch
                {
                    return 1;
                }
            }
            return 1;
        }
        set
        {
            if (!SettingsStorageCollection.ContainsKey(pingCountKey))
            {
                SettingsStorageCollection.Add(pingCountKey, null);
            }
            SettingsStorageCollection[pingCountKey] = value;
        }
    }
    private string pingTimeoutKey = "pingTimeoutKey";
    protected Int32 pingTimeout
    {
        get
        {
            if (SettingsStorageCollection.ContainsKey(pingTimeoutKey))
            {
                try
                {
                    return Convert.ToInt32(SettingsStorageCollection[pingTimeoutKey]);
                }
                catch
                {
                    return 1;
                }
            }
            return 10;
        }
        set
        {
            if (!SettingsStorageCollection.ContainsKey(pingTimeoutKey))
            {
                SettingsStorageCollection.Add(pingTimeoutKey, null);
            }
            SettingsStorageCollection[pingTimeoutKey] = value;
        }
    }
    private string providerKey = "Provider";
    protected int provider
    {
        get
        {
            if (SettingsStorageCollection.ContainsKey(providerKey))
            {
                int? result = SettingsStorageCollection[providerKey] as int?;
                return result.GetValueOrDefault(0); 
            }
            return 0;
        }
        set
        {
            if (!SettingsStorageCollection.ContainsKey(providerKey))
            {
                SettingsStorageCollection.Add(providerKey, null);
            }
            SettingsStorageCollection[providerKey] = value;
        }
    }
    private string scanComputerListKey = "ScanComputerList";
    protected List<string> scanComputerList
    {
        get
        {
            if (SettingsStorageCollection.ContainsKey(scanComputerListKey))
            {
                return SettingsStorageCollection[scanComputerListKey] as List<string>;
            }
            return null;
        }
        set
        {
            if (!SettingsStorageCollection.ContainsKey(scanComputerListKey))
            {
                SettingsStorageCollection.Add(scanComputerListKey, null);
            }
            SettingsStorageCollection[scanComputerListKey] = value;
        }
    }

    public static ScanningObjectProvider ScanningObjectState
    {
        get
        {
            ScanningObjectProvider provider = HttpContext.Current.Application["ScanningObjectState"] as ScanningObjectProvider;
            if (provider == null)
            {
                provider = new ScanningObjectProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Application["ScanningObjectState"] = provider;
            }

            return provider;
        }
    }

    #endregion

    #region Server Timer
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (remoteScanner == null) return;
        if (remoteScanner.IsCompleted && Timer1.Enabled)
        {
            Timer1.Enabled = false;
            progressType = ProgressType.Stop;
        }
        ReloadGridView();
    }

    private void SetUpServerTimer()
    {
        if (remoteScanner != null)
        {
            if (remoteScanner.IsRunning)
            {
                Timer1.Enabled = true;
            }
        }
    }
    #endregion

    #region ProgressBar
    private void SetUpProgressBar()
    {
        string key = "ProgressBarScript";
        string script = 
            @"$(document).ready(function () { ";

        if (!ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
        {            
            script += @"$('#sProgressBar').progressBar({
                    boxImage: 'App_Themes/" + Profile.Theme + @"/Images/progressbar.gif',
                    barImage: 'App_Themes/" + Profile.Theme + @"/Images/progressbg.gif',
                    width: 400,                    
                    increment: 100, 
                    speed: 1
                });";
        }
        int percent = 0;
        if (remoteScanner != null)
        {
            percent = remoteScanner.ProgressPercentage;
        }
        script += 
                @"$('#sProgressBar').progressBar("+ percent + @");
            }); ";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), key, script.ToString(), true);
    }
    #endregion

    #region Options
    private string ResetSortingStr = "@ResetSorting";
    protected string ResetSortingPostBackStr
    {
        get
        {
            return Page.ClientScript.GetPostBackEventReference(UpdatePanel1, ResetSortingStr);
        }
    }

    private void SetUpOptions()
    {
        string key = "OptionsScript";
        string script =
            @"$(document).ready(function () { 
                $('#divOptionsOpen').menu({
                    menu: '" + divOptionsMenu.ClientID + @"'
                },
			    function (val) {
                    if (val == 'resetSorting') {" +
                        ResetSortingPostBackStr + @";
                    }
			    });
            });";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), key, script.ToString(), true);
    }
    #endregion

    #region Selection Options
    private void SetUpSelectionOptions()
    {
        string key = "SelectionOptionsScript";
        string script =  @"
            var allCheckBoxSelector = '#" + GridView1.ClientID + @" input[id*=""cBoxSelectAll""]:checkbox';
            var checkBoxSelector = '#" + GridView1.ClientID + @" input[id*=""cboxIsSelected""]:checkbox';
            var lblSelectedTotalCountSelector = '#" + lblSelectedTotalCount.ClientID + @"';
            var gridViewSelector = '#" + GridView1.ClientID + @"';
            var hfSelectOptionsSelector = '#" + hfSelectOptions.ClientID + @"';

            function ToggleCheckUncheckAllOptionAsNeeded() {
                var totalCountAvailable = GetTotalCountAvailable();
                var totalCountSelected = GetTotalCountSelected();
                var countCheckboxes = CountCheckboxes();
                var noCheckboxesOnPageAreChecked = (countCheckboxes.checked === 0);
                var allCheckboxesOnPageAreChecked = (countCheckboxes.available === countCheckboxes.checked);
                var noCheckboxesAreChecked = (totalCountSelected === 0);
                var allCheckboxesAreChecked = (totalCountAvailable === totalCountSelected);

                if (countCheckboxes.available == 0)
                {
                    $(allCheckBoxSelector).attr('disabled', true);
                    $(allCheckBoxSelector).attr('checked', false);
                }
                else {
                    $(allCheckBoxSelector).attr('checked', allCheckboxesOnPageAreChecked);
                }

                if (allCheckboxesOnPageAreChecked) {
                    $('#selectionOptionsMenu').disableMenuItems('selectAllPage');
                }
                else {
                    $('#selectionOptionsMenu').enableMenuItems('selectAllPage');
                }

                if (noCheckboxesOnPageAreChecked) {
                    $('#selectionOptionsMenu').disableMenuItems('unselectAllPage');
                }
                else {
                    $('#selectionOptionsMenu').enableMenuItems('unselectAllPage');
                }

                if (allCheckboxesAreChecked) {
                    $('#selectionOptionsMenu').disableMenuItems('selectAll');
                }
                else {
                    $('#selectionOptionsMenu').enableMenuItems('selectAll');
                }

                if (noCheckboxesAreChecked) {
                    $('#selectionOptionsMenu').disableMenuItems('unselectAll');
                }
                else {
                    $('#selectionOptionsMenu').enableMenuItems('unselectAll');
                }

                $(lblSelectedTotalCountSelector).html(totalCountSelected);
                
                if(isNaN(totalCountAvailable))
                {
                    $('#divInstall').hide();
                }
                else
                {
                    if (totalCountAvailable == 0) {
                        $('#divInstall').hide();
                    }
                    else if (!$('#divInstall').is(':visible'))
                    {
                        $('#divInstall').show();
                    }
                }
                
            }
        

            function CountCheckboxes()
            {
                var availableCheckboxes = 0;
                var checkedCheckboxes = 0;
                $(checkBoxSelector).each(function(index) {
                    if (!$(this).is(':disabled'))
                    {
                        availableCheckboxes++;
                    }
                    if ($(this).is(':checked'))
                    { 
                        checkedCheckboxes++;
                    }
                });
                return {available : availableCheckboxes, checked : checkedCheckboxes};
            }

            function SetSelectAll(value) {
                $(gridViewSelector).attr('" + selectAllKey + @"', value);
            }
            
            function GetTotalCountSelected() {
                return parseInt($(gridViewSelector).attr('" + totalCountSelectedKey + @"'));
            }

            function GetTotalCountAvailable() {
                return parseInt($(gridViewSelector).attr('" + totalCountAvailableKey + @"'));
            }

            function SetTotalCountSelected(totalCountSelected) {
                $(gridViewSelector).attr('" + totalCountSelectedKey + @"', totalCountSelected);
            }

            function EditTotalCountOnSelectAllOnPage()
            {
                var totalCountSelected = GetTotalCountSelected();
                var countCheckboxes = CountCheckboxes();
                totalCountSelected -= countCheckboxes.checked;
                totalCountSelected += countCheckboxes.available;  
                SetTotalCountSelected(totalCountSelected);
            }

            function EditTotalCountOnUnselectAllOnPage()
            {
                var totalCountSelected = GetTotalCountSelected();
                var countCheckboxes = CountCheckboxes()
                totalCountSelected -= countCheckboxes.checked;             
                SetTotalCountSelected(totalCountSelected);
            }

            function EditTotalCountOnSelectAll()
            {
                var totalCountAvailable = GetTotalCountAvailable();  
                SetTotalCountSelected(totalCountAvailable);
                $(hfSelectOptionsSelector).val('" + SelectOptions.SelectAll + @"');
            }

            function EditTotalCountOnUnselectAll()
            {           
                SetTotalCountSelected(0);
                $(hfSelectOptionsSelector).val('" + SelectOptions.UnselectAll + @"');
            }

            $(document).ready(function () { 
                $('#divSelectionOptions').menu({
                    menu: 'selectionOptionsMenu'
                },
			    function (val) {
                    if (val=='sortAsc') { " +
                            SortIsSelectedAscPostBackStr + @";
                    }
                    else if (val=='sortDesc') { " +
                            SortIsSelectedDescPostBackStr + @";
                    }
                    else if (val=='selectAllPage') { 
                        EditTotalCountOnSelectAllOnPage();
                        $(checkBoxSelector).each(function() {
                            if (!$(this).is(':disabled'))
                            {
                                $(this).attr('checked', true); 
                            }
                        });                 
                        ToggleCheckUncheckAllOptionAsNeeded();                            
                    }
                    else if (val=='unselectAllPage') { 
                        EditTotalCountOnUnselectAllOnPage();
                            $(checkBoxSelector).each(function() {
                            if (!$(this).is(':disabled'))
                            {
                                $(this).attr('checked', false); 
                            }
                        });
                        ToggleCheckUncheckAllOptionAsNeeded();
                    }
                    else if (val=='selectAll') { 
                        EditTotalCountOnSelectAll();
                        SetSelectAll('select_all');
                        $(checkBoxSelector).each(function() {
                            if (!$(this).is(':disabled'))
                            {
                                $(this).attr('checked', true); 
                            }
                        });                  
                        ToggleCheckUncheckAllOptionAsNeeded();
                    }
                    else if (val=='unselectAll') { 
                        EditTotalCountOnUnselectAll();
                        SetSelectAll('unselect_all');
                        $(checkBoxSelector).each(function() {
                            if (!$(this).is(':disabled'))
                            {
                                $(this).attr('checked', false); 
                            }
                        });
                        ToggleCheckUncheckAllOptionAsNeeded();
                    }
			    });

                $(allCheckBoxSelector).bind('click', function (event) {
                    event.stopPropagation(); 
                    if ($(this).is(':checked'))
                    {
                        EditTotalCountOnSelectAllOnPage();
                    }
                    else
                    {
                        EditTotalCountOnUnselectAllOnPage();
                    }
                    var checked = $(this).is(':checked');
                    $(checkBoxSelector).each(function() {
                        if (!$(this).is(':disabled'))
                        {
                            $(this).attr('checked', checked); 
                        }
                    });
                    ToggleCheckUncheckAllOptionAsNeeded();
                });

                $(checkBoxSelector).bind('click', function () {
                    var totalCountSelected = GetTotalCountSelected();
                    if ($(this).is(':checked'))
                    {                        
                        totalCountSelected++;                        
                    }
                    else
                    {
                        totalCountSelected--;
                    }
                    SetTotalCountSelected(totalCountSelected);
                    ToggleCheckUncheckAllOptionAsNeeded();
                });

                ToggleCheckUncheckAllOptionAsNeeded();              
            });";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), key, script.ToString(), true);        
    }

    private string SortIsSelectedAscStr = "@SortIsSelectedAsc";
    protected string SortIsSelectedAscPostBackStr
    {
        get
        {
            return Page.ClientScript.GetPostBackEventReference(UpdatePanel1, SortIsSelectedAscStr);
        }
    }

    private string SortIsSelectedDescStr = "@SortIsSelectedDesc";
    protected string SortIsSelectedDescPostBackStr
    {
        get
        {
            return Page.ClientScript.GetPostBackEventReference(UpdatePanel1, SortIsSelectedDescStr);
        }
    }
    #endregion

    #region MSI Options
    protected void lbtnNTW_Click(object sender, EventArgs e)
    {
        if (UploadFile(fuNTW, lblNTW_MSI))
            RewriteXML();
    }

    protected void lbtnNTS_Click(object sender, EventArgs e)
    {
        if (UploadFile(fuNTS, lblNTS_MSI))
            RewriteXML();
    }

    protected void lbtnVista_Click(object sender, EventArgs e)
    {
        if (UploadFile(fuVista, lblVISTA_MSI))
            RewriteXML();
    }

    protected void lbtnVis_Click(object sender, EventArgs e)
    {
        if (UploadFile(fuVis, lblVIS_MSI))
            RewriteXML();
    }

    protected void lbtnRemoteConsoleScanner_Click(object sender, EventArgs e)
    {
        if (UploadFile(fuRemoteConsoleScanner, lblRemoteConsoleScanner_MSI))
            RewriteXML();
    }

    private bool UploadFile(FileUpload fu, Label lbl)
    {
        if (fu.HasFile == false)
        {
            return false;
        }
        else
        {
            if (!Vba32MsiStorage.IsMSI(fu.FileName))
            {
                //String csname1 = "PopupScriptMSIFail";
                //Type cstype = this.GetType();
                //// Get a ClientScriptManager reference from the Page class.
                //ClientScriptManager cs = Page.ClientScript;
                //// Check to see if the startup script is already registered.
                //if (!cs.IsStartupScriptRegistered(cstype, csname1))
                //{
                //    String cstext1 = String.Format("alert('{0}.');", Resources.Resource.CorrectOnlyMSI);
                //    cs.RegisterStartupScript(cstype, csname1, cstext1, true);
                //}
                string key = "LoadMSIErrorScript";
                string script =
                    @"$(document).ready(function () { 
                    alert('" + Resources.Resource.LoadOnlyMSI + @"');
                });";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), key, script.ToString(), true);

                return false;
            }
            // Save the file
            string fileName = fu.FileName;

            string filePath = Server.MapPath("~/Downloads/" + fileName);
            fu.SaveAs(filePath);

            lbl.Text = fileName;

            //Change xml file

            return true;
        }
    }

    private void RewriteXML()
    {
        Vba32MsiStorage msiStorage = new Vba32MsiStorage();
        Dictionary<String, String> dict = new Dictionary<String, String>();
        dict.Add(Vba32VersionInfo.Vba32NTW, !String.IsNullOrEmpty(lblNTW_MSI.Text) ? lblNTW_MSI.Text : String.Empty);
        dict.Add(Vba32VersionInfo.Vba32NTS, !String.IsNullOrEmpty(lblNTS_MSI.Text) ? lblNTS_MSI.Text : String.Empty);
        dict.Add(Vba32VersionInfo.Vba32Vista, !String.IsNullOrEmpty(lblVISTA_MSI.Text) ? lblVISTA_MSI.Text : String.Empty);
        dict.Add(Vba32VersionInfo.Vba32Vis, !String.IsNullOrEmpty(lblVIS_MSI.Text) ? lblVIS_MSI.Text : String.Empty);
        dict.Add("RemoteConsoleScanner", !String.IsNullOrEmpty(lblRemoteConsoleScanner_MSI.Text) ? lblRemoteConsoleScanner_MSI.Text : String.Empty);

        msiStorage.Write(dict);
    }
    #endregion  
  
    #region Ip Ranges List Tab

    protected void lbtnImport_Click(object sender, EventArgs e)
    {
        ImportComputersList();
    }

    /// <summary>
    /// Import any comps from file to list
    /// </summary>
    protected void ImportComputersList()
    {
        if (fuImport.HasFile)
        {
            HttpPostedFile file = fuImport.PostedFile;

            System.IO.StreamReader reader =
                new System.IO.StreamReader(file.InputStream);

            while (!reader.EndOfStream)
            {
                string str = reader.ReadLine();
                lboxCompIncludeList.Items.Add(str);
            }

        }
    }

    protected void lbtnAddNew_Click1(object sender, EventArgs e)
    {
        string newName = tboxNewComp.Text;
        if (!String.IsNullOrEmpty(newName))
        {
            lboxCompIncludeList.Items.Add(newName);
        }
        tboxNewComp.Text = "";
        SaveIpRangesState();
    }

    protected void lbtnRemove_Click(object sender, EventArgs e)
    {
        if (lboxCompIncludeList.SelectedIndex != -1)
            lboxCompIncludeList.Items.RemoveAt(lboxCompIncludeList.SelectedIndex);
        SaveIpRangesState();
    }
    #endregion

    #region Install

    protected void lbtnInstall_Click(object sender, EventArgs e)
    {
        bool rebootAfterInstall = cbRebootAfterInstall.Checked;
        string pathDir = Server.MapPath("~/Downloads/");
        List<RemoteInstallEntity> installEntities = new List<RemoteInstallEntity>();

        Vba32MsiStorage msiStorage = new Vba32MsiStorage();
        msiStorage.Read();

        foreach (RemoteInfoEntityShow next in  scanResultDict.Values)
        {
            if (next.IsSelected)
            {
                next.IsDisabled = false;
                RemoteInstallEntity rie = new RemoteInstallEntity();
                rie.IP = next.IPAddress.ToString();
                rie.ComputerName = next.Name;
                rie.VbaVersion = ddlInstallProduct.SelectedIndex == 0 ? Vba32MsiStorage.GetVba32VersionByOSVersion(next.OSVersion) : Vba32VersionInfo.Vba32RemoteConsoleScanner;
                rie.SourceFullPath = pathDir + msiStorage.GetPathMSI(rie.VbaVersion);
                installEntities.Add(rie);
            }
        }

        RemoteInstaller installer = new RemoteInstaller(GetCredentials(), 40, ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
        installer.MethodType = GetRemoteMethod();
        installer.InstallAll(installEntities, rebootAfterInstall);
        Response.Redirect("~/TasksInstall.aspx");
    }

    #endregion

    #region Get Scan Variables
    private List<IPAddress> GetComputersListToExclude()
    {
        List<IPAddress> ipList = new List<IPAddress>();
        List<string> list = null;

        using (VlslVConnection conn =
            new VlslVConnection(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            try
            {
                ComputersManager cmg = new ComputersManager(conn);
                conn.OpenConnection();

                list = cmg.GetRegisteredCompList(true);
            }
            finally
            {
                conn.CloseConnection();
            }
        }

        foreach (string item in list)
        {
            ipList.Add(IPAddress.Parse(item));
        }

        return ipList;
    }


    private Credentials GetCredentials()
    {
        string domain = tboxDomainCr.Text;
        string login = tboxLoginCr.Text;
        string pass = tboxPasswordCr.Text;
        return new Credentials(domain, login, pass);
    }

    private Int32 GetPingCount()
    {
        Int32 count = 1;
        try
        {
            count = Convert.ToInt32(txtPingCount.Text);
        }
        catch { }
        return count;
    }

    private Int32 GetPingTimeout()
    {
        Int32 timeout = 10;
        try
        {
            timeout = Convert.ToInt32(txtPingTimeout.Text);
        }
        catch { }
        return timeout;
    }

    private List<IPRange> GetIpRanges()
    {
        List<IPRange> list = new List<IPRange>();
        foreach (ListItem item in lboxCompIncludeList.Items)
        {
            try
            {
                string[] ipadresses = item.Text.Split(new char[] { '-' });
                if (ipadresses.Length > 1)
                    list.Add(new IPRange(IPAddress.Parse(ipadresses[0]), IPAddress.Parse(ipadresses[1])));
                else list.Add(new IPRange(IPAddress.Parse(ipadresses[0]), IPAddress.Parse(ipadresses[0])));
            }
            catch
            {
                Debug.WriteLine("Error while parsing IP ranges");
            }
        }
        return list;
    }

    private RemoteMethodsEnum GetRemoteMethod()
    {
        if (rbtnlProviders.SelectedValue == Resources.Resource.RemoteService)
        {
            return RemoteMethodsEnum.RemoteService;
        }
        return RemoteMethodsEnum.Wmi;
    }
    #endregion

    #region WebMethods
    [WebMethod]
    public static void SetComment(String ip, String text)
    {
        try
        {
            if (String.IsNullOrEmpty(text.Replace(" ", "")))
                ScanningObjectState.DeleteComment(ip);
            else
                ScanningObjectState.AddComment(new ScanningObjectEntity(ip, text));
        }
        catch (Exception e)
        {
            throw new Exception(String.Format("SetComment() :: ", e.Message));
        }
    }
    #endregion

}