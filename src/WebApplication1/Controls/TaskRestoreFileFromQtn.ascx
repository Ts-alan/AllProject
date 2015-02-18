<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_TaskRestoreFileFromQtn" Codebehind="TaskRestoreFileFromQtn.ascx.cs" %>
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