<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_PrimitiveFilterDropDownList" Codebehind="PrimitiveFilterDropDownList.ascx.cs" %>
<%@ Register Src="~/Controls/PrimitiveFilterTemplate.ascx" TagName="PrimitiveFilterTemplate" TagPrefix="flt" %>
<%@ Register Src="~/Controls/MultipleSelectionDropDownList.ascx" TagName="MultipleSelectionDropDownList" TagPrefix="ctrl" %>

<flt:PrimitiveFilterTemplate runat="server" ID="fltTemplate" TextFilter="Name" >
<FilterTemplate>
    <div style="padding: 5px; padding-top: 3px;">
       <ctrl:MultipleSelectionDropDownList runat="server" ID="MultipleSelectionDropDownList1"></ctrl:MultipleSelectionDropDownList>
    </div>
</FilterTemplate>
</flt:PrimitiveFilterTemplate>