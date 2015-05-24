(function () {
    var app = angular.module("app", ["ngGrid", "ui.bootstrap"]);
    app.controller("CtrlController", function ($scope, $http, $log, $modal) {
        $scope.myData = [];
       //получить таблицу
        function indexRow(parameters) {

            $scope.index = parameters;
        }
        var getData = function () {
            $scope.myData = $http.get("/api/Values/get").
                success(function (response) {

                    $scope.myData = response;

               });
            $scope.gridOptions = {
                data: 'myData',
                multiSelect: false,
                ////footerRowHeight: 42,
                //enableColumnResize: true,
                //enableRowReordering:true,
                //enableHighlighting:true,
                columnDefs: [
                    { field: "Id", width: 200 },
                    { field: "Name", width: 200 },
                    { field: "DateGreate", width: 260 },
                    { field: "DateChange", width: 260 },
                    { field: "Description", width: 200 }
                ],

                rowTemplate: '<div  ng-Dblclick="edit(row.rowIndex)"  ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell {{col.cellClass}}"><div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }">&nbsp;</div><div ng-cell></div></div>',
                afterSelectionChange:
                    function(rowItem, event) {
                        indexRow(rowItem.entity.Id);
                        $log.log(rowItem.entity.Id);
                    }
            };
        }
        getData();
        //добавить
        $scope.add = function () {
            var modalInstance = $modal.open(
                {
                    templateUrl: '/Home/TestView',
                    controller: "ModelWindowsController",
                    resolve: {
                        lol: function () {
                            return null;
                        }
                    }
                }
            );
            modalInstance.result.then(function (selectedCustomer) {

                $scope.myData2 = $http.post("/api/Values/Post", selectedCustomer).
                    success(function (response) {
                        getData();
                    });
            });
        }
        //удалить
        $scope.delete = function () {
            var string = "?key1=" + $scope.index;
            $http.delete("/api/Values/delete" + string).success(function () {
                getData();
            });
        }
        //редактировать
        $scope.edit = function (row) {
            var string = "?id=" + row;
            $http.get("/api/Values/Get" + string).
                success(function (response) {
                    $modal.open(
                    {
                        templateUrl: '/Home/TestView',
                        controller: "ModelWindowsController",
                        resolve: {
                            lol: function () {
                                return response;
                            }
                        },

                    }
                    ).result.then(function (result) {
                       
                        var id = "?id=" + $scope.index;
                      
                        $http.put("/api/Values/Put/" + id, result).success(function () {
                            getData();
                        });
                    });
                });
        }
    });
    app.controller("ModelWindowsController", function ($scope, $modalInstance, $log, lol) {
        $scope.modalnull = lol;
        $scope.ok = function () {
            $modalInstance.close($scope.modalnull);
            return lol;

        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        }
    });

    //app.service('sharedProperties', function () {
    //    var property = { "Id": null, "Name": null, "DateGreate": null, "DateChange": null, "Description": null };

    //    return {
    //        getProperty: function () {
    //            return property;
    //        },
    //        setProperty: function (value) {
    //            property = value;
    //        }
    //    };
    //});



})()

