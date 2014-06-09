<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureLoader.ascx.cs" Inherits="Controls_TaskConfigureLoader" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:100%"><%=Resources.Resource.CongLdrConfigureLoader%></div>

<script language="javascript" type="text/javascript" >

    function pageLoad() {
        $("#TabsLoader").tabs({ cookie: { expires: 30} });
        $("input[class='control']").button();
        $("input[type='button']").button();

        HideTable1(); 
        HideTable2(); 

        var activeRowNo=$('#<%=UpdatePathHdnActiveRowNo.ClientID %>').attr('value');
        btnDeleteUpdateEnabled(activeRowNo);

           $('#<%= UpdateAddButton.ClientID %>').on("click", function () {
                var dOpt = {
                    width: 350,                                       
                    resizable: false,
                    close: function(event, ui)
                        {
                            btnAddUpdatePathDialogSetDefaultValues() ; 
                            $('#divOverlay').css('display','none');
                        },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () {
                            var btn = '<%=AddUpdatePathDialogApplyButton.UniqueID %>';
                            if( Page_ClientValidate('AddDialogUpdatePathValidationGroup'))
                            { 
                                SetActiveRowNo(0);    
                                __doPostBack(btn, '');                                                              
                                $('#AddUpdatePathDialog').dialog('close');
                                                              
                            }                            
                        },
                       <%=Resources.Resource.CancelButtonText%>: function () {                           
                            $('#AddUpdatePathDialog').dialog('close');                            
                        }
                    }
                };
                $('#AddUpdatePathDialog').dialog(dOpt);
                $('#divOverlay').css('display','inline');
                $('#AddUpdatePathDialog').parent().appendTo(jQuery("form:first"));
             });
                $("[UpdateRowSelected]").hover(function(){
                    if($(this).attr('UpdateRowSelected')=="true") return;
                    $(this).css('background-color', 'yellow');
                },
                function () { 
                    if($(this).attr('UpdateRowSelected')=="true") return;           
                    $(this).css('background-color', '');
                }
            );
            $("[UpdateRowSelected]").on("click",function(){
                if($(this).attr("UpdateRowSelected")=="true") return;
                $(this).css('background-color', '#3399ff');
                $("[UpdateRowSelected=true]").css('background-color', '');
                $("[UpdateRowSelected=true]").attr('UpdateRowSelected',false);
                $(this).attr('UpdateRowSelected',true);                
                var rowNo=$(this).children().find('[value]').attr('value');                
                SetActiveRowNo(rowNo);
             });
        };
        function SetActiveRowNo(no)
        {
            $('#<%=UpdatePathHdnActiveRowNo.ClientID %>').attr('value',no);
            btnDeleteUpdateEnabled(no);
        }
        /**/
        function btnDeleteUpdateEnabled(rowNo)
        {
            var enabled=false;
            if(rowNo!=0) {
                enabled=true;
                var tableRow=$("[id*='hdnUpdateRowNo'][value='"+rowNo+"']").parent().parent(); 
                tableRow.css('background-color', '#3399ff');
                tableRow.attr('UpdateRowSelected',true); 
            }
            var delbtn=$('#<%=UpdateDeleteButton.ClientID %>');
            var chgbtn=$('#<%=UpdateChangeButton.ClientID %>');
            var upbtn=$('#<%=UpdateMoveUPButton.ClientID %>');
            var downbtn=$('#<%=UpdateMoveDownButton.ClientID %>');
            delbtn.button( "option", "disabled",!enabled);
            chgbtn.button( "option", "disabled",!enabled);
            upbtn.button( "option", "disabled",!enabled);
            downbtn.button( "option", "disabled",!enabled);
        }
        /**/
        function btnAddUpdatePathDialogSetDefaultValues()
        {
           $('#<%=AddDialogUpdatePath.ClientID %>').val('');
        };
        function UpdatePathChangeButtonClientClick()
        {
            var index=$('#<%=UpdatePathHdnActiveRowNo.ClientID %>').attr('value');
            if(index==0)
                return;
            var tableRow=$("[id*='hdnUpdateRowNo'][value='"+index+"']").parent().parent();           
            PathChangeDialog(tableRow);            
        }
        function PathChangeDialog(tableRow)
        {
            var path=tableRow.children().find("[id*='CongLdrUpdatePath']").text();
            $('#' + '<%=AddDialogUpdatePath.ClientID %>').val(path);                    
                var dOpt = {
                width: 350,
                resizable: false,
                close: function(event, ui)
                {
                    $('#divOverlay').css('display','none');
                    btnAddUpdatePathDialogSetDefaultValues();                            
                },
                buttons: {
                    <%=Resources.Resource.Apply%>: function () {
                        var btn = '<%=UpdatePathChangeButton.UniqueID %>';
                        if( Page_ClientValidate('AddDialogUpdatePathValidationGroup'))
                        {   
                            
                            __doPostBack(btn, '');
                                                                                      
                            $('#AddUpdatePathDialog').dialog('close');                                
                        }     
                    },
                    <%=Resources.Resource.CancelButtonText%>: function () {                            
                        $('#AddUpdatePathDialog').dialog('close');
                    }
                }
            };
            $('#AddUpdatePathDialog').dialog(dOpt);
            $('#divOverlay').css('display','inline');
            $('#AddUpdatePathDialog').parent().appendTo(jQuery("form:first"));
        } 
        function HideTable1() {
         var cbox = $("#<%= cboxProxyEnabled.ClientID %>").is(":checked");
         $("#<%=ddlProxyType.ClientID %>").prop("disabled", !cbox);
         $("#<%= tboxProxyAddress.ClientID%>").prop("disabled", !cbox);
         $("#<%= tboxProxyPort.ClientID%>").prop("disabled", !cbox);

         HideTable1_1(cbox);
     }

     function HideTable1_1(checked) {
         $("#<%= cboxProxyAuthorizationEnabled.ClientID %>").prop("disabled", !checked);
         $("#<%= cboxProxyAuthorizationEnabled.ClientID %>").parent().prop("disabled", !checked);
         var cbox = $("#<%= cboxProxyAuthorizationEnabled.ClientID %>").is(":checked");
         if (!checked) {             
             cbox = false;
         }
         $("#<%= tboxProxyAuthorizationUserName.ClientID%>").prop("disabled", !cbox);
         $("#<%= tboxProxyAuthorizationPassword.ClientID%>").prop("disabled", !cbox);
     }

    function HideTable2() {
        var cbox = $("#<%= cboxAuthorizationEnabled.ClientID %>").is(":checked");
        $("#<%= tboxAuthorizationUserName.ClientID%>").prop("disabled", !cbox);
        $("#<%= tboxAuthorizationPassword.ClientID%>").prop("disabled", !cbox);
    }  

