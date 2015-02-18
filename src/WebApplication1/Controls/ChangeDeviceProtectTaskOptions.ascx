<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_ChangeDeviceProtectTaskOptions" Codebehind="ChangeDeviceProtectTaskOptions.ascx.cs" %>

<div  runat="server" id="tskChangeDeviceProtect" style="display:none;" title='<%$Resources:Resource, TaskChangeDeviceProtect %>' >
    <table  class="ListContrastTable" style="width:560px" runat="server" id="tblChangeDevice">
    <tr>
        <td>
            <asp:Label runat="server" ID="lblMode"><%=Resources.Resource.Mode%></asp:Label>&nbsp;
            <asp:DropDownList runat="server" ID="ddlMode">            
                <asp:ListItem Value='0' Text='<%$Resources:Resource, Deactivate %>' ></asp:ListItem>
                <asp:ListItem Value='1' Text='<%$Resources:Resource, Activate %>' ></asp:ListItem>     
                <asp:ListItem Value='2' Text='<%$Resources:Resource, LogEvents %>' ></asp:ListItem>             
            </asp:DropDownList>            
        </td>
    </tr>
    </table>  
</div>