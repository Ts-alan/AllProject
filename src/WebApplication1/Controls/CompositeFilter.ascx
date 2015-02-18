<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Controls_CompositeFilter" Codebehind="CompositeFilter.ascx.cs" %>
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
            <asp:LinkButton ID="lbtnApply" SkinID="ButtonSmall" runat="server" OnClick="lbtnApply_Click" ValidationGroup="FilterValidation" >
                <%=Resources.Resource.Apply %>
            </asp:LinkButton>
            <asp:LinkButton ID="lbtnClear" SkinID="ButtonSmall" runat="server" OnClick="lbtnClear_Click">
                <%=Resources.Resource.Clear %>
            </asp:LinkButton>
            <asp:LinkButton ID="lbtnSaveAs" SkinID="ButtonSmall" runat="server" OnClientClick="lbtnSaveAs_OnClientClick(); return false;"
                 OnClick="lbtnSaveAs_Click" ValidationGroup="FilterValidation">
                 <%=Resources.Resource.SaveAs %>
            </asp:LinkButton>
            <asp:LinkButton ID="lbtnSave" SkinID="ButtonSmall" runat="server" OnClick="lbtnSave_Click" ValidationGroup="FilterValidation">
                <%=Resources.Resource.Save %>
            </asp:LinkButton>
            <asp:LinkButton ID="lbtnDelete" runat="server" SkinID="ButtonSmall" OnClick="lbtnDelete_Click" OnClientClick="lbtnDelete_OnClientClick(); return false; ">
                <%=Resources.Resource.Delete %>
            </asp:LinkButton>
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
        $( "#dialog-message" ).dialog({
            modal: true,
            buttons: {
                    '<%= Resources.Resource.Yes %>': function() {
                    document.getElementById('<%= lbtnDelete.ClientID %>').onclick = null;
                    window.location.href = document.getElementById('<%= lbtnDelete.ClientID %>').href;
                    $( this ).dialog( "close" );
                    },
                    '<%= Resources.Resource.No %>': function() {
                        $( this ).dialog( "close" );
                    }
                }
            });
        }
</script>
<cc1:SaveAsDialog ID="saveAsDialogFilter" runat="server" NameEmptyErrorMessage="<%$ Resources:Resource, ErrorFilterNameEmpty %>"
    NameRestrictedErrorMessage="<%$ Resources:Resource, ErrorFilterNameRestricted %>" 
    NameExistsConfirmRewriteMessage="<%$ Resources:Resource, ConfirmRewriteFilter %>" 
    CallbackFunction="FilterSaveAsDialogCallback"/>
<div id="dialog-message" title="<%= Resources.Resource.Delete %>" style="display:none">
  <p>
   <%= Resources.Resource.AreYouSureFilter %>
  </p>
</div>