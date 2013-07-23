<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrimitiveFilterIpAddress.ascx.cs"
    Inherits="Controls_PrimitiveFilterIpAddress" %>
<%@ Register Src="~/Controls/PrimitiveFilterTemplate.ascx" TagName="PrimitiveFilterTemplate"
    TagPrefix="flt" %>
<script type="text/javascript">
    var IpAddressActionButtons = function () {
        return {
            disable: function () {
                lbtnDeleteIpAddress = $('#<%= lbtnDeleteIpAddress.ClientID %>');
                lbtnEditIpAddress = $('#<%= lbtnEditIpAddress.ClientID %>');
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    lbtnDeleteIpAddress.attr('disabled', true);
                    lbtnEditIpAddress.attr('disabled', true);
                }
                else {
                    
                    lbtnDeleteIpAddress.css('color', 'gray');
                    lbtnEditIpAddress.css('color','gray');
                    
                }
                lbtnEditIpAddress.unbind("click");
                lbtnDeleteIpAddress.unbind("click");
            },

            enable: function () {
                lbtnDeleteIpAddress = $('#<%= lbtnDeleteIpAddress.ClientID %>');
                lbtnEditIpAddress = $('#<%= lbtnEditIpAddress.ClientID %>');
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    lbtnDeleteIpAddress.attr('disabled',false);
                    lbtnEditIpAddress.attr('disabled', false);
                }
                else {
                    lbtnDeleteIpAddress.css('color','blue');
                    lbtnEditIpAddress.css('color',"blue");
                }
                lbtnEditIpAddress.unbind("click");
                lbtnDeleteIpAddress.unbind("click");
                lbtnDeleteIpAddress.click(function () {
                    IpAddressListBox.remove();
                    return false;
                })
                lbtnEditIpAddress.click(function () {
                    IpAddressListBox.edit();
                    return false;
                })
            }
        };
    } ();

    var NewIpAddressTextBox = function () {
        return {
            getText: function () {
                return $('#<%= tboxNewIpAddress.ClientID %>').val();
            },

            setText: function (text) {
                $('#<%= tboxNewIpAddress.ClientID %>').val(text);
            }
        };
    } ();

    var IpAddressListBox = function () 
    {
        function clear() 
        {
            $('#<%= lboxIpAddress.ClientID %>>option').remove();
        }

        function matchRegexp(text) {
            var regexp = new RegExp('^\\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.\\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.\\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.\\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(-(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))?$');
            return regexp.test(text);
        }

        function findInvalid() {
           var ret = -1;
            $('#<%= lboxIpAddress.ClientID %>>option').each(function(i){
                if (!matchRegexp($(this).text()) || $(this).text() != $(this).val())
                {
                   ret = $(this).val();
                }
              })
            return ret;
        }

        function validateText() {
            return Page_ClientValidate('NewIpAddressValidation');
        }
        return {
            validateList: function () {
                var invalid = findInvalid();
                if (invalid != -1) 
                {
                    $('#<%= lboxIpAddress.ClientID %>').val(invalid);
                    NewIpAddressTextBox.setText(invalid);
                    validateText();
                    return false;
                }
                return true;
             },

            add: function () {
                lboxIpAddress = $('#<%= lboxIpAddress.ClientID %>');
                if (!validateText()) {
                    var tooltip = $('#<%= tboxNewIpAddress.ClientID %>').attr('title',"ip field").tooltip({close:function(){$(this).tooltip("destroy").removeAttr("title")},tooltipClass:"highlight", content: '<%= Resources.Resource.IpAddressFilterInIncorrectFormat %>' }).tooltip("open");
                    return;
                }
                var data = NewIpAddressTextBox.getText();
                $('#<%= lboxIpAddress.ClientID %>').append($('<option></option>').val(data).text(data).css('background-color',"#8CFF8C"));
                lboxIpAddress.val(data)
                if ( $('#<%= lboxIpAddress.ClientID %>>option').length == 1) 
                {
                    IpAddressActionButtons.enable();
                }
            },

            remove: function () {
                var ipbox =  $("#<%= lboxIpAddress.ClientID %>");
                var option = $("#<%= lboxIpAddress.ClientID %> option");
                var index = option.index($("#<%= lboxIpAddress.ClientID %> option:selected"));
                if(index == -1) return;
                $.when( $("#<%= lboxIpAddress.ClientID %> option:selected").remove() ).then(function(){
                    if( index < option.length)
                    {
                        index--;
                        ipbox.val(option.eq(index).val())
                    }
                    validateText();
                    NewIpAddressTextBox.setText(ipbox.val());
                });
            },

            edit: function () {
                var option = $("#<%= lboxIpAddress.ClientID %> option");
                var index = option.index($("#<%= lboxIpAddress.ClientID %> option:selected"));
                if (index == -1) return;
                if (!validateText()) {
                    var tooltip = $('#<%= tboxNewIpAddress.ClientID %>').attr('title',"ip field").tooltip({close:function(){$(this).tooltip("destroy").removeAttr("title")},tooltipClass:"highlight", content: '<%= Resources.Resource.IpAddressFilterInIncorrectFormat %>' }).tooltip("open");
                    return;
                }
                var data = NewIpAddressTextBox.getText();
                option.eq(index).text(data).val(data).css('background-color',"#8CFF8C")
            },

            onChange: function () {
                var lbox = $("#<%= lboxIpAddress.ClientID %>");
                var option = $("#<%= lboxIpAddress.ClientID %> option");
                var index = option.index($("#<%= lboxIpAddress.ClientID %> option:selected"));
                if (index == -1) return;
                NewIpAddressTextBox.setText(lbox.val());
                validateText();
            },

            generate: function () {
                var arr = new Array();
                $("#<%= lboxIpAddress.ClientID %> option").each(function(){
                    arr.push($(this).val());
                })
               
                return arr.join('&');
            },
            populate: function (text) {
                lboxIpAddress = $('#<%= lboxIpAddress.ClientID %>');
                var option = false;
                clear();

                var splited = text.split('&');

                for (i = 0; i < splited.length; i++) {
                    option = $("#<%= lboxIpAddress.ClientID %> option");
                    if (splited[i] == '') continue;
                    
                    if (matchRegexp(splited[i])) {
                        lboxIpAddress.append($('<option></option>').text(splited[i]).val(splited[i]).css('background-color',"#8CFF8C"))
                    }
                    else {
                        lboxIpAddress.append($('<option></option>').text(splited[i]).val(splited[i]).css('background-color',"#FF8C8C"))
                    }
                }
                option = $("#<%= lboxIpAddress.ClientID %> option");
                if (option.length > 0) {
                    lboxIpAddress.val(option.eq(0).val())
                    NewIpAddressTextBox.setText(lboxIpAddress.val());
                    validateText();
                    IpAddressActionButtons.enable();
                }
                else {
                    NewIpAddressTextBox.setText('');
                    IpAddressActionButtons.disable();
                }
            }
        };
    } ();

    var IpAddressTextBox = function () {
        return {
            getText: function () {
                return $('#<%= tboxFilter.ClientID %>').val();
            },

            setText: function (text) {
                $('#<%= tboxFilter.ClientID %>').val(text);
            }
        };
    } ();

    var IpAddressValidator = function () {
        return {
            enable: function () {
                regexFilter = document.getElementById('<%= regexFilter.ClientID %>');
                ValidatorValidate(regexFilter);
            },

            disable: function (text) {
                regexFilter = document.getElementById('<%= regexFilter.ClientID %>');

                regexFilterExt = $find('<%= regexFilterExt.ClientID %>');
                if (AjaxControlToolkit.ValidatorCalloutBehavior._currentCallout ===
                            regexFilterExt) {
                    AjaxControlToolkit.ValidatorCalloutBehavior._currentCallout.hide();
                }

            }
        };
    } ();

    var NewIpAddressValidator = function () {
        return {
            disable: function (text) {
              /*  reqNewIpAddressExt = $find('<= reqNewIpAddressExt.ClientID %>');
                regexNewIpAddressExt = $find('<= regexNewIpAddressExt.ClientID %>');
                if ((AjaxControlToolkit.ValidatorCalloutBehavior._currentCallout === regexFilterExt) ||
                    (AjaxControlToolkit.ValidatorCalloutBehavior._currentCallout === regexNewIpAddressExt)) {
                    //control is currently active, hide it
                    AjaxControlToolkit.ValidatorCalloutBehavior._currentCallout.hide();
                }
                //hide highlight on element to validate
                if (regexNewIpAddressExt._highlightCssClass && regexNewIpAddressExt._invalid) {
                    Sys.UI.DomElement.removeCssClass(regexNewIpAddressExt._elementToValidate, regexNewIpAddressExt._highlightCssClass)
                }
                if (reqNewIpAddressExt._highlightCssClass && reqNewIpAddressExt._invalid) {
                    Sys.UI.DomElement.removeCssClass(reqNewIpAddressExt._elementToValidate, reqNewIpAddressExt._highlightCssClass)
                }*/
            }
        };
    } ();

    var IpAddressDialog = function () {
        var dialog;
        var asyncPostBack = false;

        function onApply() {
            if (IpAddressListBox.validateList()) {
                IpAddressTextBox.setText(IpAddressListBox.generate());
             }
        }

        function onHide() {
            IpAddressValidator.enable();
            NewIpAddressValidator.disable();
            PageRequestManagerHelper.enableAsyncPostBack();
           $('#<%= dlgIpAddress.ClientID  %>').css('display', "none");
        }

        function onShow() {
            PageRequestManagerHelper.abortAsyncPostBack();
            PageRequestManagerHelper.disableAsyncPostBack();
        }

        return {
            setAsyncPostBack: function () {
                asyncPostBack = true;
            },
            show: function () {
                
                if ($('#<%= imgHelper.ClientID%>').attr("disabled") === "disabled") { return; }
                if (!dialog || asyncPostBack) {
                
                dialog =  $('#<%= dlgIpAddress.ClientID  %>').dialog({
                        title: '<%=Resources.Resource.IPAddress %>',
                        width: 340,
                        draggable:false,
                        modal: true,
                        open:function(){
                            $(this).find('a').button();
                            
                        },
                        close:function(){
                            onHide();
                            dialog = false;
                            $(this).dialog("destroy");
                        },
                        buttons: {'<%= Resources.Resource.Apply  %>':function(){
                            dialog.dialog("close");
                            onApply();
                        }},

                    });
                    asyncPostBack = false;
                    $('#<%= lbtnAddIpAddress.ClientID %>').click(function(){
                        IpAddressListBox.add(); 
                        return false;
                    });
                }
                IpAddressValidator.disable();
                IpAddressListBox.populate(IpAddressTextBox.getText());
            }
        };
    } ();
