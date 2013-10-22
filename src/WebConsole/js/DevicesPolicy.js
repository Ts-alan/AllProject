
$(document).ready(function () {
    function acc_activate(event, ui) {

        var acc = $(ui.newHeader).attr('acc');
        if (acc == "false") {
            var str = "#accordion" + $(ui.newHeader).attr('id');
            HeaderFunction(ui.newPanel);
            $(str).accordion({ beforeActivate: function (event, ui) { acc_before_activate(event, ui); }, activate: function (event, ui) { acc_activate(event, ui); }, collapsible: true, heightStyle: "content", autoHeight: false, active: false });
            $(ui.newHeader).attr('acc', "true");
        }
        var comp = $(ui.newPanel).find('[comp = "false"]');
        comp.attr('comp', true);
        /* загрузка данных об устройствах для компьютера */
        comp.click(function () {
            $("#accordion").mask(Resource.Loading);
            var id = $(this).attr('cp');
            var name = $(this).attr('name');
            $.ajax({
                type: "POST",
                url: "DevicesPolicy.aspx/GetComputersData",
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
                        width: 1000,
                        modal: true,
                        resizable: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    };
                    d.dialog(dOpt);
                    d.find("button").button();
                },
                error: function (msg) { ShowJSONMessage(msg); }
            });
            $("#accordion").unmask();
        });
        /* смена фонового цвета при наведении */
        comp.hover(function () {
            var id = $(this).attr('cp');
            $('div[cp=' + id + ']').css('background-color', 'yellow');
        },
        function () {
            var id = $(this).attr('cp');
            $('div[cp=' + id + ']').css('background-color', '');
        }
        );

        $("#accordion").unmask();
    }

    function acc_before_activate(event, ui) {
        var id = $(ui.newHeader).attr('id');

        if ($(ui.newHeader).attr('load') == 'false') {

            if (id == null) return;
            $("#accordion").mask(Resource.Loading);
            $.ajax({
                type: "POST",
                url: "DevicesPolicy.aspx/GetData",
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

                    }
                }

            });
            $(ui.newHeader).attr('load', 'true');
        }
    }
    $("#divModalDialog").dialog({ autoOpen: false })

    $(function () {
        /* $("#tabs").tabs({ cookie: { expires: 30} });*/
        listRootLoad();
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
                url: "DevicesPolicy.aspx/GetChangeCommentDialog",
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
                        modal: true,
                        resizable: false
                    };
                    d.dialog(dOpt);
                    d.find("button").button();
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
                url: "DevicesPolicy.aspx/ChangeComment",
                data: "{id:" + id + ", comment:'" + comment + "'}",
                contentType: "application/json; charset=utf-8",
                success: function () {

                    $("td[dp=" + id + "][type='comment']").html(comment);
                    $("span[dp=" + id + "][type='comment']").html(comment);
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
                url: "DevicesPolicy.aspx/RemoveDevicePolicy",
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
            changeState($(this), dp, cp, state);
        });
        function changeState(img, dp, cp, state) {
            if (state == "Enabled" || state == "Undefined") {
                img.attr('state', "Disabled");
                state = "Disabled";
                img.attr('src', "App_Themes/Main/Images/disabled.gif");
            } else if (state == "Disabled") {
                img.attr('state', "Enabled");
                state = "Enabled";
                img.attr('src', "App_Themes/Main/Images/enabled.gif");
            }
            $.ajax({
                type: "POST",
                url: "DevicesPolicy.aspx/ChangeDevicePolicyStateComputer",
                data: "{dp:" + dp + ",cp:" + cp + ",state:'" + state + "'}",
                contentType: "application/json; charset=utf-8",
                success: function () {

                }
            });
        };
        /* change devicePolicy state for computer*/
        $(document).on("click", "img[treestatecp]", function () {
            var dp = $(this).attr('treestatedev');
            var cp = $(this).attr('treestatecp');
            var state = $(this).attr('state');
            //установка смены чекбоксов
            changeState($(this), dp, cp, state);
        });
        /* add new policy to computer */
        $(document).on("click", "button[dpc]", function () {
            var id = $(this).attr('dpc');
            var serial = $('input[dpc]').val();
            $.ajax({
                type: "POST",
                url: "DevicesPolicy.aspx/AddNewDevicePolicyToComputer",
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

                        row += "<td type='comment' dp=" + deviceID + " >" + comment + "</td>";

                        row += "<td style='width:60px'>" + "</td>";

                        var select = "<img style='cursor:pointer' dp=" + deviceID + " cp=" + id + " state=Undefined src=\'App_Themes/Main/Images/undefined.gif\' />";
                        row += "<td>" + select + "</td>";
                        row += "<td><img style='cursor:pointer' title=" + Resource.ChangeComment + " comdp=" + deviceID + " serialdp=" + serial + " src=\'App_Themes/Main/Images/table_edit.png\' /><img title='" + Resource.Delete + "' deldp=" + policyID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
                        /*  row += "</tr>";*/
                        $("table[cp=" + id + "] > tbody:last").append('<tr></tr>');
                        var lastLine = $("table[cp=" + id + "] tr:last");

                        lastLine.html(row);
                        lastLine.attr('style', 'text-align:center');

                    }
                    else {

                        alert(Resource.Error);
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
                url: "DevicesPolicy.aspx/AddNewDevicePolicyGroup",
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
                            row += "<td><img style='cursor:pointer' dp= " + DevID + " gdp=" + id + " state=Undefined src=\'App_Themes/Main/Images/undefined.gif\' /></td>";
                            row += "<td><img style='cursor:pointer' title='" + Resource.ChangeComment + "' comdp=" + DevID + " serialdp=" + deviceMsg.serial + " src=\'App_Themes/Main/Images/table_edit.png\' />";
                            if (id >= 0)
                                row += "<img style='cursor:pointer' title='" + Resource.Delete + "' delgroupid=" + id + " delgroupdevid=" + DevID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
                            else row += "<img style='cursor:pointer' title='" + Resource.Delete + "' delwithoutgroupdevid=" + DevID + " src=\'App_Themes/Main/Images/deleteicon.png\' /></td>";
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

                        alert(Resource.Error);
                    }
                },
                error: function (msg) {
                    ShowJSONMessage(msg);
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
                url: "DevicesPolicy.aspx/ChangeDevicePolicyStateGroup",
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
                url: "DevicesPolicy.aspx/RemoveDevicePolicyGroup",
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
                url: "DevicesPolicy.aspx/RemoveDevicePolicyWithoutGroup",
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

        var header = $(content).find("h3");
        header.append('<button class="stop" style="position:absolute; right: 5px; bottom:1px; top: 1px ">' + Resource.Devices + '</button>');
        var button = $(header).find("button")
        button.button();
        button.on("click", function (event) {
            var id = $(this).parent().attr('id');
            var name = $(this).parent().find('a>span').text();
            if (id == "null") id = -1;
            event.stopImmediatePropagation();
            event.preventDefault();

            $.ajax({
                type: "POST",
                url: "DevicesPolicy.aspx/GetGroupDeviceData",
                dataType: "json",
                data: "{id:\"" + id + "\"}",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {

                    $("#divModalDialog").html('');
                    $("#divModalDialog").dialog("destroy");
                    var d = $("#divModalDialog");
                    d.html(msg);
                    var dOpt = {
                        title: name,
                        width: 1000,
                        modal: true,
                        resizable: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    };
                    d.dialog(dOpt);
                    d.find("button").button();
                },
                error: function (msg) { ShowJSONMessage(msg); }
            });
        }
        );
    };
    /* загрузка начальных данных */
    function listRootLoad() {
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/GetListRootGroup",
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
            url: "DevicesPolicy.aspx/GetAllDevices",
            dataType: "json",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                $("#tab2").html(msg);
            },
            error: function (msg) { ShowJSONMessage(msg); }
        });
    }

    /*  $(document).on("click", '[deldev]', function () {

    if (!confirm('Are you sure?'))
    return;
    var id = $(this).attr('deldev');
    $.ajax({
    type: "POST",
    url: "DevicesPolicy.aspx/ DeleteDeviceFromPanel",
    data: "{id:" + id + "}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
    alert(msg);
    $("[deldev=" + id + "]").parent().parent().remove();
    ShowRefreshMessage();
    }
    })
    });*/

    $(document).on("click", "img[treedeldp]", function () {
        var id = $(this).attr('treedeldp');
        var devID = $(this).attr('treedeldev');
        var del = $(this);
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/RemoveDevicePolicy",
            data: "{id:" + id + "}",
            contentType: "application/json; charset=utf-8",
            success: function () {

                while (del.children().length == 0 || del.children() == null) {
                    var tbody = del.parent().parent().parent();

                    if (!del.parent().parent().hasClass("ui-dialog")) {
                        del.parent().parent().remove();
                    }
                    var childCount = tbody.children().length;
                    if (childCount == 0) {
                        var devID = tbody.parent().attr("treetabledevID");
                        del = tbody.parent().parent().parent();
                        $("[treetabledevID=" + devID + "]").remove();
                    } else break;
                }

            },
            error: function (msg) {
                ShowJSONMessage(msg);
            }
        });
        return false;
    });

    $(document).on("click", 'input[comdp]', function () {
        var id = $(this).attr('comdp');
        var serial = $(this).attr('serialdp');
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/GetChangeCommentDialog",
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
                    modal: true,
                    resizable: false
                };
                d.dialog(dOpt);
                d.find("button").button();
            },
            error: function (msg) {
                alert(msg);
            }
        })
    });
    $(document).on("click", 'a[deviceID]', function () {

        var id = $(this).attr('deviceID');
        var serial = $(this).html();

        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/GetDeviceTreeDialog",
            data: "{id:" + id + ",serial:'" + serial + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var d = $('#deviceTreeDialog');
                d.html(msg);
                var dOpt = {
                    title: serial,
                    width: '700px',
                    modal: true,
                    resizable: false
                };
                d.dialog(dOpt);
                d.find("button").button();
                $(this).find('table').attr('class', 'ListContrastTable');
                $('[treeacc]').accordion({ collapsible: true, heightStyle: "content", autoHeight: false, active: false });


                /* удаление группы компьютеров*/

                var header = $(d).find("h3");
                header.append('<img class="deleteimg" style="position:absolute; right: 5px; " src="App_Themes/Main/Images/deleteicon.png"/>');
                var deleteimg = $(header).find("[class='deleteimg']");

                deleteimg.on("click", function (event) {

                    var url, data;
                    var devId = $(this).parent().attr('treetableID');
                    var groupId = $(this).parent().attr('treetabledevID');
                    if (groupId == -1) {
                        url = "DevicesPolicy.aspx/RemoveDevicePolicyWithoutGroup";
                        data = "{id:" + devId + "}";
                    }
                    else {
                        url = "DevicesPolicy.aspx/RemoveDevicePolicyGroup";
                        data = "{devid:" + devId + ",groupid:" + groupId + "}";
                    }
                    event.stopImmediatePropagation();
                    event.preventDefault();
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: data,
                        contentType: "application/json; charset=utf-8",
                        success: function () {
                            var del = $("h3[treetabledevID=" + groupId + "]").parent();
                            $("h3[treetabledevID=" + groupId + "]").remove();
                            $("div[treetabledevID=" + groupId + "]").remove();
                            while (del.children().length == 0 || del.children() == null) {
                                var tbody = del.parent().parent().parent();
                                if (!del.parent().parent().hasClass("ui-dialog")) {
                                    del.parent().parent().remove();
                                }
                                var childCount = tbody.children().length;
                                if (childCount == 0) {
                                    var devID = tbody.parent().attr("treetabledevID");
                                    del = tbody.parent().parent().parent();
                                    $("[treetabledevID=" + devID + "]").remove();
                                } else break;
                            }
                        },
                        error: function (msg) {
                            ShowJSONMessage(msg);
                        }
                    });
                    return false;
                });
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });
    $(document).on("click", 'button[addcompdev]', function () {
        var serial = $(this).attr("addcompdev");


        ComputersDialog.show(serial);
        return false;
    });
    //-------------Unknown Policy---
    $(document).on("click", 'input[acp]', function () {
        var id = $(this).attr('acp');
        var action = $(this).attr('action');
        var comp = $('[devcompdp=' + id + ']').html();

        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/ActionDevice",
            data: "{id:" + id + " ,computerName:'" + comp + "' ,action:'" + action + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function () {

                $("input[UpdatePanelButton]").click();
            },
            error: function (msg) {
                ShowJSONMessage(msg);
            }
        })
    });

    $(document).on("click", 'input[acpAll]', function () {

        var action = $(this).attr('action');
        var devpolicies = "";
        var computerNames = "";
        $("[dcp]").each(function () {
            devpolicies += $(this).attr("dcp") + ";";
            computerNames += $('[devcompdp=' + $(this).attr("dcp") + ']').html() + ";";
        });
        
         $.ajax({
        type: "POST",
        url: "DevicesPolicy.aspx/ActionForAllDevices",
        data: "{action:'" + action + "',devpolicies:'" + devpolicies + "',computerNames:'"+computerNames+"'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {

        $("input[UpdatePanelButton]").click();
        },
        error: function (msg) {
            ShowJSONMessage(msg);
        }
        })
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
});
