<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_CompFiltersExtra" Codebehind="CompFiltersExtra.ascx.cs" %>
<table>
    <tr>
        <td>
            <asp:DropDownList ID="ddlTermDomainName" runat="server" SkinID="ddlLogic">
            </asp:DropDownList>
        </td>
        <td>
            <asp:textbox id="tboxDomainName" Runat="server"></asp:textbox>
        </td>
        <td>
            <asp:checkbox id="cboxDomainName" Runat="server"></asp:checkbox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlTermVba32Version" runat="server" SkinID="ddlLogic">
            </asp:DropDownList>
        </td>
        <td>
            <asp:textbox id="tboxVba32Version" Runat="server"></asp:textbox>
        </td>
        <td>
            <asp:checkbox id="cboxVba32Version" Runat="server"></asp:checkbox>
         </td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlTermRam" runat="server" SkinID="ddlLogic">
            </asp:DropDownList>
        </td>
        <td>
            <asp:textbox id="tboxRam" SkinID="IntDiapasonValueBox" Runat="server"></asp:textbox>&nbsp;-&nbsp;
             <asp:textbox id="tboxRam2" SkinID="IntDiapasonValueBox" Runat="server"></asp:textbox>
        </td>
        <td>
            <asp:checkbox id="cboxRam" Runat="server"></asp:checkbox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlTermCpu" runat="server" SkinID="ddlLogic">
            </asp:DropDownList>
        </td>
        <td>    
            <asp:textbox id="tboxCPU" SkinID="IntDiapasonValueBox" Runat="server"></asp:textbox>&nbsp;-&nbsp;
            <asp:textbox id="tboxCPU2" SkinID="IntDiapasonValueBox" Runat="server"></asp:textbox>
        </td>
        <td>
            <asp:checkbox id="cboxCPU" Runat="server"></asp:checkbox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlTermOSType"  runat="server" SkinID="ddlLogic">
            </asp:DropDownList>
        </td>
        <td>
            <asp:TextBox ID="tboxOStype" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:checkbox id="cboxOStype" Runat="server"></asp:checkbox>
        </td>
    </tr>
</table>