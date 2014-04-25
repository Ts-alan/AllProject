﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CustomizableTask.ascx.cs"
    Inherits="Controls_CustomizableTask" %>
<%@ Register TagPrefix="custom" Namespace="VirusBlokAda.CC.CustomControls" Assembly="CustomControls" %>
<script type="text/javascript" language="javascript">
    //ddlTask OnChange event handler - generates function name depending on ClientID
    function ddlTasks_<%=ClientID %>_OnChange() {
        var ddlTasks = $('#<%=ddlTasks.ClientID %>');
        //value selected in dropdown
        var selectedValue = ddlTasks.val();
        var hfTemporaryTaskKey = $('#<%=hfTemporaryTaskKey.ClientID %>');
        //value corresponding to Temporary Task
        var temporaryTaskValue = hfTemporaryTaskKey.val();
        var hfTemporaryTaskIsClear = $('#<%=hfTemporaryTaskIsClear.ClientID %>');
        //value is true if options in temporary task are not set
        var temporaryTaskIsClear = hfTemporaryTaskIsClear.val();
        var ibtnCustomize = $('#<%=ibtnCustomize.ClientID %>');
        var ibtnDelete = $('#<%=ibtnDelete.ClientID %>');
        
        var rbtnUseTask = $('#<%=rbtnUseTask.ClientID %>');
        //set images on Delete and Customize image buttons according to selected item in dropdown
        if (temporaryTaskValue == selectedValue) {
            if (temporaryTaskIsClear == '1') {
                ibtnCustomize.attr('src', '<%=TaskIcons.OptionsDisabled %>');
                ibtnDelete.attr('src','<%=TaskIcons.EraseDisabled %>');
                ibtnDelete.click( function () { return false; });
            }
            else {
                ibtnCustomize.attr('src','<%=TaskIcons.OptionsEnabled %>');
                ibtnDelete.attr('src', '<%=TaskIcons.Erase %>');
                ibtnDelete.unbind('click');
            }
        }
        else {
            ibtnCustomize.attr('src', '<%=TaskIcons.OptionsEnabled %>');
            ibtnDelete.attr('src',  '<%=TaskIcons.Delete %>');
            ibtnDelete.unbind('click');
        }
        //enabled/disables Task Panel action buttons depending on selected item in dropdown list
        if (rbtnUseTask.checked) {
            if ((temporaryTaskValue == selectedValue) && (temporaryTaskIsClear == '1')) {
                TaskHelper.DisableTaskPanelActionButtons();
            }
            else {
                TaskHelper.EnableTaskPanelActionButtons();
            }
        }
    }

    //rbtnUseTask OnClick event handler - generates function name depending on ClientID
    function rbtnUseTask_<%=ClientID %>_OnClick() {  
        var ddlTasks = $('#<%=ddlTasks.ClientID %>');
        //value selected in dropdown
        var selectedValue = ddlTasks.val();
        var hfTemporaryTaskKey =$('#<%=hfTemporaryTaskKey.ClientID %>');
        //value corresponding to Temporary Task
        var temporaryTaskValue = hfTemporaryTaskKey.val();
        var hfTemporaryTaskIsClear = $('#<%=hfTemporaryTaskIsClear.ClientID %>');
        //value is true if options in temporary task are not set
        var temporaryTaskIsClear = hfTemporaryTaskIsClear.val();

        //enabled/disables Task Panel action buttons depending on selected item in dropdown list
        if ((temporaryTaskValue == selectedValue) && (temporaryTaskIsClear == '1')) {
            TaskHelper.DisableTaskPanelActionButtons();
        }
        else {
            TaskHelper.EnableTaskPanelActionButtons();
        }
    }
 </script>

<div runat="server" id="divCustomizableTask" class="customizableTask">
    <div class="name">
        <asp:Label runat="Server" ID="lblName"></asp:Label>
    </div>
    <div class="radio">
        <custom:RadioButton2 ID="rbtnUseTask" runat="server" GroupName="GroupTasks"/>
        <asp:HiddenField ID="hfTemporaryTaskKey" runat="server"  />
        <asp:HiddenField ID="hfTemporaryTaskIsClear" runat="server"  />
    </div>
    <br />
    <div class="dropdown">
        <asp:DropDownList ID="ddlTasks" runat="server" >
        </asp:DropDownList>
    </div>
    <div class="action">
        <asp:ImageButton ID="ibtnCustomize" runat="server" OnClick="ibtnCustomize_Click"/>
        <asp:ImageButton ID="ibtnDelete" runat="server" OnClick="ibtnDelete_Click"/>
        <asp:Button ID="btnHiddenApply" runat="server" Text="" SkinID="HiddenButton" OnClick="btnHiddenApply_Click" />
        <asp:Button ID="btnHiddenSave" runat="server" Text="" SkinID="HiddenButton" OnClick="btnHiddenSave_Click" />
        <asp:Button ID="btnHiddenSaveAs" runat="server" Text="" SkinID="HiddenButton" OnClick="btnHiddenSaveAs_Click" />
        <asp:HiddenField ID="hfSaveAsName" runat="server"  />
        <asp:HiddenField ID="hfUsedNames" runat="server"  />
        <asp:HiddenField ID="hfRestrictedNames" runat="server"  />

    </div>
</div>
<custom:StorageControl ID="CustomizableTaskStateStorage" StorageType="Session" runat="server" />
