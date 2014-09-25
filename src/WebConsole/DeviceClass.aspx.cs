using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using System.Configuration;
using System.Text;
using VirusBlokAda.CC.DataBase;
using VirusBlokAda.CC.Filters.Common;

public partial class DeviceClassPage : PageBase
{
    #region Properties

    private static Dictionary<Int32, Group> GroupDictionary;

    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterScripts();
        ResourceRegister(GetResourcesList());
        InitializeGroupBranch();

        if (!IsPostBack)
        {
            InitFields();
        }

        Controls_PagerUserControl.AddGridViewExtendedAttributes(GridView1, ObjectDataSource1);
    }

    #region Initializing

    protected override void InitFields()
    {
        GridView1.EmptyDataText = Resources.Resource.EmptyMessage;
        fltMode.DataSource = DBProviders.Policy.GetDeviceClassModeList();
        fltMode.DataBind();
    }

    private void RegisterScripts()
    {
        RegisterScript(@"js/jstree.js");
        RegisterScript(@"js/jQuery/jquery.cookie.js");
        RegisterScript(@"js/DeviceClass.js");
        RegisterScript(@"js/json2.js");
        RegisterScript(@"js/jQuery/jquery.loadmask.js");
        RegisterScript(@"js/DeviceClass_Tree.js");
        RegisterScript(@"js/PageRequestManagerHelper.js");
    }

    private String[] GetResourcesList()
    {
        return new String[] {"Loading", "ChangeComment", "Delete", 
            "DeviceClass", "Error", "ErrorRequestingDataFromServer",
            "Computers", "Computers", "Apply", "ErrorFieldIsEmpty",
            "ClassName", "GuidRegexErrorMessage", "AddNewDeviceClass",
            "Enabled", "Disabled", "BlockWrite", "ErrorExistPolicy","NothingIsAdded"
        };
    }

    private static void InitializeGroupBranch()
    {
        GroupDictionary = new Dictionary<Int32, Group>();
        GetDictionaryOfGroups(null);
        GroupDictionary.Add(-1, new Group(-1, null, null, null));
    }

    public static void GetDictionaryOfGroups(Group? root)
    {
        foreach (Group group in DBProviders.Group.GetSubgroups(root))
        {
            GroupDictionary.Add(group.ID, group);
            GetDictionaryOfGroups(group);
        }
    }

    #endregion

    #region WebMethods

    [WebMethod]
    public static void AddDeviceClass(String uid, String className, String comment)
    {
        DBProviders.Policy.AddDeviceClass(new DeviceClass(uid, comment, className));
    }

    [WebMethod]
    public static void DeleteDeviceClass(String uid)
    {
        DBProviders.Policy.DeleteDeviceClass(new DeviceClass(uid));
    }

    [WebMethod]
    public static String GetDeviceTreeDialog(Int16 id, String uid)
    {
        List<DeviceClassPolicy> policyList = DBProviders.Policy.GetDeviceClassComputerList(new DeviceClass(id, String.Empty, String.Empty, String.Empty));
        String addButton = "<button addcompdev='" + id + "'>" + ResourceControl.GetStringForCurrentCulture("Add") + "</button>";
        return ConvertDeviceTreeDialog(policyList, GetBranchOfTreeByDevice(policyList), id, uid) + addButton;
    }

    [WebMethod]
    public static Boolean AddNewDeviceClassPolicyByComputerList(Int16 id, String comps)
    {
        String[] compArray = comps.Split(new Char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

        DeviceClassPolicy dp = new DeviceClassPolicy(
            new ComputersEntity(), 
            new DeviceClass(id, String.Empty, String.Empty, String.Empty),
            DeviceClassMode.Disabled);

        Boolean isSuccess = false;
        foreach (String comp in compArray)
        {
            dp.Computer.ID = Convert.ToInt16(comp);
            DeviceClassPolicy policy = DBProviders.Policy.AddDeviceClassPolicy(dp);
            if (policy.ID != 0) 
                isSuccess = true;
        }
        return isSuccess;
    }

    /* начальный список групп*/
    [WebMethod]
    public static String GetListRootGroup()
    {
        return ConvertListRootGroup() + ConvertGroupData(null);
    }

    /*конвертация начального списка*/
    [WebMethod]
    private static String ConvertListRootGroup()
    {
        StringBuilder groupListData = new StringBuilder();
        foreach(Group group in DBProviders.Group.GetSubgroups(null))
        {
            groupListData.Append(ConvertGroupData(group));
            groupListData.Append(Environment.NewLine);
        }
        return groupListData.ToString();
    }

    //получение данных группы по id
    [WebMethod]
    public static String GetData(String id)
    {
        StringBuilder str = new StringBuilder("{");
        if (String.IsNullOrEmpty(id))
        {
            str.Append(ConvertWithoutGroupData());
            str.Append("\"}");
            return str.ToString();
        }
        Int32 groupID = Convert.ToInt32(id);        
        Boolean isEmpty = true;
        str.Append(ConvertSubGroupData(groupID, out isEmpty));
        str.Append(ConvertCompListData(groupID, isEmpty));
        str.Append("\"}");
        return str.ToString();
    }

    /* получение данных о компьютере */
    [WebMethod]
    public static String GetComputersData(Int32 id)
    {
        ComputersEntity comp = new ComputersEntity();
        comp.ID = (Int16)id;
        return ConvertComputerDataForClient(id, DBProviders.Policy.GetDeviceClassPolicyList(comp));
    }

    /*изменение состояния */
    [WebMethod]
    public static void ChangeDevicePolicyStateComputer(Int16 devId, Int16 compId, String state)
    {
        ComputersEntity comp = new ComputersEntity();
        comp.ID=compId;
        DeviceClass device = new DeviceClass();
        device.ID=devId;
        if (DBProviders.Policy.ChangeModeToDeviceClassPolicy(new DeviceClassPolicy(comp, device, DeviceClassModeExtensions.Get(state))) == 0)
            throw new Exception("This policy is not exist.");
    }

    /* изменение комментария*/
    [WebMethod]
    public static String GetChangeCommentDialog(String uid, String comment)
    {
        DeviceClass dc = new DeviceClass(uid, comment, String.Empty);
        String label = "<div>" + ResourceControl.GetStringForCurrentCulture("DeviceClassComment") + "</div>";
        String text = "<input type=text ccuid=" + uid + " style='width:450px' value='" + dc.Class + "'></input>";
        String button = "<button ccuid=" + uid + ">" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "</button>";

        return label + text + button;
    }

    [WebMethod]
    public static void ChangeComment(String uid, String comment)
    {
        DBProviders.Policy.ChangeCommentToDeviceClass(new DeviceClass(uid, comment, String.Empty));
    }

    /* добавление нового устройства к компьютеру*/
    [WebMethod]
    public static String AddNewDeviceClassPolicyToComputer(Int16 id, String uid)
    {
        DeviceClassPolicy dp = new DeviceClassPolicy(new ComputersEntity(), new DeviceClass(), DeviceClassMode.Disabled);
        dp.Computer.ID = id;
        dp.ClassOfDevice = DBProviders.Policy.GetDeviceClass(uid);

        if(dp.ClassOfDevice == null)
            return "{\"success\":\"false\", \"info\":\"" + ResourceControl.GetStringForCurrentCulture("NoFoundUID") + ".\"}";
        
        dp = DBProviders.Policy.AddDeviceClassPolicy(dp);
        if (dp.ID != 0)
        {
            return ConvertDeviceClassPolicy(dp);
        }
        else 
            return "{\"success\":\"false\"}";
    }

    /* удаление устройства из компьютера*/
    [WebMethod]
    public static void RemoveDeviceClassPolicy(Int16 devId, Int16 compId)
    {
        ComputersEntity comp = new ComputersEntity();
        comp.ID = compId;
        DeviceClass devClass = new DeviceClass();
        devClass.ID = devId;
        DBProviders.Policy.DeleteDeviceClassPolicy(new DeviceClassPolicy(comp,devClass));
    }

    [WebMethod]
    public static String GetGroupDeviceData(Int32 id)
    {
        if (id < 0)
            return ConvertGroupDataForClient(id, DBProviders.Policy.GetDeviceClassPolicyList());
        else
            return ConvertGroupDataForClient(id, DBProviders.Policy.GetDeviceClassPolicyList(new Group(id, String.Empty, String.Empty, null)));
    }

    [WebMethod]
    public static void ChangeDeviceClassPolicyStateGroup(Int16 dp, Int32 gp, String state)//deviceClass,group,state
    {
        DeviceClassPolicy dcp = new DeviceClassPolicy();
        dcp.ClassOfDevice = new DeviceClass();
        dcp.ClassOfDevice.ID = dp;
        dcp.Mode = DeviceClassModeExtensions.Get(state);
        Group group = new Group();
        group.ID = gp;
        if (gp <= 0)
            DBProviders.Policy.ChangeModeToDeviceClassPolicyWithoutGroup(dcp);
        else
            DBProviders.Policy.ChangeModeToDeviceClassPolicy(dcp, group);
    }

    //добавление устройства в группу
    [WebMethod]
    public static String AddNewDeviceClassPolicyGroup(Int32 id, String uid)
    {
        DeviceClassPolicy dp = new DeviceClassPolicy();
        dp.ClassOfDevice = DBProviders.Policy.GetDeviceClass(uid);
        dp.Mode = DeviceClassMode.Disabled;

        if (dp.ClassOfDevice == null)
            return "{\"success\":\"false\", \"info\":\"" + ResourceControl.GetStringForCurrentCulture("NoFoundUID") + ".\"}";

        Group group = new Group();
        group.ID = id;

        Int32 count = 0;

        if (id <= 0)
            count = DBProviders.Policy.AddDeviceClassPolicyWithoutGroup(dp);
        else
            count = DBProviders.Policy.AddDeviceClassPolicy(dp, group);

        if(count == 0)
            return "{\"success\":\"true\", \"count\":\"0\"}";

        return ConvertDeviceClass(dp.ClassOfDevice);
    }

    /* удаление устройства из группы*/
    [WebMethod]
    public static void RemoveDeviceClassPolicyGroup(Int16 devid, Int32 groupid)
    {
        DBProviders.Policy.DeleteDeviceClassPolicy(
            new DeviceClass(devid, String.Empty, String.Empty, String.Empty),
            new Group(groupid, String.Empty, String.Empty, null));
    }

    [WebMethod]
    public static void RemoveDeviceClassPolicyWithoutGroup(Int16 id)
    {
        DBProviders.Policy.DeleteDeviceClassPolicyWithoutGroup(new DeviceClass(id, String.Empty, String.Empty, String.Empty));
    }

    #endregion

    #region Additional methods for WebMethod

    //конвертирует список компьютеров для группы с id
    private static String ConvertCompListData(Int32 groupID, Boolean isEmpty)
    {
        List<ComputersEntity> compList = DBProviders.Group.GetComputersByGroup(groupID);
        if (compList.Count == 0)
        {
            if (isEmpty)
                return "<tr class='gridViewRowAlternating'><td style='text-align:center'>" + ResourceControl.GetStringForCurrentCulture("GroupHasNotComps") + "</td></tr>";
            else 
                return "<tr><td></td></tr>";
        }
        String compListData = "";
        String cssStyle = "";
        for (Int32 i = 0; i < compList.Count; i++)
        {
            cssStyle = (i % 2 == 0) ? "gridViewRow" : "gridViewRowAlternating";
            compListData += "<tr style='cursor: pointer' class='" + cssStyle + "'>" + ConvertComputerData(compList[i]) + "</tr>";
        }

        return compListData;
    }

    //конвертирует список подгрупп для группы с id
    private static String ConvertSubGroupData(Int32 groupID, out Boolean isEmpty)
    {
        isEmpty = true;
        StringBuilder subGroupData = new StringBuilder("\"acc\":\"");
        List<Group> groupList = DBProviders.Group.GetSubgroups(groupID);
        if (groupList.Count > 0)
        {
            isEmpty = false;
            subGroupData.Append("false\",\"text\":\"");
            subGroupData.Append("<tr width='100%'><td colspan='2'><div acc=\'false\' id=\'accordion" + groupID.ToString() + "\'>");
            foreach(Group group in groupList)
            {
                subGroupData.Append(ConvertGroupData(group));
            }
            subGroupData.Append("</div></td></tr>");
        }
        else
        {
            isEmpty = true;
            subGroupData.Append("null\",\"text\":\"");
        }

        return subGroupData.ToString();
    }

    //конвертирует содержание группы
    private static String ConvertGroupData(Group? group)
    {
        StringBuilder groupData = new StringBuilder();
        if (group == null)
        {
            groupData.Append("<h3 id=null acc=null load=false >");
            groupData.Append("<a style='font-size:10pt !important; '>");
            groupData.AppendFormat("<span id=null >{0}</span>", ResourceControl.GetStringForCurrentCulture("ComputersWithoutGroups"));
        }
        else
        {
            Group gr = (Group)group;
            groupData.AppendFormat("<h3  id='{0}' acc=null load=false><a style='font-size:10pt !important'>", gr.ID);
            groupData.AppendFormat("<span>{0}</span>", gr.Name);
        }
        groupData.Append("</a></h3>");
        groupData.Append("<div><table width='100%' class='ListContrastTable'></table></div>");
        return groupData.ToString();
    }

    //без групп
    private static String ConvertWithoutGroupData()
    {
        List<ComputersEntity> compList = DBProviders.Group.GetComputersWithoutGroup();
        StringBuilder withoutGroupData = new StringBuilder("\"acc\":\"null\",\"text\":\"");
        if (compList.Count == 0)
        {
            withoutGroupData.AppendFormat("<tr class='gridViewRowAlternating'><td style='text-align:center'>{0}</td></tr>", ResourceControl.GetStringForCurrentCulture("GroupHasNotComps"));
            return withoutGroupData.ToString();
        }
        String cssStyle = "";
        for (Int32 i = 0; i < compList.Count; i++)
        {
            cssStyle = (i % 2 == 0) ? "gridViewRow" : "gridViewRowAlternating";
            withoutGroupData.AppendFormat("<tr style='cursor: pointer; font-size:10pt !important' class='{0}'>{1}</tr>", cssStyle, ConvertComputerData(compList[i]));
        }
        return withoutGroupData.ToString();
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

    private static String ConvertComputerDataForClient(Int32 id, List<DeviceClassPolicy> list)
    {
        String table = "<table style='width:100% ' class='ListContrastTable' cp=" + id + "><thead class='gridViewHeader'><th style='text-align:center'>" +
            ResourceControl.GetStringForCurrentCulture("ClassName") + "</th><th style='text-align:center'>" +
            "UID" + "</th><th style='text-align:center'>" +
            ResourceControl.GetStringForCurrentCulture("Comment") + "</th><th style='text-align:center'>" +            
            ResourceControl.GetStringForCurrentCulture("State") + "</th><th style='text-align:center'>" +
            ResourceControl.GetStringForCurrentCulture("Actions") + "</th></thead><tbody>";
        String cssStyle = "gridViewRow";

        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(VirusBlokAda.CC.Common.RegularExpressions.GUID);

        foreach (DeviceClassPolicy dp in list)
        {
            String row = "<tr style='text-align:center' class='" + cssStyle + "'><td>" + dp.ClassOfDevice.ClassName + "</td>";

            row += "<td style='width:60px' uidtd>" + dp.ClassOfDevice.UID + "</td>";
            row += "<td><span class='main' uid='" + dp.ClassOfDevice.UID + "' type='comment'>" + dp.ClassOfDevice.Class + "</span></td>";

            String select = "<img style='cursor:pointer' dp=" + dp.ClassOfDevice.ID + " cp=" + id + (!reg.IsMatch(dp.ClassOfDevice.UID) ? " IsUsbClass=true" : " IsUsbClass=false") + " state=";
            switch (dp.Mode)
            {
                case DeviceClassMode.Undefined:
                    select += "Undefined src=\'App_Themes/Main/Images/undefined.gif\' />";
                    break;
                case DeviceClassMode.Enabled:
                    select += "Enabled src=\'App_Themes/Main/Images/enabled.gif\' />";
                    break;
                case DeviceClassMode.Disabled:
                    select += "Disabled src=\'App_Themes/Main/Images/disabled.gif\' />";
                    break;
                case DeviceClassMode.BlockWrite:
                    select += "BlockWrite src=\'App_Themes/Main/Images/BlockWrite.gif\' />";
                    break;
            }
            row += "<td>" + select + "</td>";
            row += "<td><img  style='cursor:pointer' title='" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "' ccUID='" + dp.ClassOfDevice.UID + "' comment='" + dp.ClassOfDevice.Class + "' src=\'App_Themes/Main/Images/table_edit.png\' />&nbsp;&nbsp;";
            row += "<img style='cursor:pointer' title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' deldp=" + dp.ClassOfDevice.ID + " compId=" + id + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
            row += "</tr>";
            table += row;
            cssStyle = (cssStyle == "gridViewRow") ? "gridViewRowAlternating" : "gridViewRow";
        }
        table += "</table>";
        String text = "<div>" + ResourceControl.GetStringForCurrentCulture("InputUID") + ": <input type=text dpc=" + id + " style='width:350px'></input>   ";
        String button = "<button style='width:auto' dpc=" + id + ">" + ResourceControl.GetStringForCurrentCulture("Add") + "</button></div>";
        
        return table + text + button;
    }

    private static String ConvertDeviceClassPolicy(DeviceClassPolicy dp)
    {
        return "{\"success\":\"true\", \"id\":\"" + dp.ClassOfDevice.ID + "\", \"uid\":\"" + dp.ClassOfDevice.UID + "\", \"comment\":\"" + dp.ClassOfDevice.Class + "\",\"className\":\"" + dp.ClassOfDevice.ClassName + "\"}";
    }

    private static String ConvertGroupDataForClient(Int32 groupID, List<DeviceClassPolicy> list)
    {
        String table = "<table style='width:100% ' class='ListContrastTable' gdp=" + groupID + "><thead class='gridViewHeader'><th></th><th style='text-align:center'>" +
            ResourceControl.GetStringForCurrentCulture("ClassName") + "</th><th style='text-align:center'>" +
            "UID" + "</th><th style='text-align:center'>" +
            ResourceControl.GetStringForCurrentCulture("Comment") + "</th><th style='text-align:center'>" +
            ResourceControl.GetStringForCurrentCulture("State") + "</th><th style='text-align:center'>" +
            ResourceControl.GetStringForCurrentCulture("Actions") + "</th></thead><tbody>";
        String cssStyle = "gridViewRow";
        String all;
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(VirusBlokAda.CC.Common.RegularExpressions.GUID);
        foreach (DeviceClassPolicy dp in list)
        {
            all = "";
            if (dp.ID == 0)
            {
                all = "<img nfadp=" + dp.ClassOfDevice.ID + " src=\'App_Themes/Main/Images/notForAll.gif \' title='" + ResourceControl.GetStringForCurrentCulture("ApplyNotForAll") + "' />";
            }
            String row = "<tr style='text-align:center' class='" + cssStyle + "'><td>" + all + "</td><td>" + dp.ClassOfDevice.ClassName + "</td>";
            row += "<td style='width:60px' uidtd>" + dp.ClassOfDevice.UID + "</td>";
            row += "<td><span class='main' uid='" + dp.ClassOfDevice.UID + "' type='comment'>" + dp.ClassOfDevice.Class + "</span></td>";
                        
            String select = "<img style='cursor:pointer' dp= " + dp.ClassOfDevice.ID + " gdp=" + groupID + (!reg.IsMatch(dp.ClassOfDevice.UID) ? " IsUsbClass=true" : " IsUsbClass=false") + " state=";
            switch (dp.Mode)
            {
                case DeviceClassMode.Undefined:
                    select += "Undefined src=\'App_Themes/Main/Images/undefined.gif\' />";
                    break;
                case DeviceClassMode.Enabled:
                    select += "Enabled src=\'App_Themes/Main/Images/enabled.gif\' />";
                    break;
                case DeviceClassMode.Disabled:
                    select += "Disabled src=\'App_Themes/Main/Images/disabled.gif\' />";
                    break;
                case DeviceClassMode.BlockWrite:
                    select += "BlockWrite src=\'App_Themes/Main/Images/BlockWrite.gif\' />";
                    break;
            }
            row += "<td>" + select + "</td>";
            row += "<td><img style='cursor:pointer' title='" + ResourceControl.GetStringForCurrentCulture("ChangeComment") + "' ccUID='" + dp.ClassOfDevice.UID + "' comment='" + dp.ClassOfDevice.Class + "' src=\'App_Themes/Main/Images/table_edit.png\' />&nbsp;&nbsp;";
            if (groupID >= 0)
                row += "<img style='cursor:pointer' title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' delgroupid=" + groupID + " delgroupdevid=" + dp.ClassOfDevice.ID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
            else 
                row += "<img style='cursor:pointer' title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' delwithoutgroupdevid=" + dp.ClassOfDevice.ID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
            row += "</tr>";
            table += row;
            cssStyle = (cssStyle == "gridViewRow") ? "gridViewRowAlternating" : "gridViewRow";
        }
        table += "</table>";
        String text = "<div>" + ResourceControl.GetStringForCurrentCulture("InputUID") + ": <input type=text dgr=" + groupID + " style='width:350px'></input>   ";
        String button = "<button style='width:auto' dgr=" + groupID + ">" + ResourceControl.GetStringForCurrentCulture("Add") + "</button></div>";

        return table + text + button;
    }

    private static String ConvertDeviceClass(DeviceClass device)
    {
        if (device.ID == 0)
            return "{\"success\":\"false\"}";
        return "{\"success\":\"true\", \"count\":\"1\", \"id\":\"" + device.ID + "\", \"uid\":\"" + device.UID + "\", \"comment\":\"" + device.Class + "\",\"className\":\"" + device.ClassName + "\"}";
    }

    private static String ConvertDeviceTreeDialog(List<DeviceClassPolicy> compList, BranchOfTree tree, Int16 DeviceClassID, String DeviceClassUID)
    {
        StringBuilder treeDialog = new StringBuilder("<div id='treeAccordion' treeacc=true>");
        foreach (BranchOfTree branch in tree.ChildrenBranchs)
        {
            treeDialog.Append(ConvertDeviceBranchOfTree(compList, branch, DeviceClassID, DeviceClassUID));
        }

        if (tree.Computers.Count > 0)
            treeDialog.Append(ConvertComputersWithoutGroupBranch(compList, tree, DeviceClassID, DeviceClassUID));

        treeDialog.Append("</div>");

        return treeDialog.ToString();
    }

    private static String ConvertComputersWithoutGroupBranch(List<DeviceClassPolicy> compList, BranchOfTree tree, Int16 DeviceClassID, String DeviceClassUID)
    {
        String branchString = "<h3 treetabledevID=-1 treetableID=" + DeviceClassID + ">" + ResourceControl.GetStringForCurrentCulture("ComputersWithoutGroups") + "</h3>";
        branchString += "<div treetabledevID=-1>";
        branchString += "<table treetabledevID=-1 width='100%' class='ListContrastTable'>";
        String cssStyle = "gridViewRow";
        foreach (ComputersEntity comp in tree.Computers)
        {
            cssStyle = (cssStyle == "gridViewRow") ? "gridViewRowAlternating" : "gridViewRow";
            branchString += ConvertDeviceCompOfTree(compList, comp, DeviceClassID, DeviceClassUID, cssStyle);
        }
        branchString += "</table></div>";

        return branchString;
    }

    private static String ConvertDeviceBranchOfTree(List<DeviceClassPolicy> compList, BranchOfTree tree, Int16 DeviceClassID, String DeviceClassUID)
    {
        String branchString = "<h3 treetableID=" + DeviceClassID + " treetabledevID=" + tree.Root.ID + ">" + tree.Root.Name + "</h3>";
        branchString += "<div treetabledevID=" + tree.Root.ID + ">";
        branchString += "<table width='100%' class='ListContrastTable' treetabledevID=" + tree.Root.ID + ">";

        if (tree.ChildrenBranchs.Count > 0)
        {
            branchString += "<tr ><td colSpan=4 width='100%'>";
            branchString += "<div  id='treeAccordion_" + tree.Root.ID + "' treeacc=true>";
            foreach (BranchOfTree branch in tree.ChildrenBranchs)
            {
                branchString += ConvertDeviceBranchOfTree(compList, branch, DeviceClassID, DeviceClassUID);
            }
            branchString += "</div></td></tr>";
        }

        String cssStyle = "gridViewRow";
        foreach (ComputersEntity comp in tree.Computers)
        {
            cssStyle = (cssStyle == "gridViewRow") ? "gridViewRowAlternating" : "gridViewRow";
            branchString += ConvertDeviceCompOfTree(compList, comp, DeviceClassID, DeviceClassUID, cssStyle);
        }
        branchString += "</table>";
        branchString += " </div>";

        return branchString;
    }

    private static String ConvertDeviceCompOfTree(List<DeviceClassPolicy> compList, ComputersEntity comp, Int16 DeviceClassID, String DeviceClassUID, String cssStyle)
    {
        DeviceClassPolicy dp = compList.Find(
            delegate(DeviceClassPolicy dev)
            {
                return (dev.Computer.ID == comp.ID);
            }
        );

        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(VirusBlokAda.CC.Common.RegularExpressions.GUID);
        String compString = "<tr treedp='" + dp.ID + "' class='" + cssStyle + "'>";
        compString += "<td >" + comp.ComputerName + "</td>";
        String select = "<img style='cursor:pointer'  treestatedev=" + DeviceClassID + " treestatecp=" + comp.ID + (!reg.IsMatch(DeviceClassUID) ? " IsUsbClass=true" : " IsUsbClass=false") + " state=";
        switch (dp.Mode)
        {
            case DeviceClassMode.Undefined:
                select += "Enabled src=\'App_Themes/Main/Images/undefined.gif\' title='" + ResourceControl.GetStringForCurrentCulture("Undefined") + "' />";
                break;
            case DeviceClassMode.Enabled:
                select += "Enabled src=\'App_Themes/Main/Images/enabled.gif\' title='" + ResourceControl.GetStringForCurrentCulture("Enabled") + "' />";
                break;
            case DeviceClassMode.Disabled:
                select += "Disabled src=\'App_Themes/Main/Images/disabled.gif\' title='" + ResourceControl.GetStringForCurrentCulture("Disabled") + "' />";
                break;
            case DeviceClassMode.BlockWrite:
                select += "BlockWrite src=\'App_Themes/Main/Images/BlockWrite.gif\' title='" + ResourceControl.GetStringForCurrentCulture("BlockWrite") + "' />";
                break;
        }
        compString += "<td >" + select + "</td>";
        compString += "<td><img title='" + ResourceControl.GetStringForCurrentCulture("Delete") + "' treedeldev=" + DeviceClassID +
            " treedelcp=" + dp.Computer.ID + " style='cursor:pointer' src=\'App_Themes/Main/Images/deleteicon.png\' />";
        compString += "</tr>";

        return compString;
    }

    public static BranchOfTree GetBranchOfTreeByDevice(List<DeviceClassPolicy> compList)
    {
        BranchOfTree tree = new BranchOfTree();
        foreach (DeviceClassPolicy dp in compList)
        {
            Int32 GroupID = dp.ID;
            Group group = GroupDictionary[GroupID];
            String rootName = group.Name;
            BranchOfTree branch = new BranchOfTree(group);
            branch.AddComputer(dp.Computer);
            while (!tree.IsRootExist(rootName))
            {
                Int32? parentGroupID = group.ParentID;
                if (parentGroupID != null)
                {
                    group = GroupDictionary[(Int32)parentGroupID];
                    rootName = group.Name;
                    BranchOfTree newBranch = new BranchOfTree(group);
                    newBranch.AddBranch(branch);
                    branch = newBranch;
                }
                else break;
            }
            tree.AddBranch(branch);
        }
        return tree;
    }

    #endregion
    
    protected void DeleteDeviceFromPanel(object sender, EventArgs e)
    {
        DeleteDeviceClass((sender as ImageButton).Attributes["uid"]);
        GridView1.DataBind();
    }

    protected void UpdateGridView(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
    
    protected void DeviceClassFilterContainer_ActiveFilterChanged(object sender, FilterEventArgs e)
    {
        GridView1.PageIndex = 0;
        GridView1.Where = e.Where;
        GridView1.DataBind();
    }

    protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(VirusBlokAda.CC.Common.RegularExpressions.GUID);
            (e.Row.Cells[3].FindControl("ibtnDelete") as ImageButton).Visible = reg.IsMatch((e.Row.DataItem as DeviceClass).UID);
        }
    }
}