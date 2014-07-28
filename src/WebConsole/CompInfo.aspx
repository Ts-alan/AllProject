<%@ Page Language="C#" validateRequest="false" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="CompInfo.aspx.cs" Inherits="CompInfo" Title="Untitled Page" %>
<%@ OutputCache Location="None" %>
<%@ Register Src="~/Controls/TaskConfigureMonitor.ascx" TagName="Monitor" TagPrefix="cc" %>
<%@ Register Src="~/Controls/TaskConfigureLoader.ascx" TagName="Loader" TagPrefix="cc" %>
<%@ Register Src="~/Controls/TaskConfigureQuarantine.ascx" TagName="Quarantine" TagPrefix="cc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true"></ajaxToolkit:ToolkitScriptManager>
 <script type="text/javascript">

     function alertMessage(message) {
         alert(message);
     };
     function pageLoad() {
         $(document).ready(function () {
             $("button").button();
             $("input[type='submit']").button();
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
                     $('button[dpc]').css('display', 'none');
                     $('input[dpc]').css('display', 'none');
                 }
             });
         }
         /*------- работа с устройствами для компьютера --------*/
         /* изменение комментария (вызов диалогового окна)*/
         $(document).on("click", 'img[comdp]', function () {
             var id = $(this).attr('comdp');
             var serial = $(this).attr('serialdp');
             $.ajax({
                 type: "POST",
                 url: "DevicesPolicy.aspx/GetChangeCommentDialog",
                 data: "{id:" + id + "}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (msg) {
                     $("#commentDialog").html('');
                     var d = $('#commentDialog');
                     d.html(msg);
                     var dOpt = {
                         title: serial,
                         width: '700px',
                         modal: true,
                         resizable: false
                     };
                     d.dialog(dOpt);
                     d.find("button").button();
                 },
                 error: function (msg) {
                     alert(msg);
                 }
             })
         });
         /* изменение комментария */
         $(document).on("click", "button[dcdpc]", function () {
             var id = $(this).attr('dcdpc');
             var comment = $('input[dcdpc]').val();
             $.ajax({
                 type: "POST",
                 url: "DevicesPolicy.aspx/ChangeComment",
                 data: "{id:" + id + ", comment:'" + comment + "'}",
                 contentType: "application/json; charset=utf-8",
                 success: function () {

                     $("td[dp=" + id + "][type='comment']").html(comment);
                     $("span[dp=" + id + "][type='comment']").html(comment);
                     $("#commentDialog").dialog('close');
                 },
                 error: function (msg) {
                     ShowJSONMessage(msg);
                 }
             });
             return false;
         });
         /*удаление devicePolicy для компьютера */
         $(document).on("click", "img[deldp]", function () {
             var id = $(this).attr('deldp');
             $.ajax({
                 type: "POST",
                 url: "DevicesPolicy.aspx/RemoveDevicePolicy",
                 data: "{id:" + id + "}",
                 contentType: "application/json; charset=utf-8",
                 success: function () {
                     $("img[deldp=" + id + "]").parent().parent().remove();
                 },
                 error: function (msg) {
                     ShowJSONMessage(msg);
                 }
             });
             return false;
         });
         /* change devicePolicy state for computer*/
         $(document).on("click", "img[cp]", function () {
             var dp = $(this).attr('dp');
             var cp = $(this).attr('cp');
             var state = $(this).attr('state');
             //установка смены чекбоксов
             changeState($(this), dp, cp, state);
         });
         function changeState(img, dp, cp, state) {
             if (state == "Enabled" || state == "Undefined") {
                 img.attr('state', "Disabled");
                 state = "Disabled";
                 img.attr('src', "App_Themes/Main/Images/disabled.gif");
             } else if (state == "Disabled") {
                 img.attr('state', "Enabled");
                 state = "Enabled";
                 img.attr('src', "App_Themes/Main/Images/enabled.gif");
             }
             $.ajax({
                 type: "POST",
                 url: "DevicesPolicy.aspx/ChangeDevicePolicyStateComputer",
                 data: "{dp:" + dp + ",cp:" + cp + ",state:'" + state + "'}",
                 contentType: "application/json; charset=utf-8",
                 success: function () {

                 }
             });
         };
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
     </div>

     </div>
 </div>
<h3><a href='#'><%=Resources.Resource.Devices%></a></h3>
 <div>
    <div id="divDevices" runat="server" style="width:850px;"></div>
 </div>
<%--<h3><a href="#"><%=Resources.Resource.Filter%></a></h3>
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
 </div>--%>
</div>
    <div id="commentDialog" style="display:none"></div>
</asp:Content>