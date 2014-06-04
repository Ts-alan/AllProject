<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureMonitor.ascx.cs" Inherits="Controls_TaskConfigureMonitor" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:100%"><%=Resources.Resource.CongLdrConfigureMonitor%></div>

<script language="javascript" type="text/javascript">
    function pageLoad() {
        var defExts = ".COM.EXE.DLL.DRV.SYS.OV?.VXD.SCR.CPL.OCX.BPL.AX.PIF.DO?.XL?.HLP.RTF.WI?.WZ?.MSI.MSC.HT*.VB*.JS.JSE.ASP*.CGI.PHP*.?HTML.BAT.CMD.EML.NWS.MSG.XML.MSO.WPS.PPT.PUB.JPG.JPEG.ANI.INF.SWF.PDF";
        $("#TabsMonitor").tabs({ cookie: { expires: 30} });
        $("input[class='control']").button();
        $("input[type='button']").button();
        if($('#<%= MonitorFileExtensionsTextBox.ClientID %>').val()=='')
            $('#<%= MonitorFileExtensionsTextBox.ClientID %>').val(defExts);

        $('#<%= MonitorFileExtensionResetButton.ClientID %>').on("click", function () {
            $('#<%= MonitorFileExtensionsTextBox.ClientID %>').val(defExts);
        });


        $('#<%= btnPathAdd.ClientID %>').on("click", function () {
                var dOpt = {
                    width: 350,                                       
                    resizable: false,
                    close: function(event, ui)
                        {
                            btnAddExcludedDialogSetDefaultValues() ; 
                            $('#divOverlay').css('display','none');
                        },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () {
                            var btn = '<%=AddExcludedDialogApplyButton.UniqueID %>';
                            if( Page_ClientValidate('AddExcludedDialogValidationGroup'))
                            {    
                                __doPostBack(btn, '');                                                              
                                $('#AddExcludedDialog').dialog('close');                                
                            }                            
                        },
                       <%=Resources.Resource.CancelButtonText%>: function () {                           
                            $('#AddExcludedDialog').dialog('close');                            
                        }
                    }
                };
                $('#AddExcludedDialog').dialog(dOpt);
                $('#divOverlay').css('display','inline');
                $('#AddExcludedDialog').parent().appendTo(jQuery("form:first"));
             });
                $("[pathrowselected]").hover(function(){
                    if($(this).attr('pathrowselected')=="true") return;
                    $(this).css('background-color', 'yellow');
                },
                function () { 
                    if($(this).attr('pathrowselected')=="true") return;           
                    $(this).css('background-color', '');
                }
            );
            $("[pathrowselected]").on("click",function(){
                if($(this).attr("pathrowselected")=="true") return;
                $(this).css('background-color', '#3399ff');
                $("[pathrowselected=true]").css('background-color', '');
                $("[pathrowselected=true]").attr('pathrowselected',false);
                $(this).attr('pathrowselected',true);                
                var rowNo=$(this).children().find('[value]').attr('value');                
                 $('#<%=PathHdnActiveRowNo.ClientID %>').attr('value',rowNo);
             });
    };
    function btnAddExcludedDialogSetDefaultValues()
    {
       $('#<%=AddExcludedDialogPath.ClientID %>').val('');
    };
    function PathChangeButtonClientClick()
    {
        var index=$('#<%=PathHdnActiveRowNo.ClientID %>').attr('value');
        if(index==0)
            return;
        var tableRow=$("[id*='PathHdnRowNo'][value='"+index+"']").parent().parent();           
            PathChangeDialog(tableRow);            
    }
    function PathChangeDialog(tableRow)
    {
        var path=tableRow.children().find("[id*='CongMonExcludedPath']").text();
        $('#' + '<%=AddExcludedDialogPath.ClientID %>').val(path);                    
            var dOpt = {
            width: 350,
            resizable: false,
            close: function(event, ui)
            {
                $('#divOverlay').css('display','none');
                btnAddExcludedDialogSetDefaultValues();                            
            },
            buttons: {
                <%=Resources.Resource.Apply%>: function () {
                    var btn = '<%=PathChangeButton.UniqueID %>';
                    if( Page_ClientValidate('AddExcludedDialogValidationGroup'))
                    {    
                        __doPostBack(btn, '');                                                              
                        $('#AddExcludedDialog').dialog('close');                                
                    }     
                },
                <%=Resources.Resource.CancelButtonText%>: function () {                            
                    $('#AddExcludedDialog').dialog('close');
                }
            }
        };
        $('#AddExcludedDialog').dialog(dOpt);
        $('#divOverlay').css('display','inline');
        $('#AddExcludedDialog').parent().appendTo(jQuery("form:first"));
    } 



