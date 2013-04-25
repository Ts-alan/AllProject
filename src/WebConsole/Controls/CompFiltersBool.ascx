<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompFiltersBool.ascx.cs" Inherits="Controls_CompFiltersBool" %>
<table>
				<tr>
				    <td style="height: 24px">
                        <asp:DropDownList ID="ddlTermVba32KeyValid" runat="server" SkinID="ddlLogic">
                        </asp:DropDownList>
				    </td>
					<td style="height: 24px">
						<asp:checkbox id="cboxIsVba32KeyValid" Runat="server"></asp:checkbox>
					</td>
					<td style="height: 24px" >
						<asp:checkbox id="cboxVba32KeyValid" Runat="server"></asp:checkbox>
					</td>
				</tr>
				<tr>
				    <td>
                        <asp:DropDownList ID="ddlTermVba32Integrity" runat="server" SkinID="ddlLogic">
                        </asp:DropDownList>
				    </td>
					<td>
					    <asp:checkbox id="cboxIsVba32Integrity" Runat="server"></asp:checkbox>
					</td>
					<td>
					    <asp:checkbox id="cboxVba32Integrity" Runat="server"></asp:checkbox>
					</td>
				</tr>
				<tr>
				     <td>
                        <asp:DropDownList ID="ddlTermControlCenter" runat="server" SkinID="ddlLogic">
                        </asp:DropDownList>
				    </td>
					<td>
						<asp:checkbox id="cboxIsControlCenter" Runat="server"></asp:checkbox>
					</td>
					<td >
						<asp:checkbox id="cboxControlCenter" Runat="server"></asp:checkbox>
					</td>
				</tr>
</table> 
