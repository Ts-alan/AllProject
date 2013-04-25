<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TemporaryGroupFilter.ascx.cs"
    Inherits="Controls_TemporaryGroupFilter" %>
<%@ Register TagPrefix="custom" Namespace="CustomControls" Assembly="CustomControls" %>
<script language="javascript" type="text/javascript">
    var TemporaryGroupFilter = function () {
        return {
            RegisterClickEvents: function () {
                $('html').click(function () {
                    $('#<%= divOptions.ClientID%>').hide();
                });

                $('#<%= divOptions.ClientID%>').click(function (event) {
                    event.stopPropagation();
                });

                $('#<%= divOptionsHideShow.ClientID%>').click(function (event) {
                    if ($('#<%= divOptions.ClientID%>').is(':hidden')) {
                        $('#<%= divOptions.ClientID%>').show();
                    }
                    else {
                        $('#<%= divOptions.ClientID%>').hide();
                    }
                    event.stopPropagation();
                });
            }
        };
    } ();
</script>
<div class="GiveButton" style="width: 160px; float: left" runat="server" id="divFilterHeader">
    <asp:LinkButton ID="lbtnTempGroupHeader" runat="server" ForeColor="white" Width="100%"
        OnClick="lbtnTempGroupHeader_Click"><%=Resources.Resource.TemporaryGroup%></asp:LinkButton>
</div>
<div id="divOptionsHideShow" class="GiveButton" runat="server" style="width: 18px;
    border-left-width: 0px; float: left;">
    <asp:Image ID="imgOptionsHideShow" runat="server" />
</div>
<div runat="server" id="divOptions" style="z-index: 200; position: absolute; display: none; margin-top:20px;">
    <div class="GiveButton" style="width: 180px;" runat="server" id="div1">
        <asp:LinkButton ID="lbtnAddToTempGroup" runat="server" ForeColor="white" Width="100%" OnClick="lbtnAddToTempGroup_Click"><%=Resources.Resource.Add %></asp:LinkButton>
    </div>
    <div class="GiveButton" style="width: 180px; border-top-width:0px;" runat="server" id="div2">
        <asp:LinkButton ID="lbtnClearTempGroup" runat="server" ForeColor="white" Width="100%" OnClick="lbtnClearTempGroup_Click"><%=Resources.Resource.Clear %></asp:LinkButton>
    </div>
        <div class="GiveButton" style="width: 180px;" runat="server" id="div3">
        <asp:LinkButton ID="lbtnApplyTempGroup" runat="server" ForeColor="white" Width="100%" OnClick="lbtnApplyTempGroup_Click"><%=Resources.Resource.Apply %></asp:LinkButton>
    </div>
    <div class="GiveButton" style="width: 180px; border-top-width:0px;" runat="server" id="div4">
        <asp:LinkButton ID="lbtnCancelTempGroup" runat="server" ForeColor="white" Width="100%" OnClick="lbtnCancelTempGroup_Click"><%=Resources.Resource.CancelButtonText %></asp:LinkButton>
    </div>
</div>
<custom:StorageControl ID="TemporaryGroupFilterStorage" StorageName="TemporaryGroupFilterStorage"
    StorageType="Session" runat="server" />
