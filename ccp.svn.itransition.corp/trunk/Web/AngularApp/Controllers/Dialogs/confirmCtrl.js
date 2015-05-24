CCPApp.controller("confirmCtrl", [
    '$scope', '$state', '$modalInstance', 'msg',
    function ($scope, $state, $modalInstance, msg) {
        $scope.msg = "Are you sure you want to " + msg + "?";

        $scope.yes = function() {
            $modalInstance.close('YES');
        }
        $scope.no = function () {
            $modalInstance.close('NO');
        }

    }
]);