var CollapsiblePanel = function () {
    return {
        Toggle: function (collapsiblePanelClientID, imbtnSwitchClientID, hfSwitchClientID, ProfileTheme) {
            var collapsiblePanel = document.getElementById(collapsiblePanelClientID);
            var imbtnSwitch = document.getElementById(imbtnSwitchClientID);
            var hfSwitch = document.getElementById(hfSwitchClientID);
            if (collapsiblePanel.style["visibility"] != "visible") {
                collapsiblePanel.style["visibility"] = "visible";
                collapsiblePanel.style["height"] = "auto";
                imbtnSwitch.src = 'App_Themes/' + ProfileTheme + '/Images/arrow_up.gif';
                hfSwitch.value = 'true';
            }
            else {
                collapsiblePanel.style["visibility"] = "hidden";
                collapsiblePanel.style["height"] = "0";
                collapsiblePanel.style["overflow"] = "hidden";
                imbtnSwitch.src = 'App_Themes/' + ProfileTheme + '/Images/arrow_down.gif';
                hfSwitch.value = 'false';
            }
        }
    };
} ();