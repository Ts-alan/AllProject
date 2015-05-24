'use strict';

CCPApp.controller("userCtrl", ['$scope', '$state', '$stateParams', '$modalInstance', 'dataService', 'dialogService',
    function ($scope, $state, $stateParams, $modalInstance, dataService, dialogService) {
        var id = $stateParams.UserId;
        $scope.loaded = false;
        $scope.loaderstyle = 'loader';
        $scope.user = {};
        var userFormat = function (user) {
            return {
                text: user.FirstName + ' ' + user.LastName,
                id: user.UserId
            }
        }
        var roleFormat = function (role) {
            return {
                text: role.RoleName,
                id: role.RoleId
            }
        }
        var setDefaultApprovers = function () {
            $scope.user.approvers = [];
            for (var i = 0; i < tiersCount; i++) {
                $scope.user.approvers.push({});
            }
        }

        var tiersCount;
        dataService.get('User/TiersCount').success(function (tiers) {
            tiersCount = tiers;
            if (id > 0) {
                dataService.get('users', id)
                    .success(function (data) {
                        $scope.user = data;
                        setDefaultApprovers();
                        if (data.Approvers.length == tiersCount) {
                            $scope.user.approvers = [];
                            $scope.user.isSalesPerson = true;
                            angular.forEach(data.Approvers, function (tier) {
                                $scope.user.approvers.push({
                                    model: userFormat(tier.Approver)
                                });
                            });
                        }
                        $scope.user.Role = roleFormat(data.Role);
                        $scope.loaderstyle = '';
                        $scope.loaded = true;
                    })
                    .error(function () {
                        $scope.error = "User not found";
                        $scope.loaderstyle = '';
                        $scope.loaded = true;
                    });
            } else {
                $scope.loaderstyle = '';
                $scope.loaded = true;
                setDefaultApprovers();
            }
        })
        .error(function () {
            $scope.loaderstyle = '';
            $scope.loaded = true;
        });
        $scope.error = "";
        $scope.save = function () {
            dialogService.confirm("save user").result.then(function (res) {
                if (res == "YES") {

                    var item = angular.copy($scope.user);

                    if (item.Role) {
                        item.RoleId = item.Role.id;
                        item.Role = null;
                    }
                    if (item.isSalesPerson) {
                        var salesPerson =
                        {
                            User: item,
                            ApproverTiers: []
                        }
                        var tier = 1;
                        angular.forEach(item.approvers, function(approver) {
                                salesPerson.ApproverTiers.push(
                                {
                                    SalesPersonId: item.UserId,
                                    ApproverId: approver.model.id,
                                    TierId: tier++
                                });
                        });
                        dataService.post('User', salesPerson).success(function() {
                            $modalInstance.close();
                            $state.go('^');
                        }).error(function(errors) {
                            if (errors.DataErrors != null) {
                                $scope.dataErrors = errors.DataErrors;
                            }
                            if (errors.LogicError != null) {
                                dialogService.error(errors.LogicError);
                            }
                        });

                    } else {
                        var notSalesPerson = {
                            User: item
                        }
                        dataService.post('User', notSalesPerson).success(function() {
                            $modalInstance.close();
                            $state.go('^');
                        }).error(function(errors) {
                            if (errors.DataErrors != null) {
                                $scope.dataErrors = errors.DataErrors;
                            }
                            if (errors.LogicError != null) {
                                dialogService.error(errors.LogicError);
                            }
                        });
                    }
                }
            });
        };
        $scope.cancel = function () {
            $state.go('^');
            $modalInstance.dismiss('cancel');
        };

        $modalInstance.result.then(
            function () { },
            function () { $state.go('^'); });

        var roles = { results: [] };
        $scope.roleOptions = {
            placeholder: "Select Role",
            initSelection: function (element, callback) {
                callback($(element).data('$ngModelController').$modelValue);
            },
            query: function (query) {
                dataService.get('list/roles', query.term).then(function (res) {
                    roles.results = [];
                    angular.forEach(res.data, function (r) {
                        roles.results.push(roleFormat(r));
                    });
                    query.callback(roles);
                });
            }
        };


        $scope.$watch('user.Role', function (newVal, oldVal) {
            if (newVal != undefined){
                if (newVal.id > 1) {
                    $scope.user.isSalesPerson = false;
                }
            }
        }, true);


        var approvers = { results: [] };
        $scope.approverOptions = {
            placeholder: "Select Approver",
            initSelection: function (elem, callback) {
                callback();
            },
            query: function (query) {
                var params = {
                    term: query.term,
                    userId: $scope.user.UserId,
                    ApproversIds: []
                };
                angular.forEach($scope.user.approvers, function (approver) {
                    if (approver.model != undefined) {
                        params.ApproversIds.push(approver.model.id);
                    }
                });
                dataService.post('list/approvers', params).then(function (res) {
                    approvers.results = [];
                    angular.forEach(res.data, function (user) {
                        approvers.results.push(userFormat(user));
                    });
                    query.callback(approvers);
                });
            }
        }
    }]);

