<%@ Page Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="UpdateCC.aspx.cs" Inherits="UpdateCC" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager runat="server" ID="scriptManager1" EnableScriptGlobalization="true"></ajaxToolkit:ToolkitScriptManager>
<script type="text/javascript">
     $(document).ready(function(){
     $("#tabs").tabs({ cookie: { expires: 30 } });
     });

     function HideTable1() {
         var cbox = $("#<%= cboxProxyEnabled.ClientID %>").is(":checked");
         $("#<%= tboxProxyAddress.ClientID%>").attr("disabled", !cbox);
         $("#<%= tboxProxyPort.ClientID%>").attr("disabled", !cbox);
         HideTable1_1(cbox);
     }

     function HideTable1_1(checked) {
         $("#<%= cboxProxyAuthorizationEnabled.ClientID %>").attr("disabled", !checked);
         var cbox = $("#<%= cboxProxyAuthorizationEnabled.ClientID %>").is(":checked");
         if (!checked) {             
             cbox = false;
         }
         $("#<%= tboxProxyAuthorizationUserName.ClientID%>").attr("disabled", !cbox);
         $("#<%= tboxProxyAuthorizationPassword.ClientID%>").attr("disabled", !cbox);
     }

    function HideTable2() {
        var cbox = $("#<%= cboxAuthorizationEnabled.ClientID %>").is(":checked");
        $("#<%= tboxAuthorizationUserName.ClientID%>").attr("disabled", !cbox);
        $("#<%= tboxAuthorizationPassword.ClientID%>").attr("disabled", !cbox);
    }     
</script>
<div class="title"><%=Resources.Resource.UpdateCC%></div>
<asp:UpdatePanel runat="server" ID="upnlUpdate" UpdateMode="Always">
<ContentTemplate>
    <div>
        <asp:LinkButton ID="lbtnUpdate" runat="server" OnClick="lbtnUpdate_Click"
            CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" 
            style="padding: 5px; margin-top: 10px; margin-bottom: 10px; margin-left: 5px; width: 100px;">
                <%=Resources.Resource.UpdateButtonText%>
        </asp:LinkButton>
        <asp:LinkButton ID="lbtnCancelUpdate" runat="server" OnClick="lbtnCancelUpdate_Click"
            CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" 
            style="padding: 5px; margin-top: 10px; margin-bottom: 10px; margin-left: 5px; width: 100px;">
                <%=Resources.Resource.CancelButtonText%>
        </asp:LinkButton>
    </div>
    <div runat="server" id="divLastUpdate" style="padding: 5px 5px 0px 5px;">
        <asp:Label runat="server" ID="lblLastUpdate"></asp:Label>
    </div>
    <div style="padding: 5px;">
        <asp:Label runat="server" ID="lblLastSuccessUpdate"></asp:Label>
    </div>
    <asp:Timer runat="server" ID="timer1" OnTick="timer1_tick" Interval="10000"></asp:Timer>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="timer1" EventName="Tick" />
</Triggers>
</asp:UpdatePanel>

<div class="title"><%=Resources.Resource.Settings%></div>
<div id='tabs'>
      <ul>
        <li><a href='#0'><span><%=Resources.Resource.Main %></span></a></li>
        <li><a href='#2'><span><%=Resources.Resource.Authorization %></span></a></li>
        <li><a href='#1'><span><%=Resources.Resource.Proxy %></span></a></li>
      </ul>
      <div id='0'>
           <table class="ListContrastTableMain" style="width:700px;">
           <tr>
               <td style="padding-top: 5px;padding-bottom: 5px;">
                   <asp:Label runat="server" id="lblUpdateSource" SkinId="LeftLabel"/>
               </td>
               <td style="padding-top: 5px;padding-bottom: 5px;">
                   <asp:TextBox runat="server" ID="tboxUpdateSource" style="width:400px;" />
               </td>
           </tr>            
           </table>
      </div>
      <div id='1'>
            <table class="ListContrastTableMain" style="width:700px">
             <tr>
                <td style="padding-top: 5px;padding-bottom: 5px;">
                    <asp:CheckBox ID="cboxProxyEnabled" runat="server" onclick="HideTable1()" />
                </td>
            </tr>
            <tr>
               <td style="padding-left: 20px;">
                    <asp:Label runat="server" id="lblProxyAddress" Width="100px" />
                    <asp:TextBox runat="server" ID="tboxProxyAddress" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 20px;">
                    <asp:Label runat="server" id="lblProxyPort" Width="100px" />
                    <asp:TextBox runat="server" ID="tboxProxyPort"/>
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;padding-bottom: 5px;">
                    <asp:CheckBox ID="cboxProxyAuthorizationEnabled" runat="server"  onclick="HideTable1_1(true)" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 20px;">
                    <asp:Label runat="server" id="lblProxyAuthorizationUserName" Width="100px"/>
                    <asp:TextBox runat="server" ID="tboxProxyAuthorizationUserName" autocomplete="off" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 20px;padding-bottom: 5px;">
                    <asp:Label runat="server" id="lblProxyAuthorizationPassword" Width="100px" />
                    <asp:TextBox runat="server" ID="tboxProxyAuthorizationPassword" TextMode="Password" autocomplete="off" />
                </td>
            </tr>
            </table>
      </div>
      <div id='2'>
        <table class="ListContrastTableMain" style="width:700px">
            <tr>
                <td style="padding-top: 5px;padding-bottom: 5px;">
                    <asp:CheckBox ID="cboxAuthorizationEnabled" runat="server"  onclick="HideTable2()" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 20px;">
                    <asp:Label runat="server" id="lblAuthorizationUserName" Width="100px"/>
                    <asp:TextBox runat="server" ID="tboxAuthorizationUserName" autocomplete="off" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 20px;padding-bottom: 5px;">
                    <asp:Label runat="server" id="lblAuthorizationPassword" Width="100px" />
                    <asp:TextBox runat="server" ID="tboxAuthorizationPassword" TextMode="Password" autocomplete="off" />
                </td>
            </tr>                
          </table>
      </div>
</div>
<div>
    <asp:LinkButton ID="lbtnSave" runat="server" OnClick="lbtnSave_Click"
        CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" 
        style="padding: 5px; margin-top: 10px; margin-bottom: 10px; margin-left: 5px; width: 100px;">
            <%=Resources.Resource.Save%>
    </asp:LinkButton>
</div>
</asp:Content>

