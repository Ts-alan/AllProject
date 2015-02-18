<%@ Page Title="" Language="C#" MasterPageFile="~/mstrPageNew.master" AutoEventWireup="true" Inherits="Default3"  EnableEventValidation="false" Codebehind="Default3.aspx.cs" %>

<%@ Register Src="~/Controls/TaskPanel.ascx" TagName="TaskPanel" TagPrefix="tsk" %>
<%@ Register Src="~/Controls/SimpleTask.ascx" TagName="SimpleTask" TagPrefix="tsk" %>
<%@ Register Src="~/Controls/CustomizableTask.ascx" TagName="CustomizableTask" TagPrefix="tsk" %>
<%@ Register Src="~/Controls/CreateProcessTaskOptions.ascx" TagName="CreateProcessTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/ConfigurePasswordTaskOptions.ascx" TagName="ConfigurePasswordTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/RestoreFileFromQtnTaskOptions.ascx" TagName="RestoreFileFromQtnTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/UninstallTaskOptions.ascx" TagName="UninstallTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/ProductInstallTaskOptions.ascx" TagName="ProductInstallTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/ChangeDeviceProtectTaskOptions.ascx" TagName="ChangeDeviceProtectTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/TestTaskOptions.ascx" TagName="TestTaskOptions"
    TagPrefix="tsk" %>
<%@ Register Src="~/Controls/TaskOptionsDialog.ascx" TagName="TaskOptionsDialog"
    TagPrefix="tsk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    <Scripts>
        <asp:ScriptReference Path="~/js/Safari3AjaxHack.js" />
    </Scripts>
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <tsk:TaskPanel ID="TaskPanel" runat="server" OnTaskAssign="CompositeTaskPanel_TaskAssign">
                <TasksTemplate>
                    <tsk:SimpleTask ID="SimpleTask1" runat="server" TaskName="<%$ Resources:Resource, TaskNameVba32LoaderEnable %>"
                        TaskType="Vba32LoaderLaunch" />
                    <tsk:SimpleTask ID="SimpleTask2" runat="server" TaskName="<%$ Resources:Resource, TaskNameVba32LoaderDisable %>"
                        TaskType="Vba32LoaderExit" />
                    <tsk:SimpleTask ID="SimpleTask3" runat="server" TaskName="<%$ Resources:Resource, TaskNameVba32MonitorEnable %>"
                        TaskType="Vba32MonitorEnable" />
                    <tsk:SimpleTask ID="SimpleTask4" runat="server" TaskName="<%$ Resources:Resource, TaskNameVba32MonitorDisable %>"
                        TaskType="Vba32MonitorDisable" />
                    <tsk:SimpleTask ID="SimpleTask5" runat="server" TaskName="<%$ Resources:Resource, MenuSystemInfo %>"
                        TaskType="QuerySystemInformation" />
                    <tsk:SimpleTask ID="SimpleTask6" runat="server" TaskName="<%$ Resources:Resource, TaskNameListProcesses %>"
                        TaskType="QueryProcessesList" />
                    <tsk:SimpleTask ID="SimpleTask7" runat="server" TaskName="<%$ Resources:Resource, TaskNameComponentState %>"
                        TaskType="QueryComponentsState" />
                    <tsk:TaskOptionsDialog ID="TaskOptionsDialog1" runat="server" />
                    <tsk:CustomizableTask ID="CreateProcessTask" runat="server" TaskName="<%$ Resources:Resource, CreateProcess %>"
                        TaskOptionsID="CreateProcessTaskOptions1" />
                    <tsk:CreateProcessTaskOptions ID="CreateProcessTaskOptions1" runat="server" />

                   <tsk:CustomizableTask ID="ConfigurePasswordTask" runat="server" TaskName="<%$ Resources:Resource, TaskNameConfigurePassword %>"
                        TaskOptionsID="ConfigurePasswordTaskOptions1" />
                    <tsk:ConfigurePasswordTaskOptions ID="ConfigurePasswordTaskOptions1" runat="server" />

                    <tsk:CustomizableTask ID="RestoreFileFromQtnTask" runat="server" TaskName="<%$ Resources:Resource, TaskNameRestoreFileFromQtn %>"
                        TaskOptionsID="RestoreFileFromQtnTaskOptions1" />
                    <tsk:RestoreFileFromQtnTaskOptions ID="RestoreFileFromQtnTaskOptions1" runat="server" />

                    <tsk:CustomizableTask ID="UninstallTask" runat="server" TaskName="<%$ Resources:Resource, TaskUninstall %>"
                        TaskOptionsID="UninstallTaskOptions1" />
                    <tsk:UninstallTaskOptions ID="UninstallTaskOptions1" runat="server" />

                    <tsk:CustomizableTask ID="ProductInstallTask" runat="server" TaskName="<%$ Resources:Resource, Install %>"
                        TaskOptionsID="ProductInstallTaskOptions1" />
                    <tsk:ProductInstallTaskOptions ID="ProductInstallTaskOptions1" runat="server" />

                    <tsk:CustomizableTask ID="ChangeDeviceProtectTask" runat="server" TaskName="<%$ Resources:Resource, TaskChangeDeviceProtect %>"
                        TaskOptionsID="ChangeDeviceProtectTaskOptions1" />
                    <tsk:ChangeDeviceProtectTaskOptions ID="ChangeDeviceProtectTaskOptions1" runat="server" />

                    <tsk:CustomizableTask ID="TestTask" runat="server" TaskName="Test"
                        TaskOptionsID="TestTaskOptions1" />
                    <tsk:TestTaskOptions ID="TestTaskOptions1" runat="server" />
                </TasksTemplate>
            </tsk:TaskPanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
