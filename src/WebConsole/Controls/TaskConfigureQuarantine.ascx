<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureQuarantine.ascx.cs" Inherits="Controls_TaskConfigureQuarantine" %>
<div class="tasksection" runat="server" id="HeaderName" style="width:100%"><%=Resources.Resource.TaskNameConfigureQuarantine%></div>

<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        $("#TabsQuarantine").tabs({ cookie: { expires: 30} });
    });



</script>
<div id="TabsQuarantine" style="width:560px">
    <ul>
        <li><a href="#tab1"><%=Resources.Resource.JournalEvents%></a> </li>
    </ul>
    <div id="tab1" class="divSettings">
        <asp:UpdatePanel ID="JournalEventUpdatePanel" runat="server">
            <ContentTemplate>
                <asp:Panel ID="JournalEventPanel" runat='server' EnableViewState="false">
                    <asp:Table ID="JournalEventTable" runat="server" CssClass="ListContrastTable" rules="cols">
                        <asp:TableHeaderRow ID="TableHeaderRow1"  runat="server" CssClass="gridViewHeader">
                            <asp:TableHeaderCell runat="server" id="tdEvent" style="width: 150px;text-align: center;" >
                                <asp:Label runat="server" ><%=Resources.Resource.Events %></asp:Label>                            
                            </asp:TableHeaderCell>
                            <asp:TableHeaderCell runat="server" id="tdWindowsJournal" style="width: 120px;text-align: center;" >
                                <asp:Label runat="server" ><%=Resources.Resource.WindowsJournal %></asp:Label>
                            </asp:TableHeaderCell>
                            <asp:TableHeaderCell runat="server" id="tdLocalJournal" style="width: 120px;text-align: center;" >
                                <asp:Label runat="server" ><%=Resources.Resource.LocalJournal %></asp:Label>
                            </asp:TableHeaderCell>
                            <asp:TableHeaderCell runat="server" id="tdCCJournal" style="width: 120px;text-align: center;">
                                <asp:Label runat="server"  ><%=Resources.Resource.CCJournal %></asp:Label>
                            </asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
   
</div>
