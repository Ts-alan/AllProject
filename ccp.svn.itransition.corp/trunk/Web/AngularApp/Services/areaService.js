'use strict';

CCPApp.factory('areaService', ['dataService','authService','$q', function (dataService,authService,$q) {

    var areaServiceFactory = {};

    var roles = {};

    //var getAreas = function () {
    //    var deferred = $q.defer();
    //    dataService.get('area').success(function(data) {
    //        roles = data;
    //        deferred.resolve();
    //        }
    //        );
    //    return deferred.promise;
    //}


    var isAllowedArea = function (state) {
        roles.forEach(function(item) {
            if (state.indexOf(item.AreaName) > -1) {
                item.Roles.forEach(function(i) {
                    if (authService.authentication.role === i) {
                        return true;
                    }
                });
            }
        } );
        return false;
    }

    areaServiceFactory.isAllowedArea = isAllowedArea;
   // areaServiceFactory.getAreas = getAreas;
    areaServiceFactory.roles = roles;

    return areaServiceFactory;
}]);