<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/mstrPageMain.master" CodeFile="PoliciesPage.aspx.cs" Inherits="_PoliciesPage" %>

<%@ Register Assembly="PagingControl" Namespace="PagingControls" TagPrefix="cc1" %>

<%@ Register Src="Controls/TaskConfigureMonitor.ascx" TagName="TaskConfigureMonitor"    TagPrefix="uc" %>
<%@ Register Src="Controls/TaskConfigureQuarantine.ascx" TagName="TaskConfigureQuarantine"    TagPrefix="uc" %>
<%@ Register Src="Controls/TaskConfigureLoader.ascx" TagName="TaskConfigureLoader"    TagPrefix="uc" %>
<%@ Register Src="~/Controls/TaskChangeDeviceProtectEx.ascx" TagName="TaskChangeDeviceProtect" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">

<script type="text/javascript">   
        
    function OnLoaderClick()
    {
        var cbox = $get("<%=cboxRunLoader.ClientID%>");
        var elem = $get("<%=cboxRunMonitor.ClientID%>");
        
        elem.disabled = !cbox.checked;
    }
    
    function OnClientCheck()
    {
        var tbox = $get('<%=tboxPolicyName.ClientID%>');
        var expr = new RegExp("^[a-zA-Z\u0451\u0401а-яА-Я_0-9 -]+$");
        if(!expr.test(tbox.value))
        {
            alert('<%=Resources.Resource.ErrorPolicyName%>');
            return false;            
        }
        else
        {
            return true;
        }
    }   
        
</script>

<script type="text/javascript" src="js/Groups/ext-1.1.1/adapter/ext/ext-base.js"></script>
<script type="text/javascript" src="js/Groups/ext-1.1.1/ext-all.js"></script> 
<script type="text/javascript" src="js/Groups/ext-1.1.1/ext-core.js"></script> 

<script language="javascript" type="text/javascript">

var TabsExample = {
            init: function () {
                var tabs = new Ext.TabPanel('tabs1');
                tabs.addTab('tab1', "<%=Resources.Resource.Management %>");
                tabs.addTab('tab2', "<%=Resources.Resource.AppointmentPolicy %>");
                tabs.activate('tab1');                     
            }
        }
        Ext.EventManager.onDocumentReady(TabsExample.init, TabsExample, true);
        

