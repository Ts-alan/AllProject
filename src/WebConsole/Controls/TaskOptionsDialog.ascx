<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TaskOptionsDialog.ascx.cs"
    Inherits="Controls_TaskOptionsDialog" %>
    <%@ Register Src="~/Controls/SaveAsDialog.ascx" TagName="SaveAsDialog" TagPrefix="cc1" %>
<script type="text/javascript" language="javascript">
    Sys.Application.add_load(TaskOptionsDialog_ApplicationLoadHandler);
    var TaskOptionsDialog_ApplicationLoadHandler_Registered = false;

    function TaskOptionsDialog_ApplicationLoadHandler(sender, args) {
        //register TaskOptionsDialog_ApplicationLoadHandler only once
        if (!TaskOptionsDialog_ApplicationLoadHandler_Registered) {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(TaskOptionsDialog_BeginRequest);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(TaskOptionsDialog_EndRequest);
            TaskOptionsDialog_ApplicationLoadHandler_Registered = true;
        }
    }

    function TaskOptionsDialog_BeginRequest(sender, args) {
        //on begin async postback if control that started it is ibtnCustomize from CustomizableTask
        var elem = args.get_postBackElement();
        var TaskOptionsControl = TaskOptionsControlsArray.get(elem.id);
        //change image to show loading process
        if (TaskOptionsControl != null) {
            if (elem.src.lastIndexOf('<%=TaskIcons.OptionsEnabled %>') > -1) {
                elem.src = '<%=TaskIcons.OptionsEnabledLoading %>';
            }
            else if (elem.src.lastIndexOf('<%=TaskIcons.OptionsDisabled %>') > -1) {
                elem.src = '<%=TaskIcons.OptionsDisabledLoading  %>';
            }
            args.get_request().set_userContext(TaskOptionsControl.ibtnCustomizeClientID);
        }
    }

    function TaskOptionsDialog_EndRequest(sender, args) {
        //on end async postback if control that started it is ibtnCustomize from CustomizableTask
        var elemid = args.get_response().get_webRequest().get_userContext();
        var TaskOptionsControl = TaskOptionsControlsArray.get(elemid);
        //change image to show that it was loaded
        if (TaskOptionsControl != null) {
            var elem = document.getElementById(TaskOptionsControl.ibtnCustomizeClientID);
            if (elem.src.lastIndexOf('<%=TaskIcons.OptionsEnabledLoading %>') > -1) {
                elem.src = '<%=TaskIcons.OptionsEnabled %>';
            }
            else if (elem.src.lastIndexOf('<%=TaskIcons.OptionsDisabledLoading  %>') > -1) {
                elem.src = '<%=TaskIcons.OptionsDisabled %>';
            }
            if (!args.get_response().get_aborted()) {
                //show options dialog
                TaskOptionsDialog.show(TaskOptionsControl);
            }
        }
    }

    var TaskOptionsControlsArray = function () {
        //this object stores needed clientIDs for each CustomizableTask control
        var array;
        return {
            add: function (ibtnCustomizeClientID, divOptionsClientID, btnHiddenApplyClientID,
                btnHiddenSaveClientID, btnHiddenSaveAsClientID, hfSaveAsNameClientID, hfUsedNamesClientID,
                hfRestrictedNamesClientID, ddlTasksClientID, hfTemporaryTaskKeyClientID) {
                if (array == undefined) {
                    array = new Array();
                }
                obj = { ibtnCustomizeClientID: ibtnCustomizeClientID, divOptionsClientID: divOptionsClientID,
                    btnHiddenApplyClientID: btnHiddenApplyClientID, btnHiddenSaveClientID: btnHiddenSaveClientID,
                    btnHiddenSaveAsClientID: btnHiddenSaveAsClientID, hfSaveAsNameClientID: hfSaveAsNameClientID,
                    hfUsedNamesClientID: hfUsedNamesClientID, hfRestrictedNamesClientID: hfRestrictedNamesClientID,
                    ddlTasksClientID: ddlTasksClientID, hfTemporaryTaskKeyClientID: hfTemporaryTaskKeyClientID
                };
                array.push(obj);
            },
            get: function (ibtnCustomizeClientID) {
                if (array == undefined) {
                    return null;
                }
                for (var i = 0; i < array.length; i++) {
                    if (array[i].ibtnCustomizeClientID === ibtnCustomizeClientID) {
                        return array[i];
                    }
                }
                return null;
            }
        };
    } ();

    var TaskOptionsDialog = function () {
        //TaskOptionsDialog javascript object
        var dialog = false;
        var CurrentTaskOptionsControl;

        function onApply() {
            if (!validate()) {
                  return;
            }
            dialog.dialog("close");
            var btnHiddenApply = $('#' + CurrentTaskOptionsControl.btnHiddenApplyClientID);
            btnHiddenApply.click();
        }

        function onSave() {

            dialog.dialog("close");
            var btnHiddenSave = $('#' + CurrentTaskOptionsControl.btnHiddenSaveClientID);
            btnHiddenSave.click();
        }

        function onSaveAs() {

            //show save as dialog
            eval('<%= saveAsDialogTask.JavascriptObjectShow %>');
        }

        function validate() {
            if (typeof Page_ClientValidate == 'function') {
                if (!Page_ClientValidate('TaskValidation')) {
                    return false;
                }
            }
            return true;
        }

        function initSaveAsDialog() {
            //init save as dialog restricted and used names
            var restrictedNames = $('#' + CurrentTaskOptionsControl.hfRestrictedNamesClientID).val();
            eval('<%= saveAsDialogTask.JavascriptObject%>.setRestrictedNames(' + restrictedNames + ');');
            var usedNames = $('#' + CurrentTaskOptionsControl.hfUsedNamesClientID).val();
            eval('<%= saveAsDialogTask.JavascriptObject%>.setUsedNames(' + usedNames + ');');
        }
        function showSaveButton() {
            //return false if temporary task value is selected
            var hfTemporaryTaskKey = $('#' + CurrentTaskOptionsControl.hfTemporaryTaskKeyClientID).val();
            var selectedValue = $('#' + CurrentTaskOptionsControl.ddlTasksClientID).val();
            return (hfTemporaryTaskKey != selectedValue);
        }

        return {
            add_TaskOptionsControl: function (ibtnCustomizeClientID, divOptionsClientID, btnHiddenApplyClientID,
                btnHiddenSaveClientID, btnHiddenSaveAsClientID, hfSaveAsNameClientID, hfUsedNamesClientID,
                hfRestrictedNamesClientID, ddlTasksClientID, hfTemporaryTaskKeyClientID) {
                TaskOptionsControlsArray.add(ibtnCustomizeClientID, divOptionsClientID, btnHiddenApplyClientID,
                    btnHiddenSaveClientID, btnHiddenSaveAsClientID, hfSaveAsNameClientID, hfUsedNamesClientID,
                    hfRestrictedNamesClientID, ddlTasksClientID, hfTemporaryTaskKeyClientID);
            },
            get_TaskOptionsControl: function (ibtnCustomizeClientID) {
                return TaskOptionsControlsArray.get(ibtnCustomizeClientID);
            },
            show: function (TaskOptionsControl) {

                CurrentTaskOptionsControl = TaskOptionsControl;
                dialog = $('div#' + TaskOptionsControl.divOptionsClientID).dialog({
                    buttons: [
                        { text: "<%= Resources.Resource.Apply  %>", click: onApply },
                        { text: "<%= Resources.Resource.Save  %>", click: onSave },
                        { text: "<%= Resources.Resource.SaveAs  %>", click: onSaveAs },
                    ],
                    width: 600,
                    draggable: false,
                    modal: true,
                    resizable: false,
                    height: 400,
                    close: function () {
                        dialog.dialog("destroy");
                        $('div#' + TaskOptionsControl.divOptionsClientID).hide();
                    }
                });
                //if (showSaveButton()) {
                //    dialog.addButton('', onSave, dialog);
                //}

                initSaveAsDialog();

            },
            saveCallback: function (name) {
                //save callback function
                dialog.dialog("close");
                var hfSaveAsName = $('#' + CurrentTaskOptionsControl.hfSaveAsNameClientID).val(name);
                var btnHiddenSaveAs = $('#' + CurrentTaskOptionsControl.btnHiddenSaveAsClientID);
                btnHiddenSaveAs.click();
            }
        };
    } ();
</script>
<cc1:SaveAsDialog ID="saveAsDialogTask" runat="server" NameEmptyErrorMessage="<%$ Resources:Resource, ErrorTaskNameEmpty %>"
    NameRestrictedErrorMessage="<%$ Resources:Resource, ErrorTaskNameRestricted %>" 
    NameExistsConfirmRewriteMessage="<%$ Resources:Resource, ConfirmRewriteTask %>" 
    CallbackFunction="TaskOptionsDialog.saveCallback" />
