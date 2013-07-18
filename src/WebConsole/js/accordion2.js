﻿
$(document).ready(function () {
    function acc_activate(event, ui) {
        $("#accordion").unmask();
        var acc = $(ui.newHeader).attr('acc');
        if (acc == "false") {
            var str = "#accordion" + $(ui.newHeader).attr('id');
            $(str).accordion({ beforeActivate: function (event, ui) { acc_before_activate(event, ui); }, activate: function (event, ui) { acc_activate(event, ui); }, collapsible: true, heightStyle: "content", autoHeight: false, active: false });
            $(ui.newHeader).attr('acc', "true");
        }
        var comp = $(ui.newPanel).find('[comp = "false"]');
        comp.attr('comp', true);
        /* загрузка данных об устройствах для компьютера */
        comp.click(function () {
            $("#accordion").mask('Loading');
            var id = $(this).attr('cp');
            var name = $(this).attr('name');
            $.ajax({
                type: "POST",
                url: "accordion2.aspx/GetComputersData",
                dataType: "json",
                data: "{id:\"" + id + "\"}",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    $("#divModalDialog").html('');
                    $("#divModalDialog").dialog('destroy');
                    var d = $("#divModalDialog");
                    d.html(msg);
                    var dOpt = {
                        title: name,
                        width: 550,
                        modal: true,
                        resizable: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    };
                    d.dialog(dOpt);
                },
                error: function (msg) { ShowJSONMessage(msg); }
            });
            $("#accordion").unmask();
        });
        /* смена фонового цвета при наведении */
         comp.hover(function ()
        {
        var id = $(this).attr('cp');
        $('div[cp=' + id + ']').css('background-color', 'yellow');
        },
        function () {
        var id = $(this).attr('cp');
        $('div[cp=' + id + ']').css('background-color', '');
        }
        );
        





    }
    function acc_before_activate(event, ui) {

        var id = $(ui.newHeader).attr('id');

        if ($(ui.newHeader).attr('load') == 'false') {

            if (id == null) return;
            $("#accordion").mask('Loading');
            $.ajax({
                type: "POST",
                url: "accordion2.aspx/GetData",
                dataType: "json",
                data: "{id:" + id + "}",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    var jsonmsg = JSON.parse(msg);
                    $(ui.newPanel).html('<table class="ListContrastTable"><tbody>' + jsonmsg.text + '</tbody></table>');
                    var acc = jsonmsg.acc;
                    /**/
                    if (acc == "null")
                        $(ui.newHeader).attr('acc', "null");
                    if (acc == "false") {
                        $(ui.newHeader).attr('acc', "false");
                        $(ui.newHeader).attr('style', "font-size:1em");
                        HeaderFunction(ui.newContent);
                    }
                }

            });
            $(ui.newHeader).attr('load', 'true');
        }
    }
    $("#divModalDialog").dialog({ autoOpen: false })

    $(function () {
        /* $("#tabs").tabs({ cookie: { expires: 30} });*/
        listRootlLoad();
        /* DevicesLoad();*/
        /*---------------------------------------------*/
        /*---------Вкладка Groups---------------------*/
        /*------- работа с устройствами для компьютера --------*/
        /* изменение комментария (вызов диалогового окна)*/
        $(document).on("click", 'img[comdp]', function () {
            var id = $(this).attr('comdp');
            var serial = $(this).attr('serialdp');
            $.ajax({
                type: "POST",
                url: "accordion2.aspx/GetChangeCommentDialog",
                data: "{id:" + id + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $("#commentDialog").html('');
                    var d = $('#commentDialog');
                    d.html(msg);
                    var dOpt = {
                        title: serial,
                        width: '700px',
                        resizable: false
                    };
                    d.dialog(dOpt);
                },
                error: function (msg) {
                    alert(msg);
                }
            })
        });
        /* изменение комментария */
        $(document).on("click", "button[dcdpc]", function () {
            var id = $(this).attr('dcdpc');
            var comment = $('input[dcdpc]').val();
            $.ajax({
                type: "POST",
                url: "accordion2.aspx/ChangeComment",
                data: "{id:" + id + ", comment:'" + comment + "'}",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    $("td[dp=" + id + "][type='comment']").html(comment);
                    $("#commentDialog").dialog('close');
                },
                error: function (msg) {
                    ShowJSONMessage(msg);
                }
            });
            return false;
        });
        /*удаление devicePolicy для компьютера */
        $(document).on("click", "img[deldp]", function () {
            var id = $(this).attr('deldp');
            $.ajax({
                type: "POST",
                url: "accordion2.aspx/RemoveDevicePolicy",
                data: "{id:" + id + "}",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    $("img[deldp=" + id + "]").parent().parent().remove();
                },
                error: function (msg) {
                    ShowJSONMessage(msg);
                }
            });
            return false;
        });
        /* change devicePolicy state for computer*/
        $(document).on("click", "img[cp]", function () {
            var dp = $(this).attr('dp');
            var cp = $(this).attr('cp');
            var state = $(this).attr('state');
            //установка смены чекбоксов
            if (state == "Enabled" || state == "Undefined") {
                $(this).attr('state', "Disabled");
                state = "Disabled";
                $(this).attr('src', "App_Themes/Main/Images/disabled.gif");
            } else if (state == "Disabled") {
                $(this).attr('state', "Enabled");
                state = "Enabled";
                $(this).attr('src', "App_Themes/Main/Images/enabled.gif");
            }
            $.ajax({
                type: "POST",
                url: "accordion2.aspx/ChangeDevicePolicyStateComputer",
                data: "{dp:" + dp + ",cp:" + cp + ",state:'" + state + "'}",
                contentType: "application/json; charset=utf-8",
                success: function () {

                }
            });
        });
        /* add new policy to computer */
        $(document).on("click", "button[dpc]", function () {
            var id = $(this).attr('dpc');
            var serial = $('input[dpc]').val();
            $.ajax({
                type: "POST",
                url: "accordion2.aspx/AddNewDevicePolicyToComputer",
                data: "{id:" + id + ", serial:'" + serial + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    var deviceMsg = JSON.parse(msg);
                    var success = deviceMsg.success;
                    if (success == "true") {
                        var row = "<td>" + serial.toString() + "</td>";
                        var comment = deviceMsg.comment;
                        var deviceID = deviceMsg.deviceID;
                        var policyID = deviceMsg.policyID;
                        var change = deviceMsg.change;
                        var del = deviceMsg.del;
                        row += "<td type='comment' dp=" + deviceID + " >" + comment + "</td>";

                        row += "<td style='width:60px'>" + "</td>";

                        var select = "<img dp=" + deviceID + " cp=" + id + " state=Undefined src=\'App_Themes/Main/Images/undefined.gif\' />";
                        row += "<td>" + select + "</td>";
                        row += "<td><img title=" + change + " comdp=" + deviceID + " serialdp=" + serial + " src=\'App_Themes/Main/Images/table_edit.png\' /><img title='" + del + "' deldp=" + policyID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
                        /*  row += "</tr>";*/
                        $("table[cp=" + id + "] > tbody:last").append('<tr></tr>');
                        var lastLine = $("table[cp=" + id + "] tr:last");

                        lastLine.html(row);
                        lastLine.attr('style', 'text-align:center');
                    }
                    else {

                        alert(deviceMsg.error);
                    }
                },
                error: function (msg) {
                    ShowJSONMessage(msg);
                }
            });
            return false;
        });


        /*----- работа с устройствами для групп ---------*/
        /* Add new device to group
        if(id<0) add new device to withoutGroup */
        $(document).on("click", "button[dgr]", function () {
            var id = $(this).attr('dgr');
            var serial = $('input[dgr]').val();
            $.ajax({
                type: "POST",
                url: "accordion2.aspx/AddNewDevicePolicyGroup",
                data: "{id:" + id + ", serial:'" + serial + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    var deviceMsg = JSON.parse(msg);
                    var success = deviceMsg.success;
                    if (success == "true") {
                        var DevID = deviceMsg.id;
                        var img = $('img[dp=' + DevID + ']');
                        if (img.length != 0) {
                            img.attr('src', "App_Themes/Main/Images/undefined.gif");
                        }
                        else {

                            var row = "<td></td><td>" + deviceMsg.serial + "</td>";
                            row += "<td type='comment' dp=" + DevID + ">" + deviceMsg.comment + "</td><td></td>";
                            row += "<td style='width:60px'></td>";
                            row += "<td><img dp= " + DevID + " gdp=" + id + " state=Undefined src=\'App_Themes/Main/Images/undefined.gif\' /></td>";
                            row += "<td><img title='ChangeComment' comdp=" + DevID + " serialdp=" + deviceMsg.serial + " src=\'App_Themes/Main/Images/table_edit.png\' />";
                            if (id >= 0)
                                row += "<img title='Delete' delgroupid=" + id + " delgroupdevid=" + DevID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
                            else row += "<img title='Delete' delwithoutgroupdevid=" + DevID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
                            $("table[gdp=" + id + "] > tbody:last").append('<tr></tr>');
                            var lastLine = $("table[gdp=" + id + "] tr:last");
                            lastLine.html(row);
                            lastLine.attr('style', 'text-align:center');
                        }
                        $('img[nfadp=' + DevID + ']').remove();
                        /*  $("div[cp=" + id + "]").click();
                        ShowRefreshMessage();*/
                    }
                    else {
                        var error = deviceMsg.error;
                        alert(error);
                    }
                },
                error: function () {
                    alert('fail');
                }
            });

            return false;
        });
        /* change devicePolicy state for group*/
        $(document).on("click", "img[gdp]", function () {
            var gp = $(this).attr('gdp');
            var dp = $(this).attr('dp');
            var state = $(this).attr('state');
            //установка смены чекбоксов
            if (state == "Enabled" || state == "Undefined") {
                $(this).attr('state', "Disabled");
                state = "Disabled";
                $(this).attr('src', "App_Themes/Main/Images/disabled.gif");
            } else if (state == "Disabled") {
                $(this).attr('state', "Enabled");
                state = "Enabled";
                $(this).attr('src', "App_Themes/Main/Images/enabled.gif");
            }
            $.ajax({
                type: "POST",
                url: "accordion2.aspx/ChangeDevicePolicyStateGroup",
                data: "{dp:" + dp + " ,gp:" + gp + ",state:'" + state + "'}",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    //alert(state);
                }
            });
        });
        /*удаление device для группы */
        $(document).on("click", "img[delgroupdevid]", function () {
            var devId = $(this).attr('delgroupdevid');
            var groupId = $(this).attr('delgroupid');
            $.ajax({
                type: "POST",
                url: "accordion2.aspx/RemoveDevicePolicyGroup",
                data: "{devid:" + devId + ",groupid:" + groupId + "}",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    $("img[delgroupdevid=" + devId + "]").parent().parent().remove();
                },
                error: function (msg) {
                    ShowJSONMessage(msg);
                }
            });
            return false;
        });
        /* удаление девайсов для компьютеров без группы*/
        $(document).on("click", "img[delwithoutgroupdevid]", function () {
            var id = $(this).attr('delwithoutgroupdevid');
            $.ajax({
                type: "POST",
                url: "accordion2.aspx/RemoveDevicePolicyWithoutGroup",
                data: "{id:" + id + "}",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    $("img[delwithoutgroupdevid=" + id + "]").parent().parent().remove();
                },
                error: function (msg) {
                    ShowJSONMessage(msg);
                }
            });
            return false;
        });

        /* ---------------accordion --------------------*/
        /* перед открытием вкладки */



        /* после открытия вкладки */







    });



    /* запрет на раскрытие вкладок и загрузка данных об устройствах */
    function HeaderFunction(content) {

        /*    var header = $(content).find('h3');

        header.click(function (event) {
        var id = $(this).attr('id');
        var name = $(this).text();
        if (id == "null") id = -1; 
        $.ajax({
        type: "POST",
        url: "accordion2.aspx/GetGroupDeviceData",
        dataType: "json",
        data: "{id:\"" + id + "\"}",
        contentType: "application/json; charset=utf-8",
        success: function (msg) {
        $("#divModalDialog").html('');
        $("#divModalDialog").dialog('destroy');
        var d = $("#divModalDialog");
        d.html(msg);
        var dOpt = {
        title: name,
        width: 650,
        modal: true,
        resizable: true,
        buttons: {
        Ok: function () {
        $(this).dialog("close");
        }
        }
        };
        d.dialog(dOpt);
        },
        error: function (msg) { ShowJSONMessage(msg); }
        });
        });*/
    }
    /* загрузка начальных данных */
    function listRootlLoad() {
        $.ajax({
            type: "POST",
            url: "accordion2.aspx/GetListRootGroup",
            dataType: "json",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                $("#accordion").append(msg).accordion({ beforeActivate: function (event, ui) { acc_before_activate(event, ui); }, activate: function (event, ui) { acc_activate(event, ui); }, collapsible: true, heightStyle: "content", autoHeight: false, active: false });
                HeaderFunction("#accordion");
            },
            error: function (msg) { alert("Error"); ShowJSONMessage(msg); }
        });
    };
    /*---------------------------------------------*/
    /*---------Вкладка Devices---------------------*/
    /* загрузка устройств*/
    function DevicesLoad() {
        $.ajax({
            type: "POST",
            url: "accordion2.aspx/GetAllDevices",
            dataType: "json",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                $("#tab2").html(msg);
            },
            error: function (msg) { ShowJSONMessage(msg); }
        });
    }
    $("[delete=true]").bind("click", function () {
        if (!confirm('Are you sure?'))
            return;
        var id = $(this).attr('dcp');
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/DeleteDevice",
            data: "{id:" + id + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function () {
                $('[dev=' + id + ']').remove();
                /*ShowRefreshMessage();*/
            }
        })
    });

});    

    