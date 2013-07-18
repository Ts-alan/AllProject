<%@ Page Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true"
    CodeFile="GroupsAdministrate.aspx.cs" Inherits="GroupsAdministrate" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" runat="Server">
    <script type="text/javascript" src="js/Groups/ext-4.1.1/ext-all.js"></script>
    <script language="javascript" type="text/javascript">

        Ext.onReady(function () {
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

            /********************************************************************************************
            groupStore and GroupTree generation
            ************************************************************************************************/

            var groupStore = Ext.create('Ext.data.TreeStore', {
                proxy: {
                    type: 'ajax',
                    url: '<%=Request.ApplicationPath%>/Handlers/TreeWithGroupsHandler.ashx'
                },
                root: {
                    text: '<%=Resources.Resource.Groups %>',
                    draggable: false,
                    id: 'groupTreeRoot',
                    root: true,
                    leaf: false,
                    expanded: true
                },
                listeners: {
                    beforeappend: function (thisnode, addnode, options) {//max_depth test
                        var j = 0;
                        var node = null;
                        if (addnode != null) {
                            node = addnode;
                            j = 0;
                            while (node.firstChild != null) {
                                j++;
                                node = node.firstChild;
                            }
                        }
                        node = null;
                        if (thisnode != null) {
                            if (thisnode.isRoot() && addnode.isLeaf())
                                return false;
                            j = j + thisnode.getDepth();
                        }
                        if (j >= MAX_DEPTH) {
                            Ext.Msg.show({
                                title: '<%=Resources.Resource.AddNewGroup %>',
                                msg: '<%=Resources.Resource.ErrorOfNestingLavel %>' + ' (max:' + MAX_DEPTH + ')',
                                minWidth: 200,
                                buttons: Ext.MessageBox.OK,
                                multiline: false
                            });
                            return false;
                        }
                    }
                },
                folderSort: true,
                sorters: [{
                    property: 'text',
                    direction: 'ASC'
                }]
            });
            Ext.override(Ext.data.AbstractStore, {
                indexOf: Ext.emptyFn
            });
            var editPlugin = Ext.create('Ext.grid.plugin.CellEditing', {
                clicksToEdit: 2,
                listeners:
            {
                edit: function (editor, e, eOpts) {
                    groupStore.sort();
                    groupTree.getView().refresh();
                }
            }
            });
            var groupTree = Ext.create('Ext.tree.Panel', {
                id: 'groupTree',
                store: groupStore,
                width: 500,
                height: 500,
                minWidth: 490,
                hideHeaders: true,
                border: 0,
                plugins: [editPlugin],
                columns: [{ xtype: 'treecolumn', dataIndex: 'text', width: 250, editor: { xtype: 'textfield'}}],
                viewConfig:
        {
            markDirty: false,
            listeners:
            {
                beforeitemcontextmenu: function (view, record, item, index, e) {
                    e.stopEvent();
                    contextMenu.showAt(e.getXY());
                    return false;
                },
                itemdblclick: function (view, record, item, index, e) {
                    editPlugin.cancelEdit();
                    groupTree.getSelectionModel().select(record);

                    renameGroup();
                },
                drop: function (node, data, model, dropposition, options) {
                    groupStore.sort();
                    this.refresh();
                }
            },
            plugins:
            {
                ptype: 'treeviewdragdrop',
                toggleOnDblClick: false,
                appendOnly: true
            }
        }
            });
            var groupTreeRoot = groupTree.getRootNode();
            /* ************************************************************************************************
            groupTree and Store generation finished
            ********************************************************************************* */
            /*  ***************************************************************************************************************
            editing groupTree events
            ***************************************************************************************************************  */
            var newGroupId = 0;
            var MAX_DEPTH = 5;
            var MAX_NAME_LENGTH = 64;
            groupTree.getSelectionModel().on('selectionchange', function (selModel, records) {
                if (records.length == 0)
                    menuEnabling(false);
                else {
                    if (records[0].isRoot()) {
                        menuEnabling(false);
                        addAction.enable();
                    }
                    else
                        menuEnabling(!records[0].get('leaf'));
                }
            });
            function menuEnabling(b) {
                if (b) {
                    addAction.enable();
                    renameAction.enable();
                    removeAction.enable();
                    commentAction.enable();
                }
                else {
                    addAction.disable();
                    renameAction.disable();
                    removeAction.disable();
                    commentAction.disable();
                }
            };
            groupTree.on('beforeedit', function (editor, e) {//edit only folders
                e.cancel = e.record.get('leaf');
                if (e.record.isRoot())
                    e.cancel = true;
            });
            //triggered event before rename completes
            //will not rename if there is group with same name (trimmed)
            groupTree.on('edit', function (editor, e) {
                var val = trim(e.value);
                var id = e.record.get("id");
                var isSuccess = true;
                if (val.length < 1) {
                    Ext.Msg.show({
                        title: '<%=Resources.Resource.ErrorRenamingGroup%>',
                        msg: '<%=Resources.Resource.CanNotRename%>' + e.originalValue +
                        '<%=Resources.Resource.NullName%>',
                        minWidth: 200,
                        buttons: Ext.MessageBox.OK,
                        multiline: false
                    });
                    e.record.data[e.field] = e.originalValue;
                }
                else if (val.length > MAX_NAME_LENGTH) {
                    Ext.Msg.show({
                        title: '<%=Resources.Resource.ErrorRenamingGroup%>',
                        msg: '<%=Resources.Resource.LimitedAllowLength%>' + " (max: " + MAX_NAME_LENGTH + ")",
                        minWidth: 200,
                        buttons: Ext.MessageBox.OK,
                        multiline: false
                    });
                    e.record.data[e.field] = e.originalValue;
                }
                else {   isSuccess = compareNames(groupTreeRoot, val, id) }
                if (isSuccess == false) {
                    Ext.Msg.show({
                        title: '<%=Resources.Resource.ErrorRenamingGroup%>',
                        msg: '<%=Resources.Resource.CanNotRename%>' + e.originalValue +
                        '<%=Resources.Resource.GroupAllreadyExists%>',
                        minWidth: 200,
                        buttons: Ext.MessageBox.OK,
                        multiline: false
                    });
                    e.record.data[e.field] = e.originalValue;
                }
                groupStore.sort();
                groupTree.getView().refresh();
            });
            function compareNames(node, name, id) {
                var length = node.childNodes.length;
                for (var i = 0; i < length; i++) {
                    if ((node.childNodes[i].get('text') == name && node.childNodes[i].get('id') != id)) {
                        return false;
                        break;
                    }
                    if (compareNames(node.childNodes[i], name, id) == false)
                        return false;
                }
                return true;
            }
            function trim(stringToTrim) {
                return stringToTrim.replace(/^\s+|\s+$/g, "");
            }
            /********************************************************************************************
            noGroupStore and noGroupTree generation
            ************************************************************************************************/
            var noGroupStore = Ext.create('Ext.data.TreeStore', {
                proxy: {
                    type: 'ajax',
                    url: '<%=Request.ApplicationPath%>/Handlers/TreeNoGroupsHandler.ashx'
                },
                root: {
                    root: true,
                    expanded: true,
                    allowDrag: false,
                    allowDrop: true,
                    text: '<%=Resources.Resource.Computers %>',
                    id: 'noGroupTreeRoot'
                },
                listeners: {},

                folderSort: true,
                sorters: [{
                    property: 'text',
                    direction: 'ASC'
                }]
            });
            var noGroupTree = Ext.create('Ext.tree.Panel', {
                id: 'noGroupTree',
                width: 300,
                height: 300,
                minWidth: 200,
                region: 'center',
                layout: 'fit',
                store: noGroupStore,
                split: true,
                border: 1,
                title: '<%=Resources.Resource.ComputersWithoutGroups %>',
                viewConfig:
                {
                    listeners:
                    {
                        drop: function (node, data, model, dropposition, options) {
                            noGroupStore.sort();
                            this.refresh();
                        }
                    },
                    markDirty: false,
                    plugins:
                    {
                        ptype: 'treeviewdragdrop',
                        appendOnly: true
                    }
                }
            });
            var noGroupTreeRoot = noGroupTree.getRootNode();
            noGroupTreeRoot.on('beforeappend', function (parentNode, childNode, eOpts) {
                //Deny to append groups in noGroupTree
                if (!childNode.isLeaf()) return false;
                else return true;
            });
            /* ************************************************************************************************
            noGroupTree and noGroupStore generation finished
            ********************************************************************************* */




            // create the bottom toolbar
            var bottomToolBar = Ext.create('Ext.toolbar.Toolbar');
            bottomToolBar.suspendLayouts();

            bottomToolBar.add({
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
            });
            bottomToolBar.resumeLayouts(true);
            bottomToolBar.setBorder('1');
            /***********************************************************************************************************
            actions
            *************************************************************************************************************/
            var addAction = Ext.create('Ext.Action', {
                text: '<%=Resources.Resource.Add %>',
                handler: addGroup,
                iconCls: 'add-opt',
                tooltip: '<%=Resources.Resource.AddNewGroup %>'
            });
            var removeAction = Ext.create('Ext.Action', {
                text: '<%=Resources.Resource.Delete %>',
                handler: removeGroup,
                disabled: true,
                iconCls: 'remove',
                tooltip: '<%=Resources.Resource.RemoveSelectedGroup %>'
            });
            var renameAction = Ext.create('Ext.Action', {
                text: '<%=Resources.Resource.Rename %>',
                handler: renameGroup,
                disabled: true,
                iconCls: 'rename',
                tooltip: '<%=Resources.Resource.RenameSelectedGroup %>'
            });
            var commentAction = Ext.create('Ext.Action', {
                text: '<%=Resources.Resource.Comment %>',
                handler: commentGroup,
                disabled: true,
                iconCls: 'comment',
                tooltip: '<%=Resources.Resource.ChangeCommentSelectedGroup %>'
            });
            /*  ***************************************************************************************************************
            contextmenu
            ***************************************************************************************************************  */
            var contextMenu = Ext.create('Ext.menu.Menu', {
                items: [
            renameAction,
            removeAction,
            commentAction
        ]

            });
            /*  ***************************************************************************************************************
            groupToolbar
            ***************************************************************************************************************  */
            var groupToolBar = Ext.create('Ext.toolbar.Toolbar');
            groupToolBar.suspendLayouts();
            groupToolBar.add(addAction, '-', renameAction, removeAction, commentAction);
            groupToolBar.resumeLayouts(true);

            var groupTreePanel = new Ext.Panel({
                region: 'east',
                layout: 'fit',
                items: [groupTree],
                split: true,
                title: '<%=Resources.Resource.ComputersWithGroups %>',
                tbar: groupToolBar,
                width: 500,
                minWidth: 400,
                border: 1
            });




            /*  ***************************************************************************************************************
            Container and layout generation finished
            ***************************************************************************************************************  */

            /*  ***************************************************************************************************************
            Save and reload realization
            ***************************************************************************************************************  */

            //reloading trees from server
            function reload() {
                noGroupStore.reload();
                groupStore.reload();
            }
            /*********************************************************************************************************************
            groupMenu handlers
            *********************************************************************************************************************/
            // remove group
            function removeGroup() {
                sm = groupTree.getSelectionModel();
                var n = sm.getLastSelected();
                if (n == groupTree.getRootNode())
                    return;
                if (!n) {
                    //no node selected
                    return;
                }
                //group is selected - proceed

                //add computers from selected group to groupless tree
                //and remove from this group
                var s = n.firstChild;
                while (s != null) {
                    if (s.get('leaf')) {
                        noGroupTreeRoot.appendChild(s);
                    }
                    else groupTreeRoot.appendChild(s);
                    n.removeChild(s);
                    s = n.firstChild;

                } if (!n.parentNode) return;
                n.parentNode.removeChild(n);
                groupStore.sort();
                noGroupStore.sort();
                groupTree.getView().refresh();
                groupTree.getSelectionModel().deselectAll();

            }
            //renamegroup
            function renameGroup() {
                var node = groupTree.getSelectionModel().getLastSelected();  // get selected node
                editPlugin.startEdit(node, 0);
            }

            //    add group
            function addGroup() {
                var root = groupTree.getRootNode();
                var thisnode = groupTree.getSelectionModel().getLastSelected();

                if (thisnode == null || thisnode.get('leaf') == true) {
                    thisnode = groupTreeRoot;
                    groupTree.getSelectionModel().select(root);
                }
                newGroupId++;
                var success = false;
                var length = groupTreeRoot.childNodes.length;
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
                        if (compareNames(groupTreeRoot, newGroupName, newGroupId) == false) {
                            success = false;
                            break;
                        }
                    }
                    newGroupNumber++;
                }
                var newNode = thisnode.createNode({ text: newGroupName, id: 'GroupNew_' + (newGroupId), allowDrag: true, children: [], cls: 'group', qtip: 'No comment' });
                thisnode.expand();
                thisnode.appendChild(newNode);
                groupStore.sort();
                groupTree.getView().refresh();
                groupTree.getSelectionModel().select(newNode);
                renameGroup();
            }

            //comment group
            function commentGroup() {
                sm = groupTree.getSelectionModel();
                var n = sm.getSelection()[0];
                if (!n) {
                    return;
                }
                if (!n.leaf) {
                    //group is selected - proceed

                    var msg = '<%=Resources.Resource.ChangeComment %>' + ' (' + n.get('text') + '): ';
                    var comment = n.get('qtip');
                    if (comment == 'No comment') {
                        comment = '';
                    }
                    Ext.Msg.prompt('<%=Resources.Resource.Comment %>', msg, function (btn, text) {
                        if (btn == 'ok') {
                            var newComment = trim(text);
                            if (newComment == '') {
                                newComment = 'No comment';
                            }
                            n.set('qtip', newComment);
                        }
                    }, 'window', false, comment);
                }
            }
            function myNode(text, id, parentId, comment, isLeaf) {
                this.text = text;
                this.id = id;
                this.parentId = parentId;
                this.comment = comment;
                this.isLeaf = isLeaf;
            }

            function save() {
                MainPanel.setLoading('<%=Resources.Resource.SendingDataToServer %>' + '...');
                //generate arrays containing nodes info
                var groupTreeArray = new Array();
                RecursiveSaving(groupTreeRoot);
                var noGroupTreeArray = new Array();
                noGroupTreeRoot.eachChild(function (comp) {
                    noGroupTreeArray[noGroupTreeArray.length++] = new myNode(comp.text, comp.id, '0', '');
                });
                function RecursiveSaving(root) {
                    var length = root.childNodes.length;
                    for (var i = 0; i < length; i++) {
                        RecursiveSaving(root.childNodes[i]);
                    }
                    if (root.getDepth() == 0) return;
                    var comment = '';
                    if (!root.isLeaf()) comment = root.get('qtip');
                    if (comment == 'No comment') {
                        comment = '';
                    }
                    groupTreeArray[groupTreeArray.length++] = new myNode(root.get('text'), root.get('id'), root.parentNode.get('id'), comment, root.isLeaf() ? "true" : "false");
                }
                //query .net handler with ajax
                Ext.Ajax.request({
                    url: '<%=Request.ApplicationPath%>/Handlers/GetTreeDataHandler.ashx',

                    params: {
                        groupTreeArray: Ext.JSON.encode(groupTreeArray),         //pass groupTreeArray as json
                        noGroupTreeArray: Ext.JSON.encode(noGroupTreeArray)            //pass noGroupTreeArray as json
                    },
                    method: 'POST',                                                         //method
                    callback: postcallback
                });
                MainPanel.setLoading(false);
            }
            function postcallback(options, success, response) {                     //callback function
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
            }

            /*  ***************************************************************************************************************
            Save and reload realization finished
            ***************************************************************************************************************  */
            // create  mainPanel
            /********************************************************************************************
            Panel
            ************************************************************************************************/

            var MainPanel = new Ext.Panel({
                renderTo: 'mainContainer',
                layout: 'border',
                width: 800,
                height: 500,
                border: 1,
                bodyStyle: {    'z-index': 0
                },
                items:[
                groupTreePanel, noGroupTree,
                {
                    region: 'south',
                    //   layout: 'fit',
                    tbar: bottomToolBar,
                    border: 1
                }
             ]
            });
        });
    </script>
    <div id="mainContainer" 
        style="width: 800px; height: 500px;">
    </div>
</asp:Content>