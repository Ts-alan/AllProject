<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Controls_UninstallTaskOptions" Codebehind="UninstallTaskOptions.ascx.cs" %>
<div runat="server" id="tskUninstall" style="display:none" title='<%$Resources:Resource, TaskUninstall %>'>
     <table  class="ListContrastTable" style="width:560px" runat="server" id="tblInstall">
        <tr>
            <td style="padding-left: 30px;padding-top: 10px;padding-bottom: 10px;">
                <%=Resources.Resource.Product%>:
            </td>
            <td style="padding-left: 10px;padding-top: 10px;padding-bottom: 10px;">
                <asp:DropDownList runat="server" ID="ddlProduct" style="width: 220px;"></asp:DropDownList>            
            </td>
        </tr>
    </table>
</div>
