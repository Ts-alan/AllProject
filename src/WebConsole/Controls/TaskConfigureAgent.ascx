<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureAgent.ascx.cs" Inherits="Controls_TaskConfigureAgent" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:560px">Configure Agent</div>
<div class="divSettings">
<table class="ListContrastTable" style="width:560px">
    <tr>
        <td>
            <%=Resources.Resource.SelectFile %>:
        </td>
        <td>
            <asp:LinkButton ID="lbtnUpload" runat="server" OnClick="lbtnUpload_Click" Width="80"><%=Resources.Resource.Upload%></asp:LinkButton>
            <asp:FileUpload ID="fuClient" runat="server" />
            <asp:TextBox runat="server" ID="tboxConfigPath" style="display: none;"></asp:TextBox>
        </td>  
    </tr>
    <tr>
        <td>
            <%=Resources.Resource.Information %>:
        </td>
        <td>
            <asp:Label ID="lblDetails" runat="server" />
        </td>
    </tr>
</table>
</div>     