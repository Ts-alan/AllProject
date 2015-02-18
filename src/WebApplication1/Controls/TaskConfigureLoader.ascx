<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_TaskConfigureLoader" Codebehind="TaskConfigureLoader.ascx.cs" %>
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
            $('#' + '<%=tboxAddDialogUpdatePath.ClientID %>').focus();
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
            $('#' + '<%=tboxAddDialogUpdatePath.ClientID %>').focus();
        });

        $(document).on("click", '#<%= lbtnUserAdd.ClientID %>', function () {
            var disabled = $('#<%= lbtnUserAdd.ClientID %>').attr("disabled");

            if (disabled == "disabled") {
                return;
            }

            var dOpt = {
                width: 350,
                resizable: false,
                close: function (event, ui) {
                    btnAddUserPathDialogSetDefaultValues();
                    $('#divOverlay').css('display', 'none');
                },
                buttons: {
                    '<%=Resources.Resource.Apply%>': function () {
                        if (CheckUserName($('#<%=tboxAddDialogUser.ClientID %>').val())) {
                            var btn = '<%=lbtnAddUserPathDialogApply.UniqueID %>';
                            if (Page_ClientValidate('AddDialogUserPathValidationGroup')) {
                                __doPostBack(btn, '');
                                $('#AddUserPathDialog').dialog('close');
                            }
                        }
                        else {
                            alert("User already exists.");
                        }
                    },
                    '<%=Resources.Resource.CancelButtonText%>': function () {
                        $('#AddUserPathDialog').dialog('close');
                    }
                }
            };
            $('#AddUserPathDialog').dialog(dOpt);
            $('#divOverlay').css('display', 'inline');
            $('#AddUserPathDialog').parent().appendTo(jQuery("form:first"));
            $('#' + '<%=tboxAddDialogUser.ClientID %>').focus();
        });

        function btnAddUserPathDialogSetDefaultValues() {
            $('#<%=tboxAddDialogUser.ClientID %>').val('');
            $('#<%=tboxAddDialogPassword.ClientID %>').val('');
        }

        $(document).on("click", '#<%= lbtnUserChange.ClientID %>', function () {
            var disabled = $('#<%= lbtnUserChange.ClientID %>').attr("disabled");

            if (disabled == "disabled") {
                return;
            }
            var lbox = '#<%=lboxUsers.ClientID %>';
            if ($(lbox).prop("selectedIndex") < 0)
                return;
            $('#' + '<%=tboxAddDialogUser.ClientID %>').val($(lbox + " option:selected").text());
            $('#' + '<%=tboxAddDialogPassword.ClientID %>').val($(lbox + " option:selected").attr("pass"));
            var dOpt = {
                width: 350,
                resizable: false,
                close: function (event, ui) {
                    $('#divOverlay').css('display', 'none');
                    btnAddUserPathDialogSetDefaultValues();
                },
                buttons: {
                    '<%=Resources.Resource.Apply%>': function () {
                        if (CheckUserName($('#<%=tboxAddDialogUser.ClientID %>').val())) {
                            var btn = '<%=lbtnUserPathChange.UniqueID %>';
                            if (Page_ClientValidate('AddDialogUserPathValidationGroup')) {
                                __doPostBack(btn, '');
                                $('#AddUserPathDialog').dialog('close');
                            }
                        }
                        else {
                            alert("User already exists.");
                        }
                    },
                    '<%=Resources.Resource.CancelButtonText%>': function () {
                        $('#AddUserPathDialog').dialog('close');
                    }
                }
            };
            $('#AddUserPathDialog').dialog(dOpt);
            $('#divOverlay').css('display', 'inline');
            $('#AddUserPathDialog').parent().appendTo(jQuery("form:first"));
            $('#' + '<%=tboxAddDialogUser.ClientID %>').focus();
        });

        function CheckUserName(name) {
            var lbox = '#<%=lboxUsers.ClientID %>';
            var result = true;
            if ($(lbox + " option:selected").text() != name) {
                $(lbox + ' option').each(function () {
                    if ($(this).text() == name)
                        result = false;
                });
            }

            return result;
        }
    });
    $(document).on("click", '#<%= lbtnUpdateDelete.ClientID %>', function () {
        var disabled = $('#<%= lbtnUpdateDelete.ClientID %>').attr("disabled");
        
        if (disabled == "disabled") {
            return;
        }
    });

    $(document).on("click", '#<%= lbtnUserDelete.ClientID %>', function () {
        var disabled = $('#<%= lbtnUserDelete.ClientID %>').attr("disabled");

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
        <li><a href="#tabLoader4"><%=Resources.Resource.CongLdrPassword%></a> </li>
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
    <div id="tabLoader4" class="divSettings">
        <asp:UpdatePanel ID="upnlPasswords" runat="server">
            <ContentTemplate>
                <div style="width: 500px;padding-left:5px;" class="ListContrastTable">
                    <div>
                        <%=Resources.Resource.UserManaging%>
                    </div>                       
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:ListBox runat="server" ID="lboxUsers" style="width: 300px;height: 160px;" ></asp:ListBox> 
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnUserAdd" runat="server" SkinID="Button" OnClientClick="return false;" Width="100" ><%=Resources.Resource.Add %></asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lbtnUserDelete" runat="server" SkinID="Button" OnClick="lbtnUserDelete_Click" Width="100"><%=Resources.Resource.Delete %></asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lbtnUserChange" runat="server" SkinID="Button" OnClientClick="return false;" Width="100"><%=Resources.Resource.Change %></asp:LinkButton>
                                    <asp:LinkButton ID="lbtnAddUserPathDialogApply" runat='server' Style="display: none" OnClick="lbtnAddUserPathDialogApply_Click" ></asp:LinkButton>
                                    <asp:LinkButton ID="lbtnUserPathChange" runat='server' Style="display: none" OnClick="lbtnUserPathChange_Click" ></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>                    
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>       
    </div>   
</div>
<div id="divOverlay" class="ui-widget-overlay ui-front" style="display: none" ></div>
<div id="AddUpdatePathDialog" style="display: none;padding-bottom:20px;" class="ui-front">    
    <%=Resources.Resource.Path %><br />
    <asp:TextBox ID="tboxAddDialogUpdatePath" Style="width: 300px" runat='server'></asp:TextBox>
    <asp:RequiredFieldValidator ID="AddDialogUpdatePathValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
        ControlToValidate="tboxAddDialogUpdatePath" Display="None" ValidationGroup="AddDialogUpdatePathValidationGroup" >
    </asp:RequiredFieldValidator>
    <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutAddDialogUpdatePath" runat="server"
        TargetControlID="AddDialogUpdatePathValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" >
    </ajaxToolkit:ValidatorCalloutExtender>
</div>
<div id="AddUserPathDialog" style="display: none;padding-bottom:20px;" class="ui-front">    
    <%=Resources.Resource.User %>
    <br />
    <asp:TextBox ID="tboxAddDialogUser" Style="width: 300px" runat='server'></asp:TextBox>
    <asp:RequiredFieldValidator ID="AddDialogUserValidator" runat="server" ErrorMessage='<%$ Resources:Resource, UserNameRequiredErrorMessage %>'
        ControlToValidate="tboxAddDialogUser" Display="None" ValidationGroup="AddDialogUserPathValidationGroup" >
    </asp:RequiredFieldValidator>
    <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutAddDialogUser" runat="server"
        TargetControlID="AddDialogUserValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" >
    </ajaxToolkit:ValidatorCalloutExtender>
    <br />
    <%=Resources.Resource.PasswordLabelText %>
    <br />
    <asp:TextBox ID="tboxAddDialogPassword" TextMode="Password" Style="width: 300px" runat='server'></asp:TextBox>
    <asp:RequiredFieldValidator ID="AddDialogPasswordValidator" runat="server" ErrorMessage='<%$ Resources:Resource, PasswordRequiredErrorMessage %>'
        ControlToValidate="tboxAddDialogPassword" Display="None" ValidationGroup="AddDialogUserPathValidationGroup" >
    </asp:RequiredFieldValidator>
    <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutAddDialogPassword" runat="server"
        TargetControlID="AddDialogPasswordValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" >
    </ajaxToolkit:ValidatorCalloutExtender>
</div>
