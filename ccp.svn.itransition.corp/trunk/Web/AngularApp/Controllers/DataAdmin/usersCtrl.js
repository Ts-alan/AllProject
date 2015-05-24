'use strict';

CCPApp.controller('usersCtrl', ['$scope', '$modal', '$state', 'dataService', 'gridService','localStorageService',
    function ($scope, $modal, $state, dataService, gridService, localStorageService) {
        gridService.getDefaultsPagingOptions();
        gridService.getDefaultsSortingOptions();
        $scope.pagingOptions = gridService.pagingOptions;
        $scope.sortInfo = gridService.sortInfo;
        $scope.filteringOptions = [];
        $scope.fieldsToSearch = [
            ['FirstName'], ['LastName'], ['Email'], ['Role.RoleName']
        ];

        if (localStorageService.get('usersPagingOptions') != null) {
            $scope.pagingOptions = localStorageService.get('usersPagingOptions');
        }
        if (localStorageService.get('usersFilteringOptions') != null) {
            $scope.filteringOptions = localStorageService.get('usersFilteringOptions');
        }
        if (localStorageService.get('usersFreeText') != null) {
            $scope.freeText = localStorageService.get('usersFreeText');
        }
        function getUsers() {
           
            var params = gridService.getDataSource($scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize, $scope.sortInfo, $scope.filteringOptions);
            dataService.post('users', params).success(function (result) {
                $scope.data = result.Data;
                $scope.totalServerItems = result.DataCount;
            });
        }

        getUsers();
        $scope.$watch('pagingOptions', function (newVal, oldVal) {
            if (newVal !== oldVal && newVal.pageSize != oldVal.pageSize) {
                $scope.pagingOptions.currentPage = 1;
            }
            if (newVal !== oldVal || newVal.currentPage != oldVal.currentPage) {
                getUsers();
            }
            localStorageService.set('usersPagingOptions', $scope.pagingOptions);
        }, true);

        $scope.$watch('sortInfo', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.sortInfo = newVal;
                getUsers();

            }
        }, true);

        gridService.gridOptions.columnDefs = [
            { field: 'FirstName', displayName: 'First Name' },
            { field: 'LastName', displayName: 'Last Name' },
            { field: 'Email', displayName: 'Email' },
            { field: 'Role.RoleName', displayName: 'Role' }
        ];


        $scope.gridOptions = gridService.gridOptions;

        $scope.open = function (row) {
            if (row != undefined) {
                $state.go('DataAdmin.Users.User', { UserId: row.entity.UserId });
            } else {
                $state.go('DataAdmin.Users.User');
            }
            $modal.open({
                templateUrl: 'userModal',
                controller: 'userCtrl',
            })
            .result.then(function () {
                getUsers();
            });
        };

        $scope.$watch('freeText', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                if (newVal.length > 2) {
                    $scope.pagingOptions.currentPage = 1;
                    $scope.freeText = newVal;
                    $scope.fieldsToSearch.forEach(function (item) {
                        var index = -1;
                        $scope.filteringOptions.forEach(function (filteringOption, i) {
                            filteringOption.filterFieldNames.forEach(function (fieldName) {
                                item.forEach(function (itemFilterFieldName) {
                                    if (fieldName == itemFilterFieldName && filteringOption.filterOperation == 'contain') {
                                        index = i;
                                        return;
                                    }
                                });

                            });
                        });
                        if (index > -1) {
                            $scope.filteringOptions.splice(index, 1);
                        }
                    });
                    $scope.fieldsToSearch.forEach(function (item) {
                        $scope.filteringOptions.push(
                        {
                            filterFieldNames: item,
                            filterValue: newVal,
                            filterOperation: 'contain'
                        });
                    });
                    getUsers();
                } else {
                    var isCleared = true;
                    $scope.fieldsToSearch.forEach(function (item) {
                        var index = -1;
                        $scope.filteringOptions.forEach(function (filteringOption, i) {
                            filteringOption.filterFieldNames.forEach(function (fieldName) {
                                item.forEach(function (itemFilterFieldName) {
                                    if (fieldName == itemFilterFieldName && filteringOption.filterOperation == 'contain') {
                                        index = i;
                                        isCleared = false;
                                        return;
                                    }
                                });

                            });
                        });
                        if (index > -1) {
                            $scope.filteringOptions.splice(index, 1);
                        }
                    });
                    if (!isCleared) {
                        getUsers();
                    }
                }
                localStorageService.set('usersFreeText', newVal);
                localStorageService.set('usersFilteringOptions', $scope.filteringOptions);
            }
        }, true);
}]);


