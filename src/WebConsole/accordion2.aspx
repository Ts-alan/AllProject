<%@ Page Title="" Language="C#" MasterPageFile="~/mstrPageNew.master" AutoEventWireup="true" CodeFile="accordion2.aspx.cs" Inherits="accordion2" %>

<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register TagPrefix="custom" Namespace="CustomControls" Assembly="CustomControls" %>

<%@ Register Src="~/Controls/CompositeFilter.ascx" TagName="CompositeFilter" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterText.ascx" TagName="FilterText" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDateTime.ascx" TagName="FilterDateTime" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDropDownList.ascx" TagName="FilterDropDownList" TagPrefix="flt" %>
<%@ Register Src="~/Controls/AsyncLoadingStateControl.ascx" TagName="AsyncLoadingStateControl" TagPrefix="cc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true"></ajaxToolkit:ToolkitScriptManager>
    <div class="title"><%=Resources.Resource.DeviceManagment %></div>  
    <script type="text/javascript">
        $(document).ready(function () {
            $("#tabs").tabs({ cookie: { expires: 30} });
        });
    </script>



    <div id="tabs">
    <ul>
    <li><a href="#tab1"><%= Resources.Resource.Groups %></a> </li>
    <li><a href="#tab2"><%= Resources.Resource.Devices %></a> </li>
    <li><a href="#tab3"><%= Resources.Resource.Assignment%></a> </li>
    </ul>
    <div id="tab1">
      <p><div id="accordion" style="font-size:1em !important"></div></p>
   </div>
   <div id="tab2">
      <%-- <asp:UpdatePanel runat="server" ID="updatePanelDeviceFilter">
         <ContentTemplate>
          <flt:CompositeFilter ID="DeviceFilterContainer" UserFiltersTemproraryStorageName="DeviceFiltersTemp" InformationListType="Devices"
            UserFiltersProfileKey="DeviceFilters" OnClearClick="DeviceFilterContainer_ClearClick" runat="server"
            OnActiveFilterChange="DeviceFilterContainer_ActiveFilterChanged">
            <FiltersTemplate>
                <table>
                    <tr>
                        <td valign="top">
                            <flt:FilterText runat="server" ID="fltSerialNumber" NameFieldDB="SerialNo" TextFilter='<%$ Resources:Resource, SerialNo %>' />
                            <flt:FilterText runat="server" ID="fltComment" NameFieldDB="Comment" TextFilter='<%$ Resources:Resource, Comment %>' />
                            <flt:FilterDropDownList runat="server" ID="fltState" NameFieldDB="StateName" TextFilter='<%$ Resources:Resource, State %>' />
                        </td>
                        <td valign="top" style="padding-left: 20px;">
                            <flt:FilterDateTime runat="server" ID="fltLatestInsert" NameFieldDB="LatestInsert" TextFilter='<%$ Resources:Resource, DeviceInsertedDate %>' />
                        </td>
                    </tr>
                </table>
            </FiltersTemplate>
        </flt:CompositeFilter>
         </ContentTemplate>
        </asp:UpdatePanel>--%>
        
      <div class="divSettings">
                 
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True"
                SelectCountMethod="Count" SelectMethod="Get" TypeName="DeviceDataContainer" SortParameterName="SortExpression">
                <SelectParameters>
                    <asp:Parameter Name="where" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        
            <asp:UpdatePanel runat="server" ID="updatePanelDevicesGrid">
            <ContentTemplate>
                <custom:GridViewExtended ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                    AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" 
                    EnableModelValidation="True" CssClass="gridViewStyle"
                    StorageType="Session" StorageName="Devices" >
                <Columns>      
                <asp:TemplateField HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center" >
                    <HeaderTemplate> <asp:Label   runat='server' Text='<%$ Resources:Resource, SerialNo %>'></asp:Label>     </HeaderTemplate>
                    <ItemTemplate >
                        <asp:Label   runat='server' Text='<%# Eval("SerialNo") %>'></asp:Label> 
                    </ItemTemplate>
                </asp:TemplateField > 

                 <asp:TemplateField HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center" >
                    <HeaderTemplate> <asp:Label ID="TextBox1" runat='server' Text='<%$ Resources:Resource, Comment %>'></asp:Label></HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label   runat='server' Text='<%# Eval("Comment") %>'></asp:Label> 
                    </ItemTemplate></asp:TemplateField>

                <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" >
                    <HeaderTemplate> 
                       <%-- <asp:Label ID="TextBox1" runat='server' Text='<%$ Resources:Resource, Actions %>'></asp:Label> --%>
                       <asp:Literal runat='server' Text='<%$ Resources:Resource, Actions %>'  />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ImageUrl="~/App_Themes/Main/Images/comment.png" ToolTip='<%$ Resources:Resource, ChangeComment %>' runat=server /> 
                        <asp:ImageButton ImageUrl="~/App_Themes/Main/Images/delete.gif" ToolTip='<%$ Resources:Resource, Delete %>' runat=server /> 
                    </ItemTemplate>
               </asp:TemplateField>
                                                                 
                   <%-- <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" 
                        HeaderText='<%$ Resources:Resource, SerialNo %>'>
                        <HeaderStyle Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Comment" SortExpression="Comment" 
                        HeaderText='<%$ Resources:Resource, Comment %>'>
                        <HeaderStyle Width="200px" />
                    </asp:BoundField>--%>

                   
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
   </div>
   <div id="tab3">
      <p>Содержимое третьей вкладки</p>
   </div>

    </div>   
    
        
        <div id="divModalDialog" style="display:none"></div>
        <div id="commentDialog" style="display:none"></div>
</asp:Content>

