<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SimpleTask.ascx.cs" Inherits="Controls_SimpleTask" %>
<%@ Register TagPrefix="custom" Namespace="VirusBlokAda.CC.CustomControls" Assembly="CustomControls" %>
<div runat="server" id="divSimpleTask" class="simpleTask" >
    <div class="name">
        <asp:Label runat="Server" ID="lblName"  ></asp:Label>
    </div>
    <div class="radio">
        <custom:RadioButton2 ID="rbtnUseTask" runat="server" GroupName="GroupTasks"
        onclick="TaskHelper.EnableTaskPanelActionButtons();"/>
    </div>
</div>
<custom:StorageControl ID="SimpleTaskStateStorage" StorageType="Session" runat="server" />