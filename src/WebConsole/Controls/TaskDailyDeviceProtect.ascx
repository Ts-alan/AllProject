<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskDailyDeviceProtect.ascx.cs" Inherits="Controls_TaskDailyDeviceProtect" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.TaskDailyDeviceProtect%></div>
<div class="divSettings">
    <asp:CheckBox runat="server" ID="cboxUseDailyProtect" Checked="false" />
    <table  class="ListContrastTable" style="width:560px" runat="server" id="tblProtect">
        <tr>
            <td class="gridViewHeader"><%=Resources.Resource.HeaderDay %></td>
            <td class="gridViewHeader"><%=Resources.Resource.Mode %></td>
            <td class="gridViewHeader"><%=Resources.Resource.HeaderDay %></td>
            <td class="gridViewHeader"><%=Resources.Resource.Mode %></td>
        </tr>
        <tr>
            <td style="padding-left: 10px; padding-right: 10px;"><%=Resources.Resource.Monday %></td>
            <td style="padding-left: 10px; padding-right: 10px;"><asp:DropDownList runat="server" ID="ddlMonday"></asp:DropDownList></td>
            <td style="padding-left: 10px; padding-right: 10px;"><%=Resources.Resource.Friday%></td>
            <td style="padding-left: 10px; padding-right: 10px;"><asp:DropDownList runat="server" ID="ddlFriday"></asp:DropDownList></td>
        </tr>
        <tr>
            <td style="padding-left: 10px; padding-right: 10px;"><%=Resources.Resource.Tuesday%></td>
            <td style="padding-left: 10px; padding-right: 10px;"><asp:DropDownList runat="server" ID="ddlTuesday"></asp:DropDownList></td>
            <td style="padding-left: 10px; padding-right: 10px;"><%=Resources.Resource.Saturday%></td>
            <td style="padding-left: 10px; padding-right: 10px;"><asp:DropDownList runat="server" ID="ddlSaturday"></asp:DropDownList></td>
        </tr>
        <tr>
            <td style="padding-left: 10px; padding-right: 10px;"><%=Resources.Resource.Wednesday%></td>
            <td style="padding-left: 10px; padding-right: 10px;"><asp:DropDownList runat="server" ID="ddlWednesday"></asp:DropDownList></td>
            <td style="padding-left: 10px; padding-right: 10px;"><%=Resources.Resource.Sunday%></td>
            <td style="padding-left: 10px; padding-right: 10px;"><asp:DropDownList runat="server" ID="ddlSunday"></asp:DropDownList></td>
        </tr>
        <tr>
            <td style="padding-left: 10px; padding-right: 10px;"><%=Resources.Resource.Thursday%></td>
            <td style="padding-left: 10px; padding-right: 10px;"><asp:DropDownList runat="server" ID="ddlThursday"></asp:DropDownList></td>
            <td></td>
            <td></td>
        </tr>   
    </table>  
</div>