<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AutoUpdateControl.ascx.cs"
    Inherits="Controls_AutoUpdateControl" %>
<asp:Timer ID="timerAutoUpdate" runat="server" OnTick="timerAutoUpdate_Tick">
</asp:Timer>
<asp:UpdatePanel ID="updatePanelAutoUpdate" runat="server">
    <ContentTemplate>
        <asp:CheckBox ID="cboxAutoUpdate" runat="server" Checked="false" AutoPostBack="True" OnCheckedChanged="cboxAutoUpdate_CheckedChanged" />
    </ContentTemplate>
</asp:UpdatePanel>
