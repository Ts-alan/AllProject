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
        function  LoadTableFromJSON(jsonTable)
        {
            var array=[];
            array=JSON.parse(jsonTable);
            var item=new Object();
            $('#tblSchedulerTasks tbody').empty();
            for(var i=0;i<array.length;i++)
            {
                item=array[i];
                var taskType=$('#<%=ddlAddSchedulerTaskType.ClientID %> option[value='+item.TaskType+']').text();
                var taskPeriod=$('#<%=ddlAddSchedulerTaskPeriod.ClientID %> option[value='+item.TaskPeriod+']').text();                
                var taskDateTime=item.TaskDateTime.substr(0,item.TaskDateTime.length-3);

                $('#tblSchedulerTasks tbody').append('<tr trSchedulerItemSelected="false" ><td type='+item.TaskType+'>' + taskType + '</td><td period='+item.TaskPeriod+'>'+taskPeriod+'</td><td>'+taskDateTime+'</td></tr>');   
                        
            }
        };
        function SchedulerTaskAddButtonClick()
        {
            var dOpt = {
                width: 350,                                       
                resizable: false,
                close: function(event, ui)
                    {
                        $('#divOverlay').css('display','none');
                        SchedulerAddDialogSetDefault();
                       
                    },
                buttons: {
                    <%=Resources.Resource.Apply%>: function () {
                        var taskType=$('#<%=ddlAddSchedulerTaskType.ClientID %> option:selected').text();
                        var taskTypeNo=$('#<%=ddlAddSchedulerTaskType.ClientID %>').val();
                        var taskPeriod=$('#<%=ddlAddSchedulerTaskPeriod.ClientID %> option:selected').text();
                        var taskPeriodNo=$('#<%=ddlAddSchedulerTaskPeriod.ClientID %> ').val();
                        var taskDate=$('#datepickerAddSchedulerTask').val();
                        var taskTime=$('#timePickerAddSchedulerTask').val();
                        $('#tblSchedulerTasks tbody').append('<tr trSchedulerItemSelected="false" ><td type='+taskTypeNo+'>' + taskType + '</td><td period='+taskPeriodNo+'>'+taskPeriod+'</td><td>'+taskDate+' '+taskTime+'</td></tr>');   
                             
                        SchedulerSaveTableState();
                        $('#AddSchedulerTaskDialog').dialog('close');
                    },
                    <%=Resources.Resource.CancelButtonText%>: function () {                           
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
            if(row.children().length<3) return;
            var OldTaskType=row.children()[0].innerText;
            var OldTaskTypeNo=row.children()[1].getAttribute("type");
            var OldTaskPeriod=row.children()[1].innerText;
            var OldTaskPeriodNo=row.children()[1].getAttribute("period");
            var OldTaskDateTime=SeparateDateTime(row.children()[2].innerText);
            var OldTaskDate=OldTaskDateTime[0];
            var OldTaskTime=OldTaskDateTime[1];

            $('#<%=ddlAddSchedulerTaskType.ClientID %>').val(OldTaskType);
            $('#<%=ddlAddSchedulerTaskPeriod.ClientID %>').val(OldTaskPeriodNo);
            $('#datepickerAddSchedulerTask').val(OldTaskDate);
            $('#timePickerAddSchedulerTask').val(OldTaskTime);
 
                        
            var dOpt = {
                width: 350,                                       
                resizable: false,
                close: function(event, ui)
                    {
                        $('#divOverlay').css('display','none');
                        SchedulerAddDialogSetDefault();
                    },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () {
                            var taskType=$('#<%=ddlAddSchedulerTaskType.ClientID %> option:selected').text();
                            var taskTypeNo=$('#<%=ddlAddSchedulerTaskType.ClientID %>').val();
                            var taskPeriod=$('#<%=ddlAddSchedulerTaskPeriod.ClientID %> option:selected' ).text();
                            var taskPeriodNo=$('#<%=ddlAddSchedulerTaskPeriod.ClientID %>').val();
                            var taskDate=$('#datepickerAddSchedulerTask').val();
                            var taskTime=$('#timePickerAddSchedulerTask').val(); 

                            row.children()[0].innerText =taskType;
                            row.children()[0].setAttribute("type",taskTypeNo);
                            row.children()[1].innerText =taskPeriod;
                            row.children()[1].setAttribute("period",taskPeriodNo);
                            row.children()[2].innerText =taskDate+' '+taskTime;
                            SchedulerSaveTableState()
                            $('#AddSchedulerTaskDialog').dialog('close');
                        },
                        <%=Resources.Resource.CancelButtonText%>: function () {                           
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
        }
        function SchedulerTaskDeleteButtonClick()
        {
            $("[trSchedulerItemSelected=true]").remove();
            SchedulerSaveTableState()
        }
        function SchedulerSaveTableState()
        {
            var array=[];
            $('#tblSchedulerTasks tbody').children("tr").each(function(index){
                    /**/            
            var item=new Object();
            var row=$(this);
            item.TaskType=row.children()[0].getAttribute("type");
            item.TaskPeriod=row.children()[1].getAttribute("period");
            item.TaskDateTime=row.children()[2].innerText;
            array.push(item);      
            });
            var json=JSON.stringify(array);
            $('#<%=hdnSchedulerTableState.ClientID %>').val(json);
        }
</script>

<div id="divSchedulerMain" class="ListContrastTable" runat="server">
      
    <div style="height: 200px; width: 500px; overflow: scroll">
    <table id="tblSchedulerTasks"  rules="cols">
        <thead >
            <th runat="server" id="tdSchedulerTaskName" style="width: 150px; text-align: center;"  class="listRulesHeader">
                <%=Resources.Resource.TaskName%>
            </th>
            <th runat="server" id="tdSchedulerTaskPeriod" style="width: 150px; text-align: center;" class="listRulesHeader">
                <%=Resources.Resource.PeriodicityType%>
            </th>
            <th runat="server" id="tdSchedulerTaskDateTime" colspan="2" style="width: 200px; text-align: center;" class="listRulesHeader">
                <%=Resources.Resource.Time%>
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
    <%=Resources.Resource.Schedule %><br />
    <asp:DropDownList ID="ddlAddSchedulerTaskType" runat="server">
        <asp:ListItem Value="0" Text="<%$ Resources:Resource, ActionScan %>"></asp:ListItem>
        <asp:ListItem Value="1" Text="<%$ Resources:Resource, ActionUpdate %>"></asp:ListItem>
    </asp:DropDownList>
    <br />
    <%=Resources.Resource.Type %>
    <br />
    <asp:DropDownList ID="ddlAddSchedulerTaskPeriod" runat="server">
    <asp:ListItem Value="0" Text="<%$ Resources:Resource, AtSystemStartUp %>"></asp:ListItem>
    <asp:ListItem Value="1" Text="<%$ Resources:Resource, SomeDateTime %>"></asp:ListItem>
    <asp:ListItem Value="2" Text="<%$ Resources:Resource, EveryHour %>"></asp:ListItem>
    <asp:ListItem Value="3" Text="<%$ Resources:Resource, EveryDayOfWeek %>"></asp:ListItem>
    <asp:ListItem Value="4" Text="<%$ Resources:Resource, EveryDayOfMonth %>"></asp:ListItem>
    </asp:DropDownList>
    <br />
    <label ><%=Resources.Resource.DateAndTime%> </label>
    <p><input type="text" id="datepickerAddSchedulerTask"/> <input id="timePickerAddSchedulerTask" name="spinner" value="00:00 AM"/></p>

</div>
