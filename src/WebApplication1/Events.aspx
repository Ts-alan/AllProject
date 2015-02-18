<%@ Page Title="<%$ Resources:resource,PageEventsTitle %>" Language="C#" MasterPageFile="~/mstrPageNew.master"
    AutoEventWireup="true" Inherits="Events" Codebehind="Events.aspx.cs" %>

<%@ Register TagPrefix="custom" Namespace="VirusBlokAda.CC.CustomControls" Assembly="CustomControls" %>
<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register Src="~/Controls/CompositeFilter.ascx" TagName="CompositeFilter" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterText.ascx" TagName="FilterText" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterRange.ascx" TagName="FilterRange" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDropDownList.ascx" TagName="FilterList"
    TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDateTime.ascx" TagName="FilterDateTime"
    TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterComputers.ascx" TagName="FilterComputers"
    TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterIpAddress.ascx" TagName="FilterIpAddress"
    TagPrefix="flt" %>
<%@ Register Src="~/Controls/AsyncLoadingStateControl.ascx" TagName="AsyncLoadingStateControl"
    TagPrefix="cc" %>
<%@ Register Src="~/Controls/AutoUpdateControl.ascx" TagName="AutoUpdateControl"
    TagPrefix="cc" %>
<%@ Register Src="~/Controls/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="cc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <div class="title">
        <%=Resources.Resource.PageEventsTitle%>
    </div>
    <asp:UpdatePanel runat="server" ID="updatePanelEventsFilter">
        <ContentTemplate>
            <flt:CompositeFilter ID="FilterContainer" UserFiltersTemproraryStorageName="EventFiltersTemp"
                InformationListType="Events" UserFiltersProfileKey="EventFilters" runat="server"
                OnActiveFilterChange="FilterContainer_ActiveFilterChanged">
                <FiltersTemplate>
                    <table>
                        <tr>
                            <td valign="top">
                                <flt:FilterComputers runat="server" ID="fltComputers" NameFieldDB="ComputerName"
                                    TextFilter='<%$ Resources:Resource, ComputerName %>' />
                                <flt:FilterIpAddress runat="server" ID="fltIPAddress" NameFieldDB="IpAddress" TextFilter='<%$ Resources:Resource, IpAddress %>' />
                                <flt:FilterText runat="server" ID="fltDescription" NameFieldDB="Description" TextFilter='<%$ Resources:Resource, Description %>' />
                                <flt:FilterText runat="server" ID="fltObject" NameFieldDB="Object" TextFilter='<%$ Resources:Resource, Object %>' />
                                <flt:FilterText runat="server" ID="fltEvent" NameFieldDB="EventName" TextFilter='<%$ Resources:Resource, EventName %>' />
                            </td>
                            <td valign="top" style="padding-left: 20px;">
                                <flt:FilterList runat="server" ID="fltComponent" NameFieldDB="ComponentName" TextFilter='<%$ Resources:Resource, Component %>' />
                                <flt:FilterText runat="server" ID="fltComment" NameFieldDB="Comment" TextFilter='<%$ Resources:Resource, Comment %>' />
                                <flt:FilterDateTime runat="server" ID="fltEventTime" NameFieldDB="EventTime" TextFilter="<%$ Resources:Resource, EventTime %>" />
                            </td>
                        </tr>
                    </table>
                </FiltersTemplate>
            </flt:CompositeFilter>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="divSettings">
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" SelectCountMethod="Count"
            SelectMethod="Get" TypeName="EventsDataContainer" SortParameterName="SortExpression">
            <SelectParameters>
                <asp:Parameter Name="where" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:UpdatePanel ID="updatePanelEventsGrid" runat="server">
            <ContentTemplate>
                <custom:GridViewExtended ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                    AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" OnRowDataBound="GridView1_RowDataBound"
                    EnableModelValidation="True" CssClass="gridViewStyle" StorageType="Session" StorageName="Events"
                    EmptyDataText='<%$ Resources:Resource, EmptyMessage %>'>
                    <Columns>
                        <asp:HyperLinkField DataTextField="ComputerName" DataNavigateUrlFormatString="CompInfo.aspx?CompName={0}"
                            DataNavigateUrlFields="ComputerName" SortExpression="ComputerName" HeaderText='<%$ Resources:Resource, ComputerName %>'>
                            <HeaderStyle Width="150px" />
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="IPAddress" SortExpression="IPAddress" HeaderText='<%$ Resources:Resource, IPAddress %>'>
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" SortExpression="Description" HeaderText='<%$ Resources:Resource, Description %>'>
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText='<%$ Resources:Resource, EventName %>'>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblEventName"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="EventName" SortExpression="EventName" HeaderText='<%$ Resources:Resource, EventName %>'>
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EventTime" SortExpression="EventTime" HeaderText='<%$ Resources:Resource, EventTime %>'>
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:TemplateField SortExpression="ComponentName" HeaderText='<%$ Resources:Resource, Component %>'>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblComponentName"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Object" SortExpression="Object" HeaderText='<%$ Resources:Resource, Object %>'>
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Comment" SortExpression="Comment" HtmlEncode="false" HeaderText='<%$ Resources:Resource, Comment %>'>
                            <HeaderStyle Width="100px" />
                            <ItemStyle CssClass="word-wrap-cell device-serial-col" />
                        </asp:BoundField>
                    </Columns>
                    <PagerSettings Position="TopAndBottom" Visible="true" />
                    <PagerTemplate>
                        <paging:Paging runat="server" ID="Paging1" />
                    </PagerTemplate>
                    <HeaderStyle CssClass="gridViewHeader" />
                    <AlternatingRowStyle CssClass="gridViewRowAlternating" />
                    <RowStyle CssClass="gridViewRow" />
                </custom:GridViewExtended>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="AutoUpdateControl1" EventName="AutoUpdate">
                </asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <table width="100%" class="subWebParts">
        <tr>
            <td align="left" style="width: 33%">
                <cc:AutoUpdateControl runat="server" ID="AutoUpdateControl1" InformationListType="Events"
                    OnAutoUpdate="AutoUpdateControl1_AutoUpdate" />
            </td>
            <td align="right" style="width: 33%">
                <cc:ExportToExcel runat="server" ID="ExportToExcel1"/>
            </td>
        </tr>
    </table>
    <cc:AsyncLoadingStateControl runat="server" ID="AsyncLoadingStateControl1" />
</asp:Content>
