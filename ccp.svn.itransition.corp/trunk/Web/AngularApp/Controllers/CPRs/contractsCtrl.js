'use strict';

CCPApp.controller('contractsCtrl', ['$scope', '$state', 'dataService', 'gridService', 'authService', 'localStorageService',
    function ($scope, $state, dataService, gridService, authService, localStorageService) {
        $scope.canCreate = authService.authentication.role != "Approver";
        gridService.getDefaultsPagingOptions();
        gridService.getDefaultsSortingOptions();
        if (localStorageService.get('CPRsPaging') == null) {
            $scope.pagingOptions = gridService.pagingOptions;
        } else {
            $scope.pagingOptions = localStorageService.get('CPRsPaging');
        }
        $scope.sortInfo = gridService.sortInfo;
        $scope.isLoaded = true;
        $scope.filteringOptions = [];
        $scope.fieldsToSearch = [
            ['Summary'], ['CPRNumber'],
            ['StartDate'],
            ['EndDate'], ['ContractStatusType.ContractStatusName'], ['SalesPerson.FirstName', 'SalesPerson.LastName'], ['TDGA']
        ];
        function getContracts() {
            $scope.isLoaded = false;
            var params = gridService.getDataSource($scope.pagingOptions.currentPage, $scope.pagingOptions.pageSize, $scope.sortInfo, $scope.filteringOptions);
            dataService.post('contracts', params).success(function (result) {
                $scope.data = result.Data;
                $scope.totalServerItems = result.DataCount;
                $scope.isLoaded = true;
            });
        }

        if (localStorageService.get('CPRsFilters') == null) {
            $scope.filter = {
                freeText: "",
                selectedSalesPerson: {
                    text: 'All',
                    firstName: '',
                    lastName: '',
                    id: 0
                },
                selectedStatus: {
                    text: 'All',
                    id: 0
                },
                startDate: "",
                endDate: ""
            };
        } else {
            $scope.filter = localStorageService.get('CPRsFilters');
            if (localStorageService.get('CPRsFilteringOptions') != null) {
                $scope.filteringOptions = localStorageService.get('CPRsFilteringOptions');
            }
        }
        getContracts();


        $scope.$watch('pagingOptions', function (newVal, oldVal) {
            if (newVal !== oldVal && newVal.pageSize != oldVal.pageSize) {
                $scope.pagingOptions.currentPage = 1;
            }
            if (newVal !== oldVal || newVal.currentPage != oldVal.currentPage) {
                getContracts();
            }
        }, true);

        $scope.$watch('sortInfo', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.sortInfo = newVal;
                getContracts();
            }
        }, true);



        gridService.gridOptions.columnDefs = [
                { field: 'CPRNumber', displayName: 'CPR number' },
                //{ field: 'Summary', displayName: 'Summary' },
                { field: 'StartDate', displayName: 'Start Date', cellFilter: 'date:\'MM/dd/yyyy\'' },
                { field: 'EndDate', displayName: 'End Date', cellFilter: 'date:\'MM/dd/yyyy\'' },
                { displayName: 'Sales Person', field: 'SalesPerson.FirstName', cellTemplate: '<div class="ngCellText colt{{$index}}">{{row.entity.SalesPerson.FirstName + " " + row.entity.SalesPerson.LastName}}</div>' },
                { field: 'ContractStatusType.ContractStatusName', displayName: 'Status' },
                { field: 'TDGA', displayName: 'TDGA' }
        ];

        $scope.gridOptions = gridService.gridOptions;

        function getSelectOptions(modelName, fieldNameToText, fieldNameToId) {
            return {
                placeholder: "",
                initSelection: function (element, callback) {
                    callback($(element).data('$ngModelController').$modelValue);
                },
                query: function (query) {
                    dataService.get('list/' + modelName, query.term).then(function (res) {
                        var all = "All";
                        var object = { results: [] };

                        if (query.term === "" || all.toLowerCase().indexOf(query.term.toLowerCase()) > -1) {
                            object.results.push({
                                text: 'All',
                                id: 0
                            });
                        }
                        angular.forEach(res.data, function (c) {
                            object.results.push({
                                text: c[fieldNameToText],
                                id: c[fieldNameToId]
                            });
                        });
                        query.callback(object);
                    });
                }
            }
        }

        //salesPerson select
        $scope.salesPersonOptions = {
            placeholder: "",
            initSelection: function (element, callback) {
                callback($(element).data('$ngModelController').$modelValue);
            },
            query: function (query) {
                dataService.get('list/salespersons', query.term).then(function (res) {
                    var all = "All";
                    var object = { results: [] };

                    if (query.term === "" || all.toLowerCase().indexOf(query.term.toLowerCase()) > -1) {
                        object.results.push({
                            text: 'All',
                            id: 0
                        });
                    }
                    angular.forEach(res.data, function (c) {
                        object.results.push({
                            text: c['FirstName'] + ' ' + c['LastName'],
                            firstName: c['FirstName'],
                            lastName: c['LastName'],
                            id: c['UserId']
                        });
                    });
                    query.callback(object);
                });
            }
        };


        $scope.$watch('filter.selectedSalesPerson', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.pagingOptions.currentPage = 1;
                $scope.selectedCustomer = newVal;
                addFilterItem(['SalesPerson.FirstName'], newVal.firstName, 'equal');
                addFilterItem(['SalesPerson.LastName'], newVal.lastName, 'equal');
                getContracts();
                localStorageService.set('CPRsFilters', $scope.filter);
                localStorageService.set('CPRsFilteringOptions', $scope.filteringOptions);
            }
        }, true);

        //endUserSelect

        function addFilterItem(filterFieldNames, filterValue, filterOperation) {
            var index = -1;
            $scope.filteringOptions.forEach(function (item, i) {
                item.filterFieldNames.forEach(function (itemFilterFieldName) {
                    filterFieldNames.forEach(function (filterFieldName) {
                        if (itemFilterFieldName == filterFieldName && item.filterOperation == filterOperation) {
                            index = i;
                            return;
                        }
                    });
                });
            });
            if (index > -1) {
                $scope.filteringOptions.splice(index, 1);
            }
            if (filterValue != "All") {
                $scope.filteringOptions.push({
                    filterFieldNames: filterFieldNames,
                    filterValue: filterValue,
                    filterOperation: filterOperation
                });
            }

        }


        //$scope.endUserOptions = getSelectOptions("endUsers", "EndUserName", "EndUserId");


        //$scope.selectedEndUser = {
        //    text: 'All',
        //    id: 0
        //};

        //$scope.$watch('selectedEndUser', function (newVal, oldVal) {
        //    if (newVal !== oldVal) {
        //        $scope.pagingOptions.currentPage = 1;
        //        $scope.selectedEndUser = newVal;
        //        addFilterItem('EndUser.EndUserName', newVal.text, 'equal');
        //        getContracts();
        //    }
        //}, true);

        //StatusSelect


        $scope.statusOptions = getSelectOptions("statuses", "ContractStatusName", "ContractStatusId");




        $scope.$watch('filter.selectedStatus', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.pagingOptions.currentPage = 1;
                $scope.selectedCustomer = newVal;
                addFilterItem(['ContractStatusType.ContractStatusName'], newVal.text, 'equal');
                getContracts();
                localStorageService.set('CPRsFilters', $scope.filter);
                localStorageService.set('CPRsFilteringOptions', $scope.filteringOptions);
            }
            //if (newVal == undefined) {
            //    var index = -1;
            //    $scope.filteringOptions.forEach(function (item, i) {
            //        item.filterFieldNames.forEach(function (fieldName) {
            //            if (fieldName == 'ContractStatusType.ContractStatusName' && i.filterOperation == "equal") {
            //                index = i;
            //                return;
            //            }
            //        });
            //    });
            //    if (index > -1) {
            //        $scope.filteringOptions.splice(index, 1);
            //        getContracts();
            //    }
            //} else {
            //    $scope.pagingOptions.currentPage = 1;
            //    $scope.selectedStatus = newVal;
            //    addFilterItem(['ContractStatusType.ContractStatusName'], newVal.text, 'equal');
            //    getContracts();
            //}
        }, true);

        ///free text

        $scope.$watch('filter.freeText', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                if (newVal.length > 1) {
                    $scope.pagingOptions.currentPage = 1;
                    $scope.filter.freeText = newVal;
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
                    getContracts();
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
                        getContracts();
                    }
                }
                localStorageService.set('CPRsFilters', $scope.filter);
                localStorageService.set('CPRsFilteringOptions', $scope.filteringOptions);
            }
        }, true);

        ///datepicker

        $scope.$watch('filter.startDate', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                if (newVal == undefined) {
                    var index = -1;
                    $scope.filteringOptions.forEach(function (item, i) {
                        item.filterFieldNames.forEach(function (fieldName) {
                            if (fieldName == 'StartDate' && i.filterOperation == "greaterThan") {
                                index = i;
                                return;
                            }
                        });
                    });
                    if (index > -1) {
                        $scope.filteringOptions.splice(index, 1);
                        getContracts();
                    }
                } else {
                    $scope.pagingOptions.currentPage = 1;
                    $scope.filter.startDate = newVal;
                    addFilterItem(['StartDate'], $scope.filter.startDate, 'greaterThan');
                    getContracts();
                }
                localStorageService.set('CPRsFilters', $scope.filter);
                localStorageService.set('CPRsFilteringOptions', $scope.filteringOptions);
            }
        }, true);


        $scope.$watch('filter.endDate', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                if (newVal == undefined) {
                    var index = -1;
                    $scope.filteringOptions.forEach(function (item, i) {
                        item.filterFieldNames.forEach(function (fieldName) {
                            if (fieldName == 'EndDate') {
                                index = i;
                                return;
                            }
                        });
                    });
                    if (index > -1) {
                        $scope.filteringOptions.splice(index, 1);
                        getContracts();
                    }
                } else {
                    $scope.pagingOptions.currentPage = 1;
                    $scope.filter.endDate = newVal;
                    addFilterItem(['EndDate'], $scope.filter.endDate, 'lessThan');
                    getContracts();
                }
                localStorageService.set('CPRsFilters', $scope.filter);
                localStorageService.set('CPRsFilteringOptions', $scope.filteringOptions);
            }
        }, true);


        $scope.datePicker = {
            opened: false,
            opened2: false
        };

        $scope.clear = function () {
            $scope.dt = null;
        };

        $scope.toggleMin = function () {
            $scope.minDate = $scope.minDate ? null : new Date();
        };
        $scope.toggleMin();

        $scope.startDateOpen = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.datePicker.opened2 = false;
            $scope.datePicker.opened = !$scope.datePicker.opened;
        };

        $scope.endDateOpen = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.datePicker.opened = false;
            $scope.datePicker.opened2 = !$scope.datePicker.opened2;
        };

        $scope.format = 'MM/dd/yyyy';

        $scope.open = function (row) {
            if (row != undefined) {
                $state.go('CPR', { ContractId: row.entity.ContractId });
            } else {
                $state.go('CPR');
            }
        }
        $scope.status = {
            open: true
        };

        $scope.$watch('pagingOptions', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                localStorageService.set('CPRsPaging', $scope.pagingOptions);
            }
        }, true);
    }
]);
