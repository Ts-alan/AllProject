<%@ Page Language="C#" MaintainScrollPositionOnPostback="true"  validateRequest="false" MasterPageFile="~/mstrPageNew.master" AutoEventWireup="true" CodeFile="Computers2.aspx.cs" Inherits="Computers2" Title="Untitled Page" EnableEventValidation="false" %>

<%@ Register Src="~/Controls/CompositeFilter.ascx" TagName="CompositeFilter" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterText.ascx" TagName="FilterText" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterComputers.ascx" TagName="FilterComputers" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDropDownList.ascx" TagName="FilterList" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDateTime.ascx" TagName="FilterDate" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterRange.ascx" TagName="FilterRange" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterIpAddress.ascx" TagName="FilterIPAddress" TagPrefix="flt" %>

<%@ Register Src="~/Controls/TaskPanel.ascx" TagName="TaskPanel" TagPrefix="tsk" %>
<%@ Register Src="~/Controls/SimpleTask.ascx" TagName="SimpleTask" TagPrefix="tsk" %>
<%@ Register Src="~/Controls/CustomizableTask.ascx" TagName="CustomizableTask" TagPrefix="tsk" %>
<%@ Register Src="~/Controls/CreateProcessTaskOptions.ascx" TagName="CreateProcessTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/ConfigurePasswordTaskOptions.ascx" TagName="ConfigurePasswordTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/RestoreFileFromQtnTaskOptions.ascx" TagName="RestoreFileFromQtnTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/UninstallTaskOptions.ascx" TagName="UninstallTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/ProductInstallTaskOptions.ascx" TagName="ProductInstallTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/ChangeDeviceProtectTaskOptions.ascx" TagName="ChangeDeviceProtectTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/TestTaskOptions.ascx" TagName="TestTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/TaskOptionsDialog.ascx" TagName="TaskOptionsDialog"
    TagPrefix="tsk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<script type="text/javascript">

    /*Ext.onReady(function () {
        Ext.resetElement = Ext.getBody();

        var IPAddress = "<%=Resources.Resource.IPAddress %>";
        var ControlCenter = "<%=Resources.Resource.ControlCenter %>";
        var CPU = "<%=Resources.Resource.CPU %>" + "(" + "<%=Resources.Resource.Herz%>" + ")";
        var DomainName = "<%=Resources.Resource.DomainName %>";
        var VBA32Integrity = "<%=Resources.Resource.VBA32Integrity %>";
        var LatestInfected = "<%=Resources.Resource.LatestInfected %>";
        var LatestMalware = "<%=Resources.Resource.LatestMalware %>";
        var LatestUpdate = "<%=Resources.Resource.LatestUpdate %>";
        var OSType = "<%=Resources.Resource.OSType %>";
        var RAM = "<%=Resources.Resource.RAM %>" + "(" + "<%=Resources.Resource.Megabyte%>" + ")";
        var RecentActive = "<%=Resources.Resource.RecentActive %>";
        var UserLogin = "<%=Resources.Resource.UserLogin %>";
        var VBA32KeyValid = "<%=Resources.Resource.VBA32KeyValid %>";
        var VBA32Version = "<%=Resources.Resource.VBA32Version %>";
        var Description = "<%=Resources.Resource.Description %>";
        var Policy = "<%=Resources.Resource.Policy %>";
        var DefaultPolicy = "<%=Resources.Resource.DefaultPolicy%>";
        var MachineName = "<%=Environment.MachineName%>";
        var ShortMinValue = "<%=Int16.MinValue%>";
        var DateTimeMinValue = "<%=DateTime.MinValue%>";
        var Components = "<%=Resources.Resource.Components%>";
        var ComponentName = "<%=Resources.Resource.ComponentName%>";
        var ComponentState = "<%=Resources.Resource.State%>";
        var MoreInfo = "<%=Resources.Resource.MoreInfo%>";

        var hdnWhere = "";

        Ext.tip.QuickTipManager.init();
        Ext.apply(Ext.tip.QuickTipManager.getQuickTip(), {
            maxWidth: 200,
            minWidth: 100,
            showDelay: 500    // Show 500ms after entering target
        });

        Ext.define('TreeNode', {
            extend: 'Ext.data.Model',
            fields: [
                        { name: 'text', type: 'string' },
                        { name: 'ip', type: 'string' },
                        { name: 'os', type: 'string' }

                     ]
        });

        treeStore = Ext.create('Ext.data.TreeStore', {
            model: 'TreeNode',
            proxy: {
                type: 'ajax',
                url: 'Handlers/ComputerPageHandler.ashx'

            },
            root: {
                text: 'Root',
                draggable: false,
                id: 'TreeRoot',
                root: true,
                leaf: false,
                expanded: true
            },
            folderSort: true,
            sorters: [{
                property: 'text',
                direction: 'ASC'
            }],
            listeners: {
                load: function (thisStore, records, successful, eOpts) {

                    if (!successful) {
                        Ext.Msg.alert("Resource.Error", "Resource.ErrorRequestingDataFromServer");

                    }
                }
            }
        });
        var tree = Ext.create('Ext.tree.Panel', {
            id: "treeComputers2",
            animate: true,
            store: treeStore,
            autoScroll: true,
            rootVisible: false,
            viewConfig:
            {
                listeners:
                {
                    beforeitemcontextmenu: function (view, record, item, index, e) {
                        e.stopEvent();
                        contextMenu.showAt(e.getXY());
                        return false;
                    }
                }
            }
        });
        tree.getSelectionModel().on('selectionchange', function (selModel, records) {
            if (records.length > 0)
                var node = records[0];
            hideInfo();
            if (node && node.isLeaf()) {
                showInfoAction.enable();
            }
            else {
                showInfoAction.disable();
            }
        });
        tree.on("checkchange", function (node, checked, eOpts) {
            toggleCheck(node, checked);
        });
        treeRoot = tree.getRootNode();
        treeRoot.expand(false);

        /* Menu Actions */
        
      /*  var showInfoAction = Ext.create('Ext.Action', {
            text: '<%=Resources.Resource.MoreInfo %>',
            handler: showInfoNode,
            disabled: true,
            tooltip: '<%=Resources.Resource.ShowAdditionalInfo %>'
        });
        var checkedAllAction = Ext.create('Ext.Action', {
            handler: checkedAllTrue,
            text: '<%=Resources.Resource.SelectAll %>',
            tooltip: '<%=Resources.Resource.SelectAllComputers %>'
        });
        var uncheckedAllAction = Ext.create('Ext.Action', {
            handler: checkedAllFalse,
            text: '<%=Resources.Resource.UnselectAll %>',
            tooltip: '<%=Resources.Resource.UnselectAllComputers %>'
        });
        var expandAllAction = Ext.create('Ext.Action', {
            handler: expandAll,
            text: '<%=Resources.Resource.Expand %>',
            tooltip: '<%=Resources.Resource.ExpandAllGroups %>'
        });
        var collapseAllAction = Ext.create('Ext.Action', {
            handler: collapseAll,
            text: '<%=Resources.Resource.Collapse %>',
            tooltip: '<%=Resources.Resource.CollapseAllGroups %>'
        });


        /* contextMenu and Toolbar*/
      /*  var contextMenu = Ext.create('Ext.menu.Menu', {
            items: [showInfoAction]
        });

        var computersToolBar = Ext.create('Ext.toolbar.Toolbar');
        computersToolBar.suspendLayouts();
        computersToolBar.add(checkedAllAction, uncheckedAllAction, '-',
                   expandAllAction, collapseAllAction, '-', showInfoAction);
        computersToolBar.resumeLayouts(true);

        /* Main Panel */
      /*  var ComputerTreePanel = new Ext.Panel({
            renderTo: 'mainContainer',
            region: 'east',
            layout: 'fit',
            items: [tree],
            split: true,
            title: '<%=Resources.Resource.ComputersWithGroups %>',
            tbar: computersToolBar,
            width: 400,
            height: 550,
            minHeight: 500,
            minWidth: 320,
            border: 1

        });
        //function to check/uncheck all  child nodes and parent path
        function toggleCheck(node, isCheck) {
            if (node) {
                var args = [isCheck];
                node.cascadeBy(function () {
                    c = args[0];
                    this.set('checked', c);
                }, null, args);
                if (node != treeRoot)
                    toggleParentCheck(node, isCheck);

            }
            getCompsInfo();
        }
        function toggleParentCheck(node, isCheck) {
            while (node.parentNode != treeRoot) {
                parentChecked = false;
                node.parentNode.eachChild(function (next) {
                    if (next.get('checked')) {
                        parentChecked = true;
                    }
                });
                node.parentNode.set('checked', parentChecked);
                node = node.parentNode;
            }
        }
        function expandAll() {
            treeRoot.eachChild(function (group) {
                group.expand();
            });
        }

        function collapseAll() {
            treeRoot.eachChild(function (group) {
                group.collapse();
            });
        }

        function checkedAllTrue() {
            checkedAll(true);
        }

        function checkedAllFalse() {
            checkedAll(false);
        }

        function checkedAll(checked) {
            toggleCheck(treeRoot, checked);

        }
        function showInfoNode() {
            var node = tree.getSelectionModel().getLastSelected();  // get selected node
            if (node && node.isLeaf()) {
                $.ajax({
                    type: "POST",
                    url: "Computers2.aspx/GetAdditionalInfo",
                    dataType: "json",
                    data: "{id:'" + node.data.id + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (msg) {
                        showInfo(JSON.parse(msg));
                    },
                    error: function (msg) { alert(msg.responseText) }
                });
            }
        }
        function hideInfo() {
            $get('AdditionalInfoPanel').innerHTML = "";
            $get('AdditionalInfoPanel').style.display = "none";
        }

        function showInfo(compInfo) {
            $get('AdditionalInfoPanel').style.display = "";
            var result = "<div style='margin-bottom: 10px;color: White;'><b>" + compInfo.computerName + "</b></div><div><table class='tableInfo'>";
            result += "<tr><td class='r0'>" + IPAddress + "</td><td class='r0'>" + compInfo.ipAddress + "</td></tr>";
            result += "<tr><td class='r1'>" + UserLogin + "</td><td class='r1'>" + (compInfo.userLogin == "" ? "-" : compInfo.userLogin) + "</td></tr>";
            result += "<tr><td class='r0'>" + ControlCenter + "</td><td class='r0'><div class='" + ((compInfo.controlCenter || MachineName == compInfo.computerName) ? "enabledImage" : "disabledImage") + "'></div></td></tr>";
            result += "<tr><td class='r1'>" + DomainName + "</td><td class='r1'>" + (compInfo.domainName == "" ? "-" : compInfo.domainName) + "</td></tr>";
            result += "<tr><td class='r0'>" + OSType + "</td><td class='r0'>" + compInfo.osName + "</td></tr>";
            result += "<tr><td class='r1'>" + RAM + "</td><td class='r1'>" + (compInfo.ram == ShortMinValue ? "-" : compInfo.ram) + "</td></tr>";
            result += "<tr><td class='r0'>" + CPU + "</td><td class='r0'>" + (compInfo.cpu == ShortMinValue ? "-" : compInfo.cpu) + "</td></tr>";
            result += "<tr><td class='r1'>" + RecentActive + "</td><td class='r1'>" + compInfo.recentActive + "</td></tr>";
            result += "<tr><td class='r0'>" + LatestUpdate + "</td><td class='r0'>" + (compInfo.latestUpdate == DateTimeMinValue ? "-" : compInfo.latestUpdate) + "</td></tr>";
            result += "<tr><td class='r1'>" + VBA32Version + "</td><td class='r1'>" + (compInfo.vba32Version == "" ? "-" : compInfo.vba32Version) + "</td></tr>";
            result += "<tr><td class='r0'>" + LatestInfected + "</td><td class='r0'>" + (compInfo.latestInfected == DateTimeMinValue ? "-" : compInfo.latestInfected) + "</td></tr>";
            result += "<tr><td class='r1'>" + LatestMalware + "</td><td class='r1'>" + (compInfo.latestMalware == "" ? "-" : compInfo.latestMalware) + "</td></tr>";
            result += "<tr><td class='r0'>" + VBA32Integrity + "</td><td class='r0'><div class='" + (compInfo.vba32Integrity ? "enabledImage" : "disabledImage") + "'></div></td></tr>";
            result += "<tr><td class='r1'>" + VBA32KeyValid + "</td><td class='r1'><div class='" + (compInfo.vba32KeyValid ? "enabledImage" : "disabledImage") + "'></div></td></tr>";
            result += "<tr><td class='r0'>" + Description + "</td><td class='r0'>" + compInfo.description + "</td></tr>";

            var defaultPolicyName = $get('<%=hdnDefaultPolicyName.ClientID%>').value;
            var policyValue = !compInfo.policyName ? "-" : compInfo.policyName;
            if (policyValue == "-" && defaultPolicyName != "") policyValue = defaultPolicyName + " (" + DefaultPolicy + ")";
            result += "<tr><td class='r0'>" + Policy + "</td><td class='r0'>" + policyValue + "</td></tr>";
            result += "</table><div style='padding: 10px 0px 5px 0px;'><b>" + Components + "</b></div><table class='tableInfo'><tr><td>" + ComponentName + "</td><td>" + ComponentState + "</td></tr>";
            for (var i = 0; i < compInfo.components.length; i++) {
                result += "<tr><td class='r" + i % 2 + "'>" + compInfo.components[i].name + "</td><td class='r" + i % 2 + "'>" + compInfo.components[i].state + "</td></tr>";
            }
            result += "</table><div style='padding-top: 7px;'><a href='CompInfo.aspx?CompName=" + compInfo.computerName + "'>" + MoreInfo + "</a></div></div>";
            $get('AdditionalInfoPanel').innerHTML = result;
        }
        
        function getCompsInfo() {
            computers = new Array();
            compIP = new Array();
            treeRoot.eachChild(function (group) {
                RecursiveAddNodes(group);

                function RecursiveAddNodes(rootNode) {
                    for (var i = 0; i < rootNode.childNodes.length; i++) {
                        RecursiveAddNodes(rootNode.childNodes[i]);
                    }
                    
                    if (rootNode.isLeaf() && (rootNode.get('checked'))) {
                        computers.push(rootNode.get("text").toLowerCase());
                        compIP.push(rootNode.get("ip"));
                    }
                }
            });
            $get('<%=hdnSelectedCompsNames.ClientID %>').value = computers.join('&');
            $get('<%=hdnSelectedCompsIP.ClientID %>').value = compIP.join('&');
        }
        /*$get('<%=btnReload.ClientID%>').onclick = function (param) {
            
            hdnWhere = param;
            treeStore.proxy.extraParams = { where: hdnWhere };

            treeStore.load();
        }*/


   /* });*/



    var IPAddress = "<%=Resources.Resource.IPAddress %>";
    var ControlCenter = "<%=Resources.Resource.ControlCenter %>";
    var CPU = "<%=Resources.Resource.CPU %>" + "(" + "<%=Resources.Resource.Herz%>" + ")";
    var DomainName = "<%=Resources.Resource.DomainName %>";
    var VBA32Integrity = "<%=Resources.Resource.VBA32Integrity %>";
    var LatestInfected = "<%=Resources.Resource.LatestInfected %>";
    var LatestMalware = "<%=Resources.Resource.LatestMalware %>";
    var LatestUpdate = "<%=Resources.Resource.LatestUpdate %>";
    var OSType = "<%=Resources.Resource.OSType %>";
    var RAM = "<%=Resources.Resource.RAM %>" + "(" + "<%=Resources.Resource.Megabyte%>" + ")";
    var RecentActive = "<%=Resources.Resource.RecentActive %>";
    var UserLogin = "<%=Resources.Resource.UserLogin %>";
    var VBA32KeyValid = "<%=Resources.Resource.VBA32KeyValid %>";
    var VBA32Version = "<%=Resources.Resource.VBA32Version %>";
    var Description = "<%=Resources.Resource.Description %>";
    var Policy = "<%=Resources.Resource.Policy %>";
    var DefaultPolicy = "<%=Resources.Resource.DefaultPolicy%>";
    var MachineName = "<%=Environment.MachineName%>";
    var ShortMinValue = "<%=Int16.MinValue%>";
    var DateTimeMinValue = "<%=DateTime.MinValue%>";
    var Components = "<%=Resources.Resource.Components%>";
    var ComponentName = "<%=Resources.Resource.ComponentName%>";
    var ComponentState = "<%=Resources.Resource.State%>";
    var MoreInfo = "<%=Resources.Resource.MoreInfo%>";

    var hdnWhere = "";
    $(document).ready(function () {

        $("input[type=button]").button();


        $.jstree.defaults.checkbox.whole_node = false;
        $.jstree.defaults.checkbox.tie_selection = false;
        $('#divTree').jstree({
            'core': {
                'check_callback': true,
                'multiple': false,
                'data': function (node, cb) {
                    cb(loadTreeInfo());
                }
            },
            'types': {
                'group': {
                    'icon': "App_Themes/Main/groups/images/group.png"
                },
                'computerGrey': {
                    'icon': "App_Themes/Main/groups/images/computerGrey.png"
                },
                'computerRed': {
                    'icon': "App_Themes/Main/groups/images/computerRed.png"
                },
                'computerGreen': {
                    'icon': "App_Themes/Main/groups/images/computerGreen.png"
                },
                'computerYellow': {
                    'icon': "App_Themes/Main/groups/images/computerYellow.png"
                },
                'server': {
                    'icon': "App_Themes/Main/groups/images/server.png"
                },
                'default': {
                }
            },
            'plugins': ["checkbox", "types", "contextmenu"],
            'contextmenu': {
                'items': function ($node) {
                    var dis = ($node.type == "group");

                    return {

                        'MoreInfo': {
                            'label': MoreInfo,
                            '_disabled': dis,
                            'action': function (obj) {

                                MoreInfoFunction($node);
                                getInfo();
                            }
                        }
                    }
                }
            }
        });

        $('#divTree').on('changed.jstree', function (e, data) {
            MoreInfoFunction(data.node);
        });
        $('#divTree').on('check_node.jstree', function (e, data) {
            getCheckedCompsInfo();
        });
        $('#divTree').on('uncheck_node.jstree', function (e, data) {
            getCheckedCompsInfo();
        });
        $('#divTree').on('loaded.jstree', function (e, data) {
            getCheckedCompsInfo();
        });
        $('#divTree').on('refresh.jstree', function (e, data) {
            getCheckedCompsInfo();

        });


        $get('<%=btnReload.ClientID%>').onclick = function (param) {
            hdnWhere = param;
            $('#divTree').jstree('refresh');
        };

    });
    /**/
    function getCheckedCompsInfo() {
        computers = new Array();
        compIP = new Array();
        var node;
        var gr = $('#divTree').jstree(true).get_node('Group_1');
        console.log(gr);
        var checkedObj = $('#divTree').jstree('get_checked', true);
        
        for (i = 0; i < checkedObj.length; i++) {
            if (checkedObj[i].state != null && checkedObj[i].state.checked == true) {
                /*console.log(checkedObj[i]);*/
                node = checkedObj[i].original;
                if (node != null && node.leaf) {
                    computers.push(node.text.toLowerCase());
                    compIP.push(node.ip);
                }
            }
        }
        $get('<%=hdnSelectedCompsNames.ClientID %>').value = computers.join('&');
        $get('<%=hdnSelectedCompsIP.ClientID %>').value = compIP.join('&');
    };

    function MoreInfoFunction(node) {
        hideInfo();
        if (node == null) return;
        var node = node.original;
        if (node) {
            showInfoEnable(node.leaf);
            if (node.leaf) $("#btnMoreInfo").attr("nodeId", node.id);
            else $("#btnMoreInfo").attr("nodeId", -1);
                
        }
        
    };

    function showInfoEnable(enable) {
        $("#btnMoreInfo").button("option", "disabled", !enable);
    };

    function loadTreeInfo() {
        var d = "";
        var hdnSelected = $get('<%=hdnSelectedCompsNames.ClientID %>').value;
         $.ajax({
            type: "GET",
            async:false,
            url: "Handlers/ComputerPageHandler.ashx",
            dataType: "json",
            data: { where: hdnWhere,selected:hdnSelected },
            success: function (data) {
                d = data;
            },
            error: function (e) {
                alert(e);
            }
        });
        return d;
    };


    function getInfo() {
        var nodeId = $("#btnMoreInfo").attr("nodeId");
        if (nodeId == -1) return;
        $.ajax({
            type: "POST",
            url: "Computers2.aspx/GetAdditionalInfo",
            dataType: "json",
            data: "{id:'" + nodeId + "'}",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {            
                showInfo(JSON.parse(msg));
            },
            error: function (msg) { alert(msg.responseText) }
        });
    };
    function hideInfo() {
        $get('AdditionalInfoPanel').innerHTML = "";
        $get('AdditionalInfoPanel').style.display = "none";
    };

    function showInfo(compInfo) {
        $get('AdditionalInfoPanel').style.display = "";
        var result = "<div style='margin-bottom: 10px;color: White;'><b>" + compInfo.computerName + "</b></div><div><table class='tableInfo'>";
        result += "<tr><td class='r0'>" + IPAddress + "</td><td class='r0'>" + compInfo.ipAddress + "</td></tr>";
        result += "<tr><td class='r1'>" + UserLogin + "</td><td class='r1'>" + (compInfo.userLogin == "" ? "-" : compInfo.userLogin) + "</td></tr>";
        result += "<tr><td class='r0'>" + ControlCenter + "</td><td class='r0'><div class='" + ((compInfo.controlCenter || MachineName == compInfo.computerName) ? "enabledImage" : "disabledImage") + "'></div></td></tr>";
        result += "<tr><td class='r1'>" + DomainName + "</td><td class='r1'>" + (compInfo.domainName == "" ? "-" : compInfo.domainName) + "</td></tr>";
        result += "<tr><td class='r0'>" + OSType + "</td><td class='r0'>" + compInfo.osName + "</td></tr>";
        result += "<tr><td class='r1'>" + RAM + "</td><td class='r1'>" + (compInfo.ram == ShortMinValue ? "-" : compInfo.ram) + "</td></tr>";
        result += "<tr><td class='r0'>" + CPU + "</td><td class='r0'>" + (compInfo.cpu == ShortMinValue ? "-" : compInfo.cpu) + "</td></tr>";
        result += "<tr><td class='r1'>" + RecentActive + "</td><td class='r1'>" + compInfo.recentActive + "</td></tr>";
        result += "<tr><td class='r0'>" + LatestUpdate + "</td><td class='r0'>" + (compInfo.latestUpdate == DateTimeMinValue ? "-" : compInfo.latestUpdate) + "</td></tr>";
        result += "<tr><td class='r1'>" + VBA32Version + "</td><td class='r1'>" + (compInfo.vba32Version == "" ? "-" : compInfo.vba32Version) + "</td></tr>";
        result += "<tr><td class='r0'>" + LatestInfected + "</td><td class='r0'>" + (compInfo.latestInfected == DateTimeMinValue ? "-" : compInfo.latestInfected) + "</td></tr>";
        result += "<tr><td class='r1'>" + LatestMalware + "</td><td class='r1'>" + (compInfo.latestMalware == "" ? "-" : compInfo.latestMalware) + "</td></tr>";
        result += "<tr><td class='r0'>" + VBA32Integrity + "</td><td class='r0'><div class='" + (compInfo.vba32Integrity ? "enabledImage" : "disabledImage") + "'></div></td></tr>";
        result += "<tr><td class='r1'>" + VBA32KeyValid + "</td><td class='r1'><div class='" + (compInfo.vba32KeyValid ? "enabledImage" : "disabledImage") + "'></div></td></tr>";
        result += "<tr><td class='r0'>" + Description + "</td><td class='r0'>" + compInfo.description + "</td></tr>";

        var defaultPolicyName = $get('<%=hdnDefaultPolicyName.ClientID%>').value;
        var policyValue = !compInfo.policyName ? "-" : compInfo.policyName;
        if (policyValue == "-" && defaultPolicyName != "") policyValue = defaultPolicyName + " (" + DefaultPolicy + ")";
        result += "<tr><td class='r0'>" + Policy + "</td><td class='r0'>" + policyValue + "</td></tr>";
        result += "</table><div style='padding: 10px 0px 5px 0px;'><b>" + Components + "</b></div><table class='tableInfo'><tr><td>" + ComponentName + "</td><td>" + ComponentState + "</td></tr>";
        for (var i = 0; i < compInfo.components.length; i++) {
            result += "<tr><td class='r" + i % 2 + "'>" + compInfo.components[i].name + "</td><td class='r" + i % 2 + "'>" + compInfo.components[i].state + "</td></tr>";
        }
        result += "</table><div style='padding-top: 7px;'><a href='CompInfo.aspx?CompName=" + compInfo.computerName + "'>" + MoreInfo + "</a></div></div>";
        $get('AdditionalInfoPanel').innerHTML = result;
    };

