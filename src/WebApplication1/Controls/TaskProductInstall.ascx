<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_TaskProductInstall" Codebehind="TaskProductInstall.ascx.cs" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.Install%></div>
<div class="divSettings">
    <table  class="ListContrastTable" style="width:560px" runat="server" id="tblInstall">
    <tr>
        <td style="padding-left: 30px;padding-top: 10px;">
            <%=Resources.Resource.Product%>:
        </td>
        <td style="padding-left: 10px;padding-top: 10px;">
            <asp:DropDownList runat="server" ID="ddlProduct" style="width: 220px;"></asp:DropDownList>            
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