Ext.onReady(function () {    
    /*  ***************************************************************************************************************
    Container and layout generation
    ***************************************************************************************************************  */
    // turn on quick tips
        
    Ext.QuickTips.init();

    var view = Ext.DomHelper.append('mainContainer',
        { cn: [{ id: 'policyToolBar' }, { id: 'policyTree'}] }
    );
    var bot = Ext.DomHelper.append('botContainer',
        { cn: [{ id: 'bottomToolBar'}] }
    );

    // create the bottom toolbar
    var bottomToolBar = new Ext.Toolbar('bottomToolBar');
    bottomToolBar.add({
        id: 'reload',
        text: '<%=Resources.Resource.Reload %>',
        handler: reload,
        cls: 'x-btn-text-icon reload',
        tooltip: '<%=Resources.Resource.ReloadInfoFromServer %>'
    }, '->', {
        id: 'save',
        text: '<%=Resources.Resource.Save %>',
        handler: save,
        cls: 'x-btn-text-icon save',
        tooltip: '<%=Resources.Resource.SaveInfoToServer %>'
    });

    // create  layout
    var layout = new Ext.BorderLayout('mainContainer', {
        west: {
            split: true,
            initialSize: 350,
            minSize: 200,
            maxSize: 500,
            titlebar: true,
            margins: { left: 5, right: 0, bottom: 5, top: 5 }
        },
        center: {
            title: '<%=Resources.Resource.Policies %>',
            margins: { left: 0, right: 5, bottom: 5, top: 5 }
        }
    }, 'mainContainer');

    layout.batchAdd({
        west: {
            id: 'noPolicyTree',
            autoCreate: true,
            title: '<%=Resources.Resource.NotAssignedExplicitly %>',
            autoScroll: true,
            fitToFrame: true
        },
        center: {
            el: view,
            autoScroll: true,
            fitToFrame: true,
            resizeEl: 'policyTree'
        }
    });

    /*  ***************************************************************************************************************
    Container and layout generation finished
    ***************************************************************************************************************  */

    /*  ***************************************************************************************************************
    Save and reload realization
    ***************************************************************************************************************  */

    //reloading trees from server
    function reload() {
        noPolicyTreeRoot.reload();
        policyTreeRoot.reload();
    }

    //constructor to class containing info for nodes in tree
    function myNode(text, id, parentId, comment, isLeaf) {
        this.text = text;
        this.id = id;
        this.parentId = parentId;
        this.comment = comment;
        this.isLeaf = isLeaf;
    }

    //save edited groups to server
    function save() {
        layout.el.mask('<%=Resources.Resource.SendingDataToServer %>' + '...', 'x-mask-loading');
        //generate arrays containing nodes info
        var policyTreeArray = new Array();
        policyTreeRoot.eachChild(function (root) {
            RecursiveSaving(root, root.id);
        });

        function RecursiveSaving(root, policyID) {
            var length = root.childNodes.length;
            for (var i = 0; i < length; i++) {
                RecursiveSaving(root.childNodes[i], policyID);
            }
            if (root.getDepth() == 0) return;

            if (root.isLeaf()) {
                policyTreeArray[policyTreeArray.length++] = new myNode(root.text, root.id, policyID, "", root.isLeaf() ? "true" : "false");
            }
        }
        //query .net handler with ajax
        Ext.Ajax.request({
            url: '<%=Request.ApplicationPath%>/Handlers/GetTreePoliciesDataHandler.ashx',                                                    //url
            params: { policyTreeArray: Ext.util.JSON.encode(policyTreeArray) },
            method: 'POST',                                                         //method
            callback: function (options, success, response) {                       //callback function
                if (success == false) {  //ajax request failed to complete
                    Ext.Msg.show({
                        title: '<%=Resources.Resource.SaveInfoToServer %>',
                        msg: '<%=Resources.Resource.Error %>' + ': no answer from server',
                        minWidth: 200,
                        buttons: Ext.MessageBox.OK,
                        multiline: false
                    });
                }
                else {   //ajax request copmleted successfully
                    var jsonData = Ext.util.JSON.decode(response.responseText); //decode response
                    var resultMessage = jsonData.data.result;                   //passed result string
                    var resultSuccess = jsonData.success;                       //passed query result
                    if (resultSuccess == true) {  //added info to database successfully
                        Ext.Msg.show({
                            title: '<%=Resources.Resource.SaveInfoToServer %>',
                            msg: '<%=Resources.Resource.TaskStateCompletedSuccessfully %>',
                            minWidth: 200,
                            buttons: Ext.MessageBox.OK,
                            multiline: false
                        });
                    }
                    else {  //failed to add info to database
                        Ext.Msg.show({  //show error message
                            title: '<%=Resources.Resource.SaveInfoToServer %>',
                            msg: '<%=Resources.Resource.Error %>' + ': ' + resultMessage,
                            minWidth: 200,
                            buttons: Ext.MessageBox.OK
                        });
                    }
                }
                layout.el.unmask();
            }
        });
    }
    /*  ***************************************************************************************************************
    Save and reload realization finished
    ***************************************************************************************************************  */

    /*  ***************************************************************************************************************
    Policy tree
    policy tree generation, set up and its root creation
    ***************************************************************************************************************  */


    var policyTree = new Ext.tree.TreePanel('policyTree', {
        animate: true,
        enableDD: true,
        rootVisible: false,
        loader: new Ext.tree.TreeLoader({
            dataUrl: '<%=Request.ApplicationPath%>/Handlers/TreeWithPolicyHandler.ashx',
            requestMethod: 'GET'
        }),
        containerScroll: true,
        dropConfig: { appendOnly: true },
        listeners: {
            'aftermove': {
                fn: function (tree, parentOld, parentNew, node) {
                    parentNew.removeChild(node);
                    RecursiveDeleteGroups(parentOld);
                },
                delay: 10
            }
        }
    });

    function RecursiveDeleteGroups(root) {
        if (root.childNodes.length == 0 && root.id.search('Group') != -1) {
            var parentNode = root.parentNode;
            parentNode.removeChild(root);
            RecursiveDeleteGroups(parentNode);
        }
    }

    new Ext.tree.TreeSorter(policyTree, { folderSort: true });

    var policyTreeRoot = new Ext.tree.AsyncTreeNode({
        text: 'Policies Root',
        draggable: false,
        id: 'policyTreeRoot'
    });
    policyTree.setRootNode(policyTreeRoot);

    policyTree.render();

    policyTreeRoot.expand(false, false);

    // policy tree selection model
    var sm = policyTree.getSelectionModel();

    //stop event on navigation key pressed
    policyTree.el.on('keypress', function (e) {
        if (e.isNavKeyPress()) {
            e.stopEvent();
        }
    });

    policyTree.on('move', function (tree, childNode, parentNodeOld, parentNodeNew, index) {
        this.fireEvent("aftermove", tree, parentNodeOld, parentNodeNew, childNode);
    });

    //Fires before a new child is appended, return false to cancel the append.        
    policyTree.on('beforemove', function (tree, childNode, parentNodeOld, parentNodeNew, index) {
        onBeforeMove(tree, childNode, parentNodeOld, parentNodeNew, index);
        return true;
    });

    /*  ***************************************************************************************************************
    Policy tree finish
    ***************************************************************************************************************  */

    /*  ***************************************************************************************************************
    No Policy tree
    no policy tree generation, set up and its root creation
    ***************************************************************************************************************  */
    var noPolicyTree = new Ext.tree.TreePanel('noPolicyTree', {
        animate: true,
        enableDD: true,
        containerScroll: true,
        dropConfig: { appendOnly: true },
        loader: new Ext.tree.TreeLoader({
            dataUrl: '<%=Request.ApplicationPath%>/Handlers/TreeNoPolicyHandler.ashx',
            requestMethod: 'GET'
        }),
        listeners: {
            'aftermove': {
                fn: function (tree, parentOld, parentNew, node) {
                    parentNew.removeChild(node);
                    RecursiveDeleteGroups(parentOld);
                },
                delay: 10
            }
        }
    });

    noPolicyTree.on('move', function (tree, childNode, parentNodeOld, parentNodeNew, index) {
        this.fireEvent("aftermove", tree, parentNodeOld, parentNodeNew, childNode);
    });

    var noPolicyTreeRoot = new Ext.tree.AsyncTreeNode({
        allowDrag: false,
        allowDrop: true,
        text: '<%=Resources.Resource.NotAssignedExplicitly %>',
        id: 'noPolicyTreeRoot'
    });
    noPolicyTree.setRootNode(noPolicyTreeRoot);

    noPolicyTree.render();

    noPolicyTreeRoot.expand(false, false);

    //Fires before a new child is appended, return false to cancel the append.        
    noPolicyTree.on('beforemove', function (tree, childNode, parentNodeOld, parentNodeNew, index) {
        if (parentNodeNew.id == noPolicyTreeRoot.id) return false;
        onBeforeMove(tree, childNode, parentNodeOld, parentNodeNew, index);
        return true;
    });

    function onBeforeMove(tree, childNode, parentNodeOld, parentNodeNew, index) {
        var parent = parentNodeOld;
        var arrayNodes = new Array();
        if (!childNode.isLeaf()) //if not LEAF
            arrayNodes.push(childNode);
        while (parent.id.search("Group") != -1) {//no group
            arrayNodes.push(parent);
            parent = parent.parentNode;
        }

        parent = parentNodeNew;
        var tmp;
        var indexNode = -1;
        for (var i = arrayNodes.length - 1; i > -1; i--) {
            //create copy of node
            tmp = new Ext.tree.TreeNode(arrayNodes[i]);
            tmp.attributes = arrayNodes[i].attributes;

            indexNode = FindIndexOfChild(parent, tmp);
            if (indexNode == -1) {
                parent.appendChild(tmp);
                parent = tmp;
            }
            else parent = parent.childNodes[indexNode];
        }

        if (childNode.isLeaf()) {
            if (FindIndexOfChild(parent, childNode) == -1) {
                var newNode = new Ext.tree.TreeNode(childNode);
                newNode.attributes = childNode.attributes;
                parent.appendChild(newNode);
            }
        }
        else {
            RecursiveAppending(parent, childNode.childNodes);
        }
    }

    function RecursiveAppending(root, arrayNodes) {
        for (var i = 0; i < arrayNodes.length; i++) {
            var index = FindIndexOfChild(root, arrayNodes[i]);
            var newNode;
            if (index == -1) {
                newNode = new Ext.tree.TreeNode(arrayNodes[i]);
                newNode.attributes = arrayNodes[i].attributes;
                root.appendChild(newNode);
            }
            else {
                newNode = root.childNodes[index];
            }

            RecursiveAppending(newNode, arrayNodes[i].childNodes);
        }
    }

    function FindIndexOfChild(root, node) {
        for (var index = 0; index < root.childNodes.length; index++) {
            if (root.childNodes[index].id == node.id) return index;
        }

        return -1;
    }

    new Ext.tree.TreeSorter(noPolicyTree, { folderSort: true });

    /*  ***************************************************************************************************************
    No Policy tree finish
    ***************************************************************************************************************  */

});

