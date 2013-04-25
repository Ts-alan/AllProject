<%@ Page Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" CodeFile="Visual.aspx.cs" Inherits="Visual" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<div class="title"><%=Resources.Resource.Diagram%></div>
            <table class="subsection" width=100%> 
                <tr>
			<td align=left>			
                        <asp:Label ID="lblAppearance" style="width:auto;" SkinID="SubSectionLabel" runat="Server" />&nbsp;&nbsp;
                        <asp:dropdownlist id="drpChartType" OnSelectedIndexChanged="drpChartType_SelectedIndexChanged" runat="server" autopostback="True">
                        </asp:dropdownlist>
                    </td>
                    <td>
                        <asp:Label ID="lblType" SkinID="SubSectionLabel" style="width:auto;" runat="Server" />&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlStatisticType" runat="server" SkinID="ddlFilterList" AutoPostBack="True" OnSelectedIndexChanged="ddlStatisticType_SelectedIndexChanged"/>
                    </td>
                </tr>
                </table>
                <table width=100% >
                <tr>
                    <td valign="top" align="center">
                        <asp:datagrid id="SalesByCategoryGrid" runat="server" gridlines="Both" borderwidth="0" showfooter="True" autogeneratecolumns="False" width="300" cellpadding="3" cssclass="Content">
                            <columns>
                                <asp:HyperLinkColumn DataNavigateUrlFormatString="~\CompInfo.aspx?CompName={0}"
                                    DataNavigateUrlField="CategoryName" DataTextField="CategoryName" >
                                    <headerstyle cssclass="HeaderStyle"></headerstyle>
                                    <itemstyle cssclass="ItemStyle"></itemstyle>
                                    <footerstyle cssclass="FooterStyle"></footerstyle>
                                </asp:HyperLinkColumn>
                                <asp:boundcolumn datafield="Sales"  >
                                    <headerstyle horizontalalign="Right" cssclass="HeaderStyleRight"></headerstyle>
                                    <itemstyle cssclass="ItemStyleRight"></itemstyle>
                                    <footerstyle cssclass="FooterStyleRight"></footerstyle>
                                </asp:boundcolumn>
                            </columns>
                        </asp:datagrid>
                        <asp:image id="ChartImage" runat="server" visible="False" borderwidth="0"></asp:image>
                    </td>
                </tr>
            </table>
</asp:Content>

