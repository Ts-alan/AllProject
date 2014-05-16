var ComputersTreeDeviceClass = function () {
    var root;
    var treeStore;
    var tree;
    function onTreeLoad(This, node) {}

    //function to check/uncheck all  child nodes and parent path
    function toggleCheck(node, isCheck) {
        if (node) {
            var args = [isCheck];
            node.cascadeBy(function () {
                c = args[0];
                this.set('checked', c);
            }, null, args);
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
        init: function (dialog) {
            Ext.tip.QuickTipManager.init();
            Ext.apply(Ext.tip.QuickTipManager.getQuickTip(), {
                maxWidth: 200,
                minWidth: 100,
                showDelay: 500    // Show 500ms after entering target
            });
            treeStore = Ext.create('Ext.data.TreeStore', {
                proxy: {
                    type: 'ajax',
                    url: 'Handlers/CheckedComputerTreeHandler.ashx'
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
                                dialog.dialog('close');
                                Ext.Msg.alert(Resource.Error, Resource.ErrorRequestingDataFromServer);
                        } 
                    }
                }
            });
                  
            var tree = Ext.create('Ext.tree.Panel', {
                id: "treeComputers",
                animate: true,
                store: treeStore,
                autoScroll: true,
                rootVisible: false,
                renderTo: 'dialog_div'
            });
    
            tree.on("checkchange", function (node, checked, eOpts) {
                toggleCheck(node, checked);
            });
            root = tree.getRootNode();
            root.expand(false);
        },
        reload: function () {
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

var ComputersDialogDeviceClass = function () {
    var dialog;
    var asyncPostBack = false;

    function onApply(classId) {
        comps = ComputersTreeDeviceClass.generateText();
        $.ajax({
            type: "POST",
            url: "DeviceClass.aspx/AddNewDeviceClassPolicyByComputerList",
            dataType: "json",
            data: "{id:" + classId + ",comps:'" + comps + "'}",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                if (msg == true) {
                    $("a[deviceId=" + classId + "]").trigger("click");
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
        show: function (classId) {
            var Apply = Resource.Apply.toString();
            if (!dialog || asyncPostBack) {
                $('body').append('<div id="dialog_div"></div>');
                dialog = $('#dialog_div').dialog({
                    title: Resource.Computers,
                    width: 500,
                    height: 300,
                    modal: true,
                    draggable: false,
                    close: function () {
                        onHide();
                        $(this).dialog("destroy");
                        dialog = false;
                        $('#dialog_div').remove();
                    },
                    open: onShow,
                    buttons: { Apply: function () {
                        onApply(classId);
                        dialog.dialog("close");
                    }
                    }
                });
                ComputersTreeDeviceClass.init(dialog);
                asyncPostBack = false;
            }
            else {
                ComputersTreeDeviceClass.reload();
            }
        }
    };
} ();