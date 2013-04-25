<%@ Page Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="GroupsAdministrate.aspx.cs" Inherits="GroupsAdministrate" Title="Untitled Page" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">

<script type="text/javascript" src="js/Groups/ext-1.1.1/adapter/ext/ext-base.js"></script>
<script type="text/javascript" src="js/Groups/ext-1.1.1/ext-all.js"></script> 

<script language="javascript" type="text/javascript">

    Ext.onReady(function () {
        /*  ***************************************************************************************************************
        Container and layout generation
        ***************************************************************************************************************  */
        // turn on quick tips

        Ext.QuickTips.init();

        var view = Ext.DomHelper.append('mainContainer',
        { cn: [{ id: 'groupToolBar' }, { id: 'groupTree'}] }
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

        // create the group toolbar
        var groupToolBar = new Ext.Toolbar('groupToolBar');
        groupToolBar.add({
            id: 'add',
            text: '<%=Resources.Resource.Add %>',
            handler: addGroup,
            cls: 'x-btn-text-icon add-opt',
            tooltip: '<%=Resources.Resource.AddNewGroup %>'
        }, '-', {
            id: 'rename',
            text: '<%=Resources.Resource.Rename %>',
            handler: renameGroup,
            disabled: true,
            cls: 'x-btn-text-icon rename',
            tooltip: '<%=Resources.Resource.RenameSelectedGroup %>'
        }, {
            id: 'remove',
            text: '<%=Resources.Resource.Delete %>',
            handler: removeGroup,
            disabled: true,
            cls: 'x-btn-text-icon remove',
            tooltip: '<%=Resources.Resource.RemoveSelectedGroup %>'
        }, {
            id: 'comment',
            text: '<%=Resources.Resource.Comment %>',
            handler: commentGroup,
            disabled: true,
            cls: 'x-btn-text-icon comment',
            tooltip: '<%=Resources.Resource.ChangeCommentSelectedGroup %>'
        });

        // variable for enabling and disabling toolbar items
        var toolbarButtons = groupToolBar.items.map;

        // create  layout
        var layout = new Ext.BorderLayout('mainContainer', {
            west: {
                split: true,
                initialSize: 350,
                minSize: 200,
                maxSize: 400,
                titlebar: true,
                margins: { left: 5, right: 0, bottom: 5, top: 5 }
            },
            center: {
                title: '<%=Resources.Resource.ComputersWithGroups %>',
                margins: { left: 0, right: 5, bottom: 5, top: 5 }
            }
        }, 'mainContainer');

        layout.batchAdd({
            west: {
                id: 'noGroupTree',
                autoCreate: true,
                title: '<%=Resources.Resource.ComputersWithoutGroups %>',
                autoScroll: true,
                fitToFrame: true
            },
            center: {
                el: view,
                autoScroll: true,
                fitToFrame: true,
                toolbar: groupToolBar,
                resizeEl: 'groupTree'
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
            noGroupTreeRoot.reload();
            groupTreeRoot.reload();
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
                if (!root.isLeaf()) comment = root.attributes.qtipCfg.text;
                if (comment == 'No comment') {
                    comment = '';
                }
                groupTreeArray[groupTreeArray.length++] = new myNode(root.text, root.id, root.parentNode.id, comment, root.isLeaf() ? "true" : "false");
            }

            //query .net handler with ajax
            Ext.Ajax.request({
                url: '<%=Request.ApplicationPath%>/Handlers/GetTreeDataHandler.ashx',                                                    //url
                params: { groupTreeArray: Ext.util.JSON.encode(groupTreeArray),         //pass groupTreeArray as json
                    noGroupTreeArray: Ext.util.JSON.encode(noGroupTreeArray)            //pass noGroupTreeArray as json
                },
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
        Group tree
        group tree generation, set up and its root creation
        ***************************************************************************************************************  */
        //incrementing id for new groups created
        var newGroupId = 0;

        var MAX_DEPTH = 5;
        var MAX_NAME_LENGTH = 64;

        var groupTree = new Ext.tree.TreePanel('groupTree', {
            animate: true,
            enableDD: true,
            rootVisible: true,
            loader: new Ext.tree.TreeLoader({
                dataUrl: '<%=Request.ApplicationPath%>/Handlers/TreeWithGroupsHandler.ashx',
                requestMethod: 'GET'
            }),
            containerScroll: true,
            dropConfig: { appendOnly: true }
        });

        groupTree.on('beforeappend', function (tree, parentNode, childNode, index) {
            if (!childNode.isLeaf() && parentNode.getDepth() >= MAX_DEPTH) {
                Ext.Msg.show({
                    title: '<%=Resources.Resource.AddNewGroup %>',
                    msg: '<%=Resources.Resource.ErrorOfNestingLavel %>' + ' (max:' + MAX_DEPTH + ')',
                    minWidth: 200,
                    buttons: Ext.MessageBox.OK,
                    multiline: false
                });
                return false;
            }
        });

        new Ext.tree.TreeSorter(groupTree, { folderSort: true });

        var groupTreeRoot = new Ext.tree.AsyncTreeNode({
            text: '<%=Resources.Resource.Groups %>',
            draggable: false,
            id: 'groupTreeRoot'
        });
        groupTree.setRootNode(groupTreeRoot);

        groupTree.render();

        groupTreeRoot.expand(false, false);

        groupTreeRoot.on('beforeappend', function (tree, parentNode, childNode, index) {
            if (childNode.isLeaf()) return false;
        });

        // group tree selection model
        var sm = groupTree.getSelectionModel();

        // when the group tree selection changes, enable/disable the toolbar buttons
        sm.on('selectionchange', function () {
            var n = sm.getSelectedNode();
            if (!n) {
                //no node selected - disable remove/rename
                toolbarButtons.remove.disable();
                toolbarButtons.rename.disable();
                toolbarButtons.comment.disable();
                return;
            }
            //if computer selected - disable remove/rename
            //if group selected - enable remove/rename
            toolbarButtons.remove.setDisabled(n.leaf || (n.getDepth() == 0));
            toolbarButtons.rename.setDisabled(n.leaf || (n.getDepth() == 0));
            toolbarButtons.comment.setDisabled(n.leaf || (n.getDepth() == 0));
            toolbarButtons.add.setDisabled(n.leaf);
        });

        //set context menu to group tree
        groupTree.on('contextmenu', prepareCtx);

        //don't create context menu for group tree container
        groupTree.el.swallowEvent('contextmenu', true);

        //stop event on navigation key pressed
        groupTree.el.on('keypress', function (e) {
            if (e.isNavKeyPress()) {
                e.stopEvent();
            }
        });

        /*  ***************************************************************************************************************
        Group tree finish
        ***************************************************************************************************************  */

        /*  ***************************************************************************************************************
        No Group tree
        no group tree generation, set up and its root creation
        ***************************************************************************************************************  */
        var noGroupTree = new Ext.tree.TreePanel('noGroupTree', {
            animate: true,
            enableDD: true,
            containerScroll: true,
            dropConfig: { appendOnly: true },
            loader: new Ext.tree.TreeLoader({
                dataUrl: '<%=Request.ApplicationPath%>/Handlers/TreeNoGroupsHandler.ashx',
                requestMethod: 'GET'
            })
        });

        var noGroupTreeRoot = new Ext.tree.AsyncTreeNode({
            allowDrag: false,
            allowDrop: true,
            text: '<%=Resources.Resource.Computers %>',
            id: 'noGroupTreeRoot'
        });
        noGroupTree.setRootNode(noGroupTreeRoot);

        noGroupTree.render();

        noGroupTreeRoot.expand(false, false);

        new Ext.tree.TreeSorter(noGroupTree, { folderSort: true });

        //Fires before a new child is appended, return false to cancel the append.        
        noGroupTreeRoot.on('beforeappend', function (tree, parentNode, childNode) {
            //Denied to append groups in noGroupTree
            if (!childNode.isLeaf()) return false;
            else return true;
        });

        /*  ***************************************************************************************************************
        No Group tree finish
        ***************************************************************************************************************  */

        /*  ***************************************************************************************************************
        Tree Editor
        used to allow renaming of groups, and editing the name of newly created groups
        ***************************************************************************************************************  */

        // create the editor for the component tree
        var groupTreeEditor = new Ext.tree.TreeEditor(groupTree, {
            cancelOnEsc: true,
            completeOnEnter: true,
            ignoreNoChange: true,
            revertInvalid: true,
            allowBlank: false,
            blankText: '<%=Resources.Resource.GroupNameRequired %>',
            selectOnFocus: true
        });

        //triggered event before node rename start
        groupTreeEditor.on('beforestartedit', function () {
            if (groupTreeEditor.editNode.leaf || groupTreeEditor.editNode.getDepth() == 0) {
                //computer is selected or root - exit
                return false;
            }
            groupTreeEditor.setSize(groupTreeEditor.editNode.text.length * 7 + 10, 25);
        });

        //triggered event before rename completes
        //will not rename if there is group with same name (trimmed)
        groupTreeEditor.on('beforecomplete', function (editor, value, startValue) {
            var v = trim(value);
            if (v == "") {
                //new name consists only from whitespaces
                //change name to start value, cancel editing
                this.cancelEdit(false);
                return false;
            }

            if (value.length > MAX_NAME_LENGTH) {
                Ext.Msg.show({
                    title: '<%=Resources.Resource.ErrorRenamingGroup%>',
                    msg: '<%=Resources.Resource.LimitedAllowLength%>' + " (max: " + MAX_NAME_LENGTH + ")",
                    minWidth: 200,
                    buttons: Ext.MessageBox.OK,
                    multiline: false
                });
                this.cancelEdit(false);
                return false;
            }

            var length = groupTreeRoot.childNodes.length;
            for (var i = 0; i < length; i++) {
                if (groupTreeRoot.childNodes[i].text == startValue) {
                    continue;
                }
                if (groupTreeRoot.childNodes[i].text == v) {
                    Ext.Msg.show({
                        title: '<%=Resources.Resource.ErrorRenamingGroup%>',
                        msg: '<%=Resources.Resource.CanNotRename%>' + startValue +
                        '<%=Resources.Resource.GroupAllreadyExists%>',
                        minWidth: 200,
                        buttons: Ext.MessageBox.OK,
                        multiline: false
                    });
                    this.cancelEdit(true);
                    return false;
                }
            }

            //known bug of extjs: this.setValue(x) don't change the value
            //work around: call to completeEdit will end on this event
            //the end of function will emulate completeEdit ending
            //we supose that remainVisible is false
            remainVisible = false;

            this.editing = false;
            if (this.updateEl && this.boundEl) {
                this.boundEl.update(v);
            }
            if (remainVisible !== true) {
                this.hide();
            }
            this.fireEvent("complete", this, v, this.startValue);
            return false;
        });

        function trim(stringToTrim) {
            return stringToTrim.replace(/^\s+|\s+$/g, "");
        }


        //extjs bug: tree don't sort after renaming node
        //work around: on successfull rename, triger beforechildrenrendered to resort tree
        groupTreeEditor.on('complete', function () {
            groupTree.fireEvent('beforechildrenrendered', groupTreeRoot);
        });

        /*  ***************************************************************************************************************
        Tree Editor finish
        ***************************************************************************************************************  */

        /*  ***************************************************************************************************************
        Remove, add, rename group methods realization        
        ***************************************************************************************************************  */
        // remove group handler
        // computers from removed group are added to other tree          
        function removeGroup() {
            var n = sm.getSelectedNode();
            if (!n) {
                //no node selected
                return;
            }
            if (!n.leaf) {
                //group is selected - proceed                
                RecursiveDeleting(n);
            }
        }

        //add computers from selected group to groupless tree
        //and remove from this group
        function RecursiveDeleting(root) {
            var length = root.childNodes.length;
            for (var i = length - 1; i >= 0; i--) {
                RecursiveDeleting(root.childNodes[i]);
            }
            root.parentNode.removeChild(root);
            if (root.isLeaf()) {
                noGroupTreeRoot.appendChild(root);
            }
        }

        //rename group handler
        function renameGroup() {
            var n = sm.getSelectedNode();
            if (!n) {
                //no node selected
                return;
            }
            if (!n.leaf) {
                //group is selected - proceed

                //start editing
                groupTreeEditor.triggerEdit(n);
            }
        }

        //comment group handler
        function commentGroup() {
            var n = sm.getSelectedNode();
            if (!n) {
                //no node selected
                return;
            }
            if (!n.leaf) {
                //group is selected - proceed

                var msg = '<%=Resources.Resource.ChangeComment %>' + ' (' + n.text + '): ';
                var comment = n.attributes.qtipCfg.text;
                if (comment == 'No comment') {
                    comment = '';
                }
                Ext.MessageBox.promptex('<%=Resources.Resource.Comment %>', msg, comment, function (btn, text) {
                    if (btn == 'ok') {
                        var newComment = trim(text);
                        if (newComment == '') {
                            newComment = 'No comment';
                        }
                        n.attributes.qtipCfg.text = newComment;
                    }
                });
            }
        }


        // add group handler
        function addGroup() {
            //increace unique to client side new group id
            newGroupId++;

            //generate new group name to display
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
                success = !FindGroupName(groupTreeRoot, newGroupName);
                newGroupNumber++;
            }

            function FindGroupName(root, groupName) {
                if (root.isLeaf()) return false;
                if (root.text == groupName) {
                    return true;
                }
                var length = root.childNodes.length;
                for (var i = 0; i < length; i++) {
                    if (FindGroupName(root.childNodes[i], groupName)) return true;
                }
                return false;
            }

            //create new group
            var node = createGroup(newGroupId, newGroupName);

            if (node.ownerTree != null) {
                //extjs bug: tree don't sort after appending node programmicly
                //work around: on successfull rename, triger beforechildrenrendered to resort tree
                node.ownerTree.fireEvent('beforechildrenrendered', groupTreeRoot);
                //select new node
                node.select();
            }
        }

        //create new group in groupTree
        function createGroup(_id, _text) {
            var node = new Ext.tree.TreeNode({
                text: _text,
                cls: 'group',
                allowDrag: true,
                allowDrop: true,
                id: 'GroupNew_' + (_id),
                qtipCfg: {
                    text: 'No comment',
                    xtype: 'quicktip'
                }
            });
            //append new group
            var n = sm.getSelectedNode();
            if (n != null)
                n.appendChild(node);
            else
                groupTreeRoot.appendChild(node);

            return node;
        }

        /*  ***************************************************************************************************************
        Remove, add, rename group methods realization finish       
        ***************************************************************************************************************  */

        /*  ***************************************************************************************************************
        Context menu
        context menu in group tree, with rename/remove group items
        ***************************************************************************************************************  */

        //generate context menu
        var ctxMenu = new Ext.menu.Menu({
            id: 'ctxMenu',
            items: [{
                id: 'rename',
                handler: renameGroup,
                cls: 'rename-mi',
                text: '<%=Resources.Resource.Rename %>'
            }, {
                id: 'remove',
                handler: removeGroup,
                cls: 'remove-mi',
                text: '<%=Resources.Resource.Delete %>'
            }, {
                id: 'comment',
                handler: commentGroup,
                cls: 'comment-mi',
                text: '<%=Resources.Resource.Comment %>'
            }]
        });

        //prepare context menu
        function prepareCtx(node, e) {
            //select node calling context menu
            node.select();
            var isShow = !node.leaf && (node.getDepth() != 0);
            //enable remove/rename if group is selected
            ctxMenu.items.get('remove')[isShow ? 'enable' : 'disable']();
            ctxMenu.items.get('rename')[isShow ? 'enable' : 'disable']();
            ctxMenu.items.get('comment')[isShow ? 'enable' : 'disable']();
            //showmenu on clicked point
            ctxMenu.showAt(e.getXY());
        }
        /*  ***************************************************************************************************************
        Context menu finish
        ***************************************************************************************************************  */

    });

</script>
       
   <!-- <script type="text/javascript" src="js/Groups/Groups.js"></script> -->
    <div id="mainContainer" style="width:800px;height:500px;"></div>
    <div id="botContainer" style="width:800px;height:40px;"></div>    
</asp:Content>