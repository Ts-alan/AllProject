var PageRequestManagerHelper = function () {
    function PageRequestManagerRegistered() {
        if (typeof (Sys) === 'undefined') return false;
        return (Sys.WebForms.PageRequestManager != undefined);
    }
    return {
        enableAsyncPostBack: function () {
            if (!PageRequestManagerRegistered()) return;
            Sys.WebForms.PageRequestManager.getInstance()._disableAsyncPostBack = false;
        },

        disableAsyncPostBack: function () {
            if (!PageRequestManagerRegistered()) return;
            Sys.WebForms.PageRequestManager.getInstance()._disableAsyncPostBack = true;
        },

        abortAsyncPostBack: function () {
            if (!PageRequestManagerRegistered()) return;
            if (Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {
                Sys.WebForms.PageRequestManager.getInstance().abortPostBack();
            }
        }
    };
} ();