</script>
<flt:PrimitiveFilterTemplate runat="server" ID="fltTemplate" TextFilter="Name">
    <FilterTemplate>
        <div style="padding: 5px; padding-top: 3px;">
            <div style="float:left;">
                <asp:TextBox runat="server" ID="tboxFilter" Style="width: 150px; height: 17px;"></asp:TextBox>
                <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ftboxFilter" FilterType="Custom, Numbers"
                    TargetControlID="tboxFilter" ValidChars=".*-&">
                </ajaxToolkit:FilteredTextBoxExtender>
                <asp:RegularExpressionValidator ControlToValidate="tboxFilter" ID="regexFilter"
                    runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressFilterInIncorrectFormat %>' ValidationGroup="FilterValidation"
                    ValidationExpression="^((\*|(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*|(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*|(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*|(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(-(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))?)&)*(\*|(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*|(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*|(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*|(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(-(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))?){1}$"
                    Display="None"></asp:RegularExpressionValidator>
                <ajaxToolkit:ValidatorCalloutExtender ID="regexFilterExt" runat="server" TargetControlID="regexFilter"
                    HighlightCssClass="highlight">
                </ajaxToolkit:ValidatorCalloutExtender>
            </div>
            <div runat="server" class="ImageIPAddress" style="float:left; margin-left: 5px;" id="imgHelper" onclick="IpAddressDialog.show(); return false;"></div>
        </div>
    </FilterTemplate>
