var TaskHelper = function () {
    return {
        EnableTaskPanelActionButtons: function () {
            if (TaskPanel != undefined) { TaskPanel.EnableActionButtons() };
        },
        DisableTaskPanelActionButtons: function () {
            if (TaskPanel != undefined) { TaskPanel.DisableActionButtons() };
        }
    };
} ();