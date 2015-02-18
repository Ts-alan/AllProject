<%@ Page Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" Inherits="ErrorSql" validateRequest="false" Title="Untitled Page" Codebehind="ErrorSql.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
    <div class="title"><%=Resources.Resource.Error%></div>
    <div class="divSettings"><asp:Label ID="lblMessage" runat="server"><%=Resources.Resource.ErrorSql %></asp:Label></div>
</asp:Content>

