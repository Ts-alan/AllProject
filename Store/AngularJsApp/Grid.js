
var App = angular.module('App', ['ngGrid', 'ngRoute']);
App.service('sharedProperties', function() {
    var property = '';

    return {
        getProperty: function() {
            return property;
        },
        setProperty: function(value) {
            property = value+1;
        }
    };
});
App.config(function($routeProvider) {
    $routeProvider.when('/SecondGrid',
    {
        templateUrl: '/Home/SecondGrid',
        controller: 'SecondGrid'
    });
    $routeProvider.when('/',
  {
      templateUrl: '/Home/FirstGrid',
      controller: 'Grid'
  });
});
App.controller('Grid', function ($scope, $http, $log, $location, sharedProperties) {
    $scope.myData = [];
    $http.get("/api/Values").success(function (data) {
        $log.log(data);
        $scope.myData = data;
    });
        
        $scope.gridOptions = {
            data: 'myData',
            rowTemplate: '<div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-Dblclick="transition(row)" ng-class="col.colIndex()" class="ngCell {{col.cellClass}}"><div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }">&nbsp;</div><div ng-cell></div></div>'

        };
        $scope.transition = function(indexRow) {
            
            sharedProperties.setProperty(indexRow.rowIndex);
            $location.url("/SecondGrid");
        };
    });
App.controller('SecondGrid', function ($scope, $http, $log, $location, sharedProperties) {
    $scope.myData = [];
    $http.get("/api/Values/" + sharedProperties.getProperty()).success(function (data) {
        $log.log(data);
        $scope.myData = data;
    });
        $scope.gridOptions1 = {
            data: 'myData'
            
        };
    });