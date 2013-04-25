<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrimitiveFilterText.ascx.cs" Inherits="Controls_PrimitiveFilterText" %>
<%@ Register Src="~/Controls/PrimitiveFilterTemplate.ascx" TagName="PrimitiveFilterTemplate" TagPrefix="flt" %>

<flt:PrimitiveFilterTemplate runat="server" ID="fltTemplate" TextFilter="Name" >
<FilterTemplate>
    <div style="padding: 5px; padding-top: 3px;">
        <asp:TextBox runat="server" ID="tboxFilter" style="width: 150px; height: 17px;"></asp:TextBox>
    </div>
</FilterTemplate>
</flt:PrimitiveFilterTemplate>