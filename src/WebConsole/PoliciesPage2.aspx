<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" MasterPageFile="~/mstrPageMain.master" CodeFile="PoliciesPage2.aspx.cs" Inherits="_PoliciesPage" %>

<%@ Register Assembly="PagingControl" Namespace="PagingControls" TagPrefix="cc1" %>

<%@ Register Src="Controls/TaskConfigureMonitor.ascx" TagName="TaskConfigureMonitor"    TagPrefix="uc" %>
<%@ Register Src="Controls/TaskConfigureQuarantine.ascx" TagName="TaskConfigureQuarantine"    TagPrefix="uc" %>
<%@ Register Src="Controls/TaskConfigureLoader.ascx" TagName="TaskConfigureLoader"    TagPrefix="uc" %>
<%@ Register Src="~/Controls/TaskConfigureJornalEvents.ascx" TagName="TaskConfigureJornalEvents" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">

 <script type="text/javascript" src="js/jstree.js"></script>
    <script language="javascript" type="text/javascript">
        function OnClientCheck() {
            var tbox = $get('<%=tboxPolicyName.ClientID%>');
            var expr = new RegExp('<%=VirusBlokAda.CC.Common.RegularExpressions.PolicyName %>');
            if (!expr.test(tbox.value)) {
                alert('<%=Resources.Resource.ErrorPolicyName%>');
                return false;
            }
            else {
                return true;
            }
        }
        $(document).ready(function () {
            $("#tabs1").tabs({ cookie: { expires: 30} });
            $(document).tooltip({
                content: function () {
                    if ($(this).prop('tagName') == 'LI')
                        return $(this).attr('title');
                }
            });
            $("input[type=button]").button();

            $("div[forbutton]").hover(
                function () {
                    if ($(this).attr('forbutton') == 'true')
                        $(this).addClass('button-hover');

                },
                function () {
                    $(this).removeClass('button-hover');
                }
            );


            $.jstree.defaults.checkbox.whole_node = false;
            $.jstree.defaults.checkbox.tie_selection = false;
            $.jstree.defaults.dnd.always_copy = false;
            $('#noPolicyTree').jstree({
                'core': {
                    'check_callback': function (operation, node, node_parent, node_position, more) {
                        if (operation == 'move_node') {
                            if (node.type == 'root' || node.type == 'folder') return false;
                            if (node_parent.type != 'folder' && node_parent.type != 'root') return false;
                            if (node == false) return false;

                            var parChild = node_parent.children_d;
                            if (parChild.indexOf(node.id) >= 0) return false;
                            return true;
                        }

                        if (operation == 'delete_node') {

                            return true;
                        }
                        if (operation == 'copy_node') {
                            if (node.type == 'root') return false;
                            if (node_parent.type != 'folder' && node_parent.type != 'root') return false;
                            if (node == false) return false;
                            nodeMoving(node, node_parent, false, true);
                            return false;

                        }
                        return true;
                    },
                    'multiple': false,
                    'data': function (node, cb) { cb(loadPolicyTreeInfo(false)); }
                },
                'types': {
                    'group': {
                        'icon': "App_Themes/Main/groups/images/group.png"
                    },
                    'computer': {
                        'icon': "App_Themes/Main/groups/images/monitor.png"
                    },
                    'root': {
                    },
                    'default': {
                    }
                },
                'plugins': ["state", "types", "dnd", "sort"]

            });

            $.jstree.defaults.sort = function (a, b) {
                a = this.get_node(a);
                b = this.get_node(b);
                if (a.type == 'group' && b.type != 'group') return -1;
                if (a.type != 'group' && b.type == 'group') return 1;
                return this.get_text(a) > this.get_text(b) ? 1 : -1;
            };

            $('#policyTree').jstree({
                'core': {
                    'check_callback': function (operation, node, node_parent, node_position, more) {
                        if (operation == 'move_node') {

                            if (node.type == 'root' || node.type == 'folder') return false;

                            if (node_parent.type != 'folder') return false;
                            if (node == false) return false;
                            var nodeParents = node.parents;
                            var root = this.get_node('#');
                            /* move to own parent*/
                            if (nodeParents.indexOf(node_parent.id) >= 0) return false;
                            console.log(more);
                            /* node and parent from one tree*/
                            if (more.is_multi ==false) {
                                if (more.core == true) {
                                    nodeMoving(node, node_parent, true, true);
                                    return false;
                                }
                            }

                            return true;

                        }
                        if (operation == 'copy_node') {
                            if (node.type == 'root') return false;
                            if (node_parent.type != 'folder' && node_parent.type != 'root') return false;
                            if (node == false) return false;

                            nodeMoving(node, node_parent, true, false);
                            return false;

                        }
                        return true;
                    },
                    'multiple': false,
                    'data': function (node, cb) { cb(loadPolicyTreeInfo(true)); }
                },
                'types': {
                    'group': {
                        'icon': "App_Themes/Main/groups/images/group.png"
                    },
                    'computer': {
                        'icon': "App_Themes/Main/groups/images/monitor.png"
                    },
                    'folder': {
                        'icon': "App_Themes/Main/groups/images/folder.gif"
                    },
                    'root': {
                    },
                    'default': {
                    }
                },
                'dnd': {
                    'is_draggable': function (node) { return true; }
                },
                'plugins': ["state", "types", "dnd", "sort"]

            });

            $('#policyTree').on('loaded.jstree', function (e, data) {
                var ref = $('#policyTree').jstree(true);
                var treeRoot = ref.get_node('#');
                var title = "";
                for (var i = 0; i < treeRoot.children_d.length; i++) {
                    title = ref.get_node(treeRoot.children_d[i]).original.qtip;
                    $("#" + treeRoot.children_d[i]).prop('title', title);
                }
            });
            $('#noPolicyTree').on('loaded.jstree', function (e, data) {
                var ref = $('#noPolicyTree').jstree(true);
                var treeRoot = ref.get_node('#');
                var title = "";
                for (var i = 0; i < treeRoot.children_d.length; i++) {
                    title = ref.get_node(treeRoot.children_d[i]).original.qtip;
                    $("#" + treeRoot.children_d[i]).prop('title', title);
                }
            });

            $('#policyTree').on('hover_node.jstree', function (e, data) {

                $("#" + data.node.id).prop('title', data.node.original.qtip);
            });
            $('#noPolicyTree').on('hover_node.jstree', function (e, data) {

                $("#" + data.node.id).prop('title', data.node.original.qtip);
            });
        });

        function loadPolicyTreeInfo(isPolicy) {
            var d = "";
            var handlerUrl = "";
            if (isPolicy) {
                handlerUrl = '<%=Request.ApplicationPath%>/Handlers/TreeWithPolicyHandler.ashx';
            } else handlerUrl = '<%=Request.ApplicationPath%>/Handlers/TreeNoPolicyHandler.ashx';

            $.ajax({
                type: "GET",
                async: false,
                url: handlerUrl,
                dataType: "json",
                data: {},
                success: function (data) {
                    d = data;
                },
                error: function (e) {
                    alert('<%= Resources.Resource.ErrorRequestingDataFromServer%>');
                }
            });
            return d;
        };



        function getId(id) {
            var index = id.indexOf("+");
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
            newId = newId.concat("+", parentId);
            return newId;
        }

        function FindIndexOfChild(root, node, ref) {

            for (var index = 0; index < root.children.length; index++) {
                var childNode = ref.get_node(root.children[index]);
                var rootChildName = childNode.text;
                var nodeName = node.text;
                if (rootChildName == nodeName) return index;
            }
            return -1;
        }


        function nodeMoving(childNode, newParent, fromPolicy,isToPolicy) {
            var oldRef, ref;
            
            if (fromPolicy == false) {
                oldRef = $('#noPolicyTree').jstree(true);
                
            }
            else {
                
                oldRef = $('#policyTree').jstree(true);
            }
            if (isToPolicy==true) {
                ref = $('#policyTree').jstree(true);
            }
            else {
                ref = $('#noPolicyTree').jstree(true);
            }
            var parent;
            var arrayNodes = new Array();
            var deleteNodes = new Array();
            arrayNodes.push(childNode);
            parent = ref.get_node(childNode.parent);
            while (parent.type == 'group') {

                arrayNodes.push(parent);
                parent = ref.get_node(parent.parent);
            }
            parent = newParent;
            var tmp = new Object();
            var indexNode = -1;
            var oldId, oldOriginal;
            for (var i = arrayNodes.length - 1; i > -1; i--) {
                tmp = new Object();
                tmp = copyNodeInfo(arrayNodes[i], parent.id);
                oldId = tmp.id;

                indexNode = FindIndexOfChild(parent, tmp, oldRef);

                if (indexNode == -1) {
                    oldOriginal = tmp.original;
                    var newId = oldRef.create_node(parent,tmp);                    
                    tmp = oldRef.get_node(newId);
                    oldRef.set_id(tmp, oldId);
                    tmp.original = oldOriginal;
                    if (tmp.type != 'computer')
                        parent = tmp;                    
                }
                else {
                    parent = oldRef.get_node(parent.children[indexNode]);                   
                }
            }

            if (childNode.type == 'group') {
                recursiveSaveChildNodes(parent, childNode.children, deleteNodes, ref, oldRef);
            }
            deleteNodes = deleteNodes.concat(arrayNodes);

            deleteOldNodes(deleteNodes, ref);
            setRightIdComps(ref);
        }

        function recursiveSaveChildNodes(root,childNodes,arrayNodes,ref,oldRef) {

            for (var i = 0; i < childNodes.length; i++) {
                var tmp = new Object();
                var child = ref.get_node(childNodes[i]);
                tmp = copyNodeInfo(child, root.id);
                oldId = tmp.id;
                indexNode = FindIndexOfChild(root, tmp, oldRef);
                if (indexNode == -1) {
                    oldOriginal = tmp.original;
                    var newId = oldRef.create_node(root,tmp);
                    tmp = oldRef.get_node(newId);
                    oldRef.set_id(tmp, oldId);
                    tmp.original = oldOriginal;
                }
                else {
                    tmp=oldRef.get_node(root.children[indexNode]);
                }
                recursiveSaveChildNodes(tmp, child.children, arrayNodes, ref, oldRef);
                arrayNodes.push(child);
            }
        }

        function copyNodeInfo(node,parentId) {
            var tmp = new Object();
            tmp.a_attr = node.attr;
            tmp.children = [];
            tmp.children_d = [];
            tmp.data = null;
            tmp.icon = node.icon;
            tmp.original = node.original;
            tmp.parent = parentId;
            tmp.parents = [];
            tmp.parents.push(parentId);
            tmp.state = node.state;
            tmp.text = node.text;
            tmp.type = node.type;
            tmp.li_attr = new Object();
            if (tmp.type == 'computer') {
                tmp.li_attr.id = node.li_attr.id+"+";
                tmp.id = node.id+"+";
            }
            else {
                tmp.li_attr.id = node.li_attr.id + "+" + parentId;
                tmp.id = node.id + "+" + parentId;
            }
            return tmp;
        }

        function deleteOldNodes(arrayNodes, ref) {
            for (var i = 0; i < arrayNodes.length; i++) {
                if (arrayNodes[i].type == 'computer') {
                    
                    ref.delete_node(arrayNodes[i]);
                }else if (arrayNodes[i].children.length == 0) {
                    ref.delete_node(arrayNodes[i]);
                } else {
                return;
                }
            }
        }
        function setRightIdComps(ref) {
            var root = ref.get_node('#');
            var node;
            for (var i = 0; i < root.children_d.length; i++) {
                node = ref.get_node(root.children_d[i]);
                if (node.type == 'computer') {
                    
                    var id = node.id;
                    if (id[id.length - 1] == '+') {
                        id = id.substr(0, id.length - 1);
                        ref.set_id(node, id);
                    }
                }
            }
        }

        function reloadTrees() {
            $('#policyTree').jstree('refresh');
            $('#noPolicyTree').jstree('refresh');
        };
        function myNode(text, id, parentId, comment, isLeaf) {
            this.text = text;
            this.id = id;
            this.parentId = parentId;
            this.comment = comment;
            this.isLeaf = isLeaf;
        };
        function saveTrees() {
            //generate arrays containing nodes info
            var policyTreeArray = new Array();
            var ref = $('#policyTree').jstree(true);
            var policyTreeRoot = $('#policyTree').jstree('get_node', '#');
            for (var i = 0; i < policyTreeRoot.children.length; i++) {
                var policy = $('#policyTree').jstree('get_node', policyTreeRoot.children[i]);
                RecursiveSaving(policy, policy.id);
            }
            var noPolicyTreeArray = new Array();
            var noGroupRoot = $('#noPolicyTree').jstree('get_node', '#');
            for (var i = 0; i < noGroupRoot.children.length; i++) {
                comp = $('#noPolicyTree').jstree('get_node', noGroupRoot.children[i]);
                noPolicyTreeArray[noPolicyTreeArray.length++] = new myNode(comp.text, comp.id, '0', '');
            }
            function RecursiveSaving(root,parentId) {
                var length = root.children.length;
                for (var i = 0; i < length; i++) {
                    RecursiveSaving($('#policyTree').jstree('get_node', root.children[i]),parentId);
                }
                if (root.type=='#') return;
                var comment = '';
                if(root.type=='computer')
                    policyTreeArray[policyTreeArray.length++] = new myNode(root.text, root.id, parentId, comment, root.type != 'group' ? "true" : "false");
            }
            var policyJSON = JSON.stringify(policyTreeArray);
            var noGroupJSON = JSON.stringify(noPolicyTreeArray);
            $.ajax({
                type: "POST",
                url: '<%=Request.ApplicationPath%>/Handlers/GetTreePoliciesDataHandler.ashx',
                dataType: "json",
                data: { 'policyTreeArray': policyJSON },
                success: function (d) {

                    var resultMessage = d.data.result;                   //passed result string
                    var resultSuccess = d.success;                       //passed query result
                    if (resultSuccess == true) {                          //added info to database successfully

                        messageBox('<%=Resources.Resource.SaveInfoToServer %>', '<%=Resources.Resource.TaskStateCompletedSuccessfully %>');

                    }
                    else {
                        messageBox('<%=Resources.Resource.SaveInfoToServer %>', '<%=Resources.Resource.Error %>' + ': ' + resultMessage);
                        //failed to add info to database
                    }
                },
                error: function (e) {
                    messageBox('<%=Resources.Resource.SaveInfoToServer %>', '<%=Resources.Resource.Error %>' + ': no answer from server');
                }
            });
        };

        function messageBox(title, text) {
            $('#messageText').html(text);
            $("#dialog-message").dialog({
                title: title,
                modal: true,
                width: 350,
                resizable: false,
                buttons: {
                    Ok: function () {
                        $(this).dialog('close');
                    }
                }
            });
        };
    </script>

