<%@ Page Language="C#" validateRequest=false MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" Inherits="Error" Title="Untitled Page" Codebehind="Error.aspx.cs" %>
<%@ OutputCache Location="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
     <div class="title"><%=Resources.Resource.Error%></div>
    <div class="divSettings"><asp:Label ID="lblMessage" runat="server" Text="Error"></asp:Label></div>
    <div class="divSettings"><asp:Label ID=lblClear runat=server />
        <asp:LinkButton ID="lbtnClear" runat="server" OnClick="lbtnClear_Click">LinkButton</asp:LinkButton>
    </div>
</asp:Content>

