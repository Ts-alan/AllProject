<%@ Page Title="" Language="C#" MasterPageFile="~/mstrPageNew.master" AutoEventWireup="true" CodeFile="TestTreePage.aspx.cs" Inherits="TestTreePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
<ajaxToolkit:ToolkitScriptManager  ID="ScriptManager1" runat="server" />

<script language="javascript" type="text/javascript">

    $(document).ready(function () {


          $.ajax({
        type: "GET",
        url: "Handlers/ComputerPageHandler.ashx",
        dataType: "json"
        }).done(function (treeData) {
        $('#divTree').jstree({
        'core': {
        "check_callback": true,
        'data': treeData
        },
        'types' : {
        "group" : {
        "icon" : "glyphicon glyphicon-flash"
        },
        "leaf" : {
        "icon": " x-tree-icon-leaf "
        }
        },
        'plugins': ["checkbox","state","types" ]
        });
           
        

        $('#divTree').jstree({
            'core': {
                'data': {
                    'url': 'Handlers/ComputerPageHandler.ashx',
                    'data': function (node) {
                        return { 'id': node.id };
                    }
                }
            }

        });


        /* $('#divTree').on('hover_node.jstree', function (e, data) { alert(data) });*/
    });


</script>
    <asp:Panel runat="server">
        <table>
            <tr>
                <td>
                    <input type="button" value="Collapse All" onclick="$('#divTree').jstree('close_all');">
                    <input type="button" value="Expand All" onclick="$('#divTree').jstree('open_all');">
                </td>
            </tr>
            <tr>
                <td><div id="divTree"></div></td>
            </tr>
        </table>
    </asp:Panel>

    
    

</asp:Content>