</script>

<div class="title"><%=Resources.Resource.PolicySettings%></div>
<div id='tabs1'>      
      <div id='tab1' class="tab-content">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
        <div>
            <table style="width:570px;" class="ListContrastTable" runat="server" id="tblPolicies">
                <tr>
                    <td>
                    &nbsp;                    
                        <asp:DropDownList ID="ddlPolicyNames" style="width:90%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPolicyNames_SelectedIndexChanged" />
                        <div style="height:10px"></div>
                        <div class="GiveButton1" style="float:left; width: 120px">
                            <asp:LinkButton ID="lbtnCreate" runat="server" OnClick="lbtnCreate_Click" ForeColor="white" Width="100%"><%=Resources.Resource.Create%></asp:LinkButton>
                        </div>
                        <div style="float:left;width:10px; height:10px;"></div>
                        <div class="GiveButton1" style="float:left; width: 120px" runat="server" id="divSaveAs">
                            <asp:LinkButton ID="lbtnSaveAs" runat="server" OnClick="lbtnSaveAs_Click" ForeColor="white" Width="100%"><%=Resources.Resource.SaveAs%></asp:LinkButton>
                        </div>
                        <div style="float:left;width:10px; height:10px;"></div>
                        <div class="GiveButton1" style="float:left; width: 120px" runat="server" id="divEdit">
                            <asp:LinkButton ID="lbtnEdit" runat="server" OnClick="lbtnEdit_Click" ForeColor="white" Width="100%"><%=Resources.Resource.Edit%></asp:LinkButton>
                        </div>
                        <div style="float:left;width:10px; height:10px;"></div>
                        <div class="GiveButton1" style="width: 120px" runat="server" id="divDelete">
                            <asp:LinkButton ID="lbtnDelete" runat="server" OnClick="lbtnDelete_Click" ForeColor="white" Width="100%"><%=Resources.Resource.Delete%></asp:LinkButton>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span><%=Resources.Resource.DefaultPolicy%>&nbsp;&nbsp;<asp:ImageButton runat="server" ID="imbtnIsDefaultPolicy" OnClick="imbtnIsDefaultPolicy_Click" /></span>
                    </td>
                </tr>
            </table>
            <table style="width:570px;" class="ListContrastTable">
                <tr runat="server" id="trTBOX">
                    <td>
                        <asp:TextBox ID="tboxPolicyName" runat="server" />
                        <span>&nbsp;<%=Resources.Resource.Name%></span>
                    </td>
                </tr>
                <tr>
                    <td>
                          <asp:CheckBoxList runat="server" ID="cblUsedTasks" >
                            <asp:ListItem />
                            <asp:ListItem />
                            <asp:ListItem />
                            <asp:ListItem />
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
            
            <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="0"
            HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent" FadeTransitions="false" FramesPerSecond="40" 
            TransitionDuration="250" AutoSize="None" RequireOpenedPane="false" SuppressHeaderPostbacks="true">
           <Panes>
                <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                     <Header>
                        <a href="" class="accordionLink"><%=Resources.Resource.CongLdrConfigureLoader%></a>
                     </Header>
                    <Content>                    
                        <uc:TaskConfigureLoader id="loader" runat="server" HideHeader="true" />                    
                    </Content>
                </ajaxToolkit:AccordionPane>
                
                <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                     <Header>
                        <a href="" class="accordionLink"><%=Resources.Resource.CongLdrConfigureMonitor%></a>
                     </Header>
                    <Content>                         
                        <uc:TaskConfigureMonitor ID="monitor" runat="server" HideHeader="true" />
                    </Content>
                </ajaxToolkit:AccordionPane>
                <ajaxToolkit:AccordionPane ID="AccordionPane3" runat="server">
                     <Header>
                        <a href="" class="accordionLink"><%=Resources.Resource.TaskNameConfigureQuarantine%></a>
                     </Header>
                    <Content>                        
                        <uc:TaskConfigureQuarantine ID="quarantine" runat="server" HideHeader="true" />
                    </Content>
                </ajaxToolkit:AccordionPane>
                 <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
                     <Header>
                        <a href="" class="accordionLink"><%= Resources.Resource.StartupOptionsLoaderAndMonitor %></a>
                     </Header>
                    <Content>
                        <table style="width:570px;" class="ListContrastTable">
                        <tr>
                            <td>
                                <asp:CheckBox ID="cboxRunLoader" runat="server" onclick="OnLoaderClick()" /><%=Resources.Resource.LoaderLoaded %> <br/>
                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cboxRunMonitor" runat="server" /><%=Resources.Resource.MonitorEnabled %> <br/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=Resources.Resource.TaskChangeDeviceProtect%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc:TaskChangeDeviceProtect ID="deviceProtect" runat="server" HideHeader="true" HideBound="true" />
                            </td>
                        </tr>                        
                        </table>
                    </Content>
                </ajaxToolkit:AccordionPane>                
             </Panes>
        </ajaxToolkit:Accordion>
      </div>
         <div style="text-align:center; margin:5px; height: 15px;" runat="server" id="divButtons" visible="false">
            <div class="GiveButton1" style="float:left;">
               <asp:LinkButton ID="lbtnSave" OnClick="lbtnSave_Click" runat="server" ForeColor="white" Width="100%" OnClientClick='return OnClientCheck()' />
            </div>
            <div class="GiveButton1" style="float:left;" runat="server" id="divCancelEditing" visible="false">
               <asp:LinkButton ID="lbtnCancelEditing" OnClick="lbtnCancelEditing_Click" runat="server" ForeColor="white" Width="100%"></asp:LinkButton>
            </div>
         </div>
      </div>
      <div id='tab2' class="tab-content">
            <div id="mainContainer" style="width:700px;height:500px;"></div>
            <div id="botContainer" style="width:700px;height:40px;"></div>        
      </div>
</div>

<asp:Button runat="server" ID="btnHiddenMessage" style="visibility:hidden" />
<asp:Panel ID="pnlModalPopapMessage" runat="server" Style="display: none" CssClass="modalPopupMessage">
                                    <div runat="server" id ="mpPicture" class="ModalPopupPictureSuccess">
                                    </div>
                                    <div id="ModalPopupText">
                                        <p style="vertical-align:middle;">
                                            <center><asp:Label runat="server" ID="lblMessage" ></asp:Label></center>
                                        </p>                                                                            
                                    </div>
                                    <div id="ModalPopupButton">
                                        <p style="text-align: center; vertical-align:bottom;">
                                            <asp:Button ID="btnClose" runat="server" />                                            
                                        </p>
                                    </div>
                            </asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" X="400" Y="200" TargetControlID="btnHiddenMessage" PopupControlID="pnlModalPopapMessage" CancelControlID="btnClose" BackgroundCssClass="modalBackground" DropShadow="true" PopupDragHandleControlID="pnlModalPopapMessage" />
     
</asp:Content>
