$(document).ready(function () {
    /* загрузка начальных данных */
    function listRootLoad() {
        $.ajax({
            type: "POST",
            url: "DeviceClass.aspx/GetListRootGroup",
            dataType: "json",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                $("#accordion").append(msg).accordion({
                    beforeActivate: function (event, ui) { acc_before_activate(event, ui); },
                    activate: function (event, ui) { acc_activate(event, ui); },
                    collapsible: true,
                    heightStyle: "content",
                    autoHeight: false,
                    active: false
                });
                HeaderFunction("#accordion");
            },
            error: function (msg) { alert("Error"); ShowJSONMessage(msg); }
        });
    };

    function acc_activate(event, ui) {
        var acc = $(ui.newHeader).attr('acc');
        if (acc == "false") {
            var str = "#accordion" + $(ui.newHeader).attr('id');
            HeaderFunction(ui.newPanel);
            $(str).accordion({
                beforeActivate: function (event, ui) { acc_before_activate(event, ui); },
                activate: function (event, ui) { acc_activate(event, ui); },
                collapsible: true,
                heightStyle: "content",
                autoHeight: false,
                active: false
            });
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
                url: "DeviceClass.aspx/GetComputersData",
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
                        width: 700,
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
                url: "DeviceClass.aspx/GetData",
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

    /* запрет на раскрытие вкладок и загрузка данных об устройствах */
    function HeaderFunction(content) {
        var header = $(content).find("h3");
        header.append('<button class="stop" style="position:absolute; right: 5px; bottom:1px; top: 1px ">' + Resource.DeviceClass + '</button>');
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
                url: "DeviceClass.aspx/GetGroupDeviceData",
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
                    d.find("button").button();
                },
                error: function (msg) { ShowJSONMessage(msg); }
            });
        }
        );
    };


    $("#divModalDialog").dialog({ autoOpen: false });
    $("#divAddClassDialog").dialog({ autoOpen: false });

    $(function () {
        listRootLoad();

        /* изменение комментария (вызов диалогового окна)*/
        $(document).on("click", 'img[ccUID]', function () {
            var uid = $(this).attr('ccUID');
            var comment = $(this).attr('comment');
            $.ajax({
                type: "POST",
                url: "DeviceClass.aspx/GetChangeCommentDialog",
                data: "{uid:'" + uid + "',comment:'" + comment + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $("#commentDialog").html('');
                    var d = $('#commentDialog');
                    d.html(msg);
                    var dOpt = {
                        title: uid,
                        width: '700px',
                        modal: true,
                        resizable: false
                    };
                    d.dialog(dOpt);
                    d.find("button").button();
                },
                error: function (msg) {
                    ShowJSONMessage(msg);
                }
            })
        });

        /* изменение комментария */
        $(document).on("click", "button[ccuid]", function () {
            var uid = $(this).attr('ccuid');
            var comment = $('input[ccuid]').val();
            $.ajax({
                type: "POST",
                url: "DeviceClass.aspx/ChangeComment",
                data: "{uid:'" + uid + "', comment:'" + comment + "'}",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    $("td[uid='" + uid + "'][type='comment']").html(comment);
                    $("span[uid='" + uid + "'][type='comment']").html(comment);
                    $("img[ccUID]").attr('comment', comment);
                    $("#commentDialog").dialog('close');
                },
                error: function (msg) {
                    ShowJSONMessage(msg);
                }
            });
            return false;
        });

        /*удаление deviceClassPolicy для компьютера */
        $(document).on("click", "img[deldp]", function () {
            var devId = $(this).attr('deldp');
            var compId = $(this).attr('compId');

            $.ajax({
                type: "POST",
                url: "DeviceClass.aspx/RemoveDeviceClassPolicy",
                data: "{devId:" + devId + ",compId:" + compId + "}",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    $("img[deldp=" + devId + "]").parent().parent().remove();
                },
                error: function (msg) {
                    ShowJSONMessage(msg);
                }
            });
            return false;
        });

        /* change deviceClassPolicy state for computer*/
        $(document).on("click", "img[cp]", function () {
            var dp = $(this).attr('dp');
            var cp = $(this).attr('cp');
            var state = $(this).attr('state');
            //установка смены чекбоксов
            changeState($(this), dp, cp, state);
        });

        function changeState(img, d, cp, state) {
            var titleMsg;
            switch (state) {
                case "Undefined":
                    titleMsg = Resource.Disabled;
                    state = "Disabled";
                    break;
                case "Enabled":
                    titleMsg = Resource.Disabled;
                    state = "Disabled";
                    break;
                case "Disabled":
                    if (img.attr('IsUsbClass') == 'true') {
                        titleMsg = Resource.Enabled;
                        state = "Enabled";
                    }
                    else {
                        titleMsg = Resource.BlockWrite;
                        state = "BlockWrite";
                    }
                    break;
                case "BlockWrite":
                    titleMsg = Resource.Enabled;
                    state = "Enabled";
                    break;
            }

            $.ajax({
                type: "POST",
                url: "DeviceClass.aspx/ChangeDevicePolicyStateComputer",
                data: "{devId:" + d + ",compId:" + cp + ",state:'" + state + "'}",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    img.attr('state', state);
                    img.attr('title', titleMsg);
                    img.attr('src', "App_Themes/Main/Images/" + state + ".gif");
                }
            });
        };

        /* change devicePolicy state for computer*/
        $(document).on("click", "img[treestatecp]", function () {
            var d = $(this).attr('treestatedev');
            var cp = $(this).attr('treestatecp');
            var state = $(this).attr('state');
            //установка смены чекбоксов
            changeState($(this), d, cp, state);
        });

        /* add new policy to computer */
        $(document).on("click", "button[dpc]", function () {
            var id = $(this).attr('dpc');
            var uid = $('input[dpc]').val();

            if (!ValidateEmpty(uid))
                return false;

            var isExists = false;
            $("td[uidtd]").each(function () {
                var curUID = $(this).html();
                if (curUID == uid) {
                    isExists = true;
                    return false;
                }
            });

            if (isExists) {
                alert(Resource.ErrorExistPolicy);
                $('input[dpc]').val("");
                return false;
            }

            $.ajax({
                type: "POST",
                url: "DeviceClass.aspx/AddNewDeviceClassPolicyToComputer",
                data: "{id:" + id + ", uid:'" + uid + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    var deviceMsg = JSON.parse(msg);
                    var success = deviceMsg.success;
                    if (success == "true") {
                        var row = "<td>" + deviceMsg.className + "</td>";
                        row += "<td style='width:60px' uidtd>" + deviceMsg.uid + "</td>"
                        row += "<td><span class='main' type='comment' uid='" + deviceMsg.uid + "'>" + deviceMsg.comment + "</span></td>";
                        row += "<td><img src='App_Themes/Main/Images/disabled.gif' state='Disabled' cp='" + id + "' dp='" + deviceMsg.id + "' style='cursor:pointer'></td>";
                        row += "<td>";
                        row += "<img src='App_Themes/Main/Images/table_edit.png' comment='" + deviceMsg.comment + "' ccuid='" + deviceMsg.uid + "' title='" + Resource.ChangeComment + "' style='cursor:pointer'>";
                        row += "<img src='App_Themes/Main/Images/deleteicon.png' compid='" + id + "' deldp='" + deviceMsg.id + "' title='" + Resource.Delete + "' style='cursor:pointer'>";
                        row += "</td>";

                        var cssClass = ($("table[cp=" + id + "] tr:last").attr("class") == "gridViewRowAlternating") ? "gridViewRow" : "gridViewRowAlternating";
                        $("table[cp=" + id + "] > tbody:last").append("<tr class='" + cssClass + "' style='text-align:center'></tr>");
                        var lastLine = $("table[cp=" + id + "] tr:last");
                        lastLine.html(row);
                        $('input[dpc]').val("");
                    }
                    else {
                        var message = "";
                        if (deviceMsg.info) {
                            message = " (" + deviceMsg.info + ")";
                        }
                        alert(Resource.Error + message);
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
            var uid = $('input[dgr]').val();

            if (!ValidateEmpty(uid))
                return false;

            $.ajax({
                type: "POST",
                url: "DeviceClass.aspx/AddNewDeviceClassPolicyGroup",
                data: "{id:" + id + ", uid:'" + uid + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    var deviceMsg = JSON.parse(msg);
                    var success = deviceMsg.success;
                    if (success == "true") {
                        if (deviceMsg.count == "0") {
                            alert("No objects.");
                            $('input[dgr]').val("");
                            return;
                        }
                        var img = $('img[dp=' + deviceMsg.id + ']');
                        if (img.length != 0) {
                            img.attr('src', "App_Themes/Main/Images/undefined.gif");
                            $('img[nfadp=' + deviceMsg.id + ']').remove();
                        }
                        else {
                            var row = "<td></td><td>" + deviceMsg.className + "</td>";
                            row += "<td style='width:60px' uidtd>" + deviceMsg.uid + "</td>"
                            row += "<td><span class='main' type='comment' uid='" + deviceMsg.uid + "'>" + deviceMsg.comment + "</span></td>";
                            row += "<td><img src='App_Themes/Main/Images/disabled.gif' state='Disabled' gdp='" + id + "' dp='" + deviceMsg.id + "' style='cursor:pointer'></td>";
                            row += "<td>";
                            row += "<img src='App_Themes/Main/Images/table_edit.png' comment='" + deviceMsg.comment + "' ccUID='" + deviceMsg.uid + "' title='" + Resource.ChangeComment + "' style='cursor:pointer'>";
                            if (id > 0)
                                row += "<img style='cursor:pointer' title='" + Resource.Delete + "' delgroupid=" + id + " delgroupdevid=" + deviceMsg.id + " src=\'App_Themes/Main/Images/deleteicon.png\' />";
                            else
                                row += "<img style='cursor:pointer' title='" + Resource.Delete + "' delwithoutgroupdevid=" + deviceMsg.id + " src=\'App_Themes/Main/Images/deleteicon.png\' />";
                            row += "</td>";

                            var cssClass = ($("table[gdp=" + id + "] tr:last").attr("class") == "gridViewRowAlternating") ? "gridViewRow" : "gridViewRowAlternating";
                            $("table[gdp=" + id + "] > tbody:last").append("<tr class='" + cssClass + "' style='text-align:center'></tr>");
                            var lastLine = $("table[gdp=" + id + "] tr:last");
                            lastLine.html(row);
                            $('input[dgr]').val("");
                        }
                    }
                    else {
                        var message = "";
                        if (deviceMsg.info) {
                            message = " (" + deviceMsg.info + ")";
                        }
                        alert(Resource.Error + message);
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
            var img = $(this);
            var titleMsg;
            switch (state) {
                case "Undefined":
                    titleMsg = Resource.Disabled;
                    state = "Disabled";
                    break;
                case "Enabled":
                    titleMsg = Resource.Disabled;
                    state = "Disabled";
                    break;
                case "Disabled":
                    if (img.attr('IsUsbClass') == 'true') {
                        titleMsg = Resource.Enabled;
                        state = "Enabled";
                    }
                    else {
                        titleMsg = Resource.BlockWrite;
                        state = "BlockWrite";
                    }
                    break;
                case "BlockWrite":
                    titleMsg = Resource.Enabled;
                    state = "Enabled";
                    break;
            }

            $.ajax({
                type: "POST",
                url: "DeviceClass.aspx/ChangeDeviceClassPolicyStateGroup",
                data: "{dp:" + dp + " ,gp:" + gp + ",state:'" + state + "'}",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    img.attr('state', state);
                    img.attr('title', titleMsg);
                    img.attr('src', "App_Themes/Main/Images/" + state + ".gif");
                }
            });
        });

        /*удаление device для группы */
        $(document).on("click", "img[delgroupdevid]", function () {
            var devId = $(this).attr('delgroupdevid');
            var groupId = $(this).attr('delgroupid');
            $.ajax({
                type: "POST",
                url: "DeviceClass.aspx/RemoveDeviceClassPolicyGroup",
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
                url: "DeviceClass.aspx/RemoveDeviceClassPolicyWithoutGroup",
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

    });


    /*---------Вкладка Devices---------------------*/
    $(document).on("click", "img[treedelcp]", function () {
        var compID = $(this).attr('treedelcp');
        var devID = $(this).attr('treedeldev');
        var del = $(this);
        $.ajax({
            type: "POST",
            url: "DeviceClass.aspx/RemoveDeviceClassPolicy",
            data: "{devId:" + devID + ",compId:" + compID + "}",
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
                    }
                    else
                        break;
                }
            },
            error: function (msg) {
                ShowJSONMessage(msg);
            }
        });
        return false;
    });

    $(document).on("click", 'a[deviceID]', function () {
        var id = $(this).attr('deviceID');
        var uid = $(this).attr('deviceUID');
        var text = $(this).html();
        $.ajax({
            type: "POST",
            url: "DeviceClass.aspx/GetDeviceTreeDialog",
            data: "{id:" + id + ",uid:'" + uid + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var d = $('#deviceTreeDialog');
                d.html(msg);
                var dOpt = {
                    title: text,
                    width: '700px',
                    modal: true,
                    resizable: false
                };
                d.dialog(dOpt);
                d.find("button").button();
                $(this).find('table').attr('class', 'ListContrastTable');
                $('[treeacc]').accordion({
                    collapsible: true,
                    heightStyle: "content",
                    autoHeight: false,
                    active: false
                });

                /* удаление группы компьютеров*/
                var header = $(d).find("h3");
                header.append('<img class="deleteimg" style="position:absolute; right: 5px; " src="App_Themes/Main/Images/deleteicon.png"/>');
                var deleteimg = $(header).find("[class='deleteimg']");

                deleteimg.on("click", function (event) {
                    var url, data;
                    var devId = $(this).parent().attr('treetableID');
                    var groupId = $(this).parent().attr('treetabledevID');
                    if (groupId == -1) {
                        url = "DeviceClass.aspx/RemoveDeviceClassPolicyWithoutGroup";
                        data = "{id:" + devId + "}";
                    }
                    else {
                        url = "DeviceClass.aspx/RemoveDeviceClassPolicyGroup";
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
                                }
                                else
                                    break;
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
        var classId = $(this).attr("addcompdev");
        ComputersDialogDeviceClass.show(classId);
        return false;
    });

    $(document).on("click", 'a[id=btnAddDeviceClass]', function () {
        var updateBtn = $(this).attr('updateID');
        $('#tboxNewUID').val("");
        $('#tboxNewClassName').val("");
        $('#tboxNewComment').val("");
        $("#divAddClassDialog").dialog('destroy');
        var d = $("#divAddClassDialog");
        var dOpt = {
            title: Resource.AddNewDeviceClass,
            width: 500,
            modal: true,
            resizable: true,
            buttons: {
                Ok: function () {
                    var uid = $('#tboxNewUID').val();
                    if (!ValidateUID(uid))
                        return;
                    var className = $('#tboxNewClassName').val();
                    if (!ValidateEmpty(className, Resource.ClassName))
                        return;
                    var comment = $('#tboxNewComment').val();
                    $.ajax({
                        type: "POST",
                        url: "DeviceClass.aspx/AddDeviceClass",
                        data: "{uid:'" + uid + "',className:'" + className + "',comment:'" + comment + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            if (updateBtn != null) {
                                __doPostBack(updateBtn, '');
                            }
                        },
                        error: function (msg) {
                            ShowJSONMessage(msg);
                        }
                    });

                    $(this).dialog("close");
                }
            }
        };
        d.dialog(dOpt);
        return false;
    });

    function ValidateUID(uid) {
        if (!ValidateEmpty(uid, "UID"))
            return false;

        var reGuid = /^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$/
        if (!reGuid.test(uid)) {
            alert(Resource.GuidRegexErrorMessage + "!");
            return false;
        }

        return true;
    }

    function ValidateEmpty(str, fieldName) {
        if (str == null || str.replace(" ", "") == "") {
            alert("'" + fieldName + "' " + Resource.ErrorFieldIsEmpty + "!");
            return false;
        }
        return true;
    }

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