</script>
<ajaxToolkit:ToolkitScriptManager  ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" >
    <Scripts>
        <asp:ScriptReference Path="~/js/Safari3AjaxHack.js" />
    </Scripts>
</ajaxToolkit:ToolkitScriptManager>

<asp:HiddenField runat="server" ID="hdnDefaultPolicyName" Value="" />

<div class="title"><%=Resources.Resource.PageComputersTitle%></div>
<asp:UpdatePanel runat="server" ID="updatePanelComputerFilter">
    <ContentTemplate>
        <flt:CompositeFilter ID="FilterContainer1" UserFiltersTemproraryStorageName="CompFiltersTemp"
            UserFiltersProfileKey="CompFilters" OnClearClick="Filter1_ClearClick" runat="server" 
            OnActiveFilterChange="FilterContainer_ActiveFilterChanged"  InformationListType="Computers">
            <FiltersTemplate>
                <table>
                    <tr>
                        <td valign="top">
                            <flt:FilterComputers runat="server" ID="fltComputers" NameFieldDB="ComputerName" TextFilter='<%$ Resources:Resource, ComputerName %>' />
                            <flt:FilterIPAddress runat="server" ID="fltIPAddress" NameFieldDB="IPAddress" TextFilter='<%$ Resources:Resource, IPAddress %>' />
                            <flt:FilterText runat="server" ID="fltLogin" NameFieldDB="UserLogin" TextFilter='<%$ Resources:Resource, UserLogin %>' />
                            <flt:FilterText runat="server" ID="fltDomainName" NameFieldDB="DomainName" TextFilter='<%$ Resources:Resource, DomainName %>' />
                            <flt:FilterText runat="server" ID="fltDescription" NameFieldDB="Description" TextFilter='<%$ Resources:Resource, Description %>' />
                            <flt:FilterRange runat="server" ID="fltRAM" NameFieldDB="RAM" TextFilter='<%$ Resources:Resource, RAM_MB %>' />
                            <flt:FilterRange runat="server" ID="fltCPU" NameFieldDB="CPUClock" TextFilter='<%$ Resources:Resource, CPU_MHz %>' />
                            <flt:FilterText runat="server" ID="fltOSType" NameFieldDB="OSName" TextFilter='<%$ Resources:Resource, OSType %>' />
                            <flt:FilterText runat="server" ID="fltVBA32Version" NameFieldDB="Vba32Version" TextFilter='<%$ Resources:Resource, VBA32Version %>' />
                            <flt:FilterText runat="server" ID="fltLatestMalware" NameFieldDB="LatestMalware" TextFilter='<%$ Resources:Resource, LatestMalware %>' />
                        </td>
                        <td valign="top" style="padding-left: 20px;">
                            <flt:FilterDate runat="server" ID="fltRecentActive" NameFieldDB="RecentActive" TextFilter='RecentActive' />
                            <flt:FilterDate runat="server" ID="fltLatestUpdate" NameFieldDB="LatestUpdate" TextFilter='LatestUpdate' />
                            <flt:FilterDate runat="server" ID="fltLatestInfected" NameFieldDB="LatestInfected" TextFilter='LatestInfected' />
                            <flt:FilterList runat="server" ID="fltControlCenter" NameFieldDB="ControlCenter" IsLocalized="false" TextFilter='<%$ Resources:Resource, ControlCenter %>' />
                            <flt:FilterList runat="server" ID="fltVBA32Integrity" NameFieldDB="Vba32Integrity" IsLocalized="false" TextFilter='<%$ Resources:Resource, VBA32Integrity %>' />
                            <flt:FilterList runat="server" ID="fltVBA32KeyValid" NameFieldDB="Vba32KeyValid" IsLocalized="false" TextFilter='<%$ Resources:Resource, VBA32KeyValid %>' />
                            <flt:FilterList runat="server" ID="fltPolicy" NameFieldDB="TypeName" IsLocalized="false" TextFilter='<%$ Resources:Resource, Policy %>' />
                        </td>
                    </tr>
                </table>
            </FiltersTemplate>
        </flt:CompositeFilter>
    </ContentTemplate>
