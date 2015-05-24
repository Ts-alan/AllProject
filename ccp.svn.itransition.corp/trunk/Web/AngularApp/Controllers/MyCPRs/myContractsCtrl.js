'use strict';

CCPApp.controller('myContractsCtrl', ['$scope', '$state', 'dataService', 'gridService', 'authService', 'localStorageService',
    function ($scope, $state, dataService, gridService, authService, localStorageService) {
        $scope.user = authService.authentication;
        $scope.isLoaded = true;
        console.log();
        if (localStorageService.get('MyCPRsFilters') == null) {
            $scope.filter = {
                selectedStatus: {
                    text: 'All',
                    id: 0
                }
            }
        } else {
            $scope.filter = localStorageService.get('MyCPRsFilters');
        }

        function getContracts() {
            $scope.isLoaded = false;
            dataService.post('MyContracts', { email: $scope.user.userName, statusId: $scope.filter.selectedStatus.id }).success(function (result) {
                $scope.contracts = result.reverse();
                angular.forEach($scope.contracts, function (cpr) {
                    angular.forEach(cpr.ApproveStatuses, function (status) {
                        showApprovers(status);
                        if (status.ApproveStatusType.ApproverStatusTag == "Pen") {
                            cpr.pendingTier = "(Pending Approver - " + status.ApproverTier.Approver.FirstName + " " + status.ApproverTier.Approver.LastName + ")";
                        }
                    });
                });
            });
        }

        getContracts();
        $scope.format = 'MM/dd/yyyy';

        $scope.open = function (id) {
            if (id != undefined) {
                $state.go('CPR', { ContractId: id });
            } else {
                $state.go('CPR');
            }
        }



        function showApprovers(value, index, ar) {
            if (value.ApproveStatusType == null || value.ApproveStatusType == undefined) {
                return;
            }

            if (value.ApproveStatusType.ApproverStatusName.toString().indexOf("Approv") != -1) {
                value.showApproveIcon = true;
            } else if ((value.ApproveStatusType.ApproverStatusName.toString().indexOf("Reject") != -1) || (value.ApproveStatusType.ApproverStatusName.toString().indexOf("Skipped") != -1)) {
                value.showRejectIcon = true;
            } else {
                value.showIcon = true;
            }
            value.number = String.fromCharCode(8544 + index);
            value.Status = value.ApproveStatusType.ApproverStatusName;
            value.Name = value.ApproverTier.Approver.FirstName + " " + value.ApproverTier.Approver.LastName;
        }

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

        $scope.statusOptions = getSelectOptions("statuses", "ContractStatusName", "ContractStatusId");

        $scope.$watch('filter.selectedStatus', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                getContracts();
                localStorageService.set('MyCPRsFilters', $scope.filter);
            }
        }, true);

    }
]);
