'use strict';

var CCPApp = angular.module('CCPApp', [
     'ngGrid',
     'ngRoute',
     'ui.bootstrap',
     'ui.router',
     'angular-loading-bar',
     'LocalStorageModule',
     'ui.bootstrap.tooltip',
     'ui'
     //Services
]);

CCPApp.config(function ($stateProvider, $urlRouterProvider, $httpProvider, $tooltipProvider, $provide) {

    //todo: delete when fixed version will be enabled
    //fixed angular-ui datepicker issue at https://github.com/angular-ui/bootstrap/commit/42cc3f269bae020ba17b4dcceb4e5afaf671d49b#commitcomment-7475211
    $provide.decorator('dateParser', function ($delegate) {
        var oldParse = $delegate.parse;
        $delegate.parse = function (input, format) {
            if (!angular.isString(input) || !format) {
                return input;
            }
            return oldParse.apply(this, arguments);
        };

        return $delegate;
    });

    $urlRouterProvider.when('/DataAdmin', '/DataAdmin/Users');
    $urlRouterProvider.when('', 'CPRs');
    $urlRouterProvider.otherwise('/Error');
    
    $tooltipProvider.setTriggers({ 'select2-open': 'select2-close' });

    $stateProvider
        .state('Error', {
            url: '/Error',
            templateUrl: 'Error/ClientError'
        })
        .state('NoState', {
            url: ''
        })
        .state('CPRs', {
            url: '/CPRs',
            templateUrl: 'CPR/CPRs'
        })
        .state('MyCPRs', {
            url: '/MyCPRs',
            templateUrl: 'MyCPRs/MyCPRs'
        })
        .state('CPR', {
            url: '/CPRs/CPR/{ContractId}',
            templateUrl: 'CPR/CPR'
        })
        //.state('MessageCenter', {
        //    url: '/MessageCenter',
        //    templateUrl: 'MessageCenter/MessageCenter'
        //})
        //.state('Reports', {
        //    url: '/Reports',
        //    templateUrl: 'CPR/CPRs'
        //})
        .state('ApprovalDashboard', {
            url: '/ApprovalDashboard',
            templateUrl: 'ApprovalDashboard/ApprovalDashboard'
        })
        //.state('ProductMaster', {
        //    url: '/ProductMaster',
        //    templateUrl: 'ProductMaster/ProductMaster'
        //})
        //.state('Profile', {
        //    url: '/Profile',
        //    templateUrl: 'User/Profile'
        //})
        .state('DataAdmin', {
            url: '/DataAdmin',
            templateUrl: 'DataAdmin/DataAdmin',
        })
        .state('DataAdmin.SalesPersons', {
            url: '/SalesPersons',
            templateUrl: 'DataAdmin/SalesPersons'
        })
        .state('DataAdmin.Users', {
            url: '/Users',
            templateUrl: 'DataAdmin/Users'
        })
        .state('Login', {
            url: '/Login',
            templateUrl: 'User/Login'
        })
        .state('LogOut', {
            url: '/Login',
            templateUrl: 'User/Login'
        })
        .state('DataAdmin.SalesPersons.SalesPerson', {
            url: '/SalesPerson/{SalesPersonId}'
        })
        .state('DataAdmin.Users.User', {
            url: '/User/{UserId}', 
            //templateUrl: 'Users/User'
            //onEnter: ['$modal', function ($modal) {
            //    $modal.open({
            //        templateUrl: 'userModal',
            //        controller: 'userCtrl',
            //    });
            //}]
        });
    $httpProvider.interceptors.push('authInterceptorService');
});


CCPApp.run([
    'authService', function (authService) {
        authService.fillAuthData();
    }
]);