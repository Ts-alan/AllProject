<%@ Page Title="" Language="C#" MasterPageFile="~/mstrPageNew.master" AutoEventWireup="true" CodeFile="DeviceClass.aspx.cs" Inherits="DeviceClassPage" %>

<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register TagPrefix="custom" Namespace="VirusBlokAda.CC.CustomControls" Assembly="CustomControls" %>
<%@ Register Src="~/Controls/CompositeFilter.ascx" TagName="CompositeFilter" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterText.ascx" TagName="FilterText" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDateTime.ascx" TagName="FilterDateTime" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterComputers.ascx" TagName="FilterComputers"  TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDropDownList.ascx" TagName="FilterDropDownList" TagPrefix="flt" %>
<%@ Register Src="~/Controls/AsyncLoadingStateControl.ascx" TagName="AsyncLoadingStateControl" TagPrefix="cc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true"></ajaxToolkit:ToolkitScriptManager>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#tabs").tabs({ cookie: { expires: 30} });
        });
    </script>
    
    <div class="title"><%=Resources.Resource.DeviceClassManagment %></div>

    <div id="tabs">
    <ul>
        <li><a href="#tab1"><%= Resources.Resource.Groups %></a> </li>
        <li><a href="#tab2"><%= Resources.Resource.Devices %></a> </li>
    </ul>

    <div id="tab1">
        <div id="accordion" style="font-size:1em !important"></div>
    </div>

    <div id="tab2">    
        <div class="divSettings"> 
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True"
                SelectCountMethod="Count" SelectMethod="Get" TypeName="DeviceClassDataContainer" SortParameterName="SortExpression">
                <SelectParameters>
                    <asp:Parameter Name="where" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        
            <asp:UpdatePanel  runat="server" ID="updatePanelDeviceClassGrid" UpdateMode='Conditional'  RenderMode='Block'  >
            <ContentTemplate>
                <flt:CompositeFilter ID="DeviceClassFilterContainer" UserFiltersTemproraryStorageName="DeviceClassFiltersTemp"
                    InformationListType="DeviceClass" UserFiltersProfileKey="DeviceClassFilters" runat="server"
                    OnActiveFilterChange="DeviceClassFilterContainer_ActiveFilterChanged">
                    <FiltersTemplate>
                        <table>
                            <tr>
                                <td valign="top">                       
                                    <flt:FilterText runat="server" ID="fltUID" NameFieldDB="UID" TextFilter="UID" />
                                </td>
                                <td valign="top" style="padding-left: 20px;">                        
                                    <flt:FilterText runat="server" ID="fltComment" NameFieldDB="Class" TextFilter='<%$ Resources:Resource, Comment %>' />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <flt:FilterText runat="server" ID="fltClassName" NameFieldDB="ClassName" TextFilter='<%$ Resources:Resource, ClassName %>' />
                                </td>
                                <td valign="top" style="padding-left: 20px;">
                                    <flt:FilterDropDownList runat="server" ID="fltMode" NameFieldDB="ModeName" TextFilter='<%$ Resources:Resource, State %>' />
                                </td>
                            </tr>
                        </table>
                    </FiltersTemplate>
                </flt:CompositeFilter>

                <custom:GridViewExtended ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" 
                    EnableModelValidation="True" CssClass="gridViewStyle"
                    StorageType="Session" StorageName="DeviceClass" OnRowDataBound="GridView1_RowDataBound"  >
                <Columns>      
                    <asp:TemplateField HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center" 
                        HeaderText='<%$ Resources:Resource, ClassName %>' SortExpression="ClassName">
                        <ItemTemplate>
                            <asp:LinkButton deviceID='<%# Eval("ID") %>' runat="server" Text='<%# Eval("ClassName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center" 
                        HeaderText="UID" SortExpression="UID">
                        <ItemTemplate>
                            <asp:LinkButton deviceID='<%# Eval("ID") %>' runat="server" Text='<%# Eval("UID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center" 
                        HeaderText='<%$ Resources:Resource, Comment %>' SortExpression="Class">
                        <ItemTemplate >
                            <asp:Label runat="server" Text='<%# Eval("Class") %>' uid='<%# Eval("UID") %>' type="comment"></asp:Label> 
                        </ItemTemplate>
                    </asp:TemplateField>        
                    <asp:TemplateField  HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" 
                        HeaderText='<%$ Resources:Resource, Actions %>'>
                        <ItemTemplate>
                            <asp:Image ccUID='<%# Eval("UID") %>' comment='<%# Eval("Class") %>' ImageUrl="~/App_Themes/Main/Images/comment.png" ToolTip='<%$ Resources:Resource, ChangeComment %>' style="cursor: pointer; margin-left: 10px;" runat="server" /> 
                            <asp:ImageButton ID="ibtnDelete" uid='<%# Eval("UID") %>' ImageUrl="~/App_Themes/Main/Images/deleteicon.png" ToolTip='<%$ Resources:Resource, Delete %>' style="margin-left: 10px;"
                                runat="server" onclick="DeleteDeviceFromPanel"  OnClientClick= " return confirm('Are you sure?');" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
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
                <div>
                    <a id="btnAddDeviceClass" updateID='<%= btnUpdate.UniqueID %>' class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" role="button" aria-disabled="false">
                        <span class="ui-button-text"><%=Resources.Resource.Add%></span>
                    </a>
                    <asp:Button runat="server" ID="btnUpdate" OnClick="UpdateGridView" style="display:none;" />
                </div>                
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </div>
    
    <div id="divModalDialog" style="display:none"></div>
    <div id="commentDialog" style="display:none"></div>
    <div id="deviceTreeDialog" style="display:none"></div>
    <div id="divAddClassDialog" style="display: none;">
        <table>
            <tr>
                <td style="padding-right: 20px;">UID:</td>
                <td>
                    <input type="text" id="tboxNewUID" style="width: 300px;" />                    
                    <span style="color:Red;">*</span>
                </td>
            </tr>
            <tr>
                <td style="padding-right: 20px;padding-top: 5px;"><%=Resources.Resource.ClassName %>:</td>
                <td style="padding-top: 5px;">
                    <input type="text" id="tboxNewClassName" style="width: 300px;" />
                    <span style="color:Red;">*</span>
                </td>
            </tr>
            <tr>
                <td style="padding-right: 20px;padding-top: 5px;"><%=Resources.Resource.Comment %>:</td>
                <td style="padding-top: 5px;">
                    <input type="text" id="tboxNewComment" style="width: 300px;" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-top: 5px;">
                    <span style="color:Red;">*</span> - <%=Resources.Resource.IndicatesRequiredField %>
                </td>
            </tr>
        </table>
    </div>
    
</asp:Content>

