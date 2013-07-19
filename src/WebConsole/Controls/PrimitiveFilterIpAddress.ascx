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
                    lbtnDeleteIpAddress.click(function () {
                        return false;
                    })
                    lbtnDeleteIpAddress.css('color', 'gray');
                    lbtnEditIpAddress.click(function () {
                        return false;
                    })
                    lbtnEditIpAddress.css('color','gray');
                }
            },

            enable: function () {
                lbtnDeleteIpAddress = $('#<%= lbtnDeleteIpAddress.ClientID %>');
                lbtnEditIpAddress = $('#<%= lbtnEditIpAddress.ClientID %>');
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    lbtnDeleteIpAddress.attr('disabled',false);
                    lbtnEditIpAddress.attr('disabled', false);
                }
                else {
                    lbtnDeleteIpAddress.click(function () {
                        IpAddressListBox.remove();
                        return false;
                    })
                    lbtnDeleteIpAddress.css('color','blue');
                    lbtnEditIpAddress.click(function () {
                        IpAddressListBox.edit();
                        return false;
                    })
                    lbtnEditIpAddress.css('color',"blue");
                }
            }
        };
    } ();

    var NewIpAddressTextBox = function () {
        return {
            getText: function () {
                tboxNewIpAddress = $('#<%= tboxNewIpAddress.ClientID %>');
                return tboxNewIpAddress.val();
            },

            setText: function (text) {
                tboxNewIpAddress = $('#<%= tboxNewIpAddress.ClientID %>');
                tboxNewIpAddress.val(text);
            }
        };
    } ();

    var IpAddressListBox = function () {
        function clear() {
            lboxIpAddress = document.getElementById('<%= lboxIpAddress.ClientID %>');

            for (var i = lboxIpAddress.options.length - 1; i >= 0; i--) {
                lboxIpAddress.options[i] = null;
            }
            lboxIpAddress.selectedIndex = -1;
        }

        function matchRegexp(text) {
            var regexp = new RegExp('^\\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.\\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.\\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.\\*$|^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(-(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))?$');
            return regexp.test(text);
        }

        function findInvalid() {
            lboxIpAddress = document.getElementById('<%= lboxIpAddress.ClientID %>');
            for (i = 0; i < lboxIpAddress.options.length; i++) {
                if (!matchRegexp(lboxIpAddress.options[i].text)) {
                    return i;
                }
            }
            return -1;
        }

        function validateText() {
            return Page_ClientValidate('NewIpAddressValidation');
        }
        return {
            validateList: function () {
                lboxIpAddress = document.getElementById('<%= lboxIpAddress.ClientID %>');
                var invalid = findInvalid();
                if (invalid != -1) {
                    lboxIpAddress.selectedIndex = invalid;
                    NewIpAddressTextBox.setText(lboxIpAddress.options[lboxIpAddress.selectedIndex].text);
                    validateText();
                    return false;
                }
                return true;
            },

            add: function () {
                lboxIpAddress = document.getElementById('<%= lboxIpAddress.ClientID %>');

                if (!validateText()) {

                    var tooltip = $('#<%= tboxNewIpAddress.ClientID %>').attr('title',"ip field").tooltip({close:function(){$(this).tooltip("destroy").removeAttr("title")},tooltipClass:"highlight", content: '<%= Resources.Resource.IpAddressFilterInIncorrectFormat %>' }).tooltip("open");
                   
                    return;
                }

                var newOpt = new Option();
                newOpt.text = NewIpAddressTextBox.getText();
                newOpt.value = NewIpAddressTextBox.getText();

                lboxIpAddress.options[lboxIpAddress.options.length] = newOpt;
                lboxIpAddress.selectedIndex = lboxIpAddress.options.length - 1;
                if (lboxIpAddress.options.length == 1) {
                    IpAddressActionButtons.enable();
                }
                lboxIpAddress.options[lboxIpAddress.selectedIndex].style.backgroundColor = "#8CFF8C";
            },

            remove: function () {
                lboxIpAddress = document.getElementById('<%= lboxIpAddress.ClientID %>');

                if (lboxIpAddress.selectedIndex == -1) return;

                prevIndex = lboxIpAddress.selectedIndex;

                lboxIpAddress.options[lboxIpAddress.selectedIndex] = null;
                if (prevIndex < lboxIpAddress.options.length) {
                    lboxIpAddress.selectedIndex = prevIndex;
                }
                else {
                    lboxIpAddress.selectedIndex = prevIndex - 1;
                }

                if (lboxIpAddress.options.length == 0) {
                    NewIpAddressTextBox.setText('');
                    NewIpAddressValidator.disable();
                    IpAddressActionButtons.disable();
                }
                else {
                    NewIpAddressTextBox.setText(lboxIpAddress.options[lboxIpAddress.selectedIndex].text);
                    validateText();
                }
            },

            edit: function () {
                lboxIpAddress = document.getElementById('<%= lboxIpAddress.ClientID %>');

                if (lboxIpAddress.selectedIndex == -1) return;

                if (!validateText()) {
                    $
                    var tooltip = new Ext.tip.ToolTip({
                        html: '<%= Resources.Resource.IpAddressFilterInIncorrectFormat %>',
                        autoHide: true,
                        target: document.getElementById('<%= tboxNewIpAddress.ClientID %>'),
                        anchor: 'top',
                        closable: false,
                        anchorOffset: 100,
                        listeners: {
                            hide: function (panel, eOpts) {
                                this.setDisabled(true);
                            }
                        }
                    });
                    tooltip.show();
                    return;
                }

                lboxIpAddress.options[lboxIpAddress.selectedIndex].text = NewIpAddressTextBox.getText(); ;
                lboxIpAddress.options[lboxIpAddress.selectedIndex].value = NewIpAddressTextBox.getText(); ;
                lboxIpAddress.options[lboxIpAddress.selectedIndex].style.backgroundColor = "#8CFF8C";
            },

            onChange: function () {
                lboxIpAddress = document.getElementById('<%= lboxIpAddress.ClientID %>');

                if (lboxIpAddress.selectedIndex == -1) return;

                NewIpAddressTextBox.setText(lboxIpAddress.options[lboxIpAddress.selectedIndex].text);
                validateText();
            },

            generate: function () {
                var arr = new Array();
                for (i = 0; i < lboxIpAddress.options.length; i++) {
                    arr.push(lboxIpAddress.options[i].value);
                }
                return arr.join('&');
            },

            populate: function (text) {
                lboxIpAddress = document.getElementById('<%= lboxIpAddress.ClientID %>');

                clear();

                var splited = text.split('&');

                for (i = 0; i < splited.length; i++) {
                    if (splited[i] == '') continue;
                    var newOpt = new Option();
                    newOpt.text = splited[i];
                    newOpt.value = splited[i];
                    lboxIpAddress.options[lboxIpAddress.options.length] = newOpt;
                    if (matchRegexp(splited[i])) {
                        lboxIpAddress.options[lboxIpAddress.options.length - 1].style.backgroundColor = "#8CFF8C";
                    }
                    else {
                        lboxIpAddress.options[lboxIpAddress.options.length - 1].style.backgroundColor = "#FF8C8C";
                    }
                }

                if (lboxIpAddress.options.length > 0) {
                    lboxIpAddress.selectedIndex = 0;
                    NewIpAddressTextBox.setText(lboxIpAddress.options[lboxIpAddress.selectedIndex].text);
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
                tboxIpAddress = document.getElementById('<%= tboxFilter.ClientID %>');
                return tboxIpAddress.value;
            },

            setText: function (text) {
                tboxIpAddress = document.getElementById('<%= tboxFilter.ClientID %>');
                tboxIpAddress.value = text;
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
                    //control is currently active, hide it
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
                dialog.hide();
            }
        }

        function onHide() {
            IpAddressValidator.enable();
            NewIpAddressValidator.disable();
            PageRequestManagerHelper.enableAsyncPostBack();
            document.getElementById('<%= dlgIpAddress.ClientID  %>').style.display = "none";
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
                    <asp:LinkButton ID="lbtnAddIpAddress" OnClientClick="IpAddressListBox.add(); return false;"
                        runat="server"><%=Resources.Resource.Add%></asp:LinkButton>
                    <asp:LinkButton ID="lbtnEditIpAddress" OnClientClick="IpAddressListBox.edit(); return false;"
                        runat="server"><%=Resources.Resource.Edit%></asp:LinkButton>
                    <asp:LinkButton ID="lbtnDeleteIpAddress" OnClientClick="IpAddressListBox.remove(); return false;"
                        runat="server"><%=Resources.Resource.Delete%></asp:LinkButton>
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
