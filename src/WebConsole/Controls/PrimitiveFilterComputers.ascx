<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrimitiveFilterComputers.ascx.cs"
    Inherits="Controls_PrimitiveFilterComputers" %>
<%@ Register Src="~/Controls/PrimitiveFilterTemplate.ascx" TagName="PrimitiveFilterTemplate"
    TagPrefix="flt" %>
    <script language="javascript" type="text/javascript">
        var ComputersTextBox = function () {
            RegExp.escape = function (text) {
                return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
            }
            return {
                getRegex: function () {
                    tboxComputer = $('#<%= tboxFilter.ClientID %>');
                    str = tboxComputer.val();
                    if (str == "") return null;
                    strEsc = RegExp.escape(str);
                    strQuantifier = strEsc.replace(/\\\*/g, '.*');
                    splited = new Array();
                    splited = strQuantifier.split('&');
                    for (i = 0; i < splited.length; i++) {
                        splited[i] = '^' + splited[i] + '$';
                    }
                    regex = splited.join('|');
                    return regex;
                },

                setText: function (text) {
                    $('#<%= tboxFilter.ClientID %>').val(text); ;
                }
            };
        } ();

        var ComputersTree = function () {
            var root;
            var treeStore;
            var tree;
            function onTreeLoad(This, node) {
                SetOnCheckChanged();
                SetCheckedAccordingToText();
            }

            function SetOnCheckChanged() {
                root.eachChild(function (group) {
                    RecursiveSetOnCheckChanged(group);
                });
            }

            function RecursiveSetOnCheckChanged(group) {
                groupIsChecked=group.get('checked');
                if(groupIsChecked)
                    toggleParentCheck(group,groupIsChecked);
                group.eachChild(function (node) {
                    RecursiveSetOnCheckChanged(node);
                });              
            }

            function SetCheckedAccordingToText() {
                gen = ComputersTextBox.getRegex();
                if (gen == null) return;
                regex = new RegExp(gen, "i");
                root.eachChild(function (group) {
                    RecursiveIsChecked(group);

                    function RecursiveIsChecked(rootNode) {
                        var childChecked = false;
                        for (var i = 0; i < rootNode.childNodes.length; i++) {
                            if (RecursiveIsChecked(rootNode.childNodes[i]))
                                childChecked = true;
                        }
                        if (rootNode.isLeaf() && regex.test(rootNode.get("text"))) {
                            childChecked = true;
                            rootNode.set('checked', childChecked);
                        }

                        if (!rootNode.isLeaf()) {
                            rootNode.set('checked',childChecked);
                        }

                        return childChecked;
                    }
                });
            }

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
                            url: '<%=HandlerUrl%>'
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
                                    if (response.responseText.indexOf("Logins.aspx?ReturnUrl=") > -1) {
                                        location.reload(true);
                                    }
                                    else {
                                        dialog.dialog('close');
                                        Ext.Msg.alert('<%= Resources.Resource.Error  %>', '<%= Resources.Resource.ErrorRequestingDataFromServer%>');
                                    }
                                } else onTreeLoad(this, root);
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
                                computers.push(rootNode.get("text").toLowerCase());
                        }

                    });
                    return computers.join('&');
                }
            };
        } ();

        var ComputersDialog = function () {
            var dialog;
            var asyncPostBack = false;

            function onApply() {
                ComputersTextBox.setText(ComputersTree.generateText());
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
                    if ($('#<%= imgHelper.ClientID%>').attr("disabled") === "disabled") { return; }
                    if (!dialog || asyncPostBack) {
                        $('body').append('<div id="dialog_div"></div>');
                        dialog = $('#dialog_div').dialog({
                            title: '<%=Resources.Resource.Computers %>',
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
                            buttons: { '<%= Resources.Resource.Apply  %>': function () {onApply();dialog.dialog("close");}}
                        });
                        ComputersTree.init(dialog);
                        asyncPostBack = false;
                    }
                    else {
                        ComputersTree.reload();
                    }
                }
            };
        } ();

    </script>
<flt:PrimitiveFilterTemplate runat="server" ID="fltTemplate" TextFilter="Name">
    <FilterTemplate>
        <div style="padding: 5px; padding-top: 3px;">
            <div style="float:left;">
                <asp:TextBox runat="server" ID="tboxFilter" Style="width: 150px; height: 17px;"></asp:TextBox>            
                <ajaxToolkit:FilteredTextBoxExtender ID="fltComputers" runat="server" TargetControlID="tboxFilter"
                    FilterType="Custom" InvalidChars="`~!@#$%^()=+[]{}\|;:'&quot;,<>/?" FilterMode="InvalidChars">
                </ajaxToolkit:FilteredTextBoxExtender>
            </div>
            <div runat="server" class="ImageComputer" style="float:left; margin-left: 5px;" id="imgHelper" onclick="ComputersDialog.show(); return false;"></div>
        </div>
    </FilterTemplate>
</flt:PrimitiveFilterTemplate>