</asp:UpdatePanel>
  <asp:UpdatePanel ID="updatePanelComputerTask" runat="server">
        <ContentTemplate>
            <tsk:TaskPanel ID="TaskPanel" runat="server" OnTaskAssign="CompositeTaskPanel_TaskAssign">
                <TasksTemplate>
                    <tsk:SimpleTask ID="SimpleTask1" runat="server" TaskName="<%$ Resources:Resource, TaskNameVba32LoaderEnable %>"
                        TaskType="Vba32LoaderLaunch" />
                    <tsk:SimpleTask ID="SimpleTask2" runat="server" TaskName="<%$ Resources:Resource, TaskNameVba32LoaderDisable %>"
                        TaskType="Vba32LoaderExit" />
                    <tsk:SimpleTask ID="SimpleTask3" runat="server" TaskName="<%$ Resources:Resource, TaskNameVba32MonitorEnable %>"
                        TaskType="Vba32MonitorEnable" />
                    <tsk:SimpleTask ID="SimpleTask4" runat="server" TaskName="<%$ Resources:Resource, TaskNameVba32MonitorDisable %>"
                        TaskType="Vba32MonitorDisable" />
                    <tsk:SimpleTask ID="SimpleTask5" runat="server" TaskName="<%$ Resources:Resource, MenuSystemInfo %>"
                        TaskType="QuerySystemInformation" />
                    <tsk:SimpleTask ID="SimpleTask6" runat="server" TaskName="<%$ Resources:Resource, TaskNameListProcesses %>"
                        TaskType="QueryProcessesList" />
                    <tsk:SimpleTask ID="SimpleTask7" runat="server" TaskName="<%$ Resources:Resource, TaskNameComponentState %>"
                        TaskType="QueryComponentsState" />
                    <tsk:TaskOptionsDialog ID="TaskOptionsDialog1" runat="server" />
                    <tsk:CustomizableTask ID="CreateProcessTask" runat="server" TaskName="<%$ Resources:Resource, CreateProcess %>"
                        TaskOptionsID="CreateProcessTaskOptions1" />
                    <tsk:CreateProcessTaskOptions ID="CreateProcessTaskOptions1" runat="server" />

                   <tsk:CustomizableTask ID="ConfigurePasswordTask" runat="server" TaskName="<%$ Resources:Resource, TaskNameConfigurePassword %>"
                        TaskOptionsID="ConfigurePasswordTaskOptions1" />
                    <tsk:ConfigurePasswordTaskOptions ID="ConfigurePasswordTaskOptions1" runat="server" />

                    <tsk:CustomizableTask ID="RestoreFileFromQtnTask" runat="server" TaskName="<%$ Resources:Resource, TaskNameRestoreFileFromQtn %>"
                        TaskOptionsID="RestoreFileFromQtnTaskOptions1" />
                    <tsk:RestoreFileFromQtnTaskOptions ID="RestoreFileFromQtnTaskOptions1" runat="server" />

                    <tsk:CustomizableTask ID="UninstallTask" runat="server" TaskName="<%$ Resources:Resource, TaskUninstall %>"
                        TaskOptionsID="UninstallTaskOptions1" />
                    <tsk:UninstallTaskOptions ID="UninstallTaskOptions1" runat="server" />

                    <tsk:CustomizableTask ID="ProductInstallTask" runat="server" TaskName="<%$ Resources:Resource, Install %>"
                        TaskOptionsID="ProductInstallTaskOptions1" />
                    <tsk:ProductInstallTaskOptions ID="ProductInstallTaskOptions1" runat="server" />

                    <tsk:CustomizableTask ID="ChangeDeviceProtectTask" runat="server" TaskName="<%$ Resources:Resource, TaskChangeDeviceProtect %>"
                        TaskOptionsID="ChangeDeviceProtectTaskOptions1" />
                    <tsk:ChangeDeviceProtectTaskOptions ID="ChangeDeviceProtectTaskOptions1" runat="server" />

                    <tsk:CustomizableTask ID="TestTask" runat="server" TaskName="Test"
                        TaskOptionsID="TestTaskOptions1" />
                    <tsk:TestTaskOptions ID="TestTaskOptions1" runat="server" />
                </TasksTemplate>
            </tsk:TaskPanel>
        </ContentTemplate>
    </asp:UpdatePanel>
