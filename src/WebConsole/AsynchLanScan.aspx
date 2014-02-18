<%@ Page Title="" Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true"
    CodeFile="AsynchLanScan.aspx.cs" Inherits="AsynchLanScan" %>

<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register TagPrefix="custom" Namespace="CustomControls" Assembly="CustomControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <script type="text/javascript" language="javascript" src="AsynchRequestErrorHandler.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divModalDialog").dialog({ autoOpen: false });
            $(document).on("click", 'div[class=EditComment]', function () {
                var sender = $(this);
                var ip = sender.attr('IP');
                var title = sender.attr('titleDialog');
                var saveTitle = sender.attr('saveTitle');
                var isComment = sender.attr('comment');
                var comment = '';
                if (isComment == 'true')
                    comment = $("span[IP='" + ip + "']").text().replace(/^\s\s*/, '').replace(/\s\s*$/, ''); //trim start&end spaces

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
                                        $("span[IP='" + ip + "']").text(comment);
                                        if (comment != '') {
                                            sender.attr('comment', 'true');
                                        }
                                        else
                                            sender.removeAttr('comment');
                                    }
                                });
                            }
                            d.dialog('close');
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
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= lbtnAddIpAddress.ClientID %>').click(function () {
                IpAddressListBox.add();
                return false;
            });

            $('#<%= lbtnEditIpAddress.ClientID %>').click(function () {
                IpAddressListBox.edit();
                return false;
            });

            $('#<%= lbtnDeleteIpAddress.ClientID %>').click(function () {
                IpAddressListBox.remove();
                return false;
            });

            $('#mainAccordion').accordion({ collapsible: true, active: 0 });
            $('#divAccordion').accordion({ collapsible: true, active: false });
        })
        var IpAddressActionButtons = function () {
            return {
                disable: function () {
                    lbtnDeleteIpAddress = $('#<%= lbtnDeleteIpAddress.ClientID %>');
                    lbtnEditIpAddress = $('#<%= lbtnEditIpAddress.ClientID %>');

                    if (navigator.appName == 'Microsoft Internet Explorer') {
                        lbtnDeleteIpAddress.attr('disabled', true);
                        lbtnEditIpAddress.attr('disabled', true);
                    }
                    else {
                        lbtnDeleteIpAddress.css('color', 'gray');
                        lbtnEditIpAddress.css('color', 'gray');
                    }
                    lbtnDeleteIpAddress.css('cursor', 'default');
                    lbtnEditIpAddress.css('cursor', 'default');
                },

                enable: function () {
                    lbtnDeleteIpAddress = $('#<%= lbtnDeleteIpAddress.ClientID %>');
                    lbtnEditIpAddress = $('#<%= lbtnEditIpAddress.ClientID %>');

                    if (navigator.appName == 'Microsoft Internet Explorer') {
                        lbtnDeleteIpAddress.attr('disabled', false);
                        lbtnEditIpAddress.attr('disabled', false);
                    }
                    else {
                        lbtnDeleteIpAddress.css('color', '');
                        lbtnEditIpAddress.css('color', '');
                    }
                    lbtnDeleteIpAddress.css('cursor', 'pointer');
                    lbtnEditIpAddress.css('cursor', 'pointer');

                }
            };
        } ();

        var NewIpAddressTextBox = function () {
            return {
                getText: function () {
                    return $('#<%= tboxNewIpAddress.ClientID %>').val();
                },

                setText: function (text) {
                    $('#<%= tboxNewIpAddress.ClientID %>').val(text);
                }
            };
        } ();

        var IpAddressListBox = function () {
            function clear() {
                $('#<%= lboxIpAddress.ClientID %>>option').remove();
            }

            function matchRegexp(text) {
                var regexp = new RegExp('<%=RegularExpressions.IPAddressRangeFull %>');
                return regexp.test(text);
            }

            function findInvalid() {
                var ret = -1;
                $('#<%= lboxIpAddress.ClientID %>>option').each(function (i) {
                    if (!matchRegexp($(this).text()) || $(this).text() != $(this).val()) {
                        ret = $(this).val();
                    }
                })
                return ret;
            }

            function validateText() {
                return Page_ClientValidate('NewIpAddressValidation');
            }

            function saveList() {
                var value = "";
                var intCount = $('#<%= lboxIpAddress.ClientID %>>option').length;
                var option = $("#<%= lboxIpAddress.ClientID %> option");

                for (i = 0; i < intCount; i++) {
                    value += option.eq(i).text() + ";";
                }

                var sel = "input[id=" + '<%= hdnIPAddress.ClientID %>' + "]";
                $(sel).val(value);
            }

            return {
                validateList: function () {
                    var invalid = findInvalid();
                    if (invalid != -1) {
                        $('#<%= lboxIpAddress.ClientID %>').val(invalid);
                        NewIpAddressTextBox.setText(invalid);
                        validateText();
                        return false;
                    }
                    return true;
                },

                add: function () {
                    lboxIpAddress = $('#<%= lboxIpAddress.ClientID %>');
                    if (!validateText()) {
                        var tooltip = $('#<%= tboxNewIpAddress.ClientID %>').attr('title', "ip field").tooltip({ close: function () { $(this).tooltip("destroy").removeAttr("title") }, tooltipClass: "highlight", content: '<%= Resources.Resource.IpAddressFilterInIncorrectFormat %>' }).tooltip("open");
                        return;
                    }
                    var data = NewIpAddressTextBox.getText();
                    $('#<%= lboxIpAddress.ClientID %>').append($('<option></option>').val(data).text(data));
                    lboxIpAddress.val(data)
                    if ($('#<%= lboxIpAddress.ClientID %>>option').length == 1) {
                        IpAddressActionButtons.enable();
                    }

                    saveList();
                },

                remove: function () {
                    var ipbox = $("#<%= lboxIpAddress.ClientID %>");
                    var option = $("#<%= lboxIpAddress.ClientID %> option");
                    var index = option.index($("#<%= lboxIpAddress.ClientID %> option:selected"));
                    if (index == -1) return;
                    $.when($("#<%= lboxIpAddress.ClientID %> option:selected").remove()).then(function () {
                        if (index < option.length) {
                            index--;
                            ipbox.val(option.eq(index).val())
                        }
                        validateText();
                        NewIpAddressTextBox.setText(ipbox.val());

                        if (option.length == 1) {
                            IpAddressActionButtons.disable();
                        }

                        saveList();
                    });
                },

                edit: function () {
                    var option = $("#<%= lboxIpAddress.ClientID %> option");
                    var index = option.index($("#<%= lboxIpAddress.ClientID %> option:selected"));
                    if (index == -1) return;
                    if (!validateText()) {
                        var tooltip = $('#<%= tboxNewIpAddress.ClientID %>').attr('title', "ip field").tooltip({ close: function () { $(this).tooltip("destroy").removeAttr("title") }, tooltipClass: "highlight", content: '<%= Resources.Resource.IpAddressFilterInIncorrectFormat %>' }).tooltip("open");
                        return;
                    }
                    var data = NewIpAddressTextBox.getText();
                    option.eq(index).text(data).val(data);

                    saveList();
                },

                onChange: function () {
                    var lbox = $("#<%= lboxIpAddress.ClientID %>");
                    var option = $("#<%= lboxIpAddress.ClientID %> option");
                    var index = option.index($("#<%= lboxIpAddress.ClientID %> option:selected"));
                    if (index == -1) return;
                    NewIpAddressTextBox.setText(lbox.val());
                    validateText();
                },

                generate: function () {
                    var arr = new Array();
                    $("#<%= lboxIpAddress.ClientID %> option").each(function () {
                        arr.push($(this).val());
                    })

                    return arr.join('&');
                },
                populate: function (text) {
                    lboxIpAddress = $('#<%= lboxIpAddress.ClientID %>');
                    var option = false;
                    clear();

                    var splited = text.split('&');

                    for (i = 0; i < splited.length; i++) {
                        option = $("#<%= lboxIpAddress.ClientID %> option");
                        if (splited[i] == '') continue;

                        if (matchRegexp(splited[i])) {
                            lboxIpAddress.append($('<option></option>').text(splited[i]).val(splited[i]));
                        }
                        else {
                            lboxIpAddress.append($('<option></option>').text(splited[i]).val(splited[i]));
                        }
                    }
                    option = $("#<%= lboxIpAddress.ClientID %> option");
                    if (option.length > 0) {
                        lboxIpAddress.val(option.eq(0).val())
                        NewIpAddressTextBox.setText(lboxIpAddress.val());
                        validateText();
                        IpAddressActionButtons.enable();
                    }
                    else {
                        NewIpAddressTextBox.setText('');
                        IpAddressActionButtons.disable();
                    }
                }
            };
        } ();
    </script>
    <div id='mainAccordion'>
        <h3>
            <%=Resources.Resource.Settings%></h3>
        <div>
            <table>
                <tr valign="top">
                    <td>
                        <div class="ui-accordion ui-widget ui-helper-reset" style="width: 380px;">
                            <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                    ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                <%=Resources.Resource.IPRange%>
                            </h3>
                            <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                <div>
                                    <a id="lbtnAddIpAddress" runat="server" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                                        role="button" aria-disabled="false"><span class="ui-button-text">
                                            <%=Resources.Resource.Add%></span> </a><a id="lbtnEditIpAddress" runat="server" class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                                                role="button" aria-disabled="false"><span class="ui-button-text">
                                                    <%=Resources.Resource.Edit%></span> </a><a id="lbtnDeleteIpAddress" runat="server"
                                                        class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                                                        role="button" aria-disabled="false"><span class="ui-button-text">
                                                            <%=Resources.Resource.Delete%></span> </a>
                                </div>
                                <div>
                                    <asp:TextBox ID="tboxNewIpAddress" runat="server" Style="width: 310px; margin-top: 8px;
                                        margin-bottom: 10px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqNewIpAddress" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                        ControlToValidate="tboxNewIpAddress" Display="None" ValidationGroup="NewIpAddressValidation">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ControlToValidate="tboxNewIpAddress" ID="regexNewIpAddress"
                                runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressInIncorrectFormat %>' ValidationGroup="NewIpAddressValidation" Display="None"></asp:RegularExpressionValidator>
                                </div>
                                <div>
                                    <asp:Literal ID="litIpAddressExamples" Text='<%$ Resources:Resource, IpAddressExamplesLiteral %>'
                                        runat="server"></asp:Literal>
                                </div>
                                <div>
                                    <select size="4" id="lboxIpAddress" runat="server" class="dropdownlist" onchange="IpAddressListBox.onChange()"
                                        style="height: 180px; width: 310px; margin-top: 8px; margin-left: 1px;">
                                    </select>
                                    <asp:HiddenField runat="server" ID="hdnIPAddress" />
                                </div>
                            </div>
                        </div>
                    </td>
                    <td style="padding-left: 10px;">
                        <div class="ui-accordion ui-widget ui-helper-reset" style="width: 350px;">
                            <h3 class="ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons 
                                    ui-accordion-header-active ui-corner-top" style="cursor: default !important;">
                                <%=Resources.Resource.Credentials%>
                            </h3>
                            <div class="ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content-active">
                                <div>
                                    <asp:Label ID="lblDomain" runat="server" Width="70px" />
                                    <asp:TextBox ID="tboxDomainCr" runat="server" Style="width: 180px" />
                                </div>
                                <div style="margin-top: 5px; margin-bottom: 5px;">
                                    <asp:Label ID="lblLogin" runat="server" Width="70px" />
                                    <asp:TextBox ID="tboxLoginCr" runat="server" Style="width: 180px" autocomplete="off" />
                                    <asp:RequiredFieldValidator ID="requiredLogin" ControlToValidate="tboxLoginCr" runat="server"
                                        Display="None" ErrorMessage='<%$ Resources:Resource, LoginRequiredErrorMessage %>'
                                        ValidationGroup="SettingsValidation">
                                    </asp:RequiredFieldValidator>
                                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredLoginExt" runat="server" TargetControlID="requiredLogin"
                                        HighlightCssClass="highlight">
                                    </ajaxToolkit:ValidatorCalloutExtender>
                                </div>
                                <div>
                                    <asp:Label ID="lblPass" runat="server" Width="70px" />
                                    <asp:TextBox ID="tboxPasswordCr" runat="server" Style="width: 180px" TextMode="Password"
                                        autocomplete="off" />
                                    <asp:CheckBox runat="server" ID="cboxSavePassword" Checked="false" AutoPostBack="false"
                                        Text="*" ForeColor="Red" />
                                    <br />
                                    <asp:RequiredFieldValidator ID="requiredPassword" ControlToValidate="tboxPasswordCr"
                                        runat="server" Display="None" ErrorMessage='<%$ Resources:Resource, PasswordRequiredErrorMessage %>'
                                        ValidationGroup="SettingsValidation">
                                    </asp:RequiredFieldValidator>
                                    <asp:ValidationSummary ID="SettingsValidationSummary" runat="server" ShowMessageBox="True"
                                        ShowSummary="False" ValidationGroup="SettingsValidation" HeaderText='<%$ Resources:Resource, CheckCredentials %>' />
                                    <ajaxToolkit:ValidatorCalloutExtender ID="requiredPasswordExt" runat="server" TargetControlID="requiredPassword"
                                        HighlightCssClass="highlight">
                                    </ajaxToolkit:ValidatorCalloutExtender>
                                </div>
                                <div style="margin-top: 5px;">
                                    <asp:Label runat="server" ID="Label1" Style="color: Red;">* - <%=Resources.Resource.SavePasswordInformation %></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div style="margin-top: 20px;">
                            <div id="divAccordion" style="width: 350px;">
                                <h3>
                                    <%=Resources.Resource.AdditionalSettings%></h3>
                                <div>
                                    <div>
                                        <asp:Label ID="lblPingCount" runat="server" Width="230px" />
                                        <asp:TextBox ID="txtPingCount" runat="server" Style="width: 40px" />
                                    </div>
                                    <div>
                                        <asp:RequiredFieldValidator ID="requiredPingCount" ControlToValidate="txtPingCount"
                                            Display="None" runat="server" ErrorMessage='<%$ Resources:Resource, RequestPacketCountRequiredErrorMessage %>'
                                            ValidationGroup="SettingsValidation"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rangePingCount" runat="server" ControlToValidate="txtPingCount"
                                            Display="None" ValidationGroup="SettingsValidation" MinimumValue="1" MaximumValue="10"
                                            Type="Integer"></asp:RangeValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender ID="requiredPingCountExt" runat="server" TargetControlID="requiredPingCount"
                                            HighlightCssClass="highlight">
                                        </ajaxToolkit:ValidatorCalloutExtender>
                                        <ajaxToolkit:ValidatorCalloutExtender ID="rangePingCountExt" runat="server" TargetControlID="rangePingCount"
                                            HighlightCssClass="highlight">
                                        </ajaxToolkit:ValidatorCalloutExtender>
                                    </div>
                                    <div style="margin-top: 10px;">
                                        <asp:Label ID="lblPingTimeout" runat="server" Width="230px" />
                                        <asp:TextBox ID="txtPingTimeout" runat="server" Style="width: 40px" />
                                    </div>
                                    <div>
                                        <asp:RequiredFieldValidator ID="requiredPingTimeout" ControlToValidate="txtPingTimeout"
                                            Display="None" runat="server" ErrorMessage='<%$ Resources:Resource, TimeoutRequiredErrorMessage %>'
                                            ValidationGroup="SettingsValidation"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rangePingTimeout" runat="server" ControlToValidate="txtPingTimeout"
                                            Display="None" ValidationGroup="SettingsValidation" MinimumValue="1" MaximumValue="100"
                                            Type="Integer"></asp:RangeValidator>
                                        <ajaxToolkit:ValidatorCalloutExtender ID="requiredPingTimeoutExt" runat="server"
                                            TargetControlID="requiredPingTimeout" HighlightCssClass="highlight">
                                        </ajaxToolkit:ValidatorCalloutExtender>
                                        <ajaxToolkit:ValidatorCalloutExtender ID="rangePingTimeoutExt" runat="server" TargetControlID="rangePingTimeout"
                                            HighlightCssClass="highlight">
                                        </ajaxToolkit:ValidatorCalloutExtender>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <custom:StorageControl ID="SettingsStorage" runat="server" StorageType="Session" />
    <table style="min-width: 864px">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:LinkButton ID="btnStart" ValidationGroup="SettingsValidation" runat="server"
                            OnClick="btnStart_Click" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                            Style="padding: 5px; margin-top: 10px; margin-bottom: 10px; margin-left: 10px;
                            width: 100px;">
                                <%=Resources.Resource.Start%>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnStop" runat="server" OnClick="btnStop_Click" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                            Style="padding: 5px; margin-top: 10px; margin-bottom: 10px; margin-left: 5px;
                            width: 100px;">
                                <%=Resources.Resource.Stop%>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnPause" runat="server" OnClick="btnPause_Click" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                            Style="padding: 5px; margin-top: 10px; margin-bottom: 10px; width: 100px;">
                                <%=Resources.Resource.Pause%>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnResume" runat="server" OnClick="btnResume_Click" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                            Style="padding: 5px; margin-top: 10px; margin-bottom: 10px; width: 100px;">
                                <%=Resources.Resource.Resume%>
                        </asp:LinkButton>
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
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td style="width: 20px;" align="right">
                                        <div id="divOptionsOpen" style="width: 16px; height: 16px; cursor: pointer;" title="<%=Resources.Resource.Options %>">
                                            <asp:Image runat="server" ID="imgOptions" AlternateText="Options" OnInit="imgOptions_Init" />
                                        </div>
                                    </td>
                                    <td style="width: 40px;" align="left">
                                        <div id="divSelectionOptions" class="gridViewMenu" onmouseover="$get('divSelectionOptions').className='gridViewMenuHover'"
                                            onmouseout="$get('divSelectionOptions').className='gridViewMenu'">
                                            <div id="divCbox" class="gridViewMenuCbox">
                                                <asp:CheckBox ID="cBoxSelectAll" runat="server" />
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
                    <asp:BoundField DataField="Name" HeaderText='<%$ Resources:Resource, ComputerName %>'
                        SortExpression="Name" />
                    <asp:BoundField HeaderStyle-CssClass="gridViewHeader" DataField="IPAddress" HeaderText='<%$ Resources:Resource, IPAddress %>'
                        SortExpression="IPAddress" />
                    <asp:TemplateField HeaderText='<%$ Resources:Resource, Information %>'>
                        <HeaderStyle Width="500px" />
                        <ItemTemplate>
                            <div style="padding-left: 20px; width: 90%;">
                                <div style="word-wrap: break-word; float: left; width: 90%;">
                                    <asp:Label ID="lblInformation" runat="server" Width="100%" IP="<%# ((VirusBlokAda.RemoteOperations.RemoteScan.RemoteInfo.RemoteInfoEntityShow)Container.DataItem).IPAddress %>" />
                                </div>
                                <div id="imgComment" runat="server" style="float: right; max-width: 20px;" class="EditComment"
                                    titledialog="<%# Resources.Resource.Comment %>" title='<%# Resources.Resource.AddOrEditComment %>'
                                    savetitle='<%# Resources.Resource.Save %>' ip="<%# ((VirusBlokAda.RemoteOperations.RemoteScan.RemoteInfo.RemoteInfoEntityShow)Container.DataItem).IPAddress %>">
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
            <div id="divInstall" style="margin: 5px; float: left; display: none">
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtnInstall" ValidationGroup="SettingsValidation" runat="server"
                                OnClick="lbtnInstall_Click" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                                Style="padding: 5px; margin-top: 10px; margin-bottom: 10px; margin-left: 10px;
                                width: 100px;">
                                    <%=Resources.Resource.Attach %>
                            </asp:LinkButton>
                        </td>
                        <td style="padding-left: 20px;">
                            <asp:Label ID="lblSelectedTotalCountText" runat="server" Text="<%$ Resources:Resource, TotallySelected %>"></asp:Label>
                            &nbsp
                            <asp:Label ID="lblSelectedTotalCount" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbRebootAfterInstall" Checked="false" runat="server" Visible="false" />
                        </td>
                    </tr>
                </table>
            </div>
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
            <%=Resources.Resource.Sort %>
            <%=String.Format("\u2193") %>
        </div>
        <div class="MenuItem" val="sortDesc">
            <%=Resources.Resource.Sort %>
            <%=String.Format("\u2191")%>
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
    <div id="divModalDialog" style="display: none;">
    </div>
</asp:Content>