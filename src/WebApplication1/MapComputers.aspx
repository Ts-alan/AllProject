<%@ Page Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" Inherits="MapComputers" Title="Untitled Page" Codebehind="MapComputers.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server"> 
<ajaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1"></ajaxToolkit:ToolkitScriptManager>
    <div class="title">
        <%=Resources.Resource.ComputersMap%>
    </div>
    <div runat="server" id="ItemsMap" class='contents'>
    
    </div>
</asp:Content>