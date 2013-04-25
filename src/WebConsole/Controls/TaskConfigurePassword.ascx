<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigurePassword.ascx.cs" Inherits="Controls_TaskConfigurePassword" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.TaskNameConfigurePassword%></div>
<div class="divSettings">
<table class="ListContrastTable" style="width:560px">
    <tr>
        <td>
            <%=Resources.Resource.NewPasswordLabelText %>
        </td>
        <td>
            <asp:TextBox ID="tboxPassword" runat="server"></asp:TextBox>
        </td>
    </tr>
</table>
</div>