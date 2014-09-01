<%@ Page Title="" Language="C#" MasterPageFile="~/mstrPageNew.master" AutoEventWireup="true" CodeFile="DevicesPolicy.aspx.cs" Inherits="DevicesPolicy" %>

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
  <div class="title"><%=Resources.Resource.DeviceManagment %></div>  
  <script type="text/javascript">
        $(document).ready(function () {
            $("#tabs").tabs({ cookie: { expires: 30} });
        });
  </script>
  <div style="padding: 10px;">
    <asp:UpdatePanel  runat="server" ID="upnlDeviceTypes" >
        <ContentTemplate>
            <%=Resources.Resource.DeviceType%>:&nbsp;<asp:DropDownList runat="server" ID="ddlDeviceType" OnSelectedIndexChanged="ddlDeviceType_SelectedIndexChanged" AutoPostBack="true" ddlDeviceTypes></asp:DropDownList>
        </ContentTemplate>
    </asp:UpdatePanel>
  </div>
    <div id="tabs">
    <ul>
    <li><a href="#tab1"><%= Resources.Resource.Groups %></a> </li>
    <li><a href="#tab2"><%= Resources.Resource.Devices %></a> </li>
    <li onlyUSB><a href="#tab3"><%= Resources.Resource.Assignment%></a> </li>
    </ul>
    <div id="tab1">
      <p><div id="accordion" style="font-size:1em !important"></div></p>
   </div>
   <div id="tab2">    
      <div class="divSettings"> 
            <asp:UpdatePanel  runat="server" ID="updatePanelDevicesGrid" UpdateMode='Conditional'  RenderMode='Block'  >
            <ContentTemplate>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True"
                    SelectCountMethod="Count" SelectMethod="Get" 
                    TypeName="DeviceDataContainer" SortParameterName="SortExpression" >
                    <SelectParameters>
                        <asp:Parameter Name="where" Type="String" />
                        <asp:ControlParameter Name="device_type" ControlID="ddlDeviceType" PropertyName="SelectedValue" DefaultValue="USB" />
                    </SelectParameters>                
                </asp:ObjectDataSource>

                <flt:CompositeFilter ID="DeviceFilterContainer" UserFiltersTemproraryStorageName="DevicesFiltersTemp"
                    InformationListType="Devices" UserFiltersProfileKey="DeviceFilters" runat="server"
                    OnActiveFilterChange="DeviceFilterContainer_ActiveFilterChanged">
                    <FiltersTemplate>
                        <table>
                            <tr>
                                <td valign="top">                       
                                    <flt:FilterText runat="server" ID="fltSerial" NameFieldDB="SerialNo" TextFilter='<%$ Resources:Resource, SerialNo %>' />
                                </td>
                                <td valign="top" style="padding-left: 20px;">                        
                                    <flt:FilterText runat="server" ID="fltComment" NameFieldDB="Comment" TextFilter='<%$ Resources:Resource, Comment %>' />
                                </td>
                            </tr>
                        </table>
                    </FiltersTemplate>
                </flt:CompositeFilter>

                <custom:GridViewExtended ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" 
                    EnableModelValidation="True" CssClass="gridViewStyle"
                    StorageType="Session" StorageName="Devices"  >
                <Columns>      
                <asp:TemplateField HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center" 
                    HeaderText='<%$ Resources:Resource, SerialNo %>' SortExpression="SerialNo">
                    <ItemTemplate >
                    <asp:LinkButton deviceID='<%# Eval("ID") %>' runat='server' Text='<%# Eval("SerialNo") %>' />
                     <%--   <asp:Label   runat='server' Text='<%# Eval("SerialNo") %>' ></asp:Label> --%>
                    </ItemTemplate>
                </asp:TemplateField > 

                 <asp:TemplateField HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center" HeaderText='<%$ Resources:Resource, Comment %>' SortExpression="Comment">
                    
                    <ItemTemplate >
                        <asp:Label   runat='server' Text='<%# Eval("Comment") %>' dp='<%# Eval("ID") %>' type='comment'></asp:Label> 
                    </ItemTemplate></asp:TemplateField>        
                     <asp:TemplateField  HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" HeaderText='<%$ Resources:Resource, Actions %>'>
                   
                   <ItemTemplate>
                    <asp:Image comdp='<%# Eval("ID") %>' serialdp='<%# Eval("SerialNo") %>' ImageUrl="~/App_Themes/Main/Images/comment.png" ToolTip='<%$ Resources:Resource, ChangeComment %>' runat=server /> 
                       <%--  <asp:ImageButton ID="ImageButton1" comdp='<%# Eval("ID") %>' serialdp='<%# Eval("SerialNo") %>' ImageUrl="~/App_Themes/Main/Images/comment.png" ToolTip='<%$ Resources:Resource, ChangeComment %>' runat=server /> 
                        <asp:Image ID="ImageButton2" deldev='<%# Eval("ID") %>' delete=true ImageUrl="~/App_Themes/Main/Images/delete.gif" ToolTip='<%$ Resources:Resource, Delete %>' runat=server /> --%>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton deldev='<%# Eval("ID") %>' ImageUrl="~/App_Themes/Main/Images/deleteicon.png" ToolTip='<%$ Resources:Resource, Delete %>' runat=server onclick="DeleteDeviceFromPanel"  OnClientClick= " return confirm('Are you sure?');" />
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
   </div>
    <div id='tab3'>       
     <div class="divSettings" > 
            <asp:UpdatePanel  runat="server" ID="updatePanelDevicesGrid2" UpdateMode='Conditional'  RenderMode='Block'  >
                <ContentTemplate>
                <div runat="server" id="divUnknownDevices">
                    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" EnablePaging="True"
                        SelectCountMethod="CountUnknown" SelectMethod="GetUnknown" TypeName="DeviceDataContainer" SortParameterName="SortExpression">
                        <SelectParameters>
                            <asp:Parameter Name="where" Type="String" />
                            <asp:ControlParameter Name="device_type" ControlID="ddlDeviceType" PropertyName="SelectedValue" DefaultValue="USB" />
                        </SelectParameters>
                    </asp:ObjectDataSource>

                    <flt:CompositeFilter ID="UnknownDeviceFilterContainer" UserFiltersTemproraryStorageName="UnknownDevicesFiltersTemp"
                        InformationListType="UnknownDevices" UserFiltersProfileKey="UnknownDeviceFilters" runat="server"
                        OnActiveFilterChange="UnknownDeviceFilterContainer_ActiveFilterChanged">
                        <FiltersTemplate>
                            <table>
                                <tr>
                                    <td valign="top">
                                            <flt:FilterText runat="server" ID="fltDevice2" NameFieldDB="SerialNo" TextFilter='<%$ Resources:Resource, SerialNo %>' />
                                            <flt:FilterText runat="server" ID="FilterText2" NameFieldDB="Comment" TextFilter='<%$ Resources:Resource, Comment %>' />
                                    </td>
                                    <td valign="top" style="padding-left: 20px;">
                                        <flt:FilterComputers runat="server" ID="fltComputers" NameFieldDB="ComputerName" TextFilter='<%$ Resources:Resource, ComputerName %>' />
                                        <flt:FilterDateTime runat="server" ID="fltEventTime" NameFieldDB="LatestInsert" TextFilter="<%$ Resources:Resource, DeviceInsertedDate %>" />
                                    </td>
                                </tr>
                            </table>
                        </FiltersTemplate>
                    </flt:CompositeFilter>

                    <asp:Button ID="btnUpdateAsync" style="display:none" UpdatePanelButton="true" runat="server"  OnClick="UpdatePanelReload"/>
                    <custom:GridViewExtended ID="GridView2" runat="server" AllowPaging="True" AllowSorting="true"
                        AutoGenerateColumns="False" DataSourceID="ObjectDataSource2" 
                        EnableModelValidation="True" CssClass="gridViewStyle"
                        StorageType="Session" StorageName="DevicesUnknown"  >
                    <Columns>    
                        <asp:TemplateField   HeaderStyle-HorizontalAlign="Center" HeaderText='<%$ Resources:Resource, SerialNo %>' SortExpression="SerialNo">
                            <ItemTemplate >
                                <asp:Label runat='server' dcp=<%# Eval("ID" )%> class="wrapped"  no=true Text='<%#Eval("Device.SerialNo")%>'/>
                            </ItemTemplate>
                        </asp:TemplateField > 
                         <asp:TemplateField  HeaderStyle-HorizontalAlign="Center" HeaderText='<%$ Resources:Resource, Comment %>' SortExpression="Comment">
                            <ItemTemplate > 
                                <asp:Label runat='server' Text='<%# Eval("Device.Comment") %>' dp='<%# Eval("Device.ID") %>' type='comment'></asp:Label> 
                            </ItemTemplate>
                         </asp:TemplateField>  
                         <asp:TemplateField  HeaderStyle-HorizontalAlign="Center" HeaderText='<%$ Resources:Resource, ComputerName %>' SortExpression="ComputerName">
                            <ItemTemplate > 
                                <asp:Label  runat='server' Text='<%# Eval("Computer.computerName") %>' devcompdp='<%# Eval("ID") %>' ></asp:Label> 
                            </ItemTemplate>
                         </asp:TemplateField>  
                         <asp:TemplateField  HeaderStyle-HorizontalAlign="Center" HeaderText='<%$ Resources:Resource, DeviceInsertedDate %>' SortExpression="LatestInsert">
                            <ItemTemplate > 
                                <asp:Label  runat='server' Text='<%# Eval("LatestInsert") %>' ></asp:Label> 
                            </ItemTemplate>
                         </asp:TemplateField>   
                         <asp:TemplateField  HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" >
                            <HeaderTemplate> 
                               <%-- <asp:Label ID="TextBox1" runat='server' Text='<%$ Resources:Resource, Actions %>'></asp:Label> --%>
                               <asp:Literal  runat='server' ID="lblHeaderUDeviceAction" Text='<%$ Resources:Resource, Actions %>'  />
                     
                                <br /><asp:ImageButton acpAll=allowAll action=allow runat=server ToolTip='<%$ Resources:Resource, EnableAll %> ' ImageUrl='~/App_Themes/Main/Images/enabled.gif' />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton acpAll=banAll action=disable  runat=server ToolTip='<%$ Resources:Resource, DisableAll %>' ImageUrl='~/App_Themes/Main/Images/disabled.gif'  />
                            </HeaderTemplate>
                            <ItemTemplate>                  
                                <asp:ImageButton acp=<%#Eval("ID")%> action=allow runat=server ToolTip='<%$ Resources:Resource, Enable %>' ImageUrl='~/App_Themes/Main/Images/enabled.gif'  />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton acp=<%#Eval("ID")%> action=disable runat=server ToolTip='<%$ Resources:Resource, Disable %>' ImageUrl='~/App_Themes/Main/Images/disabled.gif' />
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
                </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnUpdateAsync" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
   </div>
   </div>
    
        
        <div id="divModalDialog" style="display:none"></div>
        <div id="commentDialog" style="display:none"></div>
        <div id="deviceTreeDialog" style="display:none"/>
        
</asp:Content>

