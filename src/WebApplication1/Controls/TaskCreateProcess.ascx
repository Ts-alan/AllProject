<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_TaskCreateProcess" Codebehind="TaskCreateProcess.ascx.cs" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.CreateProcess%></div>
<div class="divSettings">
<table class="ListContrastTable" style="width:560px">
    <tr>
        <td>
            <%=Resources.Resource.CommandLine %>
        </td>
        <td>
            <asp:TextBox ID="tboxCreateProcess" runat="server" style="width:300px" ></asp:TextBox>
        </td>
       </tr>
       <tr>
            <td></td>
            <td>
                <asp:CheckBox ID="cboxCommand" runat="server" />&nbsp;&nbsp;<%= Resources.Resource.ComSpec %>
            </td>
       </tr>
</table>
</div>
         
        
