<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ConfigurePasswordTaskOptions.ascx.cs"
 Inherits="Controls_ConfigurePasswordTaskOptions" %>

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