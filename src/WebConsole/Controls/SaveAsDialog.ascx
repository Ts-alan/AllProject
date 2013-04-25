<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SaveAsDialog.ascx.cs" Inherits="Controls_SaveAsDialog" %>
<script type="text/javascript" language="javascript">
    var SaveAsDialog_<%=ClientID %> = function () {
        var dialog;
        var asyncPostBack = false;
        var restrictedNames = null;
        var usedNames = null;
        var saveCallback = null;
        var hideDisabled = false;

        function onHide() {
            if (!hideDisabled) {
                //enable async postback when save as dialog is hidden
                PageRequestManagerHelper.enableAsyncPostBack();
            }
        }

        function onShow() {
            //don't allow async postback while save as dialog is shown
            PageRequestManagerHelper.abortAsyncPostBack();
            PageRequestManagerHelper.disableAsyncPostBack();
        }

        function onSave() {
            name = GetTrimmedName();
            if (CheckIfEmpty(name)) {
                //name is empty
                document.getElementById('<%= lblSaveAsError.ClientID %>').innerHTML = '<%= NameEmptyErrorMessage %>';
                return;
            }
            if (CheckIfRestricted(name)) {
                //can't use this name
                document.getElementById('<%= lblSaveAsError.ClientID %>').innerHTML = '<%= NameRestrictedErrorMessage %>';
                return;
            }
            //hide dialog but still don't allow async postback
            hideDisabled = true;
            dialog.hide();
            hideDisabled = false;
            if (!CheckIfExists(name)) {
                //name does not exist
                onHide();
                //call callback function if it is registered
                if (typeof saveCallback == 'function') {
                    saveCallback(GetTrimmedName());
                }
            }
            else {
                //name already exist ask to rewrite
                Ext.MessageBox.buttonText.yes = '<%= Resources.Resource.Yes %>';
                Ext.MessageBox.buttonText.no = '<%= Resources.Resource.No %>';
                Ext.MessageBox.confirm('<%= Resources.Resource.Save %>', '<%= NameExistsConfirmRewriteMessage %>',
                        function (btn) {
                            if (btn == "yes") {
                                onHide();
                                if (typeof saveCallback == 'function') {
                                    saveCallback(GetTrimmedName());
                                }
                            }
                            else {
                                dialog.show();
                            }
                        });
            }
        }

        String.prototype.trim = function () {
            return this.replace(/^\s*/, "").replace(/\s*$/, "");
        }

        function GetTrimmedName() {
            return document.getElementById('<%= tboxSaveAsName.ClientID %>').value.trim();
        }

        function CheckIfEmpty(name) {
            return (name == "");
        }

        function CheckIfExists(name) {
            if (usedNames == null) {
                return false;
            }
            if (!isArray(usedNames)) {
                return false;
            }
            for (i = 0; i < usedNames.length; i++) {
                if (usedNames[i] == name) {
                    return true;
                }
            }
            return false;
        }

        function isArray(obj) {    
            return obj.constructor == Array; 
        } 


        function CheckIfRestricted(name) {
            if (restrictedNames == null) {
                return false;
            }
            if (!isArray(restrictedNames)) {
                return false;
            }
            for (i = 0; i < restrictedNames.length; i++) {
                if (restrictedNames[i] == name) {
                    return true;
                }
            }
            return false;
        }

        return {
            setSaveCallback: function (_saveCallback) {
                saveCallback = _saveCallback;
            },
            setRestrictedNames: function (_restrictedNames) {
                restrictedNames = _restrictedNames;
            },
            setUsedNames: function (_usedNames) {
                usedNames = _usedNames;
            },
            setAsyncPostBack: function () {
                asyncPostBack = true;
            },
            show: function () {
                if (!dialog || asyncPostBack) {
                    dialog = new Ext.BasicDialog('<%= dlgSaveAs.ClientID  %>', {
                        collapsible: false,
                        width: 205,
                        height: 120,
                        shadow: true,
                        resizable: false,
                        draggable: true,
                        proxyDrag: true,
                        modal: true
                    });
                    dialog.addKeyListener(27, dialog.hide, dialog);
                    dialog.addButton('<%= Resources.Resource.Save  %>', onSave, dialog);
                    dialog.on('show', onShow);
                    dialog.on('hide', onHide);
                    asyncPostBack = false;
                }
                document.getElementById('<%= tboxSaveAsName.ClientID %>').value = "";
                document.getElementById('<%= lblSaveAsError.ClientID %>').innerHTML = "";
                dialog.show();
            }
        };
    } ();
</script>
<div id="dlgSaveAs" runat="server" style="visibility: hidden; position: absolute;
    top: 0px;">
    <div class="x-dlg-hd">
        <%=Resources.Resource.SaveAs %></div>
    <div class="x-dlg-bd">
        <asp:TextBox ID="tboxSaveAsName" runat="server"></asp:TextBox>
        <ajaxToolkit:FilteredTextBoxExtender ID="ftboxSaveAsName" TargetControlID="tboxSaveAsName" 
        runat="server" InvalidChars="'&quot;""" FilterMode="InvalidChars" />
        <br />
        <asp:Label ID="lblSaveAsError" runat="server"></asp:Label>
    </div>
</div>