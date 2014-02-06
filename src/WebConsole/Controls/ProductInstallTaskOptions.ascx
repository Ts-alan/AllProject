<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductInstallTaskOptions.ascx.cs" Inherits="Controls_ProductInstallTaskOptions" %>
<div id="tskInstallProduct" runat="server" style="display:none;" title='<%$Resources:Resource, Install %>'>
    <table  class="ListContrastTable" style="width:560px" runat="server" id="tblInstall">
    <tr>
        <td style="padding-left: 30px;padding-top: 10px;">
            <%=Resources.Resource.Product%>:
        </td>
        <td style="padding-left: 10px;padding-top: 10px;">
            <asp:DropDownList runat="server" ID="ddlProduct" style="width: 220px;">
                <asp:ListItem Value='0' Text='<%$Resources:Resource, Antivirus %>' ></asp:ListItem>
                <asp:ListItem Value='1' Text='<%$Resources:Resource, RemoteConsoleScanner %>' ></asp:ListItem>
            </asp:DropDownList> 
                           
        </td>
    </tr>
    <tr>
        <td style="padding-left: 30px;padding-top: 10px;padding-bottom: 10px;">
            <%=Resources.Resource.Arguments%>:
        </td>
        <td style="padding-left: 10px;padding-top: 10px;padding-bottom: 10px;">
            <asp:TextBox runat="server" ID="tboxArguments" style="width: 220px"></asp:TextBox>
        </td>
    </tr>
    </table>
</div>