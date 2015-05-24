
'use strict';

CCPApp.controller('applicationCtrl', ['$scope', '$state', 'authService', function ($scope, $state, authService) {
    $scope.isAuthenticated = function () {
        return authService.isAuthenticated();
    }

    $scope.isLoginPage = function() {
        return $state.is('Login') || $state.is('LogOut');
    }
}]);