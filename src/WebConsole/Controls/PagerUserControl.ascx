<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PagerUserControl.ascx.cs" Inherits="Controls_PagerUserControl" %>
<div class="pagingDiv">
    <table width="100%">
        <tr>
            <td width="50%" align="left">
                <asp:LinkButton ID="lbtnFirst" runat="server" CommandName="Page" CommandArgument="First"
                    OnPreRender="lbtnFirst_PreRender" OnInit="lbtnNav_Init">
                    <asp:Image ID="imgFirst" runat="server" />
                </asp:LinkButton>
                <asp:LinkButton ID="lbtnPrev" runat="server" CommandName="Page" CommandArgument="Prev"
                    OnPreRender="lbtnPrev_PreRender" OnInit="lbtnNav_Init">
                    <asp:Image ID="imgPrev" runat="server" />
                </asp:LinkButton>
                <asp:LinkButton ID="lbtnNext" runat="server" CommandName="Page" CommandArgument="Next"
                    OnPreRender="lbtnNext_PreRender" OnInit="lbtnNav_Init">
                    <asp:Image ID="imgNext" runat="server" />
                </asp:LinkButton>
                <asp:LinkButton ID="lbtnLast" runat="server" CommandName="Page" CommandArgument="Last"
                    OnPreRender="lbtnLast_PreRender" OnInit="lbtnNav_Init">
                    <asp:Image ID="imgLast" runat="server" />
                </asp:LinkButton>
                <asp:LinkButton ID="lbtnGo" runat="server" CommandName="Page" OnPreRender="lbtnGo_PreRender"
                    OnInit="lbtnGo_Init" OnClick="btnGo_Click">
                    <asp:Label ID="lblGo" runat="server" ></asp:Label>
                </asp:LinkButton>
                <asp:Button ID="btnHiddenGo" runat="server" CommandName="Page" OnClick="btnGo_Click"
                    OnInit="btnHiddenGo_Init" />
                <div id="divControl" runat="server" oninit="divControl_Init" style="padding-top:3px;">
                    <asp:TextBox ID="tbPage" runat="server" OnPreRender="tbPage_PreRender" OnInit="tbPage_Init"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="fltPage" runat="server" TargetControlID="tbPage"
                        FilterType="Numbers">
                    </ajaxToolkit:FilteredTextBoxExtender>                    
                    <asp:Label ID="lblTotalPages" runat="server" OnInit="lblTotalPages_Init" OnPreRender="lblTotalPages_PreRender" ></asp:Label>
                </div>
            </td>
            <td width="20%" align="center">
                <asp:Label ID="lblDisplayedItems" runat="server" 
                    onprerender="lblDisplayedItems_PreRender" 
                    oninit="lblDisplayedItems_Init" ></asp:Label>
            </td>
            <td width="30%" align="right">
                <asp:Label ID="lblItemsPerPage" runat="server" oninit="lblItemsPerPage_Init" ></asp:Label>
                <asp:DropDownList ID="ddlPageSize" AutoPostBack="true" runat="server" SkinID="ddlPageSize" 
                    onprerender="ddlPageSize_PreRender" 
                    onselectedindexchanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem Text="10" Value="10" />
                <asp:ListItem Text="20" Value="20" />
                <asp:ListItem Text="50" Value="50" />
                <asp:ListItem Text="100" Value="100" />

                </asp:DropDownList>
            </td>
        </tr>
    </table>
</div>