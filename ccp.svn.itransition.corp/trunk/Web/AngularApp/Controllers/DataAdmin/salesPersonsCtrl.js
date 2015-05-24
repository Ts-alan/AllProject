'use strict';


CCPApp.controller('salesPersonsCtrl', ['$scope', '$modal', '$state', 'dataService', 'gridService',
    function ($scope, $modal, $state, dataService, gridService) {
        gridService.getDefaultsPagingOptions();
        gridService.getDefaultsSortingOptions();
        $scope.pagingOptions = gridService.pagingOptions;
        $scope.sortInfo = gridService.sortInfo;
        $scope.filteringOptions = [];
        $scope.fieldsToSearch = [
            'SalesPersonName', 'Email', 'InternalSalesPersonId', 'ContractPrefix', 'SalesPersonNumber', 'Region','Status'
        ];
        function getSalesPersons() {
            var params = gridService.getDataSource($scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize, $scope.sortInfo, $scope.filteringOptions);
            dataService.post('SalesPersons', params).success(function (result) {
                $scope.data = result.Data;
                $scope.totalServerItems = result.DataCount;
            });
        }

        getSalesPersons();

        $scope.$watch('pagingOptions', function (newVal, oldVal) {
            if (newVal !== oldVal && newVal.pageSize != oldVal.pageSize) {
                $scope.pagingOptions.currentPage = 1;
            }
            if (newVal !== oldVal || newVal.currentPage != oldVal.currentPage) {
                getSalesPersons();
            }

        }, true);

        $scope.$watch('sortInfo', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.sortInfo = newVal;
                getSalesPersons();

            }
        }, true);

        gridService.gridOptions.columnDefs = [
            { field: 'SalesPersonName', displayName: 'Name' },
            { field: 'Email', displayName: 'Email' },
            { field: 'InternalSalesPersonId', displayName: 'User ID' },
            { field: 'ContractPrefix', displayName: 'Contract Prefix' },
            { field: 'SalesPersonNumber', displayName: 'Sales Person Number' },
            { field: 'Region', displayName: 'Region' },
            { field: 'Status', displayName: 'Status' }
        ];
        $scope.gridOptions = gridService.gridOptions;

        $scope.open = function (row) {
            if (row != undefined) {
                $state.go('DataAdmin.SalesPersons.SalesPerson', { SalesPersonId: row.entity.SalesPersonId });
            } else {
                $state.go('DataAdmin.SalesPersons.SalesPerson');
            }
            $modal.open({
                templateUrl: 'salesPerson',
                controller: 'salesPersonCtrl'
            })
            .result.then(function () {
                getSalesPersons();
            });
        };

        $scope.$watch('freeText', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.pagingOptions.currentPage = 1;
                if (newVal.length > 2) {
                    $scope.freeText = newVal;
                    $scope.fieldsToSearch.forEach(function (item) {
                        var index = -1;
                        $scope.filteringOptions.forEach(function (filteringOption, i) {
                            if (filteringOption.fieldName == item && filteringOption.operation == 'contain') {
                                index = i;
                                return;
                            }
                        });
                        if (index > -1) {
                            $scope.filteringOptions.splice(index, 1);
                        }
                    });
                    $scope.fieldsToSearch.forEach(function (item) {
                        $scope.filteringOptions.push(
                        {
                            fieldName: item,
                            value: newVal,
                            operation: 'contain'
                        });
                    });
                    getSalesPersons();
                } else {
                    var isCleared = true;
                    $scope.fieldsToSearch.forEach(function (item) {
                        var index = -1;
                        $scope.filteringOptions.forEach(function (filteringOption, i) {
                            if (filteringOption.fieldName == item && filteringOption.operation == 'contain') {
                                index = i;
                                isCleared = false;
                                return;
                            }
                        });
                        if (index > -1) {
                            $scope.filteringOptions.splice(index, 1);
                        }
                    });
                    if (!isCleared) {
                        getSalesPersons();
                    }
                }

            }
        }, true);
    }]);

