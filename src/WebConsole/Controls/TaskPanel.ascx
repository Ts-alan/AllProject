<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskPanel.ascx.cs" Inherits="Controls_TaskPanel" %>
<%@ Register TagPrefix="custom" Namespace="CustomControls" Assembly="CustomControls" %>
<%@ Register Src="~/Controls/CollapsiblePanelSwitch.ascx" TagName="CollapsiblePanelSwitch"
    TagPrefix="cc1" %>
<script language="javascript" type="text/javascript">
    var TaskPanel = function () {
        return {
            //enable assigntask and assigntasktoall buttons            
            EnableActionButtons: function () {
                $('#<%= divAssignTask.ClientID %>').removeClass("ButtonDisabled");
                $('#<%= divAssignTask.ClientID %>').addClass("Button");
                $('#<%= divAssignTaskToAll.ClientID %>').removeClass("ButtonDisabled");
                $('#<%= divAssignTaskToAll.ClientID %>').addClass("Button");
                $('#<%= lbtnAssignTask.ClientID %>').click( function () { });
                $('#<%= lbtnAssignTaskToAll.ClientID %>').click(function () { });
            },
            //disable assigntask and assigntasktoall buttons
            DisableActionButtons: function () {
                $('#<%= divAssignTask.ClientID %>').removeClass("Button");
                $('#<%= divAssignTask.ClientID %>').addClass("ButtonDisabled");
                $('#<%= divAssignTaskToAll.ClientID %>').removeClass("Button");
                $('#<%= divAssignTaskToAll.ClientID %>').addClass("ButtonDisabled");
                $('#<%= lbtnAssignTask.ClientID %>').click(function(){ return false; });
                $('#<%= lbtnAssignTaskToAll.ClientID %>').click(function(){ return false; });
            }
        };
    } ();
</script>
<div>
    <table class="subsection" width="100%">
        <tr>
            <td align="left">
                <asp:Label ID="lblTasks" runat="server" Text="<%$ Resources:Resource, Tasks %>"></asp:Label>
            </td>
            <td align="right">
                <cc1:CollapsiblePanelSwitch runat="server" ID="CollapsiblePanelSwitch1" />
            </td>
        </tr>
    </table>
    <div id="divDetails" runat="server" style="visibility: visible;" enableviewstate="true">
        <div class="taskPanel">
            <asp:PlaceHolder runat="server" ID="TasksPlaceHolder" />

  
        </div>
        <div>
            <div id="divAssignTask" runat="server" style="width: 100px; float: left">
                <asp:LinkButton ID="lbtnAssignTask" SkinID="TaskActions" runat="server" OnClick="lbtnAssignTask_Click"
                    ForeColor="White" Text="<%$ Resources:Resource, AssignTask %>" />
            </div>
            <div id="divAssignTaskToAll" runat="server" style="width: 210px; float: left">
                <asp:LinkButton ID="lbtnAssignTaskToAll" SkinID="TaskActions" runat="server" OnClick="lbtnAssignTaskToAll_Click"
                    ForeColor="White" Text="<%$ Resources:Resource, AssignTaskToAll %>" />
            </div>
        </div>
    </div>
</div>
