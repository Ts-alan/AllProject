<%@ Page Language="C#" validateRequest=false MasterPageFile="~/mstrPageMain.master"  AutoEventWireup="true" CodeFile="CompFilters.aspx.cs" Inherits="CompFilters" Title="Untitled Page" %>
<%@ Register Src="~/Controls/CompFiltersMain.ascx" TagName="CompFilters" TagPrefix="cmpfltMain" %>
<%@ Register Src="~/Controls/CompFiltersExtra.ascx" TagName="CompFilters" TagPrefix="cmpfltExtra" %>
<%@ Register Src="~/Controls/CompFiltersDate.ascx" TagName="CompFilters" TagPrefix="cmpfltDate" %>
<%@ Register Src="~/Controls/CompFiltersBool.ascx" TagName="CompFilters" TagPrefix="cmpfltBool" %>
<%@ OutputCache Location="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">

<script language="javascript" type="text/javascript">

function ValidationFilterName()
{
    var tbox = document.getElementById('ctl00_cphMainContainer_tboxSaveAs');
    
    if(tbox.value == "")
    {
        alert("Name is empty.");
        return false;
    }
    if(tbox.value.length > 40)
    {
        alert("No correct length name.");
        return false;
    }

    var expr = new RegExp('<%=RegularExpressions.FilterName %>');
    if(!expr.test(tbox.value))
    {
        alert('No correct name.');
        return false;            
    }
    else
    {
        return true;
    }
}

</script>

    <div class="title"><%=Resources.Resource.Filter%></div>
    <div  class="divSettings">
    <table class="ListContrastTable">
        <tr>
             <td colspan="2" align="center">
                <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
          <td class="area">
            <cmpfltMain:CompFilters runat="server" ID="cmpfltMain" />
          </td>
          <td class="area">
            <cmpfltExtra:CompFilters runat="server" ID="cmpfltExtra" />
          </td>
        </tr>
         <tr>
          <td colspan="2" class="area">
            <cmpfltDate:CompFilters runat="server" ID="cmpfltDate" />
          </td>
          </tr>
         <tr>
          <td colspan="2" class="area">
            <cmpfltBool:CompFilters runat="server" ID="cmpfltBool" />
          </td>
        </tr>
    </table> 
    </div>
    <div  class="divSettings">
        <table class="FilterActions">
				<tr>
					<td><asp:textbox id="tboxSaveAs" Runat="server" style="width:340px"></asp:textbox></td>
					<td><asp:LinkButton id="btnSaveAs" Runat="server" onclick="btnSaveAs_Click" OnClientClick="return ValidationFilterName()"></asp:LinkButton></td>
				</tr>
			</table>  
	</div>
</asp:Content>

