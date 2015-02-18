<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_TaskSaveIntegrityCheck" Codebehind="TaskSaveIntegrityCheck.ascx.cs" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.TaskSaveIntegrityCheck%></div>
<div class="divSettings">
    <table  class="ListContrastTable" style="width:560px" runat="server" id="tblSaveIntegrityCheck">
    <tr>
        <td style="padding-left: 30px;padding-top: 10px;padding-bottom: 10px;">
            <%=Resources.Resource.Command%>:
        </td>
        <td style="padding-left: 10px;padding-top: 10px;padding-bottom: 10px;">
            <asp:DropDownList runat="server" ID="ddlCommand" style="width: 220px;"></asp:DropDownList>            
        </td>
    </tr>
    <tr>
        <td style="padding-left: 30px;padding-top: 10px;padding-bottom: 10px;">
            <%=Resources.Resource.Integrity%>:
        </td>
        <td style="padding-left: 10px;padding-top: 10px;padding-bottom: 10px;">
            <asp:DropDownList runat="server" ID="ddlIntegrity" style="width: 220px;"></asp:DropDownList>            
        </td>
    </tr>
    </table>
</div>