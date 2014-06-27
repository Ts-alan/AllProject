<%@ Page Language="C#" MasterPageFile="~/mstrPageNew.master" AutoEventWireup="true" CodeFile="Components.aspx.cs" Inherits="Components" Title="<%$ Resources:resource,PageComponentsTitle %>" %>
<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register TagPrefix="custom" Namespace="VirusBlokAda.CC.CustomControls" Assembly="CustomControls" %>

<%@ Register Src="~/Controls/CompositeFilter.ascx" TagName="CompositeFilter" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterText.ascx" TagName="FilterText" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterComputers.ascx" TagName="FilterComputers" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDropDownList.ascx" TagName="FilterList" TagPrefix="flt" %>
<%@ Register Src="~/Controls/AsyncLoadingStateControl.ascx" TagName="AsyncLoadingStateControl" TagPrefix="cc" %>
<%@ Register Src="~/Controls/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="cc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="server" ID="ToolkitScriptManager1" EnableScriptGlobalization="true"></ajaxToolkit:ToolkitScriptManager>

    <div class="title"><%=Resources.Resource.PageComponentsTitle%></div>
    <asp:UpdatePanel runat="server" ID="updatePanelComponentFilter">
        <ContentTemplate>
            <flt:CompositeFilter ID="FilterContainer1" UserFiltersTemproraryStorageName="ComponentFiltersTemp"
                UserFiltersProfileKey="ComponentFilters" OnClearClick="Filter1_ClearClick" runat="server"
                OnActiveFilterChange="FilterContainer_ActiveFilterChanged" InformationListType="Components">
                <FiltersTemplate>
                    <table>
                        <tr>
                            <td valign="top">
                                <flt:FilterComputers runat="server" ID="fltComputers" NameFieldDB="ComputerName" TextFilter='<%$ Resources:Resource, ComputerName %>' />
                                <flt:FilterList runat="server" ID="fltComponentName" NameFieldDB="ComponentName" TextFilter='<%$ Resources:Resource, ComponentName %>' />
                                <flt:FilterList runat="server" ID="fltComponentState" NameFieldDB="ComponentState" TextFilter="<%$ Resources:Resource, State %>" />
                            </td>
                            <td valign="top" style="padding-left: 20px;">
                                <flt:FilterText runat="server" ID="fltVersion" NameFieldDB="Version" TextFilter='<%$ Resources:Resource, Version %>' />
                                <flt:FilterText runat="server" ID="fltSettings" NameFieldDB="Name" TextFilter='<%$ Resources:Resource, Settings %>' />
                            </td>
                        </tr>
                    </table>
            </FiltersTemplate>
        </flt:CompositeFilter>
    </ContentTemplate>
    </asp:UpdatePanel>
    
    <div class="divSettings">
                 
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True"
            SelectCountMethod="Count" SelectMethod="Get" TypeName="ComponentsDataContainer" SortParameterName="SortExpression">
            <SelectParameters>
                <asp:Parameter Name="where" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        
        <asp:UpdatePanel runat="server" ID="updatePanelComponentsGrid">
            <ContentTemplate>
                <custom:GridViewExtended ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                    AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" OnRowDataBound="GridView1_RowDataBound" 
                    EnableModelValidation="True" CssClass="gridViewStyle"
                    StorageType="Session" StorageName="Components" EmptyDataText='<%$ Resources:Resource, EmptyMessage %>'>
                    <Columns>                    
                        <asp:HyperLinkField DataTextField="ComputerName" DataNavigateUrlFormatString="CompInfo.aspx?CompName={0}" DataNavigateUrlFields="ComputerName" SortExpression="ComputerName" 
                            HeaderText='<%$Resources:Resource, ComputerName%>' >
                            <HeaderStyle Width="200px" />
                        </asp:HyperLinkField>                       
                        <asp:TemplateField SortExpression="ComponentName" HeaderText='<%$ Resources:Resource, Component %>'>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblComponentName"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="190px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ComponentState" SortExpression="ComponentState" 
                            HeaderText='<%$Resources:Resource, ComponentState%>'>
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Version" SortExpression="Version" 
                            HeaderText='<%$Resources:Resource, Version%>'>
                            <HeaderStyle Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Name" SortExpression="Name" 
                            HeaderText='<%$Resources:Resource, Settings%>'>
                            <HeaderStyle Width="180px" />
                        </asp:BoundField>
                    </Columns> 
                    <PagerSettings Position="TopAndBottom" Visible="true" />           
                    <PagerTemplate>
                        <paging:Paging runat="server" ID="Paging1" />
                    </PagerTemplate>
                    <HeaderStyle CssClass="gridViewHeader" />
                    <AlternatingRowStyle CssClass = "gridViewRowAlternating" />
                    <RowStyle CssClass="gridViewRow" />
                </custom:GridViewExtended>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <table width="100%" class="subWebParts">
        <tr>
            <td align="left" style="width:33%">
            </td> 
			<td align="right" style="width:33%">
                <cc:ExportToExcel runat="server" ID="ExportToExcel1"/>
			</td>
        </tr>
    </table>
    <cc:AsyncLoadingStateControl runat="server" ID="AsyncLoadingStateControl1" />  
</asp:Content>