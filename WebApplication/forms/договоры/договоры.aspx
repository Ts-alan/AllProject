<%@Page AutoEventWireup="true" Inherits="TemplateEngine.WebSitePage" Language="C#" MasterPageFile="~/forms/��������/��������.master" %>
<asp:Content runat="server" ID="Body" ContentPlaceHolderID="BodyContent">
<asp:Panel ID="BodyWebForm" runat="server" Style="position:absolute; border-bottom: solid 1px #000000;  top:29pt; margin-bottom:0pt;" Width="100%" Height="322pt" BackColor="#d7e4bd">
<asp:UpdatePanel ID="BodyAjaxForm" runat="server">
<contenttemplate>
 <%-- ===== Begin template Body Sections ===== --%>
<asp:TextBox runat="server" TextMode="MultiLine" style="COLOR:#404040; POSITION:absolute; TOP:19pt; LEFT:203pt; HEIGHT:31px; WIDTH:493pt; BACKGROUND-COLOR:#ebf1de; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;border: #000000 solid 1px;" id="���"></asp:TextBox>
<asp:Label runat="server" id="�������0" style="COLOR:#7f7f7f; POSITION:absolute; TOP:19pt; LEFT:19pt; HEIGHT:31px; WIDTH:183pt; BACKGROUND-COLOR:transparent; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;" Text="���"></asp:Label>
<asp:TextBox runat="server" TextMode="MultiLine" style="COLOR:#404040; POSITION:absolute; TOP:57pt; LEFT:203pt; HEIGHT:31px; WIDTH:493pt; BACKGROUND-COLOR:#ebf1de; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;border: #000000 solid 1px;" id="���������.������������"></asp:TextBox>
<asp:Label runat="server" id="�������3" style="COLOR:#7f7f7f; POSITION:absolute; TOP:57pt; LEFT:19pt; HEIGHT:31px; WIDTH:183pt; BACKGROUND-COLOR:transparent; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;" Text="���������.������������"></asp:Label>
<asp:TextBox runat="server" TextMode="MultiLine" style="COLOR:#404040; POSITION:absolute; TOP:95pt; LEFT:203pt; HEIGHT:31px; WIDTH:493pt; BACKGROUND-COLOR:#ebf1de; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;border: #000000 solid 1px;" id="�����������������.������������"></asp:TextBox>
<asp:Label runat="server" id="�������6" style="COLOR:#7f7f7f; POSITION:absolute; TOP:95pt; LEFT:19pt; HEIGHT:31px; WIDTH:183pt; BACKGROUND-COLOR:transparent; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;" Text="[���� �������������].������������"></asp:Label>
<asp:TextBox runat="server" TextMode="MultiLine" style="COLOR:#404040; POSITION:absolute; TOP:134pt; LEFT:203pt; HEIGHT:31px; WIDTH:493pt; BACKGROUND-COLOR:#ebf1de; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;border: #000000 solid 1px;" id="��������.������������"></asp:TextBox>
<asp:Label runat="server" id="�������9" style="COLOR:#7f7f7f; POSITION:absolute; TOP:134pt; LEFT:19pt; HEIGHT:31px; WIDTH:183pt; BACKGROUND-COLOR:transparent; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;" Text="��������.������������"></asp:Label>
<asp:TextBox runat="server" style="COLOR:#404040; POSITION:absolute; TOP:172pt; LEFT:203pt; HEIGHT:20px; WIDTH:493pt; BACKGROUND-COLOR:#ebf1de; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;border: #000000 solid 1px;" id="��������������"></asp:TextBox>
<asp:Label runat="server" id="�������12" style="COLOR:#7f7f7f; POSITION:absolute; TOP:172pt; LEFT:19pt; HEIGHT:20px; WIDTH:183pt; BACKGROUND-COLOR:transparent; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;" Text="���� ����������"></asp:Label>
<asp:TextBox runat="server" TextMode="MultiLine" style="COLOR:#404040; POSITION:absolute; TOP:199pt; LEFT:203pt; HEIGHT:42px; WIDTH:493pt; BACKGROUND-COLOR:#ebf1de; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;border: #000000 solid 1px;" id="����������"></asp:TextBox>
<asp:Label runat="server" id="�������15" style="COLOR:#7f7f7f; POSITION:absolute; TOP:199pt; LEFT:19pt; HEIGHT:42px; WIDTH:183pt; BACKGROUND-COLOR:transparent; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;" Text="����������"></asp:Label>
<asp:TextBox runat="server" style="COLOR:#404040; POSITION:absolute; TOP:248pt; LEFT:203pt; HEIGHT:20px; WIDTH:493pt; BACKGROUND-COLOR:#ebf1de; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;border: #000000 solid 1px;" id="�����"></asp:TextBox>
<asp:Label runat="server" id="�������18" style="COLOR:#7f7f7f; POSITION:absolute; TOP:248pt; LEFT:19pt; HEIGHT:20px; WIDTH:183pt; BACKGROUND-COLOR:transparent; FONT-FAMILY:Calibri; FONT-SIZE:11pt;text-align:left;" Text="�����"></asp:Label>
<asp:Button runat="server" style="COLOR:#404040; POSITION:absolute; TOP:285pt; LEFT:143pt; HEIGHT:30px; WIDTH:59pt; background: url(./images/BG_������23_.bmp) center no-repeat;BACKGROUND-COLOR:#f0f0f0; FONT-FAMILY:Calibri; FONT-SIZE:11pt;" id="������23" Text="" onClick="������23_Click"></asp:Button>
<asp:Button runat="server" style="COLOR:#404040; POSITION:absolute; TOP:285pt; LEFT:228pt; HEIGHT:30px; WIDTH:58pt; background: url(./images/BG_������24_.bmp) center no-repeat;BACKGROUND-COLOR:#f0f0f0; FONT-FAMILY:Calibri; FONT-SIZE:11pt;" id="������24" Text="" onClick="������24_Click"></asp:Button>
<asp:Button runat="server" style="COLOR:#404040; POSITION:absolute; TOP:285pt; LEFT:313pt; HEIGHT:30px; WIDTH:58pt; background: url(./images/BG_������25_.bmp) center no-repeat;BACKGROUND-COLOR:#f0f0f0; FONT-FAMILY:Calibri; FONT-SIZE:11pt;" id="������25" Text="" onClick="������25_Click"></asp:Button>
<asp:Button runat="server" style="COLOR:#404040; POSITION:absolute; TOP:285pt; LEFT:398pt; HEIGHT:30px; WIDTH:58pt; background: url(./images/BG_������26_.bmp) center no-repeat;BACKGROUND-COLOR:#f0f0f0; FONT-FAMILY:Calibri; FONT-SIZE:11pt;" id="������26" Text="" onClick="������26_Click"></asp:Button>
<%-- ===== End template Body Sections ===== --%>
</contenttemplate>
</asp:UpdatePanel>
</asp:Panel>
</asp:Content>

