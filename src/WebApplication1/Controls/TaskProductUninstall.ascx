<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_TaskProductUninstall" Codebehind="TaskProductUninstall.ascx.cs" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.TaskUninstall%></div>
<div class="divSettings">
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