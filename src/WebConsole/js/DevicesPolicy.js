$(document).ready(function() {
    $("#tabs").tabs({ cookie: { expires: 30 } });
//-----------Devices--------------
    $("button[dpc]").live("click", function() {
        var id = $(this).attr('dpc');
        var serial = $('input[dpc]').val();
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/AddNewDevicePolicy",
            data: "{id:" + id + ", serial:'" + serial + "'}",
            contentType: "application/json; charset=utf-8",
            success: function() {
                $("div[cp="+id+"]").click();
                ShowRefreshMessage();
            },
            error: function() {
                //alert('fail');
            }
        });
        return false;
    });
    $("img[dp]").live("click", function() {
        var id = $(this).attr('dp');
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/RemoveDevicePolicy",
            data: "{id:" + id + "}",
            contentType: "application/json; charset=utf-8",
            success: function() {
                $("img[dp=" + id + "]").parent().parent().remove();
            },
            error:function(msg)
            {
                ShowJSONMessage(msg);
            }
        });
    });
    $("select[dp]").live("click", function() {
        //alert('run');
        var id = $(this).attr('dp');
        var state = $(this).attr('value');
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/ChangeDevicePolicyState",
            data: "{id:" + id + ",state:'" + state + "'}",
            contentType: "application/json; charset=utf-8",
            success: function() {
                //alert(state);
            }
        });
    });
    $("div[cp]").click(function() {
        var id = $(this).attr('cp');
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/GetComputersData",
            data: "{id:" + id + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(msg) {               
                $('div[cpc]').html('');  
                $('div[cpc]').dialog('destroy');  
                var d = $('div[cpc=' + id + ']');
                d.html(msg);
                var dOpt= {
                    title : d.attr('ncp'),
                    width : '800px',
                    resizable: false
                };
                d.dialog(dOpt);
                
            }
        })
    });
    $("img[cpp]").click(function() {
        var id = $(this).attr('cpp');
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/ApplyPolicy",
            data: "{id:" + id + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(msg) {                
                $('div[cpcp]').html('');  
                $('div[cpcp]').dialog('destroy');  
                var d = $('div[cpcp=' + id + ']');
                d.html(msg);
                var dOpt= {
                    title : d.attr('ncp'),
                    width : '200px',
                    resizable: false                    
                };
                d.dialog(dOpt);
            }
        })
    });
    $("div[cp]").hover(function() {
        var id = $(this).attr('cp');
        $('div[cp=' + id + ']').css('background-color', 'yellow');
    },
    function() {
        var id = $(this).attr('cp');
        $('div[cp=' + id + ']').css('background-color', '');
    });
//------------------Computers----------
    $("button[ddpc]").live("click", function() {
        var id = $(this).attr('ddpc');
        var name = $('input[ddpc]').val();
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/AddNewComputerPolicy",
            data: "{id:" + id + ", name:'" + name + "'}",
            contentType: "application/json; charset=utf-8",
            success: function() {
                $("td[dcp="+id+"] > img[addcomp=true]").click();
            },
            error: function(msg) {
                ShowJSONMessage(msg);
            }
        });
        return false;
    });
    $("[addcomp=true]").click(function() {
        /*var no = $(this).attr('no');
        if(no) return false;*/
        
        var id = $(this).attr('dcp');
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/GetDevicesData",
            data: "{id:" + id + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(msg) {
                $('div[dcpc]').html('');  
                $('div[dcpc]').dialog('destroy');  
                var d = $('div[dcpc=' + id + ']');
                d.html(msg);
                //Cut title, if it has big length
                var dTitle = d.attr('dncp');
                if(d.attr('dncp').length > 45) dTitle = dTitle.substr(0, 45) + "..";
                
                var dOpt= {
                    title : dTitle,
                    width : '600px',
                    resizable: false
                };
                d.dialog(dOpt);
            }
        })
    });
     $("[delete=true]").click(function() {
        if(!confirm('Are you sure?'))
            return;
        var id = $(this).attr('dcp');
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/DeleteDevice",
            data: "{id:" + id + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function() {
                $('[dcp='+id+']').remove();  
                ShowRefreshMessage();               
            }
        })
    });
    
    $('[comment=true]').click(function(){
        var id = $(this).attr('dcp');
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/GetChangeCommentDialog",
            data: "{id:" + id + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(msg) {
                $('div[dcpc]').html('');  
                $('div[dcpc]').dialog('destroy');  
                var d = $('div[dcpc=' + id + ']');
                d.html(msg);
                var dOpt= {
                    title : d.attr('dncp'),
                    width : '666px',
                    resizable: false
                };
                d.dialog(dOpt);
            }
        })
    });
    $("button[dcdpc]").live("click", function() {
        var id = $(this).attr('dcdpc');
        var comment = $('input[dcdpc]').val();
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/ChangeComment",
            data: "{id:" + id + ", comment:'" + comment + "'}",
            contentType: "application/json; charset=utf-8",
            success: function() {
                $("td[dcp="+id+"][comment=false]").html(comment);
                $('div[dcpc=' + id + ']').dialog('destroy');
            },
            error: function(msg) {
                ShowJSONMessage(msg);
            }
        });
        return false;
    });
//-------------Unknown Policy---
 $("img[acp]").click(function() {
        var id = $(this).attr('acp');
        var action = $(this).attr('action');
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/ActionDevice",
            data: "{id:" + id + ", action:'" +action+ "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function() {
                $('[dcp='+id+']').remove();
            },
            error:function(msg)
            {
                ShowJSONMessage(msg);
            }
        })
    });
    
    $("img[acpAll]").click(function() {        
        var action = $(this).attr('action');
        $.ajax({
            type: "POST",
            url: "DevicesPolicy.aspx/ActionDeviceAll",
            data: "{action:'" +action+ "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function() {
                $('[rcp]').remove();
            },
            error:function(msg)
            {
                ShowJSONMessage(msg);
            }
        })
    });
    
    $("[dcp]").hover(function() {
        var id = $(this).attr('dcp');
        $('[dcp=' + id + ']').css('background-color', 'yellow');
    },
    function() {
        var id = $(this).attr('dcp');
        $('[dcp=' + id + ']').css('background-color', '');
    });
    
    function ShowJSONMessage(msg)
    {
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
    
    function ShowRefreshMessage()
    {
        $('.helpMessage').css('visibility','visible');
    }
    
});