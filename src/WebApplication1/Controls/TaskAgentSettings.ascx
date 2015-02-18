<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_TaskAgentSettings" Codebehind="TaskAgentSettings.ascx.cs" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.TaskAgentSettings%></div>
<div class="divSettings">
    <table  class="ListContrastTable" style="width:560px" runat="server" id="tblAgentSettings">
    <tr>
        <td>
            <asp:Label runat="server" ID="lblPoollingTimeInterval"><%=Resources.Resource.PoollingTimeInterval%></asp:Label>&nbsp;
            <asp:TextBox runat="server" ID="txtPoollingTimeInterval" style="width: 40px;"></asp:TextBox>
            <asp:Label runat="server" ID="lblTime"><%=Resources.Resource.Minutes%></asp:Label>&nbsp;
        </td>
    </tr>
    </table>  
</div>