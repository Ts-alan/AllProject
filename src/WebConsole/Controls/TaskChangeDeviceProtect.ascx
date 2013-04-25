<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskChangeDeviceProtect.ascx.cs" Inherits="Controls_TaskChangeDeviceProtect" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.TaskChangeDeviceProtect%></div>
<div class="divSettings">
    <table  class="ListContrastTable" style="width:560px" runat="server" id="tblProtect">
    <tr>
        <td>
            <asp:Label runat="server" ID="lblMode"><%=Resources.Resource.Mode%></asp:Label>&nbsp;
            <asp:DropDownList runat="server" ID="ddlMode"></asp:DropDownList>            
        </td>
    </tr>
    </table>  
</div>