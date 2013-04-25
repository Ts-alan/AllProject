<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrimitiveFilterDateTime.ascx.cs"
    Inherits="Controls_PrimitiveFilterDateTime" %>
<%@ Register Src="~/Controls/PrimitiveFilterTemplate.ascx" TagName="PrimitiveFilterTemplate"
    TagPrefix="flt" %>
<flt:PrimitiveFilterTemplate runat="server" ID="fltTemplate" TextFilter="Name" Height="75px">
    <FilterTemplate>
        <div style="padding-left: 3px; width: 250px;">
            <table>
                <tr>
                    <td style="width: 20px;">
                        <input id="rbtnDateInterval" type="radio" runat="server" name="DateTimeGroup" checked="true"/>
                    </td>
                    <td>
                        <asp:Panel ID="panDateInterval" runat="server">
                            <div style="margin: 1px;">
                                <asp:TextBox runat="server" ID="tboxDateFrom" Style="width: 80px; height: 17px;"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender runat="server" ID="CalendarExtender1" FirstDayOfWeek="Monday"
                                    Format="dd.MM.yyyy" TargetControlID="tboxDateFrom" />
                                <ajaxToolkit:MaskedEditExtender ID="meDateFrom" runat="server" TargetControlID="tboxDateFrom"
                                    Mask="99/99/9999" MaskType="Date" />
                                <ajaxToolkit:MaskedEditValidator ID="valDateFrom" runat="server" ControlExtender="meDateFrom"
                                    ControlToValidate="tboxDateFrom" Display="None" IsValidEmpty="True" ValidationGroup="FilterValidation">
                                </ajaxToolkit:MaskedEditValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="valDateFromExt" runat="server" TargetControlID="valDateFrom"
                                    HighlightCssClass="highlight" />
                                &nbsp;
                                <asp:TextBox runat="server" ID="tboxTimeFrom" Style="width: 35px;height: 17px;"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="meTimeFrom" runat="server" AcceptAMPM="false"
                                    Mask="99:99" MaskType="Time" TargetControlID="tboxTimeFrom">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:MaskedEditValidator ID="valTimeFrom" runat="server" ControlExtender="meTimeFrom"
                                    ControlToValidate="tboxTimeFrom" Display="none" IsValidEmpty="True" MinimumValue="00:00"
                                    MaximumValue="23:59" ValidationGroup="FilterValidation">
                                </ajaxToolkit:MaskedEditValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="valTimeFromExt" runat="server" TargetControlID="valTimeFrom"
                                    HighlightCssClass="highlight" />
                                <asp:CompareValidator runat="server" ID="cmpDate" Operator="LessThan" Type="Date"
                                    ControlToValidate="tboxDateFrom" ControlToCompare="tboxDateTo" Display="None"
                                    ValidationGroup="FilterValidation"></asp:CompareValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="vlaCmpDate" runat="server" TargetControlID="cmpDate"
                                    HighlightCssClass="highlight" />
                            </div>
                            <div style="margin: 1px;">
                                <asp:TextBox runat="server" ID="tboxDateTo" Style="width: 80px;height: 17px;"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender runat="server" ID="CalendarExtender2" FirstDayOfWeek="Monday"
                                    Format="dd.MM.yyyy" TargetControlID="tboxDateTo" />
                                <ajaxToolkit:MaskedEditExtender ID="meDateTo" runat="server" TargetControlID="tboxDateTo"
                                    Mask="99/99/9999" MaskType="Date" />
                                <ajaxToolkit:MaskedEditValidator ID="valDateTo" runat="server" ControlExtender="meDateTo"
                                    ControlToValidate="tboxDateTo" Display="None" IsValidEmpty="True" ValidationGroup="FilterValidation">
                                </ajaxToolkit:MaskedEditValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="valDateToExt" runat="server" TargetControlID="valDateTo"
                                    HighlightCssClass="highlight" />
                                &nbsp;
                                <asp:TextBox runat="server" ID="tboxTimeTo" Style="width: 35px;height: 17px;"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="meTimeTo" runat="server" AcceptAMPM="false" Mask="99:99"
                                    MaskType="Time" TargetControlID="tboxTimeTo">
                                </ajaxToolkit:MaskedEditExtender>
                                <ajaxToolkit:MaskedEditValidator ID="valTimeTo" runat="server" ControlExtender="meTimeTo"
                                    ControlToValidate="tboxTimeTo" Display="none" IsValidEmpty="True" MinimumValue="00:00"
                                    MaximumValue="23:59" ValidationGroup="FilterValidation">
                                </ajaxToolkit:MaskedEditValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="valTimeToExt" runat="server" TargetControlID="valTimeTo"
                                    HighlightCssClass="highlight" />
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input id="rbtnDateDeclination" type="radio" runat="server" name="DateTimeGroup"/>
                    </td>
                    <td>
                        <asp:Panel ID="panDateDeclination" runat="server">
                            <div>
                                <asp:DropDownList runat="server" ID="ddlDeclinationDirection" Style="width: 70px;">
                                </asp:DropDownList>
                                <asp:TextBox runat="server" ID="tboxDeclinationValue" Style="width: 30px; height: 17px; margin-left: 3px;
                                    margin-right: 1px;"></asp:TextBox>
                                <asp:DropDownList runat="server" ID="ddlIntervalType" Style="width: 80px;">
                                </asp:DropDownList>
                                <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="fDeclinationValue" FilterType="Numbers"
                                    TargetControlID="tboxDeclinationValue">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RangeValidator runat="server" ID="valDeclinationValue" Type="Integer" MinimumValue="1"
                                    MaximumValue="100" ValidationGroup="FilterValidation" ControlToValidate="tboxDeclinationValue"
                                    Display="None"></asp:RangeValidator>
                                <ajaxToolkit:ValidatorCalloutExtender ID="valDeclinationValueExt" runat="server"
                                    TargetControlID="valDeclinationValue" HighlightCssClass="highlight" />
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
    </FilterTemplate>
</flt:PrimitiveFilterTemplate>
