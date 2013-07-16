<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TestTaskOptions.ascx.cs"
    Inherits="Controls_TestTaskOptions" %>
<div id="tskTest" runat="server" style="display:none;">
        <asp:TextBox ID="TextBox1" runat="server" Style="width: 300px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="TextBox1"
            runat="server" ErrorMessage="RequiredFieldValidator1" ValidationGroup="TaskValidation"
            Display="None"></asp:RequiredFieldValidator>
        <ajaxToolkit:ValidatorCalloutExtender2 PopupPosition="BottomLeft" ID="ValidatorCalloutExtender1"
            runat="server" TargetControlID="RequiredFieldValidator1" HighlightCssClass="highlight"
            Width="300" />
        <asp:TextBox ID="TextBox2" runat="server" Style="width: 300px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="TextBox2"
            runat="server" ErrorMessage="RequiredFieldValidator2" ValidationGroup="TaskValidation"
            Display="None"></asp:RequiredFieldValidator>
        <ajaxToolkit:ValidatorCalloutExtender2 PopupPosition="BottomLeft" ID="ValidatorCalloutExtender2"
            runat="server" TargetControlID="RequiredFieldValidator2" HighlightCssClass="highlight"
            Width="300" />
</div>
