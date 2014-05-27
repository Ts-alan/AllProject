<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureJornalEvents.ascx.cs" Inherits="Controls_TaskConfigureJornalEvents" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.JournalEvents%></div>
<div class="divSettings">
    <asp:UpdatePanel ID="JournalEventUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Panel ID="JournalEventPanel" runat='server' EnableViewState="false">
                <asp:Table ID="JournalEventTable" runat="server" CssClass="ListContrastTable">
                    <asp:TableHeaderRow ID="TableHeaderRow1" runat="server">
                        <asp:TableHeaderCell runat="server" id="tdEvent" style="width: 150px;text-align: center;" class="listRulesHeader">
                            <asp:Label ID="Label1" runat="server" ><%=Resources.Resource.Events %></asp:Label>                            
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell runat="server" id="tdWindowsJournal" style="width: 120px;text-align: center;" class="listRulesHeader">
                            <asp:Label ID="Label2" runat="server" ><%=Resources.Resource.WindowsJournal %></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell runat="server" id="tdLocalJournal" style="width: 120px;text-align: center;" class="listRulesHeader">
                            <asp:Label ID="Label3" runat="server" ><%=Resources.Resource.LocalJournal %></asp:Label>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell runat="server" id="tdCCJournal" style="width: 120px;text-align: center;" class="listRulesHeader">
                            <asp:Label ID="Label4" runat="server"  ><%=Resources.Resource.CCJournal %></asp:Label>
                        </asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>