</flt:PrimitiveFilterTemplate>
<div id="dlgIpAddress" runat="server" style="display:none" >
    <p>
                    <asp:LinkButton ID="lbtnAddIpAddress" runat="server"><%=Resources.Resource.Add%></asp:LinkButton>
                    <asp:LinkButton ID="lbtnEditIpAddress" runat="server"><%=Resources.Resource.Edit%></asp:LinkButton>
                    <asp:LinkButton ID="lbtnDeleteIpAddress" runat="server"><%=Resources.Resource.Delete%></asp:LinkButton>
        <asp:TextBox ID="tboxNewIpAddress" runat="server"  Style="width: 300px; margin-top: 8px;  margin-bottom: 10px;"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqNewIpAddress" runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressRequired %>'
            ControlToValidate="tboxNewIpAddress" Display="None" ValidationGroup="NewIpAddressValidation">
        </asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ControlToValidate="tboxNewIpAddress" ID="regexNewIpAddress"
            runat="server" ErrorMessage='<%$ Resources:Resource, IpAddressInIncorrectFormat %>' ValidationGroup="NewIpAddressValidation"
            ValidationExpression="^\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(-(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))?$"
            Display="None"></asp:RegularExpressionValidator>
        <asp:Literal ID="litIpAddressExamples" Text='<%$ Resources:Resource, IpAddressExamplesLiteral %>' runat="server"></asp:Literal>
        <select size="4"   ID="lboxIpAddress" runat="server" class="dropdownlist" onchange="IpAddressListBox.onChange()" style="height:180px;width:300px; margin-top: 8px; margin-left: 1px;">
        </select>
 </p>
</div>
