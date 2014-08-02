<%@ Page Language="C#" MaintainScrollPositionOnPostback="false" EnableEventValidation="false" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="Processes.aspx.cs" Inherits="Processes" Title="Untitled Page" %>
<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register TagPrefix="custom" Namespace="VirusBlokAda.CC.CustomControls" Assembly="CustomControls" %>

<%@ Register Src="~/Controls/CompositeFilter.ascx" TagName="CompositeFilter" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterText.ascx" TagName="FilterText" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterRange.ascx" TagName="FilterRange" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDateTime.ascx" TagName="FilterDateTime" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterComputers.ascx" TagName="FilterComputers" TagPrefix="flt" %>
<%@ Register Src="~/Controls/AsyncLoadingStateControl.ascx" TagName="AsyncLoadingStateControl"
    TagPrefix="cc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="server" ID="ToolkitScriptManager1" EnableScriptGlobalization="true"></ajaxToolkit:ToolkitScriptManager>
   
    <script type="text/javascript">
        $(document).ready(function () {
            $(document).on("click", 'a[id=lbtnTerminate]', function () {
                var sender = $(this);
                var procName = sender.attr('procName');
                var compName = sender.attr('compName');

                $.ajax({
                    type: "POST",
                    url: "Processes.aspx/TerminateProcess",
                    dataType: "json",
                    data: "{procName:\"" + procName + "\", compName:\"" + compName + "\"}",
                    contentType: "application/json; charset=utf-8",
                    error: function (msg) {
                        ShowJSONMessage(msg);
                    },
                    success: function (msg) {
                        alert(msg);
                    }
                });
            });

            function ShowJSONMessage(msg) {
                var m = JSON.parse(msg.responseText, function (key, value) {
                    var type;
                    if (value && typeof value === 'object') {
                        type = value.type;
                        if (typeof type === 'string' && typeof window[type] === 'function') {
                            return new (window[type])(value);
                        }
                    }
                    return value;
                });
                alert(m.Message);
            }
        });
    </script>

    <div class="title"><%=Resources.Resource.PageProcessTitle%></div>
    <asp:UpdatePanel runat="server" ID="updatePanelProcessesFilter">
        <ContentTemplate>
            <flt:CompositeFilter ID="FilterContainer" UserFiltersTemproraryStorageName="ProcessFiltersTemp" InformationListType="Processes"
                UserFiltersProfileKey="ProcessFilters" OnClearClick="Filter1_ClearClick" runat="server"
                OnActiveFilterChange="FilterContainer_ActiveFilterChanged">
                <FiltersTemplate>
                    <table>
                        <tr>
                            <td valign="top">
                                <flt:FilterComputers runat="server" ID="fltComputers" NameFieldDB="ComputerName" TextFilter='<%$ Resources:Resource, ComputerName %>' />
                                <flt:FilterText runat="server" ID="fltProcessName" NameFieldDB="ProcessName" TextFilter='<%$ Resources:Resource, ProcessName %>' />
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
                 
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True"
            SelectCountMethod="Count" SelectMethod="Get" TypeName="ProcessDataContainer" SortParameterName="SortExpression">
            <SelectParameters>
                <asp:Parameter Name="where" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        
        <asp:UpdatePanel runat="server" ID="updatePanelProcessesGrid">
            <ContentTemplate>
                <custom:GridViewExtended ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                    AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" 
                    EnableModelValidation="True" CssClass="gridViewStyle"
                    StorageType="Session" StorageName="Processes" >
                    <Columns>                    
                        <asp:HyperLinkField DataTextField="ComputerName" DataNavigateUrlFormatString="CompInfo.aspx?CompName={0}" DataNavigateUrlFields="ComputerName" SortExpression="ComputerName" 
                            HeaderText='<%$ Resources:Resource, ComputerName %>' >
                            <HeaderStyle Width="200px" />
                        </asp:HyperLinkField>                       
                        <asp:BoundField DataField="ProcessName" SortExpression="ProcessName" 
                            HeaderText='<%$ Resources:Resource, ProcessName %>'>
                            <HeaderStyle Width="370px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MemorySize" SortExpression="MemorySize" 
                            HeaderText='<%$ Resources:Resource, MemorySize %>'>
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LastDate" SortExpression="LastDate" 
                            HeaderText='<%$ Resources:Resource, Date2 %>'>
                            <HeaderStyle Width="180px" />
                        </asp:BoundField>
                        <asp:TemplateField>
                        <HeaderStyle Width="100px" />
                        <ItemTemplate>
                            <a id='lbtnTerminate' style="cursor:pointer;"
                            procName='<%#System.IO.Path.GetFileName(DataBinder.Eval(Container.DataItem, "ProcessName").ToString())%>' 
                            compName='<%#DataBinder.Eval(Container.DataItem, "ComputerName")%>'>
                                <%=Resources.Resource.Terminate %>
                            </a>
                        </ItemTemplate>
                        </asp:TemplateField>
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
            <td align="left" style="width:33%"></td>              
			<td align="right" style="width:33%">
                <asp:LinkButton ID="lbtnExcel" runat="server" OnClick="lbtnExcel_Click"></asp:LinkButton>
			</td>
        </tr>
    </table>
    <cc:AsyncLoadingStateControl runat="server" ID="AsyncLoadingStateControl1" />
</asp:Content>