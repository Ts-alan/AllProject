function ComputerTreeDeviceInit() {
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

function ComputerTreeDeviceReload() {
    $('#dialog_div').jstree('refresh');
};

function ComputerTreeDeviceGenerateText() {
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

var ComputersDialogDevice = function () {
    var dialog;
    var asyncPostBack = false;

    function onApply(serial) {

        comps = ComputerTreeDeviceGenerateText();
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/AddNewDevicePolicyByComputerList",
            dataType: "json",

            data: "{serial:'" + serial + "',comps:'" + comps + "'}",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {

                if (msg == true) {
                    $("a:contains('" + serial + "')").trigger("click");
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
        show: function (serial) {

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
                    buttons: { Apply: function () { onApply(serial); dialog.dialog("close"); } }
                });                        
                ComputerTreeDeviceInit();
                asyncPostBack = false;
            }
            else {
                ComputerTreeDeviceReload();
            }
        }
    };
} ();