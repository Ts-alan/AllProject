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

using System.Reflection;
using System.IO;
using System.Drawing;

using VirusBlokAda.CC.DataBase;
using ARM2_dbcontrol.Generation;
using ARM2_dbcontrol.Filters;
using System.Collections.Generic;

/// <summary>
/// Settings extra  
/// </summary>
public partial class SettingsExtra : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScript(@"js/jQuery/jquery.cookie.js");
        //RegisterLink("~/App_Themes/" + Profile.Theme + @"\ui.all.css");
        Page.Title = Resources.Resource.PageSettingsExtraTitle;
        
        if (!IsPostBack)
        {
            InitFields();
        }

        GetClientState();
    }

    /// <summary>
    /// Initialization fields
    /// </summary>
    protected override void InitFields()
    {
        InitializeColorOptions();

        if (Roles.IsUserInRole("Administrator"))
        {
            UpdateData();
        }
        else
        {
            tblEvent.Visible = false;
            liEventColor.Visible = false;            
        }
        LoadStateFromDataBase();
    }

    /// <summary>
    /// Update data
    /// </summary>
    private void UpdateData()
    {   
        string filter = "EventName like '%'";
        string sort = "EventName ASC";

        int count = DBProviders.Event.GetEventTypesCount(filter);
        int pageSize = 20;
        int pageCount = (int)Math.Ceiling((double)count / pageSize);

        dlEvents.DataSource = DBProviders.Event.GetEventTypeList(filter, sort, 1, (Int16)pageSize);

        dlEvents.DataBind();
        //LoadStateFromDataBase();
    }

    /// <summary>
    /// Color to link
    /// </summary>
    /// <param name="ddlMultiColor"></param>
    private void colorManipulation(DropDownList ddlMultiColor)
    {
        int row;
        for (row = 0; row < ddlMultiColor.Items.Count - 1; row++)
        {
            ddlMultiColor.Items[row].Attributes.Add("style",
                "background-color:" + ddlMultiColor.Items[row].Value);
        }
        ddlMultiColor.BackColor =
           Color.FromName(ddlMultiColor.SelectedItem.Text);
    }


    /// <summary>
    /// Item command
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void dlEvents_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "SortCommand":
                dlEvents.EditItemIndex = -1;
                UpdateData();
                break;

            case "EditCommand":
                dlEvents.EditItemIndex = e.Item.ItemIndex;
                UpdateData();
                break;

            case "CancelCommand":
                dlEvents.EditItemIndex = -1;
                UpdateData();
                break;

            case "UpdateCommand":

                EventTypesEntity event_ = new EventTypesEntity(Convert.ToInt16((e.Item.FindControl("lblID") as Label).Text),
                        (e.Item.FindControl("lblName") as Label).Text, (e.Item.FindControl("ddlMultiColor") as DropDownList).SelectedValue, false, false, false);

                DBProviders.Event.UpdateColor(event_);
                //Label lbtn = (Label)e.Item.FindControl("lblName");
                //if (lbtn != null)
                //    Anchor.ScrollToObj(lbtn.ClientID, Page);

                dlEvents.EditItemIndex = -1;
                UpdateData();

                break;
        }
    }

    /// <summary>
    /// Item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dlEvents_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //Item
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            (e.Item.FindControl("lbtnNameSel") as LinkButton).Text = ((EventTypesEntity)e.Item.DataItem).EventName;
            Color clr = Color.FromName(((EventTypesEntity)e.Item.DataItem).Color);
            Color clr2 = Color.FromArgb((byte)~clr.R, (byte)~clr.G, (byte)~clr.B);

            (e.Item.FindControl("lbtnColor") as LinkButton).Text = ((EventTypesEntity)e.Item.DataItem).Color;

            (e.Item.FindControl("lbtnColor") as LinkButton).Attributes.Add("style", "color:" + "#" +
               clr2.R.ToString() + clr2.G.ToString() + clr2.B.ToString() + ";" + "background-color:" + clr.Name);
        
            (e.Item.FindControl("lbtnNameSel") as LinkButton).Attributes.Add("style", "color:" +"#"+
                clr2.R.ToString() + clr2.G.ToString() + clr2.B.ToString() + ";" + "background-color:" + clr.Name);
        }

        if (e.Item.ItemType == ListItemType.EditItem)
        {
            (e.Item.FindControl("lbtnUpdate") as LinkButton).Text = Resources.Resource.UpdateButtonText;
            (e.Item.FindControl("lbtnCancel") as LinkButton).Text = Resources.Resource.CancelButtonText;

            ColorGenerator clrs = new ColorGenerator();
            DropDownList ddlList = (e.Item.FindControl("ddlMultiColor") as DropDownList);
            ddlList.DataSource = clrs.GetFinalColorList();
            ddlList.DataBind();
            colorManipulation(ddlList);
        }


    }
    protected void pcPaging_NextPage(object sender, EventArgs e)
    {
        dlEvents.EditItemIndex = -1;
        UpdateData();
        //Anchor.ScrollToObj(pcPaging.ClientID, Page);

    }
    protected void pcPaging_PrevPage(object sender, EventArgs e)
    {
        dlEvents.EditItemIndex = -1;
        UpdateData();
        //Anchor.ScrollToObj(pcPaging.ClientID, Page);
    }

    protected void pcPaging_HomePage(object sender, EventArgs e)
    {
        dlEvents.EditItemIndex = -1;
        UpdateData();
        //Anchor.ScrollToObj(pcPaging.ClientID, Page);
    }

    protected void pcPaging_LastPage(object sender, EventArgs e)
    {
        dlEvents.EditItemIndex = -1;
        UpdateData();
        //Anchor.ScrollToObj(pcPaging.ClientID, Page);
    }

    /// <summary>
    /// Save state
    /// </summary>
    /// <returns></returns>
    private SettingsEntity SaveStateForCompPage()
    {
        SettingsEntity settings = new SettingsEntity();

        if (cboxCPUClock.Checked) settings.ShowCPUClock = true; else settings.ShowCPUClock = false;
        if (cboxLatestInfected.Checked) settings.ShowLatestInfected = true; else settings.ShowLatestInfected = false;
        if (cboxLatestMalware.Checked) settings.ShowLatestMalware = true; else settings.ShowLatestMalware = false;
        if (cboxLatestUpdate.Checked) settings.ShowLatestUpdate = true; else settings.ShowLatestUpdate = false;
        if (cboxOSType.Checked) settings.ShowOSType = true; else settings.ShowOSType = false;
        if (cboxRAM.Checked) settings.ShowRAM = true; else settings.ShowRAM = false;
        if (cboxRecentActive.Checked) settings.ShowRecentActive = true; else settings.ShowRecentActive = false;
        settings.ShowComputerName = true;
        if (cboxShowControlCenter.Checked) settings.ShowControlCenter = true; else settings.ShowControlCenter = false;
        if (cboxShowDomainName.Checked) settings.ShowDomainName = true; else settings.ShowDomainName = false;
        settings.ShowIPAdress = true;
        if (cboxUserLogin.Checked) settings.ShowUserLogin = true; else settings.ShowUserLogin = false;
        if (cboxVba32Integrity.Checked) settings.ShowVba32Integrity = true; else settings.ShowVba32Integrity = false;
        if (cboxVba32KeyValid.Checked) settings.ShowVba32KeyValid = true; else settings.ShowVba32KeyValid = false;
        if (cboxVba32Version.Checked) settings.ShowVba32Version = true; else settings.ShowVba32Version = false;
        if (cboxPolicyName.Checked) settings.ShowPolicyName = true; else settings.ShowPolicyName = false;
        if (cboxDescription.Checked) settings.ShowDescription = true; else settings.ShowDescription = false;


        //Intervals
       /* Validation vld = new Validation(tboxIntervalComponents.Text);
        if (!vld.CheckInterval())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
            + Resources.Resource.Component);
        settings.IntervalCmpts = Convert.ToInt32(tboxIntervalComponents.Text);

        vld.Value = tboxIntervalComputers.Text;
        if (!vld.CheckInterval())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
             + Resources.Resource.Computers);
        settings.IntervalComps = Convert.ToInt32(tboxIntervalComputers.Text);*/

        Validation vld = new Validation("asd");
        vld.Value = tboxIntervalEvents.Text;
        if (!vld.CheckInterval())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
              + Resources.Resource.Events);
        settings.IntervalAutoUpdateEvents = Convert.ToInt32(tboxIntervalEvents.Text);

        /*vld.Value = tboxIntervalProcesses.Text;
        if (!vld.CheckInterval())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
              + Resources.Resource.Processes);
        settings.IntervalProcs = Convert.ToInt32(tboxIntervalProcesses.Text);*/

         vld.Value = tboxIntervalTasks.Text;
        if (!vld.CheckInterval())
            throw new ArgumentException(Resources.Resource.ErrorInvalidValue + ": "
                        + Resources.Resource.Tasks);
        settings.IntervalAutoUpdateTasks = Convert.ToInt32(tboxIntervalTasks.Text);


        //if (cboxIntervalComponents.Checked) settings.EnableAJAXCmpts = true; else settings.EnableAJAXCmpts = false;
        //if (cboxIntervalComputers.Checked) settings.EnableAJAXComps = true; else settings.EnableAJAXComps = false;
        //if (cboxIntervalEvents.Checked) settings.EnableAJAXEvents = true; else settings.EnableAJAXEvents = false;
        //if (cboxIntervalProcesses.Checked) settings.EnableAJAXProcs = true; else settings.EnableAJAXProcs = false;
        //if (cboxIntervalTasks.Checked) settings.EnableAJAXTasks = true; else settings.EnableAJAXTasks = false;




        return settings;

    }

    /// <summary>
    /// Load state
    /// </summary>
    private void LoadStateFromDataBase()
    {
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

        //computers shows        
        cboxShowControlCenter.Checked = settings.ShowControlCenter;
        cboxCPUClock.Checked = settings.ShowCPUClock;
        cboxShowDomainName.Checked = settings.ShowDomainName;        
        cboxLatestInfected.Checked = settings.ShowLatestInfected;
        cboxLatestMalware.Checked = settings.ShowLatestMalware;
        cboxLatestUpdate.Checked = settings.ShowLatestUpdate;
        cboxOSType.Checked = settings.ShowOSType;
        cboxRAM.Checked = settings.ShowRAM;
        cboxRecentActive.Checked = settings.ShowRecentActive;
        cboxUserLogin.Checked = settings.ShowUserLogin;
        cboxVba32Integrity.Checked = settings.ShowVba32Integrity;
        cboxVba32KeyValid.Checked = settings.ShowVba32KeyValid;
        cboxVba32Version.Checked = settings.ShowVba32Version;
        cboxPolicyName.Checked = settings.ShowPolicyName;
        cboxDescription.Checked = settings.ShowDescription;

        //AJAX option's

        tboxIntervalEvents.Text = settings.IntervalAutoUpdateEvents.ToString();
        tboxIntervalTasks.Text = settings.IntervalAutoUpdateTasks.ToString();


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
                return;
            }
            
            Session["ColorOptions"] = options;
            
        }
        else
        {
            options = (ComputerColorOptionsEntity)Session["ColorOptions"];
        }


        ddlColorIntegrity.SelectedIndex = options.IntegrityColor.ColorIndex;
        ddlColorKey.SelectedIndex = options.KeyColor.ColorIndex;
        ddlGoodStateColor.SelectedIndex = options.GoodStateColor.ColorIndex;

        ddlColorLU1.SelectedIndex = options.LastUpdateOption1.ColorIndex;
        ddlColorLU2.SelectedIndex = options.LastUpdateOption2.ColorIndex;
        ddlColorLU3.SelectedIndex = options.LastUpdateOption3.ColorIndex;
        ddlTypeLU1.SelectedIndex = options.LastUpdateOption1.isHour ? 0 : 1;
        if (!options.LastUpdateOption1.isHour) AddItemsTime(ddlTimeLU1, false);
        ddlTypeLU2.SelectedIndex = options.LastUpdateOption2.isHour ? 0 : 1;
        if (!options.LastUpdateOption2.isHour) AddItemsTime(ddlTimeLU2, false);
        ddlTypeLU3.SelectedIndex = options.LastUpdateOption3.isHour ? 0 : 1;
        if (!options.LastUpdateOption3.isHour) AddItemsTime(ddlTimeLU3, false);
        ddlTimeLU1.SelectedIndex = options.LastUpdateOption1.Time - 1;
        ddlTimeLU2.SelectedIndex = options.LastUpdateOption2.Time - 1;
        ddlTimeLU3.SelectedIndex = options.LastUpdateOption3.Time - 1;

        ddlColorLI1.SelectedIndex = options.LastInfectionOption1.ColorIndex;
        ddlColorLI2.SelectedIndex = options.LastInfectionOption2.ColorIndex;
        ddlColorLI3.SelectedIndex = options.LastInfectionOption3.ColorIndex;
        ddlTypeLI1.SelectedIndex = options.LastInfectionOption1.isHour ? 0 : 1;
        if (!options.LastInfectionOption1.isHour) AddItemsTime(ddlTimeLI1, false);
        ddlTypeLI2.SelectedIndex = options.LastInfectionOption2.isHour ? 0 : 1;
        if (!options.LastInfectionOption2.isHour) AddItemsTime(ddlTimeLI2, false);
        ddlTypeLI3.SelectedIndex = options.LastInfectionOption3.isHour ? 0 : 1;
        if (!options.LastInfectionOption3.isHour) AddItemsTime(ddlTimeLI3, false);
        ddlTimeLI1.SelectedIndex = options.LastInfectionOption1.Time - 1;
        ddlTimeLI2.SelectedIndex = options.LastInfectionOption2.Time - 1;
        ddlTimeLI3.SelectedIndex = options.LastInfectionOption3.Time - 1;

        ddlColorLA1.SelectedIndex = options.LastActiveOption1.ColorIndex;
        ddlColorLA2.SelectedIndex = options.LastActiveOption2.ColorIndex;
        ddlColorLA3.SelectedIndex = options.LastActiveOption3.ColorIndex;
        ddlTypeLA1.SelectedIndex = options.LastActiveOption1.isHour ? 0 : 1;
        if (!options.LastActiveOption1.isHour) AddItemsTime(ddlTimeLA1, false);
        ddlTypeLA2.SelectedIndex = options.LastActiveOption2.isHour ? 0 : 1;
        if (!options.LastActiveOption2.isHour) AddItemsTime(ddlTimeLA2, false);
        ddlTypeLA3.SelectedIndex = options.LastActiveOption3.isHour ? 0 : 1;
        if (!options.LastActiveOption3.isHour) AddItemsTime(ddlTimeLA3, false);
        ddlTimeLA1.SelectedIndex = options.LastActiveOption1.Time - 1;
        ddlTimeLA2.SelectedIndex = options.LastActiveOption2.Time - 1;
        ddlTimeLA3.SelectedIndex = options.LastActiveOption3.Time - 1;

        hdnBlockItems.Value = "";
        foreach (EPriorityComputerColor priority in options.Priority)
        {
            switch (priority)
            {
                case EPriorityComputerColor.Integrity:
                    hdnBlockItems.Value += String.Format("{0};", Resources.Resource.Integrity);
                    break;
                case EPriorityComputerColor.Key:
                    hdnBlockItems.Value += String.Format("{0};", Resources.Resource.KeyState);
                    break;
                case EPriorityComputerColor.LastActive:
                    hdnBlockItems.Value += String.Format("{0};", Resources.Resource.LastActive);
                    break;
                case EPriorityComputerColor.LastInfected:
                    hdnBlockItems.Value += String.Format("{0};", Resources.Resource.LastInfected);
                    break;
                case EPriorityComputerColor.LastUpdate:
                    hdnBlockItems.Value += String.Format("{0};", Resources.Resource.LastUpdate);
                    break;
            }
        }
    }

    private void GetClientState()
    {
        lboxPriority.Items.Clear();
        foreach (string str in hdnBlockItems.Value.Split(';'))
        {
            if (str != String.Empty && str != null)
                lboxPriority.Items.Add(str);
        }
    }

    /// <summary>
    /// Save click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SettingsEntity settings = SaveStateForCompPage();
        Profile.Settings = settings.Serialize();

        ComputerColorOptionsEntity options = SaveColorOptions();
        Profile.ComputerColorOptions = options.Serialize();

        Session["Settings"] = settings;
        Session["ColorOptions"] = options;

        ColorOptionsManipulation();
    }

    private ComputerColorOptionsEntity SaveColorOptions()
    {
        GetClientState();
        List<EPriorityComputerColor> priority = new List<EPriorityComputerColor>();
        foreach (ListItem item in lboxPriority.Items)
        {
            if(item.Text == Resources.Resource.LastUpdate)
            {
                priority.Add(EPriorityComputerColor.LastUpdate);
            }
            else
                if(item.Text == Resources.Resource.LastActive)
                {
                    priority.Add(EPriorityComputerColor.LastActive);
                }
                else
                    if(item.Text == Resources.Resource.LastInfected)
                    {
                        priority.Add(EPriorityComputerColor.LastInfected);
                    }
                    else
                        if(item.Text == Resources.Resource.KeyState)
                        {
                            priority.Add(EPriorityComputerColor.Key);
                        }
                        else
                            if(item.Text == Resources.Resource.Integrity)
                            {
                                priority.Add(EPriorityComputerColor.Integrity);
                            }        
        }
        
        return new ComputerColorOptionsEntity(priority,
            ComputerColorOptionsEntity.GetColorOptions(ddlColorLU1.SelectedItem.Text, ddlColorLU1.SelectedIndex, ddlTimeLU1.SelectedIndex + 1, ddlTypeLU1.SelectedIndex == 0 ? true : false),
            ComputerColorOptionsEntity.GetColorOptions(ddlColorLU2.SelectedItem.Text, ddlColorLU2.SelectedIndex, ddlTimeLU2.SelectedIndex + 1, ddlTypeLU2.SelectedIndex == 0 ? true : false),
            ComputerColorOptionsEntity.GetColorOptions(ddlColorLU3.SelectedItem.Text, ddlColorLU3.SelectedIndex, ddlTimeLU3.SelectedIndex + 1, ddlTypeLU3.SelectedIndex == 0 ? true : false),
            ComputerColorOptionsEntity.GetColorOptions(ddlColorLI1.SelectedItem.Text, ddlColorLI1.SelectedIndex, ddlTimeLI1.SelectedIndex + 1, ddlTypeLI1.SelectedIndex == 0 ? true : false),
            ComputerColorOptionsEntity.GetColorOptions(ddlColorLI2.SelectedItem.Text, ddlColorLI2.SelectedIndex, ddlTimeLI2.SelectedIndex + 1, ddlTypeLI2.SelectedIndex == 0 ? true : false),
            ComputerColorOptionsEntity.GetColorOptions(ddlColorLI3.SelectedItem.Text, ddlColorLI3.SelectedIndex, ddlTimeLI3.SelectedIndex + 1, ddlTypeLI3.SelectedIndex == 0 ? true : false),
            ComputerColorOptionsEntity.GetColorOptions(ddlColorLA1.SelectedItem.Text, ddlColorLA1.SelectedIndex, ddlTimeLA1.SelectedIndex + 1, ddlTypeLA1.SelectedIndex == 0 ? true : false),
            ComputerColorOptionsEntity.GetColorOptions(ddlColorLA2.SelectedItem.Text, ddlColorLA2.SelectedIndex, ddlTimeLA2.SelectedIndex + 1, ddlTypeLA2.SelectedIndex == 0 ? true : false),
            ComputerColorOptionsEntity.GetColorOptions(ddlColorLA3.SelectedItem.Text, ddlColorLA3.SelectedIndex, ddlTimeLA3.SelectedIndex + 1, ddlTypeLA3.SelectedIndex == 0 ? true : false),
            ComputerColorOptionsEntity.GetColorOptions(ddlColorKey.SelectedItem.Text, ddlColorKey.SelectedIndex, 0, false),
            ComputerColorOptionsEntity.GetColorOptions(ddlColorIntegrity.SelectedItem.Text, ddlColorIntegrity.SelectedIndex, 0, false),
            ComputerColorOptionsEntity.GetColorOptions(ddlGoodStateColor.SelectedItem.Text, ddlGoodStateColor.SelectedIndex, 0, false)
            );
    }

    private void InitializeColorOptions()
    {
        lboxPriority.Items.Add(Resources.Resource.LastUpdate);
        lboxPriority.Items.Add(Resources.Resource.LastInfected);
        lboxPriority.Items.Add(Resources.Resource.Integrity);
        lboxPriority.Items.Add(Resources.Resource.KeyState);
        lboxPriority.Items.Add(Resources.Resource.LastActive);

        hdnBlockItems.Value = "";
        foreach (ListItem item in lboxPriority.Items)
        {
            hdnBlockItems.Value += String.Format("{0};", item.Text);
        }

        ColorGenerator clrs = new ColorGenerator();
        ddlColorKey.DataSource = clrs.GetFinalColorList();
        ddlColorIntegrity.DataSource = clrs.GetFinalColorList();
        ddlGoodStateColor.DataSource = clrs.GetFinalColorList();
        ddlColorLA1.DataSource = clrs.GetFinalColorList();
        ddlColorLA2.DataSource = clrs.GetFinalColorList();
        ddlColorLA3.DataSource = clrs.GetFinalColorList();
        ddlColorLI1.DataSource = clrs.GetFinalColorList();
        ddlColorLI2.DataSource = clrs.GetFinalColorList();
        ddlColorLI3.DataSource = clrs.GetFinalColorList();
        ddlColorLU1.DataSource = clrs.GetFinalColorList();
        ddlColorLU2.DataSource = clrs.GetFinalColorList();
        ddlColorLU3.DataSource = clrs.GetFinalColorList();

        ddlColorKey.DataBind();
        ddlColorIntegrity.DataBind();
        ddlGoodStateColor.DataBind();
        ddlColorLA1.DataBind();
        ddlColorLA2.DataBind();
        ddlColorLA3.DataBind();
        ddlColorLI1.DataBind();
        ddlColorLI2.DataBind();
        ddlColorLI3.DataBind();
        ddlColorLU1.DataBind();
        ddlColorLU2.DataBind();
        ddlColorLU3.DataBind();

        ColorOptionsManipulation();

        AddItemsType(ddlTypeLA1);
        AddItemsType(ddlTypeLA2);
        AddItemsType(ddlTypeLA3);
        AddItemsType(ddlTypeLI1);
        AddItemsType(ddlTypeLI2);
        AddItemsType(ddlTypeLI3);
        AddItemsType(ddlTypeLU1);
        AddItemsType(ddlTypeLU2);
        AddItemsType(ddlTypeLU3);

        AddItemsTime(ddlTimeLA1, true);
        AddItemsTime(ddlTimeLA2, true);
        AddItemsTime(ddlTimeLA3, true);
        AddItemsTime(ddlTimeLI1, true);
        AddItemsTime(ddlTimeLI2, true);
        AddItemsTime(ddlTimeLI3, true);
        AddItemsTime(ddlTimeLU1, true);
        AddItemsTime(ddlTimeLU2, true);
        AddItemsTime(ddlTimeLU3, true);
    }

    private void AddItemsType(DropDownList ddl)
    {
        if (ddl.Items.Count == 0)
        {
            ddl.Items.Add(Resources.Resource.Hours);
            ddl.Items.Add(Resources.Resource.Days2);

            ddl.SelectedIndex = 0;
        }
    }

    private void AddItemsTime(DropDownList ddl, bool isHour)
    {
        if (ddl == null) return;
        ddl.Items.Clear();
        if (isHour)
        {
            for (int i = 1; i < 24; i++)
                ddl.Items.Add(i.ToString());
        }
        else
        {
            for (int i = 1; i < 29; i++)
                ddl.Items.Add(i.ToString());
        }

        ddl.SelectedIndex = 0;
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string ddlID = ((DropDownList)sender).ID.Replace("Type", "Time");
        string pnlID = ddlID.Substring(8, 1);
        Panel pnl = new Panel();
        switch (pnlID[0])
        {
            case 'U':
                pnl = pnlLastUpdate;                
                break;
            case 'A':
                pnl = pnlLastActive;
                break;
            case 'I':
                pnl = pnlLastInfection;
                break;
        }
        if (((DropDownList)sender).SelectedIndex == 0)
            AddItemsTime(pnl.FindControl(ddlID) as DropDownList, true);
        else AddItemsTime(pnl.FindControl(ddlID) as DropDownList, false);

        ColorOptionsManipulation();

        pnlLastActive.Style["display"] = "none";
        pnlLastInfection.Style["display"] = "none";
        pnlLastUpdate.Style["display"] = "none";
        pnl.Style["display"] = "";
    }

    private void ColorOptionsManipulation()
    {
        colorManipulation(ddlColorKey);
        colorManipulation(ddlColorIntegrity);
        colorManipulation(ddlGoodStateColor);
        colorManipulation(ddlColorLA1);
        colorManipulation(ddlColorLA2);
        colorManipulation(ddlColorLA3);
        colorManipulation(ddlColorLI1);
        colorManipulation(ddlColorLI2);
        colorManipulation(ddlColorLI3);
        colorManipulation(ddlColorLU1);
        colorManipulation(ddlColorLU2);
        colorManipulation(ddlColorLU3);
    }

}
