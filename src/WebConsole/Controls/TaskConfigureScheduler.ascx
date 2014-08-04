<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureScheduler.ascx.cs" Inherits="Controls_TaskConfigureScheduler" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:670px"><%=Resources.Resource.ConfigureScheduler%></div>
<script type="text/javascript" src="js/Globalize.js"></script>
<script language="javascript" type="text/javascript">
    $(document).ready(function(){
        $( "#datepickerAddSchedulerTask" ).datepicker();
        $( "#datepickerAddSchedulerTask" ).datepicker( "option", "dateFormat", "dd.mm.yy" );

        $( "#timePickerAddSchedulerTask" ).timespinner();
        $( "#timePickerAddSchedulerTask" ).timespinner("option", "culture", "de-DE");

        SchedulerAddDialogSetDefault();
        LoadTableFromJSON($('#<%=hdnSchedulerTableState.ClientID %>').val());
 
  });
        /*  Hover/Click Templates   */
        $(document).on("mouseenter","[trSchedulerItemSelected]",function () {
            if ($(this).attr('trSchedulerItemSelected') == "true") return;
            $(this).css('background-color', 'yellow');
        });
        $(document).on("mouseleave", "[trSchedulerItemSelected]",function () {
            if ($(this).attr('trSchedulerItemSelected') == "true") return;
            $(this).css('background-color', '');
        });
        $(document).on("click","[trSchedulerItemSelected]",function(){
            if($(this).attr("trSchedulerItemSelected")=="true") return;
            $(this).css('background-color', '#3399ff');
            $("[trSchedulerItemSelected=true]").css('background-color', '');
            $("[trSchedulerItemSelected=true]").attr('trSchedulerItemSelected',false);
            $(this).attr('trSchedulerItemSelected',true);              
        });

          $.widget( "ui.timespinner", $.ui.spinner, {
            options: {
              step: 60 * 1000,
              page: 60
            }, 
            _parse: function( value ) {
                if ( typeof value === "string" ) {
                    // already a timestamp
                    if ( Number( value ) == value ) {
                        return Number( value );
                    }
                    return +Globalize.parseDate( value );
                }
                return value;
            }, 
            _format: function( value ) {
                return Globalize.format( new Date(value), "t" );
            }
        });
      
        //load data from json to table
        function LoadTableFromJSON(jsonTable) {
            var array=[];
            array=JSON.parse(jsonTable);
            var item=new Object();
            $('#tblSchedulerTasks tbody').empty();
            var gridStyle = "gridViewRow";
            for (var i = 0; i < array.length; i++) {
                item=array[i];
                var taskType=$('#<%=ddlAddSchedulerTaskType.ClientID %> option[value='+item.TaskType+']').text();
                var taskPeriod = $('#<%=ddlAddSchedulerTaskPeriod.ClientID %> option[value=' + item.TaskPeriod + ']').text();                
                var taskDateTime=item.TaskDateTime.substr(0,16);
                var taskIsConsideringSystemLoad = item.IsConsideringSystemLoad ? "1" : "0";

                $('#tblSchedulerTasks tbody').append('<tr trSchedulerItemSelected="false" ><td type=' + item.TaskType + '>' + taskType + '</td><td period=' + item.TaskPeriod + '>' + taskPeriod + '</td><td>' + taskDateTime + '</td><td style="display:none;">' + taskIsConsideringSystemLoad + '</td></tr>');
            }
            SchedulerTableChangeStyle();
        };

        function SchedulerTaskAddButtonClick()
        {
            var dOpt = {
                width: 350,
                resizable: false,
                close: function (event, ui) {
                    $('#divOverlay').css('display', 'none');
                    SchedulerAddDialogSetDefault();

                },
                buttons: {
                    '<%=Resources.Resource.Apply%>': function () {
                        var taskType = $('#<%=ddlAddSchedulerTaskType.ClientID %> option:selected').text();
                        var taskTypeNo = $('#<%=ddlAddSchedulerTaskType.ClientID %>').val();
                        var taskPeriod = $('#<%=ddlAddSchedulerTaskPeriod.ClientID %> option:selected').text();
                        var taskPeriodNo = $('#<%=ddlAddSchedulerTaskPeriod.ClientID %> ').val();
                        var taskDate = $('#datepickerAddSchedulerTask').val();
                        var taskTime = $('#timePickerAddSchedulerTask').val();
                        var taskIsConsideringSystemLoad = $('#<%=cboxConsideringSystemLoad.ClientID %>').is(':checked') == true ? "1" : "0";


                        $('#tblSchedulerTasks tbody').append('<tr trSchedulerItemSelected="false" ><td type=' + taskTypeNo + '>' + taskType + '</td><td period=' + taskPeriodNo + '>' + taskPeriod + '</td><td>' + taskDate + ' ' + taskTime + '</td><td style="display:none;">' + taskIsConsideringSystemLoad + '</td></tr>');
                        SchedulerTableChangeStyle()
                        SchedulerSaveTableState();
                        $('#AddSchedulerTaskDialog').dialog('close');
                    },
                    '<%=Resources.Resource.CancelButtonText%>': function () {
                        $('#AddSchedulerTaskDialog').dialog('close');
                    }
                }
            };
            $('#AddSchedulerTaskDialog').dialog(dOpt);
            $('#divOverlay').css('display','inline');
            $('#AddSchedulerTaskDialog').parent().appendTo(jQuery("form:first"));
        };

        //separate "Date Time" to {date,time}
        function SeparateDateTime(dateTime)
        {
            return dateTime.split(' ');
        };

        function SchedulerTaskChangeButtonClick()
        {        
            var row=$("[trSchedulerItemSelected=true]");
            if(row.children().length<4) return;
            
            var OldTaskTypeNo=row.children()[0].getAttribute("type");            
            var OldTaskPeriodNo=row.children()[1].getAttribute("period");
            var OldTaskDateTime=SeparateDateTime(row.children()[2].innerHTML);
            var OldTaskDate=OldTaskDateTime[0];
            var OldTaskTime = OldTaskDateTime[1];
            var OldTaskIsConsideringSystemLoad = row.children()[3].innerHTML == "1" ? true : false;

            $('#<%=ddlAddSchedulerTaskType.ClientID %>').val(OldTaskTypeNo);
            $('#<%=ddlAddSchedulerTaskPeriod.ClientID %>').val(OldTaskPeriodNo);
            $('#datepickerAddSchedulerTask').val(OldTaskDate);
            $('#timePickerAddSchedulerTask').val(OldTaskTime);
            $('#<%=cboxConsideringSystemLoad.ClientID %>').prop('checked', OldTaskIsConsideringSystemLoad);
 
                        
            var dOpt = {
                width: 350,                                       
                resizable: false,
                close: function(event, ui)
                    {
                        $('#divOverlay').css('display','none');
                        SchedulerAddDialogSetDefault();
                    },
                    buttons: {
                        '<%=Resources.Resource.Apply%>': function () {
                            var taskType=$('#<%=ddlAddSchedulerTaskType.ClientID %> option:selected').text();
                            var taskTypeNo=$('#<%=ddlAddSchedulerTaskType.ClientID %>').val();
                            var taskPeriod=$('#<%=ddlAddSchedulerTaskPeriod.ClientID %> option:selected' ).text();
                            var taskPeriodNo=$('#<%=ddlAddSchedulerTaskPeriod.ClientID %>').val();
                            var taskDate=$('#datepickerAddSchedulerTask').val();
                            var taskTime=$('#timePickerAddSchedulerTask').val(); 
                            var taskIsConsideringSystemLoad = $('#<%=cboxConsideringSystemLoad.ClientID %>').is(':checked');

                            row.children()[0].innerHTML = taskType;
                            row.children()[0].setAttribute("type",taskTypeNo);
                            row.children()[1].innerHTML = taskPeriod;
                            row.children()[1].setAttribute("period",taskPeriodNo);
                            row.children()[2].innerHTML = taskDate + ' ' + taskTime;
                            row.children()[3].innerHTML = taskIsConsideringSystemLoad ? "1" : "0";

                            SchedulerSaveTableState()
                            $('#AddSchedulerTaskDialog').dialog('close');
                        },
                        '<%=Resources.Resource.CancelButtonText%>': function () {                           
                            $('#AddSchedulerTaskDialog').dialog('close');                           
                        }
                    }
            };
            $('#AddSchedulerTaskDialog').dialog(dOpt);
            $('#divOverlay').css('display','inline');
            $('#AddSchedulerTaskDialog').parent().appendTo(jQuery("form:first"));
        };

        function SchedulerAddDialogSetDefault()
        {
            $('#<%=ddlAddSchedulerTaskType.ClientID %>').val("0");
            $('#<%=ddlAddSchedulerTaskPeriod.ClientID %>').val("0");
            $('#datepickerAddSchedulerTask').val("01.01.2014");
            $('#timePickerAddSchedulerTask').val("00:00");
            $('#<%=cboxConsideringSystemLoad.ClientID %>').prop('checked', false);
        }

        function SchedulerTaskDeleteButtonClick()
        {
            $("[trSchedulerItemSelected=true]").remove();
            SchedulerTableChangeStyle();
            SchedulerSaveTableState();
        }

        function SchedulerSaveTableState()
        {
            var array = [];
            $('#tblSchedulerTasks tbody').children("tr").each(function (index) {
                /**/
                var item = new Object();
                var row = $(this);
                item.TaskType = row.children()[0].getAttribute("type");
                item.TaskPeriod = row.children()[1].getAttribute("period");
                item.TaskDateTime = row.children()[2].innerHTML;
                item.IsConsideringSystemLoad = row.children()[3].innerHTML == "1" ? true : false;
                array.push(item);
            });
            var json=JSON.stringify(array);
            $('#<%=hdnSchedulerTableState.ClientID %>').val(json);
        }

        function SchedulerTableChangeStyle() {
            var i = 0;
            $('#tblSchedulerTasks tbody').children("tr").each(function (index) {
                if (i % 2 == 0) {
                    gridStyle = "gridViewRow";
                }
                else
                    gridStyle = "gridViewRowAlternating";
                var row = $(this);
                row.removeClass();
                row.addClass(gridStyle);
                i++;
            });
        }
