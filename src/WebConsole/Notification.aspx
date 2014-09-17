<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="Notification.aspx.cs" Inherits="Notification" Title="Untitled Page" %>
<%@ Register Src="Controls/NotifyCnfg.ascx" TagName="Notify" TagPrefix="NotifyCnfg" %>
<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register TagPrefix="custom" Namespace="VirusBlokAda.CC.CustomControls" Assembly="CustomControls" %>
<%@ Register Src="~/Controls/AsyncLoadingStateControl.ascx" TagName="AsyncLoadingStateControl" TagPrefix="cc" %>

<%@ OutputCache Location="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager  ID="ScriptManager1" runat="server" />

<script type="text/javascript">
    var cboxUseMail = '#<%=cboxUseMail.ClientID %>';
    var cboxUseMailAuthorization = '#<%=cboxAuthorizationEnabled.ClientID %>';    
    var cboxUseJabber = '#<%=cboxUseJabber.ClientID %>';
    var cboxUseFlowAnalysis = '#<%=cboxUseFlowAnalysis.ClientID %>';

    $(document).ready(function () {
        $("#tabs").tabs({ cookie: { expires: 30} });

        $(document).on("click", cboxUseMail, function () {
            hideValidatorCallout("useMail");
            var enabled = $(this).is(":checked");
            $("[useMail]").each(function () {
                $(this).attr('disabled', !enabled);
            });

            $("[useMailAuthorization]").each(function () {
                $(this).attr('disabled', !(enabled && $(cboxUseMailAuthorization).is(":checked")));
            });
        });

        $(document).on("click", cboxUseMailAuthorization, function () {
            hideValidatorCallout("useMail");
            var enabled = $(this).is(":checked");
            $("[useMailAuthorization]").each(function () {
                $(this).attr('disabled', !enabled);
            });
        });

        $(document).on("click", cboxUseJabber, function () {
            hideValidatorCallout("useJabber");
            var enabled = $(this).is(":checked");
            $("[useJabber]").each(function () {
                $(this).attr('disabled', !enabled);
            });
        });

        $(document).on("click", cboxUseFlowAnalysis, function () {
            hideValidatorCallout("useFlowAnalysis");
            var enabled = $(this).is(":checked");
            $("[useFlowAnalysis]").each(function () {
                $(this).attr('disabled', !enabled);
            });
        });

        $(document).on("click", "img[eid]", function () {
            var img = $(this);
            var id = img.attr("eid");
            var isEnabled = (img.attr("state") == "Enabled");

            $.ajax({
                type: "POST",
                url: "Notification.aspx/UpdateNotify",
                dataType: "json",
                data: "{id:" + id + ",isEnabled:" + isEnabled + "}",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    var newState = isEnabled ? "Disabled" : "Enabled";
                    img.attr("state", newState);
                    img.attr("src", "App_Themes/Main/Images/" + newState + ".gif");
                    img.attr("title", isEnabled ? Resource.Enable : Resource.Disable);
                },
                error: function (msg) { ShowJSONMessage(msg); }
            });
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

    function hideValidatorCallout(useAttr) {
        $("table[class='ajax__validatorcallout ajax__validatorcallout_popup_table']").each(function () {
            $find($(this).attr("id").replace("_popupTable", "")).hide();
        });
        $("[" + useAttr + "]").each(function () {
            $(this).attr("class", "textbox");
        });
    }

    function SaveAll() {
        var valid = true;
        if ($(cboxUseMail).is(":checked")) {
            if (!Page_ClientValidate('MailValidator'))
                valid = false;

            if ($(cboxUseMailAuthorization).is(":checked")) {
                if (!Page_ClientValidate('MailAuthorizationValidator'))
                    valid = false;
            }
        }

        if ($(cboxUseJabber).is(":checked")) {
            if (!Page_ClientValidate('JabberValidator'))
                valid = false;
        }

        if ($(cboxUseFlowAnalysis).is(":checked")) {
            if (!Page_ClientValidate('FlowAnalysisValidator'))
                valid = false;
        }

        return valid;
    }
     
</script>

<div class="title"><%=Resources.Resource.Vba32NSSettings%></div>

<div id='tabs'>
    <ul>
        <li><a href='#0'><span><%=Resources.Resource.Mail %></span></a></li>
        <li><a href='#1'><span><%=Resources.Resource.Jabber %></span></a></li>
        <li><a href='#2'><span><%=Resources.Resource.Notification %></span></a></li>
        <li><a href='#3'><span><%=Resources.Resource.RegisteredEvents%></span></a></li>
    </ul>

    <div id='0'>
        <table  class="ListContrastTable" style="width:500px;" >
            <tr>
                <td colspan="3" style="padding-top: 5px; padding-left: 5px;">
                    <asp:CheckBox runat="server" ID="cboxUseMail" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 25px;">
                    <%=Resources.Resource.MailServer %>:
                    <span style="color: Red;">*</span>
                </td>
                <td align="left" style="padding-top: 5px;">
                    <asp:TextBox ID="tboxMailServer" runat="server" useMail></asp:TextBox>
                    
                    <asp:RequiredFieldValidator runat="server" ID="requiredMailServer" ControlToValidate="tboxMailServer"
                        ErrorMessage='<%$ Resources:Resource, ServerRequired %>' Display="None" ValidationGroup="MailValidator">
                    </asp:RequiredFieldValidator>
                    
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="requiredMailServerCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredMailServer" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>                    
                </td>
            </tr>
            <tr>
                <td style="padding-left: 25px;">
                    <%=Resources.Resource.Port %>:                    
                </td>
                <td align="left" style="padding-top: 5px;">
                    <asp:TextBox ID="tboxMailPort" runat="server" useMail style="width: 50px;"></asp:TextBox>
                    
                    <asp:RangeValidator runat="server" ID="rangeMailPort" ControlToValidate="tboxMailPort" Type="Integer" MinimumValue="1" MaximumValue="9999"
                         Display="None" ValidationGroup="MailValidator">
                    </asp:RangeValidator>
                    
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="rangeMailPortCallout" HighlightCssClass="highlight" 
                        TargetControlID="rangeMailPort" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>                    
                </td>
            </tr>
            <tr>
                <td style="padding-left: 25px;">
                    <%=Resources.Resource.MailFrom %>:
                    <span style="color: Red;">*</span>
                </td>
                <td align="left" style="padding-top: 5px;">
                    <asp:TextBox ID="tboxMailFrom" runat="server" useMail></asp:TextBox>
                    
                    <asp:RequiredFieldValidator runat="server" ID="requiredMailFrom" ControlToValidate="tboxMailFrom"
                        ErrorMessage='<%$ Resources:Resource, SenderRequired %>' Display="None" ValidationGroup="MailValidator">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="regularMailFrom" ControlToValidate="tboxMailFrom"
                        ErrorMessage='<%$ Resources:Resource, ErrorInvalidEmail %>' Display="None" ValidationGroup="MailValidator" >
                    </asp:RegularExpressionValidator> 

                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="requiredMailFromCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredMailFrom" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="regularMailFromCallout" HighlightCssClass="highlight" 
                        TargetControlID="regularMailFrom" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 25px;">
                    <%=Resources.Resource.MailDisplayName %>:
                </td> 
                <td align="left" style="padding-top: 5px;padding-bottom: 5px;">
                    <asp:TextBox ID="tboxMailDisplayName" runat="server" useMail></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 25px;padding-top: 5px;padding-bottom: 5px;" colspan="2">
                    <input id="cboxEnableSsl" runat="server" type="checkbox" useMail />&nbsp;<%=Resources.Resource.EnableSsl%>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 25px;padding-top: 5px;padding-bottom: 5px;" colspan="2">
                    <input id="cboxAuthorizationEnabled" runat="server" type="checkbox" useMail />&nbsp;<%=Resources.Resource.UseAuthorization%>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 45px;">
                    <%=Resources.Resource.User%>
                </td>
                <td align="left" style="padding-top: 5px;padding-bottom: 5px;">
                    <asp:TextBox runat="server" ID="tboxAuthorizationUserName" autocomplete="off" useMailAuthorization />

                    <asp:RequiredFieldValidator runat="server" ID="requiredMailUsername" ControlToValidate="tboxAuthorizationUserName"
                        ErrorMessage='<%$ Resources:Resource, UserNameRequiredErrorMessage %>' Display="None" ValidationGroup="MailAuthorizationValidator">
                    </asp:RequiredFieldValidator>
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="regularMailUsernameCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredMailUsername" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                </td>
            </tr>
            <tr>                
                <td style="padding-left: 45px;">
                    <%=Resources.Resource.PasswordLabelText%>
                </td>
                <td align="left" style="padding-top: 5px;padding-bottom: 5px;">
                    <asp:TextBox runat="server" ID="tboxAuthorizationPassword" TextMode="Password" autocomplete="off" useMailAuthorization />
                </td>
            </tr>
        </table>
    </div>
    
    <div id='1'>
        <table class="ListContrastTable" style="width:500px" >
            <tr>
                <td colspan="2" style="padding-top: 5px; padding-left: 5px;">
                    <asp:CheckBox runat="server" ID="cboxUseJabber" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 25px;">
                    <%=Resources.Resource.JabberServer %>:
                    <span style="color: Red;">*</span>
                </td>
                <td align="center" style="padding-top: 5px;">
                    <asp:TextBox ID="tboxJabberServer" runat="server" useJabber></asp:TextBox>
                    
                    <asp:RequiredFieldValidator runat="server" ID="requiredJabberServer" ControlToValidate="tboxJabberServer"
                        ErrorMessage='<%$ Resources:Resource, ServerRequired %>' Display="None" ValidationGroup="JabberValidator">
                    </asp:RequiredFieldValidator>
                    
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="requiredJabberServerCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredJabberServer" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>                    
                </td>
            </tr>
            <tr>
                <td style="padding-left: 25px;">
                    <%=Resources.Resource.JabberFrom %>:
                    <span style="color: Red;">*</span>
                </td>
                <td align="center" style="padding-top: 5px;">
                    <asp:TextBox ID="tboxJabberFrom" runat="server" useJabber></asp:TextBox>

                    <asp:RequiredFieldValidator runat="server" ID="requiredJabberFrom" ControlToValidate="tboxJabberFrom"
                        ErrorMessage='<%$ Resources:Resource, SenderRequired %>' Display="None" ValidationGroup="JabberValidator">
                    </asp:RequiredFieldValidator>
                    
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="requiredJabberFromCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredJabberFrom" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 25px;">
                    <%=Resources.Resource.JabberPassword %>:
                    <span style="color: Red;">*</span>
                </td>
                <td align="center" style="padding-top: 5px;padding-bottom: 5px;">
                    <asp:TextBox ID="tboxJabberPassword" runat="server" TextMode="Password" useJabber></asp:TextBox>

                    <asp:RequiredFieldValidator runat="server" ID="requiredJabberPassword" ControlToValidate="tboxJabberPassword"
                        ErrorMessage='<%$ Resources:Resource, PasswordRequiredErrorMessage %>' Display="None" ValidationGroup="JabberValidator">
                    </asp:RequiredFieldValidator>
                    
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="requiredJabberPasswordCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredJabberPassword" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                </td>
            </tr>
        </table>
    </div>
    
    <div id='2'>
        <table class="ListContrastTable" style="width:500px">
            <tr>
                <td colspan="4" style="padding-top: 5px; padding-left: 5px;">
                    <asp:CheckBox ID="cboxUseFlowAnalysis" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 25px;"></td>
                <td style="padding-top: 10px;">
                    <%=Resources.Resource.EventsCount %>
                </td>
                <td style="padding-top: 10px;">
                    <%=Resources.Resource.TimeInterval %>
                </td>
                <td style="padding-top: 10px;">
                    <%=Resources.Resource.ComputersCount %>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 25px;">
                    <%=Resources.Resource.Epidemy %>
                </td>
                <td>
                    <asp:TextBox ID="tboxGlobalEpidemyLimit" style="width:40px" runat="server" useFlowAnalysis />

                    <asp:RequiredFieldValidator runat="server" ID="requiredGlobalEpidemyLimit" ControlToValidate="tboxGlobalEpidemyLimit"
                        ErrorMessage='<%$ Resources:Resource, ValueRequired %>' Display="None" ValidationGroup="FlowAnalysisValidator">
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator runat="server" ID="rangeGlobalEpidemyLimit" ControlToValidate="tboxGlobalEpidemyLimit"
                        Type="Integer" MinimumValue="0" MaximumValue="1000" Display="None" ValidationGroup="FlowAnalysisValidator" >
                    </asp:RangeValidator>                    
                    
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="requiredGlobalEpidemyLimitCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredGlobalEpidemyLimit" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="rangeGlobalEpidemyLimitCallout" HighlightCssClass="highlight" 
                        TargetControlID="rangeGlobalEpidemyLimit" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                </td>
                <td>
                    <asp:TextBox ID="tboxGlobalEpidemyTimeLimit" style="width:40px" runat="server" useFlowAnalysis />

                    <asp:RequiredFieldValidator runat="server" ID="requiredGlobalEpidemyTimeLimit" ControlToValidate="tboxGlobalEpidemyTimeLimit"
                        ErrorMessage='<%$ Resources:Resource, ValueRequired %>' Display="None" ValidationGroup="FlowAnalysisValidator">
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator runat="server" ID="rangeGlobalEpidemyTimeLimit" ControlToValidate="tboxGlobalEpidemyTimeLimit"
                        Type="Integer" MinimumValue="0" MaximumValue="1000" Display="None" ValidationGroup="FlowAnalysisValidator" >
                    </asp:RangeValidator>                    
                    
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="requiredGlobalEpidemyTimeLimitCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredGlobalEpidemyTimeLimit" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="rangeGlobalEpidemyTimeLimitCallout" HighlightCssClass="highlight" 
                        TargetControlID="rangeGlobalEpidemyTimeLimit" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                </td>
                <td>
                    <asp:TextBox ID="tboxGlobalEpidemyCompCount" style="width:40px" runat="server" useFlowAnalysis />

                    <asp:RequiredFieldValidator runat="server" ID="requiredGlobalEpidemyCompCount" ControlToValidate="tboxGlobalEpidemyCompCount"
                        ErrorMessage='<%$ Resources:Resource, ValueRequired %>' Display="None" ValidationGroup="FlowAnalysisValidator">
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator runat="server" ID="rangeGlobalEpidemyCompCount" ControlToValidate="tboxGlobalEpidemyCompCount"
                        Type="Integer" MinimumValue="0" MaximumValue="1000" Display="None" ValidationGroup="FlowAnalysisValidator" >
                    </asp:RangeValidator>                    
                    
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="requiredGlobalEpidemyCompCountCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredGlobalEpidemyCompCount" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="rangeGlobalEpidemyCompCountCallout" HighlightCssClass="highlight" 
                        TargetControlID="rangeGlobalEpidemyCompCount" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 25px;">
                    <%=Resources.Resource.LocalHearth %>
                </td>
                <td>
                    <asp:TextBox ID="tboxLocalHearthLimit" style="width:40px" runat="server" useFlowAnalysis />

                    <asp:RequiredFieldValidator runat="server" ID="requiredLocalHearthLimit" ControlToValidate="tboxLocalHearthLimit"
                        ErrorMessage='<%$ Resources:Resource, ValueRequired %>' Display="None" ValidationGroup="FlowAnalysisValidator">
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator runat="server" ID="rangeLocalHearthLimit" ControlToValidate="tboxLocalHearthLimit"
                        Type="Integer" MinimumValue="0" MaximumValue="1000" Display="None" ValidationGroup="FlowAnalysisValidator" >
                    </asp:RangeValidator>                    
                    
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="requiredLocalHearthLimitCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredLocalHearthLimit" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="rangeLocalHearthLimitCallout" HighlightCssClass="highlight" 
                        TargetControlID="rangeLocalHearthLimit" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                </td>
                <td>
                    <asp:TextBox ID="tboxLocalHearthTimeLimit" style="width:40px" runat="server" useFlowAnalysis />

                    <asp:RequiredFieldValidator runat="server" ID="requiredLocalHearthTimeLimit" ControlToValidate="tboxLocalHearthTimeLimit"
                        ErrorMessage='<%$ Resources:Resource, ValueRequired %>' Display="None" ValidationGroup="FlowAnalysisValidator">
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator runat="server" ID="rangeLocalHearthTimeLimit" ControlToValidate="tboxLocalHearthTimeLimit"
                        Type="Integer" MinimumValue="0" MaximumValue="1000" Display="None" ValidationGroup="FlowAnalysisValidator" >
                    </asp:RangeValidator>                    
                    
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="requiredLocalHearthTimeLimitCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredLocalHearthTimeLimit" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="rangeLocalHearthTimeLimitCallout" HighlightCssClass="highlight" 
                        TargetControlID="rangeLocalHearthTimeLimit" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                </td>
                <td></td>
            </tr>
            <tr>
                <td style="padding-left: 25px;">
                    <%=Resources.Resource.EventFlow%>
                </td>
                <td style="padding-bottom: 5px;">
                    <asp:TextBox ID="tboxLimit" style="width:40px" runat="server" useFlowAnalysis />

                    <asp:RequiredFieldValidator runat="server" ID="requiredLimit" ControlToValidate="tboxLimit"
                        ErrorMessage='<%$ Resources:Resource, ValueRequired %>' Display="None" ValidationGroup="FlowAnalysisValidator">
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator runat="server" ID="rangeLimit" ControlToValidate="tboxLimit"
                        Type="Integer" MinimumValue="0" MaximumValue="1000" Display="None" ValidationGroup="FlowAnalysisValidator" >
                    </asp:RangeValidator>                    
                    
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="requiredLimitCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredLimit" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="rangeLimitCallout" HighlightCssClass="highlight" 
                        TargetControlID="rangeLimit" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                </td>
                <td style="padding-bottom: 5px;">
                    <asp:TextBox ID="tboxTimeLimit" style="width:40px" runat="server" useFlowAnalysis />

                    <asp:RequiredFieldValidator runat="server" ID="requiredTimeLimit" ControlToValidate="tboxTimeLimit"
                        ErrorMessage='<%$ Resources:Resource, ValueRequired %>' Display="None" ValidationGroup="FlowAnalysisValidator">
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator runat="server" ID="rangeTimeLimit" ControlToValidate="tboxTimeLimit"
                        Type="Integer" MinimumValue="0" MaximumValue="1000" Display="None" ValidationGroup="FlowAnalysisValidator" >
                    </asp:RangeValidator>                    
                    
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="requiredTimeLimitCallout" HighlightCssClass="highlight" 
                        TargetControlID="requiredTimeLimit" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="rangeTimeLimitCallout" HighlightCssClass="highlight" 
                        TargetControlID="rangeTimeLimit" PopupPosition="Right">
                    </ajaxToolkit:ValidatorCalloutExtender2>
                </td>
                <td></td>
            </tr>
        </table>
    </div>

    <div id='3'>
        <table class="ListContrastTable" style="width:500px">
            <tr>
	            <td valign="top">
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" SelectCountMethod="CountForNotification"
                        SelectMethod="GetForNotification" TypeName="EventsDataContainer" SortParameterName="SortExpression">
                    </asp:ObjectDataSource>

                    <asp:UpdatePanel runat="server" ID="upnlEvents">
                    <ContentTemplate>
                        <custom:GridViewExtended ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                            AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
                            EnableModelValidation="True" CssClass="gridViewStyle" EmptyDataText='<%$ Resources:Resource, EmptyMessage %>'>
                            <Columns>
                                <asp:TemplateField HeaderText='<%$Resources:Resource, Notify %>' SortExpression="Notify">
                                    <HeaderStyle Width="80px" />
                                    <ItemTemplate>
                                        <img id="ibtnNotify" runat="server" style="cursor: pointer;" alt="" src="" eid='<%# DataBinder.Eval(Container.DataItem, "ID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='<%$Resources:Resource, EventName %>' SortExpression="EventName">
                                    <ItemTemplate>
					                    <asp:LinkButton id="lbtnEventName" CommandName="SelectCommand" CommandArgument="EventName" runat="server" Text='<%# DatabaseNameLocalization.GetNameForCurrentCulture(DataBinder.Eval(Container.DataItem, "EventName").ToString())%>' EventName='<%# DataBinder.Eval(Container.DataItem, "EventName")%>' SkinId="LabelContrast"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="EventName" SortExpression="EventName" HeaderText='<%$ Resources:Resource, EventName %>'>
                                    <HeaderStyle Width="150px" />
                                </asp:BoundField>
                            </Columns>
                            <PagerSettings Position="TopAndBottom" Visible="true" />
                            <PagerTemplate>
                                <paging:Paging runat="server" ID="Paging1" />
                            </PagerTemplate>
                            <HeaderStyle CssClass="gridViewHeader" ForeColor="White" />
                            <AlternatingRowStyle CssClass="gridViewRowAlternating" />
                            <RowStyle CssClass="gridViewRow" />
                        </custom:GridViewExtended>

                        <asp:Panel runat="server" ID="pnlModalPopap" Style="display: none" CssClass="modalPopupNotify">
                            <div runat="server" id="divPopupHeader" class="modalPopupNotifyHeader">
                                <asp:Label ID="lblSelectedEventName" SkinID="SubSectionLabel" style="font-size: 14px; font-weight:bold" runat="server" />
                            </div>    
		                            <NotifyCnfg:Notify ID="notify" runat="server" />			    
                            <div style="vertical-align:middle; height:50px">            
                                <asp:LinkButton runat="server" ID="lbtnSave" SkinID="Button" OnClick="lbtnSave_Click" >
                                    <%=Resources.Resource.Save%>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnCancel" runat="server" SkinID="Button">
                                    <%=Resources.Resource.Close%>
                                </asp:LinkButton>
                                <asp:Button ID="btnModalPopup" runat="server" Style="visibility:hidden" />            
                            </div>
                        </asp:Panel>
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnModalPopup" X="400" Y="200" PopupControlID="pnlModalPopap" CancelControlID="btnCancel" BackgroundCssClass="modalBackground" PopupDragHandleControlID="divPopupHeader" />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    
    <asp:UpdatePanel runat="server" ID="upnlSave">
    <ContentTemplate>
        <asp:LinkButton ID="lbtnSaveAll" runat="server" OnClientClick="return SaveAll();" OnClick="lbtnSaveAll_Click" SkinID="Button" >
            <%=Resources.Resource.Save%>
        </asp:LinkButton>
    </ContentTemplate>
    </asp:UpdatePanel>

</div>

</asp:Content>

