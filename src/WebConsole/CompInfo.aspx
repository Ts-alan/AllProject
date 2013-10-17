<%@ Page Language="C#" validateRequest="false" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="CompInfo.aspx.cs" Inherits="CompInfo" Title="Untitled Page" %>
<%@ OutputCache Location="None" %>
<%@ Register Src="~/Controls/TaskConfigureMonitor.ascx" TagName="Monitor" TagPrefix="cc" %>
<%@ Register Src="~/Controls/TaskConfigureLoader.ascx" TagName="Loader" TagPrefix="cc" %>
<%@ Register Src="~/Controls/TaskConfigureQuarantine.ascx" TagName="Quarantine" TagPrefix="cc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true"></ajaxToolkit:ToolkitScriptManager>
 <script type="text/javascript">
     $(function () {
         $("#accordion").accordion({ heightStyle: 'content' });
         $("#accordion tr[isHover]").hover(function () {
             $(this).css('background-color', 'yellow');
         },
            function () {
                $(this).css('background-color', '');
            });
         $('#<%= divDevices.ClientID %>').ready(function () {
             UpdateDevicesList();
         });

         var divComponents = '#<%=divComponents.ClientID %>';
         $(divComponents).accordion({ collapsible: true, active: false, heightStyle: 'content' });
     });

     function UpdateDevicesList() {
         var id = $('#<%= divDevices.ClientID %>').attr('dpc');
         $.ajax({
             type: "POST",
             url: "DevicesPolicy.aspx/GetComputersData",
             data: "{id:" + id + "}",
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (msg) {
                 $('div[dpc]').html(msg);
                 $('button[dpc]').attr('disabled', 'disabled');
                 $('input[dpc]').attr('disabled', 'disabled');
             }
         });
     }
    </script>
<div id="accordion">
<h3><a href="#"><%=Resources.Resource.ComputerInfo%></a></h3>
<div>
 <div class="divSettings">
    <table class="ListContrastTable" style="width:850px;">
    <tr isHover>
        <td>
            <%=Resources.Resource.ComputerName%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblComputerName" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.UserLogin%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblUserLogin" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.IPAddress%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblIPAdress" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.DomainName%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblDomainName" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.VBA32Version%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblVBA32Version" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.LatestUpdate%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblLatestUpdate" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.RecentActive%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblRecentActive" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.OSType%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblOSType" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.CPU%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblCPU" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.RAM%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblRAM" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.ControlCenter%>:
        </td>
        <td style="width:50%">
            <asp:Image runat="server" ID="imgControlCenter" />
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.VBA32Integrity%>:
        </td>
        <td style="width:50%">
            <asp:Image runat="server" ID="imgVBA32Integrity" />
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.VBA32KeyValid%>:
        </td>
        <td style="width:50%">
            <asp:Image runat="server" ID="imgKeyValid" />
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.LatestInfected%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblLatestInfected" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.LatestMalware%>:
        </td>
        <td style="width:50%">
            <asp:Label ID="lblLatestMalware" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
        <td>
            <%=Resources.Resource.Policy%>
        </td>
        <td style="width:50%">
            <asp:Label ID="lblPolicyName" runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr isHover>
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
     <div class="divSettings" style="width:850px;">
     <div id='divComponents' runat="server">
        <h3><a><img runat="server" id="imgLoader" style="margin-right: 10px;" /><span>Vba32 Loader</span></a></h3>        
        <div>
            <cc:Loader runat="server" ID="tskLoader" HideHeader="true" Visible="false" />
            <span runat="server" id="lblLoader" visible="false"></span>
        </div>
        <h3><a><img runat="server" id="imgMonitor" style="margin-right: 10px;" /><span>Vba32 Monitor</span></a></h3>        
        <div>
            <cc:Monitor runat="server" ID="tskMonitor" HideHeader="true" Visible="false" />
            <span runat="server" id="lblMonitor" visible="false"></span>
        </div>
        <h3><a><img runat="server" id="imgQuarantine" style="margin-right: 10px;" /><span>Vba32 Quarantine</span></a></h3>        
        <div>
            <cc:Quarantine runat="server" ID="tskQuarantine" HideHeader="true" Visible="false" />
            <span runat="server" id="lblQuarantine" visible="false"></span>
        </div>
            
     </div>
     </div>
 </div>
<h3><a href='#'><%=Resources.Resource.Devices%></a></h3>
 <div>
    <div id="divDevices" runat="server" style="width:850px;"></div>
 </div>
<h3><a href="#"><%=Resources.Resource.Filter%></a></h3>
<div>
 <div class="divSettings">
    <table class="ListContrastTable" style="width:850px;">
        <tr isHover>
            <td>
                &nbsp;&nbsp;
                <asp:LinkButton ID="lbtnComputersList" runat="server" OnClick="lbtnComputersList_Click" >
                    <%=Resources.Resource.Computers%>
                </asp:LinkButton>
            </td>
        </tr>
        <tr isHover>
            <td>
                &nbsp;&nbsp;
                <asp:LinkButton ID="lbtnEventsList" runat="server" OnClick="lbtnEventsList_Click">
                    <%=Resources.Resource.Events %>
                </asp:LinkButton>
            </td>
        </tr>
        <tr isHover>
            <td>
                &nbsp;&nbsp;
                <asp:LinkButton ID="lbtnTasksList" runat="server" OnClick="lbtnTasksList_Click">
                    <%=Resources.Resource.Tasks%>
                </asp:LinkButton>
            </td>
        </tr>
        <tr isHover>
            <td>
                &nbsp;&nbsp;
                <asp:LinkButton ID="lbtnComponentsList" runat="server" OnClick="lbtnComponentsList_Click">
                    <%=Resources.Resource.Components%>
                </asp:LinkButton>
            </td>
        </tr>
        <tr isHover>
            <td>
                &nbsp;&nbsp;
                <asp:LinkButton ID="lbtnProcessesList" runat="server" OnClick="lbtnProcessesList_Click" >
                    <%=Resources.Resource.Processes%>
                </asp:LinkButton>
            </td>
        </tr>
    </table>
 </div>
 </div>
</div>
</asp:Content>