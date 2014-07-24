<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureLoader.ascx.cs" Inherits="Controls_TaskConfigureLoader" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:100%"><%=Resources.Resource.CongLdrConfigureLoader%></div>

<script language="javascript" type="text/javascript" >

    $(document).ready(function () {
        $("#TabsLoader").tabs({ cookie: { expires: 30} });

        HideTableLoader1();
        HideTableLoader2();

        $(document).on("click", '#<%= lbtnUpdateAdd.ClientID %>', function () {
            var disabled = $('#<%= lbtnUpdateAdd.ClientID %>').attr("disabled");

            if (disabled == "disabled") {
                return;
            }
            var dOpt = {
                width: 350,
                resizable: false,
                close: function (event, ui) {
                    btnAddUpdatePathDialogSetDefaultValues();
                    $('#divOverlay').css('display', 'none');
                },
                buttons: {
                    '<%=Resources.Resource.Apply%>': function () {
                        var btn = '<%=lbtnAddUpdatePathDialogApply.UniqueID %>';
                        if (Page_ClientValidate('AddDialogUpdatePathValidationGroup')) {
                            __doPostBack(btn, '');
                            $('#AddUpdatePathDialog').dialog('close');
                        }
                    },
                    '<%=Resources.Resource.CancelButtonText%>': function () {
                        $('#AddUpdatePathDialog').dialog('close');
                    }
                }
            };
            $('#AddUpdatePathDialog').dialog(dOpt);
            $('#divOverlay').css('display', 'inline');
            $('#AddUpdatePathDialog').parent().appendTo(jQuery("form:first"));
        });

        function btnAddUpdatePathDialogSetDefaultValues() {
            $('#<%=tboxAddDialogUpdatePath.ClientID %>').val('');
        }

        $(document).on("click", '#<%= lbtnUpdateChange.ClientID %>', function () {
            var disabled = $('#<%= lbtnUpdateChange.ClientID %>').attr("disabled");

            if (disabled == "disabled") {
                return;
            }
            var lbox = '#<%=lboxUpdatePathes.ClientID %>';
            if ($(lbox).prop("selectedIndex") < 0)
                return;
            $('#' + '<%=tboxAddDialogUpdatePath.ClientID %>').val($(lbox + " option:selected").text());
            var dOpt = {
                width: 350,
                resizable: false,
                close: function (event, ui) {
                    $('#divOverlay').css('display', 'none');
                    btnAddUpdatePathDialogSetDefaultValues();
                },
                buttons: {
                    '<%=Resources.Resource.Apply%>': function () {
                        var btn = '<%=lbtnUpdatePathChange.UniqueID %>';
                        if (Page_ClientValidate('AddDialogUpdatePathValidationGroup')) {
                            __doPostBack(btn, '');
                            $('#AddUpdatePathDialog').dialog('close');
                        }
                    },
                    '<%=Resources.Resource.CancelButtonText%>': function () {
                        $('#AddUpdatePathDialog').dialog('close');
                    }
                }
            };
            $('#AddUpdatePathDialog').dialog(dOpt);
            $('#divOverlay').css('display', 'inline');
            $('#AddUpdatePathDialog').parent().appendTo(jQuery("form:first"));
        });
    });
    $(document).on("click", '#<%= lbtnUpdateDelete.ClientID %>', function () {
        var disabled = $('#<%= lbtnUpdateDelete.ClientID %>').attr("disabled");
        
        if (disabled == "disabled") {
            return;
        }
    });
    function HideTableLoader1() {
        var cbox = $("#<%= cboxProxyEnabled.ClientID %>").is(":checked");
        $("#<%=ddlProxyType.ClientID %>").prop("disabled", !cbox);
        $("#<%= tboxProxyAddress.ClientID%>").prop("disabled", !cbox);
        $("#<%= tboxProxyPort.ClientID%>").prop("disabled", !cbox);

        HideTableLoader1_1(cbox);
    }

    function HideTableLoader1_1(checked) {
        $("#<%= cboxProxyAuthorizationEnabled.ClientID %>").prop("disabled", !checked);
        $("#<%= cboxProxyAuthorizationEnabled.ClientID %>").parent().prop("disabled", !checked);
        var cbox = $("#<%= cboxProxyAuthorizationEnabled.ClientID %>").is(":checked");
        if (!checked) {
            cbox = false;
        }
        $("#<%= tboxProxyAuthorizationUserName.ClientID%>").prop("disabled", !cbox);
        $("#<%= tboxProxyAuthorizationPassword.ClientID%>").prop("disabled", !cbox);
    }

    function HideTableLoader2() {
        var cbox = $("#<%= cboxAuthorizationEnabled.ClientID %>").is(":checked");
        $("#<%= tboxAuthorizationUserName.ClientID%>").prop("disabled", !cbox);
        $("#<%= tboxAuthorizationPassword.ClientID%>").prop("disabled", !cbox);
    }