</script>

<div id="divSchedulerMain" class="ListContrastTable" runat="server" style="width:501px">
      
    <div style="height: 200px; width: 500px; overflow: scroll">
    <table id="tblSchedulerTasks"  rules="cols">
        <thead >
            <th runat="server" id="tdSchedulerTaskName" style="width: 150px; text-align: center;"  class="gridViewHeader">
                <%=Resources.Resource.TaskName%>
            </th>
            <th runat="server" id="tdSchedulerTaskPeriod" style="width: 150px; text-align: center;" class="gridViewHeader">
                <%=Resources.Resource.PeriodicityType%>
            </th>
            <th runat="server" id="tdSchedulerTaskDateTime" colspan="2" style="width: 200px; text-align: center;" class="gridViewHeader">
                <%=Resources.Resource.Time%>
            </th>
            <th runat="server" id="tdSchedulerTaskIsConsideringSystemLoad" style="display:none;" class="gridViewHeader">
            </th>
        </thead>
        <tbody></tbody>
    </table>
    </div>
    <div>
        <asp:LinkButton ID="lbtnSchedulerTaskAdd"  runat="server" SkinID="Button" Width="120" OnClientClick="SchedulerTaskAddButtonClick(); return false;"><%=Resources.Resource.Add %></asp:LinkButton>
        <asp:LinkButton ID="lbtnSchedulerTaskDelete" runat="server" SkinID="Button" Width="120" OnClientClick="SchedulerTaskDeleteButtonClick(); return false;"><%=Resources.Resource.Delete %></asp:LinkButton>
        <asp:LinkButton ID="lbtnSchedulerTaskChange" runat="server" SkinID="Button" Width="120" OnClientClick="SchedulerTaskChangeButtonClick(); return false;" ><%=Resources.Resource.Change %></asp:LinkButton>
    </div>          
    <div id="divOverlay" class="ui-widget-overlay ui-front" style="display: none"></div>
    <asp:HiddenField ID="hdnSchedulerTableState" runat="server" Value=""  />
