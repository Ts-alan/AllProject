'use strict';

CCPApp.controller('contractCtrl', ['$scope', '$state', '$stateParams', 'dataService', 'dialogService', 'authService', 'dateParser', 'dateFilter',
    function ($scope, $state, $stateParams, dataService, dialogService, authService, dateParser, dateFilter) {
        $scope.cpr = {};
        $scope.canEdit = true;
        $scope.loaded = false;
        $scope.canReject = false;
        $scope.canCancel = false;
        $scope.canApprove = false;
        $scope.Approvers = [];
        $scope.UserId = 0;
        $scope.UserRole = authService.authentication.role;
        $scope.UserEmail = authService.authentication.userName;

        var id = $stateParams.ContractId;

        function showApprovers(value, index, ar) {
            if (value.ApproveStatusType == null || value.ApproveStatusType == undefined) {
                return;
            }

            if (value.ApproveStatusType.ApproverStatusName.toString().indexOf("Approv") != -1) {
                value.showApproveIcon = true;
            }
            else if ((value.ApproveStatusType.ApproverStatusName.toString().indexOf("Reject") != -1) || (value.ApproveStatusType.ApproverStatusName.toString().indexOf("Skipped") != -1)) {
                value.showRejectIcon = true;
            }
            else {
                value.showIcon = true;
            }
            value.number = String.fromCharCode(8544 + index);
            value.Status = value.ApproveStatusType.ApproverStatusName;
            value.Name = value.ApproverTier.Approver.FirstName + " " + value.ApproverTier.Approver.LastName;
        }

        if (id > 0) {
            dataService.get('Contract', id).success(function (data) {
                if (data != null) {
                    $scope.cpr = data;
                    if ($scope.cpr.EndDate != null) {
                        $scope.cpr.EndDate = dateFilter($scope.cpr.EndDate, 'MM/dd/yyyy');
                    }

                    if ($scope.cpr.StartDate != null) {
                        $scope.cpr.StartDate = dateFilter($scope.cpr.StartDate, 'MM/dd/yyyy');
                    }

                    if (($scope.UserRole == "Admin" ||
                            (data.ApproveStatuses.length > 0))
                        &&
                        //(data.StatusId > 2 && data.StatusId != 6 && data.StatusId != 5)
                        (data.ContractStatusType.ContractStatusTag == "Pen")
                            ) {
                        // && (data.ApproveStatuses[0].ApproverTier.Approver.Email == $scope.UserEmail) && data.)
                        $scope.canApprove = false;
                        angular.forEach(data.ApproveStatuses, function (item) {
                            if ((item.ApproverTier.Approver.Email == $scope.UserEmail || $scope.UserRole == "Admin") && item.ApproveStatusType.ApproverStatusTag == "Pen") {
                                $scope.canApprove = true;
                            }
                        });

                    } else {
                        $scope.canApprove = false;
                    }

                    if (data.ContractStatusType.ContractStatusTag == "Pen" && ($scope.UserRole == "Admin" || data.SalesPerson.Email == $scope.UserEmail)) {
                        $scope.canCancel = true;
                    }


                    if ($scope.UserRole == "Admin" || (data.ContractStatusType.ContractStatusTag == "Dr" || data.ContractStatusType.ContractStatusTag == "Ret")) {
                        if (data.SalesPersonId != null) {
                            if (data.SalesPerson.Email == $scope.UserEmail) {
                                $scope.canEdit = true;
                            } else {
                                if ($scope.UserRole != "Admin") {
                                    $scope.canEdit = false;
                                }
                            }
                        } else {
                            if ($scope.UserRole != "Admin") {
                                $scope.canEdit = false;
                            }
                        }
                    } else {
                        $scope.canEdit = false;
                    }

                    if (data.SalesPersonId != null) {
                        $scope.cpr.initiator = {
                            text: data.SalesPerson.FirstName + ' ' + data.SalesPerson.LastName,
                            id: data.SalesPerson.UserId
                        }
                    }

                    dataService.get('Contract/GetApprovers', id).success(function (data) {
                        $scope.Approvers = data;
                        if ($scope.Approvers != null)
                            if (!$scope.cpr.isInitiatorSelected) {
                                $scope.cpr.isInitiatorSelected = true;
                            }
                        $scope.Approvers.forEach(showApprovers);
                        $scope.loaded = true;
                    }).error(function (error) {
                        $scope.Approvers = [];
                        $scope.error = error;
                    });
                    //$scope.loaded = true;

                }
            }).error(function (error) {
                $scope.error = error;
            });

        }
        else {
            dataService.get('User/SalesPerson').success(function (data) {
                if (data.FirstName != undefined && data.LastName != undefined) {
                    $scope.cpr.initiator = {
                        text: data.FirstName + ' ' + data.LastName,
                        id: data.UserId
                    }
                }
                else {
                    $scope.cpr.initiator = null;
                }
                $scope.loaded = true;
                ////////////////
            });
        }
        var initiators = { results: [] };
        $scope.initiatorOptions = {
            placeholder: "Select Initiator",
            initSelection: function (elem, callback) {
                callback();
            },
            query: function (query) {
                dataService.get('list/initiators', query.term).then(function (res) {
                    initiators.results = [];
                    angular.forEach(res.data, function (user) {
                        initiators.results.push({
                            text: user.FirstName + ' ' + user.LastName,
                            id: user.UserId
                        });
                    });
                    query.callback(initiators);
                });
            }
        }

        $scope.open = function ($event) {
            if ($scope.canEdit) {
                $event.preventDefault();
                $event.stopPropagation();
                $scope.opened2 = false;
                $scope.opened = !$scope.opened;
            }
        };
        $scope.open2 = function ($event) {
            if ($scope.canEdit) {
                $event.preventDefault();
                $event.stopPropagation();
                $scope.opened = false;
                $scope.opened2 = !$scope.opened2;
            }
        };

        $scope.format = 'MM/dd/yyyy';

        $scope.parseDate = function parseDate(viewValue) {
            if (!viewValue) {
                return null;
            } else if (angular.isDate(viewValue) && !isNaN(viewValue)) {
                return dateFilter(viewValue, this.format);
            } else if (angular.isString(viewValue)) {
                return viewValue;
            } else {
                return viewValue;
            }
        }

        $scope.$watch('cpr.EndDate', function (newVal, oldVal) {
            if (newVal != undefined && newVal != oldVal) {
                $scope.cpr.EndDate = $scope.parseDate(newVal);
            }
        });

        $scope.$watch('cpr.StartDate', function (newVal, oldVal) {
            if (newVal != undefined && newVal != oldVal) {
                $scope.cpr.StartDate = $scope.parseDate(newVal);
            }
        });

        if (($scope.UserRole == 'Approver' || $scope.UserRole == 'Admin')) {
            if (id > 0) {
                dataService.get('ApproveStatus', id).success(function (data) {
                    if ($scope.UserRole == 'Approver')
                        if (data.ApproverTier != null) {
                            data.ApproverTier.Approver.Email == $scope.UserEmail ? $scope.canReject = true : $scope.canReject = false;
                        } else {
                            $scope.canReject = false;
                        }
                    if ($scope.UserRole == 'Admin')
                        if (data.ApproverTier != null)
                            data.ApproverTier.Approver != null && data.ApproveStatusType.ApproverStatusTag == "Pen" ? $scope.canReject = true : $scope.canReject = false;
                        else {
                            $scope.canReject = false;
                        }
                }).error(function () {
                    $scope.canReject = false;
                });
            }

        }

        $scope.$watch('cpr.initiator', function (newVal, oldVal) {
            if ($scope.loaded) {
                if (newVal != null && newVal !== oldVal) {
                    dataService.get('Contract/GetInitApprovers', newVal.id).success(function (data) {
                        $scope.Approvers = data;
                        if ($scope.Approvers != null)
                            if (!$scope.cpr.isInitiatorSelected) {
                                $scope.cpr.isInitiatorSelected = true;
                            }
                        $scope.Approvers.forEach(showApprovers);
                        $scope.loaded = true;
                    }).error(function (error) {
                        $scope.Approvers = [];
                        $scope.error = error;
                    });
                }
            }
        }, true);

        $scope.save = function () {
            dialogService.confirm("save CPR").result.then(function (res) {
                if (res == "YES") {
                    if ($scope.cpr.initiator) {
                        $scope.cpr.SalesPersonId = $scope.cpr.initiator.id;
                    }
                    //$scope.cpr.SalesPerson = null;
                    if ($scope.cpr.StartDate) {

                        $scope.cpr.StartDate = $scope.cpr.StartDate;
                    }
                    if ($scope.cpr.EndDate) {

                        $scope.cpr.EndDate = $scope.cpr.EndDate;
                    }
                    dataService.post('save/contract', $scope.cpr).success(function () {
                        $state.go('CPRs');
                    }).error(function (errors) {
                        if (errors.LogincError != null) {
                            dialogService.error(errors.LogicError);
                        }

                        if (errors.DataErrors != null) {
                            $scope.dataErrors = errors.DataErrors
                        }
                    });
                }
            });

        }
        $scope.submit = function () {
            dialogService.confirm("submit CPR").result.then(function (res) {
                if (res == "YES") {
                    if ($scope.cpr.initiator) {
                        $scope.cpr.SalesPersonId = $scope.cpr.initiator.id;
                    }
                    //$scope.cpr.SalesPerson = null;
                    dataService.post('submit/contract', $scope.cpr).success(function () {
                        $state.go('CPRs');
                    }).error(function (errors) {
                        if (errors.LogincError != null) {
                            dialogService.error(errors.LogicError);
                        }

                        if (errors.DataErrors != null) {
                            $scope.dataErrors = errors.DataErrors;
                        }
                    });
                }
            });


        }
        $scope.cancel = function () {
            dialogService.confirm("cancel CPR").result.then(function (res) {
                if (res == "YES") {
                    if ($scope.UserRole == 'Initiator' || $scope.UserRole == 'Admin') {
                        dataService.get('cancel/contract', $scope.cpr.ContractId).success(function () {
                            $state.go('CPRs');
                        }

                        ).error(function () {
                            $scope.workflowAction = null;
                        });
                    }
                } else if (res == "NO") {
                    $scope.workflowAction = null;
                }
            });
        }
        // $scope.reject = function (cpr) {
        //     dataService.post('reject/contract', cpr).success(function () {
        //         $state.go('CPRs');
        //     }).error(function (errorMsg) {
        //         dialogService.error(errorMsg);
        //     });
        // }
        $scope.back = function () {
            if (document.referrer != "") {
                history.back();
            }
        }
        var workflow = {
            results: [
                { id: 1, text: "Approve" },
                { id: 2, text: "Reject" }
            ]
        };
        $scope.workflowOptions = {
            minimumResultsForSearch: -1,
            placeholder: "Workflow actions",
            initSelection: function (elem, callback) {
                callback();
            },
            data: workflow
        }
        $scope.approve = function () {
            dialogService.confirm("approve CPR").result.then(function (res) {
                if (res == "YES") {
                    dataService.get('Contract/Approve', $scope.cpr.ContractId).success(function () {
                        $state.go('CPRs');
                    }).error(function () {
                        $scope.workflowAction = null;
                    });
                } else if (res == "NO") {
                    $scope.workflowAction = null;
                }
            });
        }

        $scope.reject = function () {
            dialogService.confirm("reject CPR").result.then(function (res) {
                if (res == "YES") {
                    if ($scope.UserRole == 'Approver' || $scope.UserRole == 'Admin') {
                        dataService.get('Contract/Reject', $scope.cpr.ContractId).success(function () {
                            $state.go('CPRs');
                        }

                        ).error(function () {
                            $scope.workflowAction = null;
                        });
                    }
                } else if (res == "NO") {
                    $scope.workflowAction = null;
                }
            });
        }
        $scope.return = function () {
            dialogService.confirm("return CPR").result.then(function (res) {
                if (res == "YES") {
                    if ($scope.UserRole == 'Approver' || $scope.UserRole == 'Admin') {
                        dataService.get('return/contract', $scope.cpr.ContractId).success(function () {
                            $state.go('CPRs');
                        }

                        ).error(function () {
                            $scope.workflowAction = null;
                        });
                    }
                } else if (res == "NO") {
                    $scope.workflowAction = null;
                }
            });
        }

        //  $scope.$watch('workflowAction', function (newVal, oldVal) {
        //      if (newVal !== undefined && newVal !== null && newVal.text == "Approve") {
        //          dialogServiceOfChangeStatusOfCPR.confirm("approve").result.then(function (res) {
        //              if (res == "YES") {
        //                  dataService.get('Contract/Approve', $scope.cpr.ContractId).success(function () {
        //                      $state.go('CPRs');
        //                  }).error(function () {
        //                      $scope.workflowAction = null;
        //                  });
        //              } else if (res == "NO") {
        //                  $scope.workflowAction = null;
        //              }
        //          });
        //      }
        //      if (newVal !== undefined && newVal !== null && newVal.text == "Reject") {
        //          dialogServiceOfChangeStatusOfCPR.confirm("reject").result.then(function (res) {
        //              if (res == "YES") {
        //                  if ($scope.UserRole == 'Approver' || $scope.UserRole == 'Admin')
        //                 {
        //                      dataService.get('Contract/Reject', $scope.cpr.ContractId).success(function ()
        //                      {
        //                          $state.go('CPRs');
        //                      }
        //
        //          ).error(function () {
        //                $scope.workflowAction = null;
        //              });
        //            }
        //          } else if (res == "NO") {
        //                $scope.workflowAction = null;
        //              }
        //            });
        //          }
        //        }, true);
    }
]);