<%@ Page Language="C#" EnableEventValidation="false" validateRequest="false" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="TasksInstall.aspx.cs" Inherits="TasksInstall" Title="<%$ Resources:resource,InstallUninstallTasks %>" %>

<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register TagPrefix="custom" Namespace="CustomControls" Assembly="CustomControls" %>

<%@ Register Src="~/Controls/CompositeFilter.ascx" TagName="CompositeFilter" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterComputers.ascx" TagName="FilterComputers" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDropDownList.ascx" TagName="FilterList" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDateTime.ascx" TagName="FilterDate" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterIpAddress.ascx" TagName="FilterIP" TagPrefix="flt" %>
<%@ Register Src="~/Controls/AsyncLoadingStateControl.ascx" TagName="AsyncLoadingStateControl" TagPrefix="cc" %>
<%@ Register Src="~/Controls/AutoUpdateControl.ascx" TagName="AutoUpdateControl"
    TagPrefix="cc" %>
    <%@ Register Src="~/Controls/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="cc" %>
<%@ OutputCache Location="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager runat="server" ID="ToolkitScriptManager1" EnableScriptGlobalization="true"></ajaxToolkit:ToolkitScriptManager>
<div class="title"><%=Resources.Resource.InstallUninstallTasks %></div>

<asp:UpdatePanel runat="server" ID="updatePanelComponentFilter">
     <ContentTemplate>
      <flt:CompositeFilter ID="FilterContainer1" UserFiltersTemproraryStorageName="TasksInstalFilltersTemp"
        UserFiltersProfileKey="TasksInstallFilters" OnClearClick="Filter1_ClearClick" runat="server"
        OnActiveFilterChange="FilterContainer_ActiveFilterChanged" InformationListType="None">
        <FiltersTemplate>
            <table>
                <tr>
                    <td valign="top">
                        <flt:FilterComputers runat="server" ID="fltComputers" NameFieldDB="ComputerName" Mode="InstallComputers" TextFilter='<%$ Resources:Resource, ComputerName %>' />
                        <flt:FilterIP runat="server" ID="fltIPAddress" NameFieldDB="IPAddress" TextFilter='<%$ Resources:Resource, IPAddress %>' />
                        <flt:FilterList runat="server" ID="fltTaskType" NameFieldDB="TaskType" TextFilter="<%$ Resources:Resource, TaskType %>" />
                    </td>
                    <td valign="top" style="padding-left: 20px;">
                        <flt:FilterList runat="server" ID="fltStatus" NameFieldDB="Status" TextFilter="<%$ Resources:Resource, Status %>" />
                        <flt:FilterList runat="server" ID="fltVBA32Version" NameFieldDB="Vba32Version" TextFilter="<%$ Resources:Resource, VBA32Version %>" />
                        <flt:FilterDate runat="server" ID="fltDate" NameFieldDB="InstallationDate" TextFilter="<%$ Resources:Resource, DateIssued %>" />
                    </td>
                </tr>
            </table>
        </FiltersTemplate>
    </flt:CompositeFilter>
     </ContentTemplate>
    </asp:UpdatePanel>
          
     <div class="divSettings">
     
         <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True"
                SelectCountMethod="Count" SelectMethod="Get" TypeName="InstallTasksDataContainer" SortParameterName="SortExpression">
                <SelectParameters>
                    <asp:Parameter Name="where" Type="String" />
                </SelectParameters>
         </asp:ObjectDataSource>
         
         <asp:UpdatePanel ID="updatePanelTasksInstallGrid" runat="server">
         <ContentTemplate>
            <custom:GridViewExtended ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                    AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" 
                    EnableModelValidation="True" CssClass="gridViewStyle"
                    StorageType="Session" StorageName="TasksInstall" EmptyDataText='<%$ Resources:Resource, EmptyMessage %>'>
                <Columns>                    
                    <asp:HyperLinkField DataTextField="ComputerName" SortExpression="ComputerName" 
                        HeaderText='<%$ Resources:Resource, ComputerName %>'>
                        <HeaderStyle Width="200px" />
                    </asp:HyperLinkField>                       
                    <asp:BoundField DataField="IPAddress" SortExpression="IPAddress" 
                        HeaderText='<%$ Resources:Resource, IPAddress %>'>
                        <HeaderStyle Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TaskType" SortExpression="TaskType" 
                        HeaderText='<%$ Resources:Resource, TaskType %>'>
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Vba32Version" SortExpression="Vba32Version" 
                        HeaderText='<%$ Resources:Resource, VBA32Version %>'>
                        <HeaderStyle Width="130px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="InstallationDate" SortExpression="InstallationDate" 
                        HeaderText='<%$ Resources:Resource, DateIssued %>'>
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" SortExpression="Status" 
                        HeaderText='<%$ Resources:Resource, Status %>'>
                        <HeaderStyle Width="130px" />
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
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="AutoUpdateControl1" EventName="AutoUpdate"></asp:AsyncPostBackTrigger>
            </Triggers>      
	     </asp:UpdatePanel>
     </div>

     <table width="100%" class="subWebParts">
        <tr>             
            <td align="left" style="width: 33%">
                <cc:AutoUpdateControl runat="server" ID="AutoUpdateControl1" InformationListType="Tasks" 
                 OnAutoUpdate="AutoUpdateControl1_AutoUpdate"/>
            </td>
			 <td align="right" style="width:33%">
                <cc:ExportToExcel runat="server" ID="ExportToExcel1"/>              
			 </td>
        </tr>
     </table>
             <cc:AsyncLoadingStateControl runat="server" ID="AsyncLoadingStateControl1" />  
</asp:Content>

