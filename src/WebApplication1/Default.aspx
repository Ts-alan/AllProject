<%@ Page Language="C#" validateRequest="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" Inherits="_Default" Title="Untitled Page" Codebehind="Default.aspx.cs" %>

<%@ Register Assembly="CustomControls" Namespace="VirusBlokAda.CC.CustomControls" TagPrefix="cc1" %>
<%@ OutputCache Location="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
   <div class="title"><%=Resources.Resource.Information%></div>
    <div class="divSettings">
        <div class="subsection"><%=Resources.Resource.Administrator%></div>
        <table class="ListContrastTableMain">
            <tr>
                <td  style="padding-left:5px">
                    <%= Resources.Resource.UserLogin %>:
                </td>
                <td style="width:50%">                    
                    <asp:LoginName ID="lgName" runat="server" />
                </td>
            </tr>
            <tr>
               <td style="padding-left:5px">
                    <%= Resources.Resource.FirstName %>:
                </td>
                <td style="width:50%">
                    <%= Profile.FirstName == String.Empty ? "admin" : Profile.FirstName%>
                </td>
            </tr>
            <tr>
                <td style="padding-left:5px">
                    <%= Resources.Resource.LastName %>:
                </td>
                <td style="width:50%">
                    <%= Profile.LastName == String.Empty ? "admin" : Profile.LastName%>
                
                </td>
            </tr>
                         <tr>
                <td style="padding-left:5px">
                    <%= Resources.Resource.Role %>:
                </td>
                <td style="width:50%">
                    <%= Roles.IsUserInRole("Administrator")? Resources.Resource.Administrator:"" %>
                    <%= Roles.IsUserInRole("Operator")? Resources.Resource.Operator:"" %>
                    <%= Roles.IsUserInRole("Viewer")? Resources.Resource.Viewer:"" %>
                </td>
            </tr>
            <tr>
                <td style="padding-left:5px">
                    <%= Resources.Resource.LastLogin %>:
                </td>
                <td style="width:50%">
                    <asp:Label ID="lblLastVist" runat="server" SkinID="LabelContrast"></asp:Label>
                </td>
            </tr>
            <!--
            <tr>
                <td style="padding-left:5px">
                    <%= Resources.Resource.BrowserType %>:
                </td>
                <td style="width:50%">
                    <%=Request.Browser.Browser %>&nbsp;<%=Request.Browser.MajorVersion%>.<%=Request.Browser.MinorVersion%>
                
                </td>
            </tr>
            -->
            </table>
            <!-- My code -->
            
           <div class="subsection"><%=Resources.Resource.LicenseKey %></div>
           <table class="ListContrastTableMain">
            <tr runat="server" id="rowSuccess" class="stateKeyBad" visible="false">
                <td colspan="2" align="center"><asp:Label runat="server" ID="lblSuccess"><%=Resources.Resource.KeyNotFound %></asp:Label></td>                
            </tr>
            <tr runat="server" id="rowLicenseNumber">
                <td style="padding-left:5px">
                    <%=Resources.Resource.LicenseNumber %>
                </td>
                <td style="width:50%">
                    <asp:Label runat="server" ID="lblLicenseNumber"/>&nbsp;&nbsp;
                </td>
            </tr> 
            <tr runat="server" id="rowCustomerName">
                <td style="padding-left:5px">
                   <%=Resources.Resource.CustomerName %>
                </td>
                <td style="width:50%">
                   <asp:Label runat="server" id="lblCustomerName"/>&nbsp;&nbsp;
                </td>
            </tr>
            <tr id="rowKeyState" runat="server">
                <td style="padding-left:5px">
                    <%=Resources.Resource.KeyState %>
                </td>
                <td style="width:50%">
                    <asp:Label runat="server" ID="lblKeyState"/>&nbsp;&nbsp;
                </td>
            </tr>
            <tr id="rowExpirationDate" runat="server">
                <td style="padding-left:5px">
                    <%=Resources.Resource.ExpirationDate %>
                </td>
                <td style="width:50%">
                    <asp:Label runat="server" ID="lblExpirationDate"/>&nbsp;&nbsp;
                </td>
            </tr>                                            
            <tr id="rowComputerLimit" runat="server">
                <td style="padding-left:5px">
                    <%=Resources.Resource.ComputerLimit %>
                </td>
                <td style="width:50%">
                    <asp:Label runat="server" ID="lblComputerLimit"/>&nbsp;&nbsp;
                </td>
            </tr>               
           </table>
           
           <!-- ************************* -->
            
            <div class="subsection"><%=Resources.Resource.DataBaseServer %>&nbsp;Control Center</div>
           <table class="ListContrastTableMain">
            <tr>
                <td style="padding-left:5px">
                   <%=Resources.Resource.Name %>
                </td>
                <td style="width:50%">
                   <asp:Label runat="server" id="lblARM2DataBaseDataSource"/>&nbsp;&nbsp;
                </td>
            </tr>
             <tr>
                <td style="padding-left:5px">
                  <%= Resources.Resource.User %>
                </td>
                <td style="width:50%">
                    <asp:Label runat="server" ID="lblARM2DataBaseUserID" />
                </td>
            </tr>
            <tr>
                <td style="padding-left:5px">
                    <%=Resources.Resource.InitialCatalog %>&nbsp;Control Center
                </td>
               <td style="width:50%">
                    <asp:Label runat="server" ID="lblARM2DataBaseInitialCatalog" />
               </td>
            </tr>
            </table>
            <table class="ListContrastTableMain">
            <tr>
                <td style="padding-left:5px">
                    Name DB
                </td>
               <td style="width:50%">
                    <asp:Label runat="server" ID="lblARM2DBName" />
               </td>
            </tr>
            <tr runat="server" id="rowDBSize">
                <td style="padding-left:5px">
                    Size
                </td>
               <td style="width:50%">
                    <asp:Label runat="server" ID="lblARM2DBSize" />
               </td>
            </tr>
            <tr>
                <td style="padding-left:5px">
                    Path
                </td>
               <td style="width:50%">
                    <asp:Label runat="server" ID="lblARM2DBPath" />
               </td>
            </tr>
           </table>
           <table class="ListContrastTableMain">
            <tr>
                <td style="padding-left:5px">
                    Name DB
                </td>
               <td style="width:50%">
                    <asp:Label runat="server" ID="lblARM2LogName" />
               </td>
            </tr>
            <tr runat="server" id="rowDBLogSize">
                <td style="padding-left:5px">
                    Size
                </td>
               <td style="width:50%">
                    <asp:Label runat="server" ID="lblARM2LogSize" />
               </td>
            </tr>
            <tr>
                <td style="padding-left:5px">
                    Path
                </td>
               <td style="width:50%">
                    <asp:Label runat="server" ID="lblARM2LogPath" />
               </td>
            </tr>
           </table>
           
           
           <div class="subsection"><%=Resources.Resource.DataBaseServer %>&nbsp;Membership</div>
           <table class="ListContrastTableMain">
              <tr>
                <td style="padding-left:5px">
                    <%=Resources.Resource.Name %>
                </td>
                <td style="width:50%">
                    <asp:Label runat="server" ID="lblMembershipDataSource" />
                </td>
            </tr>
             <tr>
                <td style="padding-left:5px">
                  <%= Resources.Resource.User %>
                </td>
                <td style="width:50%">
                    <asp:Label runat="server" ID="lblMembershipUserID" />
                </td>
            </tr>
            <tr>
                <td style="padding-left:5px">
                    <%=Resources.Resource.InitialCatalog %>&nbsp;Membership
                </td>
                <td style="width:50%">
                    <asp:Label runat="server" ID="lblMembershipInitialCatalog" />
                </td>
            </tr>
        </table>
        
    </div>

    <div>
    <div class="subsection"><%=Resources.Resource.Vba32CCServicesState%></div>
    <asp:Table runat="server" ID="tblService" class="ListContrastTableMain" />
    
    </div>
   
</asp:Content>

