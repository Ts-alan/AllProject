<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrimitiveFilterDropDownList.ascx.cs" Inherits="Controls_PrimitiveFilterDropDownList" %>
<%@ Register Src="~/Controls/PrimitiveFilterTemplate.ascx" TagName="PrimitiveFilterTemplate" TagPrefix="flt" %>
<%@ Register Src="~/Controls/MultipleSelectionDropDownList.ascx" TagName="MultipleSelectionDropDownList" TagPrefix="ctrl" %>

<flt:PrimitiveFilterTemplate runat="server" ID="fltTemplate" TextFilter="Name" >
<FilterTemplate>
    <div style="padding: 5px; padding-top: 3px;">
       <ctrl:MultipleSelectionDropDownList runat="server" ID="MultipleSelectionDropDownList1"></ctrl:MultipleSelectionDropDownList>
    </div>
</FilterTemplate>
</flt:PrimitiveFilterTemplate>