<div class="title"><%=Resources.Resource.PolicySettings%></div>
<div id='tabs1'>      
    <ul>
        <li><a href="#tab1"><%=Resources.Resource.Management %></a> </li>
        <li><a href="#tab2"><%=Resources.Resource.AppointmentPolicy %></a> </li>    
    </ul>
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
        <div id="mainContainer" style="width: 800px; height: 500px;">
            <div id="treePanel" class="tree-panel-body" style="z-index: 0; width: 700px; height: 500px; left: 0px; top: 0px;">
                <div id="noPolicyTreePanel" class="x-panel x-box-item" style="width: 300px; height: 475px; margin: 0px; left: 0px; top: 0px;">
                    <div id="noPolicyTreeBody" class="tree-panel-body" style=" left: 0px; top: 0px; height: 475px;right:0px">
                        <div id="noPolicyTree" style=""></div>
                    </div>
                </div>

                <div id="policyTreePanel" class="x-panel x-box-item" style="width: 400px; height: 475px; margin: 0px; left: 300px; top: 0px;">           
                    <div id="policyTreeBody" class="tree-panel-body x-grid-body x-layout-fit" style="width: 400px; left: 0px; top: 0px; height: 475px;">
                        <div id="policyTree" style="height:400px"></div>
                    </div>
                </div>
                <div class="x-toolbar x-docked x-toolbar-default" style="border-width: 1px; left: 0px; bottom:0px;right:0px;">
                    <div style="width: 694px; height: 22px;" class="x-box-inner " role="presentation">
                        <div  style="position:absolute;width:20000px;left:0px;top:0px;height:1px"></div>
                        <div forbutton='true' class="x-btn x-btn-default-toolbar-small x-btn-default-toolbar-small-icon-text-left" style="border-width: 1px; left: 0px; top: 0px; margin: 0px;float:left" id="reloadButtonToolbar">
                            <button id="reloadTreesButton" style="height: 16px;"  type="button" class="x-btn-center" hidefocus="true" role="button" autocomplete="off" onclick="return reloadTrees();">
                                <span class="x-btn-icon reload"></span>
                                <span class="x-btn-inner"><%=Resources.Resource.Reload %></span>                                
                            </button>
                        </div>                     
                        <div forbutton='true' class="x-btn  x-btn-default-toolbar-small x-btn-default-toolbar-small-icon-text-left" style="border-width: 1px; top: 0px;margin:0px;float:right" id="saveButtonToolbar">
                            <button id="saveTreesButton" type="button" class="x-btn-center"  onclick="return saveTrees();">
                                <span class="x-btn-icon save"></span>
                                <span class="x-btn-inner"><%=Resources.Resource.Save %></span>                                
                            </button>
                        </div>
                    </div>
                </div>
            </div>
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

