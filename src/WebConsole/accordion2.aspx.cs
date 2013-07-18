using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;

using VirusBlokAda.Vba32CC.Groups;
using VirusBlokAda.Vba32CC.Policies;
using VirusBlokAda.Vba32CC.Policies.Devices.Policy;
using VirusBlokAda.Vba32CC.Policies.Devices;
using ARM2_dbcontrol.DataBase;


using System.Configuration;
using Filters.Common;
public partial class accordion2 : PageBase
{

    public static GroupProvider GroupState
    {
        get
        {
            GroupProvider provider = HttpContext.Current.Application["GroupState"] as GroupProvider;
            if (provider == null)
            {
                provider = new GroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Application["GroupState"] = provider;
            }

            return provider;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScript(@"js/jQuery/jquery.cookie.js");
        RegisterScript(@"js/accordion2.js");
        RegisterScript(@"js/json2.js");
        RegisterScript(@"js/jQuery/jquery.loadmask.js");

        if (!IsPostBack)
        {
            InitFields();
        }

        Controls_PagerUserControl.AddGridViewExtendedAttributes(GridView1, ObjectDataSource1);
       
        //Validation
    }
    protected override void InitFields()
    {
        GridView1.EmptyDataText = Resources.Resource.EmptyMessage;

        //fltState.DataSource = PolicyState.GetPolicyStates();
        //fltState.DataBind();
    }

    //protected void DeviceFilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    //{
    //    GridView1.PageIndex = 0;
    //    GridView1.Where = e.Where;
    //    GridView1.DataBind();
    //}
    #region GroupTab
    #region loadGroups
    /* начальный список групп*/
    [WebMethod]
    public static string GetListRootGroup()
    {
        GroupProvider provider = GroupState;

        string str = ConvertListRootGroup();
        str += ConvertGroupData(null);
        return str;
      }
    /*конвертация начального списка*/
    [WebMethod]
    private static string ConvertListRootGroup()
    {
        GroupProvider provider = GroupState;
        List<Group> groupList = provider.GetSubgroups(null);
        string groupListData = "";
        for (int i = 0; i < groupList.Count; i++)
        {
            groupListData += ConvertGroupData(groupList[i]);
            groupListData += Environment.NewLine;
        }
        return groupListData;
    }
    //получение данных группы по id
    [WebMethod]
    public static string GetData(string id)
    {
        string str = "";
        int groupID = Convert.ToInt32(id);
        GroupProvider provider = GroupState;
        str += "{";
        if (id == null)
        {
            str += ConvertWithoutGroupData();
            str += "\"}";
            return str;
        }

        str += ConvertSubGroupData(groupID);
        str += ConvertCompListData(groupID);
        str += "\"}";
        return str;
    }
    //конвертирует список компьютеров для группы с id
    private static string ConvertCompListData(int groupID)
    {
        GroupProvider provider = GroupState;
        List<ComputersEntity> compList = provider.GetComputersByGroup(groupID);
        if (compList.Count == 0)
            return "<tr><td></td></tr>";
        string compListData = "";
        for (int i = 0; i < compList.Count; i++)
        {
            compListData += "<tr>" + ConvertComputerData(compList[i]).ToString() + "</tr>";
        }
        return compListData;
    }
    //конвертирует список подгрупп для группы с id
    private static string ConvertSubGroupData(int groupID)
    {
        GroupProvider provider = GroupState;
        string subGroupData = "\"acc\":\"";
        List<Group> groupList = provider.GetSubgroups(groupID);
        if (groupList.Count > 0)
        {
            subGroupData += "false\",\"text\":\"";
            subGroupData += "<tr width='100%'><td colspan='2'><div acc=\'false\' id=\'accordion" + groupID.ToString() + "\'>";
            for (int i = 0; i < groupList.Count; i++)
            {
                subGroupData += ConvertGroupData(groupList[i]);
            };
            subGroupData += "</div></td></tr>";
        }
        else
        {
            subGroupData += "null\",\"text\":\"";
        }

        return subGroupData;
    }
    //конвертирует содержание группы
    private static string ConvertGroupData(Group? group)
    {
        string groupData = "";
        if (group == null)
        {
            groupData += "<h3 id=null acc=null load=false ><a style='font-size:10pt !important'>";
            groupData += "<span id=null >";
            groupData += Resources.Resource.ComputersWithoutGroups + "</span>";
        }
        else
        {
            Group gr = (Group)group;
            groupData += "<h3  id=\'" + gr.ID.ToString() + "\' acc=null load=false><a style='font-size:10pt !important'>";
            groupData += "<span  >";
            groupData += gr.Name.ToString() + "</span>";
        }
        groupData += "</a></h3>";
        groupData += "<div><table width='100%' class='ListContrastTable'></table></div>";
        return groupData;
    }
    //без групп
    private static string ConvertWithoutGroupData()
    {
        GroupProvider provider = GroupState;
        List<ComputersEntity> compList = provider.GetComputersWithoutGroup();
        string withoutGroupData = "\"acc\":\"null\",\"text\":\"";
        if (compList.Count == 0)
        {
            withoutGroupData += "<tr><td/></tr>";
        }
        for (int i = 0; i < compList.Count; i++)
        {
            withoutGroupData += ConvertComputerData(compList[i]).ToString() ;
        }
        return withoutGroupData;
    }
    /* конвертирует данные компьютера */
    private static string ConvertComputerData(ComputersEntity comp)
    {

        string compData = "<tr style='cursor: pointer; font-size:10pt !important'><td ><div cp=" + comp.ID + " name=" + comp.ComputerName + " comp=false >";
        compData += comp.ComputerName;
        compData += "</div></td>";
        compData += "<td width='50%' align='center'><div cp=" + comp.ID + "  name=" + comp.ComputerName + " comp=false >" + comp.IPAddress + "</div></td></tr>";
        return compData;
    }
    #endregion


    #region Computers

    public static PolicyProvider PolicyState
    {
        get
        {
           PolicyProvider provider = HttpContext.Current.Application["PolicyState"] as PolicyProvider;
            if (provider == null)
            {
                provider = new PolicyProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Application["PolicyState"] = provider;
            }

            return provider;
        }
    }
    /* получение данных о компьютере */
    [WebMethod]
    public static  string GetComputersData(int id)
    {
        System.Diagnostics.Debug.Write("GetComputersData:" + id);


        return ConvertComputerDataForClient(id,
            PolicyState.GetDevicesPoliciesByComputer(id)); ;
    }
    private static string ConvertComputerDataForClient(int id, List<DevicePolicy> list)
    {
        string table = "<table style='width:100% ' class='ListContrastTable' cp=" + id + "><thead><th style='text-align:center'>" +
        ResourceControl.GetStringForCurrentCulture("SerialNo") + "</th><th style='text-align:center'>" +
        ResourceControl.GetStringForCurrentCulture("Comment") + "</th><th style='text-align:center'>" +
        ResourceControl.GetStringForCurrentCulture("DeviceInsertedDate") + "</th><th style='text-align:center'>" +
        ResourceControl.GetStringForCurrentCulture("State") + "</th><th style='text-align:center'>" +
        ResourceControl.GetStringForCurrentCulture("Actions") + "</th></thead><tbody>";
        foreach (DevicePolicy dp in list)
        {
            string row = "<tr style='text-align:center'><td>" + Anchor.FixString(dp.Device.SerialNo, 30) + "</td>";
            string comment = dp.Device.Comment;
            if (String.IsNullOrEmpty(comment))
                comment = Anchor.GetCommentFromSerial(dp.Device.SerialNo);

            row += "<td type='comment' dp=" + dp.Device.ID + " >" + comment + "</td>";
            //row += "<td>" + (dp.LatestInsert==DateTime.MinValue?"":dp.LatestInsert.ToString()) + "</td>";
            row += "<td style='width:60px'>" + dp.LatestInsert ?? "" + "</td>";
            
            string select = "<img dp=" + dp.Device.ID + " cp="+ id+" state=";
                int i = (int)dp.State;
                switch (i)
                {
                    case 0:
                        select += "Undefined src=\'App_Themes/Main/Images/undefined.gif\' />" ;
                        break;
                    case 1:
                        select += "Enabled src=\'App_Themes/Main/Images/enabled.gif\' />";
                        break;
                    case 2:
                        select += "Disabled src=\'App_Themes/Main/Images/disabled.gif\' />";
                        break;
                }
            row += "<td>" + select + "</td>";
            row += "<td><img title='" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "' comdp=" + dp.Device.ID + " serialdp=" + dp.Device.SerialNo + " src=\'App_Themes/Main/Images/table_edit.png\' /><img title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' deldp=" + dp.ID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
            row += "</tr>";
            table += row;
        }
        table += "</table>";
        string text = "<div><input type=text dpc=" + id + " style='width:400px'></input>";
        string button = "<button style='width:auto' dpc=" + id + ">" + ResourceControl.GetStringForCurrentCulture("Add") + "</button></div>";
     /*   if (list.Count == 0)
            table = "";*/
        return table+text+button;
    }
    /*изменение состояния */
    [WebMethod]
    public static void ChangeDevicePolicyStateComputer(int dp,int cp, string state)// deviceId,compId,state
    {
        System.Diagnostics.Debug.Write("ChangeDevicePolicyState with id:" + dp + ", state:" + state);


        PolicyState.ChangeDevicePolicyStatusForComputer((Int16)dp,(Int16)cp, state);


    }
    [WebMethod]
    /* изменение комментария*/
    public static string GetChangeCommentDialog(int id)
    {
        Device device = PolicyState.GetDevice(id);
        if (String.IsNullOrEmpty(device.Comment))
            device.Comment = Anchor.GetCommentFromSerial(device.SerialNo);
        string label = "<div>" + ResourceControl.GetStringForCurrentCulture("DeviceComment") + "</div>";
        string text = "<input type=text dcdpc=" + id + " style='width:400px' value='" + device.Comment + "'></input>";
        string button = "<button dcdpc=" + id + ">" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "</button>";

        return label + text + button;
    }
    [WebMethod]
    public static void ChangeComment(int id, string comment)
    {
        Device device = new Device();
        device.ID = id;
        device.Comment = comment;

        PolicyState.EditDevice(device);
    }
    /* добавление нового устройства к компьютеру*/
    [WebMethod]
    public static string AddNewDevicePolicyToComputer(short id, string serial)
    {
        serial = serial.Replace(" ", "");
        System.Diagnostics.Debug.Write("AddNewDevicePolicy id:" + id + ", serial:" + serial);

        if (serial == null || serial == String.Empty)
            throw new Exception("1");

        Device device = new Device(serial, DeviceType.USB);

        /*   device = PoliciesState.AddDevice(device);*/

        ComputersEntity computer = new ComputersEntity();
        computer.ID = id;
        DevicePolicy dp = new DevicePolicy(device, computer);
        dp.State = DevicePolicyState.Undefined;
        DevicePolicy policy = PolicyState.AddDevicePolicyToComputer(dp);
        if (policy.Device.ID != 0)
        {
            return ConvertDevicePolicy(policy);
        }
        else return "{\"success\":\"false\",\"error\":\""+ResourceControl.GetStringForCurrentCulture("DeviceCanNotAdded")+"\"}";
    }
    private static string ConvertDevicePolicy(DevicePolicy dp)
    {
        return "{\"success\":\"true\", \"deviceID\":\"" + dp.Device.ID + "\", \"comment\":\"" + dp.Device.Comment + "\",\"policyID\":\"" + dp.ID + "\",\"change\":\"" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "\",\"del\":\"" + ResourceControl.GetStringForCurrentCulture("Delete") + "\"}";
    }
    /* удаление устройства из компьютера*/
    [WebMethod]
    public static void RemoveDevicePolicy(int id)
    {
        PolicyState.DeleteDevicePolicyByID(id);
    }
    #endregion


    #region Group
    [WebMethod]
    public static /*List<DevicePolicy>*/ string GetGroupDeviceData(int id)
    {
       
        System.Diagnostics.Debug.Write("GetGroupDeviceData" + id); 
        if(id<0) 
            return ConvertGroupDataForClient(id, PolicyState.GetDevicesPoliciesWithoutGroup());
        else
        return ConvertGroupDataForClient(id, PolicyState.GetDevicesPoliciesByGroup(id)); 
    }

    private static string ConvertGroupDataForClient(int groupID, List<DevicePolicy> list)
    {
        string table = "<table style='width:100% text-align:center !important' class='ListContrastTable' gdp=" + groupID + "><thead><th></th><th style='text-align:center'>" +
       ResourceControl.GetStringForCurrentCulture("SerialNo") + "</th><th style='text-align:center'>" +
       ResourceControl.GetStringForCurrentCulture("Comment") + "</th><th style='text-align:center'>" +
       ResourceControl.GetStringForCurrentCulture("ComputerName") + "</th><th style='text-align:center'>" +
       ResourceControl.GetStringForCurrentCulture("DeviceInsertedDate") + "</th><th style='text-align:center'>" +
       ResourceControl.GetStringForCurrentCulture("State") + "</th><th style='text-align:center'>" +
       ResourceControl.GetStringForCurrentCulture("Actions") + "</th></thead><tbody>";
        string all = "";
        foreach (DevicePolicy dp in list)
        {
            all = "";
            if (dp.Device.LastComputer == "0")
            {
                all = "<img nfadp="+dp.Device.ID+" src=\'App_Themes/Main/Images/notForAll.gif \' />";
            }
            string row = "<tr style='text-align:center'><td>"+all+"</td><td>"+ Anchor.FixString(dp.Device.SerialNo, 30) + "</td>";
            string comment = dp.Device.Comment;
            if (String.IsNullOrEmpty(comment))
                comment = Anchor.GetCommentFromSerial(dp.Device.SerialNo);

            row += "<td type='comment' dp=" + dp.Device.ID + ">" + comment + "</td>";
            row += "<td>" + dp.Computer.ComputerName + "</td>";
            //row += "<td>" + (dp.LatestInsert==DateTime.MinValue?"":dp.LatestInsert.ToString()) + "</td>";
            row += "<td style='width:60px'>" + dp.LatestInsert ?? "" + "</td>";

            string select = "<img dp= "+dp.Device.ID+" gdp=" + groupID + " state=";
            int i = (int)dp.State;
            switch (i)
            {
                case 0:
                    select += "Undefined src=\'App_Themes/Main/Images/undefined.gif\' />";
                    break;
                case 1:
                    select += "Enabled src=\'App_Themes/Main/Images/enabled.gif\' />";
                    break;
                case 2:
                    select += "Disabled src=\'App_Themes/Main/Images/disabled.gif\' />";
                    break;
            }
            row += "<td>" + select + "</td>";
            row += "<td><img title='" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "' comdp=" + dp.Device.ID + " serialdp=" + dp.Device.SerialNo + " src=\'App_Themes/Main/Images/table_edit.png\' />";
            if(groupID>=0)                   
                row+="<img title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' delgroupid=" + groupID + " delgroupdevid=" + dp.Device.ID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
            else row += "<img title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' delwithoutgroupdevid=" + dp.Device.ID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
            row += "</tr>";
            table += row;
        }
        table += "</table>";
        string text = "<div><input type=text dgr=" + groupID + " style='width:400px'></input>";
        string button = "<button style='width:auto' dgr=" + groupID + ">" + ResourceControl.GetStringForCurrentCulture("Add") + "</button></div>";
        /*   if (list.Count == 0)
               table = "";*/
        return table + text + button;
    }
    [WebMethod]
    public static void ChangeDevicePolicyStateGroup(int dp, int gp, string state)//device,group,state
    {
        System.Diagnostics.Debug.Write("ChangeDevicePolicyStateForGroup with Id:" + dp + ", state:" + state);
        if (gp <= 0) PolicyState.ChangeDevicePolicyStatusWithoutGroup((Int16)dp, state);
        else
        PolicyState.ChangeDevicePolicyStatusForGroup((Int16)dp,gp,state);
    }
    //добавление устройства в группу
    [WebMethod]
    public static string AddNewDevicePolicyGroup(int id, string serial)
    {

        serial = serial.Replace(" ", "");
        System.Diagnostics.Debug.Write("AddNewDevicePolicyToGroup id:" + id + ", serial:" + serial);

        if (serial == null || serial == String.Empty)
            throw new Exception("1");
        Device device = new Device(serial, DeviceType.USB);
        Device newDevice;
        if (id < 0)
            newDevice = PolicyState.AddDeviceToWithoutGroup(device);
        else newDevice = PolicyState.AddDeviceToGroup(id, device);

        string row = newDevice.ID + " " + newDevice.Comment + " " + newDevice.SerialNo;

        return ConvertDevice(newDevice);
    }

    private static string ConvertDevice(Device device)
    {
        if (device.ID == 0)
            return "{\"success\":\"false\",\"error\":\""+ResourceControl.GetStringForCurrentCulture("DeviceCanNotAdded")+  "\"}";
        return "{\"success\":\"true\",\"id\":\"" + device.ID + "\",\"serial\":\"" + device.SerialNo + "\",\"comment\":\"" + device.Comment + "\"}";
    }
    /* удаление устройства из группы*/
    [WebMethod]
    public static void RemoveDevicePolicyGroup(int devid,int groupid)
    {
        PolicyState.RemoveDevicePolicyGroup(devid,groupid);
    }
    [WebMethod]
    public static void RemoveDevicePolicyWithoutGroup(int id)
    {
        PolicyState.RemoveDevicePolicyWithoutGroup(id);
    }
    #endregion

    #endregion

    #region DevicesTab
    [WebMethod]
    public static string GetAllDevices()
    {
       List<Device>DevicesList= PolicyState.GetDevicesList();
       return ConvertDevicesList(DevicesList);
    }
    private static string ConvertDevicesList(List<Device> list)
    {
        string table = "<table style='width:100% text-align:left !important' class='ListContrastTable' ><thead><th style='text-align:left'>" +
        ResourceControl.GetStringForCurrentCulture("SerialNo") + "</th><th style='text-align:left'>" +
        ResourceControl.GetStringForCurrentCulture("Comment") + "</th><th style='text-align:left'>" +
        ResourceControl.GetStringForCurrentCulture("ComputerName") + "</th><th style='text-align:left'>" +
        ResourceControl.GetStringForCurrentCulture("DeviceInsertedDate") + "</th><th style='text-align:left'>" +
        ResourceControl.GetStringForCurrentCulture("Actions") + "</th></thead><tbody>";
        foreach (Device dev in list)
        {
            string row = "<tr style='text-align:left' dev="+dev.ID+"><td>" + Anchor.FixString(dev.SerialNo, 30) + "</td>";
            string comment=dev.Comment;
            if (String.IsNullOrEmpty(comment))
                comment = Anchor.GetCommentFromSerial(dev.SerialNo);

            row += "<td dp="+dev.ID+" type='comment'>" + comment + "</td>";
            row += "<td>" + dev.LastComputer + "</td>";
            row += "<td>" + dev.LastInserted + "</td>";
            row += "<td style='text-align:center'><img title='" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "' comdp=" + dev.ID + " serialdp=" + dev.SerialNo + " src=\'App_Themes/Main/Images/table_edit.png\' /> <img title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' delete=true dcp="+dev.ID+" src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
           
            row += "</tr>";
            table += row;
        }
        table += "</tbody></table>";
        return table;
    }
    [WebMethod]
    public static string AddNewDevicePolicy(int id, string serial)
    {
        string row = "";
        
        return row;
    }
    #endregion
}