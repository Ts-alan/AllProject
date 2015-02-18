var RadioDateTime = function () {
    function panelEnable(sel, enable) {
        var activeValidators = new Array();
        $(sel + " *").each(function () {
            if (this.id === "") {
                // make sure that only selectors with ids are disabled
                // not options, div containers etc.
                return;
            }
            this.disabled = !enable;

            if (enable) {
                $(this).removeAttr("radioDisabled");
            }
            else {
                $(this).attr("radioDisabled", "");
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
            if (enable) {
                ValidatorEnable(this, true);
                activeValidators.push(this);
            }
            else {
                ValidatorEnable(this, false);
            }
        });

        if (enable) {
            while (activeValidators.length > 0) {
                ValidatorValidate(activeValidators.shift());
            }
        }
    }

    return {
        dateIntervalActive: function (panDateInterval, panDateDeclination) {
            panelEnable('#' + panDateInterval, true);
            panelEnable('#' + panDateDeclination, false);
        },

        dateDeclinationActive: function (panDateInterval, panDateDeclination) {
            panelEnable('#' + panDateInterval, false);
            panelEnable('#' + panDateDeclination, true);
        }
    };
} ();