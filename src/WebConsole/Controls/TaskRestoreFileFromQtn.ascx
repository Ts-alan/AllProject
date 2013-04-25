<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskRestoreFileFromQtn.ascx.cs" Inherits="Controls_TaskRestoreFileFromQtn" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.TaskNameRestoreFileFromQtn%></div>
<div class="divSettings">
<table class="ListContrastTable" style="width:560px">
    <tr>
        <td>
            <%=Resources.Resource.QtnFilePath%>
        </td>
        <td>
            <asp:TextBox ID="tboxCreateProcess" runat="server" style="width:300px" ></asp:TextBox>
        </td>
       </tr>
</table>
</div>