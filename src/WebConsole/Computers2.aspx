<%@ Page Language="C#" MaintainScrollPositionOnPostback="true"  validateRequest="false" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="Computers2.aspx.cs" Inherits="Computers2" Title="Untitled Page" EnableEventValidation="false" %>

<%@ Register Src="~/Controls/CompositeFilter.ascx" TagName="CompositeFilter" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterText.ascx" TagName="FilterText" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterComputers.ascx" TagName="FilterComputers" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDropDownList.ascx" TagName="FilterList" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDateTime.ascx" TagName="FilterDate" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterRange.ascx" TagName="FilterRange" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterIpAddress.ascx" TagName="FilterIPAddress" TagPrefix="flt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<script type="text/javascript">
    Ext.onReady(function () {
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
        var ComputersTree = function () {
            var root;
            var treeStore;
            var tree;
            function onTreeLoad(This, node) { }

            //function to check/uncheck all  child nodes and parent path
            function toggleCheck(node, isCheck) {
                if (node) {
                    var args = [isCheck];
                    node.cascadeBy(function () {
                        c = args[0];
                        this.set('checked', c);
                    }, null, args);
                    if (node != root)
                        toggleParentCheck(node, isCheck);

                }
            }
            function toggleParentCheck(node, isCheck) {
                while (node.parentNode != root) {
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
            return {
                init: function () {
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
                        /*    { name: 'duration', type: 'string' }*/
                                ]
                    });

                    treeStore = Ext.create('Ext.data.TreeStore', {
                        model: 'TreeNode',
                        proxy: {
                            type: 'ajax',
                            url: 'Handlers/ComputerPageHandler.ashx',
                            expraParams:
                            {
                                where: Ext.JSON.encode(hdnWhere)
                            }
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
                        id: "treeComputers",
                        animate: true,
                        store: treeStore,
                        autoScroll: true,
                        rootVisible: false
                    });
                    tree.on("checkchange", function (node, checked, eOpts) {
                        toggleCheck(node, checked);
                    });
                    root = tree.getRootNode();
                    root.expand(false);


                    // when the group tree selection changes, enable/disable the toolbar buttons
                    tree.getSelectionModel().on('selectionchange', function (selModel, records) {
                        if (records.length > 0)
                            var node = records[0];

                        if (node && node.isLeaf()) {
                            /**/


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
                        else {
                            hideInfo();
                        }
                    });

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




                    var computersToolBar = Ext.create('Ext.toolbar.Toolbar');
                    computersToolBar.suspendLayouts();
                    computersToolBar.add({
                        id: 'checkedAll',
                        handler: checkedAllTrue,
                        text: '<%=Resources.Resource.SelectAll %>',
                        tooltip: '<%=Resources.Resource.SelectAllComputers %>'
                    },
                    {
                        id: 'uncheckedAll',
                        handler: checkedAllFalse,
                        text: '<%=Resources.Resource.UnselectAll %>',
                        tooltip: '<%=Resources.Resource.UnselectAllComputers %>'
                    },
                    {
                        id: 'expandAll',
                        handler: expandAll,
                        text: '<%=Resources.Resource.Expand %>',
                        tooltip: '<%=Resources.Resource.ExpandAllGroups %>'
                    },
                    {
                        id: 'collapseAll',
                        handler: collapseAll,
                        text: '<%=Resources.Resource.Collapse %>',
                        tooltip: '<%=Resources.Resource.CollapseAllGroups %>'
                    });
                    computersToolBar.resumeLayouts(true);
                    function expandAll() {
                        root.eachChild(function (group) {
                            group.expand();
                        });
                    }

                    function collapseAll() {
                        root.eachChild(function (group) {
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
                        toggleCheck(root, checked);

                    }
                    var ComputerTreePanel = new Ext.Panel({
                        renderTo: 'mainContainer',
                        region: 'east',
                        layout: 'fit',
                        items: [tree],
                        split: true,
                        title: '<%=Resources.Resource.ComputersWithGroups %>',
                        tbar: computersToolBar,
                        width: 400,
                        minWidth: 320,
                        border: 1
                    });

                },
                reload: function () {
                    if (treeStore)
                        treeStore.reload();
                },
                generateText: function () {
                    computers = new Array();
                    root.eachChild(function (group) {
                        RecursiveAdd(group);

                        function RecursiveAdd(rootNode) {
                            for (var i = 0; i < rootNode.childNodes.length; i++) {
                                RecursiveAdd(rootNode.childNodes[i]);
                            }
                            if (rootNode.isLeaf() && rootNode.get('checked'))
                                computers.push(rootNode.get("id").toLowerCase());
                        }

                    });
                    return computers.join('&');
                }

            };
        } ();

        var Computersdialog2 = function () {

            var asyncPostBack = false;

            function onApply(serial) {

                comps = ComputersTree.generateText();


                $.ajax({
                    type: "POST",
                    url: "DevicesPolicy.aspx/AddNewDevicePolicyByComputerList",
                    dataType: "json",

                    data: "{serial:'" + serial + "',comps:'" + comps + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (msg) {

                        if (msg == true) {
                            $("a:contains('" + serial + "')").trigger("click");
                        }
                        else alert("Ничего не добавлено");
                    },
                    error: function (msg) { alert(msg.responseText) }
                });

            }

            function onHide() {
                PageRequestManagerHelper.enableAsyncPostBack();
            }

            function onShow() {
                PageRequestManagerHelper.abortAsyncPostBack();
                PageRequestManagerHelper.disableAsyncPostBack();
            }

            return {
                setAsyncPostBack: function () {
                    asyncPostBack = true;
                },
                show: function () {


                    if (/*asyncPostBack*/true) {
                        ComputersTree.init();
                        asyncPostBack = false;
                    }
                    else {

                        ComputersTree.reload();
                    }
                }
            };
        } ();



        // OLD EXT
        /*  Ext.onReady(function () {

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

        Ext.QuickTips.init();

        var hdnWhere = "";

        var view = Ext.DomHelper.append('mainContainer',
        { cn: [{ id: 'computersToolBar' }, { id: 'ComputersTree'}] }
        );

        var computersToolBar = new Ext.Toolbar('computersToolBar');
        computersToolBar.add({
        id: 'checkedAll',
        handler: checkedAllTrue,
        text: '<%=Resources.Resource.SelectAll %>',
        tooltip: '<%=Resources.Resource.SelectAllComputers %>'
        },
        {
        id: 'uncheckedAll',
        handler: checkedAllFalse,
        text: '<%=Resources.Resource.UnselectAll %>',
        tooltip: '<%=Resources.Resource.UnselectAllComputers %>'
        },
        {
        id: 'expandAll',
        handler: expandAll,
        text: '<%=Resources.Resource.Expand %>',
        tooltip: '<%=Resources.Resource.ExpandAllGroups %>'
        },
        {
        id: 'collapseAll',
        handler: collapseAll,
        text: '<%=Resources.Resource.Collapse %>',
        tooltip: '<%=Resources.Resource.CollapseAllGroups %>'
        });

        function expandAll() {
        root.eachChild(function (group) {
        group.expand();
        });
        }

        function collapseAll() {
        root.eachChild(function (group) {
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
        root.eachChild(function (group) {
        group.fireEvent('checkchange', group, checked);
        });
        }

        // create  layout
        var layout = new Ext.BorderLayout('mainContainer', {
        west: {
        split: false,
        initialSize: 400,
        minSize: 200,
        maxSize: 500,
        titlebar: false,
        margins: { left: 5, right: 0, bottom: 5, top: 5 }
        }
        }, 'mainContainer');

        layout.batchAdd({
        west: {
        el: view,
        autoCreate: true,
        toolbar: computersToolBar,
        autoScroll: true,
        fitToFrame: true
        }
        });

        var loader = new Ext.tree.TreeLoader({
        dataUrl: '<%=Request.ApplicationPath%>/Handlers/ComputerPageHandler.ashx',
        requestMethod: 'GET',
        baseParams: {
        where: Ext.util.JSON.encode(hdnWhere)
        }
        });

        var ComputersTree = new Ext.tree.TreePanel('ComputersTree', {
        animate: true,
        enableDD: false,
        containerScroll: true,
        dropConfig: { appendOnly: true },
        loader: loader,
        rootVisible: false
        });

        loader.on("load", onTreeLoad);

        loader.on("beforeload", function (treeLoader) {
        treeLoader.baseParams.where = Ext.util.JSON.encode(hdnWhere);
        }, this);

        // group tree selection model
        var sm = ComputersTree.getSelectionModel();

        // when the group tree selection changes, enable/disable the toolbar buttons
        sm.on('selectionchange', function (defaultSelectionModel, node) {
        if (node && node.isLeaf()) {
        showInfo(node.attributes.compAdditionalInfo);
        }
        else {
        hideInfo();
        }
        });

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

        var root = new Ext.tree.AsyncTreeNode({});
        ComputersTree.setRootNode(root);

        ComputersTree.render();

        new Ext.tree.TreeSorter(ComputersTree, { folderSort: true });

        function onTreeLoad(This, node) {
        SetOnCheckChanged();
        }

        function SetOnCheckChanged() {
        root.eachChild(function (group) {
        RecursiveSetOnCheckChanged(group);
        });
        }

        function RecursiveSetOnCheckChanged(group) {
        group.on("checkchange", function (node, checked) {
        while (node.parentNode != root) {
        parentChecked = false;
        node.parentNode.eachChild(function (next) {
        if (next.attributes.checked) {
        parentChecked = true;
        }
        });
        node.parentNode.ui.toggleCheck(parentChecked);
        node.parentNode.attributes.checked = parentChecked;
        node = node.parentNode;
        }
        });

        group.on("checkchange", function (node, checked) {
        toggleCheck(this, checked)
        });

        group.eachChild(function (node) {
        RecursiveSetOnCheckChanged(node);
        });
        }

        //function to check/uncheck all the child node.
        function toggleCheck(node, isCheck) {
        if (node) {
        var args = [isCheck];
        node.cascade(function () {
        c = args[0];
        this.ui.toggleCheck(c);
        this.attributes.checked = c;
        }, null, args);
        }
        }

        $get('<%=btnReload.ClientID%>').onclick = function (param) {
        hdnWhere = param;
        root.reload();
        }*/
        $get('<%=btnReload.ClientID%>').onclick = function (param) {
            hdnWhere = param;
            root.reload();
        }

        Computersdialog2.show();
    });
</script>
<ajaxToolkit:ToolkitScriptManager  ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" />
<asp:HiddenField runat="server" ID="hdnDefaultPolicyName" Value="" />

<div class="title"><%=Resources.Resource.PageComputersTitle%></div>
<asp:UpdatePanel runat="server" ID="updatePanelComputerFilter">
    <ContentTemplate>
        <flt:CompositeFilter ID="FilterContainer1" UserFiltersTemproraryStorageName="CompFiltersTemp"
            UserFiltersProfileKey="CompFilters" OnClearClick="Filter1_ClearClick" runat="server"
            OnActiveFilterChange="FilterContainer_ActiveFilterChanged" InformationListType="Computers">
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

<div id="mainContainer" class="bigTree" style="width:410px;height:600px;float:left;"></div>

<div id="AdditionalInfoPanel" style="width: 300px; min-height: 150px; padding: 8px; margin: 5px 20px; background-color: Gray; float: left;display: none;"></div>

<input runat="server" id="btnReload" type="button" value="Reload" style="display: none;" />

</asp:Content>

