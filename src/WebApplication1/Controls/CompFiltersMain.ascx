<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_CompFiltersMain" Codebehind="CompFiltersMain.ascx.cs" %>
<table>
<tr>
    <td>
        <asp:DropDownList ID="ddlTermComputerName" runat="server" SkinID="ddlLogic">
        </asp:DropDownList>
	</td>
    <td>
        <asp:textbox id="tboxComputerName" Runat="server"></asp:textbox>
    </td>
    <td>
        <asp:checkbox id="cboxComputerName" Runat="server"></asp:checkbox>
    </td>
<tr>
     <td>
        <asp:DropDownList ID="ddlTermUserLogin" runat="server" SkinID="ddlLogic">
        </asp:DropDownList>
	</td>
    <td>
        <asp:textbox id="tboxUserlogin" Runat="server"></asp:textbox>
    </td>
     <td>
        <asp:checkbox id="cboxUserLogin" Runat="server"></asp:checkbox>
     </td>
</tr>
<tr>
     <td>
        <asp:DropDownList ID="ddlTermIPAddress" runat="server" SkinID="ddlLogic">
        </asp:DropDownList>
	</td>
    <td>
       <asp:textbox id="tboxIPAddress" Runat="server"></asp:textbox> 
    </td>
    <td>
      <asp:checkbox id="cboxIPAddress" Runat="server"></asp:checkbox>  
    </td>
</tr>
<tr>
     <td>
        <asp:DropDownList ID="ddlTermLatestMalware" runat="server" SkinID="ddlLogic">
        </asp:DropDownList>
	</td>
    <td>
        <asp:textbox id="tboxLatestMalware" Runat="server"></asp:textbox>
    </td>
    <td>
       <asp:checkbox id="cboxLatestMalware" Runat="server"></asp:checkbox> 
    </td>
</tr>
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