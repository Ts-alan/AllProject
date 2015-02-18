<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_PrimitiveFilterSingleDropDownListForStatistics" Codebehind="PrimitiveFilterDropDownListForStatistics.ascx.cs" %>
<%@ Register Src="~/Controls/PrimitiveFilterTemplate.ascx" TagName="PrimitiveFilterTemplate" TagPrefix="flt" %>
<flt:PrimitiveFilterTemplate runat="server" ID="fltTemplate" TextFilter="Name" IsVisibleLogic="false" >
<FilterTemplate>
    <div style="padding: 5px; padding-top: 3px;">
       <asp:DropDownList runat="server" ID="SingleSelectionDropDownList1">
            <asp:ListItem  Value="1" Text="<%$ Resources:Resource, StatCountEvent %>" > </asp:ListItem>
            <asp:ListItem  Value="2" Text='<%$ Resources:Resource, StatCountVirus %>' ></asp:ListItem>
            <asp:ListItem Value="3" Text="<%$ Resources:Resource, StatVirusTotalTop %>" ></asp:ListItem>
       </asp:DropDownList>
    </div>
</FilterTemplate>
</flt:PrimitiveFilterTemplate>