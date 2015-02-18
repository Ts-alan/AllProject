<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_GroupFiltersMain" Codebehind="GroupFiltersMain.ascx.cs" %>
<table>
<tr>
    <td>
        <asp:DropDownList ID="ddlTermGroupName" runat="server" SkinID="ddlLogic">
        </asp:DropDownList>
	</td>
    <td>
        <asp:textbox id="tboxGroupName" Runat="server"></asp:textbox>
    </td>
    <td>
        <asp:checkbox id="cboxGroupName" Runat="server"></asp:checkbox>
    </td>
<tr>
     <td>
        <asp:DropDownList ID="ddlTermDescription" runat="server" SkinID="ddlLogic">
        </asp:DropDownList>
	</td>
    <td>
        <asp:textbox id="tboxDescription" Runat="server"></asp:textbox>
    </td>
    <td>
       <asp:checkbox id="cboxDescription" Runat="server"></asp:checkbox> 
    </td>
</tr>
</table>