</script>
<div id="TabsLoader" style="width:100%">
    <ul>
        <li><a href="#tabLoader1"><%=Resources.Resource.Main%></a> </li>
        <li><a href='#tabLoader2'><span><%=Resources.Resource.Proxy%></span></a></li>
        <li><a href='#tabLoader3'><span><%=Resources.Resource.Authorization %></span></a></li>
        <%--<li><a href="#tabLoader4"><%=Resources.Resource.CongLdrPassword%></a> </li>--%>

    </ul>
    <div id="tabLoader1" class="divSettings" >
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div style="width: 500px;padding-left:5px;" class="ListContrastTable">
                    <div>
                        <%=Resources.Resource.UpdatePath %>
                    </div>                       
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:ListBox runat="server" ID="lboxUpdatePathes" style="width: 460px;height: 160px;" ></asp:ListBox> 
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnUpdateMoveUP" runat="server" Text='&#x2191;' SkinID="Button" OnClick="lbtnUpdateMoveUP_Click"></asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lbtnUpdateMoveDown" runat="server" Text='&#x2193;' SkinID="Button" OnClick="lbtnUpdateMoveDown_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <asp:LinkButton ID="lbtnUpdateAdd" runat="server" SkinID="Button" OnClientClick="return false;" ><%=Resources.Resource.Add %></asp:LinkButton>
                        <asp:LinkButton ID="lbtnUpdateDelete" runat="server" SkinID="Button" OnClick="lbtnUpdateDelete_Click"><%=Resources.Resource.Delete %></asp:LinkButton>
                        <asp:LinkButton ID="lbtnUpdateChange" runat="server" SkinID="Button" OnClientClick="return false;"><%=Resources.Resource.Change %></asp:LinkButton>
                        <asp:LinkButton ID="lbtnAddUpdatePathDialogApply" runat='server' Style="display: none" OnClick="lbtnAddUpdatePathDialogApply_Click" ></asp:LinkButton>
                        <asp:LinkButton ID="lbtnUpdatePathChange" runat='server' Style="display: none" OnClick="lbtnUpdatePathChange_Click" ></asp:LinkButton>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="tabLoader2" class="divSettings">
        <div class="ListContrastTable" style="width:500px;padding-left:5px;padding-bottom: 10px">
            <div style="padding-bottom: 5px;padding-top:5px" >
                <asp:CheckBox ID="cboxProxyEnabled" runat="server" Text="<%$ Resources:Resource, CongLdrUseProxyServer %>" onclick="HideTableLoader1()" />
            </div>
            <div style="padding-left:20px">
                <span style="width:100px;display:inline-block" ><%= Resources.Resource.CongLdrProxyServerType %></span>
                <asp:DropDownList runat="server" ID="ddlProxyType"  style="width:120px;">
                    <asp:ListItem Text="NO PROXY" Value="0"></asp:ListItem>
                    <asp:ListItem Text="HTTP" Value="1"></asp:ListItem>
                    <asp:ListItem Text="SOCKS4" Value="3"></asp:ListItem>
                    <asp:ListItem Text="SOCKS5" Value="4"></asp:ListItem>
                </asp:DropDownList>                
            </div>
             <div style="padding-top:5px;padding-left:20px">
                <span style="width:100px;display:inline-block" ><%= Resources.Resource.CongLdrAddress %></span>
                <asp:TextBox runat="server" ID="tboxProxyAddress" />
            </div>
            <div style="padding-top:5px;padding-left:20px">
                <span style="width:100px;display:inline-block" ><%= Resources.Resource.CongLdrPort %></span>
                <asp:TextBox runat="server" ID="tboxProxyPort" />
            </div>
            <div style="padding-top:5px">                
                <asp:CheckBox ID="cboxProxyAuthorizationEnabled" runat="server" Text="<%$ Resources:Resource, UseAuthorization %>" onclick="HideTableLoader1_1(true)" />
            </div>
            <div style="padding-top:5px;padding-left:20px">   
                <span style="width:100px;display:inline-block" ><%= Resources.Resource.CongLdrUserName %></span>
                <asp:TextBox runat="server" ID="tboxProxyAuthorizationUserName" autocomplete="off" />
            </div>
            <div style="padding-top:5px;padding-left:20px">   
                <span style="width:100px;display:inline-block" ><%= Resources.Resource.CongLdrPassword %></span>
                <asp:TextBox runat="server" ID="tboxProxyAuthorizationPassword" TextMode="Password" autocomplete="off" Value=""  />
            </div>
        </div>
    </div>
    <div id="tabLoader3" class="divSettings">
        <div class="ListContrastTable" style="width:500px;padding-left:5px">
            <div style="padding-top: 5px">
                <asp:CheckBox ID="cboxAuthorizationEnabled" runat="server" Text="<%$ Resources:Resource, UseAuthorization %>" onclick="HideTableLoader2()" />
            </div>
            <div style="padding-left: 20px;padding-top:5px">
                <span style="width:100px;display:inline-block" ><%=Resources.Resource.CongLdrUserName %></span>
                <asp:TextBox runat="server" ID="tboxAuthorizationUserName" autocomplete="off" Value=""/>
            </div>
            <div style="padding-left: 20px;padding-bottom: 5px;padding-top:5px" >
                <span style="width:100px;display:inline-block"><%=Resources.Resource.CongLdrPassword %></span>
                <asp:TextBox runat="server" ID="tboxAuthorizationPassword" TextMode="Password" autocomplete="off" />
            </div>               
        </div>
    </div>
    <%--hidden div--%>
    <div id="tabLoader4" class="divSettings" style="display:none">
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
                                                <asp:TableCell Height="50px" HorizontalAlign="Center"  >
                                                    <asp:Button ID="btnPasswordChange" runat="server" Text="<%$ Resources:Resource, Change %>" Width="120px" OnClientClick="PasswordChangeButtonClientClick()" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </ContentTemplate>                                         
                    </asp:UpdatePanel>
               </td>
            </tr>
        </table>        
    </div>   
</div>
<div id="divOverlay" class="ui-widget-overlay ui-front" style="display: none" ></div>
<div id="AddUpdatePathDialog" style="display: none;padding-bottom:20px;" class="ui-front">    
    <%=Resources.Resource.Path %><br />
    <asp:TextBox ID="tboxAddDialogUpdatePath" Style="width: 300px" runat='server'></asp:TextBox>
    <asp:RequiredFieldValidator ID="AddDialogUpdatePathValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
        ControlToValidate="tboxAddDialogUpdatePath" Display="None" ValidationGroup="AddDialogUpdatePathValidationGroup" >
    </asp:RequiredFieldValidator>
    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutAddDialogUpdatePath" runat="server"
        TargetControlID="AddDialogUpdatePathValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" >
    </ajaxToolkit:ValidatorCalloutExtender2>
</div>
