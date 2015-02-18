<%@ Page Language="C#" MasterPageFile="~/mstrPageMain.master" AutoEventWireup="true" Inherits="Visual" Title="Untitled Page" Codebehind="Visual.aspx.cs" %>

<%@ Register Src="~/Controls/CompositeFilter.ascx" TagName="CompositeFilter" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterComputers.ascx" TagName="FilterComputers" TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDropDownListForStatistics.ascx" TagName="FilterList"  TagPrefix="flt" %>
<%@ Register Src="~/Controls/PrimitiveFilterDateTime.ascx" TagName="FilterDateTime"  TagPrefix="flt" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContainer" Runat="Server">
    <div class="title"><%=Resources.Resource.Diagram%></div>
    <script type="text/javascript">
        var select = null;
        var plot = null;
        var chartData = null;
        $(window).load(function () {
            var where = $('#<%= hdnFieldWhere.ClientID %>').attr('value');
            if (where != null && where != "undefined" && where != "")
            {
                var groupBy = $('#<%= HiddenFieldGroupBy.ClientID %>').attr('value');                
                getChartData(where, groupBy);
            }
        });
        $(document).ready(function () {
            disableEnableButtons();
            BarAttributes = {
                seriesDefaults: {
                    renderer: $.jqplot.BarRenderer,
                    pointLabels: { show: true, location: 'n', fontSize: '25px', edgeTolerance: -15 },
                    rendererOptions: {
                        varyBarColor: true
                    }
                },
                axes: {
                    xaxis: {
                        renderer: $.jqplot.CategoryAxisRenderer
                    }
                }
            };
            PieAttribute = {
                seriesDefaults: {
                    renderer: jQuery.jqplot.PieRenderer,
                    rendererOptions: {
                        showDataLabels: true
                    }
                },
                legend: { show: true, location: 'e', fontSize: '12px', labels: [] }
            };
            LineAttribute = {
                axesDefaults: {
                    tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                    tickOptions: {
                        angle: 30
                    }
                },
                axes: {
                    xaxis: {
                        renderer: $.jqplot.CategoryAxisRenderer
                    },
                    yaxis: {
                        autoscale: true
                    }
                }
            };
            $('#<%= pieImageButton.ClientID %>').click(function () {
                select = "pie";
                createChart();
            });
            $('#<%= barImageButton.ClientID %>').click(function () {
                select = "bar";
                createChart();
            });
            $('#<%= lineImageButton.ClientID %>').click(function () {
                select = "line";
                createChart();
            })
        });
        function disableEnableButtons() {
            $('#<%= barImageButton.ClientID %>').attr('src', 'App_Themes/Main/Images/chart-bar-disabled.png');
            $('#<%= lineImageButton.ClientID %>').attr('src', 'App_Themes/Main/Images/chart-line-disabled.png');
            $('#<%= pieImageButton.ClientID %>').attr('src', 'App_Themes/Main/Images/chart-pie-disabled.png');
            if (select == "bar") $('#<%= barImageButton.ClientID %>').attr('src', 'App_Themes/Main/Images/chart-bar.png');
            if (select == "pie") $('#<%= pieImageButton.ClientID %>').attr('src', 'App_Themes/Main/Images/chart-pie.png');
            if (select == "line") $('#<%= lineImageButton.ClientID %>').attr('src', 'App_Themes/Main/Images/chart-line.png');
        }
        function createChart() {           
            if (plot != null) {
                plot.destroy();
            }
            $('#chart1').empty();
            if (chartData == null) {

                $('#<%= lblChartError.ClientID %>').css('display', 'inline');
                $('#<%= lblChartError.ClientID %>').text("<%=Resources.Resource.SetDiagramCriteria %>");
                select = null;
                disableEnableButtons();
                return;
            }
            if (chartData.length == 0) {
                $('#<%= lblChartError.ClientID %>').css('display', 'inline');
                $('#<%= lblChartError.ClientID %>').text("<%=Resources.Resource.NoDataChartDisplayed %>");
                select = null;
                disableEnableButtons();
                return;
            }
            $('#<%= lblChartError.ClientID %>').css('display', 'none');
            if (select == null) select = "pie";
            disableEnableButtons();
            switch (select) {
                case "pie":
                    var legend = [];
                    var count = 0;
                    for (var i = 0; i < chartData.length; i++) {
                        legend[i] = chartData[i][0] + "\t" + chartData[i][1];
                        count += +chartData[i][1];
                    }
                    chartData.push(["", 0]);
                    legend[chartData.length - 1] = "TOTAL" + "\t" + count;
                    attr = PieAttribute;
                    attr.legend.labels = legend;
                    break;
                case "bar":
                    attr = BarAttributes;
                    break;
                case "line":
                    attr = LineAttribute;
                    break;
            }
            plot = jQuery.jqplot('chart1', [chartData], attr);
            if (select == "pie") chartData.pop();
        }
        function getChartData( where, groupBy) {
           
            var attr = null;
            var ret = null;
            $.ajax({
                type: "POST",
                async: true,
                url: "Visual.aspx/getData",
                dataType: "json",
                data: "{where:\"" + where + "\", groupBy:'" + groupBy + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    ret = $.parseJSON(msg);
                    chartData = ret.data;
                    createChart();
                },
                error: function (msg) {
                    ShowJSONMessage(msg);
                }
            });
            function ShowJSONMessage(msg) {
                var m = JSON.parse(msg.responseText, function (key, value) {
                    var type;
                    if (value && typeof value === 'object') {
                        type = value.type;
                        if (typeof type === 'string' && typeof window[type] === 'function') {
                            return new (window[type])(value);
                        }
                    }
                    return value;
                });
                alert(m.Message);
            }
        }
    </script>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel runat="server" ID="updatePanelEventsFilter">
        <ContentTemplate>
            <flt:CompositeFilter ID="FilterContainer" UserFiltersTemproraryStorageName="DiagramFiltersTemp"
                InformationListType="Diagrams" UserFiltersProfileKey="DiagramFilters" runat="server"
                OnActiveFilterChange="FilterContainer_ActiveFilterChanged">
                <FiltersTemplate>
                    <table>
                        <tr>
                            <td colspan="2">
                                <flt:FilterList runat="server" ID="fltType" NameFieldDB="EventName" TextFilter='<%$ Resources:Resource, Type %>' />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <flt:FilterComputers runat="server" ID="fltComputers" NameFieldDB="ComputerName"  TextFilter='<%$ Resources:Resource, ComputerName %>' />                                   
                            </td>
                            <td valign="top" style="padding-left: 20px;">                                
                                <flt:FilterDateTime runat="server" ID="fltEventTime" NameFieldDB="EventTime" TextFilter="<%$ Resources:Resource, EventTime %>" />
                            </td>
                        </tr>
                    </table>
                </FiltersTemplate>
            </flt:CompositeFilter>
           <asp:HiddenField ID="hdnFieldWhere"  runat="server"/>
           <asp:HiddenField ID="HiddenFieldGroupBy"  runat="server"/>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Panel ID="chartTypePanel" runat="server" HorizontalAlign="Center">
        <asp:Table runat="server" HorizontalAlign="Center">
            <asp:TableRow runat="server">
                <asp:TableCell runat="server" Width="100px">                   
                    <asp:ImageButton runat="server" ID="pieImageButton" ImageUrl="~/App_Themes/Main/Images/chart-pie.png" ToolTip='<%$ Resources:Resource, Pie %>'  OnClientClick="return false"/>
                </asp:TableCell>
                <asp:TableCell ID="TableCell1" runat="server" Width="100px">
                    <asp:ImageButton ID="barImageButton" runat="server" ImageUrl="~/App_Themes/Main/Images/chart-bar.png" ToolTip='<%$ Resources:Resource, Bar %>'  OnClientClick="return false"/>
                </asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" Width="100px">
                    <asp:ImageButton ID="lineImageButton" runat="server" ImageUrl="~/App_Themes/Main/Images/chart-line.png" ToolTip='<%$ Resources:Resource, Line %>'   OnClientClick="return false"/>
                </asp:TableCell>
            </asp:TableRow>          
        </asp:Table>
    </asp:Panel>
    <div style="text-align:center; top:50px; position:relative">
        <asp:Label ID="lblChartError" runat="server" Visible="true" Font-Size='Large' ><%=Resources.Resource.SetDiagramCriteria %></asp:Label>
    </div>        
    <div id='chart1' style=" width:600px; left:27%; position:relative" ></div>  
</asp:Content>