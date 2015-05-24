'use strict';

CCPApp.controller('salesPersonCtrl', ['$scope', '$modalInstance', '$state', '$stateParams', 'dataService',
    function ($scope, $modalInstance, $state, $stateParams, dataService) {
        $scope.salesPerson = {};
        $scope.loaded = false;
        $scope.loaderstyle = 'loader';
        var id = $stateParams.SalesPersonId;
        if (id > 0) {
            dataService.get('salesPersons', id).success(function (data) {
                $scope.salesPerson = data;
                $scope.salesPerson.selectedUser = {
                    text: data.User.FirstName + ' ' + data.User.LastName + ', ' + data.User.UserId,
                    id: data.User.UserId
                }
                $scope.loaderstyle = '';
                $scope.loaded = true;
                
            })
             .error(function () {
                 $scope.loaderstyle = '';
                 $scope.loaded = true;
                 
             });;
        } else {
            $scope.loaderstyle = '';
            $scope.loaded = true;
            $scope.salesPerson = {};
        }

        $scope.save = function () {
            var item = $scope.salesPerson;
            item.UserId = item.selectedUser.id;
            item.User = null;
            dataService.post('SalesPerson', item).success(function () {
                $state.go('^');
                $modalInstance.close();
            });
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
            $state.go('^');
        };

        var users = { results: [] };
        $scope.options = {
            placeholder: "Select User",
            query: function (query) {
                dataService.get('list/users', query.term).then(function (res) {
                    users.results = [];
                    angular.forEach(res.data, function (user) {
                        users.results.push({
                            text: user.FirstName + ' ' + user.LastName + ', ' + user.UserId,
                            id: user.UserId
                        });
                    });
                    query.callback(users);
                });
            }
        }
    }
]);
