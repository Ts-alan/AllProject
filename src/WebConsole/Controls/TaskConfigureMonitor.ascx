<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureMonitor.ascx.cs" Inherits="Controls_TaskConfigureMonitor" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:100%"><%=Resources.Resource.CongLdrConfigureMonitor%></div>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $("#TabsMonitor").tabs({ cookie: { expires: 30} });

        $(document).on("click", '#<%= lbtnMonitorFileExtensionReset.ClientID %>', function () {
            $('#<%= tboxMonitorFileExtensions.ClientID %>').val('<%=ARM2_dbcontrol.Tasks.TaskConfigureMonitor.DefaultFilters %>');
        });

        $(document).on("click", '#<%= lbtnPathAdd.ClientID %>', function () {
            var disabled = $('#<%= lbtnPathAdd.ClientID %>').attr("disabled");

            if (disabled == "disabled") {
                return;
            }
            var dOpt = {
                width: 350,
                resizable: false,
                close: function (event, ui) {
                    btnAddExcludedDialogSetDefaultValues();
                    $('#divOverlay').css('display', 'none');
                },
                buttons: {
                    '<%=Resources.Resource.Apply%>': function () {
                        var btn = '<%=lbtnAddExcludedDialogApply.UniqueID %>';
                        if (Page_ClientValidate('AddExcludedDialogValidationGroup')) {
                            __doPostBack(btn, '');
                            $('#AddExcludedDialog').dialog('close');
                        }
                    },
                    '<%=Resources.Resource.CancelButtonText%>': function () {
                        $('#AddExcludedDialog').dialog('close');
                    }
                }
            };
            $('#AddExcludedDialog').dialog(dOpt);
            $('#divOverlay').css('display', 'inline');
            $('#AddExcludedDialog').parent().appendTo(jQuery("form:first"));
        });

        function btnAddExcludedDialogSetDefaultValues() {
            $('#<%=tboxAddExcludedDialogPath.ClientID %>').val('');
        }

        $(document).on("click", '#<%= lbtnPathChange.ClientID %>', function () {

            var disabled = $('#<%= lbtnPathChange.ClientID %>').attr("disabled");
            if (disabled == "disabled") {
                return;
            }

            var lbox = '#<%=lboxExcludedPath.ClientID %>';
            $('#' + '<%=tboxAddExcludedDialogPath.ClientID %>').val($(lbox + " option:selected").text());

            var dOpt = {
                width: 350,
                resizable: false,
                close: function (event, ui) {
                    $('#divOverlay').css('display', 'none');
                    btnAddExcludedDialogSetDefaultValues();
                },
                buttons: {
                    '<%=Resources.Resource.Apply%>': function () {
                        var btn = '<%=lbtnPathChangeHidden.UniqueID %>';
                        if (Page_ClientValidate('AddExcludedDialogValidationGroup')) {
                            __doPostBack(btn, '');
                            $('#AddExcludedDialog').dialog('close');
                        }
                    },
                    '<%=Resources.Resource.CancelButtonText%>': function () {
                        $('#AddExcludedDialog').dialog('close');
                    }
                }
            };
            $('#AddExcludedDialog').dialog(dOpt);
            $('#divOverlay').css('display', 'inline');
            $('#AddExcludedDialog').parent().appendTo(jQuery("form:first"));
        });
        $(document).on("click", '#<%= lbtnPathDelete.ClientID %>', function () {

            var disabled = $('#<%= lbtnPathDelete.ClientID %>').attr("disabled");
            if (disabled == "disabled") {
                return;
            }
        });
    });
