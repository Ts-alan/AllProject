﻿<%@ Page Title="" Language="C#" MasterPageFile="~/mstrPageNew.master" AutoEventWireup="true" CodeFile="accordion2.aspx.cs" Inherits="accordion2" %>

<%@ Register Src="~/Controls/PagerUserControl.ascx" TagName="Paging" TagPrefix="paging" %>
<%@ Register TagPrefix="custom" Namespace="CustomControls" Assembly="CustomControls" %>

<%@ Register Src="~/Controls/CompositeFilter.ascx" TagName="CompositeFilter" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterText.ascx" TagName="FilterText" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDateTime.ascx" TagName="FilterDateTime" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDropDownList.ascx" TagName="FilterDropDownList" TagPrefix="flt" %>
<%@ Register Src="~/Controls/AsyncLoadingStateControl.ascx" TagName="AsyncLoadingStateControl" TagPrefix="cc" %>
  <script runat=server >

    
    </script>
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

      <div class="divSettings"> 
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True"
                SelectCountMethod="Count" SelectMethod="Get" TypeName="DeviceDataContainer" SortParameterName="SortExpression">
                <SelectParameters>
                    <asp:Parameter Name="where" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        
            <asp:UpdatePanel  runat="server" ID="updatePanelDevicesGrid" UpdateMode='Conditional'  RenderMode='Block'  >
            <ContentTemplate>
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
     <div class="divSettings"> 
            <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" EnablePaging="True"
                SelectCountMethod="CountUnknown" SelectMethod="GetUnknown" TypeName="DeviceDataContainer" SortParameterName="SortExpression">
                <SelectParameters>
                    <asp:Parameter Name="where" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:UpdatePanel  runat="server" ID="updatePanelDevicesGrid2" UpdateMode='Conditional'  RenderMode='Block'  >
            
            <ContentTemplate>
            <asp:Button ID='UpdatePanelButton' runat=server  OnClick="UpdatePanelReload" OnClientClick="UpdatePanelReload"/>
                <custom:GridViewExtended ID="GridView2" runat="server" AllowPaging="True" AllowSorting="true"
                    AutoGenerateColumns="False" DataSourceID="ObjectDataSource2" 
                    EnableModelValidation="True" CssClass="gridViewStyle"
                    StorageType="Session" StorageName="Devices"  >
               <Columns>    
                <asp:TemplateField   HeaderStyle-HorizontalAlign="Center" HeaderText='<%$ Resources:Resource, SerialNo %>' SortExpression="SerialNo">
                    <ItemTemplate >
                    <asp:Label runat=server dcp=<%#Eval("ID")%> class="wrapped"  no=true Text='<%#Eval("Device.SerialNo")%>'/>
                    </ItemTemplate>
                </asp:TemplateField > 

                 <asp:TemplateField  HeaderStyle-HorizontalAlign="Center" HeaderText='<%$ Resources:Resource, Comment %>' SortExpression="Comment">
                    
                    <ItemTemplate > <asp:Label runat='server' Text='<%# Eval("Device.Comment") %>' dp='<%# Eval("Device.ID") %>' type='comment'></asp:Label> 
                    </ItemTemplate></asp:TemplateField>  
                    <asp:TemplateField  HeaderStyle-HorizontalAlign="Center" HeaderText='<%$ Resources:Resource, ComputerName %>' SortExpression="ComputerName">
                    <ItemTemplate > <asp:Label  runat='server' Text='<%# Eval("Computer.computerName") %>' dp='<%# Eval("ID") %>' ></asp:Label> 
                    </ItemTemplate></asp:TemplateField>  
                    <asp:TemplateField  HeaderStyle-HorizontalAlign="Center" HeaderText='<%$ Resources:Resource, DeviceInsertedDate %>' SortExpression="LatestInsert">
                    <ItemTemplate > <asp:Label  runat='server' Text='<%# Eval("LatestInsert") %>' dp='<%# Eval("ID") %>' ></asp:Label> 
                    </ItemTemplate></asp:TemplateField>   
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
           <%--  <tr>
               <td class="HeaderCell"><asp:LinkButton runat="server" ID="lbtnHeaderUDeviceSerialNo" OnClick="lbtnHeaderUDeviceSerialNo_Click" /></td>
                <td class="HeaderCell"><asp:LinkButton runat="server" ID="lbtnHeaderUDeviceComment" OnClick="lbtnHeaderUDeviceComment_Click" /></td>
                <td class="HeaderCell"><asp:LinkButton runat="server" ID="lbtnHeaderUDeviceComputerName" OnClick="lbtnHeaderUDeviceComputerName_Click" /></td>
                <td class="HeaderCell"><asp:LinkButton runat="server" ID="lbtnHeaderUDeviceInsertedDate" OnClick="lbtnHeaderUDeviceInsertedDate_Click" /></td>
                <td class="HeaderCell" width="70"><asp:Label runat="server" ID="lblHeaderUDeviceAction"><%=Resources.Resource.Action %></asp:Label>
                <br /><img acpAll=allowAll action=allow style="cursor:pointer;" title='<%=Resources.Resource.EnableAll %>' src='<%=String.Format(HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + Profile.Theme+ "/images/enabled.gif")%>' />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <img acpAll=banAll action=disable style="cursor:pointer;" title='<%=Resources.Resource.DisableAll %>' src='<%=String.Format(HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + Profile.Theme+ "/images/disabled.gif")%>' />
                </td>
            </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr  rcp style="text-align:center">
                    <td dcp=<%#Eval("ID")%> class="wrapped" no=true tbox=<%=tboxSerialNumberUDevice.ClientID%> cbox=<%=cboxSerialNumberUDevice.ClientID%>><%#Eval("Device.SerialNo")%></td>
                    <td dcp=<%#Eval("ID")%> class="wrapped" no=true tbox=<%=tboxCommentUDevice.ClientID%> cbox=<%=cboxCommentUDevice.ClientID%>><%#Eval("Device.Comment")%></td>
                    <td dcp=<%#Eval("ID")%> class="wrapped" no=true tbox=<%=tboxComputerNameUDevice.ClientID%> cbox=<%=cboxComputerNameUDevice.ClientID%>><%#Eval("Computer.ComputerName")%></td>
                    <td dcp=<%#Eval("ID")%> no=true><%#Eval("LatestInsert")%>&nbsp;</td>
                    <td dcp=<%#Eval("ID")%> no=true>
                        <img acp=<%#Eval("ID")%> action=allow style="cursor:pointer;" title='<%=Resources.Resource.Enable %>' src='<%=String.Format(HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + Profile.Theme+ "/images/enabled.gif")%>' />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <img acp=<%#Eval("ID")%> action=disable style="cursor:pointer;" title='<%=Resources.Resource.Disable %>' src='<%=String.Format(HttpContext.Current.Request.ApplicationPath+"/App_Themes/" + Profile.Theme+ "/images/disabled.gif")%>' />
                    </td>
                </tr>
            </ItemTemplate>
            </asp:DataList>--%>
           
            
            
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
   </div>
    
        
        <div id="divModalDialog" style="display:none"></div>
        <div id="commentDialog" style="display:none"></div>
        <div id="deviceTreeDialog" style="display:none"/>
        
</asp:Content>

