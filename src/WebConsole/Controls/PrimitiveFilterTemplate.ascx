<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrimitiveFilterTemplate.ascx.cs" Inherits="Controls_PrimitiveFilterTemplate" %>

<div runat="server" id="divPrimitiveFilterTemplate" class="primitiveFilterTemplate" >
    <div class="nameFilter">
        <asp:Label runat="Server" ID="lblNameFilter"  ></asp:Label>
    </div>
    <div class="logicFilter">
        <asp:DropDownList runat="server" ID="ddlAndOr" SkinID="ddlLogic" />
        <asp:DropDownList runat="server" ID="ddlNot" SkinID="ddlLogic" />
    </div>
    <div class="contentFilter">
        <asp:PlaceHolder runat="server" ID="placeHolderFilter"></asp:PlaceHolder> 
    </div>
    <div class="selectedFilter">
        <asp:CheckBox runat="server" ID="cboxUseFilter" />
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#<%= cboxUseFilter.ClientID%>').live('click', function () {
            var activeValidators = new Array();
            $("[id^='<%= ClientID%>'][id!='<%= cboxUseFilter.ClientID%>'][id!='<%= divPrimitiveFilterTemplate.ClientID%>']").each(function () {
                var enabled = $('#<%= cboxUseFilter.ClientID%>').is(':checked')
                if ($(this).attr("radioDisabled") === "") {
                    return;
                }

                if (enabled) {
                    $(this).removeAttr("disabled")
                }
                else {
                    $(this).attr("disabled", "disabled")
                }

                if (typeof (Page_Validators) == "undefined") {
                    return;
                }

                //check if control is validator
                var length = Page_Validators.length;
                var isvalidator = false;
                for (i = 0; i < length; i++) {
                    if (Page_Validators[i] === this) {
                        isvalidator = true;
                        break;
                    }
                }

                //_ClientState hidden input created for ValidatorCalloutExtender
                var expr = new RegExp('_ClientState$');
                if (expr.test(this.id)) {
                    //ends with _ClientState
                    var extSel = this.id.replace(expr, '');
                    //has corresponding control
                    var ext = $find(extSel);
                    if (ext.constructor ===
                        AjaxControlToolkit.ValidatorCalloutBehavior.prototype.constructor) {
                        //control has ValidatorCalloutBehavior type
                        if (AjaxControlToolkit.ValidatorCalloutBehavior._currentCallout ===
                            ext) {
                            //control is currently active, hide it
                            AjaxControlToolkit.ValidatorCalloutBehavior._currentCallout.hide();
                        }
                        //hide highlight on element to validate
                        if (ext._highlightCssClass && ext._invalid) {
                            Sys.UI.DomElement.removeCssClass(ext._elementToValidate, ext._highlightCssClass)
                        }
                        return;
                    }
                }

                if (!isvalidator) return;
                if (enabled) {
                    ValidatorEnable(this, true);
                    activeValidators.push(this);
                }
                else {
                    ValidatorEnable(this, false);
                }
            });

            if ($('#<%= cboxUseFilter.ClientID%>').is(':checked')) {
                while (activeValidators.length > 0) {
                    ValidatorValidate(activeValidators.shift());
                }
            }
        });
    });
</script>