<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent">
<asp:Panel ID="HeaderWebForm" runat="server" Style="position:absolute; border-bottom: solid 1px #000000; " Width="100%" Height="29pt" BackColor="#c3d69b">
<asp:UpdatePanel ID="HeaderAjaxForm" runat="server">
<contenttemplate>
 <%-- ===== Begin template Header Sections ===== --%>
<asp:Panel runat="server" id="����_�������0" style="BORDER:0pt none #a6a6a6; POSITION:absolute; TOP:5pt; LEFT:17pt; HEIGHT:25px; WIDTH:36pt; background-image:url(./images/����_�������0.bmp); background-repeat:no-repeat; background-position:center "></asp:Panel><asp:Label runat="server" id="����_���������0" style="COLOR:#1f497d; POSITION:absolute; TOP:5pt; LEFT:54pt; HEIGHT:25px; WIDTH:557pt; BACKGROUND-COLOR:transparent; FONT-FAMILY:Cambria; FONT-SIZE:18pt;text-align:left;" Text="��������, ����������� ������������"></asp:Label>
<%-- ===== End template Header Sections ===== --%>
</contenttemplate>
</asp:UpdatePanel>
</asp:Panel>
<cc1:AlwaysVisibleControlExtender ID="AlwaysVisibleHeader" runat="server" HorizontalSide="Center" ScrollEffectDuration=".1" TargetControlID="HeaderWebForm" VerticalSide="Top"></cc1:AlwaysVisibleControlExtender>
</asp:Content>

<asp:Content runat="server" ID="Footer" ContentPlaceHolderID="FooterContent">
<asp:Panel ID="FooterWebForm" runat="server" Style="position:absolute; border-bottom: solid 1px #000000; " Width="100%" Height="0pt" BackColor="#ffffff">
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
