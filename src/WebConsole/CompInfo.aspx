<%@ Page Language="C#" validateRequest=false MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="CompInfo.aspx.cs" Inherits="CompInfo" Title="Untitled Page" %>
<%@ OutputCache Location="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
 <script type="text/javascript">
        $(function() {
            $("#accordion").accordion();
            $("#accordion tr").hover(function() {
                $(this).css('background-color', 'yellow');
            },
            function() {
                $(this).css('background-color', '');
            });
            $('#<%= divDevices.ClientID %>').ready(function (){
                 UpdateDevicesList();
             });
        }); 
        function UpdateDevicesList()
        {
             var id = $('#<%= divDevices.ClientID %>').attr('dpc');
                $.ajax({
                    type: "POST",
                    url: "DevicesPolicy.aspx/GetComputersData",
                    data: "{id:" + id + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(msg) {
                        $('div[dpc]').html(msg);  
                        $('button[dpc]').attr('disabled','disabled');
                        $('input[dpc]').attr('disabled','disabled');
                    }
                })
        }
    </script>
    <div id="accordion">
	<h3><a href="#"><%=Resources.Resource.ComputerInfo%></a></h3>
	<div>

 <div class="divSettings">
    <table class="ListContrastTable" style="width:600px;">
    <tr>
        <td>
            <%=Resources.Resource.ComputerName%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblComputerName" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.UserLogin%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblUserLogin" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.IPAddress%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblIPAdress" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
     <tr>
        <td>
            <%=Resources.Resource.DomainName%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblDomainName" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.VBA32Version%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblVBA32Version" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.LatestUpdate%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblLatestUpdate" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.RecentActive%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblRecentActive" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.OSType%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblOSType" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.CPU%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblCPU" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.RAM%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblRAM" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.ControlCenter%>:
        </td>
        <td style="width:50%">
            <asp:Image runat=server ID=imgControlCenter />
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.VBA32Integrity%>:
        </td>
        <td style="width:50%">
            <asp:Image runat=server ID=imgVBA32Integrity />
        </td>
    </tr>
     <tr>
        <td>
            <%=Resources.Resource.VBA32KeyValid%>:
        </td>
        <td style="width:50%">
            <asp:Image runat=server ID=imgKeyValid />
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.LatestInfected%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblLatestInfected" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.LatestMalware%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblLatestMalware" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.Policy%>
        </td>
        <td style="width:50%">
            <asp:Label ID="lblPolicyName" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.Description%>:
        </td>
        <td style="width:50%">
            <asp:TextBox ID="tboxDescription" runat=server />&nbsp;&nbsp;
            <asp:LinkButton ID="lbtnSaveDescription" runat=server OnClick="lbtnSaveDescription_Click" />
        </td>
    </tr>
    </table>
 </div>
</div>
 <h3><a href="#"><%=Resources.Resource.Components%></a></h3>
 <div>
     <div class="divSettings" style="width:600px;">
        <asp:Table table runat=server id=tblComponentState class=ListContrastTable>
        </asp:Table>

        </div>
</div>
<h3><a href='#'><%=Resources.Resource.Devices%></a></h3>
 <div>
    <div id="divDevices" runat="server" style="width:600px;"></div>
 </div>
<h3><a href="#"><%=Resources.Resource.Filter%></a></h3>
<div>
 <div class="divSettings">
    <table class="ListContrastTable" style="width:600px;">
    <tr>
         <td>
            &nbsp;&nbsp;<asp:LinkButton ID="lbtnComputersList" runat="server" OnClick="lbtnComputersList_Click" >
                <%=Resources.Resource.Computers%>
            </asp:LinkButton>
        </td>
    </tr>
    
    <tr>
        <td>
            &nbsp;&nbsp;<asp:LinkButton ID="lbtnEventsList" runat="server" OnClick="lbtnEventsList_Click">
                <%=Resources.Resource.Events %>
            </asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp;<asp:LinkButton ID="lbtnTasksList" runat="server" OnClick="lbtnTasksList_Click">
                <%=Resources.Resource.Tasks%>
            </asp:LinkButton>
        </td>
        </tr>
        <tr>
         <td>
            &nbsp;&nbsp;<asp:LinkButton ID="lbtnComponentsList" runat="server" OnClick="lbtnComponentsList_Click">
                <%=Resources.Resource.Components%>
            </asp:LinkButton>
        </td>
    </tr>
     <tr>
         <td>
            &nbsp;&nbsp;<asp:LinkButton ID="lbtnProcessesList" runat="server" OnClick="lbtnProcessesList_Click" >
                <%=Resources.Resource.Processes%>
            </asp:LinkButton>
        </td>
    </tr>
    </table>
 </div>
 </div>
</div>
</asp:Content>

