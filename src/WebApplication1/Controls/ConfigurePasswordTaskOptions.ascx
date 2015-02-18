<%@ Control Language="C#" AutoEventWireup="true"
 Inherits="Controls_ConfigurePasswordTaskOptions" Codebehind="ConfigurePasswordTaskOptions.ascx.cs" %>

<div id="tskConfigurePassword" runat="server" style="display:none;" title='<%$Resources:Resource, TaskNameConfigurePassword %>' >
    <table class="ListContrastTable" style="width: 560px">
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