<%@ Page Title="<%$ Resources:resource,PageProcessTitle %>" Language="C#" MasterPageFile="~/mstrPageNew.master" AutoEventWireup="true" Inherits="Default2" Codebehind="Default2.aspx.cs" %>
    <%@ Register TagPrefix="custom" Namespace="VirusBlokAda.CC.CustomControls" Assembly="CustomControls" %>
    <%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>

    <%@ Register Src="~/Controls/CompositeFilter.ascx" TagName="CompositeFilter" TagPrefix="flt" %>
    <%@ Register Src="~/Controls/PrimitiveFilterText.ascx" TagName="FilterText" TagPrefix="flt" %>
    <%@ Register Src="~/Controls/PrimitiveFilterRange.ascx" TagName="FilterRange" TagPrefix="flt" %>
    <%@ Register Src="~/Controls/PrimitiveFilterDateTime.ascx" TagName="FilterDateTime" TagPrefix="flt" %>

    <%@ Register Src="~/Controls/PrimitiveFilterComputers.ascx" TagName="FilterComputers"
    TagPrefix="flt" %>
    <%@ Register Src="~/Controls/AsyncLoadingStateControl.ascx" TagName="AsyncLoadingStateControl" TagPrefix="cc" %>
    <%@ Register Src="~/Controls/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="cc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" runat="Server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel runat="server" ID="updatePanelProcessesFilter">
     <ContentTemplate>
      <flt:CompositeFilter ID="FilterContainer" UserFilterCollectionTemproraryStorageName="ProcessFiltersTemp"
        UserFiltersProfileKey="ProcessFilters" runat="server" InformationListType="Processes"
        OnActiveFilterChange="FilterContainer_ActiveFilterChanged">
        <FiltersTemplate>
            <table>
                <tr>
                    <td valign="top">
                        <flt:FilterComputers runat="server" ID="fltComputers" NameFieldDB="ComputerName" TextFilter='<%$ Resources:Resource, ComputerName %>' />
                        <flt:FilterText runat="server" ID="fltIPAddress" NameFieldDB="ProcessName" TextFilter='<%$ Resources:Resource, ProcessName %>' />
                        <flt:FilterRange runat="server" ID="fltMemory" NameFieldDB="MemorySize" RangeCompareErrorMessage="Error" TextFilter='<%$ Resources:Resource, MemorySize %>'/>
                    </td>
                    <td valign="top" style="padding-left: 20px;">
                        <flt:FilterDateTime runat="server" ID="fltDateTime" NameFieldDB="LastDate" TextFilter="<%$ Resources:Resource, Date2 %>" />
                    </td>
                </tr>
            </table>
        </FiltersTemplate>
    </flt:CompositeFilter>
     </ContentTemplate>
    </asp:UpdatePanel>
   

    <div class="divSettings">
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" SelectCountMethod="Count"
            SelectMethod="Get" TypeName="ProcessDataContainer" SortParameterName="SortExpression">
            <SelectParameters>
                <asp:Parameter Name="where" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:UpdatePanel runat="server" ID="updatePanelProcessesGrid">
            <ContentTemplate>
                <custom:gridviewextended ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                    AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" EnableModelValidation="True"
                    CssClass="gridViewStyle" storagename="Processes2" storagetype="Session" EmptyDataText='<%$ Resources:Resource, EmptyMessage %>'>
                    <Columns>
                        <asp:HyperLinkField DataTextField="ComputerName" DataNavigateUrlFormatString="CompInfo.aspx?CompName={0}"
                            DataNavigateUrlFields="ComputerName" SortExpression="ComputerName" 
                            HeaderText='<%$ Resources:Resource, ComputerName %>'>
                            <HeaderStyle Width="200px"  />
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="ProcessName" SortExpression="ProcessName" 
                        HeaderText='<%$ Resources:Resource, ProcessName %>'>
                            <HeaderStyle Width="370px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MemorySize" SortExpression="MemorySize" 
                        HeaderText='<%$ Resources:Resource, MemorySizeInKb %>'>
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LastDate" SortExpression="LastDate" 
                        HeaderText='<%$ Resources:Resource, Date2 %>'>
                            <HeaderStyle Width="180px" />
                        </asp:BoundField>
                    </Columns>
                    <PagerSettings Position="TopAndBottom" Visible="true" />
                    <PagerTemplate>
                        <paging:paging runat="server" id="Paging1" />
                    </PagerTemplate>
                    <HeaderStyle CssClass="gridViewHeader" />
                    <AlternatingRowStyle CssClass="gridViewRowAlternating" />
                    <RowStyle CssClass="gridViewRow" />
                </custom:gridviewextended>
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