</script>
<div id="TabsMonitor" style="width:550px">
    <ul>
        <li><a href="#tab1"><%=Resources.Resource.CongMonitorObjects%></a> </li>
        <li><a href="#tab2"><%=Resources.Resource.JournalEvents%></a> </li>
    </ul>
    <div id="tab1" class="divSettings">
        <table class="ListContrastTable" width="540px" rules="all">
            <tr >
                <td colspan="2">
                    <asp:CheckBox ID="cboxMonitorOn" runat="server" /><%=Resources.Resource.MonitorEnabled %>
                </td>
            </tr>
            <tr>
                <td colspan="2" >
                    <table width="530px" rules="groups">
                        <thead>
                            <th align="center" style="font-weight:bold"><%= Resources.Resource.CongMonitorScanSelectedTypes %></th>
                        </thead>                        
                        <tr>
                            <td>
                                &nbsp;
                                <asp:RequiredFieldValidator ID="MonitorFileExtensionsTextBoxValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
                                    ControlToValidate="MonitorFileExtensionsTextBox" Display="None" ValidationGroup="MonitorFileExtensionsValidationGroup" />
                                 <asp:RegularExpressionValidator id="MonitorFileExtensionsRegularExpressionValidator" ControlToValidate="MonitorFileExtensionsTextBox" ValidationExpression="^(\.[\w|\?|\*]+)*$" ErrorMessage="<%$Resources:Resource, WrongExtensionValidator %>"  runat="server"/>
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutMonitorFileExtensionsTextBox" runat="server"
                                    TargetControlID="MonitorFileExtensionsTextBoxValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="RegularValidatorCalloutMonitorFileExtensionsTextBox" runat="server"
                                    TargetControlID="MonitorFileExtensionsRegularExpressionValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
                                <asp:TextBox runat="server" ID="MonitorFileExtensionsTextBox" style="width:400px"></asp:TextBox>
                                <asp:Button runat="server" Text="<%$ Resources:Resource, DefaultFiles %>" ID="MonitorFileExtensionResetButton" Width="120px" OnClientClick="return false;"/>
                              <%--  <input class='control' type="button" runat="server" id="MonitorFileExtensionResetButton" value="<%$ Resources:Resource, DefaultFiles %>" size="100px"/>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp; &nbsp; &nbsp; &nbsp;
                                <asp:Label  runat="server" style="width:100px;"><%=Resources.Resource.CongMonitorExcluding%></asp:Label>
                                &nbsp;&nbsp;
                               <asp:RegularExpressionValidator id="MonitorFilesExcludedRegularExpressionValidator" ControlToValidate="MonitorFilesExcludedTextBox" ValidationExpression="^(\.[\w|\?|\*]+)*$" ErrorMessage="<%$Resources:Resource, WrongExtensionValidator %>"  runat="server"/>
                               <ajaxToolkit:ValidatorCalloutExtender2 ID="MonitorFilesExcludedRegularExpressionValidatorCalloutExtender" runat="server"
                                    TargetControlID="MonitorFilesExcludedRegularExpressionValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
                                <asp:TextBox runat="server" ID="MonitorFilesExcludedTextBox" style="width:400px"></asp:TextBox>
                            </td>
                        </tr>                        
                    </table>
                </td>
            </tr>
            <tr>                 
                <td colspan="2">
                    <asp:UpdatePanel runat="server">                    
                        <ContentTemplate>
                            <asp:HiddenField ID="PathHdnActiveRowNo" Value='0' runat="server" />
                            <asp:Table runat="server" Style="width: 500px" GridLines="Horizontal">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell ColumnSpan="2">
                                    <asp:Label runat="server" Text="<%$ Resources:Resource, CongMonitorExcludingFoldersAndFiles %>" ></asp:Label>
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="3">
                                        <asp:Panel runat="server" Width="400px" Height="160px" Style="overflow: scroll">
                                            <asp:DataList ID="ExcludedPathDataList" runat="server" Style="table-layout: fixed; word-break: break-all;" rules="all"
                                                OnItemDataBound="ExcludedPathDataList_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr runat="server" id="trPathItem" pathrowselected="false">
                                                        <td runat="server">
                                                            <asp:HiddenField ID="PathHdnRowNo" Value="0" runat="server" />
                                                            <asp:Label runat="server" ID="CongMonExcludedPath" Width="380px"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </asp:Panel>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Table runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell Height="50px" HorizontalAlign="Center">
                                                    <asp:Button ID="btnPathAdd" runat="server" Text="<%$ Resources:Resource, Add %>" Width="120px" OnClientClick="return false;"/>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell Height="50px" HorizontalAlign="Center">
                                                    <asp:Button ID="btnPathDelete" runat="server" Text="<%$ Resources:Resource, Delete %>" Width="120px"  OnClick="PathDeleteButtonClick"   />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell Height="50px" HorizontalAlign="Center"  >
                                                    <asp:Button ID="btnPathChange" runat="server" Text="<%$ Resources:Resource, Change %>" Width="120px" OnClientClick="PathChangeButtonClientClick()" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                    <asp:Button ID="AddExcludedDialogApplyButton" runat='server' Style="display: none" OnClick="AddExcludedDialogApplyButtonClick" />
                    <asp:Button ID="PathChangeButton" runat='server' Style="display: none" OnClick="PathChangeButtonClick" />
                        
                        </ContentTemplate>                                         
                    </asp:UpdatePanel>
               </td>
            </tr>
            
            <tr >
                <td style="width:250px">
                    <table rules="groups" >
                        <thead >
                            <td style="font-weight:bold">
                                 <asp:Label   runat="server" ><%=Resources.Resource.InfectedFiles %></asp:Label> 
                            </td>                           
                        </thead>
                        <tbody>
                            <tr style="height:25px">
                                <td>
                                    <asp:Label   runat="server"><%= Resources.Resource.CongMonitorActionFirst%></asp:Label>
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
                                    <asp:Label   runat="server"><%= Resources.Resource.CongMonitorActionSecond%></asp:Label>
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
                                    <asp:Label runat="server"><%= Resources.Resource.CongMonitorActionThird%></asp:Label>
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
                                    <asp:Label runat="server"><%= Resources.Resource.CongScannerSaveCopyToQuarantine %></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td style="width:250px">
                    <table  rules="groups" >
                        <thead>
                            <td style="font-weight:bold">
                                <asp:Label  runat="server" ><%= Resources.Resource.SuspiciousFiles %></asp:Label>
                            </td>
                        </thead>                        
                        <tr style="height:25px">
                            <td >
                                <asp:Label runat="server"><%= Resources.Resource.CongMonitorActionFirst%></asp:Label>
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
                                <asp:Label runat="server"><%= Resources.Resource.CongMonitorActionSecond%></asp:Label>
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
                                <asp:Label runat="server"><%= Resources.Resource.CongScannerSaveCopyToQuarantine %></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <div id="tab2" class="divSettings">
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
    <div id="divOverlay" class="ui-widget-overlay ui-front" style="display: none" />
</div>
<div id="AddExcludedDialog" style="display: none;" class="ui-front">
    <br />
    <asp:Label  runat="server"><%=Resources.Resource.Path %></asp:Label><br />
    <asp:TextBox ID="AddExcludedDialogPath" Style="width: 300px" runat='server'></asp:TextBox>
    <asp:RequiredFieldValidator ID="AddExcludedDialogValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
        ControlToValidate="AddExcludedDialogPath" Display="None" ValidationGroup="AddExcludedDialogValidationGroup" />

    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutAddExcludedDialogPath" runat="server"
        TargetControlID="AddExcludedDialogValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />          
      
</div>
