<%@ Page Language="C#" validateRequest=false MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="Statistic.aspx.cs" Inherits="Statistic" Title="Untitled Page" %>
<%@ OutputCache Location="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
    <div class="title"><%=Resources.Resource.Statistics%></div>
    <div class="divSettings">
        <table class="ListContrastTableMain">
            <tr>
                <td><%= Resources.Resource.CountOfCompRegistred %></td>
                <td style="text-align:left;width:50%">
                    <asp:Label ID="lblCountOfCompRegistred" SkinID="LabelContrast" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td><%= Resources.Resource.CountOfEventRegistred %></td>
                <td>
                    <asp:Label ID="lblCountOfEventRegistred" SkinID="LabelContrast" runat="server" Text=""></asp:Label>
                </td>
            </tr>  
            <tr>
                <td><%= Resources.Resource.CountOfTaskRegistred %></td>
                <td>
                    <asp:Label ID="lblCountOfTaskRegistred" SkinID="LabelContrast" runat="server" Text=""></asp:Label>
                </td>
            </tr>  
            <tr>
                <td colspan="2"><br/></td>
            </tr>
            <tr>
                <td><%= Resources.Resource.CountOfCompActiveToday %> </td>
                <td>
                   <asp:Label ID="lblCountOfCompActiveToday" SkinID="LabelContrast" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td><%= Resources.Resource.CountOfEventToday %></td>
                <td>
                   <asp:Label ID="lblCountOfEventToday" SkinID="LabelContrast" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td><%= Resources.Resource.CountOfTaskTodayIssued %></td>
                <td>
                   <asp:Label ID="lblCountOfTaskTodayIssued" SkinID="LabelContrast" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td><%= Resources.Resource.CountOfTaskTodayUpdated %></td>
                <td>
                   <asp:Label ID="lblCountOfTaskTodayUpdated" SkinID="LabelContrast" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td><%= Resources.Resource.CountOfTaskTodayCompleted %></td>
                <td>
                    <asp:Label ID="lblCountOfTaskTodayCompleted" SkinID="LabelContrast" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>