﻿function ComputerTreeDeviceClassInit() {
    $.jstree.defaults.checkbox.whole_node = false;
    $.jstree.defaults.checkbox.tie_selection = false;
    $('#dialog_div').jstree({
        'core': {
            'check_callback': true,
            'multiple': false,
            'data': function (node, cb) {
                cb(loadTreeInfo());
            }
        },
        'types': {
            'group': {
                'icon': "App_Themes/Main/groups/images/group.png"
            },
            'computer': {
                'icon': "App_Themes/Main/groups/images/monitor.png"
            },
            'server': {
                'icon': "App_Themes/Main/groups/images/server.png"
            },
            'default': {
            }
        },
        'plugins': ["checkbox", "types"]
    });
};

function ComputerTreeDeviceClassReload() {
    $('#dialog_div').jstree('refresh');
};

function ComputerTreeDeviceClassGenerateText() {
    computers = new Array();
    var node;
    var checkedObj = $('#dialog_div').jstree('get_checked', true);

    for (i = 0; i < checkedObj.length; i++) {
        if (checkedObj[i].state != null && checkedObj[i].state.checked == true) {
            node = checkedObj[i].original;
            if (node != null && node.type=="computer") {
                computers.push(node.id.toLowerCase());
            }
        }
    }
    return computers.join('&');
};

function loadTreeInfo() {
    var d = "";
    $.ajax({
        type: "GET",
        async: false,
        url: "Handlers/CheckedComputerTreeHandler.ashx",
        dataType: "json",
        data: {},
        success: function (data) {
            d = data;
        },
        error: function (e) {
            dialog.dialog('close');
            console.log(e);
            alert(Resource.ErrorRequestingDataFromServer);
        }
    });
    return d;
}; 


var ComputersDialogDeviceClass = function () {
    var dialog;
    var asyncPostBack = false;

    function onApply(classId) {
        comps = ComputerTreeDeviceClassGenerateText();
        $.ajax({
            type: "POST",
            url: "DeviceClass.aspx/AddNewDeviceClassPolicyByComputerList",
            dataType: "json",
            data: "{id:" + classId + ",comps:'" + comps + "'}",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                if (msg.d == true) {
                    $("a[deviceId=" + classId + "]").trigger("click");
                }
                else alert(Resource.NothingIsAdded);
            },
            error: function (msg) { alert(msg.responseText) }
        });
    }

    function onHide() {
        PageRequestManagerHelper.enableAsyncPostBack();
    }

    function onShow() {
        PageRequestManagerHelper.abortAsyncPostBack();
        PageRequestManagerHelper.disableAsyncPostBack();
    }

    return {
        setAsyncPostBack: function () {
            asyncPostBack = true;
        },
        show: function (classId) {
            var Apply = Resource.Apply.toString();
            if (!dialog || asyncPostBack) {
                $('body').append('<div id="dialog_div"></div>');
                dialog = $('#dialog_div').dialog({
                    title: Resource.Computers,
                    width: 500,
                    height: 300,
                    modal: true,
                    draggable: false,
                    close: function () {
                        onHide();
                        $(this).dialog("destroy");
                        dialog = false;
                        $('#dialog_div').remove();
                    },
                    open: onShow,
                    buttons: { Apply: function () {
                        onApply(classId);
                        dialog.dialog("close");
                    }
                    }
                });
                ComputerTreeDeviceClassInit(dialog);
                asyncPostBack = false;
            }
            else {
                ComputerTreeDeviceClassReload();
            }
        }
    };
} ();