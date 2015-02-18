<%@ Page Language="C#" validateRequest="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" Inherits="ControlCenter" Title="Untitled Page" Codebehind="ControlCenter.aspx.cs" %>
<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register TagPrefix="custom" Namespace="VirusBlokAda.CC.CustomControls" Assembly="CustomControls" %>

<%@ OutputCache Location="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
    <script type="text/javascript">
        $(document).ready(function(){
         $("#tabs").tabs({ cookie: { expires: 30 } });
         });

         function HideFields() {
             var cbox = $("#<%=cboxMaintenanceEnabled.ClientID%>").is(":checked");
             $("#<%=tboxServer.ClientID%>").attr("disabled", !cbox);
             $("#<%=ddlEvery.ClientID%>").attr("disabled", !cbox);
             $("#<%=ddlDay.ClientID%>").attr("disabled", !cbox);
             $("#<%=ddlTime.ClientID%>").attr("disabled", !cbox);
         }

         function validateSettings() {
             var result = Page_ClientValidate('SettingsValidation');

             if (result && $("#<%=cboxMaintenanceEnabled.ClientID%>").is(":checked"))
                 result = Page_ClientValidate('MaintenanceSettingsValidation');

             return result;
         }
    </script>
    <div class="title"><%=Resources.Resource.SettingsForMaintenanceService%></div>
    <div id='tabs'>
        <ul>
            <li><a href='#0'><span><%=Resources.Resource.Time%></span></a></li>
            <li><a href='#2'><span><%=Resources.Resource.Settings%></span></a></li>
            <li><a href='#1'><span><%= Resources.Resource.EventSendTableTitle%></span></a></li>
        </ul>
        <div id='0'>
            <table class="ListContrastTableMain">
                <tr>
                    <td><%=Resources.Resource.NextSend%></td>
                    <td>
                        <asp:Label ID="lblNextSendDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td><%=Resources.Resource.LastSend%></td>
                    <td>
                        <asp:Label ID="lblLastSendDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td><%=Resources.Resource.LastSelect%></td>
                    <td>
                        <asp:Label ID="lblLastSelectDate" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div id='1'>
            <div class="divSettings">
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True"
                    SelectCountMethod="CountForNotification" SelectMethod="GetForNotification" TypeName="EventsDataContainer" SortParameterName="SortExpression">
                </asp:ObjectDataSource>
        
                <asp:UpdatePanel runat="server" ID="updatePanelComponentsGrid">
                    <ContentTemplate>
                        <custom:GridViewExtended ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                            AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" OnRowDataBound="GridView1_RowDataBound"
                            EnableModelValidation="True" CssClass="gridViewStyle" OnRowCommand="GridView1_RowCommand"
                            StorageType="Session" StorageName="EventTypes" EmptyDataText='<%$ Resources:Resource, EmptyMessage %>'>
                            <Columns>     
                                <asp:TemplateField SortExpression="Send" HeaderText='<%$Resources:Resource, Send%>'>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnSend" runat="server" CommandName="Select" CommandArgument="Send" eventID='<%# DataBinder.Eval(Container.DataItem, "ID")%>' />
                                    </ItemTemplate>
                                    <HeaderStyle Width="70px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="NoDelete" HeaderText='<%$Resources:Resource, NoDelete%>'>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnNoDelete" runat="server" CommandName="Select" CommandArgument="NoDelete" eventID='<%# DataBinder.Eval(Container.DataItem, "ID")%>' />
                                    </ItemTemplate>
                                    <HeaderStyle Width="70px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="EventName" SortExpression="EventName" 
                                    HeaderText='<%$Resources:Resource, EventName%>'>
                                </asp:BoundField>
                            </Columns> 
                            <PagerSettings Position="TopAndBottom" Visible="true" />           
                            <PagerTemplate>
                                <paging:Paging runat="server" ID="Paging1" />
                            </PagerTemplate>
                            <HeaderStyle CssClass="gridViewHeader" ForeColor="White"  />
                            <AlternatingRowStyle CssClass = "gridViewRowAlternating" />
                            <RowStyle CssClass="gridViewRow" />
                        </custom:GridViewExtended>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div id='2'>
            <asp:UpdatePanel runat="server" ID="upnlSettings">
                <ContentTemplate>
                    <table class="ListContrastTableMain">
                        <tr>
                            <td style="width: 300px">
                                <asp:Label ID="lblMaintenanceIntervalTitle" runat="server"><%=Resources.Resource.MaintenanceInterval %></asp:Label>            
                            </td>     
                            <td>
                                <asp:TextBox ID="tboxDeliveryTimeoutCheck" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredDeliveryTimeoutCheck" ControlToValidate="tboxDeliveryTimeoutCheck"
                                    Display="None" runat="server" ErrorMessage='<%$ Resources:Resource, RequiredField %>'
                                    ValidationGroup="SettingsValidation">
                                </asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeDeliveryTimeoutCheck" runat="server" ControlToValidate="tboxDeliveryTimeoutCheck"
                                    Display="None" ValidationGroup="SettingsValidation" MinimumValue="60" MaximumValue="14400" Type="Integer">
                                </asp:RangeValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="RequiredDeliveryTimeoutCheckEx" runat="server" TargetControlID="RequiredDeliveryTimeoutCheck"
                                    HighlightCssClass="highlight">
                                </ajaxToolkit:ValidatorCalloutExtender>
                                <ajaxToolkit:ValidatorCalloutExtender ID="RangeDeliveryTimeoutCheckEx" runat="server" TargetControlID="RangeDeliveryTimeoutCheck"
                                    HighlightCssClass="highlight">
                                </ajaxToolkit:ValidatorCalloutExtender>
                           </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="cboxMaintenanceEnabled" runat="server" onclick="HideFields()"  />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 30px"><%=Resources.Resource.Server %>, <%= Resources.Resource.IPAddress %></td>     
                            <td>
                                <asp:TextBox ID="tboxServer" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredServer" ControlToValidate="tboxServer"
                                    Display="None" runat="server" ErrorMessage='<%$ Resources:Resource, RequiredField %>'
                                    ValidationGroup="MaintenanceSettingsValidation">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionServer" runat="server" ControlToValidate="tboxServer"
                                    Display="None" ValidationGroup="MaintenanceSettingsValidation" ErrorMessage='<%$ Resources:Resource, IPAddressRegexErrorMessage %>'>
                                </asp:RegularExpressionValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="RequiredServerEx" runat="server" TargetControlID="RequiredServer"
                                    HighlightCssClass="highlight">
                                </ajaxToolkit:ValidatorCalloutExtender>
                                <ajaxToolkit:ValidatorCalloutExtender ID="RegularExpressionServerEx" runat="server" TargetControlID="RegularExpressionServer"
                                    HighlightCssClass="highlight">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 30px">
                                <asp:Label ID="lblSendDataInterval" runat="server"><%=Resources.Resource.SendDataInterval %></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlEvery" runat="server" style="width:auto;"  AutoPostBack="true" OnSelectedIndexChanged="ddlEvery_SelectedIndexChanged" />
                                <asp:DropDownList ID="ddlDay" runat="server" style="width:auto;"  Visible="false" />
                                &nbsp;
                                <asp:Label runat="server" ID="lblIn"><%=Resources.Resource.In%></asp:Label>            
                                <asp:DropDownList ID="ddlTime" runat="server"  style="width:auto;"  />
                                <%=Resources.Resource.Hours %>
                            </td>     
                        </tr>
                        <tr>
                            <td><%=Resources.Resource.DaysToDelete%></td>
                            <td>
                                <asp:TextBox ID="tboxDaysToDelete" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredDaysToDelete" ControlToValidate="tboxDaysToDelete"
                                    Display="None" runat="server" ErrorMessage='<%$ Resources:Resource, RequiredField %>'
                                    ValidationGroup="SettingsValidation">
                                </asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeDaysToDelete" runat="server" ControlToValidate="tboxDaysToDelete"
                                    Display="None" ValidationGroup="SettingsValidation" MinimumValue="0" MaximumValue="360" Type="Integer">
                                </asp:RangeValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="RequiredDaysToDeleteEx" runat="server" TargetControlID="RequiredDaysToDelete"
                                    HighlightCssClass="highlight">
                                </ajaxToolkit:ValidatorCalloutExtender>
                                <ajaxToolkit:ValidatorCalloutExtender ID="RangeDaysToDeleteEx" runat="server" TargetControlID="RangeDaysToDelete"
                                    HighlightCssClass="highlight">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td><%=Resources.Resource.TaskDaysToDelete%> </td>
                            <td>
                                <asp:TextBox ID="tboxTasksDaysToDelete" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredTasksDaysToDelete" ControlToValidate="tboxTasksDaysToDelete"
                                    Display="None" runat="server" ErrorMessage='<%$ Resources:Resource, RequiredField %>'
                                    ValidationGroup="SettingsValidation">
                                </asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeTasksDaysToDelete" runat="server" ControlToValidate="tboxTasksDaysToDelete"
                                    Display="None" ValidationGroup="SettingsValidation" MinimumValue="0" MaximumValue="360" Type="Integer">
                                </asp:RangeValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="RequiredTasksDaysToDeleteEx" runat="server" TargetControlID="RequiredTasksDaysToDelete"
                                    HighlightCssClass="highlight">
                                </ajaxToolkit:ValidatorCalloutExtender>
                                <ajaxToolkit:ValidatorCalloutExtender ID="RangeTasksDaysToDeleteEx" runat="server" TargetControlID="RangeTasksDaysToDelete"
                                    HighlightCssClass="highlight">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblComputersDaysToDelete" runat="server"><%=Resources.Resource.ComputersDaysToDelete%></asp:Label>            
                            </td>
                            <td>
                                <asp:TextBox ID="tboxComputersDaysToDelete" runat="server" />
                                <asp:RequiredFieldValidator ID="requiredComputersDaysToDelete" ControlToValidate="tboxComputersDaysToDelete"
                                    Display="None" runat="server" ErrorMessage='<%$ Resources:Resource, RequiredField %>'
                                    ValidationGroup="SettingsValidation">
                                </asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rangeComputersDaysToDelete" runat="server" ControlToValidate="tboxComputersDaysToDelete"
                                    Display="None" ValidationGroup="SettingsValidation" MinimumValue="0" MaximumValue="360" Type="Integer">
                                </asp:RangeValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="requiredComputersDaysToDeleteExt" runat="server" TargetControlID="requiredComputersDaysToDelete"
                                    HighlightCssClass="highlight">
                                </ajaxToolkit:ValidatorCalloutExtender>
                                <ajaxToolkit:ValidatorCalloutExtender ID="rangeComputersDaysToDeleteExt" runat="server" TargetControlID="rangeComputersDaysToDelete"
                                    HighlightCssClass="highlight">
                                </ajaxToolkit:ValidatorCalloutExtender>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:LinkButton ID="lbtnSaveBoxes" SkinID="Button" runat="server" OnClick="lbtnSaveBoxes_Click" OnClientClick="return validateSettings();">
                        <%=Resources.Resource.Save%>
                    </asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>