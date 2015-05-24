'use strict';

CCPApp.controller('tabsCtrl', ['$scope', '$rootScope', '$state', 'authService', 'localStorageService', 'dialogService', function ($scope, $rootScope, $state, authService, localStorageService, dialogService) {
    $scope.manageTabs = [
    { state: "CPRs", label: 'CPRs', active: false, isVisible: false },
    { state: "MyCPRs", label: 'My CPRs', active: false, isVisible: false },
    //{ link: 'MessageCenter', state: "MessageCenter", label: 'Message Center', active: false },
    { state: "ApprovalDashboard", label: 'Approval Dashboard', active: false, isVisible: false },
    //{ link: 'ProductMaster', state: "ProductMaster", label: 'Product Master', active: false },
    { state: "DataAdmin", label: 'Data Admin', active: false, isVisible: false }
    //{ link: 'Reports', state: "Reports", label: 'Reports', active: false }
    ];

    $scope.loginTabs = [
       // { state: "Profile", label: 'Profile', active: false },
        { state: "LogOut", label: 'Log Out', active: false }
    ];

    $scope.dataAdminTabs = [
    //{ link: 'DataAdmin/SalesPersons', state: "DataAdmin.SalesPersons", label: 'Sales Rep Data', active: false },
    { state: "DataAdmin.Users", label: 'Users Data', active: false, isVisible: true }
    ];

    $scope.isActive = function (tab) {
        return ($state.includes(tab.state)) || (tab.state.indexOf($state.current.name) == 0);
    };

    //$scope.errorType = "404 Not Found";

    //$scope.returnState = "CPRs";

    //$scope.errorMessage = "Sorry, but page are you looking for does not exist";

    $scope.checkTabs= function () {
        $scope.manageTabs.forEach(function(tab) {
            tab.isVisible = authService.isAllowedArea(tab.state);
        });
    };

    var accessDenied = false;
    //$scope.initialize();
    var prevented = false;
    $scope.$on('$stateChangeStart', function(e, toState, toParams, fromState, fromParams) {
        if (authService.isAuthenticated()) {
            if (fromState.name == 'CPR' && toState.name != 'LogOut' && toState.name != 'CPR') {
                if (!prevented) {
                    e.preventDefault();                    
                    dialogService.confirm("exit form CPR?").result.then(function (res) {
                        if (res == 'YES') {
                            prevented = true;
                            $state.go(toState);
                            
                        }

                    });
                } else {
                    prevented = !prevented;
                }
                
            }
            if (toState.name != 'Error' && toState.name != 'LogOut' && toState.name.indexOf('.') < 0 && !authService.isAllowedArea(toState.name)) {
                $scope.errorType = "Access Denied";
                $scope.errorMessage = "Sorry, but you have no permision.";
                $scope.returnState = fromState.name;
                accessDenied = true;
                $state.go('Error');
                accessDenied = false;
                e.preventDefault();
            }
            if (toState.name == 'Error') {
                if (accessDenied == false) {
                    $scope.errorType = "404 Not Found";
                    $scope.errorMessage = "Sorry, but page you are looking for does not exist.";
                    $scope.returnState = fromState.name;
                }
            }
            if (toState.name == 'Login') {
                e.preventDefault();
                $state.go('CPRs');
            }
        } else {
            if (toState.name != 'Login') {
                authService.returnPage = toState.name;
                e.preventDefault();
                $state.go('Login');
            }
        }
    });



    $scope.$on("$stateChangeSuccess", function (e, toState, toParams, fromState, fromParams) {
        
        
        
        $scope.checkTabs();

        $scope.manageTabs.forEach(function(tab) {
            tab.active = $scope.isActive(tab);
        });

        $scope.loginTabs.forEach(function(tab) {
            tab.active = $scope.isActive(tab);
        });

        $scope.dataAdminTabs.forEach(function(tab) {
            tab.active = $scope.isActive(tab);
        });
    });
}]);