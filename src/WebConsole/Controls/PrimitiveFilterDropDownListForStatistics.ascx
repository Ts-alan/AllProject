<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrimitiveFilterDropDownListForStatistics.ascx.cs" Inherits="Controls_PrimitiveFilterSingleDropDownListForStatistics" %>
<%@ Register Src="~/Controls/PrimitiveFilterTemplate.ascx" TagName="PrimitiveFilterTemplate" TagPrefix="flt" %>
<flt:PrimitiveFilterTemplate runat="server" ID="fltTemplate" TextFilter="Name" IsVisibleLogic="false" >
<FilterTemplate>
    <div style="padding: 5px; padding-top: 3px;">
       <asp:DropDownList runat="server" ID="SingleSelectionDropDownList1">
            <asp:ListItem  Value="1_*" Text="<%$ Resources:Resource, StatCountEvent %>" > </asp:ListItem>
            <asp:ListItem  Value="2_vba32.virus.found" Text='<%$ Resources:Resource, StatCountVirus %>' ></asp:ListItem>
            <asp:ListItem Value="3_vba32.virus.found" Text="<%$ Resources:Resource, StatVirusTotalTop %>" ></asp:ListItem>
       </asp:DropDownList>
    </div>
</FilterTemplate>
</flt:PrimitiveFilterTemplate>