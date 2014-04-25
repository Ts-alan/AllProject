<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompositeFilter.ascx.cs"
    Inherits="Controls_CompositeFilter" %>
<%@ Register TagPrefix="custom" Namespace="VirusBlokAda.CC.CustomControls" Assembly="CustomControls" %>
<%@ Register Src="~/Controls/TemporaryGroupFilter.ascx" TagName="TemporaryGroupFilter" TagPrefix="flt" %>
<%@ Register Src="~/Controls/CollapsiblePanelSwitch.ascx" TagName="CollapsiblePanelSwitch" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/SaveAsDialog.ascx" TagName="SaveAsDialog" TagPrefix="cc1" %>
<div>
    <table class="subsection" width="100%" style="min-width: 864px;">
        <tr>
            <td align="left">
                <table>
                    <tr>
                        <td>
                            <div class="GiveButton" style="width: 90px; float: left" runat="server" id="divFilterHeader">
                                <asp:LinkButton ID="lbtnFilterHeader" runat="server" ForeColor="white" Width="100%"
                                    OnClick="lbtnFilterHeader_Click"><%=Resources.Resource.Filter %></asp:LinkButton>
                            </div>
                            <div style="width: 5px; height: 5px; float: left">
                            </div>
                            <asp:DropDownList ID="ddlUserFilters" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlUserFilters_SelectedIndexChanged"
                                SkinID="ddlFilterList">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <flt:TemporaryGroupFilter ID="TemporaryGroupFilter1" runat="server" OnTemporaryGroupFilterChange="TemporaryGroupFilter1_TemporaryGroupFilterChanged" />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <cc1:CollapsiblePanelSwitch runat="server" ID="CollapsiblePanelSwitch1"  />
            </td>
        </tr>
    </table>
    <div id="divDetails" runat="server" style="visibility: visible; min-width: 964px;" enableviewstate="true">
        <div class="compositeFilter">
            <asp:PlaceHolder runat="server" ID="FiltersPlaceHolder" />
        </div>        
        <div>
            <div class="GiveButton1" style="width: 100px; float: left">
                <asp:LinkButton ID="lbtnApply" SkinID="FilterActions" runat="server" OnClick="lbtnApply_Click"
                    ForeColor="White" Text="<%$ Resources:Resource, Apply %>" ValidationGroup="FilterValidation" />
            </div>
            <div class="GiveButton1" style="width: 100px; float: left">
                <asp:LinkButton ID="lbtnClear" SkinID="FilterActions" runat="server" OnClick="lbtnClear_Click"
                    ForeColor="White" Text="<%$ Resources:Resource, Clear %>" />
            </div>
            <div class="GiveButton1" style="width: 100px; float: left">
                <asp:LinkButton ID="lbtnSaveAs" SkinID="FilterActions" runat="server" 
                OnClientClick="lbtnSaveAs_OnClientClick(); return false;" OnClick="lbtnSaveAs_Click"
                    ForeColor="White" Text="<%$ Resources:Resource, SaveAs %>" ValidationGroup="FilterValidation"></asp:LinkButton>
            </div>
            <div class="GiveButton1" style="width: 100px; float: left" runat="server" id="divSave">
                <asp:LinkButton ID="lbtnSave" SkinID="FilterActions" runat="server" OnClick="lbtnSave_Click"
                    ForeColor="White" Text="<%$ Resources:Resource, Save %>" ValidationGroup="FilterValidation"></asp:LinkButton>
            </div>
            <div class="GiveButton1" style="width: 100px; float: left" runat="server" id="divDelete">
                <asp:LinkButton ID="lbtnDelete" runat="server" OnClick="lbtnDelete_Click" ForeColor="White"
                    Text="<%$ Resources:Resource, Delete %>" OnClientClick="lbtnDelete_OnClientClick(); return false; "></asp:LinkButton>
            </div>
        </div>
    </div>
</div>
<custom:StorageControl ID="CurrentFilterStateStorage" StorageType="Session" runat="server" />
<custom:StorageControl ID="CurrentUserFilterNameStorage" StorageType="Session" runat="server" />
<custom:StorageControl ID="UserFiltersTemproraryStorage" StorageType="Session" runat="server" />
<script type="text/javascript" language="javascript">
    function lbtnSaveAs_OnClientClick() {
        if (typeof Page_ClientValidate == 'function') {
            if (!Page_ClientValidate('FilterValidation')) {
                return;
            }
        }
        //eval used to hide validation warning
        eval('<%= saveAsDialogFilter.JavascriptObjectShow %>');
    }

    function FilterSaveAsDialogCallback() {
        document.getElementById('<%= lbtnSaveAs.ClientID %>').onclick = null;
        window.location.href = document.getElementById('<%= lbtnSaveAs.ClientID %>').href;
    }

    function lbtnDelete_OnClientClick() {
        Ext.MessageBox.buttonText.yes = '<%= Resources.Resource.Yes %>';
        Ext.MessageBox.buttonText.no = '<%= Resources.Resource.No %>';
        Ext.MessageBox.confirm('<%= Resources.Resource.Delete %>', '<%= Resources.Resource.AreYouSureFilter %>',
            function (btn) {
                if (btn == "yes") {
                    document.getElementById('<%= lbtnDelete.ClientID %>').onclick = null;
                    window.location.href = document.getElementById('<%= lbtnDelete.ClientID %>').href;
                }
            });

    }
</script>
<cc1:SaveAsDialog ID="saveAsDialogFilter" runat="server" NameEmptyErrorMessage="<%$ Resources:Resource, ErrorFilterNameEmpty %>"
    NameRestrictedErrorMessage="<%$ Resources:Resource, ErrorFilterNameRestricted %>" 
    NameExistsConfirmRewriteMessage="<%$ Resources:Resource, ConfirmRewriteFilter %>" 
    CallbackFunction="FilterSaveAsDialogCallback"/>