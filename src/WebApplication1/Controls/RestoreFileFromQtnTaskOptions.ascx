<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Controls_RestoreFileFromQtnTaskOptions" Codebehind="RestoreFileFromQtnTaskOptions.ascx.cs" %>
<div id="tskRestoreFileFromQtn" runat="server" style="display:none;" title='<%$Resources:Resource, TaskNameRestoreFileFromQtn %>'>
    <table class="ListContrastTable" style="width: 560px">
        <tr>
            <td>
                <%=Resources.Resource.QtnFilePath%>
            </td>
            <td>
                <asp:TextBox ID="tboxRestoreFileFromQtn" runat="server" Style="width: 300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tboxRestoreFileFromQtn"
                    runat="server" ErrorMessage="RequiredFieldValidator" ValidationGroup="TaskValidation" Display="None"></asp:RequiredFieldValidator>
                <ajaxToolkit:ValidatorCalloutExtender PopupPosition="BottomLeft" ID="ValidatorCalloutExtender21"
                    runat="server" TargetControlID="RequiredFieldValidator1" HighlightCssClass="highlight" Width="300"/>
            </td>
        </tr>
    </table>
</div>