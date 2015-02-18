var MultipleSelectionDropDownList = function () {
    function selectedIndexChanged(lboxOptionsClientID, lblSelectedTextClientID) {
        var lboxControl = $('#' + lboxOptionsClientID)[0];
        var arrSelectedItemsText = new Array();
        for (var i = 0; i < lboxControl.length; i++) {
            if (!lboxControl.options[i].selected) {
                continue;
            }
            arrSelectedItemsText.push(lboxControl.options[i].text);
        }
        var strSelectedText = arrSelectedItemsText.join(', ');
        if (strSelectedText.length > 25)
            strSelectedTextShort = strSelectedText.substring(0, 25) + "..";
        else strSelectedTextShort = strSelectedText;

        $('#' + lblSelectedTextClientID).html(strSelectedTextShort);
        $('#' + lblSelectedTextClientID)[0].title = strSelectedText;
    }

    function showOptions(divOptionsClientID, show) {
        if (show) {
            $('#' + divOptionsClientID).show();
        }
        else {
            $('#' + divOptionsClientID).hide();
        }
    }

    return {
        RegisterClickEvents: function (divOptionsClientID, lboxOptionsClientID, lblSelectedTextClientID,
                imgDropDownClientID) {
            $('html').click(function () {
                $('#' + divOptionsClientID).hide();
            });

            $('#' + lboxOptionsClientID).click(function (event) {
                event.stopPropagation();
            });

            $('#' + lblSelectedTextClientID).click(function (event) {
                if ($('#' + lblSelectedTextClientID).attr("disabled") === "disabled") { return; }
                show = $('#' + divOptionsClientID).is(':hidden');
                setTimeout(function () { showOptions(divOptionsClientID, show); }, 0);
            });

            $('#' + imgDropDownClientID).click(function (event) {
                if ($('#' + imgDropDownClientID).attr("disabled") === "disabled") { return; }
                show = $('#' + divOptionsClientID).is(':hidden');
                setTimeout(function () { showOptions(divOptionsClientID, show); }, 0);
            });

            $('#' + lboxOptionsClientID).change(function () {
                selectedIndexChanged(lboxOptionsClientID, lblSelectedTextClientID);
            });
        }
    };
} ();
