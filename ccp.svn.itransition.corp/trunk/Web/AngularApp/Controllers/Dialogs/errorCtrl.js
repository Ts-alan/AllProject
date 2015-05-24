CCPApp.controller("errorCtrl", [
    '$scope', '$state', '$modalInstance', 'error',
    function ($scope, $state, $modalInstance, error) {
        $scope.error = error;
        $scope.OK = function () {
            $modalInstance.close();
        };
    }
]);