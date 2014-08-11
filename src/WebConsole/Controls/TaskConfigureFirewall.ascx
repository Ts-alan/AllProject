<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskConfigureFirewall.ascx.cs" Inherits="Controls_TaskConfigureFirewall" %>

<div class="tasksection" runat="server" id="HeaderName" style="width:560px"><%=Resources.Resource.TaskNameConfigureFirewall%></div>
<div class="divSettings">

    <script type="text/javascript">
        $(document).ready(function () {
            $("#Tabs").tabs({ cookie: { expires: 30} });
        });
        function pageLoad() 
        {
            $("input[class='control']").button();
            //IP4
            $('#<%= IP4AddTcpUdpRuleButton.ClientID %>').on("click", function () {
                var dOpt = {
                    width: 350,   
                    resizable: false,
                    close: function(event, ui)
                        {
                            IP4TcpUdpDialogSetDefaultValues();  
                            $('#divOverlay').css('display','none');
                        },
                    buttons: {
                        '<%=Resources.Resource.Apply%>': function () {
                            var btn = '<%=IP4ApplyTcpUdpRuleDialogButton.UniqueID %>';
                            var txt = '#' + '<%=IP4txtLocalIPDialog.ClientID %>';
                            if( Page_ClientValidate('IP4AddressValidation'))
                            {
                                __doPostBack(btn, '');                                                              
                                $('#IP4AddTcpUdpRuleDialog').dialog('close');
                            }
                        },
                       '<%=Resources.Resource.CancelButtonText%>': function () {                           
                            $('#IP4AddTcpUdpRuleDialog').dialog('close');
                            
                        }
                    }
                };
                $('#IP4AddTcpUdpRuleDialog').dialog(dOpt);
                $('#divOverlay').css('display','inline');
                $('#IP4AddTcpUdpRuleDialog').parent().appendTo(jQuery("form:first"));
            });
            
             $('#<%= IP4AddRuleButton.ClientID %>').on("click", function () {
                var dOpt = {
                    width: 350,                                       
                    resizable: false,
                    close: function(event, ui)
                        {
                            IP4AddRuleDialogSetDefaultValues();  
                            $('#divOverlay').css('display','none');
                        },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () {
                            var btn = '<%=IP4ApplyRuleDialogButton.UniqueID %>';
                            var txt = '#' + '<%=IP4txtLocalIPDialog.ClientID %>';
                            if( Page_ClientValidate('IP4AddRuleValidationGroup'))
                            {
                                var isChecked=false;
                                if($('#<%=IP4addRuleOtherProtocol.ClientID %>').val()!='') isChecked=true;
                                $("[id*='IP4chkAddRuleProtocol']").each(function(){
                                    if($(this).prop('checked')==true) isChecked=true;
                                });
                                if(isChecked==false)
                                {
                                    alert("<%=Resources.Resource.ProtocolsAreNotSelected%>");
                                }
                                else{
                                    __doPostBack(btn, '');                                                              
                                    $('#IP4AddRuleDialog').dialog('close');
                                }
                            }
                        },
                       <%=Resources.Resource.CancelButtonText%>: function () {                           
                            $('#IP4AddRuleDialog').dialog('close');
                            
                        }
                    }
                };
                $('#IP4AddRuleDialog').dialog(dOpt);
                $('#divOverlay').css('display','inline');
                $('#IP4AddRuleDialog').parent().appendTo(jQuery("form:first"));
             });
              $('#<%= IP4AddRuleProtocolSelectButton.ClientID %>').on("click", function () {
                $("[id*='IP4chkAddRuleProtocol']").prop('checked',true);
                
              });
              $('#<%= IP4AddRuleProtocolDeselectButton.ClientID %>').on("click", function () {
                $("[id*='IP4chkAddRuleProtocol']").prop('checked',false);
                
              });
              $("[IP4tableRowSelected]").hover(function(){
                    if($(this).attr('IP4tableRowSelected')=="true") return;
                    $(this).css('background-color', 'yellow');
                },
                function () { 
                    if($(this).attr('IP4tableRowSelected')=="true") return;           
                    $(this).css('background-color', '');
                }
            );   
            $("[IP4tableRowSelected]").on("click",function(){
                if($(this).attr("IP4tableRowSelected")=="true") return;
                $(this).css('background-color', '#3399ff');
                $("[IP4tableRowSelected=true]").css('background-color', '');
                $("[IP4tableRowSelected=true]").attr('IP4tableRowSelected',false);
                $(this).attr('IP4tableRowSelected',true);
                
                var rowNo=$(this).children().find('[value]').attr('value');                
                 $('#<%=IP4hdnActiveRowNo.ClientID %>').attr('value',rowNo);
             });  
             /**/
            var indexIP4=$('#<%=IP4hdnActiveRowNo.ClientID %>').attr('value');            
            var tableRow=$("[id*='IP4hdnRowNo'][value='"+indexIP4+"']").parent().parent(); 
            tableRow.attr('IP4tableRowSelected',true);    
            $("[IP4tableRowSelected='true']").css('background-color', '#3399ff');
            //IP6
            $('#<%= IP6AddTcpUdpRuleButton.ClientID %>').on("click", function () {
                var dOpt = {
                    width: 350,   
                    resizable: false,
                    close: function(event, ui)
                        {
                            IP6TcpUdpDialogSetDefaultValues();  
                            $('#divOverlay').css('display','none');
                        },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () {
                            var btn = '<%=IP6ApplyTcpUdpRuleDialogButton.UniqueID %>';
                            var txt = '#' + '<%=IP6txtLocalIPDialog.ClientID %>';
                            if( Page_ClientValidate('IP6AddressValidation'))
                            {
                                __doPostBack(btn, '');                                                              
                                $('#IP6AddTcpUdpRuleDialog').dialog('close');
                            }
                        },
                       <%=Resources.Resource.CancelButtonText%>: function () {                           
                            $('#IP6AddTcpUdpRuleDialog').dialog('close');
                            
                        }
                    }
                };
                $('#IP6AddTcpUdpRuleDialog').dialog(dOpt);
                $('#divOverlay').css('display','inline');
                $('#IP6AddTcpUdpRuleDialog').parent().appendTo(jQuery("form:first"));
            });
            
             $('#<%= IP6AddRuleButton.ClientID %>').on("click", function () {
                var dOpt = {
                    width: 350,                                       
                    resizable: false,
                    close: function(event, ui)
                        {
                            IP6AddRuleDialogSetDefaultValues();  
                            $('#divOverlay').css('display','none');
                        },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () {
                            var btn = '<%=IP6ApplyRuleDialogButton.UniqueID %>';
                            var txt = '#' + '<%=IP6txtLocalIPDialog.ClientID %>';
                            if( Page_ClientValidate('IP6AddRuleValidationGroup'))
                            {
                                var isChecked=false;
                                if($('#<%=IP6addRuleOtherProtocol.ClientID %>').val()!='') isChecked=true;
                                $("[id*='IP6chkAddRuleProtocol']").each(function(){
                                    if($(this).prop('checked')==true) isChecked=true;
                                });
                                if(isChecked==false)
                                {
                                    alert("<%=Resources.Resource.ProtocolsAreNotSelected%>");
                                }
                                else{
                                    __doPostBack(btn, '');                                                              
                                    $('#IP6AddRuleDialog').dialog('close');
                                }
                            }
                        },
                       <%=Resources.Resource.CancelButtonText%>: function () {                           
                            $('#IP6AddRuleDialog').dialog('close');
                            
                        }
                    }
                };
                $('#IP6AddRuleDialog').dialog(dOpt);
                $('#divOverlay').css('display','inline');
                $('#IP6AddRuleDialog').parent().appendTo(jQuery("form:first"));
             });
              $('#<%= IP6AddRuleProtocolSelectButton.ClientID %>').on("click", function () {
                $("[id*='IP6chkAddRuleProtocol']").prop('checked',true);
                
              });
              $('#<%= IP6AddRuleProtocolDeselectButton.ClientID %>').on("click", function () {
                $("[id*='IP6chkAddRuleProtocol']").prop('checked',false);
                
              });
              $("[IP6tableRowSelected]").hover(function(){
                    if($(this).attr('IP6tableRowSelected')=="true") return;
                    $(this).css('background-color', 'yellow');
                },
                function () { 
                    if($(this).attr('IP6tableRowSelected')=="true") return;           
                    $(this).css('background-color', '');
                }
            );   
            $("[IP6tableRowSelected]").on("click",function(){
                if($(this).attr("IP6tableRowSelected")=="true") return;
                $(this).css('background-color', '#3399ff');
                $("[IP6tableRowSelected=true]").css('background-color', '');
                $("[IP6tableRowSelected=true]").attr('IP6tableRowSelected',false);
                $(this).attr('IP6tableRowSelected',true);
                
                var rowNo=$(this).children().find('[value]').attr('value');                
                 $('#<%=IP6hdnActiveRowNo.ClientID %>').attr('value',rowNo);
             });  
             /**/
            var indexIP6=$('#<%=IP6hdnActiveRowNo.ClientID %>').attr('value');            
            var tableRow=$("[id*='IP6hdnRowNo'][value='"+indexIP6+"']").parent().parent(); 
            tableRow.attr('IP6tableRowSelected',true);    
              $("[IP6tableRowSelected='true']").css('background-color', '#3399ff');
                                   
        };
           
         function IP4TcpUdpDialogSetDefaultValues()
            {
               $('#' + '<%=IP4txtLocalIPDialog.ClientID %>').val('');
                $('#' + '<%=IP4txtNameDialog.ClientID %>').val('');
                $('#' + '<%=IP4txtLocalPortDialog.ClientID %>').val('');
                $('#' + '<%=IP4txtDestinationIPDialog.ClientID %>').val('');
                $('#' + '<%=IP4txtDestinationPortDialog.ClientID %>').val('');
                $('#' + '<%=IP4chkAuditTcpUdpDialog.ClientID %>').prop('checked',false);
                $('#' + '<%=IP4selectProtocolDialog.ClientID %>').val('TCP');
                $('#' + '<%=IP4selectRuleDialog.ClientID %>').val('AllowReceive');
            };
            function IP4AddRuleDialogSetDefaultValues()
            {
               $('#' + '<%=IP4AddRuleDialogName.ClientID %>').val('');
               $('#' + '<%=IP4addRuleOtherProtocol.ClientID %>').val('');
               $('#' + '<%=IP4addRuleLocalIP.ClientID %>').val('');
               $('#' + '<%=IP4addRuleDestinationIP.ClientID %>').val('');
               $("[id*='IP4chkAddRuleProtocol']").prop('checked',false);
               $("[id*='IP4chkAuditTcpUdpDialog']").prop('checked',false);
               $('#' + '<%=IP4addRuleActionSelect.ClientID %>').val('AllowReceive');
            };
        function IP4ChangeButtonClientClick()
        {
            var index=$('#<%=IP4hdnActiveRowNo.ClientID %>').attr('value');
            if(index==0)
                return;
            var tableRow=$("[id*='IP4hdnRowNo'][value='"+index+"']").parent().parent();
            var ruleType=tableRow.attr('ruleType');
            if(ruleType=="TcpUdp")
                IP4TcpUdpChangeDialog(tableRow);
            else NotIP4TcpUdpChangeDialog(tableRow);
        }
        function IP4TcpUdpChangeDialog(tableRow)
        {
            var name=tableRow.children().find("[id*='IP4lblName']").text();
            $('#' + '<%=IP4txtNameDialog.ClientID %>').val(name);
            var localIP=tableRow.children().find("[id*='IP4lblLocalIP']").text();
            $('#' + '<%=IP4txtLocalIPDialog.ClientID %>').val(localIP);
            var localPort=tableRow.children().find("[id*='IP4lblLocalPort']").text();
            $('#' + '<%=IP4txtLocalPortDialog.ClientID %>').val(localPort);            
            var destIP=tableRow.children().find("[id*='IP4lblDestinationIP']").text();
            $('#' + '<%=IP4txtDestinationIPDialog.ClientID %>').val(destIP);            
            var destPort=tableRow.children().find("[id*='IP4lblDestinationPort']").text();
            $('#' + '<%=IP4txtDestinationPortDialog.ClientID %>').val(destPort);            
            var chkAudit=tableRow.children().find("[id*='IP4hdnAudit']").attr('value');
            if(chkAudit=="true" || chkAudit=="True")
                $('#' + '<%=IP4chkAuditTcpUdpDialog.ClientID %>').prop('checked',true);
            else $('#' + '<%=IP4chkAuditTcpUdpDialog.ClientID %>').prop('checked',false);            
            var protocol=tableRow.children().find("[id*='IP4lblProtocol']").text();
            $('#' + '<%=IP4selectProtocolDialog.ClientID %>').val(protocol);            
            var rule=tableRow.children().find("[id*='IP4lblRule']").attr("valueRule");
            $('#' + '<%=IP4selectRuleDialog.ClientID %>').val(rule);            
             var dOpt = {
                    width: 350,
                    resizable: false,
                     close: function(event, ui)
                        {
                            $('#divOverlay').css('display','none');
                            IP4TcpUdpDialogSetDefaultValues();                            
                        },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () {
                            var btn = '<%=IP4ChangeTcpUdpRuleDialogButton.UniqueID %>';
                            var txt = '#' + '<%=IP4txtLocalIPDialog.ClientID %>';
                            if( Page_ClientValidate('IP4AddressValidation'))
                            {
                                __doPostBack(btn, '');
                                $('#IP4AddTcpUdpRuleDialog').dialog('close');
                            }
                        },
                       <%=Resources.Resource.CancelButtonText%>: function () {                            
                            $('#IP4AddTcpUdpRuleDialog').dialog('close');
                        }
                    }
                };
                $('#IP4AddTcpUdpRuleDialog').dialog(dOpt);
                $('#divOverlay').css('display','inline');
                $('#IP4AddTcpUdpRuleDialog').parent().appendTo(jQuery("form:first"));
            }
         function NotIP4TcpUdpChangeDialog( tableRow)
         {
            var name=tableRow.children().find("[id*='IP4lblName']").text();
            $('#' + '<%=IP4AddRuleDialogName.ClientID %>').val(name);
            var localIP=tableRow.children().find("[id*='IP4lblLocalIP']").text();
            $('#' + '<%=IP4addRuleLocalIP.ClientID %>').val(localIP);         
            var destIP=tableRow.children().find("[id*='IP4lblDestinationIP']").text();
            $('#' + '<%=IP4addRuleDestinationIP.ClientID %>').val(destIP);

            var protocol=tableRow.children().find("[id*='IP4lblProtocol']").text();
            var protocolsType=protocol.split(',');            
            var number;
            var otherProtocols="";
            for(var i=0;i<protocolsType.length;i++)
            {
                if(protocolsType[i]=="*")
                {
                    otherProtocols+="*";
                }
                number=parseInt(protocolsType[i]);
                if(number<=100)
                {
                    $('[class="checkbox"][no="'+number+'"]').children().prop('checked',true);
                }                
                if(number==NaN||number>100)
                {                   
                   otherProtocols+=protocolsType[i];
                   if(i!=protocolsType.length-1)
                   otherProtocols+=", ";
                }
                 $('#<%=IP4addRuleOtherProtocol.ClientID %>').val(otherProtocols);
            }
            var rule=tableRow.children().find("[id*='IP4lblRule']").attr("valueRule");
            $('#' + '<%=IP4addRuleActionSelect.ClientID %>').val(rule);
             var dOpt = {
                    width: 350,                                       
                    resizable: false,
                    close: function(event, ui)
                        {
                            IP4AddRuleDialogSetDefaultValues();  
                            $('#divOverlay').css('display','none');
                        },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () {
                            var btn = '<%=IP4ChangeRulesDialogButton.UniqueID %>';
                            var txt = '#' + '<%=IP4txtLocalIPDialog.ClientID %>';
                            if( Page_ClientValidate('IP4AddRuleValidationGroup'))
                            {
                                __doPostBack(btn, '');                                                              
                                $('#IP4AddRuleDialog').dialog('close');
                            }
                        },
                       <%=Resources.Resource.CancelButtonText%>: function () {                           
                            $('#IP4AddRuleDialog').dialog('close');
                            }
                        }
                    }
                $('#IP4AddRuleDialog').dialog(dOpt);
                $('#divOverlay').css('display','inline');
                $('#IP4AddRuleDialog').parent().appendTo(jQuery("form:first"));
            }

         function IP6TcpUdpDialogSetDefaultValues()
            {
               $('#' + '<%=IP6txtLocalIPDialog.ClientID %>').val('');
                $('#' + '<%=IP6txtNameDialog.ClientID %>').val('');
                $('#' + '<%=IP6txtLocalPortDialog.ClientID %>').val('');
                $('#' + '<%=IP6txtDestinationIPDialog.ClientID %>').val('');
                $('#' + '<%=IP6txtDestinationPortDialog.ClientID %>').val('');
                $('#' + '<%=IP6chkAuditTcpUdpDialog.ClientID %>').prop('checked',false);
                $('#' + '<%=IP6selectProtocolDialog.ClientID %>').val('TCP');
                $('#' + '<%=IP6selectRuleDialog.ClientID %>').val('AllowReceive');
            };
            function IP6AddRuleDialogSetDefaultValues()
            {
               $('#' + '<%=IP6AddRuleDialogName.ClientID %>').val('');
               $('#' + '<%=IP6addRuleOtherProtocol.ClientID %>').val('');
               $('#' + '<%=IP6addRuleLocalIP.ClientID %>').val('');
               $('#' + '<%=IP6addRuleDestinationIP.ClientID %>').val('');
               $("[id*='IP6chkAddRuleProtocol']").prop('checked',false);
               $("[id*='IP6chkAuditTcpUdpDialog']").prop('checked',false);
               $('#' + '<%=IP6addRuleActionSelect.ClientID %>').val('AllowReceive');
            };
        function IP6ChangeButtonClientClick()
        {
            var index=$('#<%=IP6hdnActiveRowNo.ClientID %>').attr('value');
            if(index==0)
                return;
            var tableRow=$("[id*='IP6hdnRowNo'][value='"+index+"']").parent().parent();
            var ruleType=tableRow.attr('ruleType');
            if(ruleType=="TcpUdp")
                IP6TcpUdpChangeDialog(tableRow);
            else NotIP6TcpUdpChangeDialog(tableRow);
        }
        function IP6TcpUdpChangeDialog(tableRow)
        {
            var name=tableRow.children().find("[id*='IP6lblName']").text();
            $('#' + '<%=IP6txtNameDialog.ClientID %>').val(name);
            var localIP=tableRow.children().find("[id*='IP6lblLocalIP']").text();
            $('#' + '<%=IP6txtLocalIPDialog.ClientID %>').val(localIP);
            var localPort=tableRow.children().find("[id*='IP6lblLocalPort']").text();
            $('#' + '<%=IP6txtLocalPortDialog.ClientID %>').val(localPort);           
            var destIP=tableRow.children().find("[id*='IP6lblDestinationIP']").text();
            $('#' + '<%=IP6txtDestinationIPDialog.ClientID %>').val(destIP);            
            var destPort=tableRow.children().find("[id*='IP6lblDestinationPort']").text();
            $('#' + '<%=IP6txtDestinationPortDialog.ClientID %>').val(destPort);            
            var chkAudit=tableRow.children().find("[id*='IP6hdnAudit']").attr('value');
            if(chkAudit=="true" || chkAudit=="True")
                $('#' + '<%=IP6chkAuditTcpUdpDialog.ClientID %>').prop('checked',true);
            else $('#' + '<%=IP6chkAuditTcpUdpDialog.ClientID %>').prop('checked',false); 
            var protocol=tableRow.children().find("[id*='IP6lblProtocol']").text();
            $('#' + '<%=IP6selectProtocolDialog.ClientID %>').val(protocol);           
            var rule=tableRow.children().find("[id*='IP6lblRule']").attr("valueRule");
            $('#' + '<%=IP6selectRuleDialog.ClientID %>').val(rule);           
             var dOpt = {
                    width: 350,
                    resizable: false,
                     close: function(event, ui)
                        {
                            $('#divOverlay').css('display','none');
                            IP6TcpUdpDialogSetDefaultValues();
                        },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () {
                            var btn = '<%=IP6ChangeTcpUdpRuleDialogButton.UniqueID %>';
                            var txt = '#' + '<%=IP6txtLocalIPDialog.ClientID %>';
                            if( Page_ClientValidate('IP6AddressValidation'))
                            {
                                __doPostBack(btn, '');
                                $('#IP6AddTcpUdpRuleDialog').dialog('close');
                            }
                        },
                       <%=Resources.Resource.CancelButtonText%>: function () {                            
                            $('#IP6AddTcpUdpRuleDialog').dialog('close');
                        }
                    }
                };
                $('#IP6AddTcpUdpRuleDialog').dialog(dOpt);
                $('#divOverlay').css('display','inline');
                $('#IP6AddTcpUdpRuleDialog').parent().appendTo(jQuery("form:first"));
            }
         function NotIP6TcpUdpChangeDialog( tableRow)
         {
            var name=tableRow.children().find("[id*='IP6lblName']").text();
            $('#' + '<%=IP6AddRuleDialogName.ClientID %>').val(name);
            var localIP=tableRow.children().find("[id*='IP6lblLocalIP']").text();
            $('#' + '<%=IP6addRuleLocalIP.ClientID %>').val(localIP);
            var destIP=tableRow.children().find("[id*='IP6lblDestinationIP']").text();
            $('#' + '<%=IP6addRuleDestinationIP.ClientID %>').val(destIP);
           
            var protocol=tableRow.children().find("[id*='IP6lblProtocol']").text();
            var protocolsType=protocol.split(',');
            var number;
            var otherProtocols="";
            for(var i=0;i<protocolsType.length;i++)
            {
               if(protocolsType[i]=="*")
                {
                    otherProtocols+="*";
                }
                number=parseInt(protocolsType[i]);
                if(number<=100)
                {
                    $('[class="checkbox"][no="'+number+'"]').children().prop('checked',true);
                }
                if(number==NaN||number>100)
                {
                   otherProtocols+=protocolsType[i];
                   if(i!=protocolsType.length-1)
                   otherProtocols+=", ";
                }
                 $('#<%=IP6addRuleOtherProtocol.ClientID %>').val(otherProtocols);
            }           
            var rule=tableRow.children().find("[id*='IP6lblRule']").attr("valueRule");
            $('#' + '<%=IP6addRuleActionSelect.ClientID %>').val(rule);
             var dOpt = {
                    width: 350,                                       
                    resizable: false,
                    close: function(event, ui)
                        {
                            IP6AddRuleDialogSetDefaultValues();  
                            $('#divOverlay').css('display','none');
                        },
                    buttons: {
                        <%=Resources.Resource.Apply%>: function () {
                            var btn = '<%=IP6ChangeRulesDialogButton.UniqueID %>';
                            var txt = '#' + '<%=IP6txtLocalIPDialog.ClientID %>';
                            if( Page_ClientValidate('IP6AddRuleValidationGroup'))
                            {
                                __doPostBack(btn, '');                                                              
                                $('#IP6AddRuleDialog').dialog('close');
                            }
                        },
                       <%=Resources.Resource.CancelButtonText%>: function () {                           
                            $('#IP6AddRuleDialog').dialog('close');
                            }
                        }
                    }
                $('#IP6AddRuleDialog').dialog(dOpt);
                $('#divOverlay').css('display','inline');
                $('#IP6AddRuleDialog').parent().appendTo(jQuery("form:first"));
            }       
    </script>

    <div id="Tabs">
        <ul>
            <li><a href="#tab0"><%=Resources.Resource.General %></a></li>
            <li><a href="#tab1">IP v4</a> </li>
            <li><a href="#tab2">IP v6</a> </li>
            <li><a href="#tab3"><%=Resources.Resource.JournalEvents %></a> </li>
        </ul>
        <div id='tab0'>
            <div class="ListContrastTable" >
                <div style="margin-left:10px;margin-top:3px">
                    <asp:CheckBox ID="chkFirewallOn" runat="server"  />
                    <asp:Label runat="server" ><%=Resources.Resource.FirewallIsOn %></asp:Label>
                </div>
                <div style="margin-left:10px;margin-top:3px;margin-bottom:3px">
                    <p><asp:Label runat="server"><%=Resources.Resource.NetworkType %></asp:Label></p>
                    <asp:DropDownList ID="ddlFirewallNetworkType" runat="server">
                        <asp:ListItem Text="<%$ Resources:Resource, DomainName %>"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, OpenNetwork %>" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, PrivateNetwork %>"></asp:ListItem>
                        <asp:ListItem Text="<%$ Resources:Resource, CloseNetwork %>"></asp:ListItem>
                    </asp:DropDownList>
                </div> 
            </div>
        </div>
        <div id='tab1'>       
            <asp:UpdatePanel ID="IP4UpdatePanel" runat="server" >
                <ContentTemplate>
                    <asp:HiddenField ID="IP4hdnActiveRowNo" Value='0'  runat="server" />
                    <asp:Table class="ListContrastTable" ID="tblIP4UpdatePanel" style="width:771px" runat="server" >
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3">                            
                                <asp:Panel runat="server" Width="770px" Height="400px" ID="IP4pnlFirewall" style="overflow:scroll">
                                    <asp:DataList runat="server" ID="IP4dlRules" Width="750px" OnItemDataBound="IP4dlRules_ItemDataBound"  OnItemCommand="IP4dlRules_SelectedIndexChanged"  style="table-layout:fixed;word-break: break-all;" rules="all">                                        
                                        <HeaderTemplate>
                                            <tr class="gridViewHeader">
                                                <th runat="server" id="IP4tdEnable" style="width: 80px;text-align: center;" >
                                                <asp:Label runat="server" ><%=Resources.Resource.Enable%></asp:Label>                            
                                                </th>
                                                <th runat="server" id="IP4tdName" style="width: 150px;text-align: center;" >
                                                <asp:Label runat="server" ><%=Resources.Resource.Name%></asp:Label>
                                                </th>
                                                <th runat="server" id="IP4tdLocalIP" style="width: 100px;text-align: center;" >
                                                <asp:Label runat="server" ><%=Resources.Resource.LocalIP%></asp:Label>
                                                </th>
                                                <th runat="server" id="IP4tdLocalPort" style="width: 40px;text-align: center;" >
                                                <asp:Label runat="server"  ><%=Resources.Resource.Port%></asp:Label>
                                                </th>
                                                <th runat="server" id="IP4tdDestinationIP" style="width: 100px;text-align: center;" >
                                                <asp:Label runat="server" ><%=Resources.Resource.DestinationIP%></asp:Label>
                                                </th>
                                                <th runat="server" id="IP4tdDestinationPort" style="width: 40px;text-align: center;" >
                                                <asp:Label runat="server"  ><%=Resources.Resource.Port%></asp:Label>
                                                </th>
                                                <th runat="server" id="IP4tdProtocol" style="width: 125px;text-align: center;" >
                                                <asp:Label runat="server"  ><%=Resources.Resource.Protocol%></asp:Label>
                                                </th>
                                                <th runat="server" id="IP4tdRule" style="width: 115px;text-align: center;">
                                                <asp:Label runat="server"  ><%=Resources.Resource.Rules%></asp:Label>
                                                </th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate >    
                                            <tr runat="server" id="IP4trFirewallItem" IP4tableRowSelected='false'>                                  
                                                <td runat="server" id="IP4tdEnable" style="width: 80px;" >
                                                    <asp:CheckBox runat="server" id="IP4chkEnable" OnCheckedChanged="IP4chkEnable_OnCheckedChanged" AutoPostBack="true"/>
                                                    <asp:HiddenField ID="IP4hdnRowNo" Value="0"   runat="server"  /> 
                                                    <asp:HiddenField ID="IP4hdnAudit" runat='server' Value='false' />                                                                    
                                                </td>
                                                <td runat="server" id="IP4tdName" style="width: 150px;" >
                                                    <asp:Label runat="server" ID="IP4lblName"/>
                                                </td>
                                                <td runat="server" id="IP4tdLocalIP" style="width: 100px;" >
                                                    <asp:Label runat="server" ID="IP4lblLocalIP" />
                                                </td>
                                                <td runat="server" id="IP4tdLocalPort" style="width: 40px;">
                                                    <asp:Label runat="server" ID="IP4lblLocalPort"  />
                                                </td>
                                                <td runat="server" id="IP4tdDestinationIP" style="width: 100px;" >
                                                    <asp:Label runat="server" ID="IP4lblDestinationIP"/>
                                                </td>
                                                <td runat="server" id="IP4tdDestinationPort" style="width: 40px;" >
                                                    <asp:Label runat="server" ID="IP4lblDestinationPort" />
                                                </td>
                                                <td runat="server" id="IP4tdProtocol" style="width: 125px;" >
                                                    <asp:Label runat="server" ID="IP4lblProtocol" />
                                                </td>
                                                <td runat="server" id="IP4tdRule" style="width: 115px;" >
                                                    <asp:Label runat="server" ID="IP4lblRule" /> 
                                                </td>   
                                            </tr>
                                        </ItemTemplate>                         
                                    </asp:DataList>
                                </asp:Panel>                               
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign='Center'>
                                <asp:Button   ID="IP4AddRuleButton" runat='server' Text='<%$ Resources:Resource, AddRule %>' />
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign='Center'>
                                <asp:Button   ID="IP4AddTcpUdpRuleButton"  runat='server'  Text='<%$ Resources:Resource, AddTcpUdpRule%>' OnClientClick="return false;" UseSubmitBehavior="false"/>
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign='Right'>
                                <asp:Button   ID="IP4MoveUp" runat="server" Text='&#x2191;' OnClick="IP4MoveUpButtonClick" />
                            </asp:TableCell>
                         </asp:TableRow>
                         <asp:TableRow>
                            <asp:TableCell HorizontalAlign='Center'>
                                <asp:Button   ID="IP4DeleteButton" runat='server' Text='<%$ Resources:Resource, Delete%>' OnClick="IP4DeleteButtonClick"/>
                            </asp:TableCell >
                            <asp:TableCell HorizontalAlign='Center'>
                                <asp:Button   ID="IP4ChangeButton" runat="server" Text='<%$ Resources:Resource, ChangeRule%>' OnClientClick="IP4ChangeButtonClientClick()" />
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign='Right'>
                                <asp:Button  ID="IP4MoveDown" runat="server"  Text='&#x2193;' OnClick="IP4MoveDownButtonClick" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:Button ID="IP4ApplyTcpUdpRuleDialogButton" runat='server' style="display:none"  OnClick="IP4ApplyTcpUdpRuleDialogButtonClick"   />
                    <asp:Button ID="IP4ApplyRuleDialogButton" runat='server' style="display:none"  OnClick="IP4ApplyRuleDialogButtonClick"   />
                    <asp:Button ID="IP4ChangeTcpUdpRuleDialogButton" runat='server' style="display:none"  OnClick="IP4ChangeTcpUdpRuleDialogButtonClick"   />
                    <asp:Button ID="IP4ChangeRulesDialogButton" runat='server' style="display:none"  OnClick="IP4ChangeRulesDialogButtonClick"   />
                </ContentTemplate>
            </asp:UpdatePanel>
         
            <%--Диалог для добавления новых транспортных правил--%>
            <div id="IP4AddTcpUdpRuleDialog"  style="display:none" class="ui-front">
                <asp:Panel ID="IP4dialogPanel" runat="server" Width="330px">
                    <br />
                    <asp:Label runat="server"><%=Resources.Resource.Name %></asp:Label>
                    <br />
                    <asp:TextBox ID="IP4txtNameDialog" Style="width:200px" runat='server'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="IP4requiredNameDialog" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
                        ControlToValidate="IP4txtNameDialog" Display="None" ValidationGroup="IP4AddressValidation"/>
                        <ajaxToolkit:ValidatorCalloutExtender2 ID="ajaxIP4requiredNameDialog" runat="server" TargetControlID="IP4requiredNameDialog" HighlightCssClass="highlight" PopupPosition="BottomRight" />
                    <br />
                    <br />
                    <asp:Label   runat="server" Text='<%$ Resources:Resource, LocalAddress%>'></asp:Label>
                    <br />
                    <br />
                    <asp:Table ID="IP4tblLocalDialog" runat="server" >
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell >
                                <asp:Label   runat='server'><%=Resources.Resource.LocalIP%></asp:Label>
                            </asp:TableHeaderCell>
                            <asp:TableHeaderCell>
                                <asp:Label  runat='server'><%=Resources.Resource.Port%></asp:Label>
                            </asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableRow>
                            <asp:TableCell Style="width:200px">
                                <asp:TextBox ID="IP4txtLocalIPDialog" Style="width:160px" runat="server" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="IP4requiredLocalIPDialog" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                    ControlToValidate="IP4txtLocalIPDialog" Display="None" ValidationGroup="IP4AddressValidation"/>
                                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ajaxIP4requiredLocalIPDialog" runat="server" TargetControlID="IP4requiredLocalIPDialog" HighlightCssClass="highlight" PopupPosition="BottomLeft" />
                                    <asp:RegularExpressionValidator ControlToValidate="IP4txtLocalIPDialog" ID="regexLocalIPDialog" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressInIncorrectFormat %>' ValidationGroup="IP4AddressValidation"
                                    ValidationExpression="^\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(-(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))?$"
                                    Display="None"/>
                                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ajaxRegexIP4LocalIPDialog" runat="server" TargetControlID="regexLocalIPDialog" HighlightCssClass="highlight" PopupPosition="BottomLeft" />                    
                            </asp:TableCell><asp:TableCell Style="width:120px">
                                <asp:TextBox ID="IP4txtLocalPortDialog" Style="width:80px" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="IP4requiredLocalIPPort" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                    ControlToValidate="IP4txtLocalPortDialog" Display="None" ValidationGroup="IP4AddressValidation"/>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="ajaxIP4requiredLocalIPPort" TargetControlID="IP4requiredLocalIPPort" HighlightCssClass="highlight" PopupPosition="Left"/>
                                        <asp:RegularExpressionValidator ID="rangeIP4requiredLocalIPPort" runat="server" ControlToValidate="IP4txtLocalPortDialog" ErrorMessage='<%$ Resources:Resource, IpAddressInIncorrectFormat %>' ValidationGroup="IP4AddressValidation"  Display="None"
                                        ValidationExpression= "^\*$|^([1-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])(-([1-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5]))?$" />
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="ajaxRangeIP4requiredLocalIPPort" TargetControlID="rangeIP4requiredLocalIPPort" HighlightCssClass="highlight" PopupPosition="Left"/>
                            </asp:TableCell></asp:TableRow></asp:Table><br /><br /><asp:Label   runat="server" Text='<%$ Resources:Resource, DestinationAddress%>'></asp:Label><br /><br /><asp:Table ID="IP4tblDestinationDialog" runat="server" >
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell >
                                <asp:Label  runat="server" ><%=Resources.Resource.DestinationIP%></asp:Label>
                            </asp:TableHeaderCell><asp:TableHeaderCell>
                                <asp:Label   runat="server"><%=Resources.Resource.Port%></asp:Label>
                            </asp:TableHeaderCell></asp:TableHeaderRow><asp:TableRow>
                            <asp:TableCell Style="width:200px">
                                <asp:TextBox ID="IP4txtDestinationIPDialog" Style="width:160px" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="IP4RequiredDestinationIPDialog" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                    ControlToValidate="IP4txtDestinationIPDialog" Display="None" ValidationGroup="IP4AddressValidation">
                                    </asp:RequiredFieldValidator>
                                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ajaxIP4RequiredDestinationIPDialog" runat="server" TargetControlID="IP4RequiredDestinationIPDialog" HighlightCssClass="highlight" PopupPosition="BottomLeft" />
                                    <asp:RegularExpressionValidator ControlToValidate="IP4txtDestinationIPDialog" ID="RegexDestinationIPDialog" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressInIncorrectFormat %>' ValidationGroup="IP4AddressValidation"
                                    ValidationExpression="^\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(-(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))?$"
                                    Display="None"></asp:RegularExpressionValidator>  
                                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ajaxRegexIP4RequiredDestinationIPDialog" runat="server" TargetControlID="RegexDestinationIPDialog" HighlightCssClass="highlight" PopupPosition="BottomLeft" />                                                                                  
                            </asp:TableCell><asp:TableCell Style="width:120px">
                                <asp:TextBox ID="IP4txtDestinationPortDialog" Style="width:80px" runat="server"></asp:TextBox>   
                                        <asp:RequiredFieldValidator ID="IP4requiredDestinationPortDialog" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                    ControlToValidate="IP4txtDestinationPortDialog" Display="None" ValidationGroup="IP4AddressValidation"/>
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="ajaxIP4requiredDestinationPortDialog" TargetControlID="IP4requiredDestinationPortDialog" HighlightCssClass="highlight" PopupPosition="Left"></ajaxToolkit:ValidatorCalloutExtender2>
                                    <asp:RegularExpressionValidator ID="rangeIP4requiredDestinationPortDialog" runat="server" ControlToValidate="IP4txtDestinationPortDialog" ErrorMessage='<%$ Resources:Resource, IpAddressInIncorrectFormat %>' ValidationGroup="IP4AddressValidation"  Display='None'
                                    ValidationExpression= "^\*$|^([1-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])(-([1-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5]))?$" />
                                        <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="ajaxRangeIP4requiredDestinationPortDialog" TargetControlID="RangeIP4requiredDestinationPortDialog" HighlightCssClass="highlight" PopupPosition="Left"></ajaxToolkit:ValidatorCalloutExtender2>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <br /><br />
                    <asp:CheckBox runat='server' id="IP4chkAuditTcpUdpDialog" /><asp:Label  runat='server' Text='<%$ Resources:Resource, Audit%>' />
                    <br />
                    <br />
                    <br />
                    <br />
                                                                                                                    <asp:Table ID="IP4tblCheckBoxDialog" runat='server' Width="250px">
                    <asp:TableRow>
                        <asp:TableCell >
                            <asp:Label  Width="130px" runat='server'><%=Resources.Resource.Protocol %></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell >
                            <asp:DropDownList ID="IP4selectProtocolDialog" runat="server" style="width:100px">
                                <asp:ListItem Value="TCP">TCP</asp:ListItem>
                                <asp:ListItem Value="UDP">UDP</asp:ListItem>
                            </asp:DropDownList>                      
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell >
                            <asp:Label   Width="130px" runat='server'><%=Resources.Resource.Action %></asp:Label>
                        </asp:TableCell><asp:TableCell >
                                <asp:DropDownList ID="IP4selectRuleDialog" runat="server" style="width:100px">
                                <asp:ListItem Value="AllowReceive" Text='<%$ Resources:Resource, AllowReceive %>'/>  
                                <asp:ListItem Value="AllowSend" Text='<%$ Resources:Resource, AllowSend %>'/>
                                <asp:ListItem Value="AllowReceiveSend" Text='<%$ Resources:Resource, AllowReceiveSend %>'/>
                                <asp:ListItem Value="DenyAll" Text='<%$ Resources:Resource, DenyAll %>'/>
                            </asp:DropDownList>  
                        </asp:TableCell>
                     </asp:TableRow>
                </asp:Table>
                    <br /><br />
                </asp:Panel><br /> 
             </div>
             <%--Диалог для добавления новых транспортных правил--%>
            <div id="IP4AddRuleDialog"  style="display:none;" class="ui-front" >
                    <br />
                    <asp:Label  runat="server"><%=Resources.Resource.Name %></asp:Label><br />
                    <asp:TextBox ID="IP4AddRuleDialogName" Style="width:200px" runat='server'></asp:TextBox><asp:RequiredFieldValidator ID="IP4AddRuleDialogNameValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
                        ControlToValidate="IP4AddRuleDialogName" Display="None" ValidationGroup="IP4AddRuleValidationGroup"/>
                        <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutIP4AddRuleDialogName" runat="server" TargetControlID="IP4AddRuleDialogNameValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
                    <br />
                    <br />
                    <asp:Table ID="IP4AddRuleDialogIPTable" runat="server" >
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell >
                                <asp:Label   runat='server'><%=Resources.Resource.LocalIP%></asp:Label>
                            </asp:TableHeaderCell><asp:TableHeaderCell>
                                <asp:Label   runat='server'><%=Resources.Resource.DestinationIP%></asp:Label>
                            </asp:TableHeaderCell></asp:TableHeaderRow><asp:TableRow>
                            <asp:TableCell Style="width:200px">
                                <asp:TextBox ID="IP4addRuleLocalIP" Style="width:150px" runat="server" ></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredIP4addRuleLocalIP" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                    ControlToValidate="IP4addRuleLocalIP" Display="None" ValidationGroup="IP4AddRuleValidationGroup"/>
                                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutExtenderRequiredIP4addRuleLocalIP" runat="server" TargetControlID="RequiredIP4addRuleLocalIP" HighlightCssClass="highlight" PopupPosition="BottomLeft" />
                                    <asp:RegularExpressionValidator ControlToValidate="IP4addRuleLocalIP" ID="RegularIP4addRuleLocalIP" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressInIncorrectFormat %>' ValidationGroup="IP4AddRuleValidationGroup"
                                    ValidationExpression="^\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(-(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))?$"
                                    Display="None"/>
                                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutExtenderRegularIP4addRuleLocalIP" runat="server" TargetControlID="RegularIP4addRuleLocalIP" HighlightCssClass="highlight" PopupPosition="BottomLeft" />                    
                            </asp:TableCell><asp:TableCell Style="width:200px">
                                <asp:TextBox ID="IP4addRuleDestinationIP" Style="width:150px" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredIP4addRuleDestinationIP" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                    ControlToValidate="IP4addRuleDestinationIP" Display="None" ValidationGroup="IP4AddRuleValidationGroup">
                                    </asp:RequiredFieldValidator>
                                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutExtenderRequiredIP4addRuleDestinationIP" runat="server" TargetControlID="RequiredIP4addRuleDestinationIP" HighlightCssClass="highlight" PopupPosition="Left" />
                                    <asp:RegularExpressionValidator ControlToValidate="IP4addRuleDestinationIP" ID="RegularIP4addRuleDestinationIP" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressInIncorrectFormat %>' ValidationGroup="IP4AddRuleValidationGroup"
                                    ValidationExpression="^\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(-(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))?$"
                                    Display="None"></asp:RegularExpressionValidator>  
                                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutExtenderRegularIP4addRuleDestinationIP" runat="server" TargetControlID="RegularIP4addRuleDestinationIP" HighlightCssClass="highlight" PopupPosition="Left" />                    
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <br />
                    <br />
                    <br />
                    <asp:Panel ID="addRuleIP4dialogPanel" runat="server" Height="200px" style="overflow:scroll">
                    <asp:DataList runat='server' ID="IP4dlAddRules" OnItemDataBound="IP4dlAddRules_ItemDataBound" style="table-layout:fixed;word-break: break-all; " rules="all" >
                        <HeaderTemplate>
                            <tr>
                                <th><asp:Label runat="server"></asp:Label></th><th><asp:Label runat='server' Text='<%$ Resources:Resource, Protocol %>'/></th>
                                <th><asp:Label runat='server' Text='<%$ Resources:Resource, Description %>' /></th>
                            </tr>                        
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="tr_IP4dlAddRules" runat='server'>
                                <td><asp:CheckBox ID='IP4chkAddRuleProtocol' runat='server' no='0'/></td>
                                <td><asp:Label ID='IP4lblAddRuleProtocolName1' runat='server' Width='75px'/></td>
                                <td><asp:Label ID='IP4lblAddRuleProtocolName2' runat='server' Width='200px'/></td>
                            </tr>
                        </ItemTemplate>
                    </asp:DataList>
                   </asp:Panel>
                   <br />
                    <asp:Button ID="IP4AddRuleProtocolSelectButton" runat='server' Text='<%$ Resources:Resource, SelectAll %>' UseSubmitBehavior="false" OnClientClick="return false"/>
                    <asp:Button ID="IP4AddRuleProtocolDeselectButton" runat='server' Text='<%$ Resources:Resource, UnselectAll %>' UseSubmitBehavior="false" OnClientClick="return false"/>
                    <br />
                    <br />
                    <asp:Label runat='server' Text='<%$ Resources:Resource, OtherProtocols %>'/>
                    <br />
                    <asp:TextBox runat="server" ID='IP4addRuleOtherProtocol'></asp:TextBox><asp:RegularExpressionValidator ControlToValidate="IP4addRuleOtherProtocol" ID="RegularIP4addRuleOtherProtocol" runat="server" ErrorMessage='<%$ Resources:Resource, ProtocolsInIncorrectFormat %>' ValidationGroup="IP4AddRuleValidationGroup"
                        ValidationExpression="^\*$|^((10[1-9]|1[1-9][0-9]|2[0-4][0-9]|25[0-4])(-(10[1-9]|1[1-9][0-9]|2[0-4][0-9]|25[0-4]))?\s*,\s*)*(10[1-9]|1[1-9][0-9]|2[0-4][0-9]|25[0-4])(-(10[1-9]|1[1-9][0-9]|2[0-4][0-9]|25[0-4]))?$"
                        Display="None"></asp:RegularExpressionValidator><ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutExtenderIP4addRuleOtherProtocol" runat="server" TargetControlID="RegularIP4addRuleOtherProtocol" HighlightCssClass="highlight" PopupPosition="BottomLeft" />                    
                    <br />        
                    <br />
                    <br />
                    <asp:Table  runat='server' Width="250px">                             
                        <asp:TableRow>
                            <asp:TableCell >
                                <asp:Label    Width="130px" runat='server'><%=Resources.Resource.Action %></asp:Label>
                            </asp:TableCell><asp:TableCell >
                                    <asp:DropDownList ID="IP4addRuleActionSelect" runat="server" style="width:100px">
                                    <asp:ListItem Value="AllowReceive" Text='<%$ Resources:Resource, AllowReceive %>'/>  
                                    <asp:ListItem Value="AllowSend" Text='<%$ Resources:Resource, AllowSend %>'/>
                                    <asp:ListItem Value="AllowReceiveSend" Text='<%$ Resources:Resource, AllowReceiveSend %>'/>
                                    <asp:ListItem Value="DenyAll" Text='<%$ Resources:Resource, DenyAll %>'/>
                                </asp:DropDownList>  
                            </asp:TableCell></asp:TableRow></asp:Table><br />
                    <br />
                <br /> 
             </div>

        </div>
        <div id='tab2'>  
            <asp:UpdatePanel ID="IP6UpdatePanel" runat="server" >
                <ContentTemplate>
                    <asp:HiddenField ID="IP6hdnActiveRowNo" Value='0'  runat="server" />
                    <asp:Table class="ListContrastTable" ID="tblIP6UpdatePanel" style="width:771px" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3">                            
                                <asp:Panel runat="server" Width="770px" Height="400px" ID="IP6pnlFirewall" style="overflow:scroll">
                                    <asp:DataList runat="server" ID="IP6dlRules" Width="750px" OnItemDataBound="IP6dlRules_ItemDataBound"  OnItemCommand="IP6dlRules_SelectedIndexChanged"  style="table-layout:fixed;word-break: break-all; " rules="all">                                        
                                        <HeaderTemplate>
                                        <tr class="gridViewHeader">
                                            <th runat="server" id="IP6tdEnable" style="width: 80px;text-align: center;" >
                                            <asp:Label runat="server" ><%=Resources.Resource.Enable%></asp:Label>                            
                                            </th>
                                            <th runat="server" id="IP6tdName" style="width: 150px;text-align: center;" >
                                            <asp:Label runat="server" ><%=Resources.Resource.Name%></asp:Label>
                                            </th>
                                            <th runat="server" id="IP6tdLocalIP" style="width: 100px;text-align: center;">
                                            <asp:Label runat="server" ><%=Resources.Resource.LocalIP%></asp:Label>
                                            </th>
                                            <th runat="server" id="IP6tdLocalPort" style="width: 40px;text-align: center;">
                                            <asp:Label runat="server"  ><%=Resources.Resource.Port%></asp:Label>
                                            </th>
                                            <th runat="server" id="IP6tdDestinationIP" style="width: 100px;text-align: center;" >
                                            <asp:Label runat="server" ><%=Resources.Resource.DestinationIP%></asp:Label>
                                            </th>
                                            <th runat="server" id="IP6tdDestinationPort" style="width: 40px;text-align: center;" >
                                            <asp:Label runat="server"  ><%=Resources.Resource.Port%></asp:Label>
                                            </th>
                                            <th runat="server" id="IP6tdProtocol" style="width: 125px;text-align: center;" >
                                            <asp:Label runat="server"  ><%=Resources.Resource.Protocol%></asp:Label>
                                            </th>
                                            <th runat="server" id="IP6tdRule" style="width: 115px;text-align: center;">
                                            <asp:Label runat="server"  ><%=Resources.Resource.Rules%></asp:Label>
                                            </th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate >    
                                            <tr runat="server" id="IP6trFirewallItem" IP6tableRowSelected='false'>                                  
                                                <td runat="server" id="IP6tdEnable" style="width: 80px;" >
                                                    <asp:CheckBox runat="server" id="IP6chkEnable" OnCheckedChanged="IP6chkEnable_OnCheckedChanged" AutoPostBack="true"/>
                                                    <asp:HiddenField ID="IP6hdnRowNo" Value="0"   runat="server"  /> 
                                                    <asp:HiddenField ID="IP6hdnAudit" runat='server' Value='false' />                                                                   
                                                </td>
                                                <td runat="server" id="IP6tdName" style="width: 150px;" >
                                                    <asp:Label runat="server" ID="IP6lblName"/>
                                                </td>
                                                <td runat="server" id="IP6tdLocalIP" style="width: 100px;" >
                                                    <asp:Label runat="server" ID="IP6lblLocalIP" />
                                                </td>
                                                <td runat="server" id="IP6tdLocalPort" style="width: 40px;">
                                                    <asp:Label runat="server" ID="IP6lblLocalPort"  />
                                                </td>
                                                <td runat="server" id="IP6tdDestinationIP" style="width: 100px;" >
                                                    <asp:Label runat="server" ID="IP6lblDestinationIP"/>
                                                </td>
                                                <td runat="server" id="IP6tdDestinationPort" style="width: 40px;" >
                                                    <asp:Label runat="server" ID="IP6lblDestinationPort" />
                                                </td>
                                                <td runat="server" id="IP6tdProtocol" style="width: 125px;" >
                                                    <asp:Label runat="server" ID="IP6lblProtocol" />
                                                </td>
                                                <td runat="server" id="IP6tdRule" style="width: 115px;" >
                                                    <asp:Label runat="server" ID="IP6lblRule" /> 
                                                </td>   
                                            </tr>
                                        </ItemTemplate>                                     
                                    </asp:DataList>
                                </asp:Panel>                               
                            </asp:TableCell></asp:TableRow><asp:TableRow>
                            <asp:TableCell HorizontalAlign='Center'>
                                <asp:Button   ID="IP6AddRuleButton" runat='server' Text='<%$ Resources:Resource, AddRule %>' />
                            </asp:TableCell><asp:TableCell HorizontalAlign='Center'>
                                <asp:Button   ID="IP6AddTcpUdpRuleButton"  runat='server'  Text='<%$ Resources:Resource, AddTcpUdpRule%>' OnClientClick="return false;" UseSubmitBehavior="false"/>
                            </asp:TableCell><asp:TableCell HorizontalAlign='Right'>
                                <asp:Button   ID="IP6MoveUp" runat="server" Text='&#x2191;' OnClick="IP6MoveUpButtonClick" />
                            </asp:TableCell></asp:TableRow><asp:TableRow>
                            <asp:TableCell HorizontalAlign='Center'>
                                <asp:Button   ID="IP6DeleteButton" runat='server' Text='<%$ Resources:Resource, Delete%>' OnClick="IP6DeleteButtonClick"/>
                            </asp:TableCell ><asp:TableCell HorizontalAlign='Center'>
                                <asp:Button   ID="IP6ChangeButton" runat="server" Text='<%$ Resources:Resource, ChangeRule%>' OnClientClick="IP6ChangeButtonClientClick()" />
                            </asp:TableCell><asp:TableCell HorizontalAlign='Right'>
                                <asp:Button  ID="IP6MoveDown" runat="server"  Text='&#x2193;' OnClick="IP6MoveDownButtonClick" />
                            </asp:TableCell></asp:TableRow></asp:Table><asp:Button ID="IP6ApplyTcpUdpRuleDialogButton" runat='server' style="display:none"  OnClick="IP6ApplyTcpUdpRuleDialogButtonClick"   />
                    <asp:Button ID="IP6ApplyRuleDialogButton" runat='server' style="display:none"  OnClick="IP6ApplyRuleDialogButtonClick"   />
                    <asp:Button ID="IP6ChangeTcpUdpRuleDialogButton" runat='server' style="display:none"  OnClick="IP6ChangeTcpUdpRuleDialogButtonClick"   />
                    <asp:Button ID="IP6ChangeRulesDialogButton" runat='server' style="display:none"  OnClick="IP6ChangeRulesDialogButtonClick"   />
             
               </ContentTemplate>
            </asp:UpdatePanel>
         
         <%--Диалог для добавления новых правил--%>
         <div id="IP6AddTcpUdpRuleDialog"  style="display:none" class="ui-front">
            <asp:Panel ID="IP6dialogPanel" runat="server" Width="330px">
                <br />
                <asp:Label runat="server"><%=Resources.Resource.Name %></asp:Label><br /><asp:TextBox ID="IP6txtNameDialog" Style="width:200px" runat='server'></asp:TextBox><asp:RequiredFieldValidator ID="IP6requiredNameDialog" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
                    ControlToValidate="IP6txtNameDialog" Display="None" ValidationGroup="IP6AddressValidation"/>
                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ajaxIP6requiredNameDialog" runat="server" TargetControlID="IP6requiredNameDialog" HighlightCssClass="highlight" PopupPosition="BottomRight" />
                <br /><br />
                <asp:Label   runat="server" Text='<%$ Resources:Resource, LocalAddress%>'></asp:Label>
                <br /><br />
                <asp:Table ID="IP6tblLocalDialog" runat="server" >
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell >
                            <asp:Label   runat='server'><%=Resources.Resource.LocalIP%></asp:Label>
                        </asp:TableHeaderCell><asp:TableHeaderCell>
                            <asp:Label  runat='server'><%=Resources.Resource.Port%></asp:Label>
                        </asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                    <asp:TableRow>
                        <asp:TableCell Style="width:200px">
                            <asp:TextBox ID="IP6txtLocalIPDialog" Style="width:160px" runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="IP6requiredLocalIPDialog" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                ControlToValidate="IP6txtLocalIPDialog" Display="None" ValidationGroup="IP6AddressValidation"/>
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="ajaxIP6requiredLocalIPDialog" runat="server" TargetControlID="IP6requiredLocalIPDialog" HighlightCssClass="highlight" PopupPosition="BottomLeft" />
                                <asp:RegularExpressionValidator ControlToValidate="IP6txtLocalIPDialog" ID="regexIP6LocalIPDialog" runat="server" ErrorMessage='<%$ Resources:Resource, Ip6AddressInIncorrectFormat %>' ValidationGroup="IP6AddressValidation"
                                ValidationExpression="^\*$|((^|:)([0-9a-fA-F]{0,4}|\*)){1,8}(-((^|:)([0-9a-fA-F]{0,4}|\*)){1,8})?$"
                                Display="None"/>
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="ajaxRegexIP6LocalIPDialog" runat="server" TargetControlID="regexIP6LocalIPDialog" HighlightCssClass="highlight" PopupPosition="BottomLeft" />                    
                        </asp:TableCell><asp:TableCell Style="width:120px">
                            <asp:TextBox ID="IP6txtLocalPortDialog" Style="width:80px" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="IP6requiredLocalIPPort" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                ControlToValidate="IP6txtLocalPortDialog" Display="None" ValidationGroup="IP6AddressValidation"/>
                                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="ajaxIP6requiredLocalIPPort" TargetControlID="IP6requiredLocalIPPort" HighlightCssClass="highlight" PopupPosition="Left"/>
                                    <asp:RegularExpressionValidator ID="rangeIP6requiredLocalIPPort" runat="server" ControlToValidate="IP6txtLocalPortDialog" ErrorMessage='<%$ Resources:Resource, IpAddressInIncorrectFormat %>' ValidationGroup="IP6AddressValidation"  Display="None"
                                    ValidationExpression= "^\*$|^([1-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])(-([1-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5]))?$" />
                                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="ajaxRangeIP6requiredLocalIPPort" TargetControlID="rangeIP6requiredLocalIPPort" HighlightCssClass="highlight" PopupPosition="Left"/>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <br /><br />
                <asp:Label   runat="server" Text='<%$ Resources:Resource, DestinationAddress%>'></asp:Label>
                <br /><br />
                <asp:Table ID="IP6tblDestinationDialog" runat="server" >
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell >
                            <asp:Label  runat="server" ><%=Resources.Resource.DestinationIP%></asp:Label>
                        </asp:TableHeaderCell><asp:TableHeaderCell>
                            <asp:Label   runat="server"><%=Resources.Resource.Port%></asp:Label>
                        </asp:TableHeaderCell></asp:TableHeaderRow>
                    <asp:TableRow>
                        <asp:TableCell Style="width:200px">
                            <asp:TextBox ID="IP6txtDestinationIPDialog" Style="width:160px" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="IP6RequiredDestinationIPDialog" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                ControlToValidate="IP6txtDestinationIPDialog" Display="None" ValidationGroup="IP6AddressValidation">
                                </asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="ajaxIP6RequiredDestinationIPDialog" runat="server" TargetControlID="IP6RequiredDestinationIPDialog" HighlightCssClass="highlight" PopupPosition="BottomLeft" />
                                <asp:RegularExpressionValidator ControlToValidate="IP6txtDestinationIPDialog" ID="RegexIP6DestinationIPDialog" runat="server" ErrorMessage='<%$ Resources:Resource, Ip6AddressInIncorrectFormat %>' ValidationGroup="IP6AddressValidation"
                                ValidationExpression="^\*$|((^|:)([0-9a-fA-F]{0,4}|\*)){1,8}(-((^|:)([0-9a-fA-F]{0,4}|\*)){1,8})?$" Display="None"></asp:RegularExpressionValidator>  
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="ajaxRegexIP6RequiredDestinationIPDialog" runat="server" TargetControlID="RegexIP6DestinationIPDialog" HighlightCssClass="highlight" PopupPosition="BottomLeft" />                                                                                 
                        </asp:TableCell>
                        <asp:TableCell Style="width:120px">
                            <asp:TextBox ID="IP6txtDestinationPortDialog" Style="width:80px" runat="server"></asp:TextBox>   
                                    <asp:RequiredFieldValidator ID="IP6requiredDestinationPortDialog" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                ControlToValidate="IP6txtDestinationPortDialog" Display="None" ValidationGroup="IP6AddressValidation"/>
                                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="ajaxIP6requiredDestinationPortDialog" TargetControlID="IP6requiredDestinationPortDialog" HighlightCssClass="highlight" PopupPosition="Left"></ajaxToolkit:ValidatorCalloutExtender2>
                                <asp:RegularExpressionValidator ID="rangeIP6requiredDestinationPortDialog" runat="server" ControlToValidate="IP6txtDestinationPortDialog" ErrorMessage='<%$ Resources:Resource, IpAddressInIncorrectFormat %>' ValidationGroup="IP6AddressValidation"  Display='None'
                                ValidationExpression= "^\*$|^([1-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])(-([1-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5]))?$" />
                                    <ajaxToolkit:ValidatorCalloutExtender2 runat="server" ID="ajaxRangeIP6requiredDestinationPortDialog" TargetControlID="RangeIP6requiredDestinationPortDialog" HighlightCssClass="highlight" PopupPosition="Left"></ajaxToolkit:ValidatorCalloutExtender2>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <br /><br />
                <asp:CheckBox runat='server' id="IP6chkAuditTcpUdpDialog" /><asp:Label  runat='server' Text='<%$ Resources:Resource, Audit%>' />
                <br />
                <br />
                <br />
                <br />
                <asp:Table ID="IP6tblCheckBoxDialog" runat='server' Width="250px">
                    <asp:TableRow>
                        <asp:TableCell >
                            <asp:Label  Width="130px" runat='server'><%=Resources.Resource.Protocol %></asp:Label>
                        </asp:TableCell><asp:TableCell >
                            <asp:DropDownList ID="IP6selectProtocolDialog" runat="server" style="width:100px">
                                <asp:ListItem Value="TCP">TCP</asp:ListItem>
                                <asp:ListItem Value="UDP">UDP</asp:ListItem>
                            </asp:DropDownList>                      
                        </asp:TableCell></asp:TableRow><asp:TableRow>
                        <asp:TableCell >
                            <asp:Label   Width="130px" runat='server'><%=Resources.Resource.Action %></asp:Label>
                        </asp:TableCell><asp:TableCell >
                                <asp:DropDownList ID="IP6selectRuleDialog" runat="server" style="width:100px">
                                <asp:ListItem Value="AllowReceive" Text='<%$ Resources:Resource, AllowReceive %>'/>  
                                <asp:ListItem Value="AllowSend" Text='<%$ Resources:Resource, AllowSend %>'/>
                                <asp:ListItem Value="AllowReceiveSend" Text='<%$ Resources:Resource, AllowReceiveSend %>'/>
                                <asp:ListItem Value="DenyAll" Text='<%$ Resources:Resource, DenyAll %>'/>
                            </asp:DropDownList>  
                        </asp:TableCell></asp:TableRow></asp:Table><br /><br /></asp:Panel><br /> 
         </div>



         <div id="IP6AddRuleDialog"  style="display:none;" class="ui-front" >
              
                <br />
                <asp:Label  runat="server"><%=Resources.Resource.Name %></asp:Label><br />
               <asp:TextBox ID="IP6AddRuleDialogName" Style="width:200px" runat='server'></asp:TextBox><asp:RequiredFieldValidator ID="IP6AddRuleDialogNameValidator" runat="server" ErrorMessage='<%$ Resources:Resource, FirstNameRequiredErrorMessage %>'
                    ControlToValidate="IP6AddRuleDialogName" Display="None" ValidationGroup="IP6AddRuleValidationGroup"/>
                    <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutIP6AddRuleDialogName" runat="server" TargetControlID="IP6AddRuleDialogNameValidator" HighlightCssClass="highlight" PopupPosition="BottomRight" />
                <br />
                <br />
                <asp:Table ID="IP6AddRuleDialogIPTable" runat="server" >
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell >
                            <asp:Label   runat='server'><%=Resources.Resource.LocalIP%></asp:Label>
                        </asp:TableHeaderCell><asp:TableHeaderCell>
                            <asp:Label   runat='server'><%=Resources.Resource.DestinationIP%></asp:Label>
                        </asp:TableHeaderCell></asp:TableHeaderRow><asp:TableRow>
                        <asp:TableCell Style="width:200px">
                            <asp:TextBox ID="IP6addRuleLocalIP" Style="width:150px" runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredIP6addRuleLocalIP" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                ControlToValidate="IP6addRuleLocalIP" Display="None" ValidationGroup="IP6AddRuleValidationGroup"/>
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutExtenderRequiredIP6addRuleLocalIP" runat="server" TargetControlID="RequiredIP6addRuleLocalIP" HighlightCssClass="highlight" PopupPosition="BottomLeft" />
                                <asp:RegularExpressionValidator ControlToValidate="IP6addRuleLocalIP" ID="RegularIP6addRuleLocalIP" runat="server" ErrorMessage='<%$ Resources:Resource, Ip6AddressInIncorrectFormat %>' ValidationGroup="IP6AddRuleValidationGroup"
                                ValidationExpression="^\*$|((^|:)([0-9a-fA-F]{0,4}|\*)){1,8}(-((^|:)([0-9a-fA-F]{0,4}|\*)){1,8})?$"
                                Display="None"/>
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutExtenderRegularIP6addRuleLocalIP" runat="server" TargetControlID="RegularIP6addRuleLocalIP" HighlightCssClass="highlight" PopupPosition="BottomLeft" />                    
                        </asp:TableCell><asp:TableCell Style="width:200px">
                            <asp:TextBox ID="IP6addRuleDestinationIP" Style="width:150px" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredIP6addRuleDestinationIP" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
                                ControlToValidate="IP6addRuleDestinationIP" Display="None" ValidationGroup="IP6AddRuleValidationGroup">
                                </asp:RequiredFieldValidator>
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutExtenderRequiredIP6addRuleDestinationIP" runat="server" TargetControlID="RequiredIP6addRuleDestinationIP" HighlightCssClass="highlight" PopupPosition="Left" />
                                <asp:RegularExpressionValidator ControlToValidate="IP6addRuleDestinationIP" ID="RegularIP6addRuleDestinationIP" runat="server" ErrorMessage='<%$ Resources:Resource, Ip6AddressInIncorrectFormat %>' ValidationGroup="IP6AddRuleValidationGroup"
                                ValidationExpression="^\*$|((^|:)([0-9a-fA-F]{0,4}|\*)){1,8}(-((^|:)([0-9a-fA-F]{0,4}|\*)){1,8})?$"
                                Display="None"></asp:RegularExpressionValidator>  
                                <ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutExtenderRegularIP6addRuleDestinationIP" runat="server" TargetControlID="RegularIP6addRuleDestinationIP" HighlightCssClass="highlight" PopupPosition="Left" />                    
                                                              
                        </asp:TableCell></asp:TableRow></asp:Table><br />
                <br />
                <br />
                <asp:Panel ID="addRuleIP6dialogPanel" runat="server" Height="200px" style="overflow:scroll">
                <asp:DataList runat='server' ID="IP6dlAddRules" OnItemDataBound="IP6dlAddRules_ItemDataBound" style="table-layout:fixed;word-break: break-all; " rules="all">
                    <HeaderTemplate>
                        <tr>
                            <th><asp:Label runat="server"></asp:Label></th><th><asp:Label runat='server'>Name1</asp:Label></th><th><asp:Label runat='server'>Name2</asp:Label></th></tr></HeaderTemplate><ItemTemplate>
                        <tr id="tr_IP6dlAddRules" runat='server'>
                            <td><asp:CheckBox ID='IP6chkAddRuleProtocol' runat='server' no='0'/></td>
                            <td><asp:Label ID='IP6lblAddRuleProtocolName1' runat='server' Width='75px'/></td>
                            <td><asp:Label ID='IP6lblAddRuleProtocolName2' runat='server' Width='200px'/></td>
                        </tr>
                    </ItemTemplate>
                </asp:DataList>
               </asp:Panel>
               <br />
                <asp:Button ID="IP6AddRuleProtocolSelectButton" runat='server' Text='<%$ Resources:Resource, SelectAll %>' UseSubmitBehavior="false" OnClientClick="return false"/>
                <asp:Button ID="IP6AddRuleProtocolDeselectButton" runat='server' Text='<%$ Resources:Resource, UnselectAll %>' UseSubmitBehavior="false" OnClientClick="return false"/>
                <br />
                <br />
                <asp:Label runat='server' Text='<%$ Resources:Resource, OtherProtocols %>'/>
                <br />
                <asp:TextBox runat="server" ID='IP6addRuleOtherProtocol'></asp:TextBox><asp:RegularExpressionValidator ControlToValidate="IP6addRuleOtherProtocol" ID="RegularIP6addRuleOtherProtocol" runat="server" ErrorMessage='<%$ Resources:Resource, ProtocolsInIncorrectFormat %>' ValidationGroup="IP6AddRuleValidationGroup"
                    ValidationExpression="^\*$|^((10[1-9]|1[1-9][0-9]|2[0-4][0-9]|25[0-4])(-(10[1-9]|1[1-9][0-9]|2[0-4][0-9]|25[0-4]))?\s*,\s*)*(10[1-9]|1[1-9][0-9]|2[0-4][0-9]|25[0-4])(-(10[1-9]|1[1-9][0-9]|2[0-4][0-9]|25[0-4]))?$"
                    Display="None"></asp:RegularExpressionValidator><ajaxToolkit:ValidatorCalloutExtender2 ID="ValidatorCalloutExtenderIP6addRuleOtherProtocol" runat="server" TargetControlID="RegularIP6addRuleOtherProtocol" HighlightCssClass="highlight" PopupPosition="BottomLeft" />                    
                               
                <br />
                <br />
                <br />
                <asp:Table  runat='server' Width="250px">                             
                    <asp:TableRow>
                        <asp:TableCell >
                            <asp:Label    Width="130px" runat='server'><%=Resources.Resource.Action %></asp:Label>
                        </asp:TableCell><asp:TableCell >
                                <asp:DropDownList ID="IP6addRuleActionSelect" runat="server" style="width:100px">
                                <asp:ListItem Value="AllowReceive" Text='<%$ Resources:Resource, AllowReceive %>'/>  
                                <asp:ListItem Value="AllowSend" Text='<%$ Resources:Resource, AllowSend %>'/>
                                <asp:ListItem Value="AllowReceiveSend" Text='<%$ Resources:Resource, AllowReceiveSend %>'/>
                                <asp:ListItem Value="DenyAll" Text='<%$ Resources:Resource, DenyAll %>'/>
                            </asp:DropDownList>  
                        </asp:TableCell></asp:TableRow></asp:Table><br />
                <br />
            
            <br /> 
         </div>
       </div>
        <div id='tab3'>
            <asp:UpdatePanel ID="JournalEventUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="JournalEventPanel" runat='server' EnableViewState="false">
                        <asp:Table ID="JournalEventTable"  runat="server" CssClass="ListContrastTable">
                            <asp:TableHeaderRow ID="TableHeaderRow1" runat="server" class="gridViewHeader">
                                <asp:TableHeaderCell runat="server" id="tdEvent" style="width: 150px;text-align: center;" >
                                    <asp:Label ID="Label1" runat="server" ><%=Resources.Resource.Events %></asp:Label>                            
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdWindowsJournal" style="width: 120px;text-align: center;" >
                                    <asp:Label ID="Label2" runat="server" ><%=Resources.Resource.WindowsJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdLocalJournal" style="width: 120px;text-align: center;" >
                                    <asp:Label ID="Label3" runat="server" ><%=Resources.Resource.LocalJournal %></asp:Label>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell runat="server" id="tdCCJournal" style="width: 120px;text-align: center;">
                                    <asp:Label ID="Label4" runat="server"  ><%=Resources.Resource.CCJournal %></asp:Label>
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
       </div>
    </div>     
        <div id="divOverlay" class="ui-widget-overlay ui-front" style="display:none"></div>
</div>

