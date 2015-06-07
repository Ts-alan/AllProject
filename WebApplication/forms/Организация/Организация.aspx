<%@Page AutoEventWireup="true" Inherits="TemplateEngine.WebSitePage" Language="C#" MasterPageFile="~/forms/Организация/Организация.master" %>
<asp:Content runat="server" ID="Body" ContentPlaceHolderID="BodyContent">
<asp:Panel ID="BodyWebForm" runat="server" Style="position:absolute; border-bottom: solid 1px #000000;  top:58pt; margin-bottom:30pt;" Width="100%" Height="146pt" BackColor="#d7e4bd">
<asp:UpdatePanel ID="BodyAjaxForm" runat="server">
<contenttemplate>
 <%-- ===== Begin template Body Sections ===== --%>
<asp:Button runat="server" style="COLOR:#404040; POSITION:absolute; TOP:30pt; LEFT:200pt; HEIGHT:22px; WIDTH:67pt;  FONT-FAMILY:Calibri; FONT-SIZE:11pt;" id="Кнопка4" Text="Сотрудники" onClick="Кнопка4_Click"></asp:Button>
<asp:Button runat="server" style="COLOR:#404040; POSITION:absolute; TOP:61pt; LEFT:30pt; HEIGHT:22px; WIDTH:65pt;  FONT-FAMILY:Calibri; FONT-SIZE:11pt;" id="Кнопка5" Text="Документы" onClick="Кнопка5_Click"></asp:Button>
<asp:Button runat="server" style="COLOR:#404040; POSITION:absolute; TOP:87pt; LEFT:143pt; HEIGHT:22px; WIDTH:47pt;  FONT-FAMILY:Calibri; FONT-SIZE:11pt;" id="Кнопка6" Text="Оклады" onClick="Кнопка6_Click"></asp:Button>
<asp:Button runat="server" style="COLOR:#404040; POSITION:absolute; TOP:115pt; LEFT:228pt; HEIGHT:22px; WIDTH:58pt;  FONT-FAMILY:Calibri; FONT-SIZE:11pt;" id="Кнопка8" Text="Договоры" onClick="Кнопка8_Click"></asp:Button>
<%-- ===== End template Body Sections ===== --%>
</contenttemplate>
</asp:UpdatePanel>
</asp:Panel>
</asp:Content>

<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent">
<asp:Panel ID="HeaderWebForm" runat="server" Style="position:absolute; border-bottom: solid 1px #000000; " Width="100%" Height="58pt" BackColor="#c3d69b">
<asp:UpdatePanel ID="HeaderAjaxForm" runat="server">
<contenttemplate>
 <%-- ===== Begin template Header Sections ===== --%>
<asp:Label runat="server" id="Авто_Заголовок0" style="COLOR:#5ff20e; POSITION:absolute; TOP:3pt; LEFT:13pt; HEIGHT:47px; WIDTH:335pt; BACKGROUND-COLOR:#ebf1de; FONT-FAMILY:Cambria; FONT-SIZE:18pt;font-weight:bold;font-style:italic;text-align:left;" Text="Договорная деятельность организации поставок товаров на склады"></asp:Label>
<%-- ===== End template Header Sections ===== --%>
</contenttemplate>
</asp:UpdatePanel>
</asp:Panel>
<cc1:AlwaysVisibleControlExtender ID="AlwaysVisibleHeader" runat="server" HorizontalSide="Center" ScrollEffectDuration=".1" TargetControlID="HeaderWebForm" VerticalSide="Top"></cc1:AlwaysVisibleControlExtender>
</asp:Content>

<asp:Content runat="server" ID="Footer" ContentPlaceHolderID="FooterContent">
<asp:Panel ID="FooterWebForm" runat="server" Style="position:absolute; border-bottom: solid 1px #000000; " Width="100%" Height="30pt" BackColor="#ebf1de">
<asp:UpdatePanel ID="FooterAjaxForm" runat="server">
<contenttemplate>
 <%-- ===== Begin template Footer Sections ===== --%>
<%-- ===== End template Footer Sections ===== --%>
</contenttemplate>
</asp:UpdatePanel>
</asp:Panel>
<cc1:AlwaysVisibleControlExtender ID="AlwaysVisibleFooter" runat="server" HorizontalSide="Center" ScrollEffectDuration=".1" TargetControlID="FooterWebForm" VerticalOffset="20" VerticalSide="Bottom"></cc1:AlwaysVisibleControlExtender>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="NavigationContent">
<asp:UpdatePanel ID="NavPanel" runat="server">
<contenttemplate><ctrl:NavigationPanel ID="NavigationPanel_" runat="server" ScriptManagerID="AjaxScriptManager" style="position:relative; background-color: #f0f0f0; font-size: 10px; font-family: Arial, Helvetica, sans-serif;" Width="100%" />
<cc1:AlwaysVisibleControlExtender id="AlwaysVisibleControlExtender" runat="server" horizontalside="Center" scrolleffectduration=".1" targetcontrolid="NavigationPanel_" verticalside="Bottom" />
</contenttemplate></asp:UpdatePanel></asp:Content>

<script runat="server" src="ButtonEvents.cs"></script>
