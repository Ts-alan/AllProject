<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Controls_PrimitiveFilterComputers" Codebehind="PrimitiveFilterComputers.ascx.cs" %>
<%@ Register Src="~/Controls/PrimitiveFilterTemplate.ascx" TagName="PrimitiveFilterTemplate"
    TagPrefix="flt" %>
    <script language="javascript" type="text/javascript">
        var ComputersTextBox = function () {
            RegExp.escape = function (text) {
                return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
            }
            return {
                getRegex: function () {
                    tboxComputer = $('#<%= tboxFilter.ClientID %>');
                    str = tboxComputer.val();
                    if (str == "") return null;
                    strEsc = RegExp.escape(str);
                    strQuantifier = strEsc.replace(/\\\*/g, '.*');
                    splited = new Array();
                    splited = strQuantifier.split('&');
                    for (i = 0; i < splited.length; i++) {
                        splited[i] = '^' + splited[i] + '$';
                    }
                    regex = splited.join('|');
                    return regex;
                },

                setText: function (text) {
                    $('#<%= tboxFilter.ClientID %>').val(text); ;
                }
            };
        } ();




        function pageLoad () {

            $("input[type=button]").button();

            $.jstree.defaults.checkbox.whole_node = false;
            $.jstree.defaults.checkbox.tie_selection = false;
            $('#compFilterTree').jstree({
                'core': {
                    'check_callback': true,
                    'multiple': false,
                    'data': function (node, cb) {
                        cb(loadCompFilterTreeInfo());
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
                'plugins': ["checkbox","state", "types"]

            });
        };


        function loadCompFilterTreeInfo() {
            var d = "";

            $.ajax({
                type: "GET",
                async: false,
                url: '<%=HandlerUrl%>',
                dataType: "json",
                data: {},
                success: function (data) {
                    d = data;
                },
                error: function (e) {
                    //alert(e);
                    alert('<%= Resources.Resource.ErrorRequestingDataFromServer%>');
                }
            });
            return d;
        };
        
        function ShowComputerTreeDialog() {

            var dOpt = {
                width: 550,
                modal: true,
                resizable: false,
                close: function (event, ui) {
                },
                buttons: {
                    '<%=Resources.Resource.Apply%>': function () {
                        ComputersTextBox.setText(GenerateCompFilterText());
                        $('#compFilterTreeDialog').dialog('destroy');

                    },
                    '<%=Resources.Resource.CancelButtonText%>': function () {
                        $('#compFilterTreeDialog').dialog('destroy');
                    }
                }
            };
            SetCheckedAccordingToText();
            $('#compFilterTreeDialog').dialog(dOpt);

            return false;
        };

        function GenerateCompFilterText() {
            computers = new Array();        
            var node;
            var checkedObj = $('#compFilterTree').jstree('get_checked', true);
            for (i = 0; i < checkedObj.length; i++) {
                if (checkedObj[i].state != null && checkedObj[i].state.checked == true) {
                    node = checkedObj[i].original;
                    if (node != null && node.type=="computer") {
                        computers.push(node.text.toLowerCase());
                    }
                }
            }
            
            return computers.join('&');
        };
        function SetCheckedAccordingToText() {
            gen = ComputersTextBox.getRegex();
            if (gen == null) return;
            regex = new RegExp(gen, "i");
            var root = $('#compFilterTree').jstree('get_node', '#',false);
            $.each(root.children_d, function (nodeId) {
                var node = $('#compFilterTree').jstree('get_node', nodeId);
                node = node.original;
                if (node != null && node.type == "computer") {
                    if (regex.test(node.text)) {
                        $('#compFilterTree').jstree('check_node', nodeId);
                    }
                    else {
                        $('#compFilterTree').jstree('uncheck_node', nodeId);
                    }
                }
            });
        };
                
    </script>
<flt:PrimitiveFilterTemplate runat="server" ID="fltTemplate" TextFilter="Name">
    <FilterTemplate>
        <div style="padding: 5px; padding-top: 3px;">
            <div style="float:left;">
                <asp:TextBox runat="server" ID="tboxFilter" Style="width: 150px; height: 17px;"></asp:TextBox>            
                <ajaxToolkit:FilteredTextBoxExtender ID="fltComputers" runat="server" TargetControlID="tboxFilter"
                    FilterType="Custom" InvalidChars="`~!@#$%^()=+[]{}\|;:'&quot;,<>/?" FilterMode="InvalidChars">
                </ajaxToolkit:FilteredTextBoxExtender>
            </div>            
            <div runat="server" class="ImageComputer" style="float:left; margin-left: 5px;" id="imgHelper" onclick="return ShowComputerTreeDialog();"></div>
            <div id="compFilterTreeDialog" style="display:none">
                <div id="compFilterTree">
                </div>
            </div>
        </div>
    </FilterTemplate>
</flt:PrimitiveFilterTemplate>
