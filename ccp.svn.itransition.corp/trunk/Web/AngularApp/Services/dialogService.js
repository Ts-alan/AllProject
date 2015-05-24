
CCPApp.factory('dialogService', [
    '$modal', function ($modal) {
        var fac = {};
        fac.error = function (msg) {
            return $modal.open({
                templateUrl: '/dialogs/error',
                controller: 'errorCtrl',
                resolve: {
                    error: function () {
                        return msg;
                    }
                }
            });
        }

        fac.confirm = function (msg) {
            return $modal.open({
                templateUrl: '/dialogs/confirm',
                controller: 'confirmCtrl',
                resolve: {
                    msg: function () {
                        return msg;
                    }
                }
            });
        }


        return fac;
    }
]);