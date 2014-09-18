<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/mstrPageMain.master" CodeFile="PoliciesPage.aspx.cs" Inherits="_PoliciesPage" %>

<%@ Register Src="Controls/TaskConfigureMonitor.ascx" TagName="TaskConfigureMonitor"    TagPrefix="uc" %>
<%@ Register Src="Controls/TaskConfigureQuarantine.ascx" TagName="TaskConfigureQuarantine"    TagPrefix="uc" %>
<%@ Register Src="Controls/TaskConfigureLoader.ascx" TagName="TaskConfigureLoader"    TagPrefix="uc" %>
<%@ Register Src="~/Controls/TaskConfigureJornalEvents.ascx" TagName="TaskConfigureJornalEvents" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">

<script type="text/javascript">   
        
    function OnClientCheck()
    {
        var tbox = $get('<%=tboxPolicyName.ClientID%>');
        var expr = new RegExp('<%=VirusBlokAda.CC.Common.RegularExpressions.PolicyName %>');
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

<script type="text/javascript" src="js/Groups/ext-4.1.1/ext-all.js"></script>

  <script language="javascript" type="text/javascript">
        Ext.require(['*']);
        Ext.onReady(function () {
            var TabsExample = {
                init: function () {
                    var tabs = Ext.widget('tabpanel',
                 {
                     renderTo: 'tabs1',
                     height: '800px',
                     plain: true,
                     activeTab: 1,
                     defaults: {
                         bodyPadding: 10
                     },
                     items: []
                 });
                    tabs.add({
                        id: 'paneltab1',

                        title: "<%=Resources.Resource.Management %>",
                        contentEl: 'tab1'
                    }, {
                        id: 'paneltab2',

                        title: "<%=Resources.Resource.AppointmentPolicy %>",
                        contentEl: 'tab2'
                    });
                    tabs.setActiveTab('paneltab2');
                    tabs.setActiveTab('paneltab1');
                }
            }
            Ext.EventManager.onDocumentReady(TabsExample.init);


            /*  ***************************************************************************************************************
            Container and layout generation
            ***************************************************************************************************************  */
            // turn on quick tips

            Ext.tip.QuickTipManager.init();
            Ext.apply(Ext.tip.QuickTipManager.getQuickTip(), {
                maxWidth: 200,
                minWidth: 100,
                showDelay: 500    // Show 500ms after entering target
            });





            /*  ***************************************************************************************************************
            Container and layout generation finished
            ***************************************************************************************************************  */

            /*  ***************************************************************************************************************
            Save and reload realization
            ***************************************************************************************************************  */

            //reloading trees from server
            function reload() {
                noPolicyStore.reload();
                policyStore.reload();
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
                MainPanel.setLoading('<%=Resources.Resource.SendingDataToServer %>' + '...');
                //generate arrays containing nodes info
                var policyTreeArray = new Array();
                policyTreeRoot.eachChild(function (root) {
                    RecursiveSaving(root, root.get("id"));
                });

                function RecursiveSaving(root, policyID) {
                    var length = root.childNodes.length;
                    for (var i = 0; i < length; i++) {
                        RecursiveSaving(root.childNodes[i], policyID);
                    }
                    if (root.getDepth() == 0) return;

                    if (root.isLeaf()) {
                        policyTreeArray[policyTreeArray.length++] = new myNode(root.get("text"), root.get("id"), policyID, "", root.isLeaf() ? "true" : "false");
                    }
                }
                //query .net handler with ajax
                Ext.Ajax.request({
                    url: '<%=Request.ApplicationPath%>/Handlers/GetTreePoliciesDataHandler.ashx',                                                    //url
                    params: { policyTreeArray: Ext.JSON.encode(policyTreeArray) },
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
                            var jsonData = Ext.JSON.decode(response.responseText); //decode response
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
                        MainPanel.setLoading(false);
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
            var policyStore = Ext.create('Ext.data.TreeStore', {
                proxy: {
                    type: 'ajax',
                    url: '<%=Request.ApplicationPath%>/Handlers/TreeWithPolicyHandler.ashx'
                },
                root: {
                    text: 'Policies Root',
                    draggable: false,
                    id: 'policyTreeRoot',
                    root: true,
                    leaf: false,
                    expanded: true
                },
                listeners:
                {
                    move: function (node, parentOld, parentNew, index) {
                        parentNew.removeChild(node, true);
                        RecursiveDeleteGroups(parentOld);

                    }
                },
                folderSort: true,
                sorters: [{
                    property: 'text',
                    direction: 'ASC'
                }]
            });
            var newGroupId = 0;
            var policyTree = Ext.create('Ext.tree.Panel', {
                id: 'policyTree',
                store: policyStore,
                rootVisible: false,
                region: 'east',
                layout: 'fit',
                width: 400,
                minWidth: 300,
                hideHeaders: true,
                border: 1,
                split: true,
                viewConfig:
        {
            markDirty: false,
            listeners:
            {
            },
            plugins:
            {
                ptype: 'treeviewdragdrop',
                toggleOnDblClick: false,
                appendOnly: true
            }
        }
            });
            var policyTreeRoot = policyTree.getRootNode();
            policyTreeRoot.data.allowDrop = false;
            function RecursiveDeleteGroups(root) {
                if (root.childNodes.length == 0 && root.get("id").search('Group') != -1) {
                    var parentNode = root.parentNode;
                    root.remove(true);
                    RecursiveDeleteGroups(parentNode);
                }
            }




            policyTreeRoot.expand(false, false);

            // policy tree selection model
            var sm = policyTree.getSelectionModel();

            //stop event on navigation key pressed

            policyTree.on('keypress', function (e) {
                if (e.isNavKeyPress()) {
                    e.stopEvent();
                }
            });

            //Fires before a new child is appended, return false to cancel the append.        
            policyTree.on('beforeitemmove', function (childNode, parentNodeOld, parentNodeNew, index) {
                onBeforeMove(childNode, parentNodeOld, parentNodeNew, index);
                return true;
            });

            /*  ***************************************************************************************************************
            Policy tree finish
            ***************************************************************************************************************  */

            /*  ***************************************************************************************************************
            No Policy tree
            no policy tree generation, set up and its root creation
            ***************************************************************************************************************  */
            var noPolicyStore = Ext.create('Ext.data.TreeStore', {
                proxy: {
                    type: 'ajax',
                    url: '<%=Request.ApplicationPath%>/Handlers/TreeNoPolicyHandler.ashx'
                },
                root: {
                    text: '<%=Resources.Resource.NotAssignedExplicitly %>',
                    id: 'noPolicyTreeRoot',
                    root: true,
                    expanded: true,
                    allowDrag: false,
                    allowDrop: true
                },
                listeners: {
                    move: function (node, parentOld, parentNew, index) {
                        parentNew.removeChild(node, true);
                        RecursiveDeleteGroups(parentOld);
                    }
                },


                folderSort: true,
                sorters: [{
                    property: 'text',
                    direction: 'ASC'
                }]
            });
            var noPolicyTree = Ext.create('Ext.tree.Panel', {
                id: 'noPolicyTree',
                width: 300,
                height: 400,
                minWidth: 200,
                region: 'center',
                layout: 'fit',
                store: noPolicyStore,
                split: true,
                border: 1,
                viewConfig:
                {
                    markDirty: false,
                    plugins:
                    {
                        ptype: 'treeviewdragdrop',
                        appendOnly: true
                    }
                }
            });
            var noPolicyTreeRoot = noPolicyTree.getRootNode();
            /* ************************************************************************************************
            noPolicyTree and noPolicyStore generation finished
            ********************************************************************************* */

            noPolicyTreeRoot.expand(false, false);

            //Fires before a new child is appended, return false to cancel the append.        
            noPolicyTree.on('beforeitemmove', function (childNode, parentNodeOld, parentNodeNew, index) {
                if (parentNodeNew.id == noPolicyTreeRoot.id) return false;
                onBeforeMove(childNode, parentNodeOld, parentNodeNew, index);
                return true;
            });


            /* Moving nodes*/
            function getId(id) {
                var index = id.indexOf("!");
                var newId;
                if (index == -1) {
                    newId = id;
                }
                else {
                    var newId = id.substring(0, index);
                }
                return newId;
            }
            function changeId(id, parentId) {
                var newId = getId(id);
                newId = newId.concat("!", parentId);
                return newId;
            }

            function onBeforeMove(childNode, parentNodeOld, parentNodeNew, index) {

                var parent = parentNodeOld;
                var arrayNodes = new Array();

                if (!childNode.isLeaf()) //if not LEAF
                    arrayNodes.push(childNode);
                while (parent.data.id.search("Group") != -1) {//no group
                    arrayNodes.push(parent);
                    parent = parent.parentNode;
                }
                parent = parentNodeNew;
                var tmp = new Object();
                var indexNode = -1;
                for (var i = arrayNodes.length - 1; i > -1; i--) {
                    tmp = arrayNodes[i].copy(null, false);
                    tmp.data.id = changeId(arrayNodes[i].get("id"), parent.get("id"));

                    indexNode = FindIndexOfChild(parent, tmp);

                    if (indexNode == -1) {
                        parent.appendChild(tmp);
                        parent = tmp;
                    }

                    else parent = parent.childNodes[indexNode];
                }

                if (childNode.isLeaf()) {
                    if (FindIndexOfChild(parent, childNode) == -1) {
                        var newNode = childNode.copy(null, false)
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
                        var newId = changeId(arrayNodes[i].data.id, root.data.id);
                        newNode = arrayNodes[i].copy(null, false);
                        newNode.data.id = newId;
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
                    var rootChildName = root.childNodes[index].get("text");
                    var nodeName = node.get("text");
                    if (rootChildName == nodeName) return index;
                }
                return -1;
            }

            // create the bottom toolbar
            var bottomToolBar = Ext.create('Ext.toolbar.Toolbar');
            bottomToolBar.suspendLayouts();
            bottomToolBar.add(
            {
                id: 'reload',
                text: '<%=Resources.Resource.Reload %>',
                iconCls: 'reload',  // <-- icon
                tooltip: '<%=Resources.Resource.ReloadInfoFromServer %>',
                handler: reload

            }, '->', {
                id: 'save',
                text: '<%=Resources.Resource.Save %>',
                handler: save,
                iconCls: 'save',
                tooltip: '<%=Resources.Resource.SaveInfoToServer %>'
            }
            );
            bottomToolBar.resumeLayouts(true);


            /*  ***************************************************************************************************************
            No Policy tree finish
            ***************************************************************************************************************  */
            var MainPanel = new Ext.Panel({
                renderTo: 'mainContainer',
                layout: 'border',
                width: 700,
                height: 500,
                border: 1,
                bodyStyle: { 'z-index': 0
                },
                items: [
                policyTree, noPolicyTree,
                {
                    region: 'south',
                    //   layout: 'fit',
                    tbar: bottomToolBar,
                    border: 1
                }]
                /*  bbar: bottomToolBar*/
            });
            bottomToolBar.setBorder('1');
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
                        <asp:DropDownList ID="ddlPolicyNames" style="width:90%;margin-left: 5px; margin-top:10px;" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPolicyNames_SelectedIndexChanged" />
                        <div style="text-align: left;">
                            <asp:LinkButton ID="lbtnCreate" runat="server" OnClick="lbtnCreate_Click" SkinID="Button">
                                <%=Resources.Resource.Create%>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lbtnSaveAs" runat="server" OnClick="lbtnSaveAs_Click" SkinID="Button">
                                <%=Resources.Resource.SaveAs%>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lbtnEdit" runat="server" OnClick="lbtnEdit_Click" SkinID="Button">
                                <%=Resources.Resource.Edit%>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lbtnDelete" runat="server" OnClick="lbtnDelete_Click" SkinID="Button">
                                <%=Resources.Resource.Delete%>
                            </asp:LinkButton>
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
                            <asp:ListItem />
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
            <div style="overflow:scroll;height: 600px;">
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
                                <asp:CheckBox ID="cboxRunMonitor" runat="server" /><%=Resources.Resource.MonitorEnabled %>
                            </td>
                        </tr>
                        </table>
                    </Content>
                </ajaxToolkit:AccordionPane>                
                 <ajaxToolkit:AccordionPane ID="AccordionPane5" runat="server">
                        <Header>
                        <a href="" class="accordionLink"><%=Resources.Resource.JournalEvents%></a>
                        </Header>
                    <Content>                        
                        <uc:TaskConfigureJornalEvents ID="JornalEvents" runat="server" HideHeader="true" />
                    </Content>
                </ajaxToolkit:AccordionPane>
             </Panes>
        </ajaxToolkit:Accordion>
            </div>
        </div>
        <div style="text-align:left; margin:5px; height: 15px;" runat="server" id="divButtons" visible="false">
           <asp:LinkButton ID="lbtnSave" OnClick="lbtnSave_Click" runat="server" SkinID="Button" OnClientClick='return OnClientCheck()' />
           <asp:LinkButton ID="lbtnCancelEditing" OnClick="lbtnCancelEditing_Click" runat="server" SkinID="Button"></asp:LinkButton>
        </div>
      </div>
      <div id='tab2' class="tab-content">
            <div id="mainContainer">
            </div>
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
                                            <asp:Button ID="btnClose" runat="server" Text='<%$Resources:Resource, Close %>'/>                                            
                                        </p>
                                    </div>
                            </asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" X="400" Y="200" TargetControlID="btnHiddenMessage" PopupControlID="pnlModalPopapMessage" CancelControlID="btnClose" BackgroundCssClass="modalBackground" DropShadow="true" PopupDragHandleControlID="pnlModalPopapMessage" />
     
</asp:Content>

