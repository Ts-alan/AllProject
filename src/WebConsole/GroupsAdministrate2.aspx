<%@ Page Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true"
    CodeFile="GroupsAdministrate.aspx.cs" Inherits="GroupsAdministrate" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" runat="Server">
    <script type="text/javascript" src="js/jstree.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

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
            $('#noGroupTree').jstree({
                'core': {
                    'check_callback': function (operation, node, node_parent, node_position, more) {
                        if (operation == 'move_node') {
                            if (node.type == 'group' || node.type == '#') return false;
                            if (node_parent.type != 'group' && node_parent.type != '#') return false;
                        }
                        return true;
                    },
                    'multiple': false,
                    'data': function (node, cb) { cb(loadGroupTreeInfo(false)); }
                },
                'types': {
                    'group': {
                        'icon': "App_Themes/Main/groups/images/group.png"
                    },
                    'computer': {
                        'icon': "App_Themes/Main/groups/images/monitor.png"
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

            $('#groupTree').jstree({
                'core': {
                    'check_callback': function (operation, node, node_parent, node_position, more) {
                        if (operation == 'move_node') {

                            if (node_parent.type != 'group' && node_parent.type != '#') return false;
                            var level = getLevel(node_parent);

                            var innerLevel = getInnerLevel(node);
                            if (level + innerLevel > MAX_DEPTH) {
                                messageBox('<%=Resources.Resource.AddNewGroup %>', '<%=Resources.Resource.ErrorOfNestingLavel %>' + ' (max:' + MAX_DEPTH + ')');

                                return false;
                            }
                        }
                        if (operation == 'rename_node') {
                            if (node.type != 'group') return false;
                            var isSuccess = true;

                            if (node_position.length > MAX_NAME_LENGTH) {
                                messageBox('<%=Resources.Resource.ErrorRenamingGroup%>', '<%=Resources.Resource.LimitedAllowLength%>' + " (max: " + MAX_NAME_LENGTH + ")" + node_position);
                                return false;
                            }
                            else {
                                var root = $('#groupTree').jstree(true).get_node('#');
                                isSuccess = compareNames(root, node_position, node.id);
                                if (isSuccess == false) {
                                    messageBox('<%=Resources.Resource.ErrorRenamingGroup%>', '<%=Resources.Resource.CanNotRename%>' + node_position +
                                        '<%=Resources.Resource.GroupAllreadyExists%>');
                                    return false;
                                }
                            }


                        }
                        return true;
                    },
                    'multiple': false,
                    'data': function (node, cb) { cb(loadGroupTreeInfo(true)); }
                },
                'types': {
                    'group': {
                        'icon': "App_Themes/Main/groups/images/group.png"
                    },
                    'computer': {
                        'icon': "App_Themes/Main/groups/images/monitor.png"
                    },
                    'default': {
                    }
                },
                'dnd': {
                    'is_draggable': function (node) { return true; }
                },
                "contextmenu": {
                    "items": function (node) {
                        if (node.type != 'group' && node.type != '#') return false;
                        return {
                            addGroup: {
                                "label": "<%=Resources.Resource.Add %>",
                                "icon": "add-opt",
                                "action": function (obj) { return addNewGroup(); }
                            },
                            renameGroup: {
                                "label": "<%=Resources.Resource.Rename %>",
                                "icon": "rename",
                                "action": function (obj) { return renameNode(); }
                            },
                            deleteGroup: {
                                "label": "<%=Resources.Resource.Delete %>",
                                "icon": "remove",
                                "action": function (obj) { return deleteGroup(); }
                            },
                            commentGroup: {
                                "label": "<%=Resources.Resource.Comment %>",
                                "icon": "comment",
                                "action": function (obj) { return commentGroup(); }
                            }
                        };
                    }
                },
                'plugins': ["state", "types", "dnd", "sort", "contextmenu"]

            });

            $('#groupTree').on('dblclick.jstree', function (e, data) {
                renameNode();
            });
            $('#groupTree').on('loaded.jstree', function (e, data) {
                var ref = $('#groupTree').jstree(true);
                var treeRoot = ref.get_node('#');
                var title = "";
                for (var i = 0; i < treeRoot.children_d.length; i++) {
                    title = ref.get_node(treeRoot.children_d[i]).original.qtip;
                    $("#" + treeRoot.children_d[i]).prop('title', title);
                }
            });
            $('#noGroupTree').on('loaded.jstree', function (e, data) {
                var ref = $('#noGroupTree').jstree(true);
                var treeRoot = ref.get_node('#');
                var title = "";
                for (var i = 0; i < treeRoot.children_d.length; i++) {
                    title = ref.get_node(treeRoot.children_d[i]).original.qtip;
                    $("#" + treeRoot.children_d[i]).prop('title', title);
                }
            });

            $('#noGroupTree').on('copy_node.jstree', function (e, data) {
                data.node.original = data.original.original;
                data.instance.set_id(data.node, data.original.id);
                $('#' + data.node.id).prop('title', data.node.original.qtip);

            });
            $('#groupTree').on('copy_node.jstree', function (e, data) {
                data.node.original = data.original.original;
                data.instance.set_id(data.node, data.original.id);
                $('#' + data.node.id).prop('title', data.node.original.qtip);

            });
            $('#groupTree').on('select_node.jstree', function (e, data) {
                if (data.node.type != 'group' && data.node.type != '#') setButtonsEnabled(false);
                else setButtonsEnabled(true);
            });
            $('#groupTree').on('hover_node.jstree', function (e, data) {

                $("#" + data.node.id).prop('title', data.node.original.qtip);
            });
            $('#noGroupTree').on('hover_node.jstree', function (e, data) {

                $("#" + data.node.id).prop('title', data.node.original.qtip);
            });
        });

        function loadGroupTreeInfo(isGroup) {
            var d = "";
            var handlerUrl = "";
            if (isGroup) {
                handlerUrl = '<%=Request.ApplicationPath%>/Handlers/TreeWithGroupsHandler.ashx';
            } else handlerUrl = '<%=Request.ApplicationPath%>/Handlers/TreeNoGroupsHandler.ashx';

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

        var newGroupID = 0;
        var MAX_DEPTH = 5;
        var MAX_NAME_LENGTH = 64;
        function getLevel(node) {
            var ref = $('#groupTree').jstree(true);
            var level = 0;
            var parent = node.id;
            while (parent != '#') {
                parent = ref.get_node(parent).parent;
                level++;
            }
            return level;
        }
        function getInnerLevel(node) {
            var ref = $('#groupTree').jstree(true);
            if (node.type == null)
                node = ref.get_node(node);
            if (node.type != 'group' && node.type != '#') return 0;
            else {
                return 1 + Math.max(getInnerLevel(node.children));
            }
        }


        function addNewGroup() {
            var ref = $('#groupTree').jstree(true),
			sel = ref.get_selected(true);
            var treeRoot = ref.get_node('#');
            if (!sel.length) {
                sel = treeRoot;
            }
            else {
                sel = sel[0];
                if (sel.type != 'group') return false;
            }
            var level = getLevel(sel);
            if (level == MAX_DEPTH) {
                return false;
            }
            newGroupID++;
            var success = false;
            var length = treeRoot.children.length;
            var newGroupNumber = 0;
            var newGroupName;
            while (!success) {
                success = true;
                if (newGroupNumber == 0) {
                    newGroupName = 'New group';
                }
                else {
                    newGroupName = 'New group' + '(' + (newGroupNumber) + ')';
                }
                for (var i = 0; i < length; i++) {
                    if (compareNames(treeRoot, newGroupName, newGroupID) == false) {
                        success = false;
                        break;
                    }
                }
                newGroupNumber++;
            }


            sel = ref.create_node(sel, { "text": newGroupName, "id": 'GroupNew_' + (newGroupID), "allowDrag": "true", "type": "group", "qtip": "No comment" });
            if (sel) {
                ref.edit(sel);
            }
            ref.deselect_all();
            ref.select_node(sel);
        };
        function compareNames(node, name, id) {
            var length = node.children.length;
            var childNode;
            for (var i = 0; i < length; i++) {
                childNode = $('#groupTree').jstree('get_node', node.children[i]);
                if ((childNode.text == name && childNode.id != id)) {
                    return false;
                    break;
                }

                if (childNode.type == "group" && compareNames(childNode, name, id) == false)
                    return false;
            }
            return true;
        };


        function renameNode() {
            var ref = $('#groupTree').jstree(true),
			sel = ref.get_selected(true);
            if (!sel.length) { return false; }
            sel = sel[0];
            if (sel.type != 'group') return false;
            ref.edit(sel);
        };

        function deleteGroup() {
            var ref = $('#groupTree').jstree(true);
            var groupRoot = $('#groupTree').jstree('get_node', '#');
            var noGroupRoot = $('#noGroupTree').jstree('get_node', '#');
            sel = ref.get_selected(true);
            if (!sel.length) { return false; }
            sel = sel[0];
            if (sel.id == '#') return false;
            if (sel.type != 'group') return false;
            var childNode;
            while (sel.children.length > 0) {
                childNode = $('#groupTree').jstree('get_node', sel.children[0]);
                if (childNode.type == 'group') {
                    $('#groupTree').jstree('move_node', childNode, groupRoot);
                }

                else {
                    $('#noGroupTree').jstree('move_node', childNode, noGroupRoot);
                }

            }
            ref.delete_node(sel.id);
        };

        function commentGroup() {
            var ref = $('#groupTree').jstree(true);
            sel = ref.get_selected(true);
            if (!sel.length) { return false; }
            sel = sel[0];
            if (sel.id == '#') return false;
            if (sel.type != 'group') return false;
            var comment = sel.original.qtip;
            if (comment == 'No comment') {
                comment = '';
            }
            var dOpt = {
                width: 300,
                modal: true,
                title: '<%=Resources.Resource.Comment%>',
                resizable: false,
                close: function (event, ui) {
                },
                buttons: {
                    '<%=Resources.Resource.Apply%>': function () {
                        var newComment = $('#commentEditBoxInput').val();
                        if (newComment == '') {
                            newComment = 'No comment';
                        }
                        sel.original.qtip = newComment;
                        $("#" + sel.id).prop('title', newComment);
                        $('#commentEditBox').dialog('close');

                    },
                    '<%=Resources.Resource.CancelButtonText%>': function () {
                        $('#commentEditBox').dialog('close');
                    }
                }
            };
            $('#commentEditBoxInput').val(comment);
            $('#commentEditBox').dialog(dOpt);
        };
        function reloadTrees() {
            $('#groupTree').jstree('refresh');
            $('#noGroupTree').jstree('refresh');
        };
        function myNode(text, id, parentId, comment, isLeaf) {
            this.text = text;
            this.id = id;
            this.parentId = parentId;
            this.comment = comment;
            this.isLeaf = isLeaf;
        };
        function saveTrees() {


            /*  MainPanel.setLoading('<%=Resources.Resource.SendingDataToServer %>' + '...');*/
            //generate arrays containing nodes info
            var groupTreeArray = new Array();
            var ref = $('#groupTree').jstree(true);
            var groupTreeRoot = $('#groupTree').jstree('get_node', '#');
            RecursiveSaving(groupTreeRoot);
            var noGroupTreeArray = new Array();
            var noGroupRoot = $('#noGroupTree').jstree('get_node', '#');
            for (var i = 0; i < noGroupRoot.children.length; i++) {
                comp = $('#noGroupTree').jstree('get_node', noGroupRoot.children[i]);
                noGroupTreeArray[noGroupTreeArray.length++] = new myNode(comp.text, comp.id, '0', '');
            }
            function RecursiveSaving(root) {
                var length = root.children.length;
                for (var i = 0; i < length; i++) {
                    RecursiveSaving($('#groupTree').jstree('get_node', root.children[i]));
                }
                if (getLevel(root) == 0) return;
                var comment = '';
                if (root.type == 'group') comment = root.original.qtip;
                if (comment == 'No comment') {
                    comment = '';
                }
                groupTreeArray[groupTreeArray.length++] = new myNode(root.text, root.id, root.parent, comment, root.type != 'group' ? "true" : "false");
            }
            var groupJSON = JSON.stringify(groupTreeArray);
            var noGroupJSON = JSON.stringify(noGroupTreeArray);
            $.ajax({
                type: "POST",
                url: '<%=Request.ApplicationPath%>/Handlers/GetTreeDataHandler.ashx',
                dataType: "json",
                data: { 'groupTreeArray': groupJSON, 'noGroupTreeArray': noGroupJSON },
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
        function setButtonsEnabled(enabled) {

            if (enabled) {
                $('#groupTreeToolbarCommentButton').removeClass('x-btn-disabled');
                $('#groupTreeToolbarCommentButton').attr('forbutton', true);
                $('#groupTreeToolbarAddButton').removeClass('x-btn-disabled');
                $('#groupTreeToolbarAddButton').attr('forbutton', true);
                $('#groupTreeToolbarRenameButton').removeClass('x-btn-disabled');
                $('#groupTreeToolbarRenameButton').attr('forbutton', true);
                $('#groupTreeToolbarDeleteButton').removeClass('x-btn-disabled');
                $('#groupTreeToolbarDeleteButton').attr('forbutton', true);
            }
            else {
                $('#groupTreeToolbarCommentButton').addClass('x-btn-disabled');
                $('#groupTreeToolbarCommentButton').attr('forbutton', false);
                $('#groupTreeToolbarAddButton').addClass('x-btn-disabled');
                $('#groupTreeToolbarAddButton').attr('forbutton', false);
                $('#groupTreeToolbarRenameButton').addClass('x-btn-disabled');
                $('#groupTreeToolbarRenameButton').attr('forbutton', false);
                $('#groupTreeToolbarDeleteButton').addClass('x-btn-disabled');
                $('#groupTreeToolbarDeleteButton').attr('forbutton', false);
            }
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
    <div class="title">
        <%=Resources.Resource.GroupManagment%>
    </div>
    <div id="mainContainer" style="width: 800px; height: 500px;">
        <div id="treePanel" class="tree-panel-body" style="z-index: 0; width: 800px; height: 500px; left: 0px; top: 0px;">
            <div id="noGroupTreePanel" class="x-panel x-box-item" style="width: 300px; height: 475px; margin: 0px; left: 0px; top: 0px;">
                <div id="noGroupTreePanelHeader" class=" x-docked x-panel-header-default" style=" left: 0px; top: 0px;right:0px;">
                    <span class=" x-panel-header-text-default" ><%=Resources.Resource.ComputersWithoutGroups %></span>
                </div>
                <div id="noGroupTreeBody" class="tree-panel-body" style=" left: 0px; top: 25px; height: 450px;right:0px">
                    <div id="noGroupTree" style=""></div>
                </div>
            </div>

            <div id="groupTreePanel" class="x-panel x-box-item" style="width: 500px; height: 475px; margin: 0px; left: 300px; top: 0px;">
                <div id="groupTreePanelHeader" class="x-docked x-panel-header-default" style="width: 500px; left: 0px; top: 0px;">
                    <span class="x-panel-header-text-default" ><%=Resources.Resource.ComputersWithGroups %></span>
                </div>
                <div id="groupTreeToolbar" class="x-toolbar x-docked x-toolbar-default" style="width: 500px; left: 0px; top: 25px; height:25px;">
                    <div id="groupTreeToolbarAddButton" forbutton='true' class="x-btn  x-btn-default-toolbar-small x-btn-default-toolbar-small-icon-text-left"  style="border-width: 1px;  top: 0px; margin: 0px;float:left">
                        <button id="addGroupButton" class="x-btn-center" type="button" onclick="return addNewGroup();">
                            <span class="x-btn-icon add-opt"></span>
                            <span class="x-btn-inner"><%=Resources.Resource.Add %></span>
                        </button>
                    </div>
                    <div id="groupTreeToolbarRenameButton" forbutton='false' class="x-btn  x-btn-default-toolbar-small x-btn-default-toolbar-small-icon-text-left x-btn-disabled"  style="border-width: 1px; top: 0px; margin: 0px;float:left">
                        <button id="renameGroupButton" class="x-btn-center" type="button" onclick="return renameNode();">
                            <span class="x-btn-icon rename"></span>
                            <span class="x-btn-inner"><%=Resources.Resource.Rename %></span>
                        </button>
                    </div>
                    <div id="groupTreeToolbarDeleteButton" forbutton='false' class="x-btn x-btn-default-toolbar-small x-btn-default-toolbar-small-icon-text-left x-btn-disabled"  style="border-width: 1px; top: 0px; margin: 0px;float:left">
                        <button id="deleteGroupButton" class="x-btn-center" type="button" onclick="return deleteGroup();">
                            <span class="x-btn-icon remove"></span>
                            <span class="x-btn-inner"><%=Resources.Resource.Delete %></span>
                        </button>
                    </div>
                    <div id="groupTreeToolbarCommentButton" forbutton='false' class="x-btn x-btn-default-toolbar-small x-btn-default-toolbar-small-icon-text-left x-btn-disabled"  style="border-width: 1px; top: 0px; margin: 0px;float:left">
                        <button id="commentGroupButton" class="x-btn-center" type="button" onclick="return commentGroup();">
                            <span class="x-btn-icon comment"></span>
                            <span class="x-btn-inner"><%=Resources.Resource.Comment %></span>
                        </button>
                    </div>
                </div>
                <div id="groupTreeBody" class="tree-panel-body x-grid-body x-layout-fit" style="width: 500px; left: 0px; top: 55px; height: 444px;">
                    <div id="groupTree" style="height:300px"></div>
                </div>
            </div>
            <div class="x-toolbar x-docked x-toolbar-default" style="border-width: 1px; left: 0px; bottom:0px;right:0px;">
                <div style="width: 794px; height: 22px;" class="x-box-inner " role="presentation">
                    <div  style="position:absolute;width:20000px;left:0px;top:0px;height:1px"></div>
                        <div forbutton='true' class="x-btn x-btn-default-toolbar-small x-btn-default-toolbar-small-icon-text-left" style="border-width: 1px; left: 0px; top: 0px; margin: 0px;float:left" id="reloadButtonToolbar">
                            <button id="reloadTreesButton" style="height: 16px;"  type="button" class="x-btn-center" hidefocus="true" role="button" autocomplete="off" onclick="return reloadTrees();">
                                <span class="x-btn-icon reload"></span>
                                <span class="x-btn-inner"><%=Resources.Resource.Reload %></span>                                
                            </button>
                        </div>                     
                        <div forbutton='true' class="x-btn  x-btn-default-toolbar-small x-btn-default-toolbar-small-icon-text-left" style="border-width: 1px; top: 0px;margin:0px;float:right" id="saveButtonToolbar">
                            <button   id="saveTreesButton" type="button" class="x-btn-center"  onclick="return saveTrees();">
                                <span class="x-btn-icon save"></span>
                                <span class="x-btn-inner"><%=Resources.Resource.Save %></span>                                
                            </button>
                        </div>
                </div>
            </div>
        </div>
    </div>
    <div id="commentEditBox" style="display:none">
        <p> <%=Resources.Resource.ChangeComment %></p>
        <input id="commentEditBoxInput" type="text" style="width:280px"/>
    </div>
    <div id="dialog-message"><p id="messageText"/></div>
</asp:Content>