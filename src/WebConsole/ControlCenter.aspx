<%@ Page Language="C#" validateRequest="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="ControlCenter.aspx.cs" Inherits="ControlCenter" Title="Untitled Page" %>
<%@ Register Assembly="PagingControl" Namespace="PagingControls" TagPrefix="cc1" %>

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
                <td>
                    <asp:Label ID="lblNextSendDateTitle" runat="server"><%=Resources.Resource.NextSend%></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblNextSendDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                   <asp:Label ID="lblLastSendDateTitle" runat="server"><%=Resources.Resource.LastSend%></asp:Label>
                </td>
                 <td>
                    <asp:Label ID="lblLastSendDate" runat="server"></asp:Label>
                </td>
            </tr>
             <tr>
                <td>
           
                   <asp:Label ID="lblLastSelectDateTitle" runat="server"><%=Resources.Resource.LastSelect%></asp:Label>
                </td>
                 <td>
                    <asp:Label ID="lblLastSelectDate" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
      </div>
      <div id='1'>
        <table class="ListContrastTableMain">
			<tr>
				<td><cc1:PagingControl ID="pcPagingTop" runat="server" OnNextPage="pcPaging_NextPage" OnPrevPage="pcPaging_PrevPage" OnHomePage="pcPaging_HomePage" OnLastPage="pcPaging_LastPage"/>
				<asp:datalist id="dlEvents" Width="100%" border="0" runat="server" Height="80px" OnItemCommand="dlEvents_ItemCommand" OnItemDataBound="dlEvents_ItemDataBound">
						<HeaderTemplate>
							<tr class="subsection" >
							<td style="width: 100px;">
                                <asp:LinkButton ID="lbtnSend" runat="server" CommandName="SortCommand" CommandArgument="Send"></asp:LinkButton>
							</td>
							<td style="width: 100px;">
							    <asp:LinkButton ID="lbtnNoDelete" runat="server" CommandName="SortCommand" CommandArgument="NoDelete"></asp:LinkButton>
							</td>
							<td>
							    <asp:LinkButton ID="lbtnEventName" runat="server" CommandName="SortCommand" CommandArgument="EventName"></asp:LinkButton>
							</td>
							</tr>
						</HeaderTemplate>
						<ItemTemplate>
							<tr>
							<td align="center">
								<asp:Label id="lblID" runat="server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "ID")%>'></asp:Label>
								<asp:ImageButton ID="ibtnSend" runat="server" CommandName="SelectCommand" CommandArgument="Send" />
							</td>
							<td align="center">
								<asp:ImageButton ID="ibtnNoDelete" runat="server" CommandName="SelectCommand" CommandArgument="NoDelete" />
							</td>
							<td>								
								<asp:Label id="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EventName")%>' SkinId="LabelContrast"></asp:Label>
							</td>
						    </tr>
						</ItemTemplate>
					</asp:datalist>  
					<cc1:PagingControl ID="pcPaging" runat="server" OnNextPage="pcPaging_NextPage" OnPrevPage="pcPaging_PrevPage" OnHomePage="pcPaging_HomePage" OnLastPage="pcPaging_LastPage"/>          
				</td>
			</tr>
		</table>
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
                        <ajaxToolkit:ValidatorCalloutExtender2 ID="RequiredDeliveryTimeoutCheckEx" runat="server" TargetControlID="RequiredDeliveryTimeoutCheck"
                            HighlightCssClass="highlight">
                        </ajaxToolkit:ValidatorCalloutExtender2>
                        <ajaxToolkit:ValidatorCalloutExtender2 ID="RangeDeliveryTimeoutCheckEx" runat="server" TargetControlID="RangeDeliveryTimeoutCheck"
                            HighlightCssClass="highlight">
                        </ajaxToolkit:ValidatorCalloutExtender2>
                   </td>
               </tr>
               <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="cboxMaintenanceEnabled" runat="server" onclick="HideFields()"  />
                    </td>
               </tr>
               <tr>
                   <td style="padding-left: 30px">
                        <asp:Label ID="lblServer" runat="server"><%=Resources.Resource.Server %>, <%= Resources.Resource.IPAddress %></asp:Label>            
                   </td>     
                   <td>
                        <asp:TextBox ID="tboxServer" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredServer" ControlToValidate="tboxServer"
                            Display="None" runat="server" ErrorMessage='<%$ Resources:Resource, RequiredField %>'
                            ValidationGroup="MaintenanceSettingsValidation">
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionServer" runat="server" ControlToValidate="tboxServer"
                            Display="None" ValidationGroup="MaintenanceSettingsValidation" ErrorMessage='<%$ Resources:Resource, IPAddressRegexErrorMessage %>'>
                        </asp:RegularExpressionValidator>
                        <ajaxToolkit:ValidatorCalloutExtender2 ID="RequiredServerEx" runat="server" TargetControlID="RequiredServer"
                            HighlightCssClass="highlight">
                        </ajaxToolkit:ValidatorCalloutExtender2>
                        <ajaxToolkit:ValidatorCalloutExtender2 ID="RegularExpressionServerEx" runat="server" TargetControlID="RegularExpressionServer"
                            HighlightCssClass="highlight">
                        </ajaxToolkit:ValidatorCalloutExtender2>
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
                   <td>       
                         <asp:Label ID="lblDaysToDelete" runat="server"><%=Resources.Resource.DaysToDelete%></asp:Label>
                   </td>
                   <td>
                        <asp:TextBox ID="tboxDaysToDelete" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredDaysToDelete" ControlToValidate="tboxDaysToDelete"
                            Display="None" runat="server" ErrorMessage='<%$ Resources:Resource, RequiredField %>'
                            ValidationGroup="SettingsValidation">
                        </asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeDaysToDelete" runat="server" ControlToValidate="tboxDaysToDelete"
                            Display="None" ValidationGroup="SettingsValidation" MinimumValue="0" MaximumValue="360" Type="Integer">
                        </asp:RangeValidator>
                        <ajaxToolkit:ValidatorCalloutExtender2 ID="RequiredDaysToDeleteEx" runat="server" TargetControlID="RequiredDaysToDelete"
                            HighlightCssClass="highlight">
                        </ajaxToolkit:ValidatorCalloutExtender2>
                        <ajaxToolkit:ValidatorCalloutExtender2 ID="RangeDaysToDeleteEx" runat="server" TargetControlID="RangeDaysToDelete"
                            HighlightCssClass="highlight">
                        </ajaxToolkit:ValidatorCalloutExtender2>
                   </td>
               </tr>
               <tr>
                    <td>
                        <asp:Label ID="lblTasksDaysToDelete" runat="server"><%=Resources.Resource.TaskDaysToDelete%></asp:Label>            
                    </td>
                    <td>
                        <asp:TextBox ID="tboxTasksDaysToDelete" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredTasksDaysToDelete" ControlToValidate="tboxTasksDaysToDelete"
                            Display="None" runat="server" ErrorMessage='<%$ Resources:Resource, RequiredField %>'
                            ValidationGroup="SettingsValidation">
                        </asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeTasksDaysToDelete" runat="server" ControlToValidate="tboxTasksDaysToDelete"
                            Display="None" ValidationGroup="SettingsValidation" MinimumValue="0" MaximumValue="360" Type="Integer">
                        </asp:RangeValidator>
                        <ajaxToolkit:ValidatorCalloutExtender2 ID="RequiredTasksDaysToDeleteEx" runat="server" TargetControlID="RequiredTasksDaysToDelete"
                            HighlightCssClass="highlight">
                        </ajaxToolkit:ValidatorCalloutExtender2>
                        <ajaxToolkit:ValidatorCalloutExtender2 ID="RangeTasksDaysToDeleteEx" runat="server" TargetControlID="RangeTasksDaysToDelete"
                            HighlightCssClass="highlight">
                        </ajaxToolkit:ValidatorCalloutExtender2>
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
                        <ajaxToolkit:ValidatorCalloutExtender2 ID="requiredComputersDaysToDeleteExt" runat="server" TargetControlID="requiredComputersDaysToDelete"
                            HighlightCssClass="highlight">
                        </ajaxToolkit:ValidatorCalloutExtender2>
                        <ajaxToolkit:ValidatorCalloutExtender2 ID="rangeComputersDaysToDeleteExt" runat="server" TargetControlID="rangeComputersDaysToDelete"
                            HighlightCssClass="highlight">
                        </ajaxToolkit:ValidatorCalloutExtender2>
                    </td>
               </tr>
            </table>
            <br />
            <asp:LinkButton ID="lbtnSaveBoxes" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
             runat="server" OnClick="lbtnSaveBoxes_Click" OnClientClick="return validateSettings();"
             Style="padding: 5px; margin-top: 10px; margin-bottom: 10px; margin-left: 5px;width: 100px;">
                <%=Resources.Resource.Save%>
            </asp:LinkButton>
        </ContentTemplate>
        </asp:UpdatePanel>
      </div>
</div>
</asp:Content>

