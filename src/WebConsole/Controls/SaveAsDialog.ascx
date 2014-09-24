<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SaveAsDialog.ascx.cs" Inherits="Controls_SaveAsDialog" %>
<script type="text/javascript" language="javascript">
    var SaveAsDialog_<%=ClientID %> = function () {
        var dialog = false;
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
                $('#<%= lblSaveAsError.ClientID %>').html('<%= NameEmptyErrorMessage %>')
                return;
            }
            if (CheckIfRestricted(name)) {
                //can't use this name
                $('#<%= lblSaveAsError.ClientID %>').html('<%= NameRestrictedErrorMessage %>')
                return;
            }
            //hide dialog but still don't allow async postback
            hideDisabled = true;
            dialog.dialog("close");
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
                $('body').append('<div id="confirm" style="display:none"><%= NameExistsConfirmRewriteMessage %></div>');
                $('#confirm').dialog({
                    resizable: false,
                    height:140,
                    modal: true,
                    close:function()
                    {
                         $(this).dialog("destroy");
                         $(this).remove();
                    },
                    buttons: {
                        '<%= Resources.Resource.Yes %>': function() {
                            onHide();
                            if (typeof saveCallback == 'function') {
                                saveCallback(GetTrimmedName());
                            }
                            $( this ).dialog( "close" );
                        },
                        '<%= Resources.Resource.No %>': function() {
                            $( this ).dialog( "close" );
                        }
                    }
                });
            }
        }

        String.prototype.trim = function () {
            return this.replace(/^\s*/, "").replace(/\s*$/, "");
        }

        function GetTrimmedName() {
            return $('#<%= tboxSaveAsName.ClientID %>').val().trim();
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
                    dialog =$('#<%= dlgSaveAs.ClientID  %>').dialog( {
                        resizable: false,
                        draggable: false,
                        modal: true,
                        buttons:{
                            '<%= Resources.Resource.Save  %>':onSave
                        },
                        open:onShow,
                        close:function(){
                            onHide();
                            $(this).dialog("destroy");
                        }
                    });
                }
                $('#<%= tboxSaveAsName.ClientID %>').val("");
                $('#<%= lblSaveAsError.ClientID %>').html("");
                dialog.dialog("open");
            }
        };
    } ();
</script>
<div id="dlgSaveAs" runat="server" style="display:none;" title='<%$Resources:Resource, SaveAs %>'>
        <asp:TextBox ID="tboxSaveAsName" runat="server"></asp:TextBox>
        <ajaxToolkit:FilteredTextBoxExtender ID="ftboxSaveAsName" TargetControlID="tboxSaveAsName" 
        runat="server" InvalidChars="'&quot;" FilterMode="InvalidChars" ></ajaxToolkit:FilteredTextBoxExtender>
        <br />
        <asp:Label ID="lblSaveAsError" runat="server"></asp:Label>
</div>