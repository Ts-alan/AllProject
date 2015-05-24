'use strict';

CCPApp.controller("approvalCtrl", [
    '$scope', '$state', '$stateParams', 'dataService', 'dialogService',
    function ($scope, $state, $stateParams, dataService, dialogService) {
        $scope.loaded = false;
        dataService.get('PendingContracts').success(function (data) {
            $scope.contracts = data;
            angular.forEach($scope.contracts, function (cpr) {
                angular.forEach(cpr.ApproveStatuses, function (status) {
                    showApprovers(status);
                }
                );
            });

            $scope.loaded = true;
        }).error(function () {
            $scope.loaded = true;
        });

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

        $scope.open = function (id) {
                $state.go('CPR', { ContractId: id });
        }
    }
]);