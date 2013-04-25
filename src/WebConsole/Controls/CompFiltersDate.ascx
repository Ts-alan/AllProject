<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompFiltersDate.ascx.cs" Inherits="Controls_CompFiltersDate" %>
<%@ Register Assembly="DateControl" Namespace="DateControl" TagPrefix="cc1" %>
<table>
<tr>
    <td>
        <asp:DropDownList ID="ddlTermRecentActive" runat="server" SkinID="ddlLogic">
        </asp:DropDownList>
    </td>
    <td>
    <NOBR>
        <cc1:DateCustomControl ID="dccRecentActive" RenderInterval=true  runat="server" ddlSkinID="ddcControl" />
    </NOBR>
    </td>
     <td style="width: 149px">
       <asp:checkbox id="cboxRecentActive" Runat="server"></asp:checkbox> 
    </td>
</tr>
<tr>
    <td>
        <asp:DropDownList ID="ddlTermLatestUpdate" runat="server" SkinID="ddlLogic">
        </asp:DropDownList>
    </td>
    <td>
    <NOBR>
        <cc1:DateCustomControl ID="dccLatestUpdate" RenderInterval=true  runat="server" ddlSkinID="ddcControl"/>
    </NOBR>
    </td>
     <td style="width: 149px">
        <asp:checkbox id="cboxLatestUpdate" Runat="server"></asp:checkbox>
    </td>
</tr>
<tr>
    <td>
        <asp:DropDownList ID="ddlTermLatestInfected" runat="server" SkinID="ddlLogic">
        </asp:DropDownList>
    </td>
    <td>
    <NOBR>
        <cc1:DateCustomControl ID="dccLatestInfected" RenderInterval=true runat="server"  ddlSkinID="ddcControl" />
    </NOBR>
    </td>
     <td style="width: 149px">
        <asp:checkbox id="cboxLatestInfected" Runat="server"></asp:checkbox>
    </td>
</tr>
</table>
