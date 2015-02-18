<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Controls_MultipleSelectionDropDownList" Codebehind="MultipleSelectionDropDownList.ascx.cs" %>
<asp:Panel ID="mainPanel" Height="1px" runat="server" Width="180px" BackColor="White">
    <table  style="table-layout: fixed; height: 17px; background-color: White !important; border-color: Black; border-width: 1px; border-style: solid;"
        cellspacing="0"  cellpadding="0" width="100%" >
        <tr style="height: 15px;">
            <td nowrap="nowrap">
                <asp:Label ID="lblSelectedText" runat="server" Width="163px" ToolTip="" Font-Size="Smaller"
                    Font-Names="Arial" BorderStyle="None" Height="17px" Style="font-size: 12px; cursor: default;"></asp:Label>
            </td>
            <td style="padding: 0px; background-color: #7eb3e3; width: 17px;" >
                <div id="imgDropDown" class="ImageDropDownList" runat="server"></div>
            </td>
        </tr>
    </table>
    <div runat="server" id="divOptions" style="z-index: 190; position: absolute; display: none;">
        <asp:ListBox ID="lboxOptions" runat="server" Width="180px" SelectionMode="Multiple" DataTextField="Text" DataValueField="Value"></asp:ListBox>
    </div>
</asp:Panel>
