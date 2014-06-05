<%@ Page Language="C#" validateRequest="false" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="Portable.aspx.cs" Inherits="Portable" Title="Untitled Page" %>
<%@ OutputCache Location="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<div class="title"><%=Resources.Resource.PagePortableTitle %></div>
<center><asp:Label ID="lblMessage" runat="server" ></asp:Label> </center>
<div class="divSettings">
    <table class="ListContrastTableMain">
        <tr>
            <td>
                <asp:CheckBoxList ID="cbList" runat="server">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="lbtnExport" runat="server" OnClick="lbtnExport_Click" SkinId="Button">
                    <%=Resources.Resource.Export%>
                </asp:LinkButton>
                <asp:LinkButton ID="lbtnDelete" runat="server" OnClick="lbtnDelete_Click" SkinId="Button">
                    <%=Resources.Resource.Delete%>
                </asp:LinkButton>
            </td>
        </tr>
          <tr>
            <td><br/></td>
        </tr>
        <tr>
            <td>
               <asp:FileUpload ID="fuICompFilters" runat="server" />            
                <br />
                <asp:LinkButton runat="server" ID="lbtnImport" OnClick="lbtnImport_Click" SkinId="Button">
                    <%=Resources.Resource.Import%>
                </asp:LinkButton>
            </td>
        </tr>
    </table>
</div>
</asp:Content>

