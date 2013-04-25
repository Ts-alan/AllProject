<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskCreateProcess.ascx.cs" Inherits="Controls_TaskCreateProcess" %>
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
         
        