<%--<div id="mainContainer" class="bigTree" style="width:410px;height:600px;float:left;"></div>--%>
<div id="divTreePanel" class="bigTree" style="width:500px;height:600px;float:left;">
    <div id="divTreePanelButtons">
        <input type="button" value="<%=Resources.Resource.SelectAll %>" title="<%=Resources.Resource.SelectAllComputers %>" onclick="$('#divTree').jstree('check_all');"/>
        <input type="button" value="<%=Resources.Resource.UnselectAll %>" title="<%=Resources.Resource.UnselectAllComputers %>" onclick="$('#divTree').jstree('uncheck_all');"/>
        <input type="button" value="<%=Resources.Resource.Collapse %>" title="<%=Resources.Resource.CollapseAllGroups %>" onclick="$('#divTree').jstree('close_all');"/>
        <input type="button" value="<%=Resources.Resource.Expand %>" title="<%=Resources.Resource.ExpandAllGroups %>" onclick="$('#divTree').jstree('open_all');"/>
        <input id="btnMoreInfo" type="button" disabled="true" value="<%=Resources.Resource.MoreInfo %>" title="<%=Resources.Resource.ShowAdditionalInfo %>" onclick="return getInfo();"/>
    </div>
    <div id="divTree" class="bigTree"></div>
</div>


<div id="AdditionalInfoPanel" style="width: 300px; min-height: 150px; padding: 8px; margin: 5px 20px; background-color: Gray; float: left;display: none;"></div>

<input runat="server" id="btnReload" type="button" value="Reload" style="display: none;" />
<input runat="server" id="hdnSelectedCompsNames" value=""  style="display:none;" />
<input runat="server" id="hdnSelectedCompsIP" value=""  style="display:none;" />

</asp:Content>