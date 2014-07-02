<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureIntegrityCheck.ascx.cs"
    Inherits="Controls_TaskConfigureIntegrityCheck" %>
<div class="tasksection" runat="server" id="HeaderName" style="width: 560px">
    <%=Resources.Resource.TaskNameIntegrityCheck%></div>
<div class="divSettings">
    <script type="text/javascript">
        function pageLoad() 
        {
            $("#VICTabs").tabs({ cookie: { expires: 30} });
            $("input[class='control']").button();

             $('#<%= btnFilesAdd.ClientID %>').on("click", function () {
                var dOpt = {
                    width: 350,                                       
                    resizable: false,
                    close: function(event, ui)
                        {
                            btnFilesAddDialogSetDefaultValues() ; 
                            $('#divOverlay').css('display','none');
                        },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () {
                            var btn = '<%=FilesAddDialogApplyButton.UniqueID %>';
                            if( Page_ClientValidate('FilesAddValidationGroup'))
                            {    
                                __doPostBack(btn, '');                                                              
                                $('#FilesAddDialog').dialog('close');                                
                            }                            
                        },
                       <%=Resources.Resource.CancelButtonText%>: function () {                           
                            $('#FilesAddDialog').dialog('close');                            
                        }
                    }
                };
                $('#FilesAddDialog').dialog(dOpt);
                $('#divOverlay').css('display','inline');
                $('#FilesAddDialog').parent().appendTo(jQuery("form:first"));
             });
            $("[filesRowSelected]").hover(function(){
                    if($(this).attr('filesRowSelected')=="true") return;
                    $(this).css('background-color', 'yellow');
                },
                function () { 
                    if($(this).attr('filesRowSelected')=="true") return;           
                    $(this).css('background-color', '');
                }
            );
            $("[filesRowSelected]").on("click",function(){
                if($(this).attr("filesRowSelected")=="true") return;
                $(this).css('background-color', '#3399ff');
                $("[filesRowSelected=true]").css('background-color', '');
                $("[filesRowSelected=true]").attr('filesRowSelected',false);
                $(this).attr('filesRowSelected',true);                
                var rowNo=$(this).children().find('[value]').attr('value');                
                 $('#<%=FileshdnActiveRowNo.ClientID %>').attr('value',rowNo);
             });

             $('#<%= btnRegistryAdd.ClientID %>').on("click", function () {
                var dOpt = {
                    width: 350,                                       
                    resizable: false,
                    close: function(event, ui)
                        {
                            btnRegistryAddDialogSetDefaultValues() ; 
                            $('#divOverlay').css('display','none');
                        },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () { 
                            var btn = '<%=RegistryAddDialogApplyButton.UniqueID %>';
                            if( Page_ClientValidate('RegistryAddValidationGroup'))
                            {    
                                __doPostBack(btn, '');                                                              
                                $('#RegistryAddDialog').dialog('close');                                
                            }                            
                        },
                       <%=Resources.Resource.CancelButtonText%>: function () {                           
                            $('#RegistryAddDialog').dialog('close');                            
                        }
                    }
                };
                $('#RegistryAddDialog').dialog(dOpt);
                $('#divOverlay').css('display','inline');
                $('#RegistryAddDialog').parent().appendTo(jQuery("form:first"));
             });
            $("[RegistryRowSelected]").hover(function(){
                    if($(this).attr('RegistryRowSelected')=="true") return;
                    $(this).css('background-color', 'yellow');
                },
                function () { 
                    if($(this).attr('RegistryRowSelected')=="true") return;           
                    $(this).css('background-color', '');
                }
            );
            $("[RegistryRowSelected]").on("click",function(){
                if($(this).attr("RegistryRowSelected")=="true") return;
                $(this).css('background-color', '#3399ff');
                $("[RegistryRowSelected=true]").css('background-color', '');
                $("[RegistryRowSelected=true]").attr('RegistryRowSelected',false);
                $(this).attr('RegistryRowSelected',true);                
                var rowNo=$(this).children().find('[value]').attr('value');                
                 $('#<%=RegistryhdnActiveRowNo.ClientID %>').attr('value',rowNo);
             });               
        };
        function btnFilesAddDialogSetDefaultValues()
        {            
            $('#' + '<%=FilesAddDialogPath.ClientID %>').val('');
            $('#' + '<%=FilesAddDialogTemplate.ClientID %>').val('');
        };        
        function FilesChangeButtonClientClick()
        {
            var index=$('#<%=FileshdnActiveRowNo.ClientID %>').attr('value');
            if(index==0)
                return;
            var tableRow=$("[id*='FileshdnRowNo'][value='"+index+"']").parent().parent();           
                FilesChangeDialog(tableRow);            
        }
        function FilesChangeDialog(tableRow)
        {
            var path=tableRow.children().find("[id*='lblFilesPath']").text();
            $('#' + '<%=FilesAddDialogPath.ClientID %>').val(path);
            var template=tableRow.children().find("[id*='lblFilesTemplate']").text();
            $('#' + '<%=FilesAddDialogTemplate.ClientID %>').val(template);                       
             var dOpt = {
                width: 350,
                resizable: false,
                close: function(event, ui)
                {
                    $('#divOverlay').css('display','none');
                    btnFilesAddDialogSetDefaultValues();                            
                },
                buttons: {
                    <%=Resources.Resource.Apply%>: function () {
                        var btn = '<%=FilesChangeButton.UniqueID %>';
                        if( Page_ClientValidate('FilesAddValidation'))
                        {
                            __doPostBack(btn, '');
                            $('#FilesAddDialog').dialog('close');
                        }
                    },
                    <%=Resources.Resource.CancelButtonText%>: function () {                            
                        $('#FilesAddDialog').dialog('close');
                    }
                }
            };
            $('#FilesAddDialog').dialog(dOpt);
            $('#divOverlay').css('display','inline');
            $('#FilesAddDialog').parent().appendTo(jQuery("form:first"));
            } 
            function btnRegistryAddDialogSetDefaultValues()
        {            
            $('#' + '<%=RegistryAddDialogPath.ClientID %>').val('');
        };        
        function RegistryChangeButtonClientClick()
        {
            var index=$('#<%=RegistryhdnActiveRowNo.ClientID %>').attr('value');
            if(index==0)
                return;
            var tableRow=$("[id*='RegistryhdnRowNo'][value='"+index+"']").parent().parent();           
                RegistryChangeDialog(tableRow);            
        }
        function RegistryChangeDialog(tableRow)
        {
            var path=tableRow.children().find("[id*='lblRegistryPath']").text();
            $('#' + '<%=RegistryAddDialogPath.ClientID %>').val(path);                    
             var dOpt = {
                width: 350,
                resizable: false,
                close: function(event, ui)
                {
                    $('#divOverlay').css('display','none');
                    btnRegistryAddDialogSetDefaultValues();                            
                },
                buttons: {
                    <%=Resources.Resource.Apply%>: function () {
                        var btn = '<%=RegistryChangeButton.UniqueID %>';
                        if( Page_ClientValidate('RegistryAddValidation'))
                        {
                            __doPostBack(btn, '');
                            $('#RegistryAddDialog').dialog('close');
                        }
                    },
                    <%=Resources.Resource.CancelButtonText%>: function () {                            
                        $('#RegistryAddDialog').dialog('close');
                    }
                }
            };
        $('#RegistryAddDialog').dialog(dOpt);
        $('#divOverlay').css('display','inline');
        $('#RegistryAddDialog').parent().appendTo(jQuery("form:first"));
    }      
    </script>
    <div id="VICTabs">
        <ul>
            <li><a href="#tabFiles">
                <%=Resources.Resource.Files %></a></li>
            <li><a href="#tabRegistry">
                <%=Resources.Resource.Registry %></a></li>
            <%--  <li><a href="#tabDevices"><%=Resources.Resource.Devices %></a></li>--%>
            <li><a href="#tabJournalVIC"><%=Resources.Resource.JournalEvents %></a> </li>
        </ul>
        <div id='tabFiles'>
            <asp:UpdatePanel ID="FilesUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="FileshdnActiveRowNo" Value='0' runat="server" />
                    <asp:Table class="ListContrastTable" ID="tblFilesUpdatePanel" Style="width: 500px" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3">
                                <asp:Panel runat="server" Width="370px" Height="300px" ID="FilesPanel" Style="overflow: scroll">
                                    <asp:DataList runat="server" ID="FilesDataList" OnItemDataBound="FilesDataList_ItemDataBound"
                                        OnItemCommand="FilesDataList_SelectedIndexChanged" Style="table-layout: fixed;
                                        word-break: break-all;" rules="all">
                                        <HeaderTemplate>
                                            <tr>
                                                <th runat="server" id="tdFilesPath" style="width: 230px; text-align: center;" class="listRulesHeader">
                                                    <asp:Label runat="server" Text="<%$ Resources:Resource, Path %>"></asp:Label>
                                                </th>
                                                <th runat="server" id="tdFilesTemplate" style="width: 120px; text-align: center;"
                                                    class="listRulesHeader">
                                                    <asp:Label runat="server" Text="<%$ Resources:Resource, Template %>"></asp:Label>
                                                </th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr runat="server" id="trFilesItem" filesrowselected="false">
                                                <td runat="server" id="tdFilesPath" style="width: 200px;">
                                                    <asp:HiddenField ID="FileshdnRowNo" Value="0" runat="server" />
                                                    <asp:Label runat="server" ID="lblFilesPath" />
                                                </td>
                                                <td runat="server" id="tdFilesTemplate" style="width: 150px;">
                                                    <asp:Label runat="server" ID="lblFilesTemplate" />
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
                                            <asp:Button ID="btnFilesAdd" runat="server" Text="<%$ Resources:Resource, Add %>" Width="100" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell Height="50px" HorizontalAlign="Center">
                                            <asp:Button ID="btnFilesDelete" runat="server" Text="<%$ Resources:Resource, Delete %>" Width="100"
                                                OnClick="FilesDeleteButtonClick" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell Height="50px" HorizontalAlign="Center">
                                            <asp:Button ID="btnFilesChange" runat="server" Text="<%$ Resources:Resource, Change %>" Width="100"
                                                OnClientClick="FilesChangeButtonClientClick()" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:Button ID="FilesAddDialogApplyButton" runat='server' Style="display: none" OnClick="FilesAddDialogApplyButtonClick" />
                    <asp:Button ID="FilesChangeButton" runat='server' Style="display: none" OnClick="FilesChangeButtonClick" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="FilesAddDialog" style="display: none;" class="ui-front">
            <br />
            <asp:Label runat="server"><%=Resources.Resource.Path %></asp:Label><br />
            <asp:TextBox ID="FilesAddDialogPath" Style="width: 300px" runat='server'></asp:TextBox>
            <asp:RequiredFieldValidator ID="FilesAddDialogValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
                ControlToValidate="FilesAddDialogPath" Display="None" ValidationGroup="FilesAddValidationGroup" />
            <%--<asp:RegularExpressionValidator id="FilesAddRegularExpressionValidator" ControlToValidate="FilesAddDialogPath" ValidationExpression="^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.]+)+\.([a-z])$" ErrorMessage="ZIP code must be 5 numeric digits"  runat="server"/>
            --%>
            <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutFilesAddDialogPath" runat="server"
                TargetControlID="FilesAddDialogValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
            <%-- <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutFilesAddRegularExpressionPath" runat="server"
                TargetControlID="FilesAddRegularExpressionValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
            --%>
            <br />
            <br />
            <asp:Label runat="server"><%=Resources.Resource.Template %></asp:Label><br />
            <asp:TextBox ID="FilesAddDialogTemplate" Style="width: 300px" runat='server'></asp:TextBox>
         <%--   <asp:RequiredFieldValidator ID="FilesAddDialogTemplateValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
                ControlToValidate="FilesAddDialogTemplate" Display="None" ValidationGroup="FilesAddValidationGroup" />
            <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutFilesAddDialogTemplate"
                runat="server" TargetControlID="FilesAddDialogTemplateValidator" HighlightCssClass="highlight"
                PopupPosition="BottomRight" />--%>
            <br />
            <br />
            <br />
        </div>
        <div id='tabRegistry'>
            <asp:UpdatePanel ID="RegistryUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="RegistryhdnActiveRowNo" Value='0' runat="server" />
                    <asp:Table class="ListContrastTable" ID="tblRegistryUpdatePanel" Style="width: 500px" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3">
                                <asp:Panel runat="server" Width="370px" Height="300px" ID="RegistryPanel" Style="overflow: scroll">
                                    <asp:DataList runat="server" ID="RegistryDataList" OnItemDataBound="RegistryDataList_ItemDataBound"
                                        OnItemCommand="RegistryDataList_SelectedIndexChanged" Style="table-layout: fixed;
                                        word-break: break-all;" rules="all">
                                        <HeaderTemplate>
                                            <tr>
                                                <th runat="server" id="tdRegistryPath" style="width: 350px; text-align: center;"
                                                    class="listRulesHeader">
                                                    <asp:Label runat="server" Text="<%$ Resources:Resource, Path %>"></asp:Label>
                                                </th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr runat="server" id="trRegistryItem" registryrowselected="false">
                                                <td runat="server" id="tdRegistryPath" style="width: 350px;">
                                                    <asp:HiddenField ID="RegistryhdnRowNo" Value="0" runat="server" />
                                                    <asp:Label runat="server" ID="lblRegistryPath" />
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
                                            <asp:Button ID="btnRegistryAdd" runat="server" Text="<%$ Resources:Resource, Add %>" Width="100" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell Height="50px" HorizontalAlign="Center">
                                            <asp:Button ID="btnRegistryDelete" runat="server" Text="<%$ Resources:Resource, Delete %>" Width="100"
                                                OnClick="RegistryDeleteButtonClick" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell Height="50px" HorizontalAlign="Center">
                                            <asp:Button ID="btnRegistryChange" runat="server" Text="<%$ Resources:Resource, Change %>" Width="100"
                                                OnClientClick="RegistryChangeButtonClientClick()" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:Button ID="RegistryAddDialogApplyButton" runat='server' Style="display: none"
                        OnClick="RegistryAddDialogApplyButtonClick" />
                    <asp:Button ID="RegistryChangeButton" runat='server' Style="display: none" OnClick="RegistryChangeButtonClick" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="RegistryAddDialog" style="display: none;" class="ui-front">
            <br />
            <asp:Label runat="server"><%=Resources.Resource.Path %></asp:Label><br />
            <asp:TextBox ID="RegistryAddDialogPath" Style="width: 300px" runat='server'></asp:TextBox>
            <asp:RequiredFieldValidator ID="RegistryAddDialogValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
                ControlToValidate="RegistryAddDialogPath" Display="None" ValidationGroup="RegistryAddValidationGroup" />
            <%--<asp:RegularExpressionValidator id="FilesAddRegularExpressionValidator" ControlToValidate="FilesAddDialogPath" ValidationExpression="^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.]+)+\.([a-z])$" ErrorMessage="ZIP code must be 5 numeric digits"  runat="server"/>
            --%>
            <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutRegistryAddDialogPath"
                runat="server" TargetControlID="RegistryAddDialogValidator" HighlightCssClass="highlight"
                PopupPosition="BottomRight" />
            <%-- <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutFilesAddRegularExpressionPath" runat="server"
                TargetControlID="FilesAddRegularExpressionValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
            --%>
            <br />
            <br />
        </div>
        <%-- <div id='tabDevices' >
             <asp:UpdatePanel  runat="server">
                <ContentTemplate>
                    <asp:Table  class="ListContrastTable" Style="width: 500px" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3">
                                <asp:Panel runat="server" Width="400px" Height="300px" >                                   
                                </asp:Panel>
                            </asp:TableCell>                   
                        </asp:TableRow>
                        <asp:TableRow Height="50px">
                            <asp:TableCell HorizontalAlign='Center'>
                                <asp:Button ID="btnDevicesSaveState" runat='server' Text="<%$ Resources:Resource, SaveState %>" />
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign='Center'>
                                <asp:Button ID="btnDevicesCheckState" runat='server' Text="<%$ Resources:Resource, CheckState %>" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>--%>
        <div id='tabJournalVIC'>
            <asp:UpdatePanel ID="JournalEventUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="JournalEventPanel" runat='server' EnableViewState="false">
                        <asp:Table ID="JournalEventTable"  runat="server" CssClass="ListContrastTable">
                            <asp:TableHeaderRow ID="TableHeaderRow1" runat="server">
                                <asp:TableHeaderCell runat="server" id="tdEvent" style="width: 150px;text-align: center;" class="listRulesHeader">
                                    <asp:Label ID="Label1" runat="server" ><%=Resources.Resource.Events %></asp:Label>                            
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdWindowsJournal" style="width: 120px;text-align: center;" class="listRulesHeader">
                                    <asp:Label ID="Label2" runat="server" ><%=Resources.Resource.WindowsJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdLocalJournal" style="width: 120px;text-align: center;" class="listRulesHeader">
                                    <asp:Label ID="Label3" runat="server" ><%=Resources.Resource.LocalJournal %></asp:Label>
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
    </div>
    <div id="divOverlay" class="ui-widget-overlay ui-front" style="display: none" />
</div>
