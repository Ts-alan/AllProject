<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrimitiveFilterComputers.ascx.cs"
    Inherits="Controls_PrimitiveFilterComputers" %>
<%@ Register Src="~/Controls/PrimitiveFilterTemplate.ascx" TagName="PrimitiveFilterTemplate"
    TagPrefix="flt" %>
<script type="text/javascript">
    var ComputersTextBox = function () {
        RegExp.escape = function (text) {
            return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
        }

        return {
            getRegex: function () {
                tboxComputer = document.getElementById('<%= tboxFilter.ClientID %>');
                str = tboxComputer.value;
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
                tboxComputer = document.getElementById('<%= tboxFilter.ClientID %>');
                tboxComputer.value = text;
            }
        };
    } ();

    var ComputersTree = function () {
        var root;

        function onTreeLoad(This, node) {
            SetOnCheckChanged();
            SetCheckedAccordingToText();
            ComputersDialog.unmask();
        }

        function onTreeBeforeLoad(This, node) {
            ComputersDialog.mask();
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
                    if (rootNode.isLeaf() && regex.test(rootNode.attributes.text)) {
                        childChecked = true;
                        rootNode.ui.toggleCheck(childChecked);
                        rootNode.attributes.checked = childChecked;
                    }

                    if (!rootNode.isLeaf()) {
                        rootNode.ui.toggleCheck(childChecked);
                        rootNode.attributes.checked = childChecked;
                    }

                    return childChecked;
                }
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

        return {
            init: function (dialog) {
                Ext.QuickTips.init();
                var loader = new Ext.tree.TreeLoader({
                    dataUrl: '<%=HandlerUrl%>',
                    requestMethod: 'GET',
                    listeners: {
                        loadexception: function (tl, node, response) {
                            if (response.responseText.indexOf("Logins.aspx?ReturnUrl=") > -1) {
                                // session expired
                                location.reload(true);
                            }
                            else {
                                dialog.hide();
                                Ext.Msg.alert('<%= Resources.Resource.Error  %>', '<%= Resources.Resource.ErrorRequestingDataFromServer%>');
                            }
                        }
                    }
                });

                var tree = new Ext.tree.TreePanel('<%= treeComputers.ClientID  %>', {
                    animate: true,
                    loader: loader,
                    enableDD: false,
                    containerScroll: true,
                    rootVisible: false
                });
                loader.on("load", onTreeLoad);
                loader.on("beforeload", onTreeBeforeLoad);

                new Ext.tree.TreeSorter(tree, { folderSort: true });

                root = new Ext.tree.AsyncTreeNode({});
                tree.setRootNode(root);

                tree.render();

                root.expand(false, false);
            },
            reload: function () {
                root.reload();
            },
            generateText: function () {
                computers = new Array();
                root.eachChild(function (group) {
                    RecursiveAdd(group);

                    function RecursiveAdd(rootNode) {
                        for (var i = 0; i < rootNode.childNodes.length; i++) {
                            RecursiveAdd(rootNode.childNodes[i]);
                        }

                        if (rootNode.isLeaf() && rootNode.attributes.checked)
                            computers.push(rootNode.attributes.text.toLowerCase());
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
            dialog.hide();
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
                    dialog = new Ext.BasicDialog('<%= dlgComputers.ClientID  %>', {
                        collapsible: false,
                        width: 500,
                        height: 300,
                        shadow: true,
                        minWidth: 300,
                        minHeight: 250,
                        draggable: true,
                        proxyDrag: true,
                        modal: true
                    });
                    dialog.addKeyListener(27, dialog.hide, dialog);
                    dialog.addButton('<%= Resources.Resource.Apply  %>', onApply, dialog);
                    dialog.on('hide', onHide);
                    dialog.on('show', onShow);
                    ComputersTree.init(dialog);
                    asyncPostBack = false;
                }
                else {
                    ComputersTree.reload();
                }
                dialog.show();
            },
            mask: function () {
                for (i = 0; i < dialog.buttons.length; i++) {
                    dialog.buttons[i].disable();
                }
                dialog.body.mask('<%=Resources.Resource.RequestingDataFromServer %>' + '...', 'x-mask-loading');
            },
            unmask: function () {
                for (i = 0; i < dialog.buttons.length; i++) {
                    dialog.buttons[i].enable();
                }
                dialog.body.unmask();
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
<div id="dlgComputers" runat="server" style="visibility: hidden; position: absolute;
    top: 0px;">
    <div class="x-dlg-hd">
        <%=Resources.Resource.Computers %></div>
    <div class="x-dlg-bd">
        <div runat="server" id="treeComputers">
        </div>
    </div>
</div>