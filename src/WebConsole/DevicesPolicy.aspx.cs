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
using System.Web.Services;

using ARM2_dbcontrol.DataBase;
using ARM2_dbcontrol.Filters;
using VirusBlokAda.Vba32CC.Policies;
using VirusBlokAda.Vba32CC.Policies.Devices;
using VirusBlokAda.Vba32CC.Policies.Devices.Policy;
using System.Globalization;
using ARM2_dbcontrol.Service.TaskAssignment;
using ARM2_dbcontrol.Tasks;
using System.Text;
using ARM2_dbcontrol.Common;

public partial class DevicesPolicy : PageBase
{
    private static string OrderByName;
    private static bool OrderByDirection;
    private static string OrderByNameDevice;
    private static bool OrderByDirectionDevice;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = Resources.Resource.DeviceManagment;
        if (!Roles.IsUserInRole("Administrator"))
        {
            Response.Redirect("Default.aspx");
        }

        RegisterHeader();
        if(!Page.IsPostBack)
            InitFields();
    }

    private void RegisterHeader()
    {
        RegisterScript(@"js/jQuery/jquery.cookie.js");
        RegisterScript(@"js/jQuery/jquery.contextMenu.js");
        RegisterScript(@"js/json2.js");
        RegisterScript(@"js/DevicesPolicy.js");
    }

    protected override void InitFields()
    {
        OrderByDirection = false;
        OrderByName = "LatestInsert";

        OrderByDirectionDevice = true;
        OrderByNameDevice = "SerialNo";

        InitPaging();
        InitFilterPanel();
        InitFilterPanelDevice();
        InitFilterPanelUDevice();

        UpdateDataComputers();
        UpdateDataDevices();
        UpdateDataDevicesUnknown();

        if (!KeyFileSettings.IsKeyUSBExist)
        {
            divContent.Attributes.CssStyle.Add(HtmlTextWriterStyle.Display, "none");
            divMessage.InnerHtml = Resources.Resource.MessageBuyAdditionalFeatures;
            divMessage.Attributes.CssStyle[HtmlTextWriterStyle.Display] = "";
        }
    }
    
    private void InitPaging()
    {
        LocalizePaging(pcPaging);
        LocalizePaging(pcPagingTop);
        LocalizePaging(PagingControl1);
        LocalizePaging(PagingControl2);
        LocalizePaging(PagingControl3);
        LocalizePaging(PagingControl4);
    }

    private void LocalizePaging(PagingControls.PagingControl control)
    {
        control.CurrentPageIndex = 1;
        control.PageCount = 1;
        control.PageText = Resources.Resource.Page;
        control.OfText = Resources.Resource.Of;
        control.NextText = Resources.Resource.Next;
        control.PrevText = Resources.Resource.Prev;

        control.HomeText = Resources.Resource.HomePaging;
        control.LastText = Resources.Resource.LastPaging;
    }

    private void UpdateDataComputers()
    {

        using (VlslVConnection conn = new VlslVConnection(
            ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            ComputersManager cmng = new ComputersManager(conn);

            conn.OpenConnection();
            conn.CheckConnectionState(true);

            CompFilterEntity filter = new CompFilterEntity(); ;

            List<ComputersEntity> computers = new List<ComputersEntity>();
            if (Session["CurrentCompFilterDevice"] == null)
            {
                filter = new CompFilterEntity();
                filter.ComputerName = "*";
                filter.GenerateSQLWhereStatement();
            }
            else
            {
                filter = (CompFilterEntity)Session["CurrentCompFilterDevice"];
                LoadFilter(filter);
            }

            int count;
            try
            {
                count = cmng.Count(filter.GetSQLWhereStatement);
            }
            catch
            {
                Response.Redirect("ErrorSql.aspx");
                return;
            }
            int pageSize = 20;
            int pageCount = (int)Math.Ceiling((double)count / pageSize);

            pcPaging.PageCount = pageCount;
            pcPagingTop.PageCount = pageCount;

            computers = cmng.List(filter.GetSQLWhereStatement,
                         "ComputerName ASC",
                         pcPaging.CurrentPageIndex, pageSize);

            DataList1.DataSource = computers;
            DataList1.DataBind();

            conn.CloseConnection();
        }

    }

    private void UpdateDataDevices()
    {
        DeviceFilterEntity filter = new DeviceFilterEntity();
        if (Session["CurrentDeviceFilter"] == null)
        {
            filter = new DeviceFilterEntity();
            filter.SerialNumber = "*";
            filter.GenerateSQLWhereStatement();
        }
        else
        {
            filter = (DeviceFilterEntity)Session["CurrentDeviceFilter"];
        }
        //database query would be better..

        //int count = PoliciesState.GetDevicesCount("SerialNo like '%'");
        int count;
        try
        {
            count = PoliciesState.GetDevicesCount(filter.GetSQLWhereStatement);
        }
        catch
        {
            Response.Redirect("ErrorSql.aspx");
            return;
        }
        int pageSize = 20;
        int pageCount = (int)Math.Ceiling((double)count / pageSize);

        PagingControl1.PageCount = pageCount;
        PagingControl2.PageCount = pageCount;

        try
        {
            DataList2.DataSource = PrepareDevicesForHtml(
                PoliciesState.GetDevicesList(PagingControl1.CurrentPageIndex, pageSize, filter.GetSQLWhereStatement, OrderByNameDevice + " " + (OrderByDirectionDevice == true ? "ASC" : "DESC")), filter);
        }
        catch
        {
            Response.Redirect("ErrorSql.aspx");
            return;
        }
        DataList2.DataBind();
    }

    private List<Device> PrepareDevicesForHtml(List<Device> sources, DeviceFilterEntity filter)
    {
        List<Device> destinations = new List<Device>();
        
        foreach (Device device in sources)
        {
            Device d = new Device(device.ID,
                device.SerialNo, device.Type, device.Comment, device.LastInserted, device.LastComputer);
            if (String.IsNullOrEmpty(device.Comment))
                d.Comment = Anchor.GetCommentFromSerial(device.SerialNo);
                        
            //Filter
            //if (IsConformFilter(d, filter))
            destinations.Add(d);
        }
        return destinations;
    }

    //private bool IsConformFilter(Device device, DeviceFilterEntity filter)
    //{
    //    if (filter == null) return true;
    //    string comment = device.Comment.ToLower();
    //    string[] array = filter.NameDevice.Replace("*", "").Split('&');
        
    //    if (filter.TermNameDevice == "AND")
    //    {
    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            if (comment.Contains(array[i].ToLower())) return true;
    //        }
    //        return false;
    //    }

    //    if (filter.TermNameDevice == "NOT")
    //    {
    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            if (comment.Contains(array[i].ToLower())) return false;                
    //        }
    //        return true;
    //    }

    //    return true;
    //}

    private List<DevicePolicy> PrepareDevicePolicyForHtml(List<DevicePolicy> sources)
    {
        List<DevicePolicy> destinations = new List<DevicePolicy>();

        foreach (DevicePolicy policy in sources)
        {
            Device d = new Device(policy.Device.ID,
                policy.Device.SerialNo, policy.Device.Type, policy.Device.Comment);
            if (String.IsNullOrEmpty(policy.Device.Comment))
                d.Comment = Anchor.GetCommentFromSerial(policy.Device.SerialNo);

            DevicePolicy devicePolicy = policy;
            devicePolicy.Device = d;
            destinations.Add(devicePolicy);
        }
        return destinations;
    }

    
    private void UpdateDataDevicesUnknown()
    {
        UnknownDevicesFilterEntity filter = new UnknownDevicesFilterEntity();
        if (Session["CurrentUDeviceFilter"] == null)
        {
            filter = new UnknownDevicesFilterEntity();
            filter.SerialNumber = "*";
            filter.GenerateSQLWhereStatement();
        }
        else
        {
            filter = (UnknownDevicesFilterEntity)Session["CurrentUDeviceFilter"];
        }

        int count;
        try
        {
            count = PoliciesState.GetUnknownDevicesPolicyPageCount(filter.GetSQLWhereStatement);
        }
        catch
        {
            Response.Redirect("ErrorSql.aspx");
            return;
        }
        int pageSize = 20;
        int pageCount = (int)Math.Ceiling((double)count / pageSize);

        PagingControl3.PageCount = pageCount;
        PagingControl4.PageCount = pageCount;

        try
        {
            DataList3.DataSource = PrepareDevicePolicyForHtml(
                PoliciesState.GetUnknownDevicesPolicyPage(PagingControl3.CurrentPageIndex, pageSize, filter.GetSQLWhereStatement, OrderByName + " " + (OrderByDirection == true ? "ASC" : "DESC")));
        }
        catch
        {
            Response.Redirect("ErrorSql.aspx");
            return;
        }
        DataList3.DataBind();
    }


    public static PolicyProvider PoliciesState
    {
        get
        {
            PolicyProvider provider = HttpContext.Current.Application["PoliciesState"] as PolicyProvider;
            if (provider == null)
            {
                provider = new PolicyProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Application["PoliciesState"] = provider;
            }

            return provider;
        }
    }

    [WebMethod]
    public static /*List<DevicePolicy>*/ string GetComputersData(int id)
    {
        System.Diagnostics.Debug.Write("GetComputersData:" + id);

        //return PoliciesState.GetDevicesPoliciesByComputer(id);

        return ConvertComputerDataForClient(id,
            PoliciesState.GetDevicesPoliciesByComputer(id)); ;
    }

    private static string ConvertComputerDataForClient(int id,List<DevicePolicy> list)
    {
        string table = "<table style='width:100%' class='ListContrastTable' dp=" + id + "><thead><th>"+
        ResourceControl.GetStringForCurrentCulture("SerialNo") + "</th><th>" +
        ResourceControl.GetStringForCurrentCulture("State") + "</th><th>" +
        ResourceControl.GetStringForCurrentCulture("Comment") +
        "</th><th>" + ResourceControl.GetStringForCurrentCulture("DeviceInsertedDate")+
        "</th><th></th></thead><tbody>";
        foreach (DevicePolicy dp in list)
        {
                string row = "<tr style='text-align:center'><td>" + Anchor.FixString(dp.Device.SerialNo, 30) + "</td>";
                string select = "<select dp=" + dp.ID + ">";
                for (int i = 0; i <= 2; i++) {
                    string option = "<option";
                    if (i == (int)dp.State)
                        option += " selected=selected";
                    option += " value=";
                    switch (i) {
                        case 0:
                            option += "Undefined>" + ResourceControl.GetStringForCurrentCulture("Undefined");
                            break;
                        case 1:
                            option += "Enabled>" + ResourceControl.GetStringForCurrentCulture("Enabled");
                            break;
                        case 2:
                            option += "Disabled>" + ResourceControl.GetStringForCurrentCulture("DisabledDevice");
                            break;
                    }
                    select += option + "</option>";
                }
                select += "</select>";
                row += "<td>" + select + "</td>";
            string comment = dp.Device.Comment;
                if (String.IsNullOrEmpty(comment))
                    comment = Anchor.GetCommentFromSerial(dp.Device.SerialNo);

                row += "<td>" + comment + "</td>";
            //row += "<td>" + (dp.LatestInsert==DateTime.MinValue?"":dp.LatestInsert.ToString()) + "</td>";
                row += "<td style='width:60px'>" + dp.LatestInsert??"" + "</td>";
                row += "<td>" + "<img src='"+HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + HttpContext.Current.Profile.GetPropertyValue("Theme").ToString()
                    + "/images/disabled.gif"+"' dp=" + 
                    dp.ID + " style='cursor:pointer'></img></td>";
                row += "</tr>";
                table += row;
        }
        table += "</table>";

        if (list.Count == 0)
            table = "";

        string promt = String.Format("{0}<br />", ResourceControl.GetStringForCurrentCulture("PromtForInputSerialNumber"));
        string text = "<input type=text dpc=" + id + " style='width:400px'></input>";
        string button = "<button dpc=" + id + ">"+ResourceControl.GetStringForCurrentCulture("Add")+"</button>";

        return table + promt + text + button;
    }

    [WebMethod]
    public static /*List<DevicePolicy>*/ string GetDevicesData(int id)
    {
        System.Diagnostics.Debug.Write("GetDevicesData:" + id);

        //return PoliciesState.GetDevicesPoliciesByComputer(id);

        Device device = new Device();
        device.ID = id;

        return ConvertDeviceDataForClient(id,
            PoliciesState.GetPoliciesByDevice(device) ); 
    }

    private static string ConvertDeviceDataForClient(int id, List<DevicePolicy> list)
    {
        string table = "<table style='width:100%' class='ListContrastTable' dp=" + id + "><thead><th>" +
        ResourceControl.GetStringForCurrentCulture("ComputerName") + 
        "</th><th>" + ResourceControl.GetStringForCurrentCulture("State") +
        "</th><th>" + ResourceControl.GetStringForCurrentCulture("DeviceInsertedDate") +
        "</th><th></th></thead><tbody>";
        foreach (DevicePolicy dp in list)
        {
            string row = "<tr style='text-align:center'><td>" + dp.Computer.ComputerName + "</td>";
            string select = "<select dp=" + dp.ID + ">";
            for (int i = 0; i <= 2; i++)
            {
                string option = "<option";
                if (i == (int)dp.State)
                    option += " selected=selected";
                option += " value=";
                switch (i)
                {
                    case 0:
                        option += "Undefined>" + ResourceControl.GetStringForCurrentCulture("Undefined");
                        break;
                    case 1:
                        option += "Enabled>" + ResourceControl.GetStringForCurrentCulture("Enabled");
                        break;
                    case 2:
                        option += "Disabled>" + ResourceControl.GetStringForCurrentCulture("DisabledDevice");
                        break;
                }
                select += option + "</option>";
            }
            select += "</select>";
            row += "<td>" + select + "</td>";
            row += "<td>" + dp.LatestInsert??"" + "</td>";

            row += "<td>" + "<img src='" + HttpContext.Current.Request.ApplicationPath + "/App_Themes/" + HttpContext.Current.Profile.GetPropertyValue("Theme").ToString()
                + "/images/disabled.gif" + "' dp=" +
                dp.ID + " style='cursor:pointer'></img></td>";
            row += "</tr>";
            table += row;
        }
        table += "</table>";

        if (list.Count == 0)
            table = "";

        string promt = String.Format("{0}<br />", ResourceControl.GetStringForCurrentCulture("PromtForInputComputerName"));
        string text = "<input type=text ddpc=" + id + " style='width:400px'></input>";
        string button = "<button ddpc=" + id + ">" + ResourceControl.GetStringForCurrentCulture("Add") + "</button>";

        return table + promt + text + button;
    }

    [WebMethod]
    public static void ChangeDevicePolicyState(int id, string state)
    {
        System.Diagnostics.Debug.Write("ChangeDevicePolicyState with id:" + id + ", state:" + state);

        DevicePolicy dp = PoliciesState.GetDevicePolicyByID(id);
        dp.State = (DevicePolicyState)Enum.Parse(typeof(DevicePolicyState),state);
        PoliciesState.ChangeDevicePolicyStatusForComputer(dp);
    }

    [WebMethod]
    public static void AddNewDevicePolicy(int id, string serial)
    {
        serial = serial.Replace(" ", "");
        System.Diagnostics.Debug.Write("AddNewDevicePolicy id:" + id + ", serial:" + serial);
        try
        {

            if (serial == null || serial == String.Empty) return;
            
            Device device = new Device(serial, DeviceType.USB);
            device = PoliciesState.AddDevice(device);

            ComputersEntity computer = PoliciesState.GetComputerByID(id);

            List<DevicePolicy> dpl =
                PoliciesState.GetDevicesPoliciesByComputer(computer.ComputerName);
            foreach (DevicePolicy tdp in dpl)
            {
                if (tdp.Device.SerialNo == serial) return;
            }
            

            DevicePolicy dp = new DevicePolicy(device, computer);
            PoliciesState.AddDevicePolicy(dp);
        }
        
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Write(ex);
        }
    }

    [WebMethod]
    public static void RemoveDevicePolicy(int id)
    {
        PoliciesState.DeleteDevicePolicyByID(id);
    }

    [WebMethod]
    public static void AddNewComputerPolicy(int id, string name)
    {
        System.Diagnostics.Debug.Write("AddNewComputerPolicy id:" + id + ", name:" + name);

        Device device = new Device();
        device.ID = id;

        ComputersEntity computer = new ComputersEntity();
        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            ComputersManager cmng = new ComputersManager(conn);

            conn.OpenConnection();
            conn.CheckConnectionState(true);

            computer.ID = cmng.GetComputerID(name);
        }
        if (computer.ID == -1)
            throw new Exception("Computer not exist");

        foreach (DevicePolicy item in PoliciesState.GetDevicesPoliciesByComputer(computer.ID))
        {
            if (item.Device.ID == id)
                throw new Exception("Device policy already exist");
        }

        DevicePolicy dp = new DevicePolicy(device, computer);
        PoliciesState.AddDevicePolicy(dp);

    }

    [WebMethod]
    public static void DeleteDevice(int id)
    {
        Device device = new Device();
        device.ID = id;
        PoliciesState.DeleteDevice(device);
    }

    [WebMethod]
    public static string GetChangeCommentDialog(int id)
    {
        Device device = PoliciesState.GetDevice(id);
        if (String.IsNullOrEmpty(device.Comment))
            device.Comment = Anchor.GetCommentFromSerial(device.SerialNo);
        string label = "<div>Comment for device</div>";
        string text = "<input type=text dcdpc=" + id + " style='width:400px' value='" + device.Comment + "'></input>";
        string button = "<button dcdpc=" + id + ">Change</button>";

        return label + text + button;
    }

    [WebMethod]
    public static void ChangeComment(int id, string comment)
    {
        Device device = new Device();
        device.ID = id;
        device.Comment = comment;

        PoliciesState.EditDevice(device);
    }

    [WebMethod]
    public static void ActionDevice(int id, string action)
    {
        DevicePolicy dp = PoliciesState.GetDevicePolicyByID(id);
        if (action == "allow")
            dp.State = DevicePolicyState.Enabled;
        else
            dp.State = DevicePolicyState.Disabled;

        PoliciesState.ChangeDevicePolicyStatusForComputer(dp);
    }

    [WebMethod]
    public static void ActionDeviceAll(string action)
    {
        List<DevicePolicy> list = PoliciesState.GetUnknownDevicesPolicyPage(1, PoliciesState.GetUnknownDevicesPolicyPageCount(""), "", "SerialNo ASC");
        foreach (DevicePolicy device in list)
        {
            ActionDevice(device.ID, action);
        }
    }

    [WebMethod]
    public static string ApplyPolicy(int id)
    {
        string[] ipAddr = new string[1];
        string[] compName = new string[1];
        string connStr = ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString;
        string service = ConfigurationManager.AppSettings["Service"];

        Int64[] taskId = new Int64[1];        

        using (VlslVConnection conn = new VlslVConnection(connStr))
        {
            ComputersManager cmng = new ComputersManager(conn);

            conn.OpenConnection();
            conn.CheckConnectionState(true);

            ComputersEntity comp = cmng.GetComputer(id);

            conn.CloseConnection();

            if (comp != null)
            {
                ipAddr[0] = comp.IPAddress;
                compName[0] = comp.ComputerName;
            }
        }        

        Vba32ControlCenterWrapper control = new Vba32ControlCenterWrapper(service);

        TaskUserEntity task = new TaskUserEntity();
        string param = @"<RequestPolicy></RequestPolicy>";
        try
        {
            string taskName = Resources.Resource.TaskRequestPolicy;
            taskId[0] = PreServAction.CreateTask(compName[0], taskName, BuildParam(param), Anchor.GetStringForTaskGivedUser(), connStr);
           
            control.PacketCustomAction(taskId, ipAddr, param);
        }
        catch
        {
            return ConvertStatusTask(Resources.Resource.TaskStateExecutionError);
        }

        return ConvertStatusTask(Resources.Resource.TaskGived);
    }

    private static string BuildParam(string taskParam)
    {
        StringBuilder param = new StringBuilder();

        param.Append("<Task>");
        param.Append(taskParam);
        param.AppendFormat("<Vba32CCUser>{0}</Vba32CCUser>", Anchor.GetStringForTaskGivedUser());
        param.Append("<Type>RequestPolicy</Type>");
        param.Append("</Task>");

        return param.ToString();
    }

    private static string ConvertStatusTask(string status)
    {
        return "<div style='text-align:center;font-size:15px;font-weight:bold;padding-top:30px'>" + status + "</div>";
    }

    #region Paging

    protected void pcPaging_NextPage(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        pcPaging.CurrentPageIndex = index;
        pcPagingTop.CurrentPageIndex = index;
        UpdateDataComputers();
    }

    protected void pcPaging_PrevPage(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        pcPaging.CurrentPageIndex = index;
        pcPagingTop.CurrentPageIndex = index;
        UpdateDataComputers();
    }

    protected void pcPaging_HomePage(object sender, EventArgs e)
    {
        pcPaging.CurrentPageIndex = 1;
        pcPagingTop.CurrentPageIndex = 1;

        //DataList1.EditItemIndex = -1;
        UpdateDataComputers();
    }
    protected void pcPaging_LastPage(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).PageCount;
        pcPaging.CurrentPageIndex = index;
        pcPagingTop.CurrentPageIndex = index;

        //DataList1.EditItemIndex = -1;
        UpdateDataComputers();
    }


    protected void pcPaging_NextPage2(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        PagingControl1.CurrentPageIndex = index;
        PagingControl2.CurrentPageIndex = index;
        UpdateDataDevices();
    }

    protected void pcPaging_PrevPage2(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        PagingControl1.CurrentPageIndex = index;
        PagingControl2.CurrentPageIndex = index;
        UpdateDataDevices();
    }

    protected void pcPaging_HomePage2(object sender, EventArgs e)
    {
        PagingControl1.CurrentPageIndex = 1;
        PagingControl2.CurrentPageIndex = 1;

        //DataList1.EditItemIndex = -1;
        UpdateDataDevices();
    }
    protected void pcPaging_LastPage2(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).PageCount;
        PagingControl1.CurrentPageIndex = index;
        PagingControl2.CurrentPageIndex = index;

        //DataList1.EditItemIndex = -1;
        UpdateDataDevices();
    }


    protected void pcPaging_NextPage3(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        PagingControl3.CurrentPageIndex = index;
        PagingControl4.CurrentPageIndex = index;
        UpdateDataDevicesUnknown();
    }

    protected void pcPaging_PrevPage3(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).CurrentPageIndex;
        PagingControl3.CurrentPageIndex = index;
        PagingControl4.CurrentPageIndex = index;
        UpdateDataDevicesUnknown();
    }

    protected void pcPaging_HomePage3(object sender, EventArgs e)
    {
        PagingControl3.CurrentPageIndex = 1;
        PagingControl4.CurrentPageIndex = 1;

        //DataList1.EditItemIndex = -1;
        UpdateDataDevicesUnknown();
    }
    protected void pcPaging_LastPage3(object sender, EventArgs e)
    {
        int index = ((PagingControls.PagingControl)sender).PageCount;
        PagingControl3.CurrentPageIndex = index;
        PagingControl4.CurrentPageIndex = index;

        //DataList1.EditItemIndex = -1;
        UpdateDataDevicesUnknown();
    }

    #endregion

    #region Filter
    private void InitFilterPanel()
    {
        cboxComputerName.Text = Resources.Resource.ComputerName;
        cboxIPAddress.Text = Resources.Resource.IPAddress;
        cboxUserLogin.Text = Resources.Resource.UserLogin;

        List<string> terms = new List<string>();
        terms.Add("AND");
        terms.Add("OR");
        terms.Add("NOT");

        try
        {
            ddlTermComputerName.DataSource = terms;
            ddlTermComputerName.DataBind();
        }
        catch
        {
            ddlTermComputerName.SelectedIndex = -1;
            ddlTermComputerName.SelectedValue = null;
            ddlTermComputerName.DataSource = terms;
            ddlTermComputerName.DataBind();
        }

        try
        {
            ddlTermIPAddress.DataSource = terms;
            ddlTermIPAddress.DataBind();
        }
        catch
        {
            ddlTermIPAddress.SelectedIndex = -1;
            ddlTermIPAddress.SelectedValue = null;
            ddlTermIPAddress.DataSource = terms;
            ddlTermIPAddress.DataBind();
        }

        try
        {
            ddlTermUserLogin.DataSource = terms;
            ddlTermUserLogin.DataBind();
        }
        catch
        {
            ddlTermUserLogin.SelectedIndex = -1;
            ddlTermUserLogin.SelectedValue = null;
            ddlTermUserLogin.DataSource = terms;
            ddlTermUserLogin.DataBind();
        }
    }

    public void LoadFilter(CompFilterEntity filter)
    {
        if (filter.ComputerName != String.Empty)
        {
            cboxComputerName.Checked = true;
            tboxComputerName.Text = filter.ComputerName;
            ddlTermComputerName.SelectedValue = filter.TermComputerName;
        }

        if (filter.IPAddress != String.Empty)
        {
            cboxIPAddress.Checked = true;
            tboxIPAddress.Text = filter.IPAddress;
            ddlTermIPAddress.SelectedValue = filter.TermIPAddress;
        }

        if (filter.UserLogin != String.Empty)
        {
            cboxUserLogin.Checked = true;
            tboxUserlogin.Text = filter.UserLogin;
            ddlTermUserLogin.SelectedValue = filter.TermUserLogin;
        }
    }

    public void GetCurrentStateFilter(ref CompFilterEntity fltr)
    {
        if (cboxComputerName.Checked)
        {
            fltr.ComputerName = tboxComputerName.Text;
            fltr.TermComputerName = ddlTermComputerName.SelectedValue;
        }

        if (cboxIPAddress.Checked)
        {
            fltr.IPAddress = tboxIPAddress.Text;
            fltr.TermIPAddress = ddlTermIPAddress.SelectedValue;
        }

        if (cboxUserLogin.Checked)
        {
            fltr.UserLogin = tboxUserlogin.Text;
            fltr.TermUserLogin = ddlTermUserLogin.SelectedValue;
        }
    }

    public void ClearFilter()
    {
        cboxComputerName.Checked = false;
        cboxIPAddress.Checked = false;
        cboxUserLogin.Checked = false;

        tboxComputerName.Text = String.Empty;
        tboxIPAddress.Text = String.Empty;
        tboxUserlogin.Text = String.Empty;

        ddlTermComputerName.SelectedIndex = 0;
        ddlTermIPAddress.SelectedIndex = 0;
        ddlTermUserLogin.SelectedIndex = 0;
    }

    protected void lbtnApplyFilter_Click(object sender, EventArgs e)
    {
        CompFilterEntity filter = new CompFilterEntity();

        GetCurrentStateFilter(ref filter);

        filter.CheckFilters();
        filter.GenerateSQLWhereStatement();

        pcPaging.CurrentPageIndex = 1;
        pcPagingTop.CurrentPageIndex = 1;

        if (filter.GetSQLWhereStatement != String.Empty)
            Session["CurrentCompFilterDevice"] = filter;
        else
            Session["CurrentCompFilterDevice"] = null;

        UpdateDataComputers();
    }

    protected void lbtnCancelFilter_Click(object sender, EventArgs e)
    {
        Session["CurrentCompFilterDevice"] = null;
        ClearFilter();
        UpdateDataComputers();
    }

    //Filter Devices
    private void InitFilterPanelDevice()
    {
        cboxSerialNumber.Text = Resources.Resource.SerialNo;
        cboxCommentFilter.Text = Resources.Resource.Comment;
        cboxComputerFilter.Text = Resources.Resource.ComputerName;        
        cboxLastInsertFilter.Text = Resources.Resource.DeviceInsertedDate;

        List<string> terms = new List<string>();
        terms.Add("AND");
        terms.Add("OR");
        terms.Add("NOT");

        try
        {
            ddlTermSerialNumber.DataSource = terms;
            ddlTermSerialNumber.DataBind();
        }
        catch
        {
            ddlTermSerialNumber.SelectedIndex = -1;
            ddlTermSerialNumber.SelectedValue = null;
            ddlTermSerialNumber.DataSource = terms;
            ddlTermSerialNumber.DataBind();
        }

        try
        {
            ddlTermCommentFilter.DataSource = terms;
            ddlTermCommentFilter.DataBind();
        }
        catch
        {
            ddlTermCommentFilter.SelectedIndex = -1;
            ddlTermCommentFilter.SelectedValue = null;
            ddlTermCommentFilter.DataSource = terms;
            ddlTermCommentFilter.DataBind();
        }

        try
        {
            ddlTermComputerNameFilter.DataSource = terms;
            ddlTermComputerNameFilter.DataBind();
        }
        catch
        {
            ddlTermComputerNameFilter.SelectedIndex = -1;
            ddlTermComputerNameFilter.SelectedValue = null;
            ddlTermComputerNameFilter.DataSource = terms;
            ddlTermComputerNameFilter.DataBind();
        }

        try
        {
            ddlTermLastInsertFilter.DataSource = terms;
            ddlTermLastInsertFilter.DataBind();
        }
        catch
        {
            ddlTermLastInsertFilter.SelectedIndex = -1;
            ddlTermLastInsertFilter.SelectedValue = null;
            ddlTermLastInsertFilter.DataSource = terms;
            ddlTermLastInsertFilter.DataBind();
        }

        if (dccDateInsertedFilter.FromMonthObject != null) return;

        List<string> monthList = new List<string>();
        foreach (string month in CultureInfo.CurrentCulture.DateTimeFormat.MonthNames)
            if (month != String.Empty)
                monthList.Add(month);

        dccDateInsertedFilter.FromMonthObject = monthList;
        dccDateInsertedFilter.ToMonthObject = monthList;

        List<string> dayList = new List<string>();
        for (int i = 1; i <= 31; i++)
            dayList.Add(i.ToString());

        dccDateInsertedFilter.FromDayObject = dayList;
        dccDateInsertedFilter.ToDayObject = dayList;

        List<string> yearList = new List<string>();
        for (int i = 1; i > -2; i--)
            yearList.Add(Convert.ToString(DateTime.Now.Year - i));

        dccDateInsertedFilter.FromYearObject = yearList;
        dccDateInsertedFilter.ToYearObject = yearList;

        List<string> hourList = new List<string>();
        for (int i = 0; i < 24; i++)
            hourList.Add(i.ToString());

        dccDateInsertedFilter.FromHourObject = hourList;
        dccDateInsertedFilter.ToHourObject = hourList;

        List<string> minuteList = new List<string>();
        for (int i = 0; i < 60; i++)
            minuteList.Add(i.ToString());

        dccDateInsertedFilter.FromMinuteObject = minuteList;
        dccDateInsertedFilter.ToMinuteObject = minuteList;

        dccDateInsertedFilter.SetFromText = Resources.Resource.From;
        dccDateInsertedFilter.SetToText = Resources.Resource.To;

        DateTime dt = DateTime.Now.AddMonths(-1);
        string lyear = ((int)(DateTime.Now.Year - 1)).ToString();
        dccDateInsertedFilter.SetDateFrom(dt, lyear);
        dccDateInsertedFilter.SetDateTo(DateTime.Now, lyear);

        List<string> list = Anchor.GetDateIntervals();
        dccDateInsertedFilter.Interval = list;

        List<string> intervalTypes = new List<string>();
        intervalTypes.Add(Resources.Resource.HighDate);
        intervalTypes.Add(Resources.Resource.LowDate);

        dccDateInsertedFilter.IntervalMode = intervalTypes;
    }

    public void LoadFilterDevice(DeviceFilterEntity filter)
    {
        InitFilterPanelDevice();
        string startYear = Convert.ToString(DateTime.Now.Year - 1);

        if ((filter.DateInsertedFrom != DateTime.MinValue) ||
            (filter.DateInsertedTo != DateTime.MinValue))
        {
            cboxLastInsertFilter.Checked = true;
            dccDateInsertedFilter.SetDateFrom(filter.DateInsertedFrom, startYear);
            dccDateInsertedFilter.SetDateTo(filter.DateInsertedTo, startYear);

            dccDateInsertedFilter.CheckDateTime();

            ddlTermLastInsertFilter.SelectedValue = filter.TermDateInserted;
        }

        if (filter.DateInsertedIntervalIndex != Int32.MinValue)
        {
            cboxLastInsertFilter.Checked = true;
            dccDateInsertedFilter.IsIntervalUse = true;
            dccDateInsertedFilter.SetDateInterval(filter.DateInsertedIntervalIndex);
            dccDateInsertedFilter.SetDateIntervalMode(filter.DateInsertedIntervalModeIndex);
        }

        if (filter.ComputerName != String.Empty)
        {
            cboxComputerFilter.Checked = true;
            tboxComputerFilter.Text = filter.ComputerName;
            ddlTermComputerNameFilter.SelectedValue = filter.TermComputerName;
        }

        if (filter.SerialNumber != String.Empty)
        {
            cboxSerialNumber.Checked = true;
            tboxSerialNumber.Text = filter.SerialNumber;
            ddlTermSerialNumber.SelectedValue = filter.TermSerialNumber;
        }

        if (filter.NameDevice != String.Empty)
        {
            cboxCommentFilter.Checked = true;
            tboxCommentFilter.Text = filter.NameDevice;
            ddlTermCommentFilter.SelectedValue = filter.TermNameDevice;
        }
    }

    public void GetCurrentStateFilterDevice(ref DeviceFilterEntity fltr)
    {
        if (cboxSerialNumber.Checked)
        {
            fltr.SerialNumber = tboxSerialNumber.Text;
            fltr.TermSerialNumber = ddlTermSerialNumber.SelectedValue;
        }
        else
        {
            //костыль...
            fltr.SerialNumber = "*";
            fltr.TermSerialNumber = ddlTermSerialNumber.SelectedValue;
        }

        if (cboxCommentFilter.Checked)
        {
            fltr.NameDevice = tboxCommentFilter.Text;
            fltr.TermNameDevice = ddlTermCommentFilter.SelectedValue;
        }

        if (cboxComputerFilter.Checked)
        {
            fltr.ComputerName = tboxComputerFilter.Text;
            fltr.TermComputerName = ddlTermComputerNameFilter.SelectedValue;
        }
        else
        {
            fltr.ComputerName = "*";
            fltr.TermComputerName = ddlTermComputerNameFilter.SelectedValue;
        }

        try
        {
            if (cboxLastInsertFilter.Checked)
            {

                if (dccDateInsertedFilter.IsIntervalUse)
                {
                    fltr.DateInsertedIntervalIndex = dccDateInsertedFilter.IntervalIndex;
                    fltr.DateInsertedIntervalModeIndex = dccDateInsertedFilter.IntervalModeIndex;
                }
                else
                {

                    fltr.DateInsertedFrom = dccDateInsertedFilter.GetDateFrom();
                    fltr.DateInsertedTo = dccDateInsertedFilter.GetDateTo();

                    fltr.TermDateInserted = ddlTermLastInsertFilter.SelectedValue;

                    dccDateInsertedFilter.CheckDateTime();
                }
            }
        }
        catch
        {
            throw new InvalidOperationException(Resources.Resource.ErrorInvalidDate);
        }
    }

    public void ClearFilterDevice()
    {
        cboxSerialNumber.Checked = false;
        tboxSerialNumber.Text = String.Empty;
        ddlTermSerialNumber.SelectedIndex = 0;

        cboxCommentFilter.Checked = false;
        tboxCommentFilter.Text = String.Empty;
        ddlTermCommentFilter.SelectedIndex = 0;

        cboxComputerFilter.Checked = false;
        tboxComputerFilter.Text = String.Empty;
        ddlTermComputerNameFilter.SelectedIndex = 0;

        cboxLastInsertFilter.Checked = false;
        ddlTermLastInsertFilter.SelectedIndex = 0;
        dccDateInsertedFilter.Clear();
    }

    protected void lbtnApplyFilterDevice_Click(object sender, EventArgs e)
    {
        DeviceFilterEntity filter = new DeviceFilterEntity();

        GetCurrentStateFilterDevice(ref filter);

        filter.CheckFilters();
        filter.GenerateSQLWhereStatement();

        pcPaging.CurrentPageIndex = 1;
        pcPagingTop.CurrentPageIndex = 1;

        if (filter.GetSQLWhereStatement != String.Empty)
            Session["CurrentDeviceFilter"] = filter;
        else
            Session["CurrentDeviceFilter"] = null;

        UpdateDataDevices();
    }

    protected void lbtnCancelFilterDevice_Click(object sender, EventArgs e)
    {
        Session["CurrentDeviceFilter"] = null;
        ClearFilterDevice();
        UpdateDataDevices();
    }

    //Filter Unknown Device

    private void InitFilterPanelUDevice()
    {
        cboxComputerNameUDevice.Text = Resources.Resource.ComputerName;
        cboxSerialNumberUDevice.Text = Resources.Resource.SerialNo;
        cboxCommentUDevice.Text = Resources.Resource.Comment;
        cboxLastInsertUDevice.Text = Resources.Resource.DeviceInsertedDate;

        List<string> terms = new List<string>();
        terms.Add("AND");
        terms.Add("OR");
        terms.Add("NOT");

        try
        {
            ddlTermSerialNumberUDevice.DataSource = terms;
            ddlTermSerialNumberUDevice.DataBind();
        }
        catch
        {
            ddlTermSerialNumberUDevice.SelectedIndex = -1;
            ddlTermSerialNumberUDevice.SelectedValue = null;
            ddlTermSerialNumberUDevice.DataSource = terms;
            ddlTermSerialNumberUDevice.DataBind();
        }

        try
        {
            ddlTermComputerNameUDevice.DataSource = terms;
            ddlTermComputerNameUDevice.DataBind();
        }
        catch
        {
            ddlTermComputerNameUDevice.SelectedIndex = -1;
            ddlTermComputerNameUDevice.SelectedValue = null;
            ddlTermComputerNameUDevice.DataSource = terms;
            ddlTermComputerNameUDevice.DataBind();
        }

        try
        {
            ddlTermCommentUDevice.DataSource = terms;
            ddlTermCommentUDevice.DataBind();
        }
        catch
        {
            ddlTermCommentUDevice.SelectedIndex = -1;
            ddlTermCommentUDevice.SelectedValue = null;
            ddlTermCommentUDevice.DataSource = terms;
            ddlTermCommentUDevice.DataBind();
        }

        try
        {
            ddlTermLastInsertUDevice.DataSource = terms;
            ddlTermLastInsertUDevice.DataBind();
        }
        catch
        {
            ddlTermLastInsertUDevice.SelectedIndex = -1;
            ddlTermLastInsertUDevice.SelectedValue = null;
            ddlTermLastInsertUDevice.DataSource = terms;
            ddlTermLastInsertUDevice.DataBind();
        }

        if (dccDateInserted.FromMonthObject != null) return;
        
        List<string> monthList = new List<string>();
        foreach (string month in CultureInfo.CurrentCulture.DateTimeFormat.MonthNames)
            if (month != String.Empty)
                monthList.Add(month);

        dccDateInserted.FromMonthObject = monthList;
        dccDateInserted.ToMonthObject = monthList;

        List<string> dayList = new List<string>();
        for (int i = 1; i <= 31; i++)
            dayList.Add(i.ToString());

        dccDateInserted.FromDayObject = dayList;
        dccDateInserted.ToDayObject = dayList;

        List<string> yearList = new List<string>();
        for (int i = 1; i > -2; i--)
            yearList.Add(Convert.ToString(DateTime.Now.Year - i));

        dccDateInserted.FromYearObject = yearList;
        dccDateInserted.ToYearObject = yearList;

        List<string> hourList = new List<string>();
        for (int i = 0; i < 24; i++)
            hourList.Add(i.ToString());

        dccDateInserted.FromHourObject = hourList;
        dccDateInserted.ToHourObject = hourList;

        List<string> minuteList = new List<string>();
        for (int i = 0; i < 60; i++)
            minuteList.Add(i.ToString());

        dccDateInserted.FromMinuteObject = minuteList;
        dccDateInserted.ToMinuteObject = minuteList;

        dccDateInserted.SetFromText = Resources.Resource.From;
        dccDateInserted.SetToText = Resources.Resource.To;

        DateTime dt = DateTime.Now.AddMonths(-1);
        string lyear = ((int)(DateTime.Now.Year - 1)).ToString();
        dccDateInserted.SetDateFrom(dt, lyear);
        dccDateInserted.SetDateTo(DateTime.Now, lyear);

        List<string> list = Anchor.GetDateIntervals();
        dccDateInserted.Interval = list;

        List<string> intervalTypes = new List<string>();
        intervalTypes.Add(Resources.Resource.HighDate);
        intervalTypes.Add(Resources.Resource.LowDate);

        dccDateInserted.IntervalMode = intervalTypes;
    }

    public void LoadFilterUDevice(UnknownDevicesFilterEntity filter)
    {
        InitFilterPanelUDevice();
        string startYear = Convert.ToString(DateTime.Now.Year - 1);

        if ((filter.DateInsertedFrom != DateTime.MinValue) ||
            (filter.DateInsertedTo != DateTime.MinValue))
        {
            cboxLastInsertUDevice.Checked = true;
            dccDateInserted.SetDateFrom(filter.DateInsertedFrom, startYear);
            dccDateInserted.SetDateTo(filter.DateInsertedTo, startYear);

            dccDateInserted.CheckDateTime();

            ddlTermLastInsertUDevice.SelectedValue = filter.TermDateInserted;
        }

        if (filter.DateInsertedIntervalIndex != Int32.MinValue)
        {
            cboxLastInsertUDevice.Checked = true;
            dccDateInserted.IsIntervalUse = true;
            dccDateInserted.SetDateInterval(filter.DateInsertedIntervalIndex);
            dccDateInserted.SetDateIntervalMode(filter.DateInsertedIntervalModeIndex);
        }

        if (filter.SerialNumber != String.Empty)
        {
            cboxSerialNumberUDevice.Checked = true;
            tboxSerialNumberUDevice.Text = filter.SerialNumber;
            ddlTermSerialNumberUDevice.SelectedValue = filter.TermSerialNumber;
        }

        if (filter.ComputerName != String.Empty)
        {
            cboxComputerNameUDevice.Checked = true;
            tboxComputerNameUDevice.Text = filter.ComputerName;
            ddlTermComputerNameUDevice.SelectedValue = filter.TermComputerName;
        }

        if (filter.Comment != String.Empty)
        {
            cboxCommentUDevice.Checked = true;
            tboxCommentUDevice.Text = filter.Comment;
            ddlTermCommentUDevice.SelectedValue = filter.TermComment;
        }
    }

    public void GetCurrentStateFilterUDevice(ref UnknownDevicesFilterEntity fltr)
    {
        if (cboxSerialNumberUDevice.Checked)
        {
            fltr.SerialNumber = tboxSerialNumberUDevice.Text;
            fltr.TermSerialNumber = ddlTermSerialNumberUDevice.SelectedValue;
        }
        else
        {
            //костыль...
            fltr.SerialNumber = "*";
            fltr.TermSerialNumber = ddlTermSerialNumberUDevice.SelectedValue;
        }

        if (cboxComputerNameUDevice.Checked)
        {
            fltr.ComputerName = tboxComputerNameUDevice.Text;
            fltr.TermComputerName = ddlTermComputerNameUDevice.SelectedValue;
        }
        else 
        {
            fltr.ComputerName = "*";
            fltr.TermComputerName = ddlTermComputerNameUDevice.SelectedValue;
        }

        if (cboxCommentUDevice.Checked)
        {
            fltr.Comment = tboxCommentUDevice.Text;
            fltr.TermComment = ddlTermCommentUDevice.SelectedValue;
        }
        else
        {
            fltr.Comment = "*";
            fltr.TermComment = ddlTermCommentUDevice.SelectedValue;
        }

        try
        {
            if (cboxLastInsertUDevice.Checked)
            {

                if (dccDateInserted.IsIntervalUse)
                {
                    fltr.DateInsertedIntervalIndex = dccDateInserted.IntervalIndex;
                    fltr.DateInsertedIntervalModeIndex = dccDateInserted.IntervalModeIndex;
                }
                else
                {

                    fltr.DateInsertedFrom = dccDateInserted.GetDateFrom();
                    fltr.DateInsertedTo = dccDateInserted.GetDateTo();

                    fltr.TermDateInserted = ddlTermLastInsertUDevice.SelectedValue;

                    dccDateInserted.CheckDateTime();
                }
            }
        }
        catch
        {
            throw new InvalidOperationException(Resources.Resource.ErrorInvalidDate);
        }

    }

    public void ClearFilterUDevice()
    {
        cboxSerialNumberUDevice.Checked = false;
        tboxSerialNumberUDevice.Text = String.Empty;
        ddlTermSerialNumberUDevice.SelectedIndex = 0;

        cboxComputerNameUDevice.Checked = false;
        tboxComputerNameUDevice.Text = String.Empty;
        ddlTermComputerNameUDevice.SelectedIndex = 0;

        cboxCommentUDevice.Checked = false;
        tboxCommentUDevice.Text = String.Empty;
        ddlTermCommentUDevice.SelectedIndex = 0;

        cboxLastInsertUDevice.Checked = false;
        ddlTermLastInsertUDevice.SelectedIndex = 0;
        dccDateInserted.Clear();
    }

    protected void lbtnApplyFilterUDevice_Click(object sender, EventArgs e)
    {
        UnknownDevicesFilterEntity filter = new UnknownDevicesFilterEntity();

        GetCurrentStateFilterUDevice(ref filter);

        filter.CheckFilters();
        filter.GenerateSQLWhereStatement();

        pcPaging.CurrentPageIndex = 1;
        pcPagingTop.CurrentPageIndex = 1;

        if (filter.GetSQLWhereStatement != String.Empty)
            Session["CurrentUDeviceFilter"] = filter;
        else
            Session["CurrentUDeviceFilter"] = null;

        UpdateDataDevicesUnknown();
    }

    protected void lbtnCancelFilterUDevice_Click(object sender, EventArgs e)
    {
        Session["CurrentUDeviceFilter"] = null;
        ClearFilterUDevice();
        UpdateDataDevicesUnknown();
    }

    #endregion

    #region Sorting Unknown Devices

    protected void lbtnHeaderUDeviceSerialNo_Click(object sender, EventArgs e)
    {
        if (OrderByName == "SerialNo")
            OrderByDirection = !OrderByDirection;
        else OrderByDirection = true;

        OrderByName = "SerialNo";
        UpdateDataDevicesUnknown();
    }

    protected void lbtnHeaderUDeviceComment_Click(object sender, EventArgs e)
    {
        if (OrderByName == "Comment")
            OrderByDirection = !OrderByDirection;
        else OrderByDirection = true;

        OrderByName = "Comment";
        UpdateDataDevicesUnknown();
    }

    protected void lbtnHeaderUDeviceComputerName_Click(object sender, EventArgs e)
    {
        if (OrderByName == "ComputerName")
            OrderByDirection = !OrderByDirection;
        else OrderByDirection = true;

        OrderByName = "ComputerName";
        UpdateDataDevicesUnknown();
    }

    protected void lbtnHeaderUDeviceInsertedDate_Click(object sender, EventArgs e)
    {
        if (OrderByName == "LatestInsert")
            OrderByDirection = !OrderByDirection;
        else OrderByDirection = true;

        OrderByName = "LatestInsert";
        UpdateDataDevicesUnknown();
    }

    protected void DataList3_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            (e.Item.FindControl("lbtnHeaderUDeviceSerialNo") as LinkButton).Text = Resources.Resource.SerialNo;
            (e.Item.FindControl("lbtnHeaderUDeviceComment") as LinkButton).Text = Resources.Resource.Comment;
            (e.Item.FindControl("lbtnHeaderUDeviceComputerName") as LinkButton).Text = Resources.Resource.ComputerName;
            (e.Item.FindControl("lbtnHeaderUDeviceInsertedDate") as LinkButton).Text = Resources.Resource.DeviceInsertedDate;

            if (OrderByName == "SerialNo")
            {
                if (OrderByDirection)
                    (e.Item.FindControl("lbtnHeaderUDeviceSerialNo") as LinkButton).Text = Resources.Resource.SerialNo + " \u2193";
                else (e.Item.FindControl("lbtnHeaderUDeviceSerialNo") as LinkButton).Text = Resources.Resource.SerialNo + " \u2191"; 
            }

            if (OrderByName == "Comment")
            {
                if (OrderByDirection)
                    (e.Item.FindControl("lbtnHeaderUDeviceComment") as LinkButton).Text = Resources.Resource.Comment + " \u2193";
                else (e.Item.FindControl("lbtnHeaderUDeviceComment") as LinkButton).Text = Resources.Resource.Comment + " \u2191";
            }

            if (OrderByName == "ComputerName")
            {
                if (OrderByDirection)
                    (e.Item.FindControl("lbtnHeaderUDeviceComputerName") as LinkButton).Text = Resources.Resource.ComputerName + " \u2193";
                else (e.Item.FindControl("lbtnHeaderUDeviceComputerName") as LinkButton).Text = Resources.Resource.ComputerName + " \u2191";
            }

            if (OrderByName == "LatestInsert")
            {
                if (OrderByDirection)
                    (e.Item.FindControl("lbtnHeaderUDeviceInsertedDate") as LinkButton).Text = Resources.Resource.DeviceInsertedDate + " \u2193";
                else (e.Item.FindControl("lbtnHeaderUDeviceInsertedDate") as LinkButton).Text = Resources.Resource.DeviceInsertedDate + " \u2191";
            }
 
        }
    }

    #endregion

    #region Sorting Devices

    protected void lbtnHeaderDeviceSerialNo_Click(object sender, EventArgs e)
    {
        if (OrderByNameDevice == "SerialNo")
            OrderByDirectionDevice = !OrderByDirectionDevice;
        else OrderByDirectionDevice = true;

        OrderByNameDevice = "SerialNo";
        UpdateDataDevices();
    }

    protected void lbtnHeaderDeviceComment_Click(object sender, EventArgs e)
    {
        if (OrderByNameDevice == "Comment")
            OrderByDirectionDevice = !OrderByDirectionDevice;
        else OrderByDirectionDevice = true;

        OrderByNameDevice = "Comment";
        UpdateDataDevices();
    }

    protected void lbtnHeaderDeviceLastComputer_Click(object sender, EventArgs e)
    {
        if (OrderByNameDevice == "ComputerName")
            OrderByDirectionDevice = !OrderByDirectionDevice;
        else OrderByDirectionDevice = true;

        OrderByNameDevice = "ComputerName";
        UpdateDataDevices();
    }

    protected void lbtnHeaderDeviceLatestInsert_Click(object sender, EventArgs e)
    {
        if (OrderByNameDevice == "LatestInsert")
            OrderByDirectionDevice = !OrderByDirectionDevice;
        else OrderByDirectionDevice = true;

        OrderByNameDevice = "LatestInsert";
        UpdateDataDevices();
    }

    protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            (e.Item.FindControl("lbtnHeaderDeviceSerialNo") as LinkButton).Text = Resources.Resource.SerialNo;
            (e.Item.FindControl("lbtnHeaderDeviceComment") as LinkButton).Text = Resources.Resource.Comment;
            (e.Item.FindControl("lbtnHeaderDeviceLastComputer") as LinkButton).Text = Resources.Resource.ComputerName; //change
            (e.Item.FindControl("lbtnHeaderDeviceLatestInsert") as LinkButton).Text = Resources.Resource.DeviceInsertedDate; //change

            if (OrderByNameDevice == "SerialNo")
            {
                if (OrderByDirectionDevice)
                    (e.Item.FindControl("lbtnHeaderDeviceSerialNo") as LinkButton).Text = Resources.Resource.SerialNo + " \u2193";
                else (e.Item.FindControl("lbtnHeaderDeviceSerialNo") as LinkButton).Text = Resources.Resource.SerialNo + " \u2191";
            }

            if (OrderByNameDevice == "Comment")
            {
                if (OrderByDirectionDevice)
                    (e.Item.FindControl("lbtnHeaderDeviceComment") as LinkButton).Text = Resources.Resource.Comment + " \u2193";
                else (e.Item.FindControl("lbtnHeaderDeviceComment") as LinkButton).Text = Resources.Resource.Comment + " \u2191";
            }

            if (OrderByNameDevice == "ComputerName")
            {
                if (OrderByDirectionDevice)
                    (e.Item.FindControl("lbtnHeaderDeviceLastComputer") as LinkButton).Text = Resources.Resource.ComputerName + " \u2193";
                else (e.Item.FindControl("lbtnHeaderDeviceLastComputer") as LinkButton).Text = Resources.Resource.ComputerName + " \u2191";
            }

            if (OrderByNameDevice == "LatestInsert")
            {
                if (OrderByDirectionDevice)
                    (e.Item.FindControl("lbtnHeaderDeviceLatestInsert") as LinkButton).Text = Resources.Resource.DeviceInsertedDate + " \u2193";
                else (e.Item.FindControl("lbtnHeaderDeviceLatestInsert") as LinkButton).Text = Resources.Resource.DeviceInsertedDate + " \u2191";
            }

        }
    }

    #endregion


}
