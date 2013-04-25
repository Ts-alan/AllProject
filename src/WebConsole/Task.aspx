<%@ Page Language="C#" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/mstrPageMain.master"
    AutoEventWireup="true" CodeFile="Task.aspx.cs" Inherits="Task" Title="<%$ Resources:resource,PageTaskTitle %>" %>

<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register TagPrefix="custom" Namespace="CustomControls" Assembly="CustomControls" %>
<%@ Register Src="~/Controls/CompositeFilter.ascx" TagName="CompositeFilter" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterText.ascx" TagName="FilterText" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterComputers.ascx" TagName="FilterComputers"
    TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDropDownList.ascx" TagName="FilterList"
    TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDateTime.ascx" TagName="FilterDate" TagPrefix="flt" %>
<%@ Register Src="~/Controls/AsyncLoadingStateControl.ascx" TagName="AsyncLoadingStateControl"
    TagPrefix="cc" %>
<%@ Register Src="~/Controls/AutoUpdateControl.ascx" TagName="AutoUpdateControl"
    TagPrefix="cc" %>
<%@ Register Src="~/Controls/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="cc" %>
<%@ OutputCache Location="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="server" ID="ToolkitScriptManager1" EnableScriptGlobalization="true">
    </ajaxToolkit:ToolkitScriptManager>
    <script type="text/javascript" language="javascript">
        function SelectAll() {
            var gv = $get('<%=GridView1.ClientID%>');

            var isChecked = gv.rows[1].cells[0].children[0].children[0].checked;

            for (var i = 2; i < gv.rows.length - 1; i++) {
                var rowElem = gv.rows[i];
                var cell = rowElem.cells[0];

                if (cell.children[0].children[0].disabled == false)
                    cell.children[0].children[0].checked = isChecked;
            }
        }
    </script>
    <div class="title">
        <%=Resources.Resource.PageTaskTitle%></div>
    <asp:UpdatePanel runat="server" ID="updatePanelComponentFilter">
        <ContentTemplate>
            <flt:CompositeFilter ID="FilterContainer1" UserFiltersTemproraryStorageName="TaskFiltersTemp"
                UserFiltersProfileKey="TaskFilters" OnClearClick="Filter1_ClearClick" runat="server"
                OnActiveFilterChange="FilterContainer_ActiveFilterChanged" InformationListType="Tasks">
                <FiltersTemplate>
                    <table>
                        <tr>
                            <td valign="top">
                                <flt:FilterComputers runat="server" ID="fltComputers" NameFieldDB="ComputerName"
                                    TextFilter='<%$ Resources:Resource, ComputerName %>' />
                                <flt:FilterText runat="server" ID="fltTaskName" NameFieldDB="TaskName" TextFilter='<%$ Resources:Resource, TaskName %>' />
                                <flt:FilterList runat="server" ID="fltState" NameFieldDB="TaskState" TextFilter='<%$ Resources:Resource, TaskState %>' />
                                <flt:FilterDate runat="server" ID="fltAssignmentDate" NameFieldDB="DateIssued" TextFilter='<%$ Resources:Resource, DateIssued %>' />
                            </td>
                            <td valign="top" style="padding-left: 20px;">
                                <flt:FilterDate runat="server" ID="fltCompletionDate" NameFieldDB="DateComplete"
                                    TextFilter='<%$ Resources:Resource, DateComplete %>' />
                                <flt:FilterDate runat="server" ID="fltUpdateDate" NameFieldDB="DateUpdated" TextFilter='<%$ Resources:Resource, DateUpdated %>' />
                                <flt:FilterText runat="server" ID="fltUserName" NameFieldDB="TaskUser" TextFilter='<%$ Resources:Resource, User %>' />
                            </td>
                        </tr>
                    </table>
                </FiltersTemplate>
            </flt:CompositeFilter>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="divSettings">
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" SelectCountMethod="Count"
            SelectMethod="Get" TypeName="TasksDataContainer" SortParameterName="SortExpression">
            <SelectParameters>
                <asp:Parameter Name="where" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:UpdatePanel ID="updatePanelEvent" runat="server">
            <ContentTemplate>
                <custom:GridViewExtended ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                    AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" OnRowDataBound="GridView1_RowDataBound"
                    EnableModelValidation="True" CssClass="gridViewStyle" StorageType="Session" StorageName="Tasks"
                    EmptyDataText='<%$ Resources:Resource, EmptyMessage %>'>
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle Width="60px" />
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="cboxSelectAll" onclick="SelectAll()" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cboxIsSelected" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:HyperLinkField DataTextField="ComputerName" DataNavigateUrlFormatString="CompInfo.aspx?CompName={0}"
                            DataNavigateUrlFields="ComputerName" SortExpression="ComputerName" HeaderText='<%$ Resources:Resource, ComputerName %>'>
                            <HeaderStyle Width="150px" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField DataTextField="TaskName" DataNavigateUrlFormatString="TaskCreate.aspx?ID={0}"
                            DataNavigateUrlFields="ID" SortExpression="TaskName" HeaderText='<%$ Resources:Resource, TaskName %>'>
                            <HeaderStyle Width="150px" />
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="TaskState" SortExpression="TaskState" HeaderText='<%$ Resources:Resource, TaskState %>'>
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AsDateIssued" SortExpression="DateIssued" HeaderText='<%$ Resources:Resource, DateIssued %>'>
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AsDateComplete" SortExpression="DateComplete" HeaderText='<%$ Resources:Resource, DateComplete %>'>
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AsDateUpdated" SortExpression="DateUpdated" HeaderText='<%$ Resources:Resource, DateUpdated %>'>
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TaskUser" SortExpression="TaskUser" HeaderText='<%$ Resources:Resource, User %>'>
                            <HeaderStyle Width="164px" />
                        </asp:BoundField>                        
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="lblTaskID" Text='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Position="TopAndBottom" Visible="true" />
                    <PagerTemplate>
                        <paging:Paging runat="server" ID="Paging1" />
                    </PagerTemplate>
                    <HeaderStyle CssClass="gridViewHeader" />
                    <AlternatingRowStyle CssClass="gridViewRowAlternating" />
                    <RowStyle CssClass="gridViewRow" />
                </custom:GridViewExtended>
                <div style="text-align: center;">
                    <div class="GiveButton1" style="float: left;">
                        <asp:LinkButton runat="server" ID="lbtnCancel" OnClick="lbtnCancel_Click" Width="100%"
                            ForeColor="white"></asp:LinkButton>
                    </div>
                    <div style="min-width: 300px;">
                        <asp:Label runat="server" ID="lblMessage"></asp:Label>
                    </div>
                </div>
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
                <cc:AutoUpdateControl runat="server" ID="AutoUpdateControl1" InformationListType="Tasks"
                    OnAutoUpdate="AutoUpdateControl1_AutoUpdate" />
            </td>
            <td align="right" style="width: 33%">
                <cc:ExportToExcel runat="server" ID="ExportToExcel1"/>
            </td>
        </tr>
    </table>
    <cc:AsyncLoadingStateControl runat="server" ID="AsyncLoadingStateControl1" />
</asp:Content>
