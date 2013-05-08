<%@ Page Title="" Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true"
    CodeFile="AsynchLanScan.aspx.cs" Inherits="AsynchLanScan" %>

<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register TagPrefix="custom" Namespace="CustomControls" Assembly="CustomControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" runat="Server">
    <div class="title">
        <%=Resources.Resource.PageLanScanTitle%>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <script type="text/javascript" language="javascript" src="AsynchRequestErrorHandler.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#tabs").tabs({ cookie: { expires: 30} });
        });
        function pageLoad () {
            $("div[class=EditComment]").click(function () {
                var ip = $(this).attr('IP');
                var title = $(this).attr('titleDialog');
                var saveTitle = $(this).attr('saveTitle');
                var isComment = $(this).attr('comment');
                var comment = '';
                if (isComment == 'true')
                    comment = $("span[IP=" + ip + "]").text().replace(/^\s\s*/, '').replace(/\s\s*$/, ''); //trim start&end spaces

                $("#divModalDialog").html('');
                $("#divModalDialog").dialog('destroy');
                var d = $("#divModalDialog");
                d.html(GetHtmlByComment(comment));
                var dOpt = {
                    title: title + ' (' + ip + ')',
                    width: 320,
                    modal: true,
                    resizable: false,
                    buttons: {
                        '<%=Resources.Resource.Save %>': function () {
                            var newVal = $('#txtComment').val().replace(/^\s\s*/, '').replace(/\s\s*$/, '');
                            if (comment != newVal) {
                                $.ajax({
                                    type: "POST",
                                    url: "AsynchLanScan.aspx/SetComment",
                                    data: "{ip:\"" + ip + "\", text:\"" + newVal + "\"}",
                                    contentType: "application/json; charset=utf-8",
                                    error: function (msg) {
                                        ShowJSONMessage(msg);
                                    },
                                    success: function () {
                                        comment = newVal;
                                        $("span[IP=" + ip + "]").text(comment);
                                    }
                                });
                            }
                        }
                    }
                };
                d.dialog(dOpt);


                function GetHtmlByComment(comment) {
                    return "<textarea id='txtComment' rows='5' style='width: 280px;'>" + comment + "</textarea>";
                }
            });


            function ShowJSONMessage(msg) {
                var m = JSON.parse(msg.responseText, function (key, value) {
                    var type;
                    if (value && typeof value === 'object') {
                        type = value.type;
                        if (typeof type === 'string' && typeof window[type] === 'function') {
                            return new (window[type])(value);
                        }
                    }
                    return value;
                });
                alert(m.Message);
            }
        }
    </script>
    
    
        <div id='tabs'>
            <ul>
                <li>
                    <a href='#3'><span>
                    <%=Resources.Resource.Provider%>
                    </span></a>
                </li>
                <li>
                    <a href='#0'><span>
                    <%=Resources.Resource.IPRange + "/" + Resources.Resource.List%>
                    </span></a>
                </li>
                <li>
                    <a href='#1'><span>
                    <%=Resources.Resource.Credentials%>
                    </span></a>
                </li>
                <li>
                    <a href='#4'><span>
                    <%=Resources.Resource.Settings%>
                    </span></a>
                </li>
            </ul>            
            <div id='0'>
            
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                        
                <table class="ListContrastTable" style="width: 700px">
                    <tr>
                        <td valign="top" style="width: 50%">
                            <asp:ListBox ID="lboxCompIncludeList" Width="320px" Height="110px" runat="server" />
                        </td>
                        <td valign="top" align="left" style="width: 50%">
                            <asp:TextBox ID="tboxNewComp" runat="server" Style="width: 250px" /><br />
                            <asp:RequiredFieldValidator ID="requiredNewComp" ControlToValidate="tboxNewComp" 
                            runat="server" ErrorMessage='<%$ Resources:Resource, NewCompRequiredErrorMessage %>'
                            ValidationGroup="NewCompValidation"> <br />
                        </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ControlToValidate="tboxNewComp" ID="regexNewComp" runat="server" 
                            ErrorMessage='<%$ Resources:Resource, IPRangeRegexErrorMessage %>' ValidationGroup="NewCompValidation"
                            ValidationExpression="^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(-(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))?$"
                            ></asp:RegularExpressionValidator>
                            <br />
                            <asp:LinkButton ID="lbtnAddNew" ValidationGroup="NewCompValidation" runat="server" OnClick="lbtnAddNew_Click1" /><br />
                            <asp:LinkButton ID="lbtnRemove" runat="server" OnClick="lbtnRemove_Click" /><br />
                            <br />
                            <asp:FileUpload ID="fuImport" runat="server" Visible="false" Width="120px" /><br />
                            <asp:LinkButton ID="lbtnImport" runat="server" Visible="false" OnClick="lbtnImport_Click"
                                Text="Import" />
                        </td>
                    </tr>
                </table>
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id='1'>
                <div class="ListContrastTable" style="width: 700px">
                    &nbsp;<asp:Label ID="lblDomain" runat="server" Width="80px" />&nbsp;<asp:TextBox
                        ID="tboxDomainCr" runat="server" Style="width: 190px" />&nbsp;                        
                        <br />
                    &nbsp;<asp:Label ID="lblLogin" runat="server" Width="80px" />&nbsp;<asp:TextBox ID="tboxLoginCr"
                        runat="server" Style="width: 190px" />&nbsp;
                        <asp:RequiredFieldValidator ID="requiredLogin" ControlToValidate="tboxLoginCr" 
                        runat="server" ErrorMessage='<%$ Resources:Resource, LoginRequiredErrorMessage %>'
                        ValidationGroup="SettingsValidation">
                        </asp:RequiredFieldValidator>
                        <br />
                    &nbsp;<asp:Label ID="lblPass" runat="server" Width="80px" />&nbsp;<asp:TextBox ID="tboxPasswordCr"
                        runat="server" Style="width: 190px" TextMode="Password" />&nbsp;
                        <asp:RequiredFieldValidator ID="requiredPassword" ControlToValidate="tboxPasswordCr" 
                        runat="server" ErrorMessage='<%$ Resources:Resource, PasswordRequiredErrorMessage %>'
                        ValidationGroup="SettingsValidation">
                        </asp:RequiredFieldValidator>
                        <asp:ValidationSummary ID="SettingsValidationSummary"  runat="server" ShowMessageBox="True"
                         ShowSummary="False" ValidationGroup="SettingsValidation" 
                         HeaderText='<%$ Resources:Resource, CheckCredentials %>' />
                </div>
            </div>
            <div id='3'>
                <div class="ListContrastTable" style="width: 700px">
                    <asp:RadioButtonList runat="server" ID="rbtnlProviders" AutoPostBack="false" ></asp:RadioButtonList>
                </div>
            </div>
            <div id='4'>
                <div class="ListContrastTable" style="width: 700px">
                    <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblPingCount" runat="server" Width="250px" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPingCount" runat="server" Style="width: 70px" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="requiredPingCount" ControlToValidate="txtPingCount" 
                                runat="server" ErrorMessage='<%$ Resources:Resource, RequestPacketCountRequiredErrorMessage %>'
                                ValidationGroup="SettingsValidation"></asp:RequiredFieldValidator>
                            <br />
                            <asp:RangeValidator ID="rangePingCount" runat="server" ControlToValidate="txtPingCount"
                                ValidationGroup="SettingsValidation" MinimumValue="1" MaximumValue="10" Type="Integer" ></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPingTimeout" runat="server" Width="250px" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPingTimeout" runat="server" Style="width: 70px" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="requiredPingTimeout" ControlToValidate="txtPingTimeout" 
                                runat="server" ErrorMessage='<%$ Resources:Resource, TimeoutRequiredErrorMessage %>'
                                ValidationGroup="SettingsValidation"></asp:RequiredFieldValidator>
                            <br />
                            <asp:RangeValidator ID="rangePingTimeout" runat="server" ControlToValidate="txtPingTimeout"
                                ValidationGroup="SettingsValidation" MinimumValue="1" MaximumValue="100" Type="Integer" ></asp:RangeValidator>
                        </td>
                    </tr>
                    </table>
                </div>
            </div>
        </div>
        <custom:StorageControl ID="SettingsStorage" runat="server" StorageType="Session" />
    
    
    <table style="min-width: 864px">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="ButtonStyle" style="float: left;">
                            <asp:LinkButton ID="btnStart" ValidationGroup="SettingsValidation" runat="server" OnClick="btnStart_Click" ForeColor="white" Width="100%" style="margin-top: 5px;" ><%=Resources.Resource.Start%></asp:LinkButton>
                        </div>
                        <div class="ButtonStyle" style="float: left;">
                            <asp:LinkButton ID="btnStop" runat="server" OnClick="btnStop_Click" ForeColor="white" Width="100%" style="margin-top: 5px;"><%=Resources.Resource.Stop%></asp:LinkButton>
                        </div>
                        <div class="ButtonStyle" style="float: left;">
                            <asp:LinkButton ID="btnPause" runat="server" OnClick="btnPause_Click" ForeColor="white" Width="100%" style="margin-top: 5px;"><%=Resources.Resource.Pause%></asp:LinkButton>                        
                            <asp:LinkButton ID="btnResume" runat="server" OnClick="btnResume_Click" ForeColor="white" Width="100%" style="margin-top: 5px;"><%=Resources.Resource.Resume%></asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:Label ID="lblTimer" runat="server" Width="60px"></asp:Label>
            </td>
            <td>
                <div>
                    <span class="progressBar" id="sProgressBar" style="width: 400px">0%</span>
                </div>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Always">
        <ContentTemplate>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="Get" TypeName="VirusBlokAda.RemoteOperations.RemoteScan.ScanResultDataSource"
                SelectCountMethod="Count" SortParameterName="sortExpression" EnablePaging="True"
                OnObjectCreating="ObjectDataSource1_ObjectCreating"></asp:ObjectDataSource>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1"
                EnableModelValidation="True" AllowPaging="True" AllowSorting="True" OnRowDataBound="GridView1_RowDataBound"
                CssClass="gridViewStyle" RowStyle-CssClass="gridViewRow" AlternatingRowStyle-CssClass="gridViewRowAlternating"
                HeaderStyle-CssClass="gridViewHeader" OnDataBound="GridView1_DataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle Width="60px" />
                        <HeaderTemplate>
                            <table width="100%">
                                <tr align="center">
                                    <td colspan="2"></td>
                                </tr>
                                <tr align="center">
                                    <td style="width: 20px;" align="right">
                                        <div id="divOptionsOpen" style="width: 16px; height: 16px; cursor: pointer;" title="<%=Resources.Resource.Options %>">
                                            <asp:Image runat="server" ID="imgOptions" AlternateText="Options" OnInit="imgOptions_Init" />
                                        </div>
                                    </td>                                 
                                    <td style="width: 40px;" align="left">
                                        <div id="divSelectionOptions" class="gridViewMenu" 
                                        onmouseover="$get('divSelectionOptions').className='gridViewMenuHover'" 
                                        onmouseout="$get('divSelectionOptions').className='gridViewMenu'" >
                                            <div id="divCbox" class="gridViewMenuCbox" >
                                                <asp:CheckBox ID="cBoxSelectAll" runat="server"/>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cboxIsSelected" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:Resource, Agent %>'>
                        <ItemTemplate>
                            <asp:Image ID="imgAgent" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:Resource, Loader %>'>                        
                        <ItemTemplate>
                            <asp:Image ID="imgLoader" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText='<%$ Resources:Resource, ComputerName %>'
                        SortExpression="Name" />
                    <asp:BoundField HeaderStyle-CssClass="gridViewHeader" DataField="IPAddress" HeaderText='<%$ Resources:Resource, IPAddress %>'
                        SortExpression="IPAddress" />
                    <asp:TemplateField HeaderText='<%$ Resources:Resource, Information %>'>                        
                        <ItemTemplate>
                            <div style="padding-left: 20px;">
                                <div style="word-wrap: break-word;float:left;">
                                    <asp:Label ID="lblInformation" runat="server" IP=<%# ((VirusBlokAda.RemoteOperations.RemoteScan.RemoteInfo.RemoteInfoEntityShow)Container.DataItem).IPAddress %> />    
                                </div>
                                <div id="imgComment" runat="server" style="float:right;" class="EditComment"
                                    titleDialog=<%# Resources.Resource.Comment %> title='<%# Resources.Resource.AddOrEditComment %>' saveTitle='<%# Resources.Resource.Save %>'
                                    IP=<%# ((VirusBlokAda.RemoteOperations.RemoteScan.RemoteInfo.RemoteInfoEntityShow)Container.DataItem).IPAddress %> >
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerSettings Visible="true" Position="TopAndBottom" />
                <PagerTemplate>
                    <paging:Paging runat="server" ID="Paging1" />
                </PagerTemplate>
            </asp:GridView>
            <custom:GridViewStorageControl ID="GridViewStorageControl1" runat="server" StorageType="Application"
                GridViewID="GridView1" />
            <custom:StorageControl ID="ScanStorage" runat="server" StorageType="Application" />
            <asp:HiddenField OnPreRender="hfSelectOptions_Prerender" ID="hfSelectOptions" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
    <%--Options menu--%>
    <div runat="server" id="divOptionsMenu" class="Menu">
        <div class="MenuItem" val="resetSorting">
            <%=Resources.Resource.DefaultSorting %>
        </div>
    </div>
    <%--Options menu end--%>
    <%--Selection options menu--%>
    <div id="selectionOptionsMenu" class="Menu">
        <div class="MenuItem" val="sortAsc">
            <%=Resources.Resource.Sort %> <%=String.Format("\u2193") %>
        </div>
        <div class="MenuItem" val="sortDesc">
            <%=Resources.Resource.Sort %> <%=String.Format("\u2191")%>
        </div>
        <div class="MenuItem" val="selectAllPage">
            <%=Resources.Resource.SelectAllOnPage %>
        </div>
        <div class="MenuItem" val="unselectAllPage">
            <%=Resources.Resource.UnselectAllOnPage %>
        </div>
        <div class="MenuItem" val="selectAll">
            <%=Resources.Resource.SelectAll %>
        </div>
        <div class="MenuItem" val="unselectAll">
            <%=Resources.Resource.UnselectAll %>
        </div>
    </div>
    <%--Selection options menu end--%>
    <asp:Timer ID="Timer1" runat="server" Interval="10000" Enabled="False" OnTick="Timer1_Tick">
    </asp:Timer>
    
    
    <div id="divInstall" style="margin: 5px; float: left">
        <table>        
            <tr>
                <td>
                    <asp:Label ID="lblSelectedTotalCountText" runat="server" Text="Totaly Selected: "></asp:Label>
                    <asp:Label ID="lblSelectedTotalCount" runat="server"></asp:Label>
                </td>                
                <td>
                    <div class="GiveButton1">
                        <asp:LinkButton ID="lbtnInstall" ValidationGroup="SettingsValidation" runat="server" OnClick="lbtnInstall_Click" ForeColor="white"
                            Width="100%" />
                    </div>
                </td>
                <td>
                    <asp:CheckBox ID="cbRebootAfterInstall" Checked="false" runat="server" Visible="false" />
                </td>
            </tr>
        </table>
    </div>

    <div id="divModalDialog" style="display: none;"></div>
        
</asp:Content>
