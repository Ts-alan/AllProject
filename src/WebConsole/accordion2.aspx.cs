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
    static Dictionary<int, Group> GroupDictionary;
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

        RegisterScript(@"js/accordion2_tree.js");
        RegisterScript(@"js/PageRequestManagerHelper.js");
        RegisterScript(@"js/Groups/ext-4.1.1/ext-all-debug.js");

        String scriptText = LoadResourceScript();
        RegisterBlockScript(scriptText);

        BranchOfTree tree = GetBranchOfTree(null);
        GroupDictionary = new Dictionary<int, Group>();
        Group WithoutGroup = new Group();
        WithoutGroup.ID = -1;
        GetDictionaryOfGroups(null);
        GroupDictionary.Add(-1, WithoutGroup);


        if (!IsPostBack)
        {
            InitFields();
        }

        Controls_PagerUserControl.AddGridViewExtendedAttributes(GridView1, ObjectDataSource1);
        Controls_PagerUserControl.AddGridViewExtendedAttributes(GridView2, ObjectDataSource2);

        //Validation
    }

    private String LoadResourceScript()
    {
        String resource = "var Resource={";
        resource += "Loading:'" + ResourceControl.GetStringForCurrentCulture("Loading") + "',";
        resource += "ChangeComment:'" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "',";
        resource += "Delete:'" + ResourceControl.GetStringForCurrentCulture("Delete") + "',";
        resource += "Devices:'" + ResourceControl.GetStringForCurrentCulture("Devices") + "',";
        resource += "Error:'" + ResourceControl.GetStringForCurrentCulture("Error") + "',";
        resource += "ErrorRequestingDataFromServer:'" + ResourceControl.GetStringForCurrentCulture("ErrorRequestingDataFromServer") + "',";
        resource += "Computers:'" + ResourceControl.GetStringForCurrentCulture("Computers") + "',";
        resource += "Computers:'" + ResourceControl.GetStringForCurrentCulture("Computers") + "',";
        resource += "Apply:'" + ResourceControl.GetStringForCurrentCulture("Apply") + "'";
        resource += "}";
        return resource;

    }
    protected override void InitFields()
    {
        GridView1.EmptyDataText = Resources.Resource.EmptyMessage;
        GridView2.EmptyDataText = Resources.Resource.EmptyMessage;

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
    public static String GetListRootGroup()
    {
        GroupProvider provider = GroupState;

        String str = ConvertListRootGroup();
        str += ConvertGroupData(null);
        return str;
    }
    /*конвертация начального списка*/
    [WebMethod]
    private static String ConvertListRootGroup()
    {
        GroupProvider provider = GroupState;
        List<Group> groupList = provider.GetSubgroups(null);
        String groupListData = "";
        for (int i = 0; i < groupList.Count; i++)
        {
            groupListData += ConvertGroupData(groupList[i]);
            groupListData += Environment.NewLine;
        }
        return groupListData;
    }
    //получение данных группы по id
    [WebMethod]
    public static String GetData(String id)
    {
        String str = "";
        int groupID = Convert.ToInt32(id);
        GroupProvider provider = GroupState;
        str += "{";
        if (id == null)
        {
            str += ConvertWithoutGroupData();
            str += "\"}";
            return str;
        }
        Boolean isEmpty = true;
        str += ConvertSubGroupData(groupID, out isEmpty);
        str += ConvertCompListData(groupID, isEmpty);
        str += "\"}";
        return str;
    }
    //конвертирует список компьютеров для группы с id
    private static String ConvertCompListData(int groupID, bool isEmpty)
    {
        GroupProvider provider = GroupState;
        List<ComputersEntity> compList = provider.GetComputersByGroup(groupID);
        if (compList.Count == 0)
        {
            if(isEmpty)
                return "<tr><td style='text-align:center'>" + Resources.Resource.GroupHasNotComps + "</td></tr>";
            else return "<tr><td ></td></tr>";
        }
        String compListData = "";
        String cssStyle = "";
        for (int i = 0; i < compList.Count; i++)
        {
            if (i % 2 == 0)
            {
                cssStyle = "gridViewRow";
            }
            else cssStyle = "gridViewRowAlternating";
         /*   compListData += "<tr class='"+cssStyle+"'>" + ConvertComputerData(compList[i]).ToString() + "</tr>";*/
            compListData += "<tr style='cursor: pointer' class='"+cssStyle+"'>"+ConvertComputerData(compList[i])+"</tr>";
        }
        return compListData;
    }
    //конвертирует список подгрупп для группы с id
    private static String ConvertSubGroupData(int groupID, out Boolean isEmpty)
    {
        isEmpty = true;
        GroupProvider provider = GroupState;
        String subGroupData = "\"acc\":\"";
        List<Group> groupList = provider.GetSubgroups(groupID);
        if (groupList.Count > 0)
        {
            isEmpty = false;
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
            isEmpty = true;
            subGroupData += "null\",\"text\":\"";
        }

        return subGroupData;
    }
    //конвертирует содержание группы
    private static String ConvertGroupData(Group? group)
    {
        String groupData = "";
        if (group == null)
        {
            groupData += "<h3 id=null acc=null load=false >";

            groupData += "<a style='font-size:10pt !important; '>";

            groupData += "<span id=null >";
            groupData += Resources.Resource.ComputersWithoutGroups + "</span>";
        }
        else
        {
            Group gr = (Group)group;
            groupData += "<h3  id=\'" + gr.ID.ToString() + "\' acc=null load=false><a style='font-size:10pt !important'>";
            groupData += "<span>";
            groupData += gr.Name.ToString() + "</span>";

        }
        groupData += "</a></h3>";
        groupData += "<div><table width='100%' class='ListContrastTable'></table></div>";
        return groupData;
    }
    //без групп
    private static String ConvertWithoutGroupData()
    {
        GroupProvider provider = GroupState;
        List<ComputersEntity> compList = provider.GetComputersWithoutGroup();
        String withoutGroupData = "\"acc\":\"null\",\"text\":\"";
        if (compList.Count == 0)
        {
            withoutGroupData +=  "<tr><td style='text-align:center'>" + Resources.Resource.GroupHasNotComps + "</td></tr>";
            return withoutGroupData;
        }
        String cssStyle = "";
        for (int i = 0; i < compList.Count; i++)
        {
            if (i % 2 == 0)
            {
                cssStyle = "gridViewRow";
            }
            else cssStyle = "gridViewRowAlternating";
            /*   compListData += "<tr class='"+cssStyle+"'>" + ConvertComputerData(compList[i]).ToString() + "</tr>";*/
            withoutGroupData += "<tr style='cursor: pointer; font-size:10pt !important' class='" + cssStyle + "'>" + ConvertComputerData(compList[i]) + "</tr>";
        
            
        }
        return withoutGroupData;
    }
    /* конвертирует данные компьютера */
    private static String ConvertComputerData(ComputersEntity comp)
    {

        String compData = "<td ><div cp=" + comp.ID + " name=" + comp.ComputerName + " comp=false >";
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
    public static String GetComputersData(int id)
    {
        System.Diagnostics.Debug.Write("GetComputersData:" + id);


        return ConvertComputerDataForClient(id,
            PolicyState.GetDevicesPoliciesByComputer(id)); ;
    }
    private static String ConvertComputerDataForClient(int id, List<DevicePolicy> list)
    {
        String table = "<table style='width:100% ' class='ListContrastTable' cp=" + id + "><thead class='gridViewHeader'><th style='text-align:center'>" +
        ResourceControl.GetStringForCurrentCulture("SerialNo") + "</th><th style='text-align:center'>" +
        ResourceControl.GetStringForCurrentCulture("Comment") + "</th><th style='text-align:center'>" +
        ResourceControl.GetStringForCurrentCulture("DeviceInsertedDate") + "</th><th style='text-align:center'>" +
        ResourceControl.GetStringForCurrentCulture("State") + "</th><th style='text-align:center'>" +
        ResourceControl.GetStringForCurrentCulture("Actions") + "</th></thead><tbody>";
        String cssStyle = "gridViewRow";
       
        foreach (DevicePolicy dp in list)
        {
            String row = "<tr style='text-align:center' class='"+cssStyle+"'><td>" + Anchor.FixString(dp.Device.SerialNo, 30) + "</td>";
            String comment = dp.Device.Comment;
            if (String.IsNullOrEmpty(comment))
                comment = Anchor.GetCommentFromSerial(dp.Device.SerialNo);

            row += "<td type='comment' dp=" + dp.Device.ID + " >" + comment + "</td>";
            //row += "<td>" + (dp.LatestInsert==DateTime.MinValue?"":dp.LatestInsert.ToString()) + "</td>";
            row += "<td style='width:60px'>" + dp.LatestInsert ?? "" + "</td>";

            String select = "<img style='cursor:pointer' dp=" + dp.Device.ID + " cp=" + id + " state=";
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
            row += "<td><img  style='cursor:pointer' title='" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "' comdp=" + dp.Device.ID + " serialdp=" + dp.Device.SerialNo + " src=\'App_Themes/Main/Images/table_edit.png\' />&nbsp;&nbsp;<img style='cursor:pointer' title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' deldp=" + dp.ID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
            row += "</tr>";
            table += row;
            if (cssStyle == "gridViewRow")
                cssStyle = "gridViewRowAlternating";
            else cssStyle = "gridViewRow";
        }
        table += "</table>";
        String text = "<div><input type=text dpc=" + id + " style='width:550px'></input>";
        String button = "<button style='width:auto' dpc=" + id + ">" + ResourceControl.GetStringForCurrentCulture("Add") + "</button></div>";
        /*   if (list.Count == 0)
               table = "";*/
        return table + text + button;
    }
    /*изменение состояния */
    [WebMethod]
    public static void ChangeDevicePolicyStateComputer(int dp, int cp, String state)// deviceId,compId,state
    {
        System.Diagnostics.Debug.Write("ChangeDevicePolicyState with id:" + dp + ", state:" + state);


        PolicyState.ChangeDevicePolicyStatusForComputer((Int16)dp, (Int16)cp, state);


    }
    [WebMethod]
    /* изменение комментария*/
    public static String GetChangeCommentDialog(int id)
    {
        Device device = PolicyState.GetDevice(id);
        if (String.IsNullOrEmpty(device.Comment))
            device.Comment = Anchor.GetCommentFromSerial(device.SerialNo);
        String label = "<div>" + ResourceControl.GetStringForCurrentCulture("DeviceComment") + "</div>";
        String text = "<input type=text dcdpc=" + id + " style='width:450px' value='" + device.Comment + "'></input>";
        String button = "<button dcdpc=" + id + ">" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "</button>";

        return label + text + button;
    }
    [WebMethod]
    public static void ChangeComment(int id, String comment)
    {
        Device device = new Device();
        device.ID = id;
        device.Comment = comment;

        PolicyState.EditDevice(device);
    }
    /* добавление нового устройства к компьютеру*/
    [WebMethod]
    public static String AddNewDevicePolicyToComputer(short id, String serial)
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
        else return "{\"success\":\"false\"}";
    }
    private static String ConvertDevicePolicy(DevicePolicy dp)
    {
        return "{\"success\":\"true\", \"deviceID\":\"" + dp.Device.ID + "\", \"comment\":\"" + dp.Device.Comment + "\",\"policyID\":\"" + dp.ID + "\"}";
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
    public static /*List<DevicePolicy>*/ String GetGroupDeviceData(int id)
    {

        System.Diagnostics.Debug.Write("GetGroupDeviceData" + id);
        if (id < 0)
            return ConvertGroupDataForClient(id, PolicyState.GetDevicesPoliciesWithoutGroup());
        else
            return ConvertGroupDataForClient(id, PolicyState.GetDevicesPoliciesByGroup(id));
    }

    private static String ConvertGroupDataForClient(int groupID, List<DevicePolicy> list)
    {
        String table = "<table style='width:100% text-align:center !important' class='ListContrastTable' gdp=" + groupID + "><thead><th></th><th style='text-align:center'>" +
       ResourceControl.GetStringForCurrentCulture("SerialNo") + "</th><th style='text-align:center'>" +
       ResourceControl.GetStringForCurrentCulture("Comment") + "</th><th style='text-align:center'>" +
       ResourceControl.GetStringForCurrentCulture("ComputerName") + "</th><th style='text-align:center'>" +
       ResourceControl.GetStringForCurrentCulture("DeviceInsertedDate") + "</th><th style='text-align:center'>" +
       ResourceControl.GetStringForCurrentCulture("State") + "</th><th style='text-align:center'>" +
       ResourceControl.GetStringForCurrentCulture("Actions") + "</th></thead><tbody>";
        String all = "";
        foreach (DevicePolicy dp in list)
        {
            all = "";
            if (dp.Device.LastComputer == "0")
            {
                all = "<img nfadp=" + dp.Device.ID + " src=\'App_Themes/Main/Images/notForAll.gif \' />";
            }
            String row = "<tr style='text-align:center'><td>" + all + "</td><td>" + Anchor.FixString(dp.Device.SerialNo, 30) + "</td>";
            String comment = dp.Device.Comment;
            if (String.IsNullOrEmpty(comment))
                comment = Anchor.GetCommentFromSerial(dp.Device.SerialNo);

            row += "<td type='comment' dp=" + dp.Device.ID + ">" + comment + "</td>";
            row += "<td>" + dp.Computer.ComputerName + "</td>";
            //row += "<td>" + (dp.LatestInsert==DateTime.MinValue?"":dp.LatestInsert.ToString()) + "</td>";
            row += "<td style='width:60px'>" + dp.LatestInsert ?? "" + "</td>";

            String select = "<img style='cursor:pointer' dp= " + dp.Device.ID + " gdp=" + groupID + " state=";
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
            row += "<td><img style='cursor:pointer' title='" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "' comdp=" + dp.Device.ID + " serialdp=" + dp.Device.SerialNo + " src=\'App_Themes/Main/Images/table_edit.png\' />";
            if (groupID >= 0)
                row += "<img style='cursor:pointer' title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' delgroupid=" + groupID + " delgroupdevid=" + dp.Device.ID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
            else row += "<img style='cursor:pointer' title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' delwithoutgroupdevid=" + dp.Device.ID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
            row += "</tr>";
            table += row;
        }
        table += "</table>";
        String text = "<div><input type=text dgr=" + groupID + " style='width:500px'></input>";
        String button = "<button style='width:auto' dgr=" + groupID + ">" + ResourceControl.GetStringForCurrentCulture("Add") + "</button></div>";

        return table + text + button;
    }
    [WebMethod]
    public static void ChangeDevicePolicyStateGroup(int dp, int gp, String state)//device,group,state
    {
        System.Diagnostics.Debug.Write("ChangeDevicePolicyStateForGroup with Id:" + dp + ", state:" + state);
        if (gp <= 0) PolicyState.ChangeDevicePolicyStatusWithoutGroup((Int16)dp, state);
        else
            PolicyState.ChangeDevicePolicyStatusForGroup((Int16)dp, gp, state);
    }
    //добавление устройства в группу
    [WebMethod]
    public static String AddNewDevicePolicyGroup(int id, String serial)
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

        String row = newDevice.ID + " " + newDevice.Comment + " " + newDevice.SerialNo;

        return ConvertDevice(newDevice);
    }

    private static String ConvertDevice(Device device)
    {
        if (device.ID == 0)
            return "{\"success\":\"false\"}";
        return "{\"success\":\"true\",\"id\":\"" + device.ID + "\",\"serial\":\"" + device.SerialNo + "\",\"comment\":\"" + device.Comment + "\"}";
    }
    /* удаление устройства из группы*/
    [WebMethod]
    public static void RemoveDevicePolicyGroup(int devid, int groupid)
    {
        PolicyState.RemoveDevicePolicyGroup(devid, groupid);
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
    public static String GetAllDevices()
    {
        List<Device> DevicesList = PolicyState.GetDevicesList();
        return ConvertDevicesList(DevicesList);
    }
    private static String ConvertDevicesList(List<Device> list)
    {
        String table = "<table style='width:100% text-align:left !important' class='ListContrastTable' ><thead><th style='text-align:left'>" +
        ResourceControl.GetStringForCurrentCulture("SerialNo") + "</th><th style='text-align:left'>" +
        ResourceControl.GetStringForCurrentCulture("Comment") + "</th><th style='text-align:left'>" +
        ResourceControl.GetStringForCurrentCulture("ComputerName") + "</th><th style='text-align:left'>" +
        ResourceControl.GetStringForCurrentCulture("DeviceInsertedDate") + "</th><th style='text-align:left'>" +
        ResourceControl.GetStringForCurrentCulture("Actions") + "</th></thead><tbody>";
        foreach (Device dev in list)
        {
            String row = "<tr style='text-align:left' dev=" + dev.ID + "><td>" + Anchor.FixString(dev.SerialNo, 30) + "</td>";
            String comment = dev.Comment;
            if (String.IsNullOrEmpty(comment))
                comment = Anchor.GetCommentFromSerial(dev.SerialNo);

            row += "<td dp=" + dev.ID + " type='comment'>" + comment + "</td>";
            row += "<td>" + dev.LastComputer + "</td>";
            row += "<td>" + dev.LastInserted + "</td>";
            row += "<td style='text-align:center'><img  style='cursor:pointer' title='" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "' comdp=" + dev.ID + " serialdp=" + dev.SerialNo + " src=\'App_Themes/Main/Images/table_edit.png\' /> <img style='cursor:pointer' title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' delete=true dcp=" + dev.ID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";

            row += "</tr>";
            table += row;
        }
        table += "</tbody></table>";
        return table;
    }

    [WebMethod]
    public static void DeleteDevice(int id)
    {

        Device device = new Device();
        device.ID = id;
        PolicyState.DeleteDevice(device);
    }
    #endregion

    public static void GetDictionaryOfGroups(Group? root)
    {


        GroupProvider provider = GroupState;
        List<Group> groupList = provider.GetSubgroups(root);
        foreach (Group group in groupList)
        {
            GroupDictionary.Add(group.ID, group);
            GetDictionaryOfGroups(group);
        }
    }

    public static BranchOfTree GetBranchOfTree(Group? root)
    {
        GroupProvider provider = GroupState;
        BranchOfTree tree;
        if (root == null) tree = new BranchOfTree();
        else tree = new BranchOfTree((Group)root);
        List<Group> groupList = provider.GetSubgroups(root);

        foreach (Group group in groupList)
        {
            BranchOfTree branch = GetBranchOfTree(group);
            tree.AddBranch(branch);

        }

        return tree;
    }
    private static int CompareChildrenBranchesByID(BranchOfTree br1, BranchOfTree br2)
    {
        return br1.Root.ID.CompareTo(br2.Root.ID);
    }
    public static BranchOfTree GetBranchOfTreeByDevice(List<DevicePolicy> compList)
    {

        BranchOfTree tree = new BranchOfTree();
        foreach (DevicePolicy dp in compList)
        {
            int GroupID = dp.Device.ID;

            Group group = GroupDictionary[GroupID];
            String rootName = group.Name;
            BranchOfTree branch = new BranchOfTree();
            branch.Root = group;
            branch.AddComputer(dp.Computer);
            while (!tree.IsRootExist(rootName))
            {

                int? parentGroupID = group.ParentID;
                if (parentGroupID != null)
                {
                    group = GroupDictionary[(int)parentGroupID];
                    rootName = group.Name;
                    BranchOfTree newBranch = new BranchOfTree();
                    newBranch.Root = group;
                    newBranch.AddBranch(branch);
                    branch = newBranch;
                    /*  branch.ChildrenBranchs.Sort(CompareChildrenBranchesByID);*/
                }
                else break;

            }

            tree.AddBranch(branch);

        }
        /*  tree.ChildrenBranchs.Sort(CompareChildrenBranchesByID);*/
        return tree;

    }
    /* Дерево с компьютерами для устройства */
    [WebMethod]
    public static String GetDeviceTreeDialog(int id, String serial)
    {
        GroupProvider provider = GroupState;
        Device device = new Device();
        device.ID = id;
        List<DevicePolicy> compList = PolicyState.GetComputerListByDeviceID(device);
        BranchOfTree tree = GetBranchOfTreeByDevice(compList);
        String treeDialog = ConvertDeviceTreeDialog(compList, tree, id);
        String addButton = "<button addcompdev='" + serial + "'>" + Resources.Resource.Add + "</button>";
        return treeDialog + addButton;
    }

    private static String ConvertDeviceTreeDialog(List<DevicePolicy> compList, BranchOfTree tree, int DeviceID)
    {
        String treeDialog = "";
        treeDialog = "<div id='treeAccordion' treeacc=true>";
        foreach (BranchOfTree branch in tree.ChildrenBranchs)
        {
            treeDialog += ConvertDeviceBranchOfTree(compList, branch, DeviceID);
        }
        if (tree.Computers.Count > 0)
            treeDialog += ConvertComputersWithoutGroupBranch(compList, tree, DeviceID);
        treeDialog += "</div>";
        return treeDialog;
    }

    private static String ConvertComputersWithoutGroupBranch(List<DevicePolicy> compList, BranchOfTree tree, int DeviceID)
    {
        String branchString = "";
        branchString += "<h3 treetabledevID=-1 treetableID=" + DeviceID + ">" + Resources.Resource.ComputersWithoutGroups + "</h3>";
        branchString += "<div treetabledevID=-1>";
        branchString += "<table treetabledevID=-1 width='100%' class='ListContrastTable'>";
        String cssStyle = "gridViewRow";
        foreach (ComputersEntity comp in tree.Computers)
        {
            if (cssStyle == "gridViewRow")
                cssStyle = "gridViewRowAlternating";
            else cssStyle = "gridViewRow";
            branchString += ConvertDeviceCompOfTree(compList, comp, DeviceID,cssStyle);
        }

        branchString += "</table>";
        branchString += " </div>";

        return branchString;
    }

    private static String ConvertDeviceBranchOfTree(List<DevicePolicy> compList, BranchOfTree tree, int DeviceID)
    {
        String branchString = "";
        branchString += "<h3 treetableID=" + DeviceID + " treetabledevID=" + tree.Root.ID + ">" + tree.Root.Name + "</h3>";
        branchString += "<div treetabledevID=" + tree.Root.ID + ">";
        branchString += "<table width='100%' class='ListContrastTable' treetabledevID=" + tree.Root.ID + ">";


        if (tree.ChildrenBranchs.Count > 0)
        {
            branchString += "<tr ><td colSpan=4 width='100%'>";
            branchString += "<div  id='treeAccordion_" + tree.Root.ID + "' treeacc=true>";
            foreach (BranchOfTree branch in tree.ChildrenBranchs)
            {

                branchString += ConvertDeviceBranchOfTree(compList, branch, DeviceID);


            }
            branchString += "</div></td></tr>";
        }
        String cssStyle ="gridViewRow";
       
        foreach (ComputersEntity comp in tree.Computers)
        {
            if (cssStyle == "gridViewRow")
                cssStyle = "gridViewRowAlternating";
            else cssStyle = "gridViewRow";
            branchString += ConvertDeviceCompOfTree(compList, comp, DeviceID,cssStyle);
        }

        branchString += "</table>";
        branchString += " </div>";

        return branchString;

    }

    private static String ConvertDeviceCompOfTree(List<DevicePolicy> compList, ComputersEntity comp, Int32 DeviceID,String cssStyle)
    {
        DevicePolicy dp = compList.Find(
            delegate(DevicePolicy dev)
            {
                return (dev.Computer.ID == comp.ID);
            }
        );

        String compString = "";
        compString += "<tr treedp='" + dp.ID + "' class='"+cssStyle+"'>";
        compString += "<td >" + comp.ComputerName + "</td>";
        compString += "<td >" + dp.LatestInsert + "</td>";
        String select = "<img style='cursor:pointer'  treestatedev=" + DeviceID + " treestatecp=" + comp.ID + " state=";
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
        compString += "<td >" + select + "</td>";
        compString += "<td><img title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' treedeldev=" + DeviceID + " treedeldp=" + dp.ID + " style='cursor:pointer' src=\'App_Themes/Main/Images/deleteicon.png\' />";
        compString += "</tr>";
        return compString;
    }

    [WebMethod]
    public static Boolean AddNewDevicePolicyByComputerList(String serial, String comps)
    {
        char[] sep = new char[1];
        sep[0] = '&';
        String[] compArray = comps.Split(sep, StringSplitOptions.RemoveEmptyEntries);
        serial = serial.Replace(" ", "");
        System.Diagnostics.Debug.Write("AddNewDevicePolicyByComputerList serial:" + serial + ", comps:" + comps);

        if (serial == null || serial == String.Empty)
            throw new Exception("1");

        Device device = new Device(serial, DeviceType.USB);

        /*   device = PoliciesState.AddDevice(device);*/
        Int16 id;
        ComputersEntity computer = new ComputersEntity();
        Boolean isSuccess = false;
        foreach (String c in compArray)
        {
            id = Convert.ToInt16(c);
            computer.ID = id;
            DevicePolicy dp = new DevicePolicy(device, computer);
            dp.State = DevicePolicyState.Undefined;
            DevicePolicy policy = PolicyState.AddDevicePolicyToComputer(dp);
            if (policy.Device.ID != 0) isSuccess = true;

        }
        return isSuccess;
    }

    [WebMethod]
    public static void ActionDevice(Int32 id, String computerName, String action)
    {
        DevicePolicy dp = new DevicePolicy();
        dp.ID = id;
        dp.Computer = new ComputersEntity();
        dp.Computer.ComputerName = computerName;

        if (action == "allow")
            dp.State = DevicePolicyState.Enabled;
        else
            dp.State = DevicePolicyState.Disabled;

        PolicyState.ChangeDevicePolicyStatusForComputer(dp);
    }

    [WebMethod]
    public static void ActionForAllDevices(String action, String devpolicies, String computerNames)
    {
        String[] comps = computerNames.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        String[] devices = devpolicies.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        for (Int32 index = 0; index < devices.Length; index++)
        {
            ActionDevice(Convert.ToInt32(devices[index]), comps[index], action);
        }
    }

    public void DeleteDeviceFromPanel(object sender, EventArgs e)
    {
        ImageButton button = (ImageButton)sender;
        int id = Convert.ToInt32(button.Attributes["deldev"]);
        DeleteDevice(id);
        GridView1.DataBind();
        updatePanelDevicesGrid.Update();
    }

    public void UpdatePanelReload(object sender, EventArgs e)
    {
        GridView2.DataBind();
    }

    protected void DeviceFilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    {
        GridView1.PageIndex = 0;
        GridView1.Where = e.Where;
        GridView1.DataBind();
    }

    protected void UnknownDeviceFilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    {
        GridView2.PageIndex = 0;
        GridView2.Where = e.Where;
        GridView2.DataBind();
    }
}