</script>
<div id="TabsLoader" style="width:100%">
    <ul>
        <li><a href="#tab1"><%=Resources.Resource.Main%></a> </li>
        <li><a href='#tab2'><span><%=Resources.Resource.Authorization %></span></a></li>
        <li><a href='#tab3'><span><%=Resources.Resource.Proxy %></span></a></li>
        <%--<li><a href="#tab4"><%=Resources.Resource.CongLdrPassword%></a> </li>--%>

    </ul>
    <div id="tab1" class="divSettings" >
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="UpdatePathHdnActiveRowNo" Value='0' runat="server" />
                    <asp:Table  runat="server" Style="width: 500px" GridLines="Horizontal" class="ListContrastTable">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell ColumnSpan="4">
                                <asp:Label  runat="server"><%=Resources.Resource.UpdatePath %></asp:Label>
                            </asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3">
                                <asp:Panel  runat="server" Width="460px" Height="160px" Style="overflow: scroll">
                                    <asp:DataList ID="CongLdrUpdateDataList" runat="server" Style="table-layout: fixed; word-break: break-all;" rules="all" 
                                        OnItemDataBound="CongLdrUpdateDataList_ItemDataBound">
                                        <ItemTemplate>
                                            <tr runat="server" id="trUpdateItem" UpdateRowSelected="false">
                                                <td runat="server">
                                                    <asp:HiddenField runat="server" ID="hdnUpdateRowNo" Value="0"/>
                                                    <asp:Label runat="server" ID="CongLdrUpdatePath" Width="440px"></asp:Label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </asp:Panel>
                            </asp:TableCell>
                            <asp:TableCell>
                                <table>
                                    <tr><td>
                                        <asp:Button ID="UpdateMoveUPButton" runat="server" Text='&#x2191;' OnClick="UpdatePathMoveUpButtonClick"/>
                                    </td></tr>
                                    <tr><td>
                                        <asp:Button ID="UpdateMoveDownButton" runat="server" Text='&#x2193;'  OnClick="UpdatePathMoveDownButtonClick"/>
                                    </td></tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell >
                                <asp:Button ID="UpdateAddButton" runat="server" Text="<%$ Resources:Resource, Add %>" OnClientClick="return false;"/>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="UpdateDeleteButton" runat="server"  Text="<%$ Resources:Resource, Delete %>" OnClick="UpdatePathDeleteButtonClick"/>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="UpdateChangeButton" runat="server"  Text="<%$ Resources:Resource, Change %>" OnClientClick="UpdatePathChangeButtonClientClick()"/>
                            </asp:TableCell>
                            <asp:TableCell> &nbsp</asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>  
                    <asp:Button ID="AddUpdatePathDialogApplyButton" runat='server' Style="display: none" OnClick="AddUpdatePathDialogApplyButtonClick" />
                    <asp:Button ID="UpdatePathChangeButton" runat='server' Style="display: none" OnClick="UpdatePathChangeButtonClick" />
             
                </ContentTemplate>
            </asp:UpdatePanel>
    </div>
    <div id="tab2" class="divSettings">
        <table class="ListContrastTable" style="width:500px">
            <tr>
                <td style="padding-top: 5px;padding-bottom: 5px;" colspan="2">
                    <asp:CheckBox ID="cboxProxyEnabled" runat="server" Text="<%$ Resources:Resource, CongLdrUseProxyServer %>" onclick="HideTable1()" />
                </td>
            </tr>
            <tr>
                <td >
                    <asp:Label runat="server" Text="<%$ Resources:Resource, CongLdrProxyServerType %>"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlProxyType"  style="width:120px;">
                        <asp:ListItem Text="NO PROXY" Value="0"></asp:ListItem>
                        <asp:ListItem Text="HTTP" Value="1"></asp:ListItem>
                        <asp:ListItem Text="SOCKS4" Value="3"></asp:ListItem>
                        <asp:ListItem Text="SOCKS5" Value="4"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 20px;">
                    <asp:Label runat="server"  Width="100px" Text="<%$ Resources:Resource, CongLdrAddress %>"/>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tboxProxyAddress" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 20px;">
                    <asp:Label runat="server"  Width="100px" Text="<%$ Resources:Resource, CongLdrPort %>"/>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tboxProxyPort"/>
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;padding-bottom: 5px;" colspan="2">
                    <asp:CheckBox ID="cboxProxyAuthorizationEnabled" runat="server" Text="<%$ Resources:Resource, UseAuthorization %>" onclick="HideTable1_1(true)" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 20px;">
                    <asp:Label runat="server" id="lblProxyAuthorizationUserName" Width="100px" Text="<%$ Resources:Resource, CongLdrUserName %>"/>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tboxProxyAuthorizationUserName" autocomplete="off" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 20px;padding-bottom: 5px;">
                    <asp:Label runat="server" id="lblProxyAuthorizationPassword" Width="100px" Text="<%$ Resources:Resource, CongLdrPassword %>"/>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tboxProxyAuthorizationPassword" TextMode="Password" autocomplete="off" Value=""/>
                </td>
            </tr>
        </table>
    </div>
    <div id="tab3" class="divSettings">
        <table class="ListContrastTable" style="width:500px">
            <tr>
                <td style="padding-top: 5px;padding-bottom: 5px;">
                    <asp:CheckBox ID="cboxAuthorizationEnabled" runat="server" Text="<%$ Resources:Resource, UseAuthorization %>" onclick="HideTable2()" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 20px;">
                    <asp:Label runat="server" id="lblAuthorizationUserName" Width="100px" Text="<%$ Resources:Resource, CongLdrUserName %>"/>
                    <asp:TextBox runat="server" ID="tboxAuthorizationUserName" autocomplete="off" Value=""/>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 20px;padding-bottom: 5px;" >
                    <asp:Label runat="server" id="lblAuthorizationPassword" Width="100px" Text="<%$ Resources:Resource, CongLdrPassword %>"/>
                    <asp:TextBox runat="server" ID="tboxAuthorizationPassword" TextMode="Password" autocomplete="off" />
                </td>
            </tr>                
        </table>
    </div>
                           
                            
                 
    <div id="tab4" class="divSettings" style="display:none">
        <table class="ListContrastTable">
             <tr>                 
                <td colspan="2">
                    <asp:UpdatePanel  runat="server">                    
                        <ContentTemplate>
                            <asp:HiddenField ID="PasswordHdnActiveRowNo" Value='0' runat="server" />
                            <asp:Table  runat="server" Style="width: 500px" GridLines="Horizontal">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell ColumnSpan="2">
                                    <asp:Label  runat="server" Text="EDIT PASSWORDS" ></asp:Label>
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="3">
                                        <asp:Panel  runat="server" Width="400px" Height="160px" Style="overflow: scroll">
                                            <asp:DataList ID="PasswordDataList" runat="server" Style="table-layout: fixed; word-break: break-all;" rules="all">
                                               <%-- OnItemDataBound="PasswordDataList_ItemDataBound">--%>
                                                <ItemTemplate>
                                                    <tr runat="server" id="trPasswordItem" PasswordRowSelected="false">
                                                        <td  runat="server">
                                                            <asp:HiddenField ID="PasswordHdnRowNo" Value="0" runat="server" />
                                                            <asp:Label runat="server" ID="PasswordUser" Width="380px"></asp:Label>
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
                                                    <asp:Button ID="btnPasswordAdd" runat="server" Text="<%$ Resources:Resource, Add %>" Width="120px" OnClientClick="return false;"/>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <%--<asp:TableCell Height="50px" HorizontalAlign="Center">
                                                    <asp:Button ID="btnPasswordDelete" runat="server" Text="<%$ Resources:Resource, Delete %>" Width="120px"  OnClick="PasswordDeleteButtonClick"   />
                                                </asp:TableCell>--%>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell Height="50px" HorizontalAlign="Center"  >
                                                    <asp:Button ID="btnPasswordChange" runat="server" Text="<%$ Resources:Resource, Change %>" Width="120px" OnClientClick="PasswordChangeButtonClientClick()" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                          <%--  <asp:Button ID="AddPasswordDialogApplyButton" runat='server' Style="display: none" OnClick="AddPasswordDialogApplyButtonClick" />
                            <asp:Button ID="PasswordChangeButton" runat='server' Style="display: none" OnClick="PasswordChangeButtonClick" />--%>
                        </ContentTemplate>                                         
                    </asp:UpdatePanel>
               </td>
            </tr>
        </table>        
    </div>   
</div>
<div id="divOverlay" class="ui-widget-overlay ui-front" style="display: none" />
<div id="AddUpdatePathDialog" style="display: none;" class="ui-front">
    <br />
    <asp:Label   runat="server"><%=Resources.Resource.Path %></asp:Label><br />
    <asp:TextBox ID="AddDialogUpdatePath" Style="width: 300px" runat='server'></asp:TextBox>
    <asp:RequiredFieldValidator ID="AddDialogUpdatePathValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
        ControlToValidate="AddDialogUpdatePath" Display="None" ValidationGroup="AddDialogUpdatePathValidationGroup" />

    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutAddDialogUpdatePath" runat="server"
        TargetControlID="AddDialogUpdatePathValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />          
      
</div>
