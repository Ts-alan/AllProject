'use strict';

CCPApp.controller('authCtrl', ['$scope', '$state', '$window', 'authService', 'dataService','dialogService', function ($scope, $state, $window, authService, dataService, dialogService) {
    

    $scope.loginData = {
        Email: "",
        Password: "",
    };
    $scope.currentUserName = "";
    $scope.firstName = "";
    $scope.lastName = "";

    $scope.message = "";
    $scope.emailRequired = '';
    $scope.passwordRequired = '';
    $scope.disableLogin = false;

    $scope.resetErrors = function () {
        $scope.message = "";
        $scope.emailRequired = "";
        $scope.passwordRequired = "";
    }

    $scope.refreshToken = function () {
        authService.refreshToken();
    }

    $scope.login = function () {

        if (!$scope.loginData.Email || !$scope.loginData.Password) {
            if (!$scope.loginData.Email) {
                $scope.emailRequired = '<span class="error">Login Required</span>';
            }

            if (!$scope.loginData.Password) {
                $scope.passwordRequired = '<span class="error">Password Required</span>';
            }
            return;
        }


        $scope.disableLogin = true;
        authService.login($scope.loginData).then(function () {
            $scope.currentUserName = authService.authentication.userName;
            $scope.firstName = authService.authentication.firstName;
            $scope.lastName = authService.authentication.lastName;
            if (authService.returnPage) {
                if (authService.returnPage != 'LogOut') {
                    $state.go(authService.returnPage);
                } else {
                    $state.go('CPRs');
                }
                authService.returnPage = "";
            } else {
                $state.go("CPRs");
            }
            $scope.loginData = {};
        },
         function (err) {
             $scope.message = err.error_description;
         });
        $scope.disableLogin = false;
    };

    $scope.logOut = function () {
        dialogService.confirm("logout").result.then(function (res) {
            if (res == "YES") {
                authService.logOut();
                $state.go('Login');
            }
        });
    }

    $scope.isAuthenticated = function () {
        return authService.isAuthenticated();
    }

    $scope.$watch('loginData', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.resetErrors();
        }
    }, true);

        $scope.currentUserName = authService.authentication.userName;
        $scope.firstName = authService.authentication.firstName;
        $scope.lastName = authService.authentication.lastName;
    
   
}]);