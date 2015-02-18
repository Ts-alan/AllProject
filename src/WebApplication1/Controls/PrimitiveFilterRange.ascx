<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_PrimitiveFilterRange" Codebehind="PrimitiveFilterRange.ascx.cs" %>
<%@ Register Src="~/Controls/PrimitiveFilterTemplate.ascx" TagName="PrimitiveFilterTemplate" TagPrefix="flt" %>

<flt:PrimitiveFilterTemplate runat="server" ID="fltTemplate" TextFilter="Name" >
<FilterTemplate>
    <div style="padding: 5px;">
       <asp:textbox id="tboxRangeStart" SkinID="IntDiapasonValueBox" runat="server" ></asp:textbox>&nbsp;-&nbsp;
       <asp:textbox id="tboxRangeStop" SkinID="IntDiapasonValueBox" runat="server" ></asp:textbox>
       
       <ajaxToolkit:FilteredTextBoxExtender ID="fltRangeStart" runat="server" TargetControlID="tboxRangeStart"
                        FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>
       <ajaxToolkit:FilteredTextBoxExtender ID="fltRangeStop" runat="server" TargetControlID="tboxRangeStop"
                        FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>
        <asp:CompareValidator ID="cmpRange" runat="server" ControlToValidate="tboxRangeStop" 
            ControlToCompare="tboxRangeStart" Display="None" Operator="GreaterThanEqual" SetFocusOnError="True"
            ValidationGroup="FilterValidation" Type="Integer"></asp:CompareValidator>
        <ajaxToolkit:ValidatorCalloutExtender ID="cmpRangeExt" runat="server" TargetControlID="cmpRange" HighlightCssClass="highlight">
        </ajaxToolkit:ValidatorCalloutExtender>

    </div>
</FilterTemplate>
</flt:PrimitiveFilterTemplate>