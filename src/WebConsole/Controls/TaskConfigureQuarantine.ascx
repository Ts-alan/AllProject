<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureQuarantine.ascx.cs" Inherits="Controls_TaskConfigureQuarantine" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.TaskNameConfigureQuarantine%></div>

<script language="javascript" type="text/javascript">
    function pageLoad() {
        $("#Tabs").tabs({ cookie: { expires: 30} });

    }
function ClickCheckBox()
{
    var cbox = $get('<%=cboxMaintenancePeriod.ClientID%>');
    var elem1 = $get('<%=cboxMaximumQuarantineSize.ClientID%>');
    var elem2 = $get('<%=tboxServicePeriod.ClientID%>');
    var elem3 = $get('<%=tboxMaxSize.ClientID%>');
    var elem4 = $get('<%=cboxMaximumStorageTime.ClientID%>');
    var elem5 = $get('<%=tboxMaximumStorageTime.ClientID%>');
    var elem6 = $get('<%=cboxAutomaticallySendSuspiciousObject.ClientID%>');
    var elem7 = $get('<%=cboxInteractive.ClientID%>');
        
    CheckClick(cbox, elem1);
    CheckClick(cbox, elem2);
    CheckClick(cbox, elem3);
    CheckClick(cbox, elem4);
    CheckClick(cbox, elem5);
    CheckClick(cbox, elem6);
    CheckClick(cbox, elem7);    
}

function CheckClick(cbox, elem)
{
    elem.disabled = !cbox.checked;
}

</script>
<div id="Tabs" style="width:560px">
    <ul>
        <li><a href="#tab1"><%=Resources.Resource.CongQtnGeneral%></a> </li>
        <li><a href="#tab2"><%=Resources.Resource.CongQtnMaintenance%></a> </li>
    </ul>
    <div id="tab1" class="divSettings" >
        <table  class="ListContrastTable">
            <tr>
                <td style="padding-left:5px;" align="right" >
                    <%= Resources.Resource.CongQtnRemoteStorage  %>&nbsp;&nbsp;
                </td>
                <td align=right>
                    <asp:TextBox ID="tboxRemote" runat="server" style="width:300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">    
                    <asp:CheckBox ID="cboxUseProxyServer" runat="server" /><%=Resources.Resource.CongLdrUseProxyServer%>
                </td>
            </tr>
            <tr>
                <td style="padding-left:5px;">
                    <asp:Label runat="server" ID="lblCongLdrAddress" SkinId="LabelContrast" width="100px" ></asp:Label>&nbsp;&nbsp;<asp:TextBox ID="tboxAddress" runat="server"></asp:TextBox>
                </td>
                <td align=right>
                    <asp:Label runat="server" ID="lblCongLdrPort" SkinId="LabelContrast" width="100px"></asp:Label>&nbsp;&nbsp;<asp:TextBox ID="tboxPort" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-left:5px;">
                    <asp:Label runat="server" ID="lblCongLdrUserName" SkinId="LabelContrast" width="100px"></asp:Label>&nbsp;&nbsp;<asp:TextBox ID="tboxUserName" runat="server"></asp:TextBox>
                </td>
                <td align=right>
                    <asp:Label runat="server" ID="lblCongLdrPassword" SkinId="LabelContrast" width="100px"></asp:Label>&nbsp;&nbsp;<asp:TextBox ID="tboxPassword" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
        </table>   
    </div>
    <div id="tab2" class="divSettings" >
        <table class="ListContrastTable">
            <tr>
                <td>
                    <asp:CheckBox ID="cboxMaintenancePeriod" runat="server" onclick="ClickCheckBox()" />
                    <%= Resources.Resource.CongQtnMaintenancePeriod %>
                </td>
                <td>
                    <asp:TextBox ID="tboxServicePeriod" runat="server" style="width:60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cboxMaximumQuarantineSize" runat="server" />
                    <%= Resources.Resource.CongQtnMaximumQuarantineSize %>
                </td>
                <td>
                    <asp:TextBox ID="tboxMaxSize" runat="server" style="width:60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cboxMaximumStorageTime" runat="server" />
                    <%= Resources.Resource.CongQtnMaximumStorageTime %>
                </td>
                <td>
                    <asp:TextBox ID="tboxMaximumStorageTime" runat="server" style="width:60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan='2'>
                    <asp:CheckBox ID="cboxAutomaticallySendSuspiciousObject" runat="server" />
                    <%= Resources.Resource.CongQtnAutomaticallySendSuspiciousObject %>
                </td>
            </tr>
            <tr>
                <td colspan='2'>
                    <asp:CheckBox ID="cboxInteractive" runat="server" />
                    <%= Resources.Resource.CongQtnInteractiveMaintenance %>
                </td>
            </tr>
        </table>
    </div>
</div>
