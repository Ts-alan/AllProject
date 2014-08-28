<%@ Page Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true"
    CodeFile="GroupsAdministrate2.aspx.cs" Inherits="GroupsAdministrate" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" runat="Server">
    <script type="text/javascript" src="js/jstree.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            $(document).tooltip({
                content: function () {
                    return $(this).attr('title');
                }
            });
            $("input[type=button]").button();

            $.jstree.defaults.checkbox.whole_node = false;
            $.jstree.defaults.checkbox.tie_selection = false;
            $('#noGroupTree').jstree({
                'core': {
                    'check_callback': function (operation, node, node_parent, node_position, more) {
                        if (operation == 'move_node') {
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
            $(document).on('dnd_stop.vakata', function (e, data) {
                console.log(data);
                var ccc = $('#groupTree').jstree('get_node', data.data.nodes[0]);
                console.log(ccc);
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
                                    messageBox('<%=Resources.Resource.ErrorRenamingGroup%>', '<%=Resources.Resource.CanNotRename%>' + e.originalValue +
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
                'plugins': ["state", "types", "dnd", "sort", "contextmenu"]

            });

            $('#groupTree').on('dblclick.jstree', function (e, data) {
                renameNode();
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
                
                if (childNode.type=="group" && compareNames(childNode, name, id) == false)
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
            var groupRoot=$('#groupTree').jstree('get_node', '#');
            var noGroupRoot=$('#noGroupTree').jstree('get_node', '#');
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
                title:'<%=Resources.Resource.Comment%>',
                resizable: false,
                close: function (event, ui) {
                },
                buttons: {
                    '<%=Resources.Resource.Apply%>': function () {
                        var newComment = $('#commentEditBoxInput').val();
                        if (newComment == '') {
                            newComment = 'No comment';
                        }
                        sel.original.qtip=newComment;
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
            var groupTreeRoot=$('#groupTree').jstree('get_node', '#');
            RecursiveSaving(groupTreeRoot);
            var noGroupTreeArray = new Array();
            var noGroupRoot=$('#noGroupTree').jstree('get_node', '#');
            for(var i=0;i<noGroupRoot.children.length;i++) {
                comp=$('#noGroupTree').jstree('get_node',noGroupRoot.children[i]);
                noGroupTreeArray[noGroupTreeArray.length++] = new myNode(comp.text, comp.id, '0', '');
            }
                function RecursiveSaving(root) {
                    var length = root.children.length;
                    for (var i = 0; i < length; i++) {
                        RecursiveSaving($('#groupTree').jstree('get_node', root.children[i]));
                    }
                    if (getLevel(root) == 0) return;
                    var comment = '';
                    if (root.type=='group') comment = root.original.qtip;
                    if (comment == 'No comment') {
                        comment = '';
                    }
                    groupTreeArray[groupTreeArray.length++] = new myNode(root.text, root.id, root.parent, comment, root.type!='group' ? "true" : "false");
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
            function messageBox(title,text) {
                $('#messageText').html(text);
                $("#dialog-message").dialog({
                    title: title,
                    modal: true,
                    width: 250,
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
        <table >
            <thead>
                <th><%=Resources.Resource.ComputersWithoutGroups %> </th>
                <th><%=Resources.Resource.ComputersWithGroups %></th>
            </thead>
            <tr>
                <td>
                    <div id="noGroupTree">
                        <ul>
                            <li><%=Resources.Resource.Computers %></li>
                        </ul>
                    </div>
                </td>
                <td>
                    <p>
                        <input type="button" id="addGroupButton" value="<%=Resources.Resource.Add %>" title="<%=Resources.Resource.AddNewGroup %>" onclick="return addNewGroup();"/>
                        <input type="button" id="renameGroupButton" value="<%=Resources.Resource.Rename %>" title="<%=Resources.Resource.RenameSelectedGroup %>" onclick="return renameNode();"/>
                        <input type="button" id="deleteGroupButton" value="<%=Resources.Resource.Delete %>" title="<%=Resources.Resource.RemoveSelectedGroup %>" onclick="return deleteGroup();"/>
                        <input type="button" id="commentGroupButton" value="<%=Resources.Resource.Comment %>" title="<%=Resources.Resource.ChangeCommentSelectedGroup %>" onclick="return commentGroup();"/>
                    </p>
                    <p>
                        <div id="groupTree"></div>
                    </p>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <input type="button" id="reloadTreesButton" value="<%=Resources.Resource.Reload %>" title='<%=Resources.Resource.ReloadInfoFromServer %>' onclick="return reloadTrees();"/>
                    <input type="button" id="saveTreesButton" value="<%=Resources.Resource.Save%>" title='<%=Resources.Resource.SaveInfoToServer %>' onclick="return saveTrees();"/>

                </td>
            </tr>
        </table>   
    </div>
    <div id="commentEditBox" style="display:none">
        <p> <%=Resources.Resource.ChangeComment %></p>
        <input id="commentEditBoxInput" type="text" style="width:280px"/>
    </div>
    <div id="dialog-message"><p id="messageText"/></div>
</asp:Content>