</script>
<div id="TabsMonitor" style="width:550px">
    <ul>
        <li><a href="#tabMonitor1"><%=Resources.Resource.CongMonitorObjects%></a> </li>
        <li><a href="#tabMonitor2"><%=Resources.Resource.JournalEvents%></a> </li>
    </ul>
    <div id="tabMonitor1" class="divSettings">
        <div class="ListContrastTable">
            <div style="margin-left:10px; margin-top:5px">
                <asp:CheckBox ID="cboxMonitorOn" runat="server" /><%=Resources.Resource.MonitorEnabled %>
            </div>
            <div style="margin-left:10px;margin-top:10px;">
                <p style="font-weight:bold"><%= Resources.Resource.CongMonitorScanSelectedTypes %></p>
                <p>
                    <asp:TextBox runat="server" ID="tboxMonitorFileExtensions" style="width:350px;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="MonitorFileExtensionsTextBoxValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
                        ControlToValidate="tboxMonitorFileExtensions" Display="None" ValidationGroup="MonitorFileExtensionsValidationGroup" >
                    </asp:RequiredFieldValidator>
                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutMonitorFileExtensionsTextBox" runat="server"
                        TargetControlID="MonitorFileExtensionsTextBoxValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" >
                    </ajaxToolkit:ValidatorCalloutExtender2>
                                
                    <asp:LinkButton runat="server" ID="lbtnMonitorFileExtensionReset" SkinID="Button" Width="120" OnClientClick="return false;"><%= Resources.Resource.DefaultFiles %></asp:LinkButton>
                </p>
                <b > <%=Resources.Resource.CongMonitorExcluding%>  </b>
                <asp:TextBox runat="server" ID="tboxMonitorFilesExcluded" style="width:420px"></asp:TextBox>
                <asp:RegularExpressionValidator id="MonitorFilesExcludedRegularExpressionValidator" ControlToValidate="tboxMonitorFilesExcluded" ValidationExpression="^(\.[\w|\?|\*]+)*$" ErrorMessage="<%$Resources:Resource, WrongExtensionValidator %>"  runat="server">
                </asp:RegularExpressionValidator>
                <ajaxToolkit:ValidatorCalloutExtender2 ID="MonitorFilesExcludedRegularExpressionValidatorCalloutExtender" runat="server"
                        TargetControlID="MonitorFilesExcludedRegularExpressionValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" >
                </ajaxToolkit:ValidatorCalloutExtender2>
            </div>
            <div style="margin:10px">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">                    
                        <ContentTemplate>
                            <table >
                                <th colspan="2" >
                                    <b><%=Resources.Resource.CongMonitorExcludingFoldersAndFiles %></b>
                                </th>
                                    <tr>
                                        <td>
                                            <asp:ListBox runat="server" ID="lboxExcludedPath" style="width: 350px;height: 160px;" class="ListContrastTable"></asp:ListBox>
                                        </td>
                                        <td>
                                            <table>
                                                <tr><td>
                                                    <asp:LinkButton ID="lbtnPathAdd" runat="server" SkinID="Button" Width="120" OnClientClick="return false;"><%=Resources.Resource.Add %></asp:LinkButton>
                                                </td></tr>
                                                <tr><td>
                                                    <asp:LinkButton ID="lbtnPathDelete" runat="server" SkinID="Button" Width="120" OnClick="lbtnPathDelete_Click"><%=Resources.Resource.Delete %></asp:LinkButton>
                                                </td></tr>
                                                <tr><td>
                                                    <asp:LinkButton ID="lbtnPathChange" runat="server" SkinID="Button" Width="120" OnClientClick="return false;" ><%=Resources.Resource.Change %></asp:LinkButton>
                                                </td></tr>
                                                <tr><td>
                                                    <asp:LinkButton ID="lbtnAddExcludedDialogApply" runat='server' Style="display: none;" OnClick="lbtnAddExcludedDialogApply_Click" ></asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnPathChangeHidden" runat='server' Style="display: none;" OnClick="lbtnPathChangeHidden_Click" ></asp:LinkButton>
                                                </td></tr>
                                            </table>
                                        </td>
                                    </tr>                                    
                            </table>
                        </ContentTemplate>                                         
                    </asp:UpdatePanel>
            </div>
            <div style="padding-bottom:5px;padding-left:5px">
                <table style="padding:10px">
                    <tr>
                        <td >
                            <table rules="groups"  border="1px" width="245px">
                                <thead >
                                    <td  colspan="2">
                                         <b><%=Resources.Resource.InfectedFiles %></b>
                                    </td>                           
                                </thead>
                                <tbody >
                                    <tr style="height:25px">
                                        <td>
                                            <%= Resources.Resource.CongMonitorActionFirst%>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlInfectedActions1" style="width:120px;"> 
                                                <asp:ListItem Text="<%$ Resources:Resource, Block %>"></asp:ListItem>                                       
                                                <asp:ListItem Text="<%$ Resources:Resource, Cure %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Resource, Delete %>"></asp:ListItem>                                        
                                                <asp:ListItem Text="<%$ Resources:Resource, No %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:25px">
                                        <td>
                                            <%= Resources.Resource.CongMonitorActionSecond%>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlInfectedActions2" style="width:120px;">  
                                                <asp:ListItem Text="<%$ Resources:Resource, Block %>"></asp:ListItem>                                      
                                                <asp:ListItem Text="<%$ Resources:Resource, Cure %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Resource, Delete %>"></asp:ListItem>                                       
                                                <asp:ListItem Text="<%$ Resources:Resource, No %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:25px">
                                        <td>
                                            <%= Resources.Resource.CongMonitorActionThird%>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlInfectedActions3" style="width:120px;">
                                                <asp:ListItem Text="<%$ Resources:Resource, Block %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Resource, Cure %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Resource, Delete %>"></asp:ListItem>       
                                                <asp:ListItem Text="<%$ Resources:Resource, No %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height:25px">
                                        <td colspan="2">
                                            <asp:CheckBox runat="server" ID="chkInfectedSaveCopy" />
                                            <%= Resources.Resource.CongScannerSaveCopyToQuarantine %>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td style="padding-left:10px">
                            <table  rules="groups" border="1px" width="245px">
                                <thead>
                                    <td colspan="2">
                                        <b><%= Resources.Resource.SuspiciousFiles %></b>
                                    </td>
                                </thead>                        
                                <tr style="height:25px">
                                    <td >
                                        <%= Resources.Resource.CongMonitorActionFirst%>
                                    </td>
                                    <td >
                                        <asp:DropDownList runat="server" ID="ddlSuspiciousActions1" style="width:120px;">
                                            <asp:ListItem Text="<%$ Resources:Resource, Block %>"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Resource, Delete %>"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Resource, No %>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="height:25px">
                                    <td >
                                        <%= Resources.Resource.CongMonitorActionSecond%>
                                    </td>
                                    <td >
                                        <asp:DropDownList runat="server" ID="ddlSuspiciousActions2"  style="width:120px;">
                                            <asp:ListItem Text="<%$ Resources:Resource, Block %>"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Resource, Delete %>"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Resource, No %>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="height:25px">
                                    <td colspan="2">&nbsp</td>
                                </tr>
                                <tr style="height:25px">
                                    <td colspan="2">
                                        <asp:CheckBox runat="server" ID="chkSuspiciousSaveCopy" />
                                        <%= Resources.Resource.CongScannerSaveCopyToQuarantine %>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        
    </div>

    <div id="tabMonitor2" class="divSettings">
        <asp:UpdatePanel ID="JournalEventUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="JournalEventPanel" runat='server' EnableViewState="false">
                        <asp:Table ID="JournalEventTable" runat="server" CssClass="ListContrastTable" rules="cols">
                            <asp:TableHeaderRow ID="TableHeaderRow1"  runat="server" CssClass="gridViewHeader">
                                <asp:TableHeaderCell runat="server" id="tdEvent" style="width: 150px;text-align: center;" >
                                    <asp:Label ID="Label1"  runat="server" ><%=Resources.Resource.Events %></asp:Label>                            
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdWindowsJournal" style="width: 120px;text-align: center;" >
                                    <asp:Label ID="Label2"  runat="server" ><%=Resources.Resource.WindowsJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdLocalJournal" style="width: 120px;text-align: center;" >
                                    <asp:Label ID="Label3"  runat="server" ><%=Resources.Resource.LocalJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdCCJournal" style="width: 120px;text-align: center;">
                                    <asp:Label ID="Label4" runat="server"  ><%=Resources.Resource.CCJournal %></asp:Label>
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
    </div>
    <div id="divOverlay" class="ui-widget-overlay ui-front" style="display: none" ></div>
</div>
<div id="AddExcludedDialog" style="display: none;padding-bottom: 20px;" class="ui-front">
    <%=Resources.Resource.Path %><br />
    <asp:TextBox ID="tboxAddExcludedDialogPath" Style="width: 300px" runat='server'></asp:TextBox>
    <asp:RequiredFieldValidator ID="AddExcludedDialogValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
        ControlToValidate="tboxAddExcludedDialogPath" Display="None" ValidationGroup="AddExcludedDialogValidationGroup" >
    </asp:RequiredFieldValidator>
    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutAddExcludedDialogPath" runat="server"
        TargetControlID="AddExcludedDialogValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" >
    </ajaxToolkit:ValidatorCalloutExtender2>
</div>