</div>

<div id="AddSchedulerTaskDialog" style="display:none; padding-bottom: 20px;" class="ui-front">
    <div><%=Resources.Resource.Schedule %></div>
    <div>
        <asp:DropDownList ID="ddlAddSchedulerTaskType" runat="server" style="width:230px;">
            <asp:ListItem Value="0" Text="<%$ Resources:Resource, ActionScan %>"></asp:ListItem>
            <asp:ListItem Value="1" Text="<%$ Resources:Resource, ActionUpdate %>"></asp:ListItem>
            <asp:ListItem Value="2" Text="<%$ Resources:Resource, Action_SCHD_CLEANING %>"></asp:ListItem>
            <asp:ListItem Value="3" Text="<%$ Resources:Resource, Action_check_devices %>"></asp:ListItem>
            <asp:ListItem Value="4" Text="<%$ Resources:Resource, Action_check_files %>"></asp:ListItem>
            <asp:ListItem Value="5" Text="<%$ Resources:Resource, Action_check_registry %>"></asp:ListItem>
            <asp:ListItem Value="6" Text="<%$ Resources:Resource, Action_save_devices %>"></asp:ListItem>
            <asp:ListItem Value="7" Text="<%$ Resources:Resource, Action_save_files %>"></asp:ListItem>
            <asp:ListItem Value="8" Text="<%$ Resources:Resource, Action_save_registry %>"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div><%=Resources.Resource.Type %></div>
    <div>
        <asp:DropDownList ID="ddlAddSchedulerTaskPeriod" runat="server" style="width:230px;">
        <asp:ListItem Value="0" Text="<%$ Resources:Resource, AtSystemStartUp %>"></asp:ListItem>
        <asp:ListItem Value="1" Text="<%$ Resources:Resource, SomeDateTime %>"></asp:ListItem>
        <asp:ListItem Value="2" Text="<%$ Resources:Resource, EveryHour %>"></asp:ListItem>
        <asp:ListItem Value="3" Text="<%$ Resources:Resource, EveryDayOfWeek %>"></asp:ListItem>    
        </asp:DropDownList>
    </div>
    <div>
        <asp:CheckBox runat="server" ID="cboxConsideringSystemLoad" /> &nbsp;<%=Resources.Resource.ConsideringSystemLoad%>
    </div>
    <div>
        <label ><%=Resources.Resource.DateAndTime%> </label>
        <p><input type="text" id="datepickerAddSchedulerTask"/> <input id="timePickerAddSchedulerTask" name="spinner" value="00:00 AM"/></p>
    </div>
</div>
