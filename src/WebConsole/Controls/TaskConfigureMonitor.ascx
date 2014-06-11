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
            <div>
                <asp:CheckBox ID="cboxMonitorOn" runat="server" /><%=Resources.Resource.MonitorEnabled %>
            </div>
            <div>
                <%= Resources.Resource.CongMonitorScanSelectedTypes %>
            </div>
            <div>
                <asp:TextBox runat="server" ID="tboxMonitorFileExtensions" style="width:350px;"></asp:TextBox>
                <asp:RequiredFieldValidator ID="MonitorFileExtensionsTextBoxValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
                    ControlToValidate="tboxMonitorFileExtensions" Display="None" ValidationGroup="MonitorFileExtensionsValidationGroup" >
                </asp:RequiredFieldValidator>
                <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutMonitorFileExtensionsTextBox" runat="server"
                    TargetControlID="MonitorFileExtensionsTextBoxValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" >
                </ajaxToolkit:ValidatorCalloutExtender2>
                                
                <asp:LinkButton runat="server" ID="lbtnMonitorFileExtensionReset" SkinID="Button" OnClientClick="return false;"><%= Resources.Resource.DefaultFiles %></asp:LinkButton>
            </div>
            <div>
                <%=Resources.Resource.CongMonitorExcluding%>
                <asp:TextBox runat="server" ID="tboxMonitorFilesExcluded" style="width:400px"></asp:TextBox>
                <asp:RegularExpressionValidator id="MonitorFilesExcludedRegularExpressionValidator" ControlToValidate="tboxMonitorFilesExcluded" ValidationExpression="^(\.[\w|\?|\*]+)*$" ErrorMessage="<%$Resources:Resource, WrongExtensionValidator %>"  runat="server">
                </asp:RegularExpressionValidator>
                <ajaxToolkit:ValidatorCalloutExtender2 ID="MonitorFilesExcludedRegularExpressionValidatorCalloutExtender" runat="server"
                        TargetControlID="MonitorFilesExcludedRegularExpressionValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" >
                </ajaxToolkit:ValidatorCalloutExtender2>
            </div>
            <div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">                    
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <%=Resources.Resource.CongMonitorExcludingFoldersAndFiles %>
                                        <br />
                                        <asp:ListBox runat="server" ID="lboxExcludedPath" style="width: 350px;height: 160px;"></asp:ListBox>
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
            <div>
                <table>
                    <tr>
                        <td>
                            <table rules="groups" >
                                <thead >
                                    <td style="font-weight:bold">
                                         <asp:Label runat="server" ><%=Resources.Resource.InfectedFiles %></asp:Label> 
                                    </td>                           
                                </thead>
                                <tbody>
                                    <tr style="height:25px">
                                        <td>
                                            <asp:Label  runat="server"><%= Resources.Resource.CongMonitorActionFirst%></asp:Label>
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
                                            <asp:Label ID="Label7"   runat="server"><%= Resources.Resource.CongMonitorActionSecond%></asp:Label>
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
                                            <asp:Label ID="Label8" runat="server"><%= Resources.Resource.CongMonitorActionThird%></asp:Label>
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
                                            <asp:Label ID="Label9" runat="server"><%= Resources.Resource.CongScannerSaveCopyToQuarantine %></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td style="width:250px">
                            <table  rules="groups" >
                                <thead>
                                    <td style="font-weight:bold">
                                        <asp:Label ID="Label10"  runat="server" ><%= Resources.Resource.SuspiciousFiles %></asp:Label>
                                    </td>
                                </thead>                        
                                <tr style="height:25px">
                                    <td >
                                        <asp:Label ID="Label11" runat="server"><%= Resources.Resource.CongMonitorActionFirst%></asp:Label>
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
                                        <asp:Label ID="Label12" runat="server"><%= Resources.Resource.CongMonitorActionSecond%></asp:Label>
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
                                        <asp:Label ID="Label13" runat="server"><%= Resources.Resource.CongScannerSaveCopyToQuarantine %></asp:Label>
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
                        <asp:Table ID="JournalEventTable" runat="server" CssClass="ListContrastTable">
                            <asp:TableHeaderRow ID="TableHeaderRow1"  runat="server">
                                <asp:TableHeaderCell runat="server" id="tdEvent" style="width: 150px;text-align: center;" class="listRulesHeader">
                                    <asp:Label ID="Label1"  runat="server" ><%=Resources.Resource.Events %></asp:Label>                            
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdWindowsJournal" style="width: 120px;text-align: center;" class="listRulesHeader">
                                    <asp:Label ID="Label2"  runat="server" ><%=Resources.Resource.WindowsJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdLocalJournal" style="width: 120px;text-align: center;" class="listRulesHeader">
                                    <asp:Label ID="Label3"  runat="server" ><%=Resources.Resource.LocalJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdCCJournal" style="width: 120px;text-align: center;" class="listRulesHeader">
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
