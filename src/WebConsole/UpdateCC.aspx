<%@ Page Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="UpdateCC.aspx.cs" Inherits="UpdateCC" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
    <div class="title"><%=Resources.Resource.UpdateCC%></div>
   <script type="text/javascript">
    $(document).ready(function(){
     $("#tabs").tabs({ cookie: { expires: 30 } });
     });
     
     function HideTable0()
     {
        var cbox = $("#<%= cboxPeriodicalUpdateEnabled.ClientID %>").is(":checked");
        $("#tr0").attr("disabled", !cbox);
        $("#<%= tboxUpdateSource.ClientID%>").attr("disabled", !cbox);
        $("#<%= tboxPeriodicalUpdated.ClientID%>").attr("disabled", !cbox);
     }
     function HideTable1()
     {
        var cbox = $("#<%= cboxProxyEnabled.ClientID %>").is(":checked");
        $("#<%= tboxProxyAddress.ClientID%>").attr("disabled", !cbox);
        $("#<%= tboxProxyPort.ClientID%>").attr("disabled", !cbox);
     }
     function HideTable2()
     {
        var cbox = $("#<%= cboxAuthorizationEnabled.ClientID %>").is(":checked");
        $("#<%= tboxAuthorizationUserName.ClientID%>").attr("disabled", !cbox);
        $("#<%= tboxAuthorizationPassword.ClientID%>").attr("disabled", !cbox);
        $("#<%= trAuth.ClientID%>").attr("disabled", !cbox);
     }
     function HideTable3()
     {
        var cbox = $("#<%= cboxImpersonationAccountEnabled.ClientID %>").is(":checked");
        $("#<%= tboxImpersonationAccountUsername.ClientID%>").attr("disabled", !cbox);
        $("#<%= tboxImpersonationAccountPassword.ClientID%>").attr("disabled", !cbox);
     }
</script>

<div id='tabs'>
      <ul>
        <li><a href='#0'><span><%=Resources.Resource.Main %></span></a></li>
        <li><a href='#1'><span><%=Resources.Resource.Proxy %></span></a></li>
        <li><a href='#2'><span><%=Resources.Resource.Authorization %></span></a></li>
        <li><a href='#3'><span><%=Resources.Resource.Impersonations %></span></a></li>
      </ul>
      <div id='0'>
           <table class="ListContrastTableMain" style="width:700px">
            <tr>
                <td colspan=2>
                    <asp:CheckBox ID="cboxPeriodicalUpdateEnabled" runat="server" onclick="HideTable0()" />
                </td>
            </tr>
            <tr>
                <td colspan=2>
                    <asp:Label runat="server" id="lblUpdateSource" SkinId="LeftLabel"/>
                    <asp:TextBox runat="server" ID="tboxUpdateSource" style="width:400px;" />
                </td>
            </tr>
             <tr>
                <td colspan=2>
                    <asp:Label runat="server" id="lblPeriodicalUpdate" SkinId="LeftLabel"  />
                    <asp:TextBox runat="server" ID="tboxPeriodicalUpdated" />
                </td>
            </tr>
            </table>
      </div>
      <div id='1'>
            <table class="ListContrastTableMain" style="width:700px">
             <tr>
                <td>
                    <asp:CheckBox ID="cboxProxyEnabled" runat="server" onclick="HideTable1()" />
                </td>
            </tr>
            <tr>
               <td>
                    <asp:Label runat="server" id="lblProxyAddress" Width=100px />
                    <asp:TextBox runat="server" ID="tboxProxyAddress" />
                </td>
            </tr>
            <tr>
                <td>
                  <asp:Label runat="server" id="lblProxyPort" Width=100px />
                  <asp:TextBox runat="server" ID="tboxProxyPort"/>
                </td>
            </tr>
            </table>
      </div>
      <div id='2'>
        <table class="ListContrastTableMain" style="width:700px">
            <tr>
                    <td>
                        <asp:CheckBox ID="cboxAuthorizationEnabled" runat="server"  onclick="HideTable2()" />
                    </td>
                </tr>
                <tr>
                   <td>
                        <asp:Label runat="server" id="lblAuthorizationUserName" Width=100px/>
                        <asp:TextBox runat="server" ID="tboxAuthorizationUserName" />
                    </td>
                 </tr>
                 <tr>
                    <td>
                        <asp:Label runat="server" id="lblAuthorizationPassword" Width=100px />
                        <asp:TextBox runat="server" ID="tboxAuthorizationPassword" TextMode="Password" />
                    </td>
                </tr>
                 <tr id="trAuth" runat="server">
                    <td>
                        <asp:CheckBox ID="cboxAuthorizationNTLMEnabled" runat="server"/>
                    </td>
            </tr>
          </table>
      </div>
      <div id='3'>
        <table class="ListContrastTableMain" style="width:700px">
             <tr>
                    <td>
                        <asp:CheckBox ID="cboxImpersonationAccountEnabled" runat="server"  onclick="HideTable3()" />
                    </td>
                </tr>
                <tr>
                   <td>
                        <asp:Label runat="server" id="lblImpersonationAccountUsername" Width=100px  />
                        <asp:TextBox runat="server" ID="tboxImpersonationAccountUsername" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" id="lblImpersonationAccountPassword" Width=100px  />
                        <asp:TextBox runat="server" ID="tboxImpersonationAccountPassword" TextMode="Password" />
                    </td>
                </tr>
          </table>
      </div>
</div>
<div class="GiveButton1">
     <asp:LinkButton ID="lbtnSave" runat="server" Text="Save" OnClick="lbtnSave_Click" ForeColor="white" Width="100%" /> 
</div>
</asp:Content>

