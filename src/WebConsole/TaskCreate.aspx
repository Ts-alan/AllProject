<%@ Page Language="C#" MasterPageFile="~/mstrPageMain.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="TaskCreate.aspx.cs" Inherits="TaskCreate"%>

<%@ Register Src="Controls/TaskCreateProcess.ascx" TagName="TaskUser" TagPrefix="tskCreateProcess" %>
<%@ Register Src="Controls/TaskSendFile.ascx" TagName="TaskUser" TagPrefix="tskSendFile" %>
<%@ Register Src="Controls/TaskCancelTask.ascx" TagName="TaskUser" TagPrefix="tskCancelTask" %>
<%@ Register Src="Controls/TaskListProcesses.ascx" TagName="TaskUser" TagPrefix="tskListProcesses" %>
<%@ Register Src="Controls/TaskSystemInfo.ascx" TagName="TaskUser" TagPrefix="tskSystemInfo" %>
<%@ Register Src="Controls/TaskConfigureLoader.ascx" TagName="TaskUser" TagPrefix="tskConfigureLoader" %>
<%@ Register Src="Controls/TaskConfigureMonitor.ascx" TagName="TaskUser" TagPrefix="tskConfigureMonitor" %>
<%@ Register Src="Controls/TaskConfigureScanner.ascx" TagName="TaskUser" TagPrefix="tskConfigureScanner" %>
<%@ Register Src="Controls/TaskComponentState.ascx" TagName="TaskUser" TagPrefix="tskComponentState" %>
<%@ Register Src="Controls/TaskConfigurePassword.ascx" TagName="TaskUser" TagPrefix="tskConfigurePassword" %>
<%@ Register Src="Controls/TaskConfigureQuarantine.ascx" TagName="TaskUser" TagPrefix="tskConfigureQuarantine" %>
<%@ Register Src="Controls/TaskRestoreFileFromQtn.ascx" TagName="TaskUser" TagPrefix="tskRestoreFileFromQtn" %>
<%@ Register Src="~/Controls/TaskConfigureProactive.ascx" TagName="TaskUser" TagPrefix="tskProactiveProtection" %>
<%@ Register Src="~/Controls/TaskConfigureFirewall.ascx" TagName="TaskUser" TagPrefix="tskFirewall" %>
<%@ Register Src="~/Controls/TaskChangeDeviceProtect.ascx" TagName="TaskUser" TagPrefix="tskChangeDeviceProtect" %>
<%@ Register Src="~/Controls/TaskRequestPolicy.ascx" TagName="TaskUser" TagPrefix="tskRequestPolicy" %>
<%@ Register Src="~/Controls/TaskConfigureScheduler.ascx" TagName="TaskUser" TagPrefix="tskConfigureScheduler" %>
<%@ Register Src="~/Controls/TaskRunScanner.ascx" TagName="TaskUser" TagPrefix="tskRunScanner" %>
<%@ Register Src="~/Controls/TaskConfigureIntegrityCheck.ascx" TagName="TaskUser" TagPrefix="tskConfigureIntegrityCheck" %>

<%@ OutputCache Location="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager ID="tsm" runat=server />

<script language="javascript" type="text/javascript">

    $('form').ready(function()
    {
        var w = $get('<%=hdnWidth.ClientID%>');
        var h = $get('<%=hdnHeight.ClientID%>');

        w.value = (window.innerWidth ? window.innerWidth : (document.documentElement.clientWidth ? document.documentElement.clientWidth : document.body.offsetWidth)); 
        h.value = (window.innerHeight ? window.innerHeight : (document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.offsetHeight));    
        
    });

</script>

<asp:HiddenField runat="server" ID="hdnWidth" Value="0"/>
<asp:HiddenField runat="server" ID="hdnHeight" Value="0"/>

<div class="title"><%=Resources.Resource.TaskCreate%></div>    
    <asp:Panel ID="pnlControl" runat="server">
            <tskCreateProcess:TaskUser ID="tskCreateProcess" runat="server" Visible="false"/>
            <tskSendFile:TaskUser ID="tskSendFile" runat="server" Visible="false"/>
            <tskListProcesses:TaskUser ID="tskListProcesses" runat="server" Visible="false"/>
            <tskCancelTask:TaskUser ID="tskCancelTask" runat="server" Visible="false"/>
            <tskSystemInfo:TaskUser ID="tskSystemInfo" runat="server" Visible="false"/>
            <tskConfigureLoader:TaskUser ID="tskConfigureLoader" runat="server" Visible="false"/>
            <tskConfigureMonitor:TaskUser ID="tskConfigureMonitor" runat="server" Visible="false"/>
            <tskConfigureScanner:TaskUser ID="tskConfigureScanner" runat="server" Visible="false"/>
            <tskComponentState:TaskUser ID="tskComponentState" runat="server" Visible="false"/>          
            <tskConfigurePassword:TaskUser ID="tskConfigurePassword" runat="server" Visible="false"/>          
            <tskConfigureQuarantine:TaskUser ID="tskConfigureQuarantine" runat="server" Visible="false"/>
            <tskRestoreFileFromQtn:TaskUser ID="tskRestoreFileFromQtn" runat="server" Visible="false"/>                    
            <tskProactiveProtection:TaskUser ID="tskProactiveProtection" runat="server" Visible="false" />
            <tskFirewall:TaskUser ID="tskFirewall" runat="server" Visible="false" />
            <tskChangeDeviceProtect:TaskUser ID="tskChangeDeviceProtect" runat="server" Visible="false" />
            <tskRequestPolicy:TaskUser ID="tskRequestPolicy" runat="server" Visible="false" />
            <tskConfigureScheduler:TaskUser ID="tskConfigureScheduler" runat="server" Visible="false" />
            <tskRunScanner:TaskUser ID="tskRunScanner" runat="server" Visible="false" HideHeader="true" />
            <tskConfigureIntegrityCheck:TaskUser ID="tskConfigureIntegrityCheck" runat="server" Visible="false" />
                                    
    </asp:Panel>
    <table>
    <tr>
        <td>
            <asp:Label runat="server" ID="lblTaskName"></asp:Label>
        </td>
        <td>
        &nbsp;&nbsp;<asp:TextBox ID="tbSaveAs" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:LinkButton ID="lbtnSaveAs" runat="server" OnClick="lbtnSaveAs_Click"></asp:LinkButton>
        </td>
    </tr>
    </table>
    
    <asp:Button runat="server" ID="btnHiddenMessage" style="visibility:hidden" />
<asp:Panel ID="pnlModalPopapMessage" runat="server" Style="display: none" CssClass="modalPopupMessage">
                                    <div runat="server" id ="mpPicture" class="ModalPopupPictureSuccess">
                                    </div>
                                    <div id="ModalPopupText">
                                        <p style="vertical-align:middle;">
                                            <center><asp:Label runat="server" ID="lblMessage" ></asp:Label></center>
                                        </p>                                                                            
                                    </div>
                                    <div id="ModalPopupButton">
                                        <p style="text-align: center; vertical-align:bottom;">
                                            <asp:Button ID="btnClose" runat="server" />                                            
                                        </p>
                                    </div>
                            </asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnHiddenMessage" PopupControlID="pnlModalPopapMessage" CancelControlID="btnClose" BackgroundCssClass="modalBackground" DropShadow="true" PopupDragHandleControlID="pnlModalPopapMessage" />
    